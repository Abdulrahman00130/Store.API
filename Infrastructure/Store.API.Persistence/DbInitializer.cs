using Microsoft.EntityFrameworkCore;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Products;
using Store.API.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Store.API.Persistence
{
    public class DbInitializer(StoreDbContext _context): IDbInitializer
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
    }
}
