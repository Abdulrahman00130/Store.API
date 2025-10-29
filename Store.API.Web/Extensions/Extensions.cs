using Microsoft.AspNetCore.Mvc;
using Store.API.Domain.Contracts;
using Store.API.Persistence;
using Store.API.Services;
using Store.API.Shared.ErrorModels;
using Store.API.Web.Middlewares;


namespace Store.API.Web.Extensions
{
    public static class Extensions
    {
        // Services
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.

            services.AddBuiltInServices();

            services.AddSwaggerServices();

            services.AddInfrastructureService(configuration);
            
            services.AddApplicationServices(configuration);
            
            services.ConfigureServices();

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            config.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                .Select(m => new ValidationError
                                {
                                    Field = m.Key,
                                    Errors = m.Value.Errors.Select(error => error.ErrorMessage)
                                });
                var response = new ValidationErrorResponse
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(response);
            });

            return services;
        }

        // Middlewares
        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            app.InitializeDatabaseAsync();

            app.UseGlobalErrorHandling();

            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var dbInitializer = scoped.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }
    }
}
