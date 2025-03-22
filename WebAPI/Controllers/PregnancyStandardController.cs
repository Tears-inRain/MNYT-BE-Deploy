using Application.Services.IServices;
using Application.ViewModels.Accounts;
using Application.ViewModels;
using Application.ViewModels.PregnancyStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PregnancyStandardController : Controller
    {
        IPregnancyStandardService _pregnancyStandardService;

        public PregnancyStandardController(IPregnancyStandardService service)
        {
            _pregnancyStandardService = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync(List<PregnacyStandardAddVM> items)
        {
            foreach(var item in items)
            {
                await _pregnancyStandardService.AddSync(item);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]PregnacyStandardAddVM item)
        {

            if (!ModelState.IsValid)
            {
                return base.BadRequest(ApiResponse<PregnancyStandardVM>.FailureResponse(
                    "Invalid data.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }
            try
            {
                var updatedPS = await _pregnancyStandardService.UpdateAsync(id,item);
                if (updatedPS == null)
                {
                    return base.NotFound(ApiResponse<PregnancyStandardVM>.FailureResponse("Account not found or update failed."));
                }

                return base.Ok(ApiResponse<PregnancyStandardVM>.SuccessResponse(updatedPS, "Account updated successfully."));
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, ApiResponse<PregnancyStandardVM>.FailureResponse("An error occurred while updating the account."));
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _pregnancyStandardService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _pregnancyStandardService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _pregnancyStandardService.GetAsync(id);
            return Ok(item);
        }

        [HttpGet("{type}/{pregnancyType}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByTypeAndPregnancyTypeAsync(string type, string pregnancyType)
        {
            var item = await _pregnancyStandardService.GetByTypeAndPregnancyTypeAsync(type, pregnancyType);
            return Ok(item);
        }
    }
}
