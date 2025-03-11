using Application.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Interface
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDTO> RegisterAsync(AccountRegistrationDTO registrationDto);
        Task<LoginResponseDTO> LoginAsync(AccountLoginDTO loginDto);
    }
}
