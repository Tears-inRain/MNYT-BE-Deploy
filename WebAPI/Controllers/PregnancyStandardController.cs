using Application.IServices;
using Application.ViewModels.PregnancyStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync(PregnacyStandardAddVM item)
        {
            await _pregnancyStandardService.UpdateAsync(item);
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
    }
}
