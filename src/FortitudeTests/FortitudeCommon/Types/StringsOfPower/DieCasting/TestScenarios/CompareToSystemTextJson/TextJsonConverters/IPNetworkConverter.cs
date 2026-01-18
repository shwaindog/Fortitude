using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

public class IPNetworkConverter : JsonConverter<IPNetwork>
{
    public override IPNetwork Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return IPNetwork.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, IPNetwork value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
