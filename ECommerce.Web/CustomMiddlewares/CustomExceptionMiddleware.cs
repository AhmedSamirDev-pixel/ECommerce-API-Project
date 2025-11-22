using ECommerce.Domain.Exceptions;
using ECommerce.Shared.ErrorModels;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;

namespace ECommerce.Web.CustomMiddlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate requestDelegate, ILogger<CustomExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate.Invoke(context);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                #region Response Header
                // Set Status Code for response
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                // Set Content type for response
                context.Response.ContentType = "Application/Json";
                #endregion

                #region Response Body
                // Response Object
                var response = new ErrorToReturn
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                // Return object as json
                await context.Response.WriteAsJsonAsync(response);
                #endregion
            }
        }
    }
}
