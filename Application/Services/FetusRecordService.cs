using Application.Services.IServices;
using Application.ViewModels.Fetus;
using Application.ViewModels.FetusRecord;
using AutoMapper;
using AutoMapper.Internal;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddFetusRecordAsync(List<FetusRecordAddVM> fetusRecordAddVMs)
        {
            if (fetusRecordAddVMs == null || fetusRecordAddVMs.Count == 0)
                throw new ArgumentException("No records to add");

            var fetusId = fetusRecordAddVMs.First().FetusId;

            var existingRecords = await _unitOfWork.FetusRecordRepo
                .GetAllQueryable()
                .Where(r => r.FetusId == fetusId)
                .OrderBy(r => r.Date)
                .ToListAsync();
            
            var newRecords =  _mapper.Map<List<FetusRecord>>(fetusRecordAddVMs); 

            var allRecords = existingRecords.Concat(newRecords).OrderBy(r => r.Date).ToList();

            AdjustPeriods(allRecords);

            await _unitOfWork.FetusRecordRepo.AddRangeAsync(newRecords);
            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.FetusRecordRepo.UpdateRange(existingRecords);
            await _unitOfWork.SaveChangesAsync();
        }

        private void AdjustPeriods(List<FetusRecord> records)
        {
            if (records == null || records.Count == 0)
                return;

            records = records.OrderBy(r => r.Date).ToList(); // ngày tăng dần

            var latestRecord = records.Last(); // lấy Record mới nhất
            if (!latestRecord.Date.HasValue)
                return;

            var latestDate = latestRecord.Date.Value.ToDateTime(TimeOnly.MinValue);
            var basePeriod = latestRecord.InputPeriod; // tuần nhập vào của record mới nhất

            if (!records[0].Date.HasValue)
                return;

            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Date.HasValue)
                {
                    var currentDate = records[i].Date.Value.ToDateTime(TimeOnly.MinValue);
                    var daysDifference = (latestDate - currentDate).TotalDays;

                    records[i].Period = basePeriod - (int?)(daysDifference / 7);
                }
            }
        }

        //private void AdjustPeriods(List<FetusRecord> records)
        //{
        //    for (int i = 0; i < records.Count; i++)
        //    {
        //        records[i].Period = records[i].InputPeriod - (records.Last().InputPeriod - records[i].InputPeriod);
        //    }
        //}

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.FetusRecordRepo.GetByIdAsync(id);


            _unitOfWork.FetusRecordRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<FetusRecordVM>> GetAllAsync()
        {
            var items = await _unitOfWork.FetusRecordRepo.GetAllAsync();
            var result = _mapper.Map<IList<FetusRecordVM>>(items);
            return result;
        }

        public async Task<IList<FetusRecordVM>> GetAllByFetusIdAsync(int id)
        {
            var recordQuery = _unitOfWork.FetusRecordRepo.GetAllQueryable().Where(f=>f.FetusId == id);
            var recordList = await recordQuery.ToListAsync();
            return _mapper.Map<IList<FetusRecordVM>>(recordList);
        }

        public async Task<FetusRecordVM> GetAsync(int id)
        {
            var item = await _unitOfWork.FetusRepo.GetByIdAsync(id);
            var result = _mapper.Map<FetusRecordVM>(item);
            return result;
        }

        public async Task SoftDelete(int id)
        {
            var record = await _unitOfWork.FetusRecordRepo.GetByIdAsync(id);
            
            _unitOfWork.FetusRecordRepo.SoftDelete(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(FetusRecordVM fetusRecordVM)
        {
            var record = _mapper.Map<FetusRecord>(fetusRecordVM);
            _unitOfWork.FetusRecordRepo.Update(record);

            var allRecords = await _unitOfWork.FetusRecordRepo
                .GetAllQueryable()
                .Where(r => r.FetusId == record.FetusId)
                .OrderBy(r => r.Date)
                .ToListAsync();
        

            AdjustPeriods(allRecords);

            _unitOfWork.FetusRecordRepo.UpdateRange(allRecords);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
