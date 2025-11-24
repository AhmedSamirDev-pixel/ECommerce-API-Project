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


                // Build the error response object
                var response = new ErrorToReturn
                {
                    Message = ex.Message,
                };

                
                // Determine the appropriate status code based on the exception type
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound, // Custom domain exception
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    BadRequestException badRequestException => GetBadRequestErrors(badRequestException, response),
                    _ => StatusCodes.Status500InternalServerError        // Default to server error
                };

                response.StatusCode = context.Response.StatusCode;  

                // Specify that the response will be in JSON format
                context.Response.ContentType = "Application/Json";

                // Return the error object as JSON
                await context.Response.WriteAsJsonAsync(response);
                

            }
        }
        
        private int GetBadRequestErrors(BadRequestException exception, ErrorToReturn response)
        {
            response.Errors = exception.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
