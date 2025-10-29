using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.Domain.Contracts;
using Store.API.Persistence;
using Store.API.Persistence.Data.Contexts;
using Store.API.Presentation;
using Store.API.Services;
using Store.API.Services.Abstractions;
using Store.API.Services.Mapping.Products;
using Store.API.Shared.ErrorModels;
using Store.API.Web.Extensions;
using Store.API.Web.Middlewares;

namespace Store.API.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterAllServices(builder.Configuration);
            

            var app = builder.Build();


            app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
