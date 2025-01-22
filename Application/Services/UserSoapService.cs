using Application.Soap;
using System.Net;
using System.ServiceModel;

namespace Application.Services
{
    public class UserSoapService : IUserSoapService
    {
        private readonly UserService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSoapService(UserService service, IHttpContextAccessor httpContextAccessor) 
        {
            _service = service;
            this._httpContextAccessor = httpContextAccessor;
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
            if (!_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
            {
                throw new FaultException("Anauthorized access");
            }
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
