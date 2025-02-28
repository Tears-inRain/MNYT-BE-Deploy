using Application.IServices;
using Application.ViewModels.Payment;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentMethodService> _logger;

        public PaymentMethodService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PaymentMethodService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetActivePaymentMethodsAsync()
        {
            var allPayments = await _unitOfWork.PaymentMethodRepo.GetAllAsync();
            var activePayments = allPayments.Where(p => p.IsActive);

            return _mapper.Map<IEnumerable<PaymentMethodDTO>>(activePayments);
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync()
        {
            var allPayments = await _unitOfWork.PaymentMethodRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentMethodDTO>>(allPayments);
        }

        public async Task<PaymentMethodDTO?> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepo.GetAsync(id);
            if (paymentMethod == null) return null;

            return _mapper.Map<PaymentMethodDTO>(paymentMethod);
        }

        public async Task<bool> TogglePaymentMethodAsync(int id, bool isActive)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepo.GetAsync(id);
            if (paymentMethod == null)
                return false;

            paymentMethod.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
