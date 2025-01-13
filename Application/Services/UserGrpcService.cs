using Application.gRPC;
using Domain.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;


namespace Application.Services
{
    public class UserGrpcService : gRPC.UserService.UserServiceBase
    {
        private readonly ILogger<UserGrpcService> _logger;
        private readonly UserService _service;


        public UserGrpcService(ILogger<UserGrpcService> logger, UserService service)
        {
            _logger = logger;
            _service = service;
        }

        public override Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var domainUsers = _service.GetAll();
            GetUsersResponse response = new GetUsersResponse();
            response.Users.Add(
                domainUsers.Select(domainUser => domainUser.ToGrpcIdUser()));
            return Task.FromResult(response);
        }
        public override async Task GetUsersStream(GetUsersRequest request, IServerStreamWriter<GetUsersStreamResponse> responseStream, ServerCallContext context)
        {
            var domainUsers = _service.GetAll();

            foreach (var user in domainUsers)
            {
                var response = new GetUsersStreamResponse();
                response.Users.Add(user.ToGrpcIdUser());
                await responseStream.WriteAsync(response);
                await Task.Delay(200);
            }
        }

        public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            Domain.User domainUser;
            try
            {
                domainUser = _service.GetById(request.Id.ToDomainId());
            }
            catch (FormatException)
            {
                throw new DomainException("Wrong ID format");
            }

            var response = new GetUserByIdResponse()
            {
                User = domainUser.ToGrpcIdUser()
            };
            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var domainUser = request.User.ToDomainUser();
            _service.Add(domainUser);
            var response = new AddUserResponse();
            return Task.FromResult(response);
        }

        public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var domainId = request.Id.ToDomainId();
            _service.Delete(domainId);
            var response = new DeleteUserResponse();
            return Task.FromResult(response);
        }

        public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var domainUser = request.User.ToDomainUser();
            _service.Update(domainUser);
            var response = new UpdateUserResponse();
            return Task.FromResult(response);
        }

    }
}
