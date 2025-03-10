using Application.IServices;
using Application.Services;
using Application.ViewModels.Pregnancy;
using Application.ViewModels.ScheduleUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleUserController : ControllerBase
    {
        private readonly IScheduleUserService _scheduleUserService;
        public ScheduleUserController(IScheduleUserService service)
        {
            _scheduleUserService = service;
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(ScheduleUserAddVM item)
        {
            await _scheduleUserService.AddAsync(item);
            return Ok(item);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ScheduleUserVM item)
        {
            await _scheduleUserService.UpdateAsync(item);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _scheduleUserService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _scheduleUserService.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _scheduleUserService.GetAsync(id);
            return Ok(item);
        }
    }
}
