using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.NotFound
{
    public class OrderByPaymentIntentIdNotFoundException(string paymentIntentId) : NotFoundException($"Order with payment intent id of {paymentIntentId} was not found !")
    {
    }
}
