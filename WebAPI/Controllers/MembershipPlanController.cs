using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.MembershipPlan;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanService _membershipPlanService;

        public MembershipPlanController(IMembershipPlanService membershipPlanService)
        {
            _membershipPlanService = membershipPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMembershipPlanDTO planDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<CreateMembershipPlanDTO>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var createdDto = await _membershipPlanService.CreateMembershipPlanAsync(planDto);
                return Ok(ApiResponse<MembershipPlanDTO>.SuccessResponse(
                    createdDto,
                    "Membership plan created successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CreateMembershipPlanDTO>.FailureResponse(
                    "An error occurred while creating the membership plan."
                ));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var planDto = await _membershipPlanService.GetMembershipPlanByIdAsync(id);
                if (planDto == null)
                {
                    return NotFound(ApiResponse<MembershipPlanDTO>.FailureResponse(
                        "Membership plan not found."
                    ));
                }

                return Ok(ApiResponse<MembershipPlanDTO>.SuccessResponse(
                    planDto,
                    "Membership plan retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MembershipPlanDTO>.FailureResponse(
                    "An error occurred while retrieving the membership plan."
                ));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var plans = await _membershipPlanService.GetAllMembershipPlansAsync();
                return Ok(ApiResponse<IEnumerable<MembershipPlanDTO>>.SuccessResponse(
                    plans,
                    "List of membership plans retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MembershipPlanDTO>>.FailureResponse(
                    "An error occurred while retrieving the list of membership plans."
                ));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MembershipPlanDTO planDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<MembershipPlanDTO>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var updatedDto = await _membershipPlanService.UpdateMembershipPlanAsync(planDto);
                if (updatedDto == null)
                {
                    return NotFound(ApiResponse<MembershipPlanDTO>.FailureResponse(
                        "Membership plan not found or missing ID."
                    ));
                }

                return Ok(ApiResponse<MembershipPlanDTO>.SuccessResponse(
                    updatedDto,
                    "Membership plan updated successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MembershipPlanDTO>.FailureResponse(
                    "An error occurred while updating the membership plan."
                ));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _membershipPlanService.DeleteMembershipPlanAsync(id);
                if (!isDeleted)
                {
                    return NotFound(ApiResponse<bool>.FailureResponse(
                        "Membership plan not found."
                    ));
                }

                return Ok(ApiResponse<bool>.SuccessResponse(
                    true,
                    "Membership plan deleted successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailureResponse(
                    "An error occurred while deleting the membership plan."
                ));
            }
        }
    }
}
