using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.Dtos
{
    public class UserIdDto : UserDto
    {
        public Guid Id { get; set; }
    }
}
