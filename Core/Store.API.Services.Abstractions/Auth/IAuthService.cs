using Store.API.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions.Auth
{
    public interface IAuthService
    {
        Task<UserResponse?> LoginAsync(LoginRequest request);
        Task<UserResponse?> RegisterAsync(RegisterRequest request);
        Task<UserResponse?> GetCurrentUserAsync(string email);
        Task<bool> CheckEmailExistsAsync(string email); 
        Task<AddressDto?> GetCurrentUserAddressAsync(string email); 
        Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto request, string email); 
    }
}
