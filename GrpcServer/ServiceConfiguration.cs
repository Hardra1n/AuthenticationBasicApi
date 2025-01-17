using GrpcServer.Services;
using NLog;

namespace GrpcServer
{
    public static class ServiceConfiguration
    {
        private static string MainServerConnectionStringName = "MainServerUrl";

        public static void ConfigureHttpClient(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<MainServerCaller>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString(MainServerConnectionStringName);
                if (connectionString == null)
                {
                    throw new ArgumentException("Connection string to main server was null");
                }
                options.BaseAddress = new Uri(connectionString);
            });
        }
    }
}
