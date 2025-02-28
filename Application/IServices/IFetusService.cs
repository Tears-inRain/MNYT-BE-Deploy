using Application.ViewModels.Fetus;
using Application.ViewModels.Pregnancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IFetusService
    {
        Task AddSync(FetusAddVM fetusAddVM);
        Task UpdateAsync(FetusVM fetusVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<FetusVM>> GetAllAsync();
        Task<FetusVM> GetAsync(int id);
    }
}
