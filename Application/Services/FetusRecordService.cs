using Application.IServices;
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

        public async void DeleteAsync(int id)
        {
            var record = await _unitOfWork.FetusRecordRepo.GetAsync(id);
            if (record != null)
            {
                throw new KeyNotFoundException("Record not found");
            }
            _unitOfWork.FetusRecordRepo.Delete(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FetusRecordVM> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.FetusRecordRepo.GetAsync(id);
            var convertedRecord = _mapper.Map<FetusRecordVM>(record);
            return convertedRecord;
        }

        public async void SoftDelete(int id)
        {
            var record = await _unitOfWork.FetusRecordRepo.GetAsync(id);
            if (record != null)
            {
                throw new KeyNotFoundException("Record not found");
            }
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
