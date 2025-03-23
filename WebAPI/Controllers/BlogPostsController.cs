using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.Blog;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.Services.IServices;
using Application.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlogPostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public BlogPostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var result = await _postService.GetPostByIdAsync(postId);
            if (result == null)
            {
                return NotFound(ApiResponse<ReadPostDTO>.FailureResponse("Post not found."));
            }

            return Ok(ApiResponse<ReadPostDTO>.SuccessResponse(result, "Post retrieved successfully."));
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllForumPosts()
        {
            var posts = await _postService.GetAllForumPostsAsync();
            return Ok(ApiResponse<List<ReadPostDTO>>.SuccessResponse(
                posts,
                "All forum posts retrieved successfully."
            ));
        }

        [HttpGet("all-paginated")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllForumPostsPaginated([FromQuery] QueryParameters query)
        {
            var result = await _postService.GetAllForumPostsPaginatedAsync(query);
            return Ok(ApiResponse<PaginatedList<ReadPostDTO>>.SuccessResponse(
                result, "Posts retrieved successfully."
            ));
        }

        [HttpGet("by-category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllForumByCategory([FromQuery] string category)
        {
            var posts = await _postService.GetAllForumByCategoryAsync(category);

            return Ok(ApiResponse<List<ReadPostDTO>>.SuccessResponse(
                posts,
                string.IsNullOrEmpty(category)
                    ? "All forum posts retrieved without category filter."
                    : $"All forum posts for category '{category}' retrieved successfully."
            ));
        }

        [HttpGet("admin-posts")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ReadPostDTO>>>> GetAllBlogPosts()
        {
            var posts = await _postService.GetAllBlogPostsAsync();

            return Ok(ApiResponse<List<ReadPostDTO>>.SuccessResponse(
                posts,
                "Successfully retrieved all blog posts."
            ));
        }

        [HttpPost("blog")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBlogPost([FromQuery] int authorId, [FromBody] CreatePostDTO dto)
        {
            var created = await _postService.CreateBlogPostAsync(authorId, dto);
            return Ok(ApiResponse<ReadPostDTO>.SuccessResponse(created, "Blog post created successfully."));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateForumPost([FromQuery] int authorId, [FromBody] CreatePostDTO dto)
        {
            var created = await _postService.CreateForumPostAsync(authorId, dto);
            return Ok(ApiResponse<ReadPostDTO>.SuccessResponse(created, "Forum post created successfully."));
        }

        [HttpPut("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePost(int postId, [FromQuery] int accountId, [FromBody] UpdatePostDTO dto)
        {
            var updated = await _postService.UpdatePostAsync(postId, dto, accountId);
            if (updated == null)
            {
                return NotFound(ApiResponse<ReadPostDTO>.FailureResponse("Could not update post, post not found or no permission."));
            }

            return Ok(ApiResponse<ReadPostDTO>.SuccessResponse(updated, "Post updated successfully."));
        }

        [HttpDelete("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePost(int postId, [FromQuery] int accountId)
        {
            var success = await _postService.DeletePostAsync(postId, accountId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not delete post, not found or no permission."));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post deleted successfully."));
        }

        [HttpPatch("{postId}/change-status")]
        [AllowAnonymous]
        public async Task<IActionResult> PublishPost(int postId, [FromQuery] int accountId, string status)
        {
            var success = await _postService.ChangePostStatusAsync(postId, accountId, status);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not change post status, not found or no permission."));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post status successfully."));
        }

        [HttpPatch("{postId}/publish")]
        [AllowAnonymous]
        public async Task<IActionResult> PublishPost(int postId, [FromQuery] int accountId)
        {
            var success = await _postService.PublishPostAsync(postId, accountId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not publish post, not found or no permission."));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post published successfully."));
        }

        [HttpGet("top-authors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTop3Authors()
        {
            var result = await _postService.GetTopAuthorsAsync();
            return Ok(result);
        }
    }
}
