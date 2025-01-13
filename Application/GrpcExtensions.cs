using Application.gRPC;
using Domain;
using Domain.Exceptions;
using Google.Protobuf.WellKnownTypes;

namespace Application
{
    public static class GrpcExtensions
    {
        public static gRPC.User ToGrpcUser(this Domain.User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthday = user.Birthday.ToString()
        };

        public static string ToGrpcId(this Guid id) => id.ToString();

        public static Guid ToDomainId(this string id)
        {
            return Guid.Parse(id);
        }

        public static gRPC.IdUser ToGrpcIdUser(this Domain.User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id.ToGrpcId(),
            Birthday = user.Birthday.ToString(),
        };

        public static Domain.User ToDomainUser(this gRPC.User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthday = user.Birthday.ToDateOnly()
        };

        public static Domain.User ToDomainUser(this gRPC.IdUser user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id.ToDomainId(),
            Birthday = user.Birthday.ToDateOnly()
        };

        private static DateOnly ToDateOnly(this string date)
        {
            if (!DateOnly.TryParse(date, out DateOnly dateOnly))
            {
                throw new DomainException($"Unable to parse {date} as date");
            }
            return dateOnly;
        }
    }
}
