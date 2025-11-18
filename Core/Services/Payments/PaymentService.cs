using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Orders;
using Store.API.Domain.Entities.Products;
using Store.API.Domain.Exceptions.NotFound;
using Store.API.Services.Abstractions.Payments;
using Store.API.Services.Specifications.Orders;
using Store.API.Shared.DTOs.Baskets;
using Store.API.Shared.DTOs.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.API.Domain.Entities.Products.Product;

namespace Store.API.Services.Payments
{
    public class PaymentService(IUnitOfWork _unitOfWork, IBasketRepository _basketRepository, IConfiguration _configuration, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string basketId)
        {
            // calculate amount

            // get basket to reassign each item price from product price
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new BasketNotFoundException(basketId);

            // reassign
            foreach(var basketItem in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(basketItem.Id);
                if (product is null) throw new ProductNotFoundExceptions(basketItem.Id);

                basketItem.Price = product.Price;
            }

            // calculate subtotal
            var subTotal = basket.Items.Sum(i => i.Price *  i.Quantity);

            // Calculate Total amount
            if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
            if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;
            var total = subTotal + deliveryMethod.Price;

            // send amount to stripe
            StripeConfiguration.ApiKey = _configuration["StripeOptions:Secretkey"];

            PaymentIntentService intentService = new PaymentIntentService();
            PaymentIntent paymentIntent = new PaymentIntent();

            if(basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)total * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await intentService.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)total * 100,
                };
                paymentIntent = await intentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await _basketRepository.CreateBasketAsync(basket, TimeSpan.FromDays(1));

            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<OrderResponse> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);

            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);
            if(order is null) throw new OrderByPaymentIntentIdNotFoundException(paymentIntentId);

            if(flag)
            {
                order.Status = OrderStatus.PaymentSuccess;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            
            _unitOfWork.GetRepository<Guid, Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponse>(order);
        }
    }
}
