using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)   // RequestDelegate return task that represent completion of request
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);                             // if any exception occurs then we log exception in catch block
            }catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                //Log this Exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return a custom error response
                httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType="application/json";


                // for returning response back we create new custom error model 
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong!"
                };

                await httpContext.Response.WriteAsJsonAsync(error);  // automatic error model into json
            }

        }
    }
}
