using ECommerce.Domain.Contracts.Seed;
using ECommerce.Domain.Models.Products;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Seed
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDbContext _context;
        public DataSeeding(StoreDbContext context)
        {
            _context = context;
        }

        public async Task DataSeedAsync()
        {
            // Check if there any migration pending 
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                _context.Database.Migrate(); // Apply any migration pending (Update to Database)

            // Check if there any data inside ProductBrands table.
            if (!_context.ProductBrands.AsNoTracking().Any())
            {
                // ReadAllText => Read the Json file and convert it to string.
                string productBrandData =
                  await  File.ReadAllTextAsync(@"..\InfrastructureLayer\ECommerce.Persistence\Data\brands.json");

                // Deserialize the JSON string into a list of C# ProductBrand objects
                var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandData);

                // Check if the productBrands list contains any elements.
                if (productBrands != null && productBrands.Any())
                {
                    // Add All the productBrands to the database context.
                    // AddRange => Add multiple entities to the DbSet at once.
                    _context.ProductBrands.AddRange(productBrands);
                }
            }

            // Check if there any data inside ProductTypes table.

            if (!_context.ProductTypes.AsNoTracking().Any())
            {
                // ReadAllText => Read the Json file and convert it to string.
                string productTypesData =
                 await File.ReadAllTextAsync(@"..\InfrastructureLayer\ECommerce.Persistence\Data\types.json");

                // Deserialize the JSON string into a list of C# ProductType objects
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);

                // Check if the ProductTypes list contains any elements.
                if (productTypes != null && productTypes.Any())
                {
                    // Add All the ProductTypes to the database context.
                    _context.ProductTypes.AddRange(productTypes);
                }
            }

            // Check if there any data inside Product table.

            if (!_context.Products.AsNoTracking().Any())
            {
                // ReadAllText => Read the Json file and convert it to string.
                string productData =
                    await File.ReadAllTextAsync(@"..\InfrastructureLayer\ECommerce.Persistence\Data\products.json");

                // Deserialize the JSON string into a list of C# Product objects
                var product = JsonSerializer.Deserialize<List<Product>>(productData);

                // Check if the product list contains any elements.
                if (product != null && product.Any())
                {
                    // Add All the products to the database context.
                    _context.Products.AddRange(product);
                }
            }

            _context.SaveChanges(); // Save all changes to the database

        }

    }

}

