using Application.ViewModels.Media;

namespace Application.IServices
{
    public interface IMediaService
    {
        Task<MediaDTO> CreateMediaAsync(CreateMediaDTO createMediaDto);

        Task<MediaDTO?> GetMediaByIdAsync(int id);

        Task<IEnumerable<MediaDTO>> GetAllMediaAsync();

        Task<MediaDTO?> UpdateMediaAsync(MediaDTO mediaDto);

        Task<bool> DeleteMediaAsync(int id);
    }
}
