using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;

public class JsonStringPropertyClass : IStringBearer
{
    public JsonStringPropertyClass(string jsonStringPropield) => InitializeAllSet(jsonStringPropield);

    public void InitializeAllSet(string jsonString)
    {
        JsonStringPropield = jsonString;
    }

    public void InitializeAllDefault()
    {
        JsonStringPropield = null!;
    }

    public string JsonStringPropield { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        var ctb = tos.StartComplexType(this);
        ctb.Field.AlwaysAdd(nameof(JsonStringPropield), JsonStringPropield);
        return ctb.Complete();
    }
}
