using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Types.StyledToString;

public delegate StyledTypeBuildResult CustomTypeStyler<in TToStyle>(TToStyle toStyle, IStyledTypeStringAppender toAppendTo);


public interface ICustomTypeStylerProvider<in TToStyle> : ICustomFormattableProvider
{
    CustomTypeStyler<TToStyle>  CustomTypeStyler { get; }
}

public delegate int CustomSpanFormattable<in TToFormat>(TToFormat toFormat, Span<char> destination, ReadOnlySpan<char> format, IFormatProvider? provider);

public interface ICustomFormattableProvider
{
    bool SupportSpanFormattable { get; }
    bool SupportStyleToString { get; }
    Type ForType { get; }
}

public interface ICustomSpanFormattableProvider<in TToStyle> : ICustomFormattableProvider
{
    CustomSpanFormattable<TToStyle>  CustomSpanFormattable { get; }
}
