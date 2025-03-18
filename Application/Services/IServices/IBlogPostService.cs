using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.Blog;
using Application.ViewModels;
using Application.Utils;

namespace Application.Services.IServices
{
    public interface IBlogPostService
    {
        Task<ReadBlogPostDTO> CreateBlogPostAsync(int authorId, CreateBlogPostDTO dto);
        Task<ReadBlogPostDTO?> UpdateBlogPostAsync(int postId, UpdateBlogPostDTO dto, int requestUserId);
        Task<bool> DeleteBlogPostAsync(int postId, int requestUserId);
        Task<bool> ChangeBlogPostStatusAsync(int postId, int requestAccountId, string status);
        Task<bool> PublishBlogPostAsync(int postId, int requestUserId);

        Task<ReadBlogPostDTO?> GetBlogPostByIdAsync(int postId);
        Task<List<ReadBlogPostDTO>> GetAllPostsAsync();
        Task<PaginatedList<ReadBlogPostDTO>> GetAllPostsPaginatedAsync(QueryParameters queryParameters);
        Task<List<ReadBlogPostDTO>> GetAllByCategoryAsync(string category);
    }
}
