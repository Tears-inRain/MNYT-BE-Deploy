using Application.ViewModels.Media;

namespace Application.Services.IServices
{
    public interface IMediaService
    {
        Task<ReadMediaDTO> CreateMediaAsync(CreateMediaDTO createMediaDto);

        Task<ReadMediaDTO?> GetMediaByIdAsync(int id);

        Task<IEnumerable<ReadMediaDTO>> GetAllMediaAsync();

        Task<ReadMediaDTO?> UpdateMediaAsync(ReadMediaDTO mediaDto);

        Task<bool> DeleteMediaAsync(int id);
    }
}
