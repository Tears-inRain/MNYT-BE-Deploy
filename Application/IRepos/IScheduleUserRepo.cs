using Domain.Entities;

namespace Application.IRepos
{
    public interface IScheduleUserRepo : IGenericRepo<ScheduleUser>
    {
        Task<List<ScheduleUser>> GetSchedulesByDateAsync(DateOnly date);
    }
}
