using Application.ViewModels.AccountMembership;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IAccountMembershipService
    {
        Task<ReadAccountMembershipDTO?> GetActiveMembershipAsync(int accountId);
        Task<IEnumerable<ReadAccountMembershipDTO>> GetAllAccountMembershipAsync();
        Task<AccountMembership> CreateNewMembershipAsync(int accountId, int membershipPlanId);
        Task<AccountMembership> UpgradeMembershipAsync(int accountId, int newPlanId);
    }
}
