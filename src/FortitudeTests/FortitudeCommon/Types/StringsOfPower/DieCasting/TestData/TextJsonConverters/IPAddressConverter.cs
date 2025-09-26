using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class IPAddressConverter: JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return IPAddress.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, IPAddress value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
