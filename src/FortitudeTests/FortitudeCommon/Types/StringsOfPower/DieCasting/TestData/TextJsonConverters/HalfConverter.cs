using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class HalfConverter : JsonConverter<Half>
{
    public override Half Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Half.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, Half value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
