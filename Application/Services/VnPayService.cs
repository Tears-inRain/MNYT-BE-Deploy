using Application.PaymentProviders.VnPay;
using Application.Services.IServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountMembershipService _membershipService;
        private readonly ILogger<VnPayService> _logger;

        public VnPayService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IAccountMembershipService membershipService,
            ILogger<VnPayService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

            decimal amount = newMembership.Amount ?? 0;
            string orderDesc = $"Payment for membership plan ... (#{newMembership.Id})";

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", _configuration["Vnpay:vnp_Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:vnp_TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:vnp_CurrCode"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            vnpay.AddRequestData("vnp_IpAddr", PaymentProviders.VnPay.Utils.GetIpAddress(_httpContextAccessor));
            vnpay.AddRequestData("vnp_OrderInfo", orderDesc);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:vnp_ReturnUrl"]);
            vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_Locale", _configuration["Vnpay:vnp_Locale"]);
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnpay.CreateRequestUrl(_configuration["Vnpay:vnp_BaseUrl"], _configuration["Vnpay:vnp_HashSecret"]);
            _logger.LogInformation("Payment URL: {0}", paymentUrl);

            return paymentUrl;
        }

        public async Task<bool> HandleVnPayCallbackAsync(IDictionary<string, string> queryParams)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (k, v) in queryParams)
            {
                if (k.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(k, v);
                }
            }

            if (!queryParams.TryGetValue("vnp_SecureHash", out var inputHash))
            {
                return false;
            }

            bool isValid = vnpay.ValidateSignature(inputHash, _configuration["Vnpay:vnp_HashSecret"]);
            if (!isValid)
            {
                _logger.LogWarning("Invalid signature from vnpay");
                return false;
            }

            if (!queryParams.TryGetValue("vnp_TxnRef", out var txnRef))
            {
                return false;
            }
            if (!int.TryParse(txnRef, out int membershipId))
            {
                return false;
            }
            var membership = await _unitOfWork.AccountMembershipRepo.GetAsync(membershipId);
            if (membership == null) return false;

            if (queryParams.TryGetValue("vnp_ResponseCode", out var resp) && resp == "00")
            {
                membership.Status = "Active";
                membership.PaymentStatus = "Paid";
            }
            else
            {
                membership.PaymentStatus = "Failed";
            }
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
