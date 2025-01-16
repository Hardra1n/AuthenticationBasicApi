
using Common.Dtos;
using Grpc.Core;
using System.Text.Json;

namespace GrpcServer.Services
{
    public class MainAppHttpClient : HttpClient
    {
        private const string MainAppConnectionStringName = "MainAppUrl";
        private const string UsersEndpointURI = "api/users";
        private ILogger _logger;

        public MainAppHttpClient(ILogger<MainAppHttpClient> logger, IConfiguration configuration)
        {
            _logger = logger;

            var mainAppConnectionStr 
                = configuration.GetConnectionString(MainAppConnectionStringName);
            if (mainAppConnectionStr == null) {
                throw new ArgumentException($"Unable to find connection string with name {MainAppConnectionStringName}");
            }

            BaseAddress = new Uri(mainAppConnectionStr);
        }

        public async Task<IEnumerable<UserIdDto>> GetUsers(ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Get, UsersEndpointURI);
            var response = await SendAsync(request);
            var users = await InterpretateResponseMessage<IEnumerable<UserIdDto>>(response);

            return users;
        }

        public async Task<UserIdDto> GetUserById(Guid id, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Get, $"{UsersEndpointURI}/{id}");
            var response = await SendAsync(request);
            var user = await InterpretateResponseMessage<UserIdDto>(response);
            
            return user;
        }

        public async Task CreateUser(UserDto user, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Post, UsersEndpointURI);
            request.Content = JsonContent.Create(user);
            var response = await SendAsync(request);
            await InterpretateResponseMessage(response);
        }

        public async Task UpdateUser(UserIdDto user, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Put, $"{UsersEndpointURI}");
            request.Content = JsonContent.Create(user);
            var response = await SendAsync(request);
            await InterpretateResponseMessage(response);
        }

        public async Task DeleteUser(Guid id, ServerCallContext context)
        {
            var request = CreateRequest(context, HttpMethod.Delete, $"{UsersEndpointURI}/{id}");
            var response = await SendAsync(request);
            await InterpretateResponseMessage(response);
        }

        public async Task<bool> CheckAuthentication(string authorizationHeaderValue)
        {
            HttpRequestMessage authPingMessage = new(HttpMethod.Get, "api/auth/ping");
            authPingMessage.Headers.Add("Authorization", authorizationHeaderValue);

            var response = await SendAsync(authPingMessage);

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
            requestMessage.RequestUri = new Uri(BaseAddress + uri);
            return requestMessage;
        }

        private async Task<T> InterpretateResponseMessage<T>(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(content, Extensions.GetDefaultSerializerOptions());
                if (data == null)
                {
                    throw new ArgumentException("Data was deserialized into 'null'");
                }
                return data;
            }
            await InterpretateResponseMessage(response);
            throw new NotImplementedException("Unrecongized flow of execution");
        }

        private async Task InterpretateResponseMessage(HttpResponseMessage response)
        {

        }
    }
}
