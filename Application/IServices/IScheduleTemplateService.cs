using Application.ViewModels.Pregnancy;
using Application.ViewModels.ScheduleTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IScheduleTemplateService
    {
        Task AddAsync(ScheduleTemplateAddVM pregnancyAddVM);
        Task UpdateAsync(ScheduleTemplateVM pregnancyVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<ScheduleTemplateVM>> GetAllAsync();
        Task<ScheduleTemplateVM> GetAsync(int id);
    }
}
