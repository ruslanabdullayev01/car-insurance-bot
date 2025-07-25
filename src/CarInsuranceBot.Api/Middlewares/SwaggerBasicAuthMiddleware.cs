using System.Text;

namespace Web.API.Middlewares
{
    public class SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly string _username = configuration["SwaggerAuth:Username"] ?? string.Empty;
        private readonly string _password = configuration["SwaggerAuth:Password"] ?? string.Empty;

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                var authHeader = context.Request.Headers.Authorization.ToString();
                if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader["Basic ".Length..].Trim();
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    if (username == _username && password == _password)
                    {
                        await _next(context);
                        return;
                    }
                }

                context.Response.Headers.WWWAuthenticate = "Basic";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }

}
