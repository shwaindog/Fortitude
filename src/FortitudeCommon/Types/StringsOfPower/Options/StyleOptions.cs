// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.DateTimeStyleFormat;
using static FortitudeCommon.Types.StringsOfPower.Options.InputClassFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.TimeStyleFormat;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.Options;

public struct StyleOptionsValue : IJsonFormattingOptions
{
    private StyleOptions? fallbackOptions;

    private StyleOptions? myOptionsObj;

    public const string DefaultTimeFormat        = "{0:O}";
    public const string DefaultTimeToSecFormat   = "{0:HH:mm:ss}";
    public const string DefaultTimeToMsFormat    = "{0:HH:mm:ss.fff}";
    public const string DefaultTimeToUsFormat    = "{0:HH:mm:ss.ffffff}";
    public const string DefaultTimeToTicksFormat = "{0:HH:mm:ss.fffffff}";

    public const string DefaultYyyyMMddOnly        = "{0:yyyy-MM-dd}";
    public const string DefaultYyyyMMddToSecFormat = "{0:yyyy-MM-ddTHH:mm:ss.FFFFFFFK}";
    public const string DefaultYyyyMMddToMsFormat  = "{0:yyyy-MMd-dTHH:mm:ss.fff}";
    public const string DefaultYyyyMMddToUsFormat  = "{0:yyyy-MM-ddTHH:mm:ss.ffffff}";

    public const string DefaultLogInnerDblQtReplacementOpenChars  = ""; //  considered  "\u201C";  “
    public const string DefaultLogInnerDblQtReplacementCloseChars = ""; //  considered  "\u201D";  ”
    
    public const int DefaultInstanceTrackingDebuggerBreakOnRevisitCount = 64; 
    public const int DefaultInstanceTrackingThrowExceededRevisitCount = 65; 

    public static readonly string[] DefaultLogSuppressNames = [
        "System"
      , "FortitudeCommon.Types.StringsOfPower.Forge"
    ];

    public StyleOptionsValue(StringStyle style) => this.style = style;
    public StyleOptionsValue(StyleOptions defaultOptions) => fallbackOptions = defaultOptions;

    public StyleOptionsValue(StringStyle style, int indentSize = 2, char indentChar = ' '
      , bool byteSequenceToBase64 = true, bool disableCircularRefCheck = false, bool charSArraysAsString = false)
    {
        this.style                   = style;
        this.indentChar              = indentChar;
        this.indentSize              = indentSize;
        this.byteSequenceToBase64    = byteSequenceToBase64;
        this.disableCircularRefCheck = disableCircularRefCheck;
        this.charSArraysAsString     = charSArraysAsString;
    }

    private bool?   wrapValuesInQuotes;
    private bool?   byteArrayWritesBase64String;
    private bool?   charArrayWritesCharCollection;
    private string? falseString;
    private string? trueString;
    private string? nullString;
    private bool?   enumDefaultAsNumber;
    private bool?   ignoreEmptyCollection;
    private bool?   emptyCollectionWritesNull;
    private bool?   onNullWriteEmpty;
    private string? mainItemSeparator;
    private string? altItemSeparator;
    private string? mainItemPadding;
    private string? altItemPadding;
    
    private string? mainFieldSeparator;
    private string? altFieldSeparator;
    private string? mainFieldPadding;
    private string? altFieldPadding;

    private EncodingType? graphEncoderType;
    private EncodingType? stringEncoderType;

    private Func<IJsonFormattingOptions, IEncodingTransfer>? sourceEncodingTransferResolver;
    
    private InputClassFlags? instanceMarkingIncludeInputClassesContents;
    private InputClassFlags? instancesTrackingIncludeInputClasses;

    private char?     indentChar;
    private int?      indentSize;
    private bool?     byteSequenceToBase64;
    private bool?     disableCircularRefCheck;
    private bool?     charSArraysAsString;
    private bool?     circularRefUsesRefEquals;
    private bool?     instanceMarkingMarkInstanceIdOnFirstVisit;
    private bool?     instanceMarkingMarkVirtualMemoryAddress;
    private bool?     instanceMarkingMarkRevisitCount;
    private bool?     instanceMarkingWrapInstanceIdInQuotes;
    private bool?     instanceMarkingWrapInstanceInfoFieldNamesInQuotes;
    private bool?     instanceMarkingDisabled;
    private bool?     instanceTrackingDisabled;
    private int?      instancesTrackingDebuggerBreakOnRevisitCount;
    private int?      instancesTrackingThrowOnRevisitCount;
    private bool?     asStringAlwaysWritesAsCompact;
    private bool?     asStringSeparateRestartedIndentation;
    private string?   newLineStyle;
    private int?      prettyCollectionsColumnCountWrap;
    private int?      defaultGraphMaxDepth;
    private string?   customDateTimeFormatString;
    private string?   customTimeFormatString;
    private string?   dateOnlyAsStringFormatString;
    private string?   dateTimeYyyyMMddTossFormat;
    private string?   dateTimeYyyyMMddTomsFormat;
    private string?   dateTimeYyyyMMddTousFormat;
    private string?   timeHHmmssFormat;
    private string?   timeHHmmssToMsFormat;
    private string?   timeHHmmssToUsFormat;
    private string?   timeHHmmssToTicksFormat;
    private bool?     writeKeyValuePairsAsCollection;
    private Range[]?  unicodeEscapingRanges;
    private Range[]?  exemptEscapingRanges;
    private string[]? logSuppressDisplayTypeNames;
    private string[]? logSuppressDisplayCollectionNames;
    private string[]? logSuppressDisplayCollectionElementNames;

    private FormatFlags contextContentHandlingFlags;
    private StringStyle?         style;
    private DateTimeStyleFormat? dateTimeFormat;
    private TimeStyleFormat?     timeFormat;

    private JsonEncodingTransferType?    jsonEncodingTransferType;
    private CollectionPrettyStyleFormat? prettyCollectionStyle;

    private (Range, JsonEscapeType, Func<Rune, string>)[]? cachedMappingFactoryRanges;

    private string? logInnerDoubleQuoteOpenReplacement;
    private string? logInnerDoubleQuoteCloseReplacement;

