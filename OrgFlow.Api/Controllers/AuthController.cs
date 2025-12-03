using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OegFlow.Domain.DTOs;
using OegFlow.Domain.Models;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var (success, value) = await _authService.LoginAsync(request);
            if (!success)
                return BadRequest(value);

            return Ok(value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var (success, message) = await _authService.RegisterAsync(request, "Admin");
            if (!success)
                return BadRequest(message);

            return Ok(message);
        }
    }
}

