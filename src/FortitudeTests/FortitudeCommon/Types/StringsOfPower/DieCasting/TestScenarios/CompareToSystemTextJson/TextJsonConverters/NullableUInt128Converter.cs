using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

public class NullableUInt128Converter : JsonConverter<UInt128?>
{
    public override UInt128? Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return UInt128.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, UInt128? value,
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