    public StyleOptions? MyObjInstance
    {
        get => myOptionsObj;
        set => myOptionsObj = value;
    }

    public StyleOptions MyObjInstanceOrCreate(IRecycler recycler) =>
        myOptionsObj ??= recycler.Borrow<StyleOptions>().Initialize(this);

    public StringStyle Style
    {
        readonly get => style ?? fallbackOptions?.Values.Style ?? StringStyle.CompactLog;
        set => style = value;
    }

    public FormatFlags CurrentContextContentHandling
    {
        get => contextContentHandlingFlags;
        set => contextContentHandlingFlags = value;
    }

    public bool IsSame(FormatFlags callerRequestedHandling) =>
        ((callerRequestedHandling & EncodingMask) == 0
      || (contextContentHandlingFlags & EncodingMask) == (callerRequestedHandling & EncodingMask))
     && ((callerRequestedHandling & LayoutMask) == 0
      || (contextContentHandlingFlags & LayoutMask) == (callerRequestedHandling & LayoutMask));

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

    public int IndentLevel { get; set; }

    public int IndentRepeat(int level) => level * IndentSize;

    public string LogInnerDoubleQuoteOpenReplacement
    {
        readonly get => logInnerDoubleQuoteOpenReplacement
                     ?? fallbackOptions?.Values.LogInnerDoubleQuoteOpenReplacement
                     ?? DefaultLogInnerDblQtReplacementOpenChars;
        set => logInnerDoubleQuoteOpenReplacement = value;
    }

    public string LogInnerDoubleQuoteCloseReplacement
    {
        readonly get => logInnerDoubleQuoteCloseReplacement
                     ?? fallbackOptions?.Values.LogInnerDoubleQuoteCloseReplacement
                     ?? DefaultLogInnerDblQtReplacementCloseChars;
        set => logInnerDoubleQuoteCloseReplacement = value;
    }

    public string MainItemSeparator
    {
        readonly get => mainItemSeparator ?? fallbackOptions?.Values.MainItemSeparator ?? IFormattingOptions.Cma;
        set => mainItemSeparator = value;
    }

    public string AlternateItemSeparator
    {
        readonly get => altItemSeparator ?? fallbackOptions?.Values.AlternateItemSeparator ?? IFormattingOptions.Spc;
        set => altItemSeparator = value;
    }

    public string MainItemPadding
    {
        readonly get => mainItemPadding ?? fallbackOptions?.Values.AlternateItemSeparator
         ?? Style switch
            {
              StringStyle.CompactJson => IFormattingOptions.Empty
              , StringStyle.CompactLog => IFormattingOptions.Spc
              , StringStyle.PrettyJson => IFormattingOptions.Spc
              , StringStyle.PrettyLog => IFormattingOptions.Spc
              , _                      => Style.IsCompact() & Style.IsJson() ? IFormattingOptions.Empty : IFormattingOptions.Spc
            };
        set => mainItemPadding = value;
    }

    public string AlternateItemPadding
    {
        readonly get => altItemPadding ?? fallbackOptions?.Values.AlternateItemSeparator
         ?? Style switch
            {
                StringStyle.CompactJson => IFormattingOptions.Empty
              , StringStyle.CompactLog  => IFormattingOptions.Spc
              , StringStyle.PrettyJson  => IFormattingOptions.Spc
              , StringStyle.PrettyLog   => IFormattingOptions.Spc
              , _                       => Style.IsCompact() & Style.IsJson() ? IFormattingOptions.Empty : IFormattingOptions.Spc
            };
        set => altItemPadding = value;
    }

    public string MainFieldSeparator
    {
        readonly get => mainFieldSeparator ?? fallbackOptions?.Values.MainFieldSeparator
         ?? Style switch
            {
                StringStyle.CompactLog => IFormattingOptions.DefaultMainItemSeparator
              , _                      => IFormattingOptions.Cma
            };
        set => mainFieldSeparator = value;
    }

    public string AlternateFieldSeparator
    {
        readonly get => altFieldSeparator ?? fallbackOptions?.Values.AlternateFieldSeparator
         ?? Style switch
            {
                StringStyle.CompactLog => IFormattingOptions.Empty
              , StringStyle.PrettyJson => IFormattingOptions.Spc
              , StringStyle.PrettyLog  => IFormattingOptions.Spc
              , _                      => Style.IsCompact() & Style.IsJson() ? IFormattingOptions.Empty : IFormattingOptions.Spc
            };
        set => altFieldSeparator = value;
    }

    public string MainFieldPadding
    {
        readonly get => mainFieldPadding ?? fallbackOptions?.Values.AlternateFieldSeparator
         ?? Style switch
            {
              StringStyle.CompactJson => IFormattingOptions.Empty
              , StringStyle.PrettyJson => IFormattingOptions.Spc
              , StringStyle.PrettyLog => IFormattingOptions.Spc
              , _                      => Style.IsCompact() & Style.IsJson() ? IFormattingOptions.Empty : IFormattingOptions.Spc
            };
        set => mainFieldPadding = value;
    }

    public string AlternateFieldPadding
    {
        readonly get => altFieldPadding ?? fallbackOptions?.Values.AlternateFieldSeparator
         ?? Style switch
            {
                StringStyle.CompactJson => IFormattingOptions.Empty
              , StringStyle.PrettyJson  => IFormattingOptions.Spc
              , StringStyle.PrettyLog   => IFormattingOptions.Spc
              , _                       => Style.IsCompact() & Style.IsJson() ? IFormattingOptions.Empty : IFormattingOptions.Spc
            };
        set => altFieldPadding = value;
    }

    public bool NullWritesEmpty
    {
        readonly get => onNullWriteEmpty ?? fallbackOptions?.Values.NullWritesEmpty ?? false;
        set => onNullWriteEmpty = value;
    }

    public bool NullWritesNullString
    {
        readonly get => !onNullWriteEmpty ?? fallbackOptions?.Values.NullWritesNullString ?? true;
        set => onNullWriteEmpty = !value;
    }

