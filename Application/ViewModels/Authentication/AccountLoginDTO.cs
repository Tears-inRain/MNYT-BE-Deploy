using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Authentication
{
    public class AccountLoginDTO
    {
        [Required]
        public string EmailOrUsername { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
