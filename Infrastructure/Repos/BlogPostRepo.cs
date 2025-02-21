using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class BlogPostRepo : GenericRepo<BlogPost>, IBlogPostRepo
    {
        private readonly AppDbContext _appDbContext;

        public BlogPostRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
