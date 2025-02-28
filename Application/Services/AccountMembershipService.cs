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
            var plan = await _unitOfWork.MembershipPlanRepo.GetAsync(membershipPlanId);
            if (plan == null)
            {
                throw new Exception("Membership Plan not found.");
            }

            var membership = new AccountMembership
            {
                AccountId = accountId,
                MembershipPlanId = plan.Id,
                Amount = plan.Price,
                Status = "Pending",
                PaymentStatus = "Pending",
                PaymentMethodId = (int)PaymentMethodEnum.VNPay,
                StartDate = null,
                EndDate = null
            };
            await _unitOfWork.AccountMembershipRepo.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

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

            var plan = await _unitOfWork.MembershipPlanRepo.GetAsync(membership.MembershipPlanId ?? 0);
            if (plan == null)
                return false;

            // Check response code
            if (queryParams.TryGetValue("vnp_ResponseCode", out var responseCode))
            {
                if (responseCode == "00")
                {
                    membership.Status = "Active";
                    membership.PaymentStatus = "Paid";
                    membership.StartDate = DateOnly.FromDateTime(DateTime.Now);
                    membership.EndDate = membership.StartDate.Value.AddDays(plan.Duration);

                    _logger.LogInformation($"Payment success for membership #{membershipId}");
                }
                else
                {
                    membership.PaymentStatus = "Failed";
                    _logger.LogInformation($"Payment failed for membership #{membershipId} - Code: {responseCode}");
                }
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
