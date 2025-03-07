using Application.Utils.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.Blog;
using Application.ViewModels;

namespace Application.IServices
{
    public interface IBlogPostService
    {
        Task<ReadBlogPostDTO> CreateBlogPostAsync(int authorId, CreateBlogPostDTO dto);
        Task<ReadBlogPostDTO?> UpdateBlogPostAsync(int postId, UpdateBlogPostDTO dto, int requestUserId);
        Task<bool> DeleteBlogPostAsync(int postId, int requestUserId);
        Task<bool> PublishBlogPostAsync(int postId, int requestUserId);

        Task<ReadBlogPostDTO?> GetBlogPostByIdAsync(int postId);
        Task<List<ReadBlogPostDTO>> GetAllPostsAsync();
        Task<PaginatedList<ReadBlogPostDTO>> GetAllPostsPaginatedAsync(QueryParameters queryParameters);
    }
}
