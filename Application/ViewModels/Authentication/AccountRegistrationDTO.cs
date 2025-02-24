using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Authentication
{
    public class AccountRegistrationDTO
    {
        //[Required]
        //[StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        public string? FullName { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required]
        public string Role { get; set; } = "Member";

        public bool IsExternal { get; set; } = false;
        public string? ExternalProvider { get; set; }
    }
}
