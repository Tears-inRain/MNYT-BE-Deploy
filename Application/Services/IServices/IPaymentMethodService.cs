using Application.ViewModels.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.IServices
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDTO>> GetActivePaymentMethodsAsync();

        Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync();

        Task<PaymentMethodDTO?> GetPaymentMethodByIdAsync(int id);

        Task<bool> TogglePaymentMethodAsync(int id, bool isActive);
    }
}
