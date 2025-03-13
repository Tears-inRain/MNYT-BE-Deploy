using Domain.Entities;

namespace Application.IRepos
{
    public interface IPregnancyStandardRepo: IGenericRepo<PregnancyStandard>
    {
         Task<IEnumerable<PregnancyStandard?>> GetByTypeAndPregnancyTypeAsync(string type, string pregnancyType);
    }
}
