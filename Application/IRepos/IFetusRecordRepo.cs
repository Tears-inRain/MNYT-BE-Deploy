using Domain.Entities;

namespace Application.IRepos
{
    public interface IFetusRecordRepo : IGenericRepo<FetusRecord>
    {
        Task AddFetusRecordAsync(FetusRecord newRecord);
        Task AddRangeAsync(IEnumerable<FetusRecord> records);
        void UpdateRange(IEnumerable<FetusRecord> records);
    }
}
