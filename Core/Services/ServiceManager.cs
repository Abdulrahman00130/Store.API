using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Identity;
using Store.API.Services.Abstractions;
using Store.API.Services.Abstractions.Auth;
using Store.API.Services.Abstractions.Baskets;
using Store.API.Services.Abstractions.Cache;
using Store.API.Services.Abstractions.Products;
using Store.API.Services.Auth;
using Store.API.Services.Baskets;
using Store.API.Services.Cache;
using Store.API.Services.Products;
using Store.API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork, 
                                IMapper _mapper,
                                IBasketRepository _basketRepository,
                                ICacheRepository _cacheRepository,
                                UserManager<AppUser> _userManager,
                                IOptions<JwtOptions> _options) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork, _mapper);
        public IBasketService BasketService { get; } = new BasketService(_basketRepository, _mapper);
        public ICacheService CacheService { get; } = new CacheService(_cacheRepository);
        public IAuthService AuthService { get; } = new AuthService(_userManager, _options);
    }
}
