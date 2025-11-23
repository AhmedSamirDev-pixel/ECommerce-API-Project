using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Models.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.BasketRepo
{
    // Redis implementation of IBasketRepository
    public class BasketRepository : IBasketRepository
    {
        private readonly IConnectionMultiplexer _connection; // Redis connection
        private readonly IDatabase _database; // Redis database instance

        // Constructor injects the Redis connection multiplexer
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _database = _connection.GetDatabase(); // initialize database
        }

        // Create or update a basket in Redis
        public async Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            // Serialize the CustomerBasket object to JSON
            var jsonBasket = JsonSerializer.Serialize(basket);

            // Store the basket in Redis with optional expiration
            var isCreatedOrUpdated = await _database.StringSetAsync(
                basket.Id,
                jsonBasket,
                timeToLive ?? TimeSpan.FromHours(5) // default 5 hours
            );

            // If successfully saved, return the stored basket; otherwise, return null
            return isCreatedOrUpdated ? await GetBasketAsync(basket.Id) : null;
        }

        // Delete a basket from Redis
        public async Task<bool> DeleteBasketAsync(string key)
        {
            // Removes the key from Redis
            return await _database.KeyDeleteAsync(key);
        }

        // Retrieve a basket from Redis by key
        public async Task<CustomerBasket?> GetBasketAsync(string key)
        {
            // Get the serialized basket from Redis
            var basket = await _database.StringGetAsync(key);

            // If key not found, return null
            if (basket.IsNullOrEmpty)
                return null;

            // Deserialize JSON string back to CustomerBasket
            return JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());
        }
    }
}
