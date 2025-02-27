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
        private readonly IPregnancyService _ipregnancyService;

        public PregnancyController(IPregnancyService service)
        {
            _ipregnancyService = service;
        }

        
        
    }
}
