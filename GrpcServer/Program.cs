using Application;
using GrpcServer.Services;
using NLog.Web;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication;

namespace GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            //builder.Services.AddAuthentication();
            builder.Services.AddAuthentication(GrpcCustomAuthenticationDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, GrpcCustomAuthenticationHandler>(
                    GrpcCustomAuthenticationDefaults.AuthenticationScheme, options => { });
            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient<MainAppHttpClient>();
            builder.Services.AddSingleton<MainAppHttpClient>();
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionHandlingRpcInterceptor>();
                options.EnableDetailedErrors = true;
            });

            var app = builder.Build();

            app.UseAuthentication();
            //app.UseMiddleware<GrpcCustomAuthMiddleware>();
            app.UseAuthorization();

            app.MapGrpcService<UserGrpcService>();

            app.Run();
        }
    }
}