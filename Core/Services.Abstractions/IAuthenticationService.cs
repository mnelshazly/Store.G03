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

        // Get Current User
        // Takes: Email
        // Returns: Token, Email & Display Name
        Task<UserDto> GetCurrentUserAsync(string email);

        // Check Email
        // Takes: Email
        // Returns: Boolean
        Task<bool> CheckEmailAsync(string email);

        // Get Current User Address
        // Takes: Email
        // Returns: Address
        Task<AddressDto> GetCurrentUserAddressAsync(string email);

        // Update Current User Address
        // Takes: Email and Updated Address
        // Returns: Address After Update
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email,  AddressDto addressDto);


    }
}
