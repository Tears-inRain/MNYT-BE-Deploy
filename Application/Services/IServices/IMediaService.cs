using Application.ViewModels.Media;

namespace Application.Services.IServices
{
    public interface IMediaService
    {
        Task<ReadMediaDetailDTO> CreateMediaAsync(CreateMediaDTO createMediaDto);

        Task<ReadMediaDetailDTO?> GetMediaByIdAsync(int id);

        Task<IEnumerable<ReadMediaDetailDTO>> GetAllMediaAsync();

        Task<ReadMediaDetailDTO?> UpdateMediaAsync(int mediaId, UpdateMediaDTO updateDto);

        Task<bool> DeleteMediaAsync(int id);
    }
}
