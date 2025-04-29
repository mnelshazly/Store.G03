using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects.IdentityModuleDtos;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        // Login
        // Takes: Email & Password
        // Returns: Token, Email & DisplayName
        Task<UserDto> LoginAsync(LoginDto loginDto);

        // Regiseter
        // Takes: Email, Password, UserName, DisplayName & PhoneNumber
        // Returns: Token, Email & DisplayName
        Task<UserDto> RegisterAsync(RegisterDto registerDto);


    }
}
