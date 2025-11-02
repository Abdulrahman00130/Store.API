using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.NotFound
{
    public class ProductNotFoundExceptions(int id) : NotFoundException($"Product with id {id} not found!")
    {
    }
}
