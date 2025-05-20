using Application.Authentication.Interface;
using Application.Utils.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, IDateTimeProvider dateTimeProvider, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GenerateJwtToken(int accountId, string accountUserName, string role, string? accountStatus)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                _logger.LogWarning("Attempted to generate JWT token with null or empty role.");
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty.");
            }

            var secret = _configuration["Authentication:Jwt:Secret"] ?? _configuration["JWT_SECRET"];
            var issuer = _configuration["Authentication:Jwt:Issuer"] ?? _configuration["JWT_ISSUER"];
            var audience = _configuration["Authentication:Jwt:Audience"] ?? _configuration["JWT_AUDIENCE"];

            _logger.LogInformation("JWT Configuration - Secret: {Secret}, Issuer: {Issuer}, Audience: {Audience}", secret, issuer, audience);
            
            if (string.IsNullOrEmpty(secret))
            {
                _logger.LogError("JWT Secret is not configured.");
                throw new ArgumentNullException(nameof(secret), "JWT Secret is not configured.");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                _logger.LogError("JWT Issuer is not configured.");
                throw new ArgumentNullException(nameof(issuer), "JWT Issuer is not configured.");
            }

            if (string.IsNullOrEmpty(audience))
            {
                _logger.LogError("JWT Audience is not configured.");
                throw new ArgumentNullException(nameof(audience), "JWT Audience is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                new Claim(ClaimTypes.Name, accountUserName),
                new Claim("AccountStatus", accountStatus ?? "Unknown"),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: _dateTimeProvider.UtcNow.AddHours(1), // Token valid for 1 hour
                signingCredentials: creds
            );

            _logger.LogInformation("JWT token generated successfully for UserId: {UserId}", accountId);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
