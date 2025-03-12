using Application.Services.IServices;
using Application.ViewModels;
using Application.ViewModels.Payment;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CashPaymentController : ControllerBase
    {
        private readonly ICashPaymentService _cashPaymentService;

        public CashPaymentController(ICashPaymentService cashPaymentService)
        {
            _cashPaymentService = cashPaymentService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PayByCash([FromBody] CreatePaymentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<AccountMembership>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var membership = await _cashPaymentService.CreateCashPaymentAsync(dto.AccountId, dto.MembershipPlanId);

                return Ok(ApiResponse<AccountMembership>.SuccessResponse(
                    membership,
                    "Cash payment processed successfully."
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<AccountMembership>.FailureResponse(ex.Message));
            }
        }
    }
}
