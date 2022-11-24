namespace AppointmentService.API.Middleware;

public class HeadersMiddleware
{
    private readonly RequestDelegate _next;

    public HeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
        await _next(context);
    }
}

public static class HeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseHeadersMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HeadersMiddleware>();
    }
}