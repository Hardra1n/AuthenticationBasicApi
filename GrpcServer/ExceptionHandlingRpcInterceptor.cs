using Grpc.Core.Interceptors;
using Grpc.Core;
using GrpcServer;

namespace Application
{
    public class ExceptionHandlingRpcInterceptor : Interceptor
    {
        private const int MaxBytesCountForMessage = 100000;

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {

            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                if (ex is RpcException rpcException)
                {
                    throw;
                }

                if (ex is MainServerException mainServerException)
                {
                    throw InterpretException(mainServerException);
                }

                Status status = new(StatusCode.Internal, "Server error: " + ex.Message, ex);
                throw new RpcException(status);
            }
        }

        public RpcException InterpretException(MainServerException mainServerException)
        {

            var grpcStatusCode = mainServerException.StatusCode.ToGrpcStatusCode();
            if (mainServerException.ErrorHttpMessage == null)
            {

                Status unrecongizedStatus = mainServerException.Content.Length > MaxBytesCountForMessage
                    ? new Status(grpcStatusCode, $"Response from main server was bigger then {MaxBytesCountForMessage} bytes")
                    : new Status(grpcStatusCode, mainServerException.Content);
                throw new RpcException(unrecongizedStatus);
            }
            Status status = new(grpcStatusCode, mainServerException.ErrorHttpMessage.details);
            throw new RpcException(status);
        }
    }
}
