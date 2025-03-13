
namespace Application.ViewModels.Authentication
{
    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string JWTToken { get; set; } = string.Empty;
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public bool IsExternal { get; set; }
        public string? ExternalProvider { get; set; }
    }
}
