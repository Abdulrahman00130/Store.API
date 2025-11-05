using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.API.Domain.Entities.Identity;
using Store.API.Domain.Exceptions.BadRequest;
using Store.API.Domain.Exceptions.NotFound;
using Store.API.Domain.Exceptions.Unauthorized;
using Store.API.Services.Abstractions.Auth;
using Store.API.Shared;
using Store.API.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager, IOptions<JwtOptions> _options) : IAuthService
    {
        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null) throw new UserNotFoundException(request.Email);

            var flag = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!flag) throw new UnauthorizedException();

            return new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new RegistrationBadRequestException(result.Errors.Select(e => e.Description).ToList());

            return new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            { 
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtOptions = _options.Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.Now.AddDays(jwtOptions.DurationInDays),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