    public bool EmptyCollectionWritesNull
    {
        readonly get => emptyCollectionWritesNull ?? fallbackOptions?.Values.EmptyCollectionWritesNull ?? true;
        set => emptyCollectionWritesNull = value;
    }

    public bool IgnoreEmptyCollection
    {
        readonly get => ignoreEmptyCollection ?? fallbackOptions?.Values.ignoreEmptyCollection ?? false;
        set => ignoreEmptyCollection = value;
    }

    public string True
    {
        readonly get => trueString ?? fallbackOptions?.Values.False ?? IFormattingOptions.DefaultTrueString;
        set => trueString = value;
    }

    public string False
    {
        readonly get => falseString ?? fallbackOptions?.Values.False ?? IFormattingOptions.DefaultFalseString;
        set => falseString = value;
    }
    
    public bool EnumsDefaultAsNumber
    {
        readonly get => enumDefaultAsNumber ?? fallbackOptions?.Values.EnumsDefaultAsNumber ?? IFormattingOptions.DefaultEnumAsNumber;
        set => enumDefaultAsNumber = value;
    }

    public bool CharBufferWritesAsCharCollection
    {
        readonly get => charArrayWritesCharCollection ?? !fallbackOptions?.Values.CharBufferWritesAsCharCollection ?? false;
        set => charArrayWritesCharCollection = value;
    }

    public bool ByteArrayWritesBase64String
    {
        readonly get => byteArrayWritesBase64String ?? fallbackOptions?.Values.ByteArrayWritesBase64String ?? true;
        set => byteArrayWritesBase64String = value;
    }

    public bool WrapValuesInQuotes
    {
        readonly get => wrapValuesInQuotes ?? fallbackOptions?.Values.WrapValuesInQuotes ?? false;
        set => wrapValuesInQuotes = value;
    }

    public Func<IJsonFormattingOptions, IEncodingTransfer> SourceEncodingTransfer
    {
        get =>
            sourceEncodingTransferResolver
                ??= fallbackOptions?.Values.SourceEncodingTransfer ?? DefaultEncodingTransferSelectorFactory;
        set => sourceEncodingTransferResolver = value;
    }

    public static Func<IJsonFormattingOptions, IEncodingTransfer> DefaultEncodingTransferSelectorFactory
    {
        get
        {
            return jsFmtOpts =>
            {
                if (jsFmtOpts is StyleOptionsValue styleOptionsValue)
                {
                    if (styleOptionsValue.Style.IsNotJson() || styleOptionsValue.Style.IsLog())
                    {
                        return Recycler.ThreadStaticRecycler.Borrow<PassThroughEncodingTransfer>();
                    }
                }
                if (jsFmtOpts is StyleOptions styleOptions)
                {
                    if (styleOptions.Style.IsNotJson() || styleOptions.Style.IsLog())
                    {
                        return Recycler.ThreadStaticRecycler.Borrow<PassThroughEncodingTransfer>();
                    }
                }
                return JsonFormattingOptions.DefaultEncodingTransferSelectorFactory(jsFmtOpts);
            };
        }
    }

    public JsonEncodingTransferType JsonEncodingTransferType
    {
        readonly get => jsonEncodingTransferType ?? fallbackOptions?.Values.JsonEncodingTransferType ?? JsonEncodingTransferType.Default;
        set
        {
            if (value == jsonEncodingTransferType) return;
            jsonEncodingTransferType = value;
            if (jsonEncodingTransferType != JsonEncodingTransferType.CustomEncodingTransfer)
            {
                var encodingTypeLookup = (int)jsonEncodingTransferType;
                if (encodingTypeLookup < JsonFormattingOptions.DefaultJsUnicodeEscapeRange.Length)
                {
                    UnicodeEscapingRanges = JsonFormattingOptions.DefaultJsUnicodeEscapeRange[encodingTypeLookup];
                }
            }
            else { UnicodeEscapingRanges = []; }
        }
    }

    public (Range, JsonEscapeType, Func<Rune, string>)[] CachedMappingFactoryRanges
    {
        readonly get
        {
            var checkMappingFactoryRanges = cachedMappingFactoryRanges ?? fallbackOptions?.Values.CachedMappingFactoryRanges;
            if (checkMappingFactoryRanges != null) { return checkMappingFactoryRanges; }
            switch (jsonEncodingTransferType)
            {
                case JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly:
                    return
                    [
                        (new Range(Index.Start, new Index(128)), JsonEscapeType.AsciiEscape
                       , JsonFormattingOptions.DefaultAsciiBackSlashEscapeMapping)
                    ];
                default:
                    return
                    [
                        (new Range(Index.Start, new Index(128)), JsonEscapeType.UnicodeEscape
                       , JsonFormattingOptions.DefaultAsciiBackSlashEscapeMapping)
                    ];
            }
        }
        set => cachedMappingFactoryRanges = value;
    }

    public Range[] ExemptEscapingRanges
    {
        readonly get => exemptEscapingRanges ?? fallbackOptions?.Values.ExemptEscapingRanges ?? [];
        set
        {
            if (exemptEscapingRanges != null && value.SequenceEqual(exemptEscapingRanges)) return;
            exemptEscapingRanges = value;
        }
    }

    public Range[] UnicodeEscapingRanges
    {
        readonly get =>
            unicodeEscapingRanges ?? fallbackOptions?.Values.UnicodeEscapingRanges ??
            JsonFormattingOptions.DefaultJsUnicodeEscapeRange[(int)JsonEncodingTransferType];
        set
        {
            if (unicodeEscapingRanges != null && value.SequenceEqual(unicodeEscapingRanges)) return;
            unicodeEscapingRanges = value;
        }
    }

    public EncodingType GraphEncoderType
    {
        readonly get => graphEncoderType ?? fallbackOptions?.Values.GraphEncoderType ?? EncodingType.PassThrough;
        set => graphEncoderType = value;
    }

    public EncodingType StringEncoderType
    {
        readonly get => stringEncoderType ?? fallbackOptions?.Values.GraphEncoderType ?? EncodingType.PassThrough;
        set => stringEncoderType = value;
    }

