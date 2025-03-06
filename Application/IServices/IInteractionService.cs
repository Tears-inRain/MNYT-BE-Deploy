using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IInteractionService
    {
        Task<bool> LikePostAsync(int accountId, int postId);
        Task<bool> UnlikePostAsync(int accountId, int postId);
        Task<bool> BookmarkPostAsync(int accountId, int postId);
        Task<bool> RemoveBookmarkAsync(int accountId, int postId);
    }
}
