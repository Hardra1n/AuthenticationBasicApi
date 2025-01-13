using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.Soap
{
    [DataContract]
    public class UserSoap
    {

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [XmlIgnore]
        public DateOnly BirthdayDate { get; set; }

        [DataMember]
        public string Birthday
        {
            get
            {
                return BirthdayDate.ToString();
            }
            set
            {
                if (!DateOnly.TryParse(value, out var date))
                {
                    throw new ArgumentException($"Unable to parse {value} as date.");
                }
                BirthdayDate = date;
            }
        }
    }
}
