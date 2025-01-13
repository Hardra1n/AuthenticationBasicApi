using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain
{
    public class UserSource
    {
        private readonly List<User> _users = new List<User>();
        private readonly object _locker = new();

        public UserSource()
        {

        }

        public User FindById(Guid id)
        {
            lock (_locker)
            {
                var user = _users.FirstOrDefault(user => user.Id == id);
                if (user == null)
                {
                    ThrowsUserNotExist(id);
                }

                return user;
            }
        }

        public void Add(User user)
        {
            lock (_locker)
            {
                ThrowsIfUserWithFirstNameAndLastNameExists(user);

                Guid newUserGuid = Guid.NewGuid();
                while (_users.Any(user => user.Id == newUserGuid))
                {
                    newUserGuid = Guid.NewGuid();
                }

                user.Id = newUserGuid;
                _users.Add(user);
            }
        }

        public void AddRange(IEnumerable<User> users)
        {
            lock (_locker)
            {
                foreach (var user in users)
                {
                    Add(user);
                }
            }
        }

        public void Remove(Guid id)
        {
            lock (_locker)
            {
                var index = _users.FindIndex(user => user.Id == id);
                if (index < 0)
                {
                    ThrowsUserNotExist(id);
                }

                _users.RemoveAt(index);
            }
        }

        public void Update(User user)
        {
            lock (_locker)
            {
                var collectionUserId = _users.FindIndex(usr => usr.Id == user.Id);
                if (collectionUserId < 0)
                {
                    ThrowsUserNotExist(user.Id);
                }
                ThrowsIfUserWithFirstNameAndLastNameExists(user);

                _users[collectionUserId] = user;
            }
        }

        private void ThrowsUserNotExist(Guid id)
        {
            throw new UserNotFoundException(id);
        }

        private void ThrowsIfUserWithFirstNameAndLastNameExists(User user)
        {
            if (_users.Any((u) => u.FirstName == user.FirstName && u.LastName == user.LastName && u.Id != user.Id))
            {
                throw new UserAlreadyExistsException($"User with first name '{user.FirstName}' and last name '{user.LastName}' already exists");
            }
        }

        public IEnumerable<User> GetAll()
        {
            lock (_locker)
            {
                return _users.ToImmutableList();
            }
        }
    }
}
