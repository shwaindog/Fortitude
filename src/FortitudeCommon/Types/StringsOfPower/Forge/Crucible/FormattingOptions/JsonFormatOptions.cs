using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

// ReSharper disable ClassNeverInstantiated.Global

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public enum JsonEncodingTransferType
{
    Default = 0
  , BkSlEscCtrlCharsDblQtAndBkSlOnly
  , BkSlEscCtrlCharsDblQtBkSlToEndOfLatin1
  , BkSlEscCtrlCharsDblQtBkSlToEndBmpChar
  , UniCdEscCtrlCharsDblQtOnly
  , UniCdEscCtrlCharsDblQtAndNonAscii
  , UniCdEscCtrlCharsDblQtBkSlToEndOfLatin1
  , UniCdEscCtrlCharsDblQtBkSlToEndBmpChar
  , CustomEncodingTransfer
}

public enum JsonEscapeType
{
    None
  , BackSlashEscape
  , UnicodeEscape
  , CustomRemapping
}

public interface IJsonFormattingOptions : IFormattingOptions
{
    public bool CharArrayWritesString { get; set; }

    public bool ByteArrayWritesBase64String { get; set; }

    public bool WrapValuesInQuotes { get; set; }

    public JsonEncodingTransferType JsonEncodingTransferType { get; set; }

    Func<IJsonFormattingOptions, IEncodingTransfer> SourceEncodingTransfer { get; set; }

    (Range, JsonEscapeType, Func<Rune, string>)[] CachedMappingFactoryRanges { get; set; }

    Range[] ExemptEscapingRanges { get; set; }

    Range[] UnicodeEscapingRanges { get; set; }

    bool DateTimeIsNumber { get; }
    
    bool DateTimeIsString { get; }

    public string DateTimeAsStringFormatString { get; set; }

    public string DateOnlyAsStringFormatString { get; set; }
    
    string TimeAsStringFormatString { get; set; }
    
    string NullStyle { get; }

    long DateTimeTicksToNumberPrecision(long timeStampTicks);

    private static readonly List<string> ExistingKeys = new();

    public static string CreateKey(Type encodingTransferType, (Range, JsonEscapeType, Func<Rune, string>)[] cacheRanges, Range[] exemptEscapingRanges
      , Range[] unicodeEscapingRanges)
    {
        Span<char> buildKey = stackalloc char[1024];
        buildKey.Append(encodingTransferType.Name);
        buildKey.Append('_');
        for (int i = 0; i < cacheRanges.Length; i++)
        {
            var cacheRange = cacheRanges[i];
            buildKey.AppendRange(cacheRange.Item1);
            buildKey.Append('-');
            buildKey.AppendEnum(cacheRange.Item2);
            buildKey.Append('_');
        }
        for (int i = 0; i < exemptEscapingRanges.Length; i++)
        {
            var exemptRange = exemptEscapingRanges[i];
            buildKey.Append("exmpt-");
            buildKey.AppendRange(exemptRange);
            buildKey.Append('_');
        }
        for (int i = 0; i < unicodeEscapingRanges.Length; i++)
        {
            var exemptRange = unicodeEscapingRanges[i];
            buildKey.Append("uniCdEsc-");
            buildKey.AppendRange(exemptRange);
            buildKey.Append('_');
        }
        var len = buildKey.PopulatedLength();
        buildKey = buildKey[..len];
        string? asString = null;
        for (int i = 0; i < ExistingKeys.Count; i++)
        {
            var existing = ExistingKeys[i];
            if (buildKey.SequenceMatches(existing))
            {
                asString = existing;
                break;
            }
        }
        if (asString == null)
        {
            asString = buildKey.ToString();
            ExistingKeys.Add(asString);
        }
        return asString;
    }

    private static readonly List<string> ExistingTableMappingKeys = new();

    public static string CreateMappingTableKey((Range, JsonEscapeType, Func<Rune, string>)[] cacheRanges)
    {
        Span<char> buildKey = stackalloc char[1024];
        for (int i = 0; i < cacheRanges.Length; i++)
        {
            var cacheRange = cacheRanges[i];
            buildKey.AppendRange(cacheRange.Item1);
            buildKey.Append('-');
            buildKey.AppendEnum(cacheRange.Item2);
            buildKey.Append('_');
        }
        var len = buildKey.PopulatedLength();
        buildKey = buildKey[..len];
        string? asString = null;
        for (int i = 0; i < ExistingTableMappingKeys.Count; i++)
        {
            var existing = ExistingTableMappingKeys[i];
            if (buildKey.SequenceMatches(existing))
            {
                asString = existing;
                break;
            }
        }
        if (asString == null)
        {
            asString = buildKey.ToString();
            ExistingTableMappingKeys.Add(asString);
        }
        return asString;
    }
}

