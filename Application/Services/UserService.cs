using Application.GraphQL;
using Domain;
using HotChocolate.Subscriptions;
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
        private readonly ITopicEventSender _topicEventSender;

        public UserService(ILogger<UserService> logger, IRepository<User> repository, ITopicEventSender topicEventSender)
        {
            _logger = logger;
            _repository = repository;
            _topicEventSender = topicEventSender;
        }

        public User GetById(Guid id) => _repository.GetById(id);

        public void Delete(Guid id) => _repository.Delete(id);

        public void Update(User entity) => _repository.Update(entity);

        public void Add(User entity)
        {
            _repository.Add(entity);
            var addedUser = GetAll().FirstOrDefault(user => entity.FirstName == user.FirstName && entity.LastName == user.LastName);
            if (addedUser != null)
            {
                _topicEventSender.SendAsync(nameof(Subscription.UserAdded), addedUser);
            }
        }

        public IEnumerable<User> GetAll() => _repository.GetAll();
    }
}
