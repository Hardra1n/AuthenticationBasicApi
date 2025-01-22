using Domain;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Application.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _repository;

        public UserService(ILogger<UserService> logger, IRepository<User> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public User GetById(Guid id) => _repository.GetById(id);

        public void Delete(Guid id) => _repository.Delete(id);

        public void Update(User entity) => _repository.Update(entity);

        public void Add(User entity) => _repository.Add(entity);

        public IEnumerable<User> GetAll() => _repository.GetAll();
    }
}
