using AutoMapper;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Baskets;
using Store.API.Domain.Exceptions.BadRequest;
using Store.API.Services.Abstractions.Baskets;
using Store.API.Shared.DTOs.Baskets;
using Store.API.Domain.Exceptions.NotFound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Baskets
{
    public class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundException(id);

            var dto = _mapper.Map<BasketDto>(basket);
            return dto;
        }

        public async Task<BasketDto?> CreateBasketAsync(BasketDto dto, TimeSpan duration)
        {
            var basket = _mapper.Map<CustomerBasket>(dto);
            var result = await _basketRepository.CreateBasketAsync(basket, duration);

            if (result is null) throw new CreateOrUpdateBasketBadRequestException();

            return _mapper.Map<BasketDto>(result);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var deleted = await _basketRepository.DeleteBasketAsync(id);

            if (!deleted) throw new DeleteBasketBadRequestException();

            return deleted;
        }

    }
}
