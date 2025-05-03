using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared.DataTransferObjects.IdentityModuleDtos;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = email,
                Token = await CreateTokenAsync(user)
            };
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(U => U.Address)
                                               .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException(email);

            if(user.Address is not null)
            {
                return _mapper.Map<Address, AddressDto>(user.Address);
            } else
            {
                throw new AddressNotFoundException(user.UserName);
            }
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(U => U.Address)
                                               .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException(email);

            if(user.Address is not null) // update
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.Street = addressDto.Street;
            } 
            else // Add new address
            {
                user.Address = _mapper.Map<AddressDto, Address>(addressDto);
            }

            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            // Check if Email exists
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UserNotFoundException(loginDto.Email);

            // Check Password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (IsPasswordValid)
            {
                // Return UserDto
                return new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await CreateTokenAsync(user)
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
            var user = new ApplicationUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName
            };

            // Create User [Application User]
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserDto() { DisplayName = user.DisplayName, Email = user.Email, Token = await CreateTokenAsync(user) };
            }
            else
            {
                var errors = result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(errors);
            }

            // Return UserDto
            // Throw Bad Request Exception
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer : _configuration.GetSection("JWTOptions")["Issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
