using Application.Services.IServices;
using Application.ViewModels.Fetus;
using Application.ViewModels.Pregnancy;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FetusService : IFetusService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper; 
        public FetusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddSync(FetusAddVM fetusAddVM)
        {
            var fetus = _mapper.Map<Fetus>(fetusAddVM);
            await _unitOfWork.FetusRepo.AddAsync(fetus);
            await _unitOfWork.SaveChangesAsync();
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
    }
}
