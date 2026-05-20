namespace Day2_EntityFrameworkCore.CustomMiddleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // Log Request Information
            Console.WriteLine("----- Request Started -----");

            Console.WriteLine($"Method: {context.Request.Method}");

            Console.WriteLine($"URL: {context.Request.Path}");

            Console.WriteLine($"Time: {DateTime.Now}");

            // Call next middleware
            await _next(context);

            // Log Response Information
            Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");

            Console.WriteLine("----- Request Ended -----");

        }
    }
}
