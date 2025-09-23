using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class ComplexConverter : JsonConverter<Complex>
{
    public override Complex Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Complex.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, Complex value,
        JsonSerializerOptions options)
    {
        writer.WriteRawValue("\"" + value + "\"");
    }
}
