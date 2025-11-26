using ECommerce.Domain.Contracts.CacheRepo;
using ECommerce.ServicesAbstraction.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    // Cache Services
    public class CacheServices : ICacheServices
    {
        // Dependency Injection
        private readonly ICacheRepository _cacheRepository;
        public CacheServices(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        // Get from Cache
        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacheRepository.GetAsync(cacheKey);
        }


        // Set to Cache
        public Task SetAsync(string key, object cacheValue, TimeSpan timeToLive)
        {
            var cacheValueString = JsonSerializer.Serialize(cacheValue);
            return _cacheRepository.SetAsync(key, cacheValueString, timeToLive);
        }
    }
}
