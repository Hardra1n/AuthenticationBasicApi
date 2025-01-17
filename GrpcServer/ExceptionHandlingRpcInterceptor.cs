using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Application
{
    public class ExceptionHandlingRpcInterceptor : Interceptor
    {

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

                Status status = new(StatusCode.Internal, "Server error: " + ex.Message, ex);
                throw new RpcException(status);
            }
        }
    }
}
