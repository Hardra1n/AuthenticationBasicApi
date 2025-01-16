using Common;
using Common.Dtos;
using gRPC;
using System.ComponentModel;
using System.Text.Json;

namespace GrpcServer
{
    public static class Extensions
    {
        public static UserDto ToMainAppUser(this User user)
        {
            return new UserDto()
            {
                Birthday = DateOnly.Parse(user.Birthday),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static UserIdDto ToMainAppUser(this IdUser user)
        {
            return new UserIdDto()
            {
                Id = Guid.Parse(user.Id),
                Birthday = DateOnly.Parse(user.Birthday),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static IdUser ToIdUser(this UserIdDto user)
        {
            return new IdUser()
            {
                Id = user.Id.ToString(),
                Birthday = user.Birthday.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        public static User ToUser(this UserDto user)
        {
            return new User()
            {
                Birthday = user.Birthday.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static JsonSerializerOptions GetDefaultSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new DateOnlyJsonConverter());
            return options;
        }
    }
}
