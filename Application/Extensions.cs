using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using TestingApp;

namespace Application
{
    public static class Extensions
    {
        public static IInputFormatter GetJsonPatchInputFormatter()
        {

            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new DateOnlyNewtonsoftJsonConverter());
                })
                .Services.BuildServiceProvider();

            var patchInputFormatter = builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();

            return patchInputFormatter;
        }
    }
}
