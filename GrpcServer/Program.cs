using Application;
using GrpcServer.Services;
using NLog.Web;
using Microsoft.AspNetCore.Authentication;
using NLog;

namespace GrpcServer
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("Start app");
            try
            {
                RunApp(args);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static void RunApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddGraphQLServer()
            //    .AddQueryType<Query>();
            builder.Services.AddAuthentication(GrpcCustomAuthenticationDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, GrpcCustomAuthenticationHandler>(
                    GrpcCustomAuthenticationDefaults.AuthenticationScheme, options => { });
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<MainServerCaller>();
            builder.Services.AddScoped<HttpResponseInterpretator>();
            builder.ConfigureHttpClient();

            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionHandlingRpcInterceptor>();
                options.EnableDetailedErrors = true;
            });
            builder.Services.AddGrpcReflection();

            var app = builder.Build();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGrpcService<UserGrpcService>();

            app.MapGrpcReflectionService();

            //app.MapGraphQL("/api/graphql");

            app.Run();
        }
    }
}