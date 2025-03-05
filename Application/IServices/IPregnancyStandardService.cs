using Application.ViewModels.PregnancyStandard;

namespace Application.IServices
{
    public interface IPregnancyStandardService
    {
        Task AddSync(PregnancyStandardVM pregnancyAddVM);
        Task UpdateAsync(PregnancyStandardVM pregnancyVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<PregnancyStandardVM>> GetAllAsync();
        Task<PregnancyStandardVM> GetAsync(int id);
    }
}
