using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.NotFound
{
    public class BasketNotFoundException(string id) : NotFoundException($"Basket with id {id} was not found")
    {
    }
}
