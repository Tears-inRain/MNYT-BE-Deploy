using Application.Utils.Implementation;
using Microsoft.AspNetCore.Mvc;
using Application.IServices;
using Application.ViewModels.Blog;
using Application.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogService;

        public BlogPostsController(IBlogPostService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var result = await _blogService.GetBlogPostByIdAsync(postId);
            if (result == null)
            {
                return NotFound(ApiResponse<ReadBlogPostDTO>.FailureResponse("Post not found."));
            }

            return Ok(ApiResponse<ReadBlogPostDTO>.SuccessResponse(result, "Post retrieved successfully."));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _blogService.GetAllPostsAsync();
            return Ok(ApiResponse<List<ReadBlogPostDTO>>.SuccessResponse(
                posts,
                "All blog posts retrieved successfully."
            ));
        }

        [HttpGet("all-paginated")]
        public async Task<IActionResult> GetAllPostsPaginated([FromQuery] QueryParameters query)
        {
            var result = await _blogService.GetAllPostsPaginatedAsync(query);
            return Ok(ApiResponse<PaginatedList<ReadBlogPostDTO>>.SuccessResponse(
                result, "Posts retrieved successfully."
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromQuery] int authorId, [FromBody] CreateBlogPostDTO dto)
        {
            var created = await _blogService.CreateBlogPostAsync(authorId, dto);
            return Ok(ApiResponse<ReadBlogPostDTO>.SuccessResponse(created, "Post created successfully."));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromQuery] int accountId, [FromBody] UpdateBlogPostDTO dto)
        {
            var updated = await _blogService.UpdateBlogPostAsync(postId, dto, accountId);
            if (updated == null)
            {
                return NotFound(ApiResponse<ReadBlogPostDTO>.FailureResponse("Could not update post, post not found or no permission."));
            }

            return Ok(ApiResponse<ReadBlogPostDTO>.SuccessResponse(updated, "Post updated successfully."));
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId, [FromQuery] int accountId)
        {
            var success = await _blogService.DeleteBlogPostAsync(postId, accountId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not delete post, not found or no permission."));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post deleted successfully."));
        }

        [HttpPatch("{postId}/publish")]
        public async Task<IActionResult> PublishPost(int postId, [FromQuery] int accountId)
        {
            var success = await _blogService.PublishBlogPostAsync(postId, accountId);
            if (!success)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Could not publish post, not found or no permission."));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Post published successfully."));
        }
    }
}
