using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class TestCustomSpanFormattableConverter: JsonConverter<MySpanFormattableClass>
{
    public override MySpanFormattableClass Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return new MySpanFormattableClass(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, MySpanFormattableClass value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}