
using Application.Utils.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.Blog;
using Application.IServices;
using Application.ViewModels;

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

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == dto.BlogPostId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Blog post {PostId} not found, cannot add comment", dto.BlogPostId);
                return null;
            }

            var comment = _mapper.Map<Comment>(dto);
            comment.AccountId = accountId;
            comment.CreateDate = DateTime.UtcNow;
            comment.UpdateDate = DateTime.UtcNow;
            comment.Status = "Active";

            await _unitOfWork.CommentRepo.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Comment Id: {CommentId} added successfully.", comment.Id);

            return _mapper.Map<ReadCommentDTO>(comment);
        }

        public async Task<PaginatedList<ReadCommentDTO>> GetCommentsByPostAsync(int postId, QueryParameters parameters)
        {
            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} does not exist or is deleted. Returning empty comments list.", postId);
                return new PaginatedList<ReadCommentDTO>(new List<ReadCommentDTO>(), 0, parameters.PageNumber, parameters.PageSize);
            }

            var commentsQuery = _unitOfWork.CommentRepo.GetAllQueryable("Account")
                .Where(c => c.BlogPostId == postId && !c.IsDeleted);

            var paginatedComments = await PaginatedList<Comment>.CreateAsync(
                commentsQuery.OrderByDescending(c => c.CreateDate),
                parameters.PageNumber,
                parameters.PageSize
            );

            var mappedList = _mapper.Map<List<ReadCommentDTO>>(paginatedComments.Items);

            return new PaginatedList<ReadCommentDTO>(
                mappedList,
                paginatedComments.TotalCount,
                paginatedComments.PageIndex,
                paginatedComments.Items.Count
            );
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int accountId)
        {
            _logger.LogInformation("Account {AccountId} is deleting commentId: {CommentId}", accountId, commentId);

            var comment = await _unitOfWork.CommentRepo.FindOneAsync(c => c.Id == commentId && !c.IsDeleted);
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

            comment.IsDeleted = true;
            comment.UpdateDate = DateTime.UtcNow;

            _unitOfWork.CommentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Comment Id: {CommentId} soft-deleted by account {AccountId}", commentId, accountId);
            return true;
        }
    }
}