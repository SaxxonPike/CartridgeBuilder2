using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CartridgeBuilder2.Cli.Configuration
{
    /*
     * It seems stupid that this has to be <T>. But there's a problem with the System.Text.Json implementation
     * that Newtonsoft.Json doesn't seem to have. This causes an Access Violation on .NET 5 and does not work if we
     * accept both Nullable<T> and T at the same time on .NET 6.
     */
    public class HexNumberJsonConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadInternal(ref reader);
        }

        private T ReadInternal(ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    // If this cast looks weird, it's because it is. This forces boxing/unboxing.
                    return (T)(object)reader.GetInt32();
                case JsonTokenType.String:
                {
                    var str = reader.GetString();
                    if (str == null)
                        return default;
                    if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ||
                        str.StartsWith("&h", StringComparison.OrdinalIgnoreCase))
                        return (T)(object)int.Parse(str[2..], NumberStyles.HexNumber);
                    return (T)(object)int.Parse(str, NumberStyles.Any);
                }
                default:
                    throw new JsonException("Failed to read");
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Yep. This is required.
            writer.WriteNumberValue((int)(object)value);
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert == typeof(T) || typeToConvert == typeof(string);
    }
}