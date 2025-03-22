using Application.IRepos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class ScheduleUserRepo : GenericRepo<ScheduleUser>, IScheduleUserRepo
    {
        private readonly AppDbContext _appDbContext;

        public ScheduleUserRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
        public async Task<List<ScheduleUser>> GetSchedulesByDateAsync(DateTime date)
        {
            return await _appDbContext.ScheduleUsers
                .Include(s => s.Pregnancy)
                .ThenInclude(p => p.Account)
                .Where(s => s.Date == date)
                .ToListAsync();
        }
    }
}
