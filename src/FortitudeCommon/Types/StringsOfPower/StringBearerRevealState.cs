using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Types.StringsOfPower;

public delegate StateExtractStringRange StringBearerRevealState<in TToStyle>(TToStyle toStyle, ITheOneString toAppendTo);

public interface IStringBearerRevelStateProvider<in TToStyle> : IStringBearerFormattableProvider
{
    StringBearerRevealState<TToStyle>  StringBearerRevealState { get; }
}

public delegate int StringBearerSpanFormattable<in TToFormat>(TToFormat toFormat, Span<char> destination, ReadOnlySpan<char> format, IFormatProvider? provider);

public interface IStringBearerFormattableProvider
{
    bool SupportSpanFormattable { get; }
    bool SupportStyleToString { get; }
    Type ForType { get; }
}

public interface IStringBearerSpanFormattableProvider<in TToStyle> : IStringBearerFormattableProvider
{
    StringBearerSpanFormattable<TToStyle>  StringBearerSpanFormattable { get; }
}

public static class StringBearerRevelStateExtensions
{
    public static string DefaultToString<T>(this StringBearerRevealState<T> styler, T toStyle, IRecycler? recycler = null)
    {
        var styledStringBuilder = recycler?.Borrow<TheOneString>() ?? new TheOneString();
        styledStringBuilder.ClearAndReinitialize(StringStyle.Default);
        styler(toStyle, styledStringBuilder);
        return styledStringBuilder.ToString();
    }
}