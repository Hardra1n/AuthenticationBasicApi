using System.Buffers.Text;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Application.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const char Separator = ':';
        private readonly SystemUserSource _userSource;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, SystemUserSource userSource) : base(options, logger, encoder, clock)
        {
            _userSource = userSource;
        }

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, SystemUserSource userSource) : base(options, logger, encoder)
        {
            _userSource = userSource;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authStr = Request.Headers["Authorization"].ToString();
            string basicStartingStr = "Basic ";

            if (authStr.StartsWith(basicStartingStr))
            {
                var base64Str = authStr.Substring(basicStartingStr.Length);
                if (!Base64.IsValid(base64Str))
                {
                    return GetFailedResult();
                }

                var userCredentialsStr 
                    = Encoding.Default.GetString(Convert.FromBase64String(base64Str));
                if (CheckUserCredentialsStr(userCredentialsStr))
                {
                    return GetFailedResult();
                }

                var (username, password) = TransformUserCredentials(userCredentialsStr);
                if (!_userSource.IsValidUser(username, password))
                {
                    return GetFailedResult();
                }

                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return GetFailedResult();
        }

        private bool CheckUserCredentialsStr(string userCredentials)
        {
            return userCredentials.Length < $"1{Separator}1".Length ||
                   !userCredentials.Contains(Separator) ||
                   userCredentials.IndexOf(Separator) == 0 ||
                   userCredentials.IndexOf(Separator) == userCredentials.Length - 1;
        }

        private (string, string) TransformUserCredentials(string userCredentials)
        {
            var separatorIndex = userCredentials.IndexOf(Separator);
            if (separatorIndex > 0 && separatorIndex < userCredentials.Length - 1)
            {
                var username = userCredentials[..separatorIndex];
                var password = userCredentials[(separatorIndex + 1)..];
                return (username, password);
            }

            return (string.Empty, string.Empty);
        }


        private Task<AuthenticateResult> GetFailedResult() => Task.FromResult(AuthenticateResult.Fail("Invalid auth in basic authentication"));
    }
}
