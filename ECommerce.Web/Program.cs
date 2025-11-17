
using ECommerce.Domain.Contracts.Seed;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

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

            var app = builder.Build();

            // Seed the database
            var scope = app.Services.CreateScope();
            // Get the IDataSeeding service from the service provider
            var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            // Call the DataSeed method to seed the database
            dataSeeding.DataSeed();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
