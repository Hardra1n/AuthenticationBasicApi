using Application.Authentication;
using Domain;
using Microsoft.AspNetCore.Authentication;
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

    }
}
