using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OegFlow.Domain.Models;
using OrgFlow.Application.Interfaces;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             IUserRepository userRepository,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterRequest request, string role)
        {
            var existing = await _userManager.FindByNameAsync(request.UserName);
            if (existing != null)
                return (false, "User already exists");
            var userInOrg = await _userRepository.GetUserBaseDataByEmailAsync(request.Email);
            if (userInOrg == null)
                return (false, "User it is not part of any organization");

            var user = new ApplicationUser
            {
                UserId = userInOrg.Id,
                OrganizationId = userInOrg.Department.OrganizationId,
                DepartmentId = (int)userInOrg.DepartmentId,
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.PasswordHash);

            if (!result.Succeeded)
                return (false, "User creation failed");

            if (!await _roleManager.RoleExistsAsync(userInOrg.Position.Name))
                await _roleManager.CreateAsync(new IdentityRole(userInOrg.Position.Name));

            await _userManager.AddToRoleAsync(user, userInOrg.Position.Name);

            return (true, "User created successfully");
        }

        public async Task<(bool IsSuccess, string Token)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return (false, "Invalid username");

            //if (!await _userManager.CheckPasswordAsync(user, request.Password))
            //    return (false, "Invalid password");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("OrganizationId", user.OrganizationId.ToString()),
                new Claim("DepartmentId", user.DepartmentId.ToString())
            };

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            return (true, GenerateJwt(claims));
        }

        private string GenerateJwt(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

