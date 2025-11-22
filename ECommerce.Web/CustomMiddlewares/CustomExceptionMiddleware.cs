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
                // Move to the next middleware in the pipeline
                await _requestDelegate.Invoke(context);

                // Handle cases where no endpoint matches the request (404 Not Found)
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    #region Response Body
                    // Build a custom 404 error response
                    var response = new ErrorToReturn
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"EndPoint {context.Request.Path} is Not Found"
                    };

                    // Return the response in JSON format
                    await context.Response.WriteAsJsonAsync(response);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                // Log the exception with details
                _logger.LogError(ex, ex.Message);

                #region Response Header
                // Determine the appropriate status code based on the exception type
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound, // Custom domain exception
                    _ => StatusCodes.Status500InternalServerError        // Default to server error
                };

                // Specify that the response will be in JSON format
                context.Response.ContentType = "Application/Json";
                #endregion

                #region Response Body
                // Build the error response object
                var response = new ErrorToReturn
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                // Return the error object as JSON
                await context.Response.WriteAsJsonAsync(response);
                #endregion
            }
        }
    }
}
