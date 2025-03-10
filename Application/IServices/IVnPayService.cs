using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IVnPayService
    {
        Task<string> CreateVnPayPaymentAsync(int accountId, int membershipPlanId);
        Task<bool> HandleVnPayCallbackAsync(IDictionary<string, string> queryParams);
    }
}
