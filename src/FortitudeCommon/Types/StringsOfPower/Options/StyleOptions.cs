// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.Options.DateTimeStyleFormat;
using static FortitudeCommon.Types.StringsOfPower.Options.TimeStyleFormat;

namespace FortitudeCommon.Types.StringsOfPower.Options;

public struct StyleOptionsValue
{
    private StyleOptions? fallbackOptions;

    public const string DefaultTimeToSecFormat   = "{0:HH:mm:ss}";
    public const string DefaultTimeToMsFormat    = "{0:HH:mm:ss.fff}";
    public const string DefaultTimeToUsFormat    = "{0:HH:mm:ss.ffffff}";
    public const string DefaultTimeToTicksFormat = "{0:HH:mm:ss.fffffff}";

    public const string DefaultYyyyMMddOnly = "{0:yyyy-MM-dd}";
    public const string DefaultYyyyMMddToSecFormat = "{0:yyyy-MM-ddTHH:mm:ssZ}";
    public const string DefaultYyyyMMddToMsFormat  = "{0:yyyy-MMd-dTHH:mm:ss.fff}";
    public const string DefaultYyyyMMddToUsFormat  = "{0:yyyy-MM-ddTHH:mm:ss.ffffff}";

    public StyleOptionsValue(StringStyle style) => this.style = style;
    public StyleOptionsValue(StyleOptions defaultOptions) => fallbackOptions = defaultOptions;

    public StyleOptionsValue(StringStyle style = StringStyle.Default, int indentSize = 2, char indentChar = ' '
      , bool byteSequenceToBase64 = true, bool disableCircularRefCheck = false, bool charSArraysAsString = false)
    {
        this.style                   = style;
        this.indentChar              = indentChar;
        this.indentSize              = indentSize;
        this.byteSequenceToBase64    = byteSequenceToBase64;
        this.disableCircularRefCheck = disableCircularRefCheck;
        this.charSArraysAsString     = charSArraysAsString;
    }

    private StringStyle? style;

    private char?   indentChar;
    private int?    indentSize;
    private bool?   byteSequenceToBase64;
    private bool?   disableCircularRefCheck;
    private bool?   charSArraysAsString;
    private bool?   circularRefUsesRefEquals;
    private string? newLineStyle;
    private string? nullStyle;
    private int?    prettyCollectionsColumnCountWrap;
    private bool?   enableColumnWrap;
    private int?    defaultGraphMaxDepth;

    private DateTimeStyleFormat? dateTimeFormat;
    private TimeStyleFormat?     timeFormat;

    private string? dateTimeYyyyMMddTossFormat;
    private string? dateTimeYyyyMMddTomsFormat;
    private string? dateTimeYyyyMMddTousFormat;

    private string? timeHHmmssFormat;
    private string? timeHHmmssToMsFormat;
    private string? timeHHmmssToUsFormat;
    private string? timeHHmmssToTicksFormat;
    private bool?   writeKeyValuePairsAsCollection;

    public StringStyle Style
    {
        readonly get => style ?? fallbackOptions?.Values.Style ?? StringStyle.Default;
        set => style = value;
    }

    public char IndentChar
    {
        get => indentChar ?? fallbackOptions?.Values.IndentChar ?? ' ';
        set => indentChar = value;
    }

    public int IndentSize
    {
        get => indentSize ?? fallbackOptions?.Values.IndentSize ?? 2;
        set => indentSize = value;
    }

    public int IndentRepeat(int indentLevel) => indentLevel * IndentSize;

    public string NewLineStyle
    {
        readonly get => newLineStyle ?? fallbackOptions?.Values.NewLineStyle ?? Environment.NewLine;
        set => newLineStyle = value;
    }

    public string NullStyle
    {
        readonly get => nullStyle ?? fallbackOptions?.Values.NullStyle ?? "null";
        set => nullStyle = value;
    }

    public TimeStyleFormat TimeFormat
    {
        readonly get => timeFormat ?? fallbackOptions?.Values.TimeFormat ?? StringHHmmssToTicks;
        set => timeFormat = value;
    }

    public string TimeAsStringFormatString =>
        TimeFormat switch
        {
            StringHHmmss     => TimeStringHHmmssFormatString
          , StringHHmmssToMs => TimeStringHHmmssToMsFormatString
          , StringHHmmssToUs => TimeStringHHmmssToUsFormatString
          , _                => TimeStringHHmmssToTicksFormatString
        };

    public string TimeStringHHmmssFormatString
    {
        readonly get => timeHHmmssFormat ?? fallbackOptions?.Values.TimeStringHHmmssFormatString ?? DefaultTimeToSecFormat;
        set => timeHHmmssFormat = value;
    }

    public string TimeStringHHmmssToMsFormatString
    {
        readonly get => timeHHmmssToMsFormat ?? fallbackOptions?.Values.TimeStringHHmmssToMsFormatString ?? DefaultTimeToMsFormat;
        set => timeHHmmssToMsFormat = value;
    }

