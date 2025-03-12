using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using Application.ViewModels.Fetus;
using Application.ViewModels.Blog;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Services.IServices;
using Application.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var accountDto = await _accountService.GetAccountByIdAsync(id);
                if (accountDto == null)
                {
                    return NotFound(ApiResponse<AccountDTO>.FailureResponse("Account not found."));
                }

                return Ok(ApiResponse<AccountDTO>.SuccessResponse(accountDto, "Account retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AccountDTO>.FailureResponse("An error occurred while retrieving the account."));
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> AddAsync(CreateAccountDTO createAccountDto)
        //{
        //    var created = await _accountService.AddAsync(createAccountDto);
        //    return Ok(ApiResponse<CreateAccountDTO>.SuccessResponse(created, "Post created successfully."));
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                return Ok(ApiResponse<IEnumerable<AccountDTO>>.SuccessResponse(accounts, "All accounts retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<AccountDTO>>.FailureResponse("An error occurred while retrieving accounts."));
            }
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaginated([FromQuery] QueryParameters query)
        {
            try
            {
                var pagedResult = await _accountService.GetAllAccountsPaginatedAsync(query);
                return Ok(ApiResponse<PaginatedList<AccountDTO>>.SuccessResponse(
                    pagedResult, $"Accounts retrieved successfully. Page {pagedResult.PageIndex} of {pagedResult.TotalPages}."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PaginatedList<AccountDTO>>.FailureResponse("An error occurred while retrieving paginated accounts."));
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAccountDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<UpdateAccountDTO>.FailureResponse(
                    "Invalid data.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var updatedAccount = await _accountService.UpdateAccountAsync(id, updateDto);
                if (updatedAccount == null)
                {
                    return NotFound(ApiResponse<AccountDTO>.FailureResponse("Account not found or update failed."));
                }

                return Ok(ApiResponse<AccountDTO>.SuccessResponse(updatedAccount, "Account updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AccountDTO>.FailureResponse("An error occurred while updating the account."));
            }
        }

        [HttpPatch("ban/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> BanAccount(int id)
        {
            try
            {
                var success = await _accountService.BanAccountAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<string>.FailureResponse("Account not found or could not be banned."));
                }

                return Ok(ApiResponse<string>.SuccessResponse("Account banned successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while banning the account."));
            }
        }

        [HttpPatch("unban/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UnbanAccount(int id)
        {
            try
            {
                var success = await _accountService.UnbanAccountAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<string>.FailureResponse("Account not found or could not be unbanned."));
                }

                return Ok(ApiResponse<string>.SuccessResponse("Account unbanned successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while unbanning the account."));
            }
        }
    }
}
