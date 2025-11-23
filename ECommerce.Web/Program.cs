
using ECommerce.Domain.Contracts.Seed;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Seed;
using ECommerce.Persistence.UnitOfWork;
using ECommerce.Services.BusinessServices;
using ECommerce.Services.MappingProfile;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.ErrorModels;
using ECommerce.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

            // Configure the DbContext with SQL Server provider
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Register the DataSeeding service
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            // Register the UnitOfWork service
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Configure AutoMapper with the ProjectProfile
            builder.Services.AddAutoMapper(mapper => mapper
                    .AddProfile(new ProjectProfile(builder.Configuration)));

            // Register the ServiceManager service
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

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


            var app = builder.Build();

            // Seed the database
            var scope = app.Services.CreateScope();
            // Get the IDataSeeding service from the service provider
            var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            // Call the DataSeed method to seed the database
            dataSeeding.DataSeedAsync();


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
