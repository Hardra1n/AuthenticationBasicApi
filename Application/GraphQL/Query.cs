using Application.Services;
using Domain;

namespace Application.GraphQL
{
    public class Query
    {
        private readonly UserService _service;

        public Query(UserService service)
        {
            _service = service;
        }

        public bool Ping()
        {
            return true;
        }

        public IEnumerable<IdUserGraphQL> GetUsers()
        {
            var users = _service.GetAll().Select(user => user.ToGraphQL());
            return users;
        }

        public IdUserGraphQL GetUserById(Guid id)
        {
            var user = _service.GetById(id);
            return user.ToGraphQL();
        }

    }
}