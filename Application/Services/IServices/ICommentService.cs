using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels;
using Application.Utils;
using Application.ViewModels.Post;

namespace Application.Services.IServices
{
    public interface ICommentService
    {
        Task<ReadCommentDTO?> AddCommentAsync(int accountId, CreateCommentDTO dto);
        Task<List<ReadCommentDTO>> GetCommentsByPostAsync(int postId);
        Task<PaginatedList<ReadCommentDTO>> GetCommentsByPostPaginatedAsync(int postId, QueryParameters parameters);
        Task<ReadCommentDTO?> UpdateCommentAsync(int commentId, int accountId, UpdateCommentDTO dto);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}
