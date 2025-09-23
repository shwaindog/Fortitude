using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class TestCustomSpanFormattableConverter: JsonConverter<TestCustomSpanFormattable>
{
    public override TestCustomSpanFormattable Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return new TestCustomSpanFormattable(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, TestCustomSpanFormattable value,
        JsonSerializerOptions options)
    {
        writer.WriteRawValue("\"" + value + "\"");
    }
}