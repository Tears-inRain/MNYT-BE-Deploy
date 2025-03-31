using Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IInteractionService
    {
        Task<bool> LikePostAsync(int accountId, int postId);
        Task<bool> UnlikePostAsync(int accountId, int postId);
        Task<bool> BookmarkPostAsync(int accountId, int postId);
        Task<bool> RemoveBookmarkAsync(int accountId, int postId);
        Task<List<ReadPostDTO>> GetAllBookmarksByAccountIdAsync(int accountId);
        Task<List<ReadPostDTO>> GetAllLikesByAccountIdAsync(int accountId);
        Task<List<ReadPostLikeDTO>> GetAllLikesByPostIdAsync(int PostId);
    }
}
