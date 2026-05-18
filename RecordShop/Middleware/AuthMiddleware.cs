namespace RecordShop.Middleware
{

    public class AuthMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            await next.Invoke(context);
            

        }

    }
}
