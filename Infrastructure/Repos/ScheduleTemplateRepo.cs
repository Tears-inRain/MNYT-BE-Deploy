using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class ScheduleTemplateRepo : GenericRepo<ScheduleTemplate>, IScheduleTemplateRepo
    {
        private readonly AppDbContext _appDbContext;

        public ScheduleTemplateRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
