using Application.Utils;
using Application.ViewModels;
using Application.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IAccountService
    {
        Task<AccountDTO?> GetAccountByIdAsync(int id);
        //Task<CreateAccountDTO> AddAsync(CreateAccountDTO createAccountDto);
        Task<IEnumerable<AccountDTO>> GetAllAccountsAsync();
        Task<PaginatedList<AccountDTO>> GetAllAccountsPaginatedAsync(QueryParameters queryParameters);
        Task<AccountDTO?> UpdateAccountAsync(int accountId, UpdateAccountDTO updateDto);
        Task<bool> BanAccountAsync(int accountId);
        Task<bool> UnbanAccountAsync(int accountId);
    }
}
