using Store.API.Shared.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail);
        Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodsAsync();
        Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid orderId, string userEmail);
        Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string userEmail);
    }
}
