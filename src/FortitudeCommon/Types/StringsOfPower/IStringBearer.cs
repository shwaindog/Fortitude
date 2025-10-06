using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Types.StringsOfPower;


[Obsolete("Generates temporary heap object. Consider implementing IStyledToStringObject for this type.")]
public class CallsObjectToString : Attribute { }

[Obsolete("Generates temporary heap object")]
public class CreatesParamsObjectArray : System.Attribute { }



public interface IStringBearer
{
    StateExtractStringRange RevealState(ITheOneString tos);
}

public interface IReceivesNotificationOfStringBearer : IStringBearer
{
    void StartingStyledToString(ITheOneString sbc);

    void CompletedStyledToString(ITheOneString sbc);
}

public static class StringBearerExtensions
{
    public static string SelectStyleToString(this IStringBearer makeString, StringStyle style, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<TheOneString>() ?? new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptionsValue(style));
        makeString.RevealState(styledStringBuilder);
        return styledStringBuilder.ToString();
    }
    
    public static string DefaultToString(this IStringBearer makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<TheOneString>() ?? new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptionsValue( StringStyle.Default));
        makeString.RevealState(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonCompactString(this IStringBearer makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<TheOneString>() ?? new TheOneString();
        styledStringBuilder.ClearAndReinitialize( new StyleOptionsValue( StringStyle.Compact | StringStyle.Json));
        makeString.RevealState(styledStringBuilder);
        return styledStringBuilder.ToString();
    }

    public static string JsonPrettyString(this IStringBearer makeString, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<TheOneString>() ?? new TheOneString();
        styledStringBuilder.ClearAndReinitialize(  new StyleOptionsValue(StringStyle.Pretty | StringStyle.Json));
        makeString.RevealState(styledStringBuilder);
        return styledStringBuilder.ToString();
    }
}