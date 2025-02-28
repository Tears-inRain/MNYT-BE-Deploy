using Application.ViewModels.MembershipPlan;
using Domain.Entities;

namespace Application.IServices
{
    public interface IMembershipPlanService
    {
        Task<MembershipPlanDTO> CreateMembershipPlanAsync(CreateMembershipPlanDTO planDto);

        Task<MembershipPlanDTO?> GetMembershipPlanByIdAsync(int id);

        Task<IEnumerable<MembershipPlanDTO>> GetAllMembershipPlansAsync();

        Task<MembershipPlanDTO?> UpdateMembershipPlanAsync(MembershipPlanDTO planDto);

        Task<bool> DeleteMembershipPlanAsync(int id);
    }
}
