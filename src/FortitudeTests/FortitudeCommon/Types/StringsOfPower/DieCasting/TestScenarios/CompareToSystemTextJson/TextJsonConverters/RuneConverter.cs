using System.Buffers;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

public class RuneConverter(bool escaped = true): JsonConverter<Rune>
{
    public override Rune Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        if (Rune.DecodeFromUtf16(reader.GetString()!.AsSpan(), out Rune rune, out _) == OperationStatus.Done)
        {
            return rune;
        }
        throw new JsonException("Could not decode text as a rune");
    }

    public override void Write(Utf8JsonWriter writer, Rune value,
        JsonSerializerOptions options)
    {
        if (escaped)
        {
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            writer.WriteRawValue("\""+ value + "\"");
        }
    }
}

