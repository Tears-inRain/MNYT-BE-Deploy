using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO dto)
        {
            // Ở đây, bạn có thể kiểm tra model, role user, etc.
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var paymentUrl = await _vnPayService.CreateVnPayPaymentAsync(dto.AccountId, dto.MembershipPlanId);

                return Ok(ApiResponse<string>.SuccessResponse(
                    paymentUrl,
                    "VNPAY payment URL generated successfully."
                ));
            }
            catch (Exception ex)
            {
                // log ex
                return StatusCode(500, ApiResponse<string>.FailureResponse(
                    "An error occurred while creating VNPAY payment."
                ));
            }
        }

        //    Cấu hình "vnp_ReturnUrl": "https://localhost:5001/api/VnPay/Callback"
        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            // Lấy các query params
            var query = HttpContext.Request.Query;
            var queryParams = new Dictionary<string, string>();
            foreach (var q in query)
            {
                queryParams[q.Key] = q.Value;
            }

            var success = await _vnPayService.HandleVnPayCallbackAsync(queryParams);
            if (!success)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(
                    "Payment verification failed."
                ));
            }

            // Kiểm tra vnp_ResponseCode = "00" hay không
            if (queryParams.TryGetValue("vnp_ResponseCode", out var responseCode) && responseCode == "00")
            {
                // Thanh toán thành công
                return Ok(ApiResponse<string>.SuccessResponse(
                    "Payment successful!",
                    "VNPay payment has been processed successfully."
                ));
            }
            else
            {
                // Thanh toán thất bại
                return Ok(ApiResponse<string>.FailureResponse(
                    "Payment failed. Please try again or contact support."
                ));
            }
        }
    }
}