public class JsonFormattingOptions : FormattingOptions, IJsonFormattingOptions
{
    public const string DefaultJsonDateTImeFormat = "yyyy-MM-ddTHH:mm:ss";
    public const string DefaultJsonDateOnlyFormat = "yyyy-MM-dd";
    public const string DefaultJsonTimeFormat = "HH:mm:ss.FFFFFFF";

    private string? jsonDateTImeFormat;
    private string? dateOnlyAsStringFormatString;
    private Range[] unicodeEscapingRanges = DefaultJsUnicodeEscapeRange[0];
    private bool    explicitlySetEncodingTransfer;

    //   Default = 0
    // , BkSlEscCtrlCharsDblQtAndBkSlOnly
    // , BkSlEscCtrlCharsDblQtBkSlToEndOfLatin1
    // , BkSlEscCtrlCharsDblQtBkSlToEndBmpChar
    // , UniCdEscCtrlCharsDblQtOnly
    // , UniCdEscCtrlCharsDblQtAndNonAscii
    // , UniCdEscCtrlCharsDblQtBkSlToEndOfLatin1
    // , UniCdEscCtrlCharsDblQtBkSlToEndBmpChar
    // , CustomEncodingTransfer

    #pragma warning disable CA2211
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public static Range[][] DefaultJsUnicodeEscapeRange =
        [
        [new Range(0, 34), new Range(128, CharExtensions.UnicodeCodePoints)] // Default
      , [new Range(0, 34), new Range(128, 161)] // BkSlEscCtrlCharsDblQtAndBkSlOnly - always unicode escape Console/ formatting Control Chars
      , [new Range(0, 34), new Range(128, 161)] // BkSlEscCtrlCharsDblQtBkSlToEndOfLatin1 -  always unicode escape Console/ formatting Control Chars
      , [new Range(0, 34), new Range(128, 161), new Range(0x10000, CharExtensions.UnicodeCodePoints)] // BkSlEscCtrlCharsDblQtBkSlToEndBmpChar
      , [new Range(0, 34), new Range(128, 161)] // UniCdEscCtrlCharsDblQtOnly - always unicode escape Console/ formatting Control Chars
      , [new Range(0, 34), new Range(128, CharExtensions.UnicodeCodePoints)] // UniCdEscCtrlCharsDblQtAndNonAscii
      , [new Range(0, 34), new Range(128, 161), new Range(256, CharExtensions.UnicodeCodePoints)] // UniCdEscCtrlCharsDblQtBkSlToEndOfLatin1
      , [new Range(0, 34), new Range(128, 161), new Range(0x10000, CharExtensions.UnicodeCodePoints)] // UniCdEscCtrlCharsDblQtBkSlToEndBmpChar
    ];
    #pragma warning restore CA2211

    private JsonEncodingTransferType jsonEncodingTransferType = JsonEncodingTransferType.UniCdEscCtrlCharsDblQtBkSlToEndOfLatin1;

    private (Range, JsonEscapeType, Func<Rune, string>)[]?   cachedMappingFactoryRanges;
    private Func<IJsonFormattingOptions, IEncodingTransfer>? buildEncodingTransfer;

    private Range[] exemptEscapingRanges = [];

    public bool CharArrayWritesString { get; set; }

    public bool ByteArrayWritesBase64String { get; set; } = true;

    public JsonFormattingOptions(IJsonFormattingOptions toClone) : base(toClone)
    {
        CharArrayWritesString            = toClone.CharArrayWritesString;
        ByteArrayWritesBase64String      = toClone.ByteArrayWritesBase64String;
        DateTimeIsNumber                 = toClone.DateTimeIsNumber;
        DateTimeIsString                 = toClone.DateTimeIsString;
        DateTimeAsStringFormatString     = toClone.DateTimeAsStringFormatString;
        DateOnlyAsStringFormatString     = toClone.DateOnlyAsStringFormatString;
        TimeAsStringFormatString         = toClone.TimeAsStringFormatString;
        WrapValuesInQuotes               = toClone.WrapValuesInQuotes;
        JsonEncodingTransferType         = toClone.JsonEncodingTransferType;
        CachedMappingFactoryRanges       = toClone.CachedMappingFactoryRanges;
        ExemptEscapingRanges             = toClone.ExemptEscapingRanges;
        UnicodeEscapingRanges            = toClone.UnicodeEscapingRanges;
        SourceEncodingTransfer           = toClone.SourceEncodingTransfer;
        
    }

    public JsonFormattingOptions()
    {
        CurrentEncodingTransfer = SourceEncodingTransfer(this);
    }

    public override ICustomStringFormatter Formatter
    {
        get => Stringformatter ??= new JsonFormatter();
        set => Stringformatter = value;
    }

    public virtual JsonFormatter JsonFormatter
    {
        get => (JsonFormatter)Formatter;
        set => Stringformatter = value;
    }

