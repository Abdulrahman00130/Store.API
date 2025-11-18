using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Exceptions.NotFound
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
    }
}
