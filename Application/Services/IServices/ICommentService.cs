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
    public interface ICommentService
    {
        Task<ReadCommentDTO?> AddCommentAsync(int accountId, CreateCommentDTO dto);
        Task<PaginatedList<ReadCommentDTO>> GetCommentsByPostAsync(int postId, QueryParameters parameters);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}
