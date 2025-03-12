using Application.Services.IServices;
using Application.ViewModels.Media;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MediaService> _logger;

        public MediaService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MediaService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MediaDTO> CreateMediaAsync(CreateMediaDTO createMediaDto)
        {
            var entity = _mapper.Map<Media>(createMediaDto);

            await _unitOfWork.MediaRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = new MediaDTO
            {
                Id = entity.Id,
                Type = entity.Type,
                Url = entity.Url
            };

            return resultDto;
        }

        public async Task<MediaDTO?> GetMediaByIdAsync(int id)
        {
            var entity = await _unitOfWork.MediaRepo.GetAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<MediaDTO>(entity);
        }

        public async Task<IEnumerable<MediaDTO>> GetAllMediaAsync()
        {
            var entities = await _unitOfWork.MediaRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<MediaDTO>>(entities);
        }

        public async Task<MediaDTO?> UpdateMediaAsync(MediaDTO mediaDto)
        {
            if (mediaDto.Id <= 0)
            {
                return null;
            }

            var entity = await _unitOfWork.MediaRepo.GetAsync(mediaDto.Id);
            if (entity == null)
                return null;

            _mapper.Map(mediaDto, entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MediaDTO>(entity);
        }

        public async Task<bool> DeleteMediaAsync(int id)
        {
            var entity = await _unitOfWork.MediaRepo.GetAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.MediaRepo.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
