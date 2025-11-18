using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Services.Abstractions;
using Store.API.Shared.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var orderResponse = await _serviceManager.OrderService.CreateOrderAsync(request, userEmail.Value);

            return Ok(orderResponse);
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdForSpecificUser(Guid orderId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.OrderService.GetOrderByIdForSpecificUserAsync(orderId, userEmail.Value);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.OrderService.GetOrdersForSpecificUserAsync(userEmail.Value);

            return Ok(result);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var result = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();

            return Ok(result);
        }
    }
}
