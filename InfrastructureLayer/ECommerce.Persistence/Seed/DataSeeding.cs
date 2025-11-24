using ECommerce.Domain.Contracts.Seed;
using ECommerce.Domain.Models.Products;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly StoreIdentityDbContext _storeIdentityDbContext;
        public DataSeeding(StoreDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, StoreIdentityDbContext storeIdentityDbContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _storeIdentityDbContext = storeIdentityDbContext;
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

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User1 = new ApplicationUser
                    {
                        Email = "Ahmed@gmail.com",
                        DisplayName = "Ahmed Samir",
                        PhoneNumber = "01019412738",
                        UserName = "ahmedrashed"
                    };

                    var User2 = new ApplicationUser
                    {
                        Email = "Ali@gmail.com",
                        DisplayName = "Ali Yaseer",
                        PhoneNumber = "011481782974",
                        UserName = "aliyasser"
                    };

                    var User3 = new ApplicationUser
                    {
                        Email = "Yassmine@gmail.com",
                        DisplayName = "Yassmine Mohammed",
                        PhoneNumber = "012298394857",
                        UserName = "yasminmohammed"
                    };

                    await _userManager.CreateAsync(User1, "Ahmed@Samir0");
                    await _userManager.CreateAsync(User2, "Ali@Yasser0");
                    await _userManager.CreateAsync(User3, "Yassmine@Mohammed0");

                    await _userManager.AddToRoleAsync(User1, "Admin");
                    await _userManager.AddToRoleAsync(User2, "Admin");
                    await _userManager.AddToRoleAsync(User3, "SuperAdmin");
                }

                await _storeIdentityDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}

