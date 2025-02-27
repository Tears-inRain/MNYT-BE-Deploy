using Application.IServices;
using Application.ViewModels.Pregnancy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PregnancyController : ControllerBase
    {
        IPregnancyService _pregnancyservice;

        public PregnancyController(IPregnancyService service)
        {
            _pregnancyservice = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PregnancyAddVM item)
        {
            await _pregnancyservice.AddSync(item);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(PregnancyVM item)
        {
            await _pregnancyservice.UpdateAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _pregnancyservice.DeleteAsync(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _pregnancyservice.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _pregnancyservice.GetAsync(id);
            return Ok(item);
        }
    }
}
