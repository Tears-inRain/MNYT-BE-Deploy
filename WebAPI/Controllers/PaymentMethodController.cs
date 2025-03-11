using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.ViewModels.Payment;
using Application.IServices;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _service;

        public PaymentMethodController(IPaymentMethodService service)
        {
            _service = service;
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveMethods()
        {
            var methods = await _service.GetActivePaymentMethodsAsync();
            return Ok(ApiResponse<IEnumerable<PaymentMethodDTO>>.SuccessResponse(methods,
                "List of active payment methods retrieved successfully."));
        }

        [HttpGet]
        //[Authorize(Roles = "Manager,Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var allMethods = await _service.GetAllPaymentMethodsAsync();
            return Ok(ApiResponse<IEnumerable<PaymentMethodDTO>>.SuccessResponse(allMethods,
                "List of all payment methods retrieved successfully."));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var method = await _service.GetPaymentMethodByIdAsync(id);
            if (method == null)
            {
                return NotFound(ApiResponse<PaymentMethodDTO>.FailureResponse("Payment method not found."));
            }

            return Ok(ApiResponse<PaymentMethodDTO>.SuccessResponse(method,
                "Payment method retrieved successfully."));
        }

        [HttpPut("{id}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] TogglePaymentMethodDTO toggleDto)
        {
            var result = await _service.TogglePaymentMethodAsync(id, toggleDto.IsActive);
            if (!result)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Payment method not found."));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true,
                toggleDto.IsActive
                ? "Payment method has been activated."
                : "Payment method has been deactivated."));
        }
    }
}
