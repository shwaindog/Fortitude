using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

public class NullableHalfConverter: JsonConverter<Half?>
{
    public override Half? Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Half.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, Half? value,
        JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }
}
