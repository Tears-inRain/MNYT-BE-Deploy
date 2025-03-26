using Application.IRepos;
using Application.ViewModels.Post;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class BlogPostRepo : GenericRepo<BlogPost>, IBlogPostRepo
    {
        private readonly AppDbContext _appDbContext;

        public BlogPostRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
        public async Task<List<TopAuthorDTO>> GetTopAuthorAsync(int count)
        {
            var topAuthors = await _appDbContext.BlogPosts
                .Where(p => !p.IsDeleted)
                .Include(p => p.Author)
                .GroupBy(p => new { p.AuthorId, p.Author.FullName })
                .Select(group => new TopAuthorDTO
                {
                    AuthorId = (int)group.Key.AuthorId,
                    AuthorName = group.Key.FullName,
                    PostCount = group.Count()
                })
                .OrderByDescending(a => a.PostCount)
                .Take(count)
                .ToListAsync();
            return topAuthors;
        }
    }
}
