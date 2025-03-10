using Application.IServices;
using Application.ViewModels.ScheduleUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleUserController : ControllerBase
    {
        IScheduleUserService _scheduleUserService;
        public ScheduleUserController(IScheduleUserService service)
        {
            _scheduleUserService = service;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync(ScheduleUserAddVM item)
        {
            await _scheduleUserService.AddAsync(item);
            return Ok(item);
        }
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync(ScheduleUserVM item)
        {
            await _scheduleUserService.UpdateAsync(item);

            return Ok();
        }
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _scheduleUserService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _scheduleUserService.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _scheduleUserService.GetAsync(id);
            return Ok(item);
        }
    }
}
