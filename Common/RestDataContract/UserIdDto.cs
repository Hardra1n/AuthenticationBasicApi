using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Common.Dtos
{
    public class UserIdDto : UserDto
    {
        public Guid Id { get; set; }
    }
}
