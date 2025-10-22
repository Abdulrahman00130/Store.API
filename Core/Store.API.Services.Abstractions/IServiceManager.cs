using Store.API.Services.Abstractions.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
    }
}
