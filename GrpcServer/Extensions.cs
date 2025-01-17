using Common;
using Common.Dtos;
using gRPC;
using Grpc.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel;
using System.Net;
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
                Id = ParseId(user.Id),
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

        public static Guid ParseId(string id)
        {
            if (Guid.TryParse(id, out Guid result))
            {
                return result;
            }
            throw new RpcException(new(StatusCode.InvalidArgument, $"Wrong ID format. '{id}'"));
        }

        public static DateOnly ParseDateOnly(string dateOnly)
        {
            if (DateOnly.TryParse(dateOnly, out var date))
            {
                return date;
            }
            throw new RpcException(new(StatusCode.InvalidArgument, $"Wrong dateonly format '{dateOnly}'"));
        }

        public static JsonSerializerOptions GetDefaultSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new DateOnlyJsonConverter());
            return options;
        }

        public static StatusCode ToGrpcStatusCode(this HttpStatusCode httpStatusCode)
        {
            StatusCode statusCode;
            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest:
                    statusCode = StatusCode.InvalidArgument;
                    break;
                case HttpStatusCode.Forbidden:
                    statusCode = StatusCode.PermissionDenied;
                    break;
                case HttpStatusCode.NotFound:
                    statusCode = StatusCode.NotFound;
                    break;
                case HttpStatusCode.NotImplemented:
                    statusCode = StatusCode.Unimplemented;
                    break;
                case HttpStatusCode.Unauthorized:
                    statusCode = StatusCode.Unauthenticated;
                    break;
                case HttpStatusCode.Conflict:
                    statusCode = StatusCode.Aborted;
                    break;
                default:
                    statusCode = StatusCode.Unknown;
                    break;
            }
            return statusCode;
        }
    }
}
