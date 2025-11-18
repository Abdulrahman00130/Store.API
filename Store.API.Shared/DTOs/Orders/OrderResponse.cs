using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Shared.DTOs.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; } // DeliveryMethod Name
        public ICollection<OrderItemDto> Items { get; set; }
        public decimal SubTotal { get; set; } // Price * Quantity
        public decimal Total { get; set; } // SubTotal + Delivery Method Fee
    }
}
