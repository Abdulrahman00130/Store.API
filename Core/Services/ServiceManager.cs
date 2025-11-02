using AutoMapper;
using Store.API.Domain.Contracts;
using Store.API.Services.Abstractions;
using Store.API.Services.Abstractions.Baskets;
using Store.API.Services.Abstractions.Products;
using Store.API.Services.Baskets;
using Store.API.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork, 
                                IMapper _mapper,
                                IBasketRepository _basketRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork, _mapper);
        public IBasketService BasketService { get; } = new BasketService(_basketRepository, _mapper);
    }
}
