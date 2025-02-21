using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class CommentRepo : GenericRepo<Comment>, ICommentRepo
    {
        private readonly AppDbContext _appDbContext;

        public CommentRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
