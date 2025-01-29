using Domain;

namespace Application.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        public IdUserGraphQL UserAdded([EventMessage] User user) => user.ToGraphQL();
    }
}
