using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Application.Authentication
{
    public static class AuthenticationExtensions
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtAuthOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = JwtAuthOptions.Audience,
                        
                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = JwtAuthOptions.GetSymmetricSecurityKey()
                    };
                })
                .AddBasicAuthentication();
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder authBuilder)
        {
            return authBuilder.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                BasicAuthenticationDefaults.AuthenticationScheme, options => { });
        }
    }
}

