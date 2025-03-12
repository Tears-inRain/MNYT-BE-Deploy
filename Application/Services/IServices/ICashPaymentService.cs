using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface ICashPaymentService
    {
        Task<AccountMembership> CreateCashPaymentAsync(int accountId, int membershipPlanId);
    }
}
