using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.BadRequest
{
    public class GetDeliveryMethodsBadRequest() : BadRequestException("Invalid operation when retrieving delivery methods")
    {
    }
}
