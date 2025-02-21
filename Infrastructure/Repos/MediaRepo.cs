using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class MediaRepo : GenericRepo<Media>, IMediaRepo
    {
        private readonly AppDbContext _appDbContext;

        public MediaRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
