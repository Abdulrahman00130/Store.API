using Store.API.Shared.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions.Baskets
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string id);
        Task<BasketDto?> CreateBasketAsync(BasketDto dto, TimeSpan duration);
        Task<bool> DeleteBasketAsync(string id);
    }
}
