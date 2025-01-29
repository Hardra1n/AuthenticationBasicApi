using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.GraphQL
{
    public class IdUserGraphQL : UserGraphQL
    {
        public Guid Id { get; set; }
    }
}
