using Application.Utils;
using Application.ViewModels;
using Application.ViewModels.Accounts;

namespace Application.Services.IServices
{
    public interface IAccountService
    {
        Task<AccountDTO?> GetAccountByIdAsync(int id);
        Task<IEnumerable<AccountDTO>> GetAllAccountsAsync();
        Task<PaginatedList<AccountDTO>> GetAllAccountsPaginatedAsync(QueryParameters queryParameters);
        Task<bool> CheckUsernameOrEmailExistsAsync(string email, string username);
        Task<AccountDTO?> UpdateAccountAsync(int accountId, UpdateAccountDTO updateDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<bool> BanAccountAsync(int accountId);
        Task<bool> UnbanAccountAsync(int accountId);
    }
}
