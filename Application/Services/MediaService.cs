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

        public async Task<ReadMediaDetailDTO> CreateMediaAsync(CreateMediaDTO createMediaDto)
        {
            var entity = _mapper.Map<Media>(createMediaDto);
            entity.EntityType = "System";
            entity.EntityId = 0;

            await _unitOfWork.MediaRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = new ReadMediaDetailDTO
            {
                Id = entity.Id,
                EntityType = entity.EntityType,
                EntityId = entity.EntityId,
                Type = entity.Type,
                Url = entity.Url
            };

            return resultDto;
        }

        public async Task<ReadMediaDetailDTO?> GetMediaByIdAsync(int id)
        {
            var entity = await _unitOfWork.MediaRepo.GetByIdAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<ReadMediaDetailDTO>(entity);
        }

        public async Task<IEnumerable<ReadMediaDetailDTO>> GetAllMediaAsync()
        {
            var entities = await _unitOfWork.MediaRepo.GetAllQueryable().Where(m => m.EntityType == "System").ToListAsync();
            return _mapper.Map<IEnumerable<ReadMediaDetailDTO>>(entities);
        }

        public async Task<ReadMediaDetailDTO?> UpdateMediaAsync(int mediaId, UpdateMediaDTO updateDto)
        {
            var entity = await _unitOfWork.MediaRepo.GetByIdAsync(mediaId);
            if (entity == null)
                return null;

            _mapper.Map(updateDto, entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadMediaDetailDTO>(entity);
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
