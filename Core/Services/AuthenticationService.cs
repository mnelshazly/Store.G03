using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.DataTransferObjects.IdentityModuleDtos;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
    {
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            // Check if Email exists
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null) throw new UserNotFoundException(loginDto.Email);

            // Check Password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (IsPasswordValid)
            {
                // Return UserDto
                return new UserDto()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = "TOKEN"
                };

            }
            else
            {
                throw new UnauthorizedException();
            }

        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            // Mapping Register Dto => Application User
            var User = new ApplicationUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName
            };

            // Create User [Application User]
            var result = await _userManager.CreateAsync(User, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserDto() { DisplayName = User.DisplayName, Email = User.Email, Token = "TOKEN" };
            }
            else
            {
                var errors = result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(errors);
            }

            // Return UserDto
            // Throw Bad Request Exception
        }
    }
}
