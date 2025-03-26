using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.Services.IServices;
using Application.Utils;
using Application.ViewModels.Post;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddComment(int accountId, [FromBody] CreateCommentDTO dto)
        {
            var comment = await _commentService.AddCommentAsync(accountId, dto);
            if (comment == null)
            {
                return NotFound(ApiResponse<ReadCommentDTO>.FailureResponse("Cannot add comment; post not found."));
            }
            return Ok(ApiResponse<ReadCommentDTO>.SuccessResponse(comment, "Comment added."));
        }

        [HttpGet("post/{postId}/all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByPostNoPaging(int postId)
        {
            var comments = await _commentService.GetCommentsByPostAsync(postId);
            return Ok(ApiResponse<List<ReadCommentDTO>>.SuccessResponse(comments, "All comments retrieved."));
        }

        [HttpGet("post/{postId}/paginated")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByPostPaginated(int postId, [FromQuery] QueryParameters query)
        {
            var comments = await _commentService.GetCommentsByPostPaginatedAsync(postId, query);
            return Ok(ApiResponse<PaginatedList<ReadCommentDTO>>.SuccessResponse(comments, "Comments retrieved."));
        }

        [HttpPut("{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateComment(int commentId, int accountId, [FromBody] UpdateCommentDTO dto)
        {
            var updated = await _commentService.UpdateCommentAsync(commentId, accountId, dto);
            if (updated == null)
            {
                return NotFound(ApiResponse<ReadCommentDTO>.FailureResponse("Could not update comment. Possibly not found or no permission."));
            }
            return Ok(ApiResponse<ReadCommentDTO>.SuccessResponse(updated, "Comment updated successfully."));
        }

        [HttpDelete("{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteComment(int commentId, int accountId)
        {
            var success = await _commentService.DeleteCommentAsync(commentId, accountId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not delete comment. Possibly not found or no permission."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Comment deleted."));
        }
    }
}
