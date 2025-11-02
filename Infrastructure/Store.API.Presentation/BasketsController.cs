using Microsoft.AspNetCore.Mvc;
using Store.API.Services.Abstractions;
using Store.API.Shared.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var basketDto = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basketDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(BasketDto dto)
        {
            var basketDto = await _serviceManager.BasketService.CreateBasketAsync(dto, TimeSpan.FromDays(7));
            return Ok(basketDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketById(string id)
        {
            var deleted = await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
