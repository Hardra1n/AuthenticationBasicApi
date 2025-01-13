namespace Domain
{
    public class UserRepository : IRepository<User>
    {
        private readonly UserSource _userSource;

        public UserRepository(UserSource userSource)
        {
            _userSource = userSource;
        }

        public IEnumerable<User> GetAll() => _userSource.GetAll();

        public User GetById(Guid id) => _userSource.FindById(id);

        public void Delete(Guid id) => _userSource.Remove(id);

        public void Update(User entity) => _userSource.Update(entity);

        public void Add(User entity) => _userSource.Add(entity);
    }
}
