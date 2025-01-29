using Application;
using Application.Authentication;
using Application.GraphQL;
using Application.Services;
using Application.Soap;
using Domain;
using NLog;
using NLog.Web;
using SoapCore;

namespace TestingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                RunApp(args);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
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

            // Can't interospect on IIS without allowing introspection
#pragma warning disable CS0618 // Type or member is obsolete
            builder.Services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddErrorFilter<GraphQLErrorFilter>()
                .AllowIntrospection(true);
#pragma warning restore CS0618 // Type or member is obsolete


            builder.ConfigureAuthentication();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSoapCore();
            builder.Services.AddSingleton<UserSource>();
            builder.Services.AddSingleton<SystemUserSource>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IUserSoapService, UserSoapService>();
            builder.Services.AddControllers((options) =>
            {
                options.InputFormatters.Insert(0, Extensions.GetJsonPatchInputFormatter());
            })
                .AddJsonOptions(options =>
                {
                    options.AllowInputFormatterExceptionMessages = false;
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<CustomAuthMiddleware>();
            app.UseAuthorization();

            (app as IApplicationBuilder).UseSoapEndpoint<IUserSoapService>(options =>
            {
                options.Path = "/UsersService.asmx";
                options.SoapSerializer = SoapSerializer.XmlSerializer;
            });

            app.MapControllers();
            app.MapGraphQL("/api/graphql");

            app.ConfigureInitialData();

            app.Run();
        }
    }
}
