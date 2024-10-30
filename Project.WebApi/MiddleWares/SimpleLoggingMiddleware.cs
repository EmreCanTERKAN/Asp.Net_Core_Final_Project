namespace Project.WebApi.MiddleWares
{
    public class SimpleLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public SimpleLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post && context.Request.Path.StartsWithSegments("/api/Orders"))
            {
                Console.WriteLine("Yeni sipariş verildi!");
            }

            await _next(context);
        }
    }
}
