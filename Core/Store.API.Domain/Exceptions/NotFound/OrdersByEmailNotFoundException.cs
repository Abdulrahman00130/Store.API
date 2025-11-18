using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.NotFound
{
    public class OrdersByEmailNotFoundException(string email) : NotFoundException($"Orders for user with email {email} was not found!")
    {
    }
}
