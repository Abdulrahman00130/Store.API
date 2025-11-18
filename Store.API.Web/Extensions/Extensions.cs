using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Identity;
using Store.API.Persistence;
using Store.API.Persistence.Identity.Contexts;
using Store.API.Services;
using Store.API.Shared;
using Store.API.Shared.ErrorModels;
using Store.API.Web.Middlewares;
using System.Text;


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

            services.AddIdentityServices();

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.AddAuthenticationServices(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            }
            );

            services.ConfigureServices();

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
                };
            });

            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
                options.User.RequireUniqueEmail = true
            ).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityStoreDbContext>();

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

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var dbInitializer = scoped.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }
    }
}
