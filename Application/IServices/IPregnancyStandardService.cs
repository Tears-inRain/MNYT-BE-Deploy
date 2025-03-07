using Application.ViewModels.PregnancyStandard;

namespace Application.IServices
{
    public interface IPregnancyStandardService
    {
        Task AddSync(PregnacyStandardAddVM pregnancyAddVM);
        Task UpdateAsync(PregnacyStandardAddVM pregnancyVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<PregnancyStandardVM>> GetAllAsync();
        Task<PregnancyStandardVM> GetAsync(int id);
    }
}
