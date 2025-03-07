using Application.IRepos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repos
{
    public class AccountRepo : GenericRepo<Account>, IAccountRepo
    {
        private readonly AppDbContext _appDbContext;
        public AccountRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }


        public async Task<Account> GetByUsernameOrEmail(string email, string username)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email || x.UserName == username);

            return account;
        }

        public async Task<bool> AnyAsync(Expression<Func<Account, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
