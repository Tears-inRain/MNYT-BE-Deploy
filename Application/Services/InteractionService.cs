using Domain.Entities;
using Microsoft.Extensions.Logging;
using Application.Services.IServices;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.ViewModels.Media;
using Application.ViewModels.Post;

namespace Application.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InteractionService> _logger;
        private readonly IMapper _mapper;

        public InteractionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InteractionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> LikePostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} liking post {PostId}", accountId, postId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} not found, cannot like", postId);
                return false;
            }

            var existingLike = await _unitOfWork.BlogLikeRepo.FindOneAsync(l => l.AccountId == accountId && l.PostId == postId);
            if (existingLike != null)
            {
                return true;
            }

            var like = new BlogLike
            {
                AccountId = accountId,
                PostId = postId,
            };

            await _unitOfWork.BlogLikeRepo.AddAsync(like);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Like created for postId: {PostId}, user: {AccountId}", postId, accountId);
            return true;
        }

        public async Task<bool> UnlikePostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} unliking post {PostId}", accountId, postId);

            var like = await _unitOfWork.BlogLikeRepo.FindOneAsync(l => l.AccountId == accountId && l.PostId == postId);
            if (like == null)
            {
                _logger.LogWarning("No existing like found for postId: {PostId}, account: {AccountId}", postId, accountId);
                return false;
            }

            _unitOfWork.BlogLikeRepo.SoftDelete(like);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReadPostDTO>> GetAllLikesByAccountIdAsync(int accountId)
        {
            _logger.LogInformation("Retrieving all liked posts for accountId: {AccountId}", accountId);

            var query = _unitOfWork.BlogLikeRepo
                .GetAllQueryable("Post.Author, Post.BlogBookmarks, Post.BlogLikes, Post.Comments")
                .Where(l => l.AccountId == accountId
                            && l.Post != null && !l.Post.IsDeleted);

            var likedPosts = await query
                .Select(l => l.Post!)
                .ToListAsync();

            var likedDtos = await AttachMediaAndMapAsync(likedPosts);

            return likedDtos;
        }

        public async Task<bool> BookmarkPostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} bookmarking post {PostId}", accountId, postId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} not found, cannot bookmark", postId);
                return false;
            }

            var existingBookmark = await _unitOfWork.BlogBookmarkRepo.FindOneAsync(b => b.AccountId == accountId && b.PostId == postId);
            if (existingBookmark != null)
            {
                return true;
            }

            var bookmark = new BlogBookmark
            {
                AccountId = accountId,
                PostId = postId,
            };

            await _unitOfWork.BlogBookmarkRepo.AddAsync(bookmark);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Bookmark created for postId: {PostId}, account: {AccountId}", postId, accountId);
            return true;
        }

        public async Task<bool> RemoveBookmarkAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} removing bookmark from post {PostId}", accountId, postId);

            var bookmark = await _unitOfWork.BlogBookmarkRepo.FindOneAsync(b => b.AccountId == accountId && b.PostId == postId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark does not exist for postId: {PostId}, account: {AccountId}", postId, accountId);
                return false;
            }

            _unitOfWork.BlogBookmarkRepo.SoftDelete(bookmark);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReadPostDTO>> GetAllBookmarksByAccountIdAsync(int accountId)
        {
            _logger.LogInformation("Retrieving all bookmarked posts for accountId: {AccountId}", accountId);

            var query = _unitOfWork.BlogBookmarkRepo
                .GetAllQueryable("Post.Author, Post.BlogBookmarks, Post.BlogLikes, Post.Comments")
                .Where(b => b.AccountId == accountId
                            && b.Post != null && !b.Post.IsDeleted);

            var bookmarkedPosts = await query
                .Select(b => b.Post!)
                .ToListAsync();

            var bookmarkedDtos = await AttachMediaAndMapAsync(bookmarkedPosts);

            return bookmarkedDtos;
        }

        private async Task<List<ReadPostDTO>> AttachMediaAndMapAsync(List<BlogPost> posts)
        {
            if (!posts.Any())
                return new List<ReadPostDTO>();

            var postIds = posts.Select(p => p.Id).ToList();

            var mediaList = await _unitOfWork.MediaRepo
                .GetAllQueryable()
                .Where(m => m.EntityType == "Post" && m.EntityId.HasValue && postIds.Contains(m.EntityId.Value))
                .ToListAsync();

            var mediaGrouped = mediaList
                .GroupBy(m => m.EntityId)
                .ToDictionary(g => g.Key!.Value, g => g.ToList());

            var mappedDtos = _mapper.Map<List<ReadPostDTO>>(posts);

            foreach (var postDto in mappedDtos)
            {
                if (mediaGrouped.ContainsKey(postDto.Id))
                {
                    var relatedMedia = mediaGrouped[postDto.Id];
                    postDto.Images = _mapper.Map<List<ReadMediaDTO>>(relatedMedia);
                }
                else
                {
                    postDto.Images = new List<ReadMediaDTO>();
                }
            }

            return mappedDtos;
        }
    }
}
