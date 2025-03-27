using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Application.ViewModels;
using Application.Services.IServices;
using Application.Utils;
using Application.ViewModels.Media;
using Microsoft.EntityFrameworkCore;
using Application.ViewModels.Post;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CommentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadCommentDTO?> AddCommentAsync(int accountId, CreateCommentDTO dto)
        {
            _logger.LogInformation("User {AccountId} is adding a comment to postId: {PostId}", accountId, dto.BlogPostId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == dto.BlogPostId);
            if (post == null)
            {
                _logger.LogWarning("Blog post {PostId} not found, cannot add comment", dto.BlogPostId);
                return null;
            }

            var comment = _mapper.Map<Comment>(dto);
            comment.AccountId = accountId;
            comment.Status = "Active";

            await _unitOfWork.CommentRepo.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var imageDto in dto.Images)
                {
                    var media = _mapper.Map<Media>(imageDto);
                    media.EntityType = "Comment";
                    media.EntityId = comment.Id;
                    media.Type = "image"; 

                    await _unitOfWork.MediaRepo.AddAsync(media);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Comment Id: {CommentId} added successfully.", comment.Id);

            var readDto = await AttachMediaAndMapSingleAsync(comment);
            return readDto;
        }

        public async Task<List<ReadCommentDTO>> GetCommentsByPostAsync(int postId)
        {
            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                _logger.LogWarning("Post {PostId} does not exist or is deleted. Returning empty comments list.", postId);
                return new List<ReadCommentDTO>();
            }

            var comments = await _unitOfWork.CommentRepo
                .GetAllQueryable("Account")
                .Where(c => c.BlogPostId == postId && !c.IsDeleted)
                .OrderByDescending(c => c.CreateDate)
                .ToListAsync();

            return await AttachMediaAndMapAsync(comments);
        }

        public async Task<PaginatedList<ReadCommentDTO>> GetCommentsByPostPaginatedAsync(int postId, QueryParameters parameters)
        {
            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                _logger.LogWarning("Post {PostId} does not exist or is deleted. Returning empty comments list.", postId);
                return new PaginatedList<ReadCommentDTO>(new List<ReadCommentDTO>(), 0, parameters.PageNumber, parameters.PageSize);
            }

            var commentsQuery = _unitOfWork.CommentRepo.GetAllQueryable("Account")
                .Where(c => c.BlogPostId == postId)
                .OrderByDescending(c => c.CreateDate);

            var paginatedComments = await PaginatedList<Comment>.CreateAsync(
                commentsQuery,
                parameters.PageNumber,
                parameters.PageSize
            );

            var dtosWithImages = await AttachMediaAndMapAsync(paginatedComments.Items);

            return new PaginatedList<ReadCommentDTO>(
                dtosWithImages,
                paginatedComments.TotalCount,
                paginatedComments.PageIndex,
                paginatedComments.Items.Count
            );
        }

        public async Task<ReadCommentDTO?> UpdateCommentAsync(int commentId, int accountId, UpdateCommentDTO dto)
        {
            _logger.LogInformation("Account {AccountId} is editing commentId: {CommentId}", accountId, commentId);

            var comment = await _unitOfWork.CommentRepo.GetByIdAsync(commentId);
            if (comment == null || comment.IsDeleted)
            {
                _logger.LogWarning("Comment Id: {CommentId} not found or is deleted", commentId);
                return null;
            }

            if (comment.AccountId != accountId)
            {
                _logger.LogWarning("Account {AccountId} does not own commentId: {CommentId}", accountId, commentId);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.Content))
                comment.Content = dto.Content;

            comment.UpdateDate = DateTime.UtcNow;

            _unitOfWork.CommentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Comment Id: {CommentId} updated successfully by Account {AccountId}", commentId, accountId);

            var updatedDto = await AttachMediaAndMapSingleAsync(comment);
            return updatedDto;
        }


        public async Task<bool> DeleteCommentAsync(int commentId, int accountId)
        {
            _logger.LogInformation("Account {AccountId} is deleting commentId: {CommentId}", accountId, commentId);

            var comment = await _unitOfWork.CommentRepo.GetByIdAsync(commentId);
            if (comment == null)
            {
                _logger.LogWarning("Comment Id: {CommentId} not found", commentId);
                return false;
            }

            if (comment.AccountId != accountId)
            {
                _logger.LogWarning("Account {AccountId} is not the owner of commentId: {CommentId}", accountId, commentId);
                return false;
            }

            _unitOfWork.CommentRepo.SoftDelete(comment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Comment Id: {CommentId} soft-deleted by account {AccountId}", commentId, accountId);
            return true;
        }

        private async Task<ReadCommentDTO> AttachMediaAndMapSingleAsync(Comment comment)
        {
            var mediaList = await _unitOfWork.MediaRepo.GetAllQueryable()
                .Where(m => m.EntityType == "Comment" && m.EntityId == comment.Id)
                .ToListAsync();

            var readDto = _mapper.Map<ReadCommentDTO>(comment);
            readDto.Images = _mapper.Map<List<ReadMediaDTO>>(mediaList);

            return readDto;
        }

        private async Task<List<ReadCommentDTO>> AttachMediaAndMapAsync(List<Comment> commentEntities)
        {
            if (!commentEntities.Any())
            {
                return new List<ReadCommentDTO>();
            }

            var commentIds = commentEntities.Select(c => c.Id).ToList();

            var mediaList = await _unitOfWork.MediaRepo.GetAllQueryable()
                .Where(m => m.EntityType == "Comment"
                            && m.EntityId.HasValue
                            && commentIds.Contains(m.EntityId.Value))
                .ToListAsync();

            var mediaGrouped = mediaList
                .GroupBy(m => m.EntityId)
                .ToDictionary(g => g.Key!.Value, g => g.ToList());

            var mappedDtos = _mapper.Map<List<ReadCommentDTO>>(commentEntities);

            foreach (var commentDto in mappedDtos)
            {
                if (mediaGrouped.ContainsKey(commentDto.Id))
                {
                    var relatedMedia = mediaGrouped[commentDto.Id];
                    commentDto.Images = _mapper.Map<List<ReadMediaDTO>>(relatedMedia);
                }
                else
                {
                    commentDto.Images = new List<ReadMediaDTO>();
                }
            }

            return mappedDtos;
        }
    }
}