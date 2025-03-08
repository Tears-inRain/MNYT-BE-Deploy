using Application.IRepos;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            return await _dbSet.ToListAsync();
        }

        public async Task<TModel> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void SoftDelete(TModel model)
        {
            model.IsDeleted = true;
        }

        public void Update(TModel model)
        {
            _dbSet.Update(model);
        }
        public virtual async Task<IEnumerable<TModel>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<TModel> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.ToListAsync();
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

            return query;
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

            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}
