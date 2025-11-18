using Store.API.Services.Abstractions.Auth;
using Store.API.Services.Abstractions.Baskets;
using Store.API.Services.Abstractions.Cache;
using Store.API.Services.Abstractions.Orders;
using Store.API.Services.Abstractions.Payments;
using Store.API.Services.Abstractions.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        ICacheService CacheService { get; }
        IAuthService AuthService { get; }
        IOrderService OrderService { get; }
        IPaymentService PaymentService { get; }
    }
}
