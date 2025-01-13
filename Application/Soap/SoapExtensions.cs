using Domain;

namespace Application.Soap
{
    public static class SoapExtensions
    {

        public static IdUserSoap ToIdUserSoap(this User user)
        {
            return new IdUserSoap()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public static User ToDomainUser(this UserSoap user)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static User ToDomainUser(this IdUserSoap user)
        {
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
