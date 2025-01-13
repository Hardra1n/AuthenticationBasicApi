
using System.ComponentModel;
using System.Net;
using Application;
using Application.Authentication;
using Application.Services;
using Application.Soap;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SoapCore;

namespace TestingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.ConfigureAuthentication();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSoapCore();
            //builder.Services.AddSoapMessageProcessor<SoapExceptionMessageProcessor>();
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionHandlingRpcInterceptor>();
            });

            builder.Services.AddSingleton<UserSource>();
            builder.Services.AddSingleton<SystemUserSource>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IUserSoapService, UserSoapService>();
            builder.Services.AddControllers()
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

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<CustomAuthMiddleware>();
            app.UseAuthorization();

            (app as IApplicationBuilder).UseSoapEndpoint<IUserSoapService>(options =>
            {
                options.Path = "/UsersService.asmx";
                options.SoapSerializer = SoapSerializer.XmlSerializer;
            });

            app.MapGrpcService<UserGrpcService>();
            app.MapControllers();

            app.ConfigureInitialData();
            app.Run();
        }
    }
}
