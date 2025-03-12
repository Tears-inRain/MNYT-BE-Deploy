using Application.Services.IServices;
using Application.ViewModels.Pregnancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PregnancyController : ControllerBase
    {
        private readonly IPregnancyService _pregnancyservice;

        public PregnancyController(IPregnancyService service)
        {
            _pregnancyservice = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync(PregnancyAddVM item)
        {
            await _pregnancyservice.AddSync(item);
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync(PregnancyVM item)
        {
            await _pregnancyservice.UpdateAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _pregnancyservice.DeleteAsync(id);
            return Ok();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _pregnancyservice.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _pregnancyservice.GetAsync(id);
            return Ok(item);
        }
    }
}
