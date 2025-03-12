using Application.Services.IServices;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayLibrary _vnPayLibrary;
        private readonly IAccountMembershipService _membershipService;
        private readonly ILogger<VnPayService> _logger;

        public VnPayService(
            IUnitOfWork unitOfWork,
            IVnPayLibrary vnPayLibrary,
            IAccountMembershipService membershipService,
            ILogger<VnPayService> logger)
        {
            _unitOfWork = unitOfWork;
            _vnPayLibrary = vnPayLibrary;
            _membershipService = membershipService;
            _logger = logger;
        }

        public async Task<string> CreateVnPayPaymentAsync(int accountId, int membershipPlanId)
        {
            _logger.LogInformation(
                "Begin CreateVnPayPaymentAsync - accountId={AccountId}, membershipPlanId={MembershipPlanId}",
                accountId, membershipPlanId);

            var currentActive = await _membershipService.GetActiveMembershipAsync(accountId);

            AccountMembership newMembership = null!;

            if (currentActive == null)
            {
                _logger.LogInformation("No active membership found => creating a new membership.");

                newMembership = await _membershipService.CreateNewMembershipAsync(accountId, membershipPlanId);
            }
            else
            {
                _logger.LogInformation("Active membership #{MembershipId} found => upgrading to plan #{PlanId}.",
                    currentActive.Id, membershipPlanId);

                newMembership = await _membershipService.UpgradeMembershipAsync(accountId, membershipPlanId);
            }

            newMembership.PaymentMethodId = (int)PaymentMethodEnum.VNPAY;

            var plan = await _unitOfWork.MembershipPlanRepo.GetAsync(newMembership.MembershipPlanId ?? 0);
            if (plan == null)
            {
                throw new Exception("The membership plan does not exist.");
            }

            decimal amount = newMembership.Amount ?? 0;
            string orderDesc = $"Payment for membership plan {plan.Name} (AccountMembership #{newMembership.Id})";
            string orderId = newMembership.Id.ToString();

            _logger.LogInformation("Calling CreatePaymentUrl with amount={Amount}, desc={Desc}, orderId={OrderId}",
                                   amount, orderDesc, orderId);

            var paymentUrl = _vnPayLibrary.CreatePaymentUrl(amount, orderDesc, orderId);

            _logger.LogInformation("VNPAY payment URL generated: {PaymentUrl}", paymentUrl);

            return paymentUrl;
        }

        public async Task<bool> HandleVnPayCallbackAsync(IDictionary<string, string> queryParams)
        {
            _logger.LogInformation("Start HandleVnPayCallbackAsync with queryParams: {QueryString}",
                                   string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}")));

            var validSignature = _vnPayLibrary.VerifySignature(queryParams);
            if (!validSignature)
            {
                _logger.LogWarning("Invalid signature from VNPAY. Full query: {Query}",
                                   string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}")));
                return false;
            }

            if (!queryParams.TryGetValue("vnp_TxnRef", out var orderIdStr))
            {
                _logger.LogWarning("Missing vnp_TxnRef in queryParams.");
                return false;
            }
            if (!int.TryParse(orderIdStr, out int membershipId))
            {
                _logger.LogWarning("Could not parse membershipId from vnp_TxnRef = {OrderIdStr}", orderIdStr);
                return false;
            }

            var membership = await _unitOfWork.AccountMembershipRepo.GetAsync(membershipId);
            if (membership == null)
                return false;

            var plan = await _unitOfWork.MembershipPlanRepo.GetAsync(membership.MembershipPlanId ?? 0);
            if (plan == null)
                return false;

            if (queryParams.TryGetValue("vnp_ResponseCode", out var responseCode))
            {
                if (responseCode == "00")
                {
                    membership.Status = "Active";
                    membership.PaymentStatus = "Paid";
                    membership.StartDate = DateOnly.FromDateTime(DateTime.Now);
                    membership.EndDate = membership.StartDate.Value.AddDays(plan.Duration);

                    _logger.LogInformation("Payment success for membership #{MembershipId}, set Active.", membershipId);
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
