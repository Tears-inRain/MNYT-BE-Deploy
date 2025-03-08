using Application.Utils.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.IServices;
using Application.ViewModels.Blog;
using Application.ViewModels;


namespace Application.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BlogPostService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadBlogPostDTO> CreateBlogPostAsync(int authorId, CreateBlogPostDTO dto)
        {
            _logger.LogInformation("Creating a blog post for AuthorId: {AuthorId}", authorId);

            var post = _mapper.Map<BlogPost>(dto);
            post.AuthorId = authorId;
            post.Status = "Draft";
            post.CreateDate = DateTime.UtcNow;
            post.UpdateDate = DateTime.UtcNow;

            await _unitOfWork.PostRepo.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully created BlogPost Id: {PostId}", post.Id);

            return _mapper.Map<ReadBlogPostDTO>(post);
        }

        public async Task<ReadBlogPostDTO?> UpdateBlogPostAsync(int postId, UpdateBlogPostDTO dto, int requestAccountId)
        {
            _logger.LogInformation("Updating postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return null;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} is not the author of postId: {PostId}", requestAccountId, postId);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) post.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Description)) post.Description = dto.Description;
            if (dto.Period.HasValue) post.Period = dto.Period.Value;

            post.UpdateDate = DateTime.UtcNow;

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated BlogPost Id: {PostId}", post.Id);

            return _mapper.Map<ReadBlogPostDTO>(post);
        }

        public async Task<bool> DeleteBlogPostAsync(int postId, int requestAccountId)
        {
            _logger.LogInformation("Deleting postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return false;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} not allowed to delete postId: {PostId}", requestAccountId, postId);
                return false;
            }

            post.IsDeleted = true;
            post.UpdateDate = DateTime.UtcNow;

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully soft-deleted BlogPost Id: {PostId}", post.Id);
            return true;
        }

        public async Task<bool> PublishBlogPostAsync(int postId, int requestAccountId)
        {
            _logger.LogInformation("Publishing postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found", postId);
                return false;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} not allowed to publish postId: {PostId}", requestAccountId, postId);
                return false;
            }

            post.Status = "Published";
            post.PublishedDay = DateOnly.FromDateTime(DateTime.UtcNow);
            post.UpdateDate = DateTime.UtcNow;

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully published BlogPost Id: {PostId}", post.Id);
            return true;
        }

        public async Task<ReadBlogPostDTO?> GetBlogPostByIdAsync(int postId)
        {
            var post = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);

            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return null;
            }

            return _mapper.Map<ReadBlogPostDTO>(post);
        }

        public async Task<List<ReadBlogPostDTO>> GetAllPostsAsync()
        {
            var posts = await _unitOfWork.PostRepo.GetAllAsync("Author,BlogLikes,BlogBookmarks,Comments");
            return _mapper.Map<List<ReadBlogPostDTO>>(posts);
        }

        public async Task<PaginatedList<ReadBlogPostDTO>> GetAllPostsPaginatedAsync(QueryParameters queryParameters)
        {
            var query = _unitOfWork.PostRepo.GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(p => !p.IsDeleted);

            var pagedEntities = await PaginatedList<BlogPost>.CreateAsync(
                query.OrderByDescending(p => p.CreateDate),
                queryParameters.PageNumber,
                queryParameters.PageSize
            );

            var mappedList = _mapper.Map<List<ReadBlogPostDTO>>(pagedEntities.Items);

            return new PaginatedList<ReadBlogPostDTO>(
                mappedList,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.Items.Count
            );
        }
    }
}