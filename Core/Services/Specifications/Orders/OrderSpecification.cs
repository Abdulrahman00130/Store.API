using Store.API.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Specifications.Orders
{
    public class OrderSpecification : BaseSpecifications<Guid, Order>
    {
        public OrderSpecification(Guid id, string userEmail) : base(o => o.Id ==  id && o.UserEmail == userEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }

        public OrderSpecification(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
        }
    }
}
