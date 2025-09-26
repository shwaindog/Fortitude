using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class StringBuilderConverter: JsonConverter<StringBuilder>
{
    public override StringBuilder Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return new StringBuilder(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, StringBuilder value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}