using Application.Services.IServices;
using Application.ViewModels.ScheduleTemplate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleTemplateController : ControllerBase
    {
        private readonly IScheduleTemplateService _scheduleTemplateService;
        public ScheduleTemplateController(IScheduleTemplateService scheduleTemplateService)
        {
            _scheduleTemplateService = scheduleTemplateService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(ScheduleTemplateAddVM scheduleTemplateAddVM)
        {
            await _scheduleTemplateService.AddAsync(scheduleTemplateAddVM);
            return Ok(scheduleTemplateAddVM);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ScheduleTemplateVM scheduleTemplateVM)
        {
            await _scheduleTemplateService.UpdateAsync(scheduleTemplateVM);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _scheduleTemplateService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _scheduleTemplateService.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var item = await _scheduleTemplateService.GetAsync(id);
            return Ok(item);
        }
    }
}
