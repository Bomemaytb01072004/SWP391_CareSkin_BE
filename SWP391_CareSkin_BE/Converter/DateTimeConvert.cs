using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace SWP391_CareSkin_BE.Converter
{
    public class DateTimeConvert : JsonConverter<DateTime>
    {
        private readonly string _format;

        public DateTimeConvert(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return DateTime.Parse(reader.GetString()); 
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format, CultureInfo.InvariantCulture));
        }
    }
}
