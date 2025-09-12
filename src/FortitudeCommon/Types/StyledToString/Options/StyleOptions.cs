// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.Options;

public struct StyleOptionsValue
{
    private StyleOptions? fallbackOptions;

    public const string DefaultYyyyMMddTossFormat = "{0:yyyy-MM-ddTHH:mm:ss}";
    public const string DefaultYyyyMMddTomsFormat = "{0:yyyy-MMd-dTHH:mm:ss.fff}";
    public const string DefaultYyyyMMddTousFormat = "{0:yyyy-MM-ddTHH:mm:ss.ffffff}";

    public StyleOptionsValue(StringBuildingStyle style) => this.style = style;
    public StyleOptionsValue(StyleOptions defaultOptions) => fallbackOptions = defaultOptions;

    public StyleOptionsValue(StringBuildingStyle style = StringBuildingStyle.Default, int indentSize = 2, char indentChar = ' '
      , bool byteSequenceToBase64 = true, bool disableCircularRefCheck = false, bool charSArraysAsString = false)
    {
        this.style                   = style;
        this.indentChar              = indentChar;
        this.indentSize              = indentSize;
        this.byteSequenceToBase64    = byteSequenceToBase64;
        this.disableCircularRefCheck = disableCircularRefCheck;
        this.charSArraysAsString     = charSArraysAsString;
    }

    private StringBuildingStyle? style;

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

    private TimeStyleFormat? dateTimeFormat;

    private string? dateTimeYyyyMMddTossFormat;
    private string? dateTimeYyyyMMddTomsFormat;
    private string? dateTimeYyyyMMddTousFormat;

    public StringBuildingStyle Style
    {
        readonly get => style ?? fallbackOptions?.Values.Style ?? StringBuildingStyle.Default;
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

    public TimeStyleFormat DateTimeFormat
    {
        readonly get => dateTimeFormat ?? fallbackOptions?.Values.DateTimeFormat ?? TimeStyleFormat.StringYyyyMMddToss;
        set => dateTimeFormat = value;
    }

    public string DateTimeStringYyyyMMddTossFormatString
    {
        readonly get => dateTimeYyyyMMddTossFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddTossFormatString ?? DefaultYyyyMMddTossFormat;
        set => dateTimeYyyyMMddTossFormat = value;
    }

    public string DateTimeStringYyyyMMddTomsFormatString
    {
        readonly get => dateTimeYyyyMMddTomsFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddTossFormatString ?? DefaultYyyyMMddTomsFormat;
        set => dateTimeYyyyMMddTomsFormat = value;
    }

    public string DateTimeStringYyyyMMddTousFormatString
    {
        readonly get => dateTimeYyyyMMddTousFormat ?? fallbackOptions?.Values.DateTimeStringYyyyMMddTossFormatString ?? DefaultYyyyMMddTousFormat;
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

    public bool? CircularRefUsesRefEquals
    {
        get => circularRefUsesRefEquals ?? fallbackOptions?.Values.ByteSequenceToBase64 ?? true;
        set => circularRefUsesRefEquals = value;
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
    public StyleOptionsValue Values
    {
        get => values;
        set => values = value;
    }

    public StringBuildingStyle Style
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

    public TimeStyleFormat DateTimeFormat
    {
        get => values.DateTimeFormat;
        set => values.DateTimeFormat = value;
    }

    public string DateTimeStringYyyyMMddTossFormatString
    {
        get => values.DateTimeStringYyyyMMddTossFormatString;
        set => values.DateTimeStringYyyyMMddTossFormatString = value;
    }

    public string DateTimeStringYyyyMMddTomsFormatString
    {
        get => values.DateTimeStringYyyyMMddTomsFormatString;
        set => values.DateTimeStringYyyyMMddTomsFormatString = value;
    }

    public string DateTimeStringYyyyMMddTousFormatString
    {
        get => values.DateTimeStringYyyyMMddTousFormatString;
        set => values.DateTimeStringYyyyMMddTousFormatString = value;
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
}
