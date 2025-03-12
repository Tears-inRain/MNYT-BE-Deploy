using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.ViewModels.Authentication;
using Domain.Entities;
using Application.Authentication.Interface;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountRegistrationDTO registrationDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid registration data.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            var result = await _authenticationService.RegisterAsync(registrationDto);
            if (!result.Success)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(result.Message));
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result.Message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountLoginDTO loginDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation errors occurred.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            var result = await _authenticationService.LoginAsync(loginDto);
            if (!result.Success)
            {
                return Unauthorized(ApiResponse<string>.FailureResponse(result.Message));
            }
            var loginResponse = new LoginResponseDTO
            {
                Success = true,
                Message = "Login successful.",
                JWTToken = result.JWTToken,
                Id = result.Id,
                UserName = result.UserName,
                FullName = result.FullName,
                Email = result.Email,
                Role = result.Role,
                Status = result.Status,
                IsExternal = result.IsExternal
            };

            return Ok(ApiResponse<LoginResponseDTO>.SuccessResponse(loginResponse, "Login successful."));
        }
    }
}