    public string NullString
    {
        readonly get => nullString ?? fallbackOptions?.Values.False ?? IFormattingOptions.DefaultNullString;
        set => nullString = value;
    }

    public string NewLineStyle
    {
        readonly get => newLineStyle ?? fallbackOptions?.Values.NewLineStyle ?? Environment.NewLine;
        set => newLineStyle = value;
    }

    public TimeStyleFormat TimeFormat
    {
        readonly get => timeFormat ?? fallbackOptions?.Values.TimeFormat ?? TimeStyleFormat.Default;
        set => timeFormat = value;
    }

    public string TimeAsStringFormatString
    {
        get
        {
            if (customTimeFormatString.IsNotNullOrEmpty()) { return customTimeFormatString; }
            return TimeFormat switch
                   {
                       TimeStyleFormat.Default => DefaultTimeFormat

                     , StringHHmmss     => TimeStringHHmmssFormatString
                     , StringHHmmssToMs => TimeStringHHmmssToMsFormatString
                     , StringHHmmssToUs => TimeStringHHmmssToUsFormatString
                     , _                => TimeStringHHmmssToTicksFormatString
                   };
        }
        set => customTimeFormatString = value;
    }

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

    public string DateTimeAsStringFormatString
    {
        get
        {
            if (customDateTimeFormatString != null) { return customDateTimeFormatString; }
            if (fallbackOptions?.Values.DateTimeAsStringFormatString != null && fallbackOptions?.Values.DateTimeFormat == DateTimeFormat)
            {
                return fallbackOptions!.Values.DateTimeAsStringFormatString;
            }
            return DateTimeFormat switch
                   {
                       StringYyyyMMddOnly => DateOnlyAsStringFormatString
                     , StringYyyyMMddToss => DateTimeStringYyyyMMddToSecFormatString
                     , StringYyyyMMddToms => DateTimeStringYyyyMMddToMsFormatString
                     , _                  => DateTimeStringYyyyMMddToUsFormatString
                   };
        }
        set => customDateTimeFormatString = value;
    }

    public string DateOnlyAsStringFormatString
    {
        readonly get => dateOnlyAsStringFormatString ?? fallbackOptions?.Values.DateOnlyAsStringFormatString ?? DefaultYyyyMMddOnly;
        set => dateOnlyAsStringFormatString = value;
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

    public CollectionPrettyStyleFormat PrettyCollectionStyle
    {
        readonly get => prettyCollectionStyle ?? fallbackOptions?.Values.PrettyCollectionStyle ?? CollectionPrettyStyleFormat.OneElementOnEveryLine;
        set => prettyCollectionStyle = value;
    }

    public int PrettyCollectionsColumnContentWidthWrap
    {
        readonly get => prettyCollectionsColumnCountWrap ?? fallbackOptions?.Values.PrettyCollectionsColumnContentWidthWrap ?? 120;
        set => prettyCollectionsColumnCountWrap = value;
    }

    public string[] LogSuppressDisplayTypeNames
    {
        readonly get => logSuppressDisplayTypeNames ??
                        fallbackOptions?.Values.LogSuppressDisplayTypeNames ?? DefaultLogSuppressNames;
        set => logSuppressDisplayTypeNames = value;
    }

    public string[] LogSuppressDisplayCollectionElementNames
    {
        readonly get => logSuppressDisplayCollectionElementNames ??
                        fallbackOptions?.Values.LogSuppressDisplayCollectionElementNames ?? DefaultLogSuppressNames;
        set => logSuppressDisplayCollectionElementNames = value;
    }

    public string[] LogSuppressDisplayCollectionNames
    {
        readonly get => logSuppressDisplayCollectionNames ??
                        fallbackOptions?.Values.LogSuppressDisplayCollectionNames ?? DefaultLogSuppressNames;
        set => logSuppressDisplayCollectionNames = value;
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

    public bool InstanceMarkingAlwaysMarkInstanceIds
    {
        readonly get => instanceMarkingMarkInstanceIdOnFirstVisit ?? fallbackOptions?.Values.InstanceMarkingAlwaysMarkInstanceIds ?? false;
        set => instanceMarkingMarkInstanceIdOnFirstVisit = value;
    }

    public bool InstanceMarkingMarkVirtualMemoryAddress
    {
        readonly get => instanceMarkingMarkVirtualMemoryAddress ?? fallbackOptions?.Values.InstanceMarkingMarkVirtualMemoryAddress ?? false;
        set => instanceMarkingMarkVirtualMemoryAddress = value;
    }

    public bool InstanceMarkingMarkRevisitCount
    {
        readonly get => instanceMarkingMarkRevisitCount ?? fallbackOptions?.Values.InstanceMarkingMarkRevisitCount ?? false;
        set => instanceMarkingMarkRevisitCount = value;
    }

    public bool InstanceMarkingIncludeSpanFormattableContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsSpanFormattableClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeSpanFormattableContents ?? false;
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(SpanFormattableClass, value);
    }

    public bool InstanceMarkingIncludeStringContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsStringClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeStringContents ?? false;
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(StringClass, value);
    }

