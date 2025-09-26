using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class NullableBigIntegerConverter: JsonConverter<BigInteger?>
{
    public override BigInteger? Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Int128.Parse(reader.GetString()!, null);
    }

    public override void Write(Utf8JsonWriter writer, BigInteger? value,
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