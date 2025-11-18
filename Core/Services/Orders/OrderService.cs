using AutoMapper;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Orders;
using Store.API.Domain.Entities.Products;
using Store.API.Domain.Exceptions.BadRequest;
using Store.API.Domain.Exceptions.NotFound;
using Store.API.Services.Abstractions.Orders;
using Store.API.Services.Specifications.Orders;
using Store.API.Shared.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            // 1. convert OrderAddressDto to OrderAddress
            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            // 2. Get DeliveryMethod using its id
            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            // 3. Get Order Items from the basket
            // 3.1 Get Basket
            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if(basket is null) throw new BasketNotFoundException(request.BasketId);

            // 3.2 Fill order items collection from items in the basket after converting each (basket item -> order item)
            var orderItems = new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);

                if (product.Price != item.Price) item.Price = product.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            // 4. calculate subtotal
            var subtotal = orderItems.Sum(oi => oi.Price * oi.Quantity);

            // check if an order exists with the same paymentIntent (is so -> delete it, create new updated one)
            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existOrder = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);

            if(existOrder is not null)
                _unitOfWork.GetRepository<Guid, Order>().Delete(existOrder);

            // create the order instance
            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subtotal, basket.PaymentIntentId);

            // add the instance in database
            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();

            // check if successful
            if (count <= 0) throw new CreateOrderBadRequestException();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            if (deliveryMethods is null) throw new GetDeliveryMethodsBadRequest();

            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid orderId, string userEmail)
        {
            var spec = new OrderSpecification(orderId, userEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);

            if(order is null) throw new OrderNotFoundException(orderId);

            return _mapper.Map<OrderResponse?>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string userEmail)
        {
            var spec = new OrderSpecification(userEmail);
            var orders = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            if(orders is null) throw new OrdersByEmailNotFoundException(userEmail);

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }
    }
}
