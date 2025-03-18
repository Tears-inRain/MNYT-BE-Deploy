using Application.ViewModels.Fetus;

namespace Application.Services.IServices
{
    public interface IFetusService
    {
        Task<ReadFetusDTO> CreateFetusAsync(FetusAddVM fetusAddVM);
        Task UpdateAsync(FetusVM fetusVM);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IList<FetusVM>> GetAllAsync();
        Task<FetusVM> GetAsync(int id);
        Task<IList<FetusVM>> GetAllByPregnancyIdAsync(int id);
    }
}
