using Common.Dtos;
using Domain;

namespace Application.GraphQL
{
    public static class GraphQLExtensions
    {
        public static IdUserGraphQL ToGraphQL(this User user)
        {
            return new IdUserGraphQL()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }

        public static User ToDomainUser(this IdUserGraphQL user)
        {
            return new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }

        public static User ToDomainUser(this UserGraphQL user)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }
    }
}
