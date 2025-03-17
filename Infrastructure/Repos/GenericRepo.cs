using Application.IRepos;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repos
{
    public class GenericRepo<TModel> : IGenericRepo<TModel> where TModel : BaseEntity
    {
        protected DbSet<TModel> _dbSet;

        public GenericRepo(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<TModel>();
        }

        public async Task AddAsync(TModel model)
        {
            await _dbSet.AddAsync(model);
        }

        public void Delete(TModel model)
        {
            _dbSet.Remove(model);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var result = _dbSet;
            foreach (var item in result)
            {
                if (item.IsDeleted)
                {
                    result.Remove(item);
                }
            }
            return await result.ToListAsync();
        }

        public async Task<TModel> GetAsync(int id)
        {
            TModel? model = await _dbSet.FindAsync(id);
            if (model == null || model.IsDeleted == true)
            {
                throw new Exception($"{model} not found");
            }
            return model;
        }

        public void SoftDelete(TModel model)
        {
            model.IsDeleted = true;
        }

        public void Update(TModel model)
        {
            if (model == null || model.IsDeleted == true)
            {
                throw new Exception("Data is not exist");
            }
            _dbSet.Update(model);
        }
        public virtual async Task<IEnumerable<TModel>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<TModel> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.Where(x=>x.IsDeleted!=true).ToListAsync();
        }

        public virtual IQueryable<TModel> GetAllQueryable(string includeProperties = "")
        {
            IQueryable<TModel> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query.Where(x => !x.IsDeleted);
        }

        public async Task<TModel> FindOneAsync(Expression<Func<TModel, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TModel> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.Where(x => !x.IsDeleted).FirstOrDefaultAsync(predicate);
        }
    }
}
