using gRPC;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;


namespace GrpcServer.Services
{
    public class UserGrpcService : UserService.UserServiceBase
    {
        private readonly ILogger<UserGrpcService> _logger;
        private readonly MainAppHttpClient _httpClient;


        public UserGrpcService(ILogger<UserGrpcService> logger, MainAppHttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public override async Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var users = await _httpClient.GetUsers(context);
            GetUsersResponse response = new GetUsersResponse();
            response.Users.Add(
                users.Select(user => user.ToIdUser()));
            return response;
        }
        public override async Task GetUsersStream(GetUsersRequest request, IServerStreamWriter<GetUsersStreamResponse> responseStream, ServerCallContext context)
        {
            var users = await _httpClient.GetUsers(context);

            foreach (var user in users)
            {
                var response = new GetUsersStreamResponse();
                response.Users.Add(user.ToIdUser());
                await responseStream.WriteAsync(response);
                await Task.Delay(200);

            }
        }

        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _httpClient.GetUserById(Guid.Parse(request.Id), context);
            var response = new GetUserByIdResponse();
            response.User = user.ToIdUser();
            return response;
        }

        [Authorize]
        public override async Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            await _httpClient.CreateUser(request.User.ToMainAppUser(), context);
            var response = new AddUserResponse();
            return response;
        }

        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            await _httpClient.DeleteUser(Guid.Parse(request.Id), context);
            var response = new DeleteUserResponse();
            return response;
        }

        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            await _httpClient.UpdateUser(request.User.ToMainAppUser(), context);
            var response = new UpdateUserResponse();
            return response;
        }

    }
}
