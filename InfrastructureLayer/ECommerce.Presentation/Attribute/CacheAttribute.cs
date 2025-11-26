using ECommerce.ServicesAbstraction.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Attribute
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Text;

    public class CacheAttribute(int durationInSeconds = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1) Generate a unique cache key from request path + query parameters
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            // 2) Resolve the cache service from the dependency injection container
            ICacheServices cacheServices = context.HttpContext.RequestServices.GetRequiredService<ICacheServices>();

            // 3) Try to get cached response using the generated key
            var cacheValue = await cacheServices.GetAsync(cacheKey);

            // 4) If cached data exists → return it immediately (skip executing the action)
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return; // IMPORTANT: stop execution here
            }

            // 5) If the cache is empty → continue executing the action
            var executedContext = await next.Invoke();

            // 6) After controller action executes, check if response is ObjectResult
            if (executedContext.Result is ObjectResult result)
            {
                // Serialize and store response in Redis for the specified duration
                await cacheServices.SetAsync(
                    cacheKey,
                    result.Value!,                        // <- Cache the actual action result
                    TimeSpan.FromSeconds(durationInSeconds)
                );
            }
        }

        // Helper method to build a unique cache key from request path and sorted query parameters
        private string CreateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            // Add the path (example: /api/products)
            keyBuilder.Append(request.Path + "?");

            // Add query string in a sorted order for consistency
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"{item.Key}={item.Value}&");
            }

            return keyBuilder.ToString();
        }
    }

}
