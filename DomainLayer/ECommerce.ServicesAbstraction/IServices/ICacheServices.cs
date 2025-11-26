using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface ICacheServices
    {
        Task<string?> GetAsync(string cacheKey);
        Task SetAsync(string key, object cacheValue, TimeSpan timeToLive);
    }
}
