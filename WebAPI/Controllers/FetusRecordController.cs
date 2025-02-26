using Application.IServices;
using Application.ViewModels.FetusRecord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FetusRecordController : ControllerBase
    {
        private readonly IFetusRecordService _fetusRecordService;

        public FetusRecordController(IFetusRecordService fetusRecordService)
        {
            _fetusRecordService = fetusRecordService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFetusRecord(FetusRecordAddVM fetusRecordAddVM)
        {
            await _fetusRecordService.AddAsync(fetusRecordAddVM);
            return Ok("Fetus Record Added");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(FetusRecordVM fetusRecordVM)
        {
            await _fetusRecordService.UpdateAsync(fetusRecordVM);
            return Ok("Fetus Record Updated");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var fetusRecord = await _fetusRecordService.GetByIdAsync(id);
            if (fetusRecord == null)
            {
                return NotFound();
            }
            return Ok(fetusRecord);
        }

        [HttpDelete]
        public async void DeleteAsync(int id)
        {
            _fetusRecordService.DeleteAsync(id);
            Ok("Fetus Record Delete");
        }

        [HttpDelete("soft")]
        public async void SoftDelete(int id)
        {
            _fetusRecordService.SoftDelete(id);
            Ok("Soft");
        }
    }
}
