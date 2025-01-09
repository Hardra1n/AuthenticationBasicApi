using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Application.Authentication;

public class CustomAuthMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string authorizationHeader = "Authorization";
        AuthenticateResult? result = null;
        if (context.Request.Headers.ContainsKey(authorizationHeader))
        {
            var authHeader = context.Request.Headers[authorizationHeader].ToString();
            if (authHeader.StartsWith("Bearer"))
            {
                result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            }
            else if (authHeader.StartsWith("Basic"))
            {
                result = await context.AuthenticateAsync(BasicAuthenticationDefaults.AuthenticationScheme);
            }
        }


        if (result is { Succeeded: true })
        {
            context.User = result.Principal;
        }

        await _next.Invoke(context);
    }
}