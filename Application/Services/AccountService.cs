using AutoMapper;
using Domain.Entities;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels;
using Application.ViewModels.Accounts;
using Application.ViewModels.FetusRecord;
using Application.ViewModels.Blog;
using Application.Services.IServices;
using Application.Utils;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(id);
            if (account == null) return null;

            return _mapper.Map<AccountDTO>(account);
        }

        //public async Task<CreateAccountDTO> AddAsync(CreateAccountDTO createAccountDto)
        //{
        //    Account record = _mapper.Map<Account>(createAccountDto);
        //    record.Password = BCrypt.Net.BCrypt.HashPassword(record.Password, workFactor: 12);
        //    await _unitOfWork.AccountRepo.AddAsync(record);
        //    await _unitOfWork.SaveChangesAsync();

        //    return _mapper.Map<CreateAccountDTO>(record);
        //}

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

        public async Task<AccountDTO?> UpdateAccountAsync(int accountId, UpdateAccountDTO updateDto)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return null;

            account.FullName = updateDto.FullName ?? account.FullName;
            account.PhoneNumber = updateDto.PhoneNumber ?? account.PhoneNumber;
            account.UpdateDate = DateTime.UtcNow;

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<bool> BanAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return false;

            account.Status = "Banned";
            account.UpdateDate = DateTime.UtcNow;

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbanAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepo.GetAsync(accountId);
            if (account == null) return false;

            account.Status = "Active";
            account.UpdateDate = DateTime.UtcNow;

            _unitOfWork.AccountRepo.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
