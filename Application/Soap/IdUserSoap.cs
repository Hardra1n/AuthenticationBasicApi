using System.Runtime.Serialization;

namespace Application.Soap
{
    [DataContract]
    public class IdUserSoap : UserSoap
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
