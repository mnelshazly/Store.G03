using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
