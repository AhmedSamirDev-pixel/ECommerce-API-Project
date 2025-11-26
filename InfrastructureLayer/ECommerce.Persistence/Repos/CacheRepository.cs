using ECommerce.Domain.Contracts.CacheRepo;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repos
{
    // Cache Repository Implementation
    public class CacheRepository : ICacheRepository
    {
        // Dependencies
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connectionMultiplexer, IDatabase database)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = database;
        }

        // Get
        public async Task<string?> GetAsync(string cacheKey)
        {
            var cacheValue = await _database.StringGetAsync(cacheKey);
            return cacheValue.IsNullOrEmpty ? null : cacheValue.ToString();
        }

        // Set
        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive)
        {
           await _database.StringSetAsync(cacheKey, cacheValue, timeToLive); 
        }
    }
}
