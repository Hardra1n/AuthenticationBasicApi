using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Authentication
{
    public static class JwtAuthOptions
    {

        public const string Issuer = "MyAuthServer";
        public const string Audience = "MyAuthClient";
        public const string Key = "SomeSecretKey123_SomeSecretKey123";
        public const int Lifetime = 10;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
