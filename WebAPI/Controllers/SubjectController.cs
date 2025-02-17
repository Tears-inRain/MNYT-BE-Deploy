using Application.IServices;
using Application.ViewModels.Subject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("all-subjects")]
        public async Task<IActionResult> GetAllSubject()
        {
            var subjectList = await _subjectService.GetAllAsync();

            if (subjectList.IsNullOrEmpty()) 
            {
                return BadRequest("List is empty");
            }

            return Ok(subjectList);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectAddVM item)
        {
            await _subjectService.AddAsync(item);

            return Ok("Add subject success");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            if (subject == null)
            {
                return NotFound("Item not found");
            }

            return Ok(subject);
        }
    }
}
