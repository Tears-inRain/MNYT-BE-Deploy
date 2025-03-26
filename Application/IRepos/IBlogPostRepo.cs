using Application.ViewModels.Post;
using Domain.Entities;

namespace Application.IRepos
{
    public interface IBlogPostRepo : IGenericRepo<BlogPost>
    {
        public Task<List<TopAuthorDTO>> GetTopAuthorAsync(int count);
    }
}