    public bool InstanceMarkingIncludeCharArrayContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsCharArrayClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeCharArrayContents ?? false;
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(CharArrayClass, value);
    }

    public bool InstanceMarkingIncludeCharSequenceContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsCharSequenceClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeCharSequenceContents ?? false;
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(CharSequenceClass, value);
    }

    public bool InstanceMarkingIncludeStringBuilderContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsStringBuilderClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeStringBuilderContents ?? false;
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(StringBuilderClass, value);
    }

    public bool InstanceMarkingIncludeObjectToStringContents
    {
        readonly get => InstanceMarkingIncludeAllContentOnlyContents || (instanceMarkingIncludeInputClassesContents.IsObjectToStringClassActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeObjectToStringContents ?? false);
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(ObjectToStringClass, value);
    }

    public bool InstanceMarkingIncludeAllContentOnlyContents
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsAllInputClassesActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingIncludeAllContentOnlyContents ?? false;
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(AllInputClasses, value);
    }

    public bool InstanceMarkingAsStringIndependentNumbering
    {
        readonly get => instanceMarkingIncludeInputClassesContents.IsAllInputClassesActive() 
                     ?? fallbackOptions?.Values.InstanceMarkingAsStringIndependentNumbering ?? 
                        InstanceTrackingAllAsStringHaveLocalTracking && Style.IsJson();
        
        set => instanceMarkingIncludeInputClassesContents = instanceMarkingIncludeInputClassesContents.SetTo(AsStringClasses, value);
    }

    public bool InstanceMarkingWrapInstanceIdInQuotes
    {
        readonly get => instanceMarkingWrapInstanceIdInQuotes ?? fallbackOptions?.Values.InstanceMarkingWrapInstanceIdInQuotes ?? Style.IsJson();
        set => instanceMarkingWrapInstanceIdInQuotes = value;
    }

    public bool InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
    {
        readonly get => instanceMarkingWrapInstanceInfoFieldNamesInQuotes ?? fallbackOptions?.Values.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes ?? Style.IsJson();
        set => instanceMarkingWrapInstanceInfoFieldNamesInQuotes = value;
    }

    public bool InstanceMarkingDisabled
    {
        readonly get => instanceMarkingDisabled ?? fallbackOptions?.Values.InstanceMarkingDisabled ?? false;
        set => instanceMarkingDisabled = value;
    }

    public bool InstanceTrackingDisabled
    {
        readonly get => instanceTrackingDisabled ?? fallbackOptions?.Values.InstanceTrackingDisabled ?? false;
        set => instanceTrackingDisabled = value;
    }

    public bool InstanceTrackingIncludeSpanFormattableClasses
    {
        readonly get => instancesTrackingIncludeInputClasses.IsSpanFormattableClassActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeSpanFormattableClasses ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(SpanFormattableClass, value);
    }

    public bool InstanceTrackingIncludeStringInstances
    {
        readonly get => instancesTrackingIncludeInputClasses.IsStringClassActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeStringInstances ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(StringClass, value);
    }

    public bool InstanceTrackingIncludeCharArrayInstances
    {
        readonly get => instancesTrackingIncludeInputClasses.IsCharArrayClassActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeCharArrayInstances ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(CharArrayClass, value);
    }

    public bool InstanceTrackingIncludeCharSequenceInstances
    {
        readonly get => instancesTrackingIncludeInputClasses.IsCharSequenceClassActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeCharSequenceInstances ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(CharSequenceClass, value);
    }

    public bool InstanceTrackingIncludeStringBuilderInstances
    {
        readonly get => instancesTrackingIncludeInputClasses.IsStringBuilderClassActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeStringBuilderInstances ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(StringBuilderClass, value);
    }

    public bool InstanceTrackingIncludeAllExemptClassInstances
    {
        readonly get => instancesTrackingIncludeInputClasses.IsAllInputClassesActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingIncludeStringInstances ?? false;
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(AllInputClasses, value);
    }

    public bool InstanceTrackingAllAsStringHaveLocalTracking
    {
        readonly get => instancesTrackingIncludeInputClasses.IsAsStringClassesActive() 
                     ?? fallbackOptions?.Values.InstanceTrackingAllAsStringHaveLocalTracking ?? !Style.IsLog();
        
        set => instancesTrackingIncludeInputClasses = instancesTrackingIncludeInputClasses.SetTo(AsStringClasses, value);
    }

    public int InstancesTrackingDebuggerBreakOnRevisitCount
    {
        readonly get => 
            instancesTrackingDebuggerBreakOnRevisitCount 
         ?? fallbackOptions?.Values.InstancesTrackingDebuggerBreakOnRevisitCount 
         ?? DefaultInstanceTrackingDebuggerBreakOnRevisitCount;
        set => instancesTrackingDebuggerBreakOnRevisitCount = value;
    }

    public int InstancesTrackingThrowOnRevisitCount
    {
        readonly get => 
            instancesTrackingThrowOnRevisitCount 
         ?? fallbackOptions?.Values.InstancesTrackingThrowOnRevisitCount 
         ?? DefaultInstanceTrackingThrowExceededRevisitCount;
        set => instancesTrackingThrowOnRevisitCount = value;
    }

    public bool AsStringAlwaysWritesAsCompact
    {
        readonly get => asStringAlwaysWritesAsCompact ?? fallbackOptions?.Values.AsStringAlwaysWritesAsCompact ?? !Style.IsLog();
        set => asStringAlwaysWritesAsCompact = value;
    }

    public bool AsStringSeparateRestartedIndentation
    {
        readonly get => asStringSeparateRestartedIndentation ?? fallbackOptions?.Values.AsStringSeparateRestartedIndentation ?? !Style.IsLog();
        set => asStringSeparateRestartedIndentation = value;
    }

    public StyleOptions? DefaultOptions
    {
        get => fallbackOptions;
        set => fallbackOptions = value;
    }

    object ICloneable.Clone() => Clone();

    public StyleOptionsValue Clone() => this;

    IFormattingOptions ICloneable<IFormattingOptions>.Clone()
    {
        return this;
    }
}

public class StyleOptions : ExplicitRecyclableObject, IJsonFormattingOptions, ITransferState<StyleOptions>
{
    private static int globalInstanceId;

    protected int               InstanceId = Interlocked.Increment(ref globalInstanceId);
    
    private   StyleOptionsValue values;

    private ICustomStringFormatter? formatter;

    public StyleOptions() : this(new StyleOptionsValue()) { }

    public StyleOptions(StringStyle style)
    {
        values = new StyleOptionsValue()
        {
            Style = style, MyObjInstance = this
        };
    }

    public StyleOptions Initialize(StyleOptionsValue styleOptions)
    {
        values = styleOptions;
        return this;
    }

    public StyleOptions Initialize(StyleOptions styleOptions)
    {
        values               = styleOptions.Values;
        values.MyObjInstance = this;
        formatter            = styleOptions.formatter;
        return this;
    }

    public StyleOptions(StyleOptionsValue initialValues)
    {
        values               = initialValues;
        values.MyObjInstance = this;
    }

