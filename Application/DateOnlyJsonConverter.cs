using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestingApp
{
    internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                return DateOnly.Parse(dateString);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    internal class DateOnlyNewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter<DateOnly>
    {
        //public override bool CanConvert(Type objectType)
        //{
        //    return objectType == typeof(DateOnly);
        //}

        public override DateOnly ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == Newtonsoft.Json.JsonToken.String)
            {
                var dateString = reader.Value.ToString();
                return DateOnly.Parse(dateString);
            }

            throw new JsonException();
        }

        //public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        //{
        //    if (reader.TokenType == Newtonsoft.Json.JsonToken.String)
        //    {
        //        var dateString = reader.ReadAsString();
        //        return DateOnly.Parse(dateString);
        //    }

        //    throw new JsonException();
        //}

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, DateOnly value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        //public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        //{
        //    writer.WriteValue(value.ToString());
        //}
    }
}