    public string TimeStringHHmmssToUsFormatString
    {
        readonly get => timeHHmmssToUsFormat ?? fallbackOptions?.Values.TimeStringHHmmssToUsFormatString ?? DefaultTimeToUsFormat;
        set => timeHHmmssToUsFormat = value;
    }

    public string TimeStringHHmmssToTicksFormatString
    {
        readonly get => timeHHmmssToTicksFormat ?? fallbackOptions?.Values.TimeStringHHmmssToTicksFormatString ?? DefaultTimeToTicksFormat;
        set => timeHHmmssToTicksFormat = value;
    }

    public DateTimeStyleFormat DateTimeFormat
    {
        readonly get => dateTimeFormat ?? fallbackOptions?.Values.DateTimeFormat ?? StringYyyyMMddToss;
        set => dateTimeFormat = value;
    }

    public bool DateTimeIsNumber => DateTimeFormat is SecondsFromUnixEpoch or MillsFromUnixEpoch or MicrosFromUnixEpoch or NanosFromUnixEpoch;

    public bool DateTimeIsString => DateTimeFormat is StringYyyyMMddToss or StringYyyyMMddToms or StringYyyyMMddTous;

    public long DateTimeTicksToNumberPrecision(long timeStampTicks) =>
        DateTimeFormat switch
        {
            SecondsFromUnixEpoch => (timeStampTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond
          , MillsFromUnixEpoch   => (timeStampTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerMillisecond
          , MicrosFromUnixEpoch  => (timeStampTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerMicrosecond
          , _                    => (timeStampTicks - DateTime.UnixEpoch.Ticks) * 100
        };

    public string DateTimeAsStringFormatString =>
        DateTimeFormat switch
        {
            StringYyyyMMddOnly => DateTimeStringYyyyMMddOnly
          , StringYyyyMMddToss => DateTimeStringYyyyMMddToSecFormatString
          , StringYyyyMMddToms => DateTimeStringYyyyMMddToMsFormatString
          , _                  => DateTimeStringYyyyMMddToUsFormatString
        };

    public string DateTimeStringYyyyMMddOnly
    {
        readonly get => dateTimeYyyyMMddTossFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddOnly ?? DefaultYyyyMMddOnly;
        set => dateTimeYyyyMMddTossFormat = value;
    }

    public string DateTimeStringYyyyMMddToSecFormatString
    {
        readonly get => dateTimeYyyyMMddTossFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddToSecFormatString ?? DefaultYyyyMMddToSecFormat;
        set => dateTimeYyyyMMddTossFormat = value;
    }

    public string DateTimeStringYyyyMMddToMsFormatString
    {
        readonly get => dateTimeYyyyMMddTomsFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddToSecFormatString ?? DefaultYyyyMMddToMsFormat;
        set => dateTimeYyyyMMddTomsFormat = value;
    }

    public string DateTimeStringYyyyMMddToUsFormatString
    {
        readonly get => dateTimeYyyyMMddTousFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddToSecFormatString ?? DefaultYyyyMMddToUsFormat;
        set => dateTimeYyyyMMddTousFormat = value;
    }

    public bool DisableCircularRefCheck
    {
        get => disableCircularRefCheck ?? fallbackOptions?.Values.DisableCircularRefCheck ?? false;
        set => disableCircularRefCheck = value;
    }

    public bool CharSArraysAsString
    {
        get => charSArraysAsString ?? fallbackOptions?.Values.CharSArraysAsString ?? false;
        set => charSArraysAsString = value;
    }

    public bool EnableColumnContentWidthWrap
    {
        readonly get => enableColumnWrap ?? fallbackOptions?.Values.EnableColumnContentWidthWrap ?? true;
        set => enableColumnWrap = value;
    }

    public int PrettyCollectionsColumnContentWidthWrap
    {
        readonly get => prettyCollectionsColumnCountWrap ?? fallbackOptions?.Values.PrettyCollectionsColumnContentWidthWrap ?? 120;
        set => prettyCollectionsColumnCountWrap = value;
    }

    public bool ByteSequenceToBase64
    {
        get => byteSequenceToBase64 ?? fallbackOptions?.Values.ByteSequenceToBase64 ?? true;
        set => byteSequenceToBase64 = value;
    }

    public bool WriteKeyValuePairsAsCollection
    {
        readonly get => writeKeyValuePairsAsCollection ?? fallbackOptions?.Values.WriteKeyValuePairsAsCollection ?? false;
        set => writeKeyValuePairsAsCollection = value;
    }

    public bool? CircularRefUsesRefEquals
    {
        get => circularRefUsesRefEquals ?? fallbackOptions?.Values.ByteSequenceToBase64 ?? true;
        set => circularRefUsesRefEquals = value;
    }

    public int DefaultGraphMaxDepth
    {
        readonly get => defaultGraphMaxDepth ?? fallbackOptions?.Values.DefaultGraphMaxDepth ?? int.MaxValue;
        set => defaultGraphMaxDepth = value;
    }

    public StyleOptions? DefaultOptions
    {
        get => fallbackOptions;
        set => fallbackOptions = value;
    }
}

public class StyleOptions(StyleOptionsValue initialValues)
{
    private StyleOptionsValue values = initialValues;

    public StyleOptions() : this(new StyleOptionsValue()) { }

    public StyleOptionsValue Values
    {
        get => values;
        set => values = value;
    }

    public StringStyle Style
    {
        get => values.Style;
        set => values.Style = value;
    }

    public char IndentChar
    {
        get => values.IndentChar;
        set => values.IndentChar = value;
    }

    public int IndentSize
    {
        get => values.IndentSize;
        set => values.IndentSize = value;
    }

    public string Indent
    {
        get
        {
            var sb = MutableString.SmallScratchBuffer;
            sb.Append(values.IndentChar, values.IndentSize);
            var asString = sb.ToString();
            sb.DecrementRefCount();
            return asString;
        }

        set
        {
            IndentChar = value[0];
            IndentSize = value.Length;
        }
    }

    public int IndentRepeat(int indentLevel) => values.IndentRepeat(indentLevel);

    public string NewLineStyle
    {
        get => values.NewLineStyle;
        set => values.NewLineStyle = value;
    }

    public string NullStyle
    {
        get => values.NullStyle;
        set => values.NullStyle = value;
    }

    public TimeStyleFormat TimeFormat
    {
        get => values.TimeFormat;
        set => values.TimeFormat = value;
    }

    public string TimeAsStringFormatString => values.TimeAsStringFormatString;

    public string TimeStringHHmmssFormatString
    {
        get => values.TimeStringHHmmssFormatString;
        set => values.TimeStringHHmmssFormatString = value;
    }

    public string TimeStringHHmmssToMsFormatString
    {
        get => values.TimeStringHHmmssToMsFormatString;
        set => values.TimeStringHHmmssToMsFormatString = value;
    }

    public string TimeStringHHmmssToUsFormatString
    {
        get => values.TimeStringHHmmssToUsFormatString;
        set => values.TimeStringHHmmssToUsFormatString = value;
    }

    public string TimeStringHHmmssToTicksFormatString
    {
        get => values.TimeStringHHmmssToTicksFormatString;
        set => values.TimeStringHHmmssToTicksFormatString = value;
    }
    
    public DateTimeStyleFormat DateDateTimeFormat
    {
        get => values.DateTimeFormat;
        set => values.DateTimeFormat = value;
    }

    public bool DateTimeIsNumber => values.DateTimeIsNumber;

    public bool DateTimeIsString => values.DateTimeIsString;

    public long DateTimeTicksToNumberPrecision(long timeStampTicks) => values.DateTimeTicksToNumberPrecision(timeStampTicks);

    public string DateTimeAsStringFormatString => values.DateTimeAsStringFormatString;

    public string DateTimeStringYyyyMMddOnly
    {
        get => values.DateTimeStringYyyyMMddOnly;
        set => values.DateTimeStringYyyyMMddOnly = value;
    }

    public string DateTimeStringYyyyMMddTossFormatString
    {
        get => values.DateTimeStringYyyyMMddToSecFormatString;
        set => values.DateTimeStringYyyyMMddToSecFormatString = value;
    }

    public string DateTimeStringYyyyMMddTomsFormatString
    {
        get => values.DateTimeStringYyyyMMddToMsFormatString;
        set => values.DateTimeStringYyyyMMddToMsFormatString = value;
    }

    public string DateTimeStringYyyyMMddTousFormatString
    {
        get => values.DateTimeStringYyyyMMddToUsFormatString;
        set => values.DateTimeStringYyyyMMddToUsFormatString = value;
    }

    public bool DisableCircularRefCheck
    {
        get => values.DisableCircularRefCheck;
        set => values.DisableCircularRefCheck = value;
    }

    public bool CharSArraysAsString
    {
        get => values.CharSArraysAsString;
        set => values.CharSArraysAsString = value;
    }

    public bool ByteSequenceToBase64
    {
        get => values.ByteSequenceToBase64;
        set => values.ByteSequenceToBase64 = value;
    }

    public bool WriteKeyValuePairsAsCollection
    {
        get => values.WriteKeyValuePairsAsCollection;
        set => values.WriteKeyValuePairsAsCollection = value;
    }

    public bool? CircularRefUsesRefEquals
    {
        get => values.CircularRefUsesRefEquals;
        set => values.CircularRefUsesRefEquals = value;
    }

    public StyleOptions? DefaultOptions
    {
        get => values.DefaultOptions;
        set => values.DefaultOptions = value;
    }

    public bool EnableColumnContentWidthWrap
    {
        get => values.EnableColumnContentWidthWrap;
        set => values.EnableColumnContentWidthWrap = value;
    }

    public int PrettyCollectionsColumnContentWidthWrap
    {
        get => values.PrettyCollectionsColumnContentWidthWrap;
        set => values.PrettyCollectionsColumnContentWidthWrap = value;
    }

    public int DefaultGraphMaxDepth
    {
        get => values.DefaultGraphMaxDepth;
        set => values.DefaultGraphMaxDepth = value;
    }
}
