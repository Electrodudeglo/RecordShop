using System.Diagnostics;

namespace RecordShop.Middleware
{

 
    public class CustomLogger : IMiddleware
    {
        private readonly ILogger<CustomLogger> _logger;

        public CustomLogger(ILogger<CustomLogger> logger)
        {
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await next(context);
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Unhandled exception during request");
                throw;
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation(
                "Request {method} {path} responded {statusCode} in {elapsed}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
                );
            }
        }
    }
}
