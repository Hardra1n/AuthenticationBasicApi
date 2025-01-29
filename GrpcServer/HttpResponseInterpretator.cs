using Common;
using Grpc.Core;
using System.Text.Json;

namespace GrpcServer
{
    public class HttpResponseInterpretator
    {
        private const int MaxBytesCountForMessage = 100000;
        private readonly ILogger _logger;

        public HttpResponseInterpretator(ILogger<HttpResponseInterpretator> logger)
        {
            _logger = logger;
        }
        public async Task<T> InterpretateResponseMessage<T>(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(content, Extensions.GetDefaultSerializerOptions());
                if (data == null)
                {
                    _logger.LogWarning(content);
                    throw new ArgumentException("Data from main server was deserialized into 'null'");
                }
                return data;
            }
            await InterpretateResponseMessage(response);
            throw new NotImplementedException("Unrecongized flow of execution");
        }

        //public async Task InterpretateResponseMessage(HttpResponseMessage response)
        //{
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return;
        //    }

        //    var content = await response.Content.ReadAsStringAsync();
        //    var data = JsonSerializer.Deserialize<ErrorHttpMessage>(content, Extensions.GetDefaultSerializerOptions());
        //    var grpcStatusCode = response.StatusCode.ToGrpcStatusCode();
        //    if (data == null)
        //    {

        //        Status unrecongizedStatus = content.Length > MaxBytesCountForMessage
        //            ? new Status(grpcStatusCode, $"Response from main server was bigger then {MaxBytesCountForMessage} bytes")
        //            : new Status(grpcStatusCode, content);
        //        throw new RpcException(unrecongizedStatus);
        //    }
        //    Status status = new(grpcStatusCode, data.details);
        //    throw new RpcException(status);
        //}


        public async Task InterpretateResponseMessage(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ErrorHttpMessage>(content, Extensions.GetDefaultSerializerOptions());
            throw new MainServerException()
            {
                Content = content,
                StatusCode = response.StatusCode,
                ErrorHttpMessage = data
            };
        }
    }
}