    public StyleOptionsValue Values
    {
        [DebuggerStepThrough]
        get => values;
        set
        {
            values = value;
            if (formatter != null)
            {
                if (value.Style.IsJson() && formatter.FormattingStyle.IsNotJson())
                {
                    formatter.DecrementRefCount();
                    formatter = null;
                }
                else if (value.Style.IsLog() && formatter.FormattingStyle.IsNotNone())
                {
                    formatter.DecrementRefCount();
                    formatter = null;
                }
            }

            value.MyObjInstance = this;
        }
    }

    public StringStyle Style
    {
        [DebuggerStepThrough]
        get => values.Style;
        set
        {
            if (value == values.Style) return;
            if (formatter != null)
            {
                formatter.DecrementRefCount();
                formatter = null;
            }
            values.Style = value;
        }
    }

    public FormatFlags CurrentContextContentHandling
    {
        [DebuggerStepThrough]
        get => values.CurrentContextContentHandling;
        set => values.CurrentContextContentHandling = value;
    }

    public bool IsSame(FormatFlags callerRequestedHandling) => values.IsSame(callerRequestedHandling);

    public char IndentChar
    {
        [DebuggerStepThrough]
        get => values.IndentChar;
        set => values.IndentChar = value;
    }

    public int IndentSize
    {
        [DebuggerStepThrough]
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

    public int IndentLevel
    {
        [DebuggerStepThrough]
        get => Math.Max(0, values.IndentLevel);
        set => values.IndentLevel = value;
    }

    
    [DebuggerStepThrough]
    public int IndentRepeat(int indentLevel) => values.IndentRepeat(indentLevel);

    public string LogInnerDoubleQuoteOpenReplacement
    {
        [DebuggerStepThrough]
        get => values.LogInnerDoubleQuoteOpenReplacement;
        set => values.LogInnerDoubleQuoteOpenReplacement = value;
    }

    public string LogInnerDoubleQuoteCloseReplacement
    {
        [DebuggerStepThrough]
        get => values.LogInnerDoubleQuoteCloseReplacement;
        set => values.LogInnerDoubleQuoteCloseReplacement = value;
    }

    public string MainItemSeparator
    {
        [DebuggerStepThrough]
        get => values.MainItemSeparator;
        set => values.MainItemSeparator = value;
    }

    public string AlternateItemSeparator
    {
        [DebuggerStepThrough]
        get => values.AlternateItemSeparator;
        set => values.AlternateItemSeparator = value;
    }

    public string MainItemPadding
    {
        [DebuggerStepThrough]
        get => values.MainItemPadding;
        set => values.MainItemPadding = value;
    }

    public string AlternateItemPadding
    {
        [DebuggerStepThrough]
        get => values.AlternateItemPadding;
        set => values.AlternateItemPadding = value;
    }

    public string MainFieldSeparator
    {
        [DebuggerStepThrough]
        get => values.MainFieldSeparator;
        set => values.MainFieldSeparator = value;
    }

    public string AlternateFieldSeparator
    {
        [DebuggerStepThrough]
        get => values.AlternateFieldSeparator;
        set => values.AlternateFieldSeparator = value;
    }

    public string MainFieldPadding
    {
        [DebuggerStepThrough]
        get => values.MainFieldPadding;
        set => values.MainFieldPadding = value;
    }

    public string AlternateFieldPadding
    {
        [DebuggerStepThrough]
        get => values.AlternateFieldPadding;
        set => values.AlternateFieldPadding = value;
    }

    public bool NullWritesEmpty
    {
        [DebuggerStepThrough]
        get => values.NullWritesEmpty;
        set => values.NullWritesEmpty = value;
    }

    public bool NullWritesNullString
    {
        [DebuggerStepThrough]
        get => values.NullWritesNullString;
        set => values.NullWritesNullString = value;
    }

    public bool EmptyCollectionWritesNull
    {
        [DebuggerStepThrough]
        get => values.EmptyCollectionWritesNull;
        set => values.EmptyCollectionWritesNull = value;
    }
    
    public bool EnumsDefaultAsNumber
    {
        [DebuggerStepThrough]
        get => values.EnumsDefaultAsNumber;
        set => values.EnumsDefaultAsNumber = value;
    }

    public bool IgnoreEmptyCollection
    {
        [DebuggerStepThrough]
        get => values.EmptyCollectionWritesNull;
        set => values.EmptyCollectionWritesNull = value;
    }

    public string True
    {
        [DebuggerStepThrough]
        get => values.True;
        set => values.True = value;
    }

    public string False
    {
        [DebuggerStepThrough]
        get => values.False;
        set => values.False = value;
    }

    public bool CharBufferWritesAsCharCollection
    {
        [DebuggerStepThrough]
        get => values.CharBufferWritesAsCharCollection;
        set => values.CharBufferWritesAsCharCollection = value;
    }

    public bool ByteArrayWritesBase64String
    {
        [DebuggerStepThrough]
        get => values.ByteArrayWritesBase64String;
        set => values.ByteArrayWritesBase64String = value;
    }

    public bool WrapValuesInQuotes
    {
        [DebuggerStepThrough]
        get => values.WrapValuesInQuotes;
        set => values.WrapValuesInQuotes = value;
    }

    public Func<IJsonFormattingOptions, IEncodingTransfer> SourceEncodingTransfer
    {
        [DebuggerStepThrough]
        get => values.SourceEncodingTransfer;
        set => values.SourceEncodingTransfer = value;
    }

    public JsonEncodingTransferType JsonEncodingTransferType
    {
        [DebuggerStepThrough]
        get => values.JsonEncodingTransferType;
        set => values.JsonEncodingTransferType = value;
    }

    public (Range, JsonEscapeType, Func<Rune, string>)[] CachedMappingFactoryRanges
    {
        [DebuggerStepThrough]
        get => values.CachedMappingFactoryRanges;
        set => values.CachedMappingFactoryRanges = value;
    }

    public Range[] ExemptEscapingRanges
    {
        [DebuggerStepThrough]
        get => values.ExemptEscapingRanges;
        set => values.ExemptEscapingRanges = value;
    }

    public Range[] UnicodeEscapingRanges
    {
        [DebuggerStepThrough]
        get => values.UnicodeEscapingRanges;
        set => values.UnicodeEscapingRanges = value;
    }

    public EncodingType GraphEncoderType
    {
        [DebuggerStepThrough]
        get => values.GraphEncoderType;
        set => values.GraphEncoderType = value;
    }

    public EncodingType StringEncoder
    {
        [DebuggerStepThrough]
        get => values.StringEncoderType;
        set => values.StringEncoderType = value;
    }

    public ICustomStringFormatter? Formatter 
    {
        [DebuggerStepThrough]
        get => formatter;
        set
        {
            if (ReferenceEquals(value, formatter)) return;
            formatter?.DecrementRefCount();
            formatter = value;
            value?.IncrementRefCount();
        }
    }

    public void IfExistsIncrementFormatterRefCount() => formatter?.IncrementRefCount();

    public IStyledTypeFormatting StyledTypeFormatter => (IStyledTypeFormatting)Formatter!;

    public string NewLineStyle
    {
        [DebuggerStepThrough]
        get => values.NewLineStyle;
        set => values.NewLineStyle = value;
    }

    public string NullString
    {
        [DebuggerStepThrough]
        get => values.NullString;
        set => values.NullString = value;
    }

    public TimeStyleFormat TimeFormat
    {
        [DebuggerStepThrough]
        get => values.TimeFormat;
        set => values.TimeFormat = value;
    }

    public string TimeAsStringFormatString
    {
        [DebuggerStepThrough]
        get => values.TimeAsStringFormatString;
        set => values.TimeAsStringFormatString = value;
    }

    public string TimeStringHHmmssFormatString
    {
        [DebuggerStepThrough]
        get => values.TimeStringHHmmssFormatString;
        set => values.TimeStringHHmmssFormatString = value;
    }

    public string TimeStringHHmmssToMsFormatString
    {
        [DebuggerStepThrough]
        get => values.TimeStringHHmmssToMsFormatString;
        set => values.TimeStringHHmmssToMsFormatString = value;
    }

    public string TimeStringHHmmssToUsFormatString
    {
        [DebuggerStepThrough]
        get => values.TimeStringHHmmssToUsFormatString;
        set => values.TimeStringHHmmssToUsFormatString = value;
    }

    public string TimeStringHHmmssToTicksFormatString
    {
        [DebuggerStepThrough]
        get => values.TimeStringHHmmssToTicksFormatString;
        set => values.TimeStringHHmmssToTicksFormatString = value;
    }

    public DateTimeStyleFormat DateDateTimeFormat
    {
        [DebuggerStepThrough]
        get => values.DateTimeFormat;
        set => values.DateTimeFormat = value;
    }

    public bool DateTimeIsNumber
    {
        [DebuggerStepThrough]
        get => values.DateTimeIsNumber;
    }

    public bool DateTimeIsString
    {
        
        [DebuggerStepThrough]
        get => values.DateTimeIsString;
    }

    public long DateTimeTicksToNumberPrecision(long timeStampTicks) => values.DateTimeTicksToNumberPrecision(timeStampTicks);

    public string DateTimeAsStringFormatString
    {
        [DebuggerStepThrough]
        get => values.DateTimeAsStringFormatString;
        set => values.DateTimeAsStringFormatString = value;
    }

    public string DateOnlyAsStringFormatString
    {
        [DebuggerStepThrough]
        get => values.DateOnlyAsStringFormatString;
        set => values.DateOnlyAsStringFormatString = value;
    }

    public string DateTimeStringYyyyMMddTossFormatString
    {
        [DebuggerStepThrough]
        get => values.DateTimeStringYyyyMMddToSecFormatString;
        set => values.DateTimeStringYyyyMMddToSecFormatString = value;
    }

    public string DateTimeStringYyyyMMddTomsFormatString
    {
        [DebuggerStepThrough]
        get => values.DateTimeStringYyyyMMddToMsFormatString;
        set => values.DateTimeStringYyyyMMddToMsFormatString = value;
    }

    public string DateTimeStringYyyyMMddTousFormatString
    {
        [DebuggerStepThrough]
        get => values.DateTimeStringYyyyMMddToUsFormatString;
        set => values.DateTimeStringYyyyMMddToUsFormatString = value;
    }

    public bool DisableCircularRefCheck
    {
        [DebuggerStepThrough]
        get => values.DisableCircularRefCheck;
        set => values.DisableCircularRefCheck = value;
    }

    public bool CharSArraysAsString
    {
        [DebuggerStepThrough]
        get => values.CharSArraysAsString;
        set => values.CharSArraysAsString = value;
    }

    public bool ByteSequenceToBase64
    {
        [DebuggerStepThrough]
        get => values.ByteSequenceToBase64;
        set => values.ByteSequenceToBase64 = value;
    }

    public bool WriteKeyValuePairsAsCollection
    {
        [DebuggerStepThrough]
        get => values.WriteKeyValuePairsAsCollection;
        set => values.WriteKeyValuePairsAsCollection = value;
    }

    public bool? CircularRefUsesRefEquals
    {
        [DebuggerStepThrough]
        get => values.CircularRefUsesRefEquals;
        set => values.CircularRefUsesRefEquals = value;
    }

    public StyleOptions? DefaultOptions
    {
        [DebuggerStepThrough]
        get => values.DefaultOptions;
        set => values.DefaultOptions = value;
    }

    public CollectionPrettyStyleFormat PrettyCollectionStyle
    {
        [DebuggerStepThrough]
        get => values.PrettyCollectionStyle;
        set => values.PrettyCollectionStyle = value;
    }

    public int PrettyCollectionsColumnContentWidthWrap
    {
        [DebuggerStepThrough]
        get => values.PrettyCollectionsColumnContentWidthWrap;
        set => values.PrettyCollectionsColumnContentWidthWrap = value;
    }

    public string[] LogSuppressDisplayTypeNames
    {
        [DebuggerStepThrough]
        get => values.LogSuppressDisplayTypeNames;
        set => values.LogSuppressDisplayTypeNames = value;
    }

    public string[] LogSuppressDisplayCollectionElementNames
    {
        [DebuggerStepThrough]
        get => values.LogSuppressDisplayCollectionElementNames;
        set => values.LogSuppressDisplayCollectionElementNames = value;
    }

    public string[] LogSuppressDisplayCollectionNames
    {
        [DebuggerStepThrough]
        get => values.LogSuppressDisplayCollectionNames;
        set => values.LogSuppressDisplayCollectionNames = value;
    }

    public int DefaultGraphMaxDepth
    {
        [DebuggerStepThrough]
        get => values.DefaultGraphMaxDepth;
        set => values.DefaultGraphMaxDepth = value;
    }

    public bool InstanceMarkingMarkInstanceIdOnFirstVisit
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingAlwaysMarkInstanceIds;
        set => values.InstanceMarkingAlwaysMarkInstanceIds = value;
    }

