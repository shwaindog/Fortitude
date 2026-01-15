using System.Text.Json;
using System.Text.Json.Serialization;
using FortitudeCommon.Extensions;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

public class Int128Converter : JsonConverter<Int128>
{
    public override Int128 Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Int128.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, Int128 value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}