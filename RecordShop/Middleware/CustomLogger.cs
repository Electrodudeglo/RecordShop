namespace RecordShop.Middleware
{
    public class CustomLogger : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);
            Console.WriteLine("logger Working :)");

        }

    }
}
