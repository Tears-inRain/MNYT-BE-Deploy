using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class AccountRepo : GenericRepo<Account>, IAccountRepo
    {
        private readonly AppDbContext _appDbContext;
        public AccountRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
