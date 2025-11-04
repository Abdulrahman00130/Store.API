using Store.API.Domain.Exceptions.BadRequest;
using Store.API.Domain.Exceptions.NotFound;
using Store.API.Domain.Exceptions.Unauthorized;
using Store.API.Shared.ErrorModels;

namespace Store.API.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            //1. set status code for response
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException=> StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            //2. set content type code for response
            context.Response.ContentType = "application/json";

            //3. response object (body)
            var response = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            };

            //4. return response
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"Endpoint {context.Request.Path} is not found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
