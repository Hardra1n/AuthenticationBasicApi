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
                    FirstName = "First   Name",
                    LastName = "Last Name",
                    Birthday = new DateOnly(1996, 1, 1)
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Michael",
                    LastName = "Clinton",
                    Birthday = new DateOnly(1964, 6, 21)
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Teres",
                    LastName = "Wolton",
                    Birthday = new DateOnly(2006, 8, 29)
                },
            ]);
        }

    }
}
