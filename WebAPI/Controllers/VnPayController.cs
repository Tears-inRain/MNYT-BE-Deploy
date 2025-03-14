using Application.Services.IServices;
using Application.ViewModels;
using Application.ViewModels.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("CreatePayment")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO dto)
        {
            var paymentUrl = await _vnPayService.CreateVnPayPaymentAsync(dto.AccountId, dto.MembershipPlanId);
            return Ok(ApiResponse<string>.SuccessResponse(paymentUrl, "VNPAY payment URL generated successfully."));
        }

        //    Cấu hình "vnp_ReturnUrl": "https://abcd-1234.ngrok.io/api/VnPay/Callback"
        [HttpGet("Callback")]
        [AllowAnonymous]
        public async Task<IActionResult> Callback()
        {
            var queryParams = new Dictionary<string, string>();
            foreach (var q in HttpContext.Request.Query)
            {
                queryParams[q.Key] = q.Value;
            }

            var success = await _vnPayService.HandleVnPayCallbackAsync(queryParams);
            if (!success)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Payment verification failed."));
            }

            if (queryParams.TryGetValue("vnp_ResponseCode", out var responseCode) && responseCode == "00")
            {
                return Ok(ApiResponse<string>.SuccessResponse("Payment successful!",
                       "VNPay payment has been processed successfully."));
            }
            else
            {
                return Ok(ApiResponse<string>.FailureResponse("Payment failed. Please try again or contact support."));
            }
        }
    }
}
