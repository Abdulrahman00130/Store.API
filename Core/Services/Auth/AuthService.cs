using Microsoft.AspNetCore.Identity;
using Store.API.Domain.Entities.Identity;
using Store.API.Domain.Exceptions.BadRequest;
using Store.API.Domain.Exceptions.NotFound;
using Store.API.Domain.Exceptions.Unauthorized;
using Store.API.Services.Abstractions.Auth;
using Store.API.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager) : IAuthService
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
                Token = "To Do"
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
                Token = "To Do"
            };
        }
    }
}
