using Application.Services.IServices;
using Application.ViewModels;
using Application.ViewModels.AccountMembership;
using Application.ViewModels.Blog;
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
                    return Ok(ApiResponse<ReadAccountMembershipDTO>.SuccessResponse(
                        null,
                        "No active membership for this account."
                    ));
                }
                return Ok(ApiResponse<ReadAccountMembershipDTO>.SuccessResponse(
                    active,
                    "Active membership retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReadAccountMembershipDTO>.FailureResponse(
                    "Failed to get active membership."
                ));
            }
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAccountMembership()
        {
            var result = await _membershipService.GetAllAccountMembershipAsync();
            return Ok(ApiResponse<IEnumerable<ReadAccountMembershipDTO>>.SuccessResponse(
                result,
                "All account membership retrieved successfully."
            ));
        }
    }
}
