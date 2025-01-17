using Common;
using Common.Dtos;
using Grpc.Core;
using System.Text.Json;

namespace GrpcServer.Services
{
    public class MainServerCaller
    {
        private const string UsersEndpointURI = "api/users";
        private ILogger _logger;
        private HttpClient _httpClient;
        private HttpResponseInterpretator _httpInterpretator;

        public MainServerCaller(ILogger<MainServerCaller> logger, HttpClient httpClient, HttpResponseInterpretator httpInterpretator, IConfiguration configuration)
        {
            _logger = logger;
            _httpInterpretator = httpInterpretator;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserIdDto>> GetUsers(ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Get, UsersEndpointURI);
            var response = await _httpClient.SendAsync(request);
            var users = await _httpInterpretator.InterpretateResponseMessage<IEnumerable<UserIdDto>>(response);

            return users;
        }

        public async Task<UserIdDto> GetUserById(Guid id, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Get, $"{UsersEndpointURI}/{id}");
            var response = await _httpClient.SendAsync(request);
            var user = await _httpInterpretator.InterpretateResponseMessage<UserIdDto>(response);
            
            return user;
        }

        public async Task CreateUser(UserDto user, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Post, UsersEndpointURI);
            request.Content = JsonContent.Create(user);
            var response = await _httpClient.SendAsync(request);
            await _httpInterpretator.InterpretateResponseMessage(response);
        }

        public async Task UpdateUser(UserIdDto user, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Put, $"{UsersEndpointURI}");
            request.Content = JsonContent.Create(user);
            var response = await _httpClient.SendAsync(request);
            await _httpInterpretator.InterpretateResponseMessage(response);
        }

        public async Task DeleteUser(Guid id, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Delete, $"{UsersEndpointURI}/{id}");
            var response = await _httpClient.SendAsync(request);
            await _httpInterpretator.InterpretateResponseMessage(response);
        }

        public async Task<bool> CheckAuthentication(string authorizationHeaderValue)
        {
            HttpRequestMessage authPingMessage = new(HttpMethod.Get, "api/auth/ping");
            authPingMessage.Headers.Add("Authorization", authorizationHeaderValue);

            var response = await _httpClient.SendAsync(authPingMessage);

            return response.IsSuccessStatusCode;
        }

        private HttpRequestMessage CreateRequest(ServerCallContext context, HttpMethod httpMethod, string uri) 
        {
            var requestMessage = new HttpRequestMessage();
            var authEntry = context.RequestHeaders.Get("Authorization");
            if (authEntry != null)
            {
                requestMessage.Headers.Add("Authorization", authEntry.Value);
            }

            requestMessage.Method = httpMethod;
            requestMessage.RequestUri = new Uri(_httpClient.BaseAddress + uri);
            return requestMessage;
        }
    }
}
