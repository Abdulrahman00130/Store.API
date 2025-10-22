
using Microsoft.EntityFrameworkCore;
using Store.API.Domain.Contracts;
using Store.API.Persistence;
using Store.API.Persistence.Data.Contexts;
using Store.API.Presentation;
using Store.API.Services;
using Store.API.Services.Abstractions;
using Store.API.Services.Mapping.Products;

namespace Store.API.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(o => o.AddProfile(new ProductProfile(builder.Configuration)));

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            var app = builder.Build();

            using var scoped = app.Services.CreateScope();
            var dbInitializer = scoped.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();

            // Configure the HTTP request pipeline.

            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
