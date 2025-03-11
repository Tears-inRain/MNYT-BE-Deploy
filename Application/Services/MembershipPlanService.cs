using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Application.ViewModels.MembershipPlan;
using Application.IServices;

namespace Application.Services
{
    public class MembershipPlanService : IMembershipPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MembershipPlanService> _logger;

        public MembershipPlanService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MembershipPlanService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MembershipPlanDTO> CreateMembershipPlanAsync(CreateMembershipPlanDTO createDto)
        {
            var entity = _mapper.Map<MembershipPlan>(createDto);

            await _unitOfWork.MembershipPlanRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = new MembershipPlanDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Duration = entity.Duration
            };

            return resultDto;
        }

        public async Task<MembershipPlanDTO?> GetMembershipPlanByIdAsync(int id)
        {
            var entity = await _unitOfWork.MembershipPlanRepo.GetAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<MembershipPlanDTO>(entity);
        }

        public async Task<IEnumerable<MembershipPlanDTO>> GetAllMembershipPlansAsync()
        {
            var entities = await _unitOfWork.MembershipPlanRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<MembershipPlanDTO>>(entities);
        }

        public async Task<MembershipPlanDTO?> UpdateMembershipPlanAsync(MembershipPlanDTO planDto)
        {
            if (planDto.Id <= 0)
            {
                return null;
            }

            var entity = await _unitOfWork.MembershipPlanRepo.GetAsync(planDto.Id);
            if (entity == null)
                return null;

            _mapper.Map(planDto, entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MembershipPlanDTO>(entity);
        }

        public async Task<bool> DeleteMembershipPlanAsync(int id)
        {
            var entity = await _unitOfWork.MembershipPlanRepo.GetAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.MembershipPlanRepo.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
