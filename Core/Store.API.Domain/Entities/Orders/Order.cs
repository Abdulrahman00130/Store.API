using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string? paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // Nav Prop
        public int DeliveryMethodId { get; set; }   // FK
        public ICollection<OrderItem> Items { get; set; }   //Nav Prop
        public decimal SubTotal { get; set; } // Price * Quantity
        
        //[NotMapped]
        //public decimal Total { get; set; } // SubTotal + Delivery Method Fee

        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;  // Not Mapped to anything
        public string? PaymentIntentId { get; set; }

    }
}
