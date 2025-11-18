using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.API.Services.Abstractions;
using Store.API.Services.Mapping.Auth;
using Store.API.Services.Mapping.Baskets;
using Store.API.Services.Mapping.Orders;
using Store.API.Services.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(o => o.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(o => o.AddProfile(new BasketProfile()));
            services.AddAutoMapper(o => o.AddProfile(new OrderProfile()));
            services.AddAutoMapper(o => o.AddProfile(new AuthProfile()));

            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}
