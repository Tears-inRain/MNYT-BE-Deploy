using Application.Exceptions;
using Application.IServices.Authentication;
using Application.ViewModels.Authentication;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            ILogger<AuthenticationService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<LoginResponseDTO> RegisterAsync(AccountRegistrationDTO registrationDto)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(registrationDto.Email) ||
                string.IsNullOrWhiteSpace(registrationDto.UserName) ||
                string.IsNullOrWhiteSpace(registrationDto.Password))
            {
               throw new APIException(HttpStatusCode.BadRequest, "Invalid registration data." );
            }

            // Check if account already exists (by Email or Username)
            var existedAccount = await _unitOfWork.AccountRepo.GetByUsernameOrEmail(registrationDto.Email, registrationDto.UserName);

            if (existedAccount != null)
            {
                _logger.LogWarning("User already exists with the provided email or username.");
                return new LoginResponseDTO { Success = false, Message = "User already exists." };
            }
             <(")
            // Validate role
            var allowedRoles = new List<string> { "Member", "Admin", "Manager" };
            if (string.IsNullOrWhiteSpace(registrationDto.Role) ||
                !allowedRoles.Contains(registrationDto.Role, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogError("Invalid or missing role provided.");
                return new LoginResponseDTO { Success = false, Message = "Invalid or missing role provided." };
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var account = _mapper.Map<Account>(registrationDto);

                    account.Password = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password, workFactor: 12);

                    await _unitOfWork.AccountRepo.AddAsync(account);
                    await _unitOfWork.SaveChangesAsync();

                    // Tạo token
                    var accessToken = _jwtTokenService.GenerateJwtToken(
                        account.Id,
                        account.UserName,
                        account.Role ?? "Customer",
                        account.Status
                    );

                    await transaction.CommitAsync();

                    _logger.LogInformation("User registration successful.");
                    return new LoginResponseDTO
                    {
                        Success = true,
                        Message = "Registration successful.",
                        JWTToken = accessToken,
                        Role = account.Role
                    };
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "Database connection error.");
                    await transaction.RollbackAsync();
                    return new LoginResponseDTO { Success = false, Message = "A database connection error occurred. Please try again later." };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred during registration.");
                    await transaction.RollbackAsync();
                    return new LoginResponseDTO { Success = false, Message = "An unexpected error occurred. Please try again." };
                }
            }
        }

        public async Task<LoginResponseDTO> LoginAsync(AccountLoginDTO loginDto)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(loginDto.EmailOrUsername) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return new LoginResponseDTO { Success = false, Message = "Invalid login data." };
            }

            var emailOrUsername = loginDto.EmailOrUsername.Trim();

            try
            {
                var account = await _unitOfWork.AccountRepo.GetByUsernameOrEmail(emailOrUsername, emailOrUsername);

                if (account == null || !string.Equals(account.Status, "Active", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid credentials or account is banned.");
                    return new LoginResponseDTO { Success = false, Message = "Invalid credentials or account is banned." };
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
                {
                    _logger.LogWarning("Invalid credentials provided.");
                    return new LoginResponseDTO { Success = false, Message = "Invalid credentials." };
                }

                var accessToken = _jwtTokenService.GenerateJwtToken(account.Id, account.UserName, account.Role, account.Status);

                _logger.LogInformation("User login successful for AccountId: {AccountId}", account.Id);

                return new LoginResponseDTO
                {
                    Success = true,
                    JWTToken = accessToken,
                    Message = "Login successful.",
                    Role = account.Role
                };
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Database connection error occurred during login.");
                return new LoginResponseDTO { Success = false, Message = "Login failed due to a database connection issue." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return new LoginResponseDTO { Success = false, Message = "Login failed. Please try again." };
            }
        }
    }
}
