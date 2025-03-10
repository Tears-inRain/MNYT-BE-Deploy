using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IAccountMembershipService
    {
        Task<AccountMembership?> GetActiveMembershipAsync(int accountId);
        Task<AccountMembership> CreateNewMembershipAsync(int accountId, int membershipPlanId);
        Task<AccountMembership> UpgradeMembershipAsync(int accountId, int newPlanId);
    }
}
