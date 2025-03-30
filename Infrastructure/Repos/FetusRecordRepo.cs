using Application.IRepos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class FetusRecordRepo : GenericRepo<FetusRecord>, IFetusRecordRepo
    {
        private readonly AppDbContext _appDbContext;

        public FetusRecordRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
        //public async Task AddFetusRecordAsync(FetusRecord newRecord)
        //{
        //    if (newRecord.FetusId == null || newRecord.Date == null || newRecord.InputPeriod == null)
        //    {
        //        throw new ArgumentException("FetusId, Date, and InputPeriod are required.");
        //    }
        //    var records = await _dbSet
        //        .Where(r => r.FetusId == newRecord.FetusId)
        //        .OrderBy(r => r.Date)
        //        .ToListAsync();
        //    records.Add(newRecord);
        //    records = records.OrderBy(r => r.Date).ToList();
        //    AdjustPeriods(records);

        //    await _dbSet.AddAsync(newRecord);
        //    _appDbContext.UpdateRange(records);
        //    await _appDbContext.SaveChangesAsync();
        //}

        //private void AdjustPeriods(List<FetusRecord> records)
        //{
        //    for (int i = records.Count - 1; i > 0; i--)
        //    {
        //        int daysDiff = (records[i].Date.Value.DayNumber - records[i - 1].Date.Value.DayNumber);
        //        int weeksDiff = daysDiff / 7;

        //        records[i - 1].Period = records[i].Period - weeksDiff;
        //    }
        //}
        public async Task AddRangeAsync(IEnumerable<FetusRecord> records)
        {
            await _dbSet.AddRangeAsync(records);
        }
        public void UpdateRange(IEnumerable<FetusRecord> records)
        {
            _dbSet.UpdateRange(records);
        }

    }
}
