using Application.ViewModels.Pregnancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IPregnancyService
    {
        Task AddSync(PregnancyAddVM pregnancyAddVM);
        Task Update(PregnancyVM pregnancyVM);
        Task Delete(int id);
        Task SoftDelete(PregnancyVM pregnancyVM);
        Task<IList<PregnancyVM>> GetAllAsync();
        Task<PregnancyVM> GetAsync(int id);
    }
}
