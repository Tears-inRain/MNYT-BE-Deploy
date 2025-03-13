using Application.ViewModels.Pregnancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IPregnancyService
    {
        Task AddSync(PregnancyAddVM pregnancyAddVM);
        Task UpdateAsync(PregnancyVM pregnancyVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<PregnancyVM>> GetAllAsync();
        Task<PregnancyVM> GetAsync(int id);
    }
}
