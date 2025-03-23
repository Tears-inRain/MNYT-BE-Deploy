using AutoMapper;
using Domain.Entities;
using Domain;
using Application.ViewModels;
using Application.ViewModels.Accounts;
using Application.Services.IServices;
using Application.Utils;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;

        public AccountService(
            IUnitOfWork unitOfWork,
            ILogger<AccountService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(id);
            if (account == null) return null;

            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccountsAsync()
        {
            var allAccounts = await _unitOfWork.AccountRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountDTO>>(allAccounts);
        }

        public async Task<PaginatedList<AccountDTO>> GetAllAccountsPaginatedAsync(QueryParameters queryParameters)
        {
            var query = _unitOfWork.AccountRepo.GetAllQueryable();

            var pagedResult = await PaginatedList<Account>.CreateAsync(query, queryParameters.PageNumber, queryParameters.PageSize);

            var accountDtoList = _mapper.Map<List<AccountDTO>>(pagedResult.Items);

            return new PaginatedList<AccountDTO>(
                accountDtoList,
                pagedResult.TotalCount,
                pagedResult.PageIndex,
                queryParameters.PageSize
            );
        }

        public async Task<bool> CheckUsernameOrEmailExistsAsync(string email, string username)
        {
            var existedAccount = await _unitOfWork.AccountRepo.GetByUsernameOrEmail(email, username);
            if (existedAccount != null) return true;

            return false;
        }

        public async Task<AccountDTO?> UpdateAccountAsync(int accountId, UpdateAccountDTO updateDto)
        {
            var existedAccount = await _unitOfWork.AccountRepo.GetByUsernameOrEmail(updateDto.Email, updateDto.UserName);

            if (existedAccount != null)
            {
                _logger.LogWarning("User already exists with the provided email or username.");
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "User already exists with the provided email or username.");
            }

            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return null;

            account.FullName = updateDto.FullName;
            account.PhoneNumber = updateDto.PhoneNumber;
            account.UserName = updateDto.UserName;
            account.Email = updateDto.Email;

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var account = await _unitOfWork.AccountRepo.GetByUsernameOrEmail(dto.UserNameOrEmail, dto.UserNameOrEmail);
            if (account == null)
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "Account with the given email or username does not exist.");

            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "New password and confirmation do not match.");

            account.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword, workFactor: 12);

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BanAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return false;

            account.Status = "Banned";

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbanAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return false;

            account.Status = "Active";

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
