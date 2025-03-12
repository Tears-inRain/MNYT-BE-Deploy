using Application.Services.IServices;
using Application.ViewModels.FetusRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FetusRecordController : ControllerBase
    {
        private readonly IFetusRecordService _fetusRecordService;

        public FetusRecordController(IFetusRecordService fetusRecordService)
        {
            _fetusRecordService = fetusRecordService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddFetusRecord(FetusRecordAddVM fetusRecordAddVM)
        {
            await _fetusRecordService.AddAsync(fetusRecordAddVM);
            return Ok("Fetus Record Added");
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync(FetusRecordVM fetusRecordVM)
        {
            await _fetusRecordService.UpdateAsync(fetusRecordVM);
            return Ok("Fetus Record Updated");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _fetusRecordService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var fetusRecord = await _fetusRecordService.GetAsync(id);
            if (fetusRecord == null)
            {
                return NotFound();
            }
            return Ok(fetusRecord);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task DeleteAsync(int id)
        {
            await _fetusRecordService.DeleteAsync(id);
            Ok("Fetus Record Delete");
        }

        [HttpDelete("soft")]
        [AllowAnonymous]
        public async Task SoftDelete(int id)
        {
            await _fetusRecordService.SoftDelete(id);
            Ok("Soft");
        }
    }
}
