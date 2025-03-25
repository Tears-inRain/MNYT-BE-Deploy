using Application.Services.IServices;
using Application.ViewModels.Media;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ReadMediaDTO> CreateMediaAsync(CreateMediaDTO createMediaDto)
        {
            var entity = _mapper.Map<Media>(createMediaDto);

            await _unitOfWork.MediaRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = new ReadMediaDTO
            {
                Id = entity.Id,
                Type = entity.Type,
                Url = entity.Url
            };

            return resultDto;
        }

        public async Task<ReadMediaDTO?> GetMediaByIdAsync(int id)
        {
            var entity = await _unitOfWork.MediaRepo.GetByIdAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<ReadMediaDTO>(entity);
        }

        public async Task<IEnumerable<ReadMediaDTO>> GetAllMediaAsync()
        {
            var entities = await _unitOfWork.MediaRepo.GetAllQueryable().Where(m => m.EntityType == "System").ToListAsync();
            return _mapper.Map<IEnumerable<ReadMediaDTO>>(entities);
        }

        public async Task<ReadMediaDTO?> UpdateMediaAsync(int mediaId, UpdateMediaDTO updateDto)
        {
            var entity = await _unitOfWork.MediaRepo.GetByIdAsync(mediaId);
            if (entity == null)
                return null;

            _mapper.Map(updateDto, entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadMediaDTO>(entity);
        }

        public async Task<bool> DeleteMediaAsync(int id)
        {
            var entity = await _unitOfWork.MediaRepo.GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.MediaRepo.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
