using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.G03.Api.CustomMiddleWare
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong");

                // Set Status Code For Response

                //context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                // Set Content Type For Response
                //context.Response.ContentType = "application/json"; // No need to make this step when using WriteAsJsonAsync 

                // Response Object
                var response = new ErrorToReturn()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                // Return Object As JSON
                //var responseToReturn = JsonSerializer.Serialize(response);
                //await context.Response.WriteAsync(responseToReturn);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
