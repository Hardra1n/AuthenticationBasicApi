using Application.Services;
using HotChocolate.Authorization;

namespace Application.GraphQL
{
    public class Mutation
    {
        private readonly UserService _service;

        public Mutation(UserService service)
        {
            _service = service;
        }

        public bool UpdateUser(IdUserGraphQL user)
        {
            _service.Update(user.ToDomainUser());
            return true;
        }

        public bool DeleteUser(Guid id)
        {
            _service.Delete(id);
            return true;
        }

        [Authorize]
        public bool AddUser(UserGraphQL user) 
        {
            _service.Add(user.ToDomainUser());
            return true;
        }
    }
}
