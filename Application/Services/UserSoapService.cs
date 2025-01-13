using Application.Soap;
using System.ServiceModel;

namespace Application.Services
{
    public class UserSoapService : IUserSoapService
    {
        private readonly UserService _service;

        public UserSoapService(UserService service) 
        {
            _service = service;
        }


        public bool Ping() => true;

        public IEnumerable<IdUserSoap> GetUsers()
        {
            return _service.GetAll().Select(domUser => domUser.ToIdUserSoap()).ToList();
        }

        public IdUserSoap GetUserById(Guid id)
        {
            var user = _service.GetById(id);

            return user.ToIdUserSoap();
        }

        public void AddUser(UserSoap user)
        {
            var domainUser = user.ToDomainUser();
            _service.Add(domainUser);
        }

        public void DeleteUser(Guid id)
        {
            _service.Delete(id);
        }

        public void UpdateUser(IdUserSoap user)
        {
            _service.Update(user.ToDomainUser());
        }
    }
}
