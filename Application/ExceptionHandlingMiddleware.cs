using Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Application
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            if (ex is DomainException domainException)
            {
                return HandleDomainExceptionAsync(context, domainException);
            }

            if (ex is JsonSerializationException && ex.InnerException is DomainException innerException)
            {
                return HandleDomainExceptionAsync(context, innerException);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Customize the response as needed
            var result = new ErrorHttpMessage(
                HttpStatusCode.InternalServerError.ToString(),
                ex.Message);
            return context.Response.WriteAsJsonAsync(result);
        }

        private Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
        {
            switch (ex)
            {
                case UserNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }

            var result = new ErrorHttpMessage(
                Enum.GetName(typeof(HttpStatusCode), context.Response.StatusCode),
                ex.Message);
            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
