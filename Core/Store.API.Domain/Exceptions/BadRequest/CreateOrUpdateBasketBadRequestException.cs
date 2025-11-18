using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.BadRequest
{
    public class CreateOrUpdateBasketBadRequestException(): BadRequestException("Invalid Operation When Creating or Updating Basket !")
    {
    }
}
