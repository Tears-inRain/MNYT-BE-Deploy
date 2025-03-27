using Application.Services.IServices;
using Application.ViewModels.Pregnancy;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PregnancyService : IPregnancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PregnancyService> _logger;
        public PregnancyService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<PregnancyService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadPregnancyDTO> CreatePregnancyAsync(PregnancyAddVM pregnancyAddVM)
        {
            _logger.LogInformation("Creating a pregnancy for accountId: {AccountId}", pregnancyAddVM.AccountId);

            var account = await _unitOfWork.AccountRepo.FindOneAsync(p => p.Id == pregnancyAddVM.AccountId);
            if (account == null)
            {
                _logger.LogWarning("Account {AccountId} not found, cannot create prenancy", pregnancyAddVM.AccountId);
                return null;
            }

            var pregnancy = _mapper.Map<Pregnancy>(pregnancyAddVM);
            pregnancy.CreateDate = DateTime.UtcNow;
            pregnancy.UpdateDate = DateTime.UtcNow;

            await _unitOfWork.PregnancyRepo.AddAsync(pregnancy);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully created pregnancy Id: {PregnancyId}", pregnancy.Id);

            return _mapper.Map<ReadPregnancyDTO>(pregnancy);
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.PregnancyRepo.GetByIdAsync(id);
            
            
            _unitOfWork.PregnancyRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<PregnancyVM>> GetAllAsync()
        {
            var items = await _unitOfWork.PregnancyRepo.GetAllAsync();
            var result = _mapper.Map<IList<PregnancyVM>>(items);
            return result;
        }

        public async Task<PregnancyVM> GetAsync(int id)
        {
            var item = await _unitOfWork.PregnancyRepo.GetByIdAsync(id);
            var result = _mapper.Map<PregnancyVM>(item);
            return result;
        }

        public async Task SoftDeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.PregnancyRepo.GetByIdAsync(id);
            
            _unitOfWork.PregnancyRepo.SoftDelete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task UpdateAsync(PregnancyVM pregnancyVM)
        {
            var itemToUpdate = _mapper.Map<Pregnancy>(pregnancyVM);
            _unitOfWork.PregnancyRepo.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IList<PregnancyVM>> GetAllByAccountIdAsync(int id)
        {
            var pregnancyQuery = _unitOfWork.PregnancyRepo.GetAllQueryable().Where(x => x.AccountId == id);
            var pregnancyList = await pregnancyQuery.ToListAsync(); 
            return _mapper.Map<IList<PregnancyVM>>(pregnancyList);
        }
    }
}
