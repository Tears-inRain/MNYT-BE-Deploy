using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(int userId, string userName, string roles, string accountStatus);
    }
}
