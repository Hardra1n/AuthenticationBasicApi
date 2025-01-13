using System.Runtime.Serialization;

namespace Application.Soap
{
    [DataContract]
    public class UserSoap
    {

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;
    }
}
