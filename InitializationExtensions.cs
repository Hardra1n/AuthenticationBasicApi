using Application.Authentication;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Application
{
    public static class InitializationExtensions
    {
        public static void ConfigureInitialData(this WebApplication app)
        {
            var userSource = app.Services.GetService<UserSource>();

            userSource?.AddRange([
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName1",
                    LastName = "LastName1"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName2",
                    LastName = "LastName2"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName3",
                    LastName = "LastName3"
                },
            ]);
        }

        public static void ConfigureJwtAuthentication(this WebApplicationBuilder builder)
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
                });
        }
    }

}
