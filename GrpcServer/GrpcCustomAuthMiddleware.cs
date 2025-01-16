//using GrpcServer.Services;

//namespace GrpcServer
//{
//    public class GrpcCustomAuthMiddleware
//    {
//        private RequestDelegate _next;
//        private ILogger _logger;

//        public GrpcCustomAuthMiddleware(RequestDelegate next, ILogger<GrpcCustomAuthMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context, MainAppHttpClient httpClient)
//        {
//            if (context.Request.Headers.TryGetValue("Authorization", out var value))
//            {
//                var headerValue = value.ToString();
//                await httpClient.CheckAuthentication(headerValue);
//                context.User.Identity.IsAuthenticated
//            }
//            await _next(context);
//        }
//    }
//}