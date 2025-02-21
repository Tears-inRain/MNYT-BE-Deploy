using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class BlogLikeRepo : GenericRepo<BlogLike> , IBlogLikeRepo
    {
        private readonly AppDbContext _appDbContext;

        public BlogLikeRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
