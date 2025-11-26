using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.CacheRepo
{
    public interface ICacheRepository
    {
        // Get
        Task<string?> GetAsync(string cacheKey);    

        // Set
        Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive); 
    }
}
