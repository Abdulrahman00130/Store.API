using Store.API.Shared.DTOs.Baskets;
using Store.API.Shared.DTOs.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<BasketDto> CreatePaymentIntentAsync(string basketId);
        Task<OrderResponse> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);
    }
}
