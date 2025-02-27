using Application.IServices;
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
    public class PregnancyService : IPregnancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PregnancyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddSync(PregnancyAddVM pregnancyAddVM)
        {
            var pregnancy = _mapper.Map<Pregnancy>(pregnancyAddVM);
            await _unitOfWork.PregnancyRepo.AddAsync(pregnancy);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.PregnancyRepo.GetAsync(id);
            if (itemToDelete != null)
            {
                throw new KeyNotFoundException("Pregnancy not found");
            }
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
            var item = await _unitOfWork.PregnancyRepo.GetAsync(id);
            var result = _mapper.Map<PregnancyVM>(item);
            return result;
        }

        public async Task SoftDeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.PregnancyRepo.GetAsync(id);
            if (itemToDelete != null)
            {
                throw new KeyNotFoundException("Pregnancy not found");
            }
            _unitOfWork.PregnancyRepo.SoftDelete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task UpdateAsync(PregnancyVM pregnancyVM)
        {
            var itemToUpdate = _mapper.Map<Pregnancy>(pregnancyVM);
            _unitOfWork.PregnancyRepo.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
