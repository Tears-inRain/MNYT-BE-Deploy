using Application.Services.IServices;
using Application.ViewModels;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountMembershipController : ControllerBase
    {
        private readonly IAccountMembershipService _membershipService;

        public AccountMembershipController(IAccountMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet("GetActive/{accountId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveMembership(int accountId)
        {
            try
            {
                var active = await _membershipService.GetActiveMembershipAsync(accountId);
                if (active == null)
                {
                    return Ok(ApiResponse<AccountMembership>.SuccessResponse(
                        null,
                        "No active membership for this account."
                    ));
                }
                return Ok(ApiResponse<AccountMembership>.SuccessResponse(
                    active,
                    "Active membership retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AccountMembership>.FailureResponse(
                    "Failed to get active membership."
                ));
            }
        }
    }
}
