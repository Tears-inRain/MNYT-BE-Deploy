using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class PregnancyRepo : GenericRepo<Pregnancy>, IPregnancyRepo
    {
        private readonly AppDbContext _appDbContext;

        public PregnancyRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
