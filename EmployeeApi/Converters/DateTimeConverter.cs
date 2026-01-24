using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeApi.Converters;

public class DateTimeConverter : JsonConverter<DateTime?>
{
    private readonly string[] _formats = { "dd.MM.yyyy", "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ssZ" };

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            foreach (var format in _formats)
            {
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var date))
                {
                    if (format == "dd.MM.yyyy" || format == "yyyy-MM-dd")
                    {
                        date = date.Date;
                    }
                    return DateTime.SpecifyKind(date, DateTimeKind.Utc);
                }
            }

            if (DateTime.TryParse(dateString, null, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var parsedDate))
            {
                return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }
        }

        throw new JsonException($"Не удалось преобразовать '{reader.GetString()}' в DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
        }
    }
}
