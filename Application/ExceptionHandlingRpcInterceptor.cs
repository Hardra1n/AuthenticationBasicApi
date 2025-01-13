using Grpc.Core.Interceptors;
using Grpc.Core;
using static Grpc.Core.Interceptors.Interceptor;
using Domain.Exceptions;
using System.Net;

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
                HandleException(context, ex);
                var type = typeof(TResponse);
                var constructor = type.GetConstructors().FirstOrDefault();
                var obj = constructor?.Invoke(null);
                if (obj is TResponse response)
                {
                    return response;
                }
                else
                {
                    return default(TResponse);
                }
            }
        }

        private void HandleException(ServerCallContext context, Exception ex)
        {
            if (ex is DomainException domainEx)
            {
                HandleDomainException(context, domainEx);
            }
            else
            {
                Status status = new Status(StatusCode.Internal, ex.Message);
                context.Status = status;
            }
        }

        private void HandleDomainException(ServerCallContext context, DomainException domainEx)
        {
            StatusCode statusCode;
            switch (domainEx)
            {
                case UserNotFoundException:
                    statusCode = StatusCode.NotFound;
                    break;
                case UserAlreadyExistsException:
                    statusCode = StatusCode.AlreadyExists;
                    break;
                case InnerLogicExeption:
                    statusCode = StatusCode.InvalidArgument;
                    break;
                default:
                    statusCode = StatusCode.InvalidArgument;
                    break;
            }


            Status status = new Status(statusCode, domainEx.Message);
            context.Status = status;
        }
    }
}
