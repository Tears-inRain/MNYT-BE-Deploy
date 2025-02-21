using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class BlogBookmarkRepo : GenericRepo<BlogBookmark>, IBlogBookmarkRepo
    {
        private readonly AppDbContext _appDbContext;
        public BlogBookmarkRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
