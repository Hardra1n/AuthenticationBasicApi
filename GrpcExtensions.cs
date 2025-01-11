using Application.gRPC;
using Domain;

namespace Application
{
    public static class GrpcExtensions
    {
        public static gRPC.User ToGrpcUser(this Domain.User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName
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
            Id = user.Id.ToGrpcId()
        };

        public static Domain.User ToDomainUser(this gRPC.User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        public static Domain.User ToDomainUser(this gRPC.IdUser user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id.ToDomainId()
        };
    }
}
