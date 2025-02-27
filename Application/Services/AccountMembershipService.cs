using Application.IServices;
using Application.PaymentProviders.VnPay;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AccountMembershipService : IAccountMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;
        private readonly ILogger<AccountMembershipService> _logger;

        public AccountMembershipService(
            IUnitOfWork unitOfWork,
            IVnPayService vnPayService,
            ILogger<AccountMembershipService> logger)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
            _logger = logger;
        }

        public async Task<string> CreateVnPayPaymentAsync(int accountId, int membershipPlanId)
        {
            // Lấy MembershipPlan từ DB
            var plan = await _unitOfWork.MembershipPlanRepo.GetAsync(membershipPlanId);
            if (plan == null)
            {
                throw new Exception("Membership Plan not found.");
            }

            // Tạo record AccountMembership
            var membership = new AccountMembership
            {
                AccountId = accountId,
                MembershipPlanId = plan.Id,
                Amount = plan.Price,
                Status = "Pending",
                PaymentMethodId = (int)PaymentMethodEnum.VNPay,
                StartDate = null,
                EndDate = null
            };
            await _unitOfWork.AccountMembershipRepo.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            // Tạo URL VNPAY, dùng membership.Id làm vnp_TxnRef
            var amount = plan.Price; // decimal
            var orderDesc = $"Payment for membership plan {plan.Name} (AccountMembership #{membership.Id})";
            var orderId = membership.Id.ToString();
            var paymentUrl = _vnPayService.CreatePaymentUrl(amount, orderDesc, orderId);

            return paymentUrl;
        }

        public async Task<bool> HandleVnPayCallbackAsync(IDictionary<string, string> queryParams)
        {
            var validSignature = _vnPayService.VerifySignature(queryParams);
            if (!validSignature)
            {
                _logger.LogWarning("Invalid signature from VNPAY.");
                return false;
            }

            // Lấy vnp_ResponseCode, vnp_TxnRef
            if (!queryParams.TryGetValue("vnp_TxnRef", out var orderIdStr))
                return false;
            if (!int.TryParse(orderIdStr, out int membershipId))
                return false;

            var membership = await _unitOfWork.AccountMembershipRepo.GetAsync(membershipId);
            if (membership == null)
                return false;

            // Check response code
            if (queryParams.TryGetValue("vnp_ResponseCode", out var responseCode))
            {
                if (responseCode == "00")
                {
                    membership.Status = "Paid";
                    membership.StartDate = DateOnly.FromDateTime(DateTime.Now);

                    // Ví dụ gói 120 ngày
                    membership.EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(120));

                    _logger.LogInformation($"Payment success for membership #{membershipId}");
                }
                else
                {
                    membership.Status = "Failed";
                    _logger.LogInformation($"Payment failed for membership #{membershipId} - Code: {responseCode}");
                }
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
