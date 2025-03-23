using Application.Services.IServices;
using Application.ViewModels;
using Application.ViewModels.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionsController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpPost("like/{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> LikePost(int postId, int accountId)
        {
            var success = await _interactionService.LikePostAsync(accountId, postId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Post not found. Cannot like."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Post liked successfully."));
        }

        [HttpDelete("like/{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UnlikePost(int postId, int accountId)
        {
            var success = await _interactionService.UnlikePostAsync(accountId, postId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Either post not found or it wasn't liked."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Post unliked successfully."));
        }

        [HttpGet("likes")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ReadPostDTO>>>> GetLikesByAccountId(int accountId)
        {
            var likedPosts = await _interactionService.GetAllLikesByAccountIdAsync(accountId);
            return Ok(ApiResponse<List<ReadPostDTO>>.SuccessResponse(
                likedPosts,
                $"Retrieved {likedPosts.Count} liked posts for accountId = {accountId}."
            ));
        }

        [HttpPost("bookmark/{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> BookmarkPost(int postId, int accountId)
        {
            var success = await _interactionService.BookmarkPostAsync(accountId, postId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Post not found. Cannot bookmark."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Post bookmarked successfully."));
        }

        [HttpDelete("bookmark/{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveBookmark(int postId, int accountId)
        {
            var success = await _interactionService.RemoveBookmarkAsync(accountId, postId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Either post not found or it wasn't bookmarked."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Bookmark removed successfully."));
        }

        [HttpGet("bookmarks")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ReadPostDTO>>>> GetBookmarksByAccountId(int accountId)
        {
            var bookmarkedPosts = await _interactionService.GetAllBookmarksByAccountIdAsync(accountId);
            return Ok(ApiResponse<List<ReadPostDTO>>.SuccessResponse(
                bookmarkedPosts,
                $"Retrieved {bookmarkedPosts.Count} bookmarked posts for accountId = {accountId}."
            ));
        }
    }
}