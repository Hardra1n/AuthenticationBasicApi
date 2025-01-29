using Domain.Exceptions;

namespace Application.GraphQL
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        private ILogger _logger;
        public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger)
        {
            _logger = logger;
        }


        public IError OnError(IError error)
        {
            IError responseError = error;


            if (error.Exception != null && error.Message.Contains("Unexpected Execution Error"))
            {
                responseError = responseError.WithMessage(error.Exception.Message).RemoveExtension("message");
            }

            responseError = responseError.RemoveExtension("stackTrace");

            return responseError;
        }
    }
}