    public bool DateTimeIsNumber { get; set; }
    
    public bool DateTimeIsString { get; set; }

    public string NullStyle => "null";

    public string DateTimeAsStringFormatString
    {
        get => jsonDateTImeFormat ??= DefaultJsonDateTImeFormat;
        set => jsonDateTImeFormat = value;
    }

    public string DateOnlyAsStringFormatString
    {
        get => dateOnlyAsStringFormatString ??= DefaultJsonDateOnlyFormat;
        set => dateOnlyAsStringFormatString = value;
    }

    public string TimeAsStringFormatString { get; set; } = DefaultJsonTimeFormat;

    public bool WrapValuesInQuotes { get; set; }

    public long DateTimeTicksToNumberPrecision(long timeStampTicks) => (timeStampTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond;

    public JsonEncodingTransferType JsonEncodingTransferType
    {
        get => jsonEncodingTransferType;
        set
        {
            if (value == jsonEncodingTransferType) return;
            jsonEncodingTransferType = value;
            var encodingTypeLookup = (int)jsonEncodingTransferType;
            if (encodingTypeLookup < DefaultJsUnicodeEscapeRange.Length)
            {
                UnicodeEscapingRanges = DefaultJsUnicodeEscapeRange[encodingTypeLookup];
            }
            if (explicitlySetEncodingTransfer) return;
            if (CurrentEncodingTransfer is JsonEscapingEncodingTransfer jsonEscapingEncodingTransfer)
            {
                jsonEscapingEncodingTransfer.DecrementRefCount();
            }
            CurrentEncodingTransfer = SourceEncodingTransfer(this);
        }
    }

    public override IEncodingTransfer EncodingTransfer
    {
        get => CurrentEncodingTransfer ??= SourceEncodingTransfer(this);
        set
        {
            CurrentEncodingTransfer       = value;
            explicitlySetEncodingTransfer = true;
        }
    }

    public (Range, JsonEscapeType, Func<Rune, string>)[] CachedMappingFactoryRanges
    {
        get
        {
            if (cachedMappingFactoryRanges != null)
            {
                return cachedMappingFactoryRanges;
            }
            switch (jsonEncodingTransferType)
            {
                case JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly:
                    return [(new Range(Index.Start, new Index(128)), JsonEscapeType.BackSlashEscape, DefaultAsciiBackSlashEscapeMapping)];
                default: return [(new Range(Index.Start, new Index(128)), JsonEscapeType.BackSlashEscape, DefaultAsciiBackSlashEscapeMapping)];
            }
        }
        set
        {
            cachedMappingFactoryRanges = value;
            if (explicitlySetEncodingTransfer) return;
            if (CurrentEncodingTransfer is JsonEscapingEncodingTransfer jsonEscapingEncodingTransfer)
            {
                jsonEscapingEncodingTransfer.DecrementRefCount();
            }
            CurrentEncodingTransfer = SourceEncodingTransfer(this);
        }
    }

    public Range[] ExemptEscapingRanges
    {
        get => exemptEscapingRanges;
        set
        {
            exemptEscapingRanges = value;
            if (explicitlySetEncodingTransfer) return;
            if (CurrentEncodingTransfer is JsonEscapingEncodingTransfer jsonEscapingEncodingTransfer)
            {
                jsonEscapingEncodingTransfer.DecrementRefCount();
            }
            CurrentEncodingTransfer = SourceEncodingTransfer(this);
        }
    }

    public Range[] UnicodeEscapingRanges
    {
        get => unicodeEscapingRanges;
        set
        {
            unicodeEscapingRanges = value;
            if (explicitlySetEncodingTransfer) return;
            if (CurrentEncodingTransfer is JsonEscapingEncodingTransfer jsonEscapingEncodingTransfer)
            {
                jsonEscapingEncodingTransfer.DecrementRefCount();
            }
            CurrentEncodingTransfer = SourceEncodingTransfer(this);
        }
    }

    public Func<IJsonFormattingOptions, IEncodingTransfer> SourceEncodingTransfer
    {
        get => buildEncodingTransfer ??= DefaultEncodingTransferSelectorFactory;
        set
        {
            buildEncodingTransfer = value;
            if (explicitlySetEncodingTransfer) return;
            if (CurrentEncodingTransfer is JsonEscapingEncodingTransfer jsonEscapingEncodingTransfer)
            {
                jsonEscapingEncodingTransfer.DecrementRefCount();
            }
            CurrentEncodingTransfer = SourceEncodingTransfer(this);
        }
    }

    public static Func<IJsonFormattingOptions, IEncodingTransfer> DefaultEncodingTransferSelectorFactory
    {
        get
        {
            return jsFmtOpts =>
            {
                switch (jsFmtOpts.JsonEncodingTransferType)
                {
                    case JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly:
                        return Recycler.ThreadStaticRecycler.Borrow<JsonEscapingEncodingTransfer>().Initialize(jsFmtOpts, [
                            (new Range(Index.Start, new Index(128))
                           , JsonEscapeType.BackSlashEscape
                           , DefaultAsciiBackSlashEscapeMapping)
                        ]);
                    case JsonEncodingTransferType.BkSlEscCtrlCharsDblQtBkSlToEndBmpChar:
                    case JsonEncodingTransferType.BkSlEscCtrlCharsDblQtBkSlToEndOfLatin1:
                        return Recycler.ThreadStaticRecycler.Borrow<JsonEscapingEncodingTransfer>().Initialize(jsFmtOpts, [
                            (new Range(Index.Start, new Index(256))
                           , JsonEscapeType.BackSlashEscape
                           , DefaultLatin1BackSlashMapping)
                        ]);
                    case JsonEncodingTransferType.UniCdEscCtrlCharsDblQtBkSlToEndBmpChar:
                    case JsonEncodingTransferType.UniCdEscCtrlCharsDblQtBkSlToEndOfLatin1:
                        return Recycler.ThreadStaticRecycler.Borrow<JsonEscapingEncodingTransfer>().Initialize(jsFmtOpts, [
                            (new Range(Index.Start, new Index(256))
                           , JsonEscapeType.UnicodeEscape
                           , DefaultLatin1UnicodeEscapeMapping)
                        ]);
                    case JsonEncodingTransferType.UniCdEscCtrlCharsDblQtAndNonAscii:
                    case JsonEncodingTransferType.UniCdEscCtrlCharsDblQtOnly:
                    default:
                        return Recycler.ThreadStaticRecycler.Borrow<JsonEscapingEncodingTransfer>().Initialize(jsFmtOpts, [
                            (new Range(Index.Start, new Index(128))
                           , JsonEscapeType.UnicodeEscape
                           , DefaultAsciiUnicodeEscapeMapping)
                        ]);
                }
            };
        }
    }

    public static string DefaultAsciiBackSlashEscapeMapping(Rune inputChar)
    {
        var i = inputChar.Value;
        if (i < 32)
        {
            switch (i)
            {
                case '\b': return @"\b";
                case '\t': return @"\t";
                case '\n': return @"\n";
                case '\v': return @"\v";
                case '\f': return @"\f";
                case '\r': return @"\r";
                default:
                    Span<char> hexBuffer = stackalloc char[6];
                    hexBuffer[0] = '\\';
                    hexBuffer[1] = 'u';
                    hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                    return new string(hexBuffer);
            }
        }
        switch (i)
        {
            case '\"': return "\\\"";
            case '\\': return @"\\";
            default:   return inputChar.ToString();
        }
    }

    public static string DefaultAsciiUnicodeEscapeMapping(Rune inputChar)
    {
        var        i         = inputChar.Value;
        Span<char> hexBuffer = stackalloc char[6];
        if (i < 32)
        {
            hexBuffer[0] = '\\';
            hexBuffer[1] = 'u';
            hexBuffer.AppendLowestShortAsLowerHex(i, 2);
            return new string(hexBuffer);
        }
        switch (i)
        {
            case '\"':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '<':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '>':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '\\': return @"\\";
            default:   return inputChar.ToString();
        }
    }

    public static string DefaultLatin1BackSlashMapping(Rune inputChar)
    {
        var i = inputChar.Value;
        if (i is < 32 or > 127 and < 161)
        {
            switch (i)
            {
                case '\b': return @"\b";
                case '\t': return @"\t";
                case '\n': return @"\n";
                case '\v': return @"\v";
                case '\f': return @"\f";
                case '\r': return @"\r";
                default:
                    Span<char> hexBuffer = stackalloc char[6];
                    hexBuffer[0] = '\\';
                    hexBuffer[1] = 'u';
                    hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                    return new string(hexBuffer);
            }
        }
        switch (i)
        {
            case '\"': return "\\\"";
            case '\\': return @"\\";
            default:   return inputChar.ToString();
        }
    }

    public static string DefaultLatin1UnicodeEscapeMapping(Rune inputChar)
    {
        var        i         = inputChar.Value;
        Span<char> hexBuffer = stackalloc char[6];
        if (i is < 32 or > 127 and < 161)
        {
            hexBuffer[0] = '\\';
            hexBuffer[1] = 'u';
            hexBuffer.AppendLowestShortAsLowerHex(i, 2);
            return new string(hexBuffer);
        }
        switch (i)
        {
            case '\"':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '<':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '>':
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendLowestShortAsLowerHex(i, 2);
                return new string(hexBuffer);
            case '\\': return @"\\";
            default:   return inputChar.ToString();
        }
    }
}
