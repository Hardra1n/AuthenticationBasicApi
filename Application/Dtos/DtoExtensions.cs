using Common.Dtos;
using Domain;

namespace Application.Dtos
{
    public static class DtoExtensions
    {
        public static User ToUser(this UserIdDto user)
        {
            return new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }
        public static User ToUser(this UserDto user)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }

        public static UserIdDto ToIdUserDto(this User user)
        {
            return new UserIdDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }


        public static UserDto ToUserDto(this User user)
        {
            return new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }
    }
}
