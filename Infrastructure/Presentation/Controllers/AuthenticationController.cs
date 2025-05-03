using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DataTransferObjects.IdentityModuleDtos;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Login
        [HttpPost("Login")] // POST: BaseUrl/api/Authentication/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(User);
        }

        // Register
        [HttpPost("Register")] // POST: BaseUrl/api/Authentication/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(User);
        }

        // Check Email
        [HttpGet("CheckEmail")] // GET: BaseUrl/api/Authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(result);
        }

        // Get Current User
        [Authorize]
        [HttpGet("CurrentUser")] // GET: BaseUrl/api/Authentication/CurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetCurrentUserAsync(email);
            return Ok(user);
        }

        // Get Current User Address
        [Authorize]
        [HttpGet("Address")] // GET: BaseUrl/api/Authentication/Address
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.AuthenticationService.GetCurrentUserAddressAsync(email);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("Address")] // PUT: BaseUrl/api/Authentication/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await _serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(email, addressDto);
            return Ok(updatedAddress);
        }

    }
}
