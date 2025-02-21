using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class ScheduleUserRepo : GenericRepo<ScheduleUser>, IScheduleUserRepo
    {
        private readonly AppDbContext _appDbContext;

        public ScheduleUserRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
