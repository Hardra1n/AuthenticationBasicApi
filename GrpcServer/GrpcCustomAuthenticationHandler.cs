using GrpcServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GrpcServer
{
    internal class GrpcCustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private MainServerCaller _httpClient;

        public GrpcCustomAuthenticationHandler(MainServerCaller httpClient, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _httpClient = httpClient;
        }

        public GrpcCustomAuthenticationHandler(MainServerCaller httpClient, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
            _httpClient = httpClient;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.TryGetValue("Authorization", out var value))
            {
                var headerValue = value.ToString();
                var isAuthenticated = await _httpClient.CheckAuthentication(headerValue);
                if (isAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, "DefaultUser") };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }

            return AuthenticateResult.Fail("Not authenticated on main server.");
        }
    }
}
