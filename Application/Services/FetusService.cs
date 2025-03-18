using Application.Services.IServices;
using Application.ViewModels.Fetus;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class FetusService : IFetusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FetusService> _logger;
        private readonly IMapper _mapper; 
        public FetusService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<FetusService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadFetusDTO> CreateFetusAsync(FetusAddVM fetusAddVM)
        {
            _logger.LogInformation("Creating a fetus for pregnancyId: {PregnancyId}", fetusAddVM.PregnancyId);

            var pregnancy = await _unitOfWork.PregnancyRepo.FindOneAsync(p => p.Id == fetusAddVM.PregnancyId);
            if (pregnancy == null)
            {
                _logger.LogWarning("Prenancy {PrenancyId} not found, cannot create fetus", fetusAddVM.PregnancyId);
                return null;
            }

            var fetus = _mapper.Map<Fetus>(fetusAddVM);
            fetus.CreateDate = DateTime.UtcNow;
            fetus.UpdateDate = DateTime.UtcNow;

            await _unitOfWork.FetusRepo.AddAsync(fetus);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully created fetus Id: {FetusId}", fetus.Id);

            return _mapper.Map<ReadFetusDTO>(fetus);
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.FetusRepo.GetAsync(id);
            

            _unitOfWork.FetusRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<FetusVM>> GetAllAsync()
        {
            var items = await _unitOfWork.FetusRepo.GetAllAsync();
            var result = _mapper.Map<IList<FetusVM>>(items);
            return result;
        }

        public async Task<FetusVM> GetAsync(int id)
        {
            var item = await _unitOfWork.FetusRepo.GetAsync(id);
            var result = _mapper.Map<FetusVM>(item);
            return result;
        }

        public async Task SoftDeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.FetusRepo.GetAsync(id);

            _unitOfWork.FetusRepo.SoftDelete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(FetusVM fetusVM)
        {
            var itemToUpdate = _mapper.Map<Fetus>(fetusVM);
            _unitOfWork.FetusRepo.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IList<FetusVM>> GetAllByPregnancyIdAsync(int id)
        {
            var fetusQuery = _unitOfWork.FetusRepo.GetAllQueryable().Where(x => x.PregnancyId == id);
            var fetusList = await fetusQuery.ToListAsync();
            return _mapper.Map<IList<FetusVM>>(fetusList);
        }
    }
}
