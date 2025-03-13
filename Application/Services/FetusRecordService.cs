using Application.Services.IServices;
using Application.ViewModels.Fetus;
using Application.ViewModels.FetusRecord;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FetusRecordService : IFetusRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FetusRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(FetusRecordAddVM fetusRecordAddVM)
        {
            FetusRecord record = _mapper.Map<FetusRecord>(fetusRecordAddVM);
            await _unitOfWork.FetusRecordRepo.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.FetusRecordRepo.GetAsync(id);


            _unitOfWork.FetusRecordRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<FetusRecordVM>> GetAllAsync()
        {
            var items = await _unitOfWork.FetusRecordRepo.GetAllAsync();
            var result = _mapper.Map<IList<FetusRecordVM>>(items);
            return result;
        }

        public async Task<FetusRecordVM> GetAsync(int id)
        {
            var item = await _unitOfWork.FetusRepo.GetAsync(id);
            var result = _mapper.Map<FetusRecordVM>(item);
            return result;
        }

        public async Task SoftDelete(int id)
        {
            var record = await _unitOfWork.FetusRecordRepo.GetAsync(id);
            
            _unitOfWork.FetusRecordRepo.SoftDelete(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(FetusRecordVM fetusRecordVM)
        {
            var record = _mapper.Map<FetusRecord>(fetusRecordVM);
            _unitOfWork.FetusRecordRepo.Update(record);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
