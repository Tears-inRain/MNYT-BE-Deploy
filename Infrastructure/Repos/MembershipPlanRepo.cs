using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class MembershipPlanRepo : GenericRepo<MembershipPlan> , IMembershipPlanRepo
    {
        private readonly AppDbContext _appDbContext;
        public MembershipPlanRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
