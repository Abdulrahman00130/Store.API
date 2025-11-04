using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Identity;
using Store.API.Domain.Entities.Products;
using Store.API.Persistence.Data.Contexts;
using Store.API.Persistence.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Store.API.Persistence
{
    public class DbInitializer(StoreDbContext _context,
                               IdentityStoreDbContext _identityContext,
                               UserManager<AppUser> _userManager,
                               RoleManager<IdentityRole> _roleManager): IDbInitializer
    {
        public async Task InitializeAsync()
        {
            // Create Db
            // Update Db
            if(_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Data Seeding (from json files)
            // ProductBrands
            if(!_context.ProductBrands.Any())
            {
                var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.API.Persistence\Data\DataSeeding\brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if(brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                }
            }

            // ProductTypes
            if(!_context.ProductTypes.Any())
            {
                var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.API.Persistence\Data\DataSeeding\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if(types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }

            // Products
            if(!_context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.API.Persistence\Data\DataSeeding\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if(products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task InitializeIdentityAsync()
        {
            // Create Db
            // Update Db
            if (_identityContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _identityContext.Database.MigrateAsync();
            }

            // Data Seeding
            if (!_identityContext.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_identityContext.Users.Any())
            {
                var admin = new AppUser
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "1234567890",
                };

                var superAdmin = new AppUser
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "1234567890",
                };

                await _userManager.CreateAsync(admin, "P@ssw0rd");
                await _userManager.CreateAsync(superAdmin, "P@ssw0rd");

                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }

        }
    }
}
