using Application.IServices;
using Application.ViewModels.PregnancyStandard;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PregnancyStandardController : Controller
    {
        IPregnancyStandardService _pregnancyStandardService;

        public PregnancyStandardController(IPregnancyStandardService service)
        {
            _pregnancyStandardService = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(List<PregnacyStandardAddVM> items)
        {
            foreach(var item in items)
            {
                await _pregnancyStandardService.AddSync(item);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(PregnacyStandardAddVM item)
        {
            await _pregnancyStandardService.UpdateAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _pregnancyStandardService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _pregnancyStandardService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _pregnancyStandardService.GetAsync(id);
            return Ok(item);
        }
    }
}
