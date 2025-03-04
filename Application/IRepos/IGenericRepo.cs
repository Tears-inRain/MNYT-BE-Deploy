using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepos
{
    public interface IGenericRepo<TModel> where TModel : BaseEntity
    {
        Task AddAsync(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
        void SoftDelete(TModel model);
        Task<IEnumerable<TModel>> GetAllAsync();
        IQueryable<TModel> GetAllQueryable(string includeProperties = "");
        Task<TModel> GetAsync(int id);

    }
}
