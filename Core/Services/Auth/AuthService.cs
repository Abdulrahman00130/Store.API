using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class AuthService(UserManager<AppUser> _userManager, IOptions<JwtOptions> _options, IMapper _mapper) : IAuthService
    {
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<AddressDto?> GetCurrentUserAddressAsync(string email)
        {
            var user = _userManager.Users.Include(u => u.Address).FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if(user is null) throw new UserNotFoundException(email);

            return _mapper.Map<AddressDto?>(user.Address);
        }

        public async Task<UserResponse?> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);

            return new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user),
            };
        }

        public async Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto request, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);
            if (user.Address is null)
            {
                var address = _mapper.Map<Address>(request);
                user.Address = address;
            }
            else
            {
                user.Address.FirstName = request.FirstName;
                user.Address.LastName = request.LastName;
                user.Address.Street = request.Street;
                user.Address.City = request.City;
                user.Address.Country = request.Country;
            }

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded) 
            {
                StringBuilder errors = new StringBuilder();
                foreach (var error in result.Errors)
                    errors.Append($", {error.Description}");
                throw new Exception(errors.ToString());
            }

            return _mapper.Map<AddressDto>(user.Address);
        }

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
