
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Contracts.Seed;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Persistence.BasketRepo;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Identity.Models;
using ECommerce.Persistence.Seed;
using ECommerce.Persistence.UnitOfWork;
using ECommerce.Services.BusinessServices;
using ECommerce.Services.MappingProfile;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.ErrorModels;
using ECommerce.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StackExchange.Redis;

namespace ECommerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region Databases Connections
            // Configure the DbContext with SQL Server provider
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Register the Redis connection multiplexer as a singleton
            // Singleton is used because we only need one Redis connection throughout the application
            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                // Connect to Redis using the connection string defined in appsettings.json
                // "RedisConnection": "localhost"
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });

            #endregion


            #region Business Services
            // Register the DataSeeding service
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            // Register the UnitOfWork service
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register the ServiceManager service
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            // Register the BasketRepository as a scoped service
            // This means a new instance will be created per HTTP request
            builder.Services.AddScoped<IBasketRepository, BasketRepository>(); 
            #endregion
            

            // Configure Custom Validation Error Response
            builder.Services.Configure<ApiBehaviorOptions>((options) =>
            {
                // Override the default behavior when model validation fails
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    // Extract validation errors from the ModelState dictionary
                    var errors = context.ModelState
                        .Where(model => model.Value.Errors.Any()) // Only fields with errors
                        .Select(model => new ValidationError()
                        {
                            Field = model.Key, // The name of the invalid property
                            Errors = model.Value.Errors.Select(error => error.ErrorMessage) // List of validation messages
                        });

                    // Build a structured validation error response
                    var response = new ValidationErrorToReturn()
                    {
                        ValidationErrors = errors // Attach extracted validation errors
                    };

                    // Return response with HTTP 400 BadRequest and the custom error model
                    return new BadRequestObjectResult(response);
                };
            });

            // Configure AutoMapper with the ProjectProfile
            builder.Services.AddAutoMapper(mapper => mapper
                    .AddProfile(new ProjectProfile(builder.Configuration)));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();

            var app = builder.Build();

            // Seed the database
            var scope = app.Services.CreateScope();

            // Get the IDataSeeding service from the service provider
            var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            // Call the DataSeed method to seed the database
            dataSeeding.DataSeedAsync();
            dataSeeding.IdentityDataSeedAsync();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Custom Exception Middleware
            app.UseMiddleware<CustomExceptionMiddleware>();    

            app.UseHttpsRedirection();

            // Enable serving static files
            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}