    public bool InstanceMarkingMarkVirtualMemoryAddress
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingMarkVirtualMemoryAddress;
        set => values.InstanceMarkingMarkVirtualMemoryAddress = value;
    }

    public bool InstanceMarkingMarkRevisitCount
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingMarkRevisitCount;
        set => values.InstanceMarkingMarkRevisitCount = value;
    }

    public bool InstanceMarkingIncludeSpanFormattableContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeSpanFormattableContents;
        set => values.InstanceMarkingIncludeSpanFormattableContents = value;
    }

    public bool InstanceMarkingIncludeStringContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeStringContents;
        set => values.InstanceMarkingIncludeStringContents = value;
    }

    public bool InstanceMarkingIncludeCharArrayContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeCharArrayContents;
        set => values.InstanceMarkingIncludeCharArrayContents = value;
    }

    public bool InstanceMarkingIncludeCharSequenceContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeCharSequenceContents;
        set => values.InstanceMarkingIncludeCharSequenceContents = value;
    }

    public bool InstanceMarkingIncludeStringBuilderContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeStringBuilderContents;
        set => values.InstanceMarkingIncludeStringBuilderContents = value;
    }

    public bool InstanceMarkingIncludeObjectToStringContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeObjectToStringContents;
        set => values.InstanceMarkingIncludeObjectToStringContents = value;
    }

    public bool InstanceMarkingIncludeAllContentOnlyContents
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingIncludeAllContentOnlyContents;
        set => values.InstanceMarkingIncludeAllContentOnlyContents = value;
    }

    public bool InstanceMarkingAsStringIndependentNumbering
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingAsStringIndependentNumbering;
        set => values.InstanceMarkingAsStringIndependentNumbering = value;
    }

    public bool InstanceMarkingDisabled
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingDisabled;
        set => values.InstanceMarkingDisabled = value;
    }

    public bool InstanceMarkingWrapInstanceIdInQuotes
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingWrapInstanceIdInQuotes;
        set => values.InstanceMarkingWrapInstanceIdInQuotes = value;
    }

    public bool InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
    {
        [DebuggerStepThrough]
        get => values.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes;
        set => values.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes = value;
    }

    public bool InstanceTrackingDisabled
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingDisabled;
        set => values.InstanceTrackingDisabled = value;
    }

    public bool InstanceTrackingIncludeSpanFormattableClasses
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeSpanFormattableClasses;
        set => values.InstanceTrackingIncludeSpanFormattableClasses = value;
    }

    public bool InstanceTrackingIncludeStringInstances
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeStringInstances;
        set => values.InstanceTrackingIncludeStringInstances = value;
    }

    public bool InstanceTrackingIncludeCharArrayInstances
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeCharArrayInstances;
        set => values.InstanceTrackingIncludeCharArrayInstances = value;
    }

    public bool InstanceTrackingIncludeCharSequenceInstances
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeCharSequenceInstances;
        set => values.InstanceTrackingIncludeCharSequenceInstances = value;
    }

    public bool InstanceTrackingIncludeStringBuilderInstances
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeStringBuilderInstances;
        set => values.InstanceTrackingIncludeStringBuilderInstances = value;
    }

    public bool InstanceTrackingIncludeAllExemptClassInstances
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingIncludeAllExemptClassInstances;
        set => values.InstanceTrackingIncludeAllExemptClassInstances = value;
    }

    public bool InstanceTrackingAllAsStringHaveLocalTracking
    {
        [DebuggerStepThrough]
        get => values.InstanceTrackingAllAsStringHaveLocalTracking;
        set => values.InstanceTrackingAllAsStringHaveLocalTracking = value;
    }

    public int InstancesTrackingDebuggerBreakOnRevisitCount
    {
        [DebuggerStepThrough]
        get => values.InstancesTrackingDebuggerBreakOnRevisitCount;
        set => values.InstancesTrackingDebuggerBreakOnRevisitCount = value;
    }

    public int InstancesTrackingThrowOnRevisitCount
    {
        [DebuggerStepThrough]
        get => values.InstancesTrackingThrowOnRevisitCount;
        set => values.InstancesTrackingThrowOnRevisitCount = value;
    }

    public bool AsStringAlwaysWritesAsCompact
    {
        [DebuggerStepThrough]
        get => values.AsStringAlwaysWritesAsCompact;
        set => values.AsStringAlwaysWritesAsCompact = value;
    }

    public bool AsStringSeparateRestartedIndentation
    {
        [DebuggerStepThrough]
        get => values.AsStringSeparateRestartedIndentation;
        set => values.AsStringSeparateRestartedIndentation = value;
    }

    object ICloneable.        Clone() => Clone();

    public IFormattingOptions Clone() => AlwaysRecycler.Borrow<StyleOptions>().CopyFrom(this);

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        if (source is StyleOptions styleOptions)
        {
            CopyFrom(styleOptions, copyMergeFlags);
        }
        return this;
    }

    public StyleOptions   CopyFrom(StyleOptions source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        values               = source.Values;
        values.MyObjInstance = this;
        Formatter            = source.Formatter;

        return this;
    }

    protected override void InheritedStateReset()
    {
        formatter?.DecrementRefCount();
        formatter = null!;
        base.InheritedStateReset();
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {Style}: {Style} }}";
}
