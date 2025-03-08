using Domain.Entities;
using Domain;
using Microsoft.Extensions.Logging;
using Application.IServices;

namespace Application.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InteractionService> _logger;

        public InteractionService(IUnitOfWork unitOfWork, ILogger<InteractionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> LikePostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} liking post {PostId}", accountId, postId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} not found, cannot like", postId);
                return false;
            }

            var existingLike = await _unitOfWork.BlogLikeRepo.FindOneAsync(l => l.AccountId == accountId && l.PostId == postId && !l.IsDeleted);
            if (existingLike != null)
            {
                return true;
            }

            var like = new BlogLike
            {
                AccountId = accountId,
                PostId = postId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _unitOfWork.BlogLikeRepo.AddAsync(like);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Like created for postId: {PostId}, user: {AccountId}", postId, accountId);
            return true;
        }

        public async Task<bool> UnlikePostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} unliking post {PostId}", accountId, postId);

            var like = await _unitOfWork.BlogLikeRepo.FindOneAsync(l => l.AccountId == accountId && l.PostId == postId && !l.IsDeleted);
            if (like == null)
            {
                _logger.LogWarning("No existing like found for postId: {PostId}, account: {AccountId}", postId, accountId);
                return false;
            }

            like.IsDeleted = true;
            like.UpdateDate = DateTime.UtcNow;

            _unitOfWork.BlogLikeRepo.Update(like);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BookmarkPostAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} bookmarking post {PostId}", accountId, postId);

            var post = await _unitOfWork.PostRepo.FindOneAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} not found, cannot bookmark", postId);
                return false;
            }

            var existingBookmark = await _unitOfWork.BlogBookmarkRepo.FindOneAsync(b => b.AccountId == accountId && b.PostId == postId && !b.IsDeleted);
            if (existingBookmark != null)
            {
                return true;
            }

            var bookmark = new BlogBookmark
            {
                AccountId = accountId,
                PostId = postId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _unitOfWork.BlogBookmarkRepo.AddAsync(bookmark);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Bookmark created for postId: {PostId}, account: {AccountId}", postId, accountId);
            return true;
        }

        public async Task<bool> RemoveBookmarkAsync(int accountId, int postId)
        {
            _logger.LogInformation("Account {AccountId} removing bookmark from post {PostId}", accountId, postId);

            var bookmark = await _unitOfWork.BlogBookmarkRepo.FindOneAsync(b => b.AccountId == accountId && b.PostId == postId && !b.IsDeleted);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark does not exist for postId: {PostId}, account: {AccountId}", postId, accountId);
                return false;
            }

            bookmark.IsDeleted = true;
            bookmark.UpdateDate = DateTime.UtcNow;

            _unitOfWork.BlogBookmarkRepo.Update(bookmark);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
