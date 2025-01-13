using System.ServiceModel;

namespace Application.Soap
{
    [ServiceContract]
    public interface IUserSoapService
    {

        [OperationContract]
        bool Ping();

        [OperationContract]
        IEnumerable<IdUserSoap> GetUsers();

        [OperationContract]
        IdUserSoap GetUserById(Guid id);

        [OperationContract]
        void AddUser(UserSoap user);

        [OperationContract]
        void DeleteUser(Guid id);

        [OperationContract]
        void UpdateUser(IdUserSoap user);

    }
}
