using Domain.Exceptions;
using Domain;
using System.Collections.Immutable;

namespace Application.Authentication
{
    public class SystemUserSource
    {
        private List<SystemUser> _users;
        private object _lock = new object();

        public SystemUserSource() 
        {
            _users = new List<SystemUser>
            {
                new() {Username="admin@gmail.com", Password="12345"},
                new() { Username="qwerty@gmail.com", Password="55555"}
            };
        }

        public IList<SystemUser> GetUsers()
        {
            return _users.ToImmutableList();
        }

        public bool IsValidUser(string username, string password)
            => _users.Any(user => user.Username == username && user.Password == password);
    }
}
