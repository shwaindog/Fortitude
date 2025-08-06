using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.LogEntries;

[Flags]
public enum FormatTypeFlags
{
    Unknown       = 0
  , NumberFormat  = 0x01
  , DateTime      = 0x02
  , StringPadding = 0x04
}

public readonly struct StringFormatTokenParams
    (string format, int paramNumber, int padding, Range originalStringLocation, FormatTypeFlags formatTypeFlags)
    : IEquatable<StringFormatTokenParams>
{
    public readonly string StringFormat         = format;
    public readonly int    ParameterNumber      = paramNumber;
    public readonly int    Padding              = padding;
    public readonly Range  FormatStringLocation = originalStringLocation;

    public readonly FormatTypeFlags FormatType = formatTypeFlags;

    public bool Equals(StringFormatTokenParams other)
    {
        var formatSame        = StringFormat == other.StringFormat;
        var paramNumSame      = ParameterNumber == other.ParameterNumber;
        var paddingSame       = Padding == other.Padding;
        var locationRangeSame = FormatStringLocation.Equals(other.FormatStringLocation);
        var formatTypeSame    = FormatType == other.FormatType;

        var allAreSame = formatSame && paramNumSame && paddingSame && locationRangeSame && formatTypeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => obj is StringFormatTokenParams other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(StringFormat, ParameterNumber, Padding, FormatStringLocation, (int)FormatType);
}

public class FormatBuilder : ReusableObject<FormatBuilder>
{
    private const int ScratchCharBufferSize = 256;

    private IStringBuilder sb = null!;

    private readonly List<StringFormatTokenParams> stringFormatParams = new();
    private readonly List<ReplacedAt>              deltasApplied      = new();

    private string formattedString = null!;

    public FormatBuilder() { }

    public FormatBuilder(FormatBuilder toClone)
    {
        sb.Append(toClone.sb);
    }

    private record struct ReplacedAt(int OriginalIndex, int DeltaSize);

    private static readonly string[] CommonNumberFormatString =
    [
        "{0:c}", "{0:d}", "{0:e}", "{0:f}", "{0:g}", "{0:n}", "{0:p}", "{0:x}"
      , "{0:c0}", "{0:c1}", "{0:c2}", "{0:c3}", "{0:c4}", "{0:c5}", "{0:c6}", "{0:c7}", "{0:c8}", "{0:c9}"
      , "{0:d0}", "{0:d1}", "{0:d2}", "{0:d3}", "{0:d4}", "{0:d5}", "{0:d6}", "{0:d7}", "{0:d8}", "{0:d9}"
      , "{0:e0}", "{0:e1}", "{0:e2}", "{0:e3}", "{0:e4}", "{0:e5}", "{0:e6}", "{0:e7}", "{0:e8}", "{0:e9}"
      , "{0:f0}", "{0:f1}", "{0:f2}", "{0:f3}", "{0:f4}", "{0:f5}", "{0:f6}", "{0:f7}", "{0:f8}", "{0:f9}"
      , "{0:g0}", "{0:g1}", "{0:g2}", "{0:g3}", "{0:g4}", "{0:g5}", "{0:g6}", "{0:g7}", "{0:g8}", "{0:g9}"
      , "{0:n0}", "{0:n1}", "{0:n2}", "{0:n3}", "{0:n4}", "{0:n5}", "{0:n6}", "{0:n7}", "{0:n8}", "{0:n9}"
      , "{0:p0}", "{0:p1}", "{0:p2}", "{0:p3}", "{0:p4}", "{0:p5}", "{0:p6}", "{0:p7}", "{0:p8}", "{0:p9}"
      , "{0:x0}", "{0:x1}", "{0:x2}", "{0:x3}", "{0:x4}", "{0:x5}", "{0:x6}", "{0:x7}", "{0:x8}", "{0:x9}"
      , "{0:0.0}", "{0:0.00}", "{0:0.000}", "{0:0.0000}", "{0:0.00000}", "{0:0.000000}", "{0:0000000}", "{0:00000000}", "{0:000000000}"
      , "{0:0000000000}"
      , "{0:#.00}", "{0:#0.00}", "{0:#00.00}", "{0:#,000.00}", "{0:#0,000.00}", "{0::#00,000.00}", "{0::#,000,000.00}"
      , "{0:#}", "{0:#0}", "{0:#00}", "{0:#,000}", "{0:#0,000}", "{0::#00,000}", "{0::#,000,000}", "{0::#0,000,000}", "{0::#,000,000,000}"
      , "{0::#0,000,000,000}"
      , "{0:#00.00%}", "{0:#00.0%}", "{0:#00%}", "{0:##0.00%}", "{0:##0.0%}", "{0:##0%}"
      , "{0:#.0%}", "{0:#.00%}", "{0:#.000%}", "{0:#.0000%}", "{0:#.00000%}"
      , "{0:0.0%}", "{0:0.00%}", "{0:0.000%}", "{0:0.0000%}", "{0:0.00000%}"
      , "{0,1}", "{0,2}", "{0,3}", "{0,4}", "{0,5}", "{0,6}", "{0,7}", "{0,8}", "{0,9}", "{0,10}", "{0,11}", "{0,12}", "{0,13}", "{0,14}", "{0,15}"
      , "{0,16}", "{0,17}", "{0,18}", "{0,19}", "{0,20}", "{0,21}", "{0,22}", "{0,23}", "{0,24}", "{0,25}", "{0,26}", "{0,27}", "{0,28}", "{0,30}"
      , "{0,-1}", "{0,-2}", "{0,-3}", "{0,-4}", "{0,-5}", "{0,-6}", "{0,-7}", "{0,-8}", "{0,-9}", "{0,-10}", "{0,-11}", "{0,-12}", "{0,-13}"
      , "{0,-14}", "{0,-15}", "{0,-16}", "{0,-17}", "{0,-18}", "{0,-19}", "{0,-20}", "{0,-21}", "{0,-22}", "{0,-23}", "{0,-24}", "{0,-25}", "{0,-26}"
      , "{0,-30}", "{0,1:c}", "{0,2:c}", "{0,3:c}", "{0,4:c}", "{0,5:c}", "{0,6:c}", "{0,7:c}", "{0,8:c}", "{0,9:c}", "{0,10:c}", "{0,11:c}"
      , "{0,12:c}"
      , "{0,13:c}", "{0,14:c}", "{0,15:c}", "{0,16:c}", "{0,17:c}", "{0,18:c}", "{0,19:c}", "{0,20:c}", "{0,21:c}", "{0,22:c}", "{0,23:c}", "{0,24:c}"
      , "{0,-1:c}", "{0,-2:c}", "{0,-3:c}", "{0,-4:c}", "{0,-5:c}", "{0,-6:c}", "{0,-7:c}", "{0,-8:c}", "{0,-9:c}", "{0,-10:c}", "{0,-11:c}"
      , "{0,-12:c}", "{0,-13:c}", "{0,-14:c}", "{0,-15:c}", "{0,-16:c}", "{0,-17:c}", "{0,-18:c}", "{0,-19:c}", "{0,-20:c}", "{0,-21:c}", "{0,-22:c}"
      , "{0,-25:c}", "{0,1:d}", "{0,2:d}", "{0,3:d}", "{0,4:d}", "{0,5:d}", "{0,6:d}", "{0,7:d}", "{0,8:d}", "{0,9:d}", "{0,10:d}", "{0,11:d}"
      , "{0,12:d}"
      , "{0,13:d}", "{0,14:d}", "{0,15:d}", "{0,16:d}", "{0,17:d}", "{0,18:d}", "{0,19:d}", "{0,20:d}", "{0,21:d}", "{0,22:d}", "{0,23:d}", "{0,24:d}"
      , "{0,-1:d}", "{0,-2:d}", "{0,-3:d}", "{0,-4:d}", "{0,-5:d}", "{0,-6:d}", "{0,-7:d}", "{0,-8:d}", "{0,-9:d}", "{0,-10:d}", "{0,-11:d}"
      , "{0,-12:d}", "{0,-13:d}", "{0,-14:d}", "{0,-15:d}", "{0,-16:d}", "{0,-17:d}", "{0,-18:d}", "{0,-19:d}", "{0,-20:d}", "{0,-21:d}", "{0,-22:d}"
      , "{0,1:e}", "{0,2:e}", "{0,3:e}", "{0,4:e}", "{0,5:e}", "{0,6:e}", "{0,7:e}", "{0,8:e}", "{0,9:e}", "{0,10:e}", "{0,11:e}", "{0,12:e}"
      , "{0,13:e}", "{0,14:e}", "{0,15:e}", "{0,16:e}", "{0,17:e}", "{0,18:e}", "{0,19:e}", "{0,20:e}", "{0,21:e}", "{0,22:e}", "{0,23:e}", "{0,24:e}"
      , "{0,-1:e}", "{0,-2:e}", "{0,-3:e}", "{0,-4:e}", "{0,-5:e}", "{0,-6:e}", "{0,-7:e}", "{0,-8:e}", "{0,-9:e}", "{0,-10:e}", "{0,-11:e}"
      , "{0,-12:e}", "{0,-13:e}", "{0,-14:e}", "{0,-15:e}", "{0,-16:e}", "{0,-17:e}", "{0,-18:e}", "{0,-19:e}", "{0,-20:e}", "{0,-21:e}", "{0,-22:e}"
      , "{0,1:f}", "{0,2:f}", "{0,3:f}", "{0,4:f}", "{0,5:f}", "{0,6:f}", "{0,7:f}", "{0,8:f}", "{0,9:f}", "{0,10:f}", "{0,11:f}", "{0,12:f}"
      , "{0,13:f}", "{0,14:f}", "{0,15:f}", "{0,16:f}", "{0,17:f}", "{0,18:f}", "{0,19:f}", "{0,20:f}", "{0,21:f}", "{0,22:f}", "{0,23:f}", "{0,24:f}"
      , "{0,-1:f}", "{0,-2:f}", "{0,-3:f}", "{0,-4:f}", "{0,-5:f}", "{0,-6:f}", "{0,-7:f}", "{0,-8:f}", "{0,-9:f}", "{0,-10:f}", "{0,-11:f}"
      , "{0,-12:f}", "{0,-13:f}", "{0,-14:f}", "{0,-15:f}", "{0,-16:f}", "{0,-17:f}", "{0,-18:f}", "{0,-19:f}", "{0,-20:f}", "{0,-21:f}", "{0,-22:f}"
      , "{0,1:g}", "{0,2:g}", "{0,3:g}", "{0,4:g}", "{0,5:g}", "{0,6:g}", "{0,7:g}", "{0,8:g}", "{0,9:g}", "{0,10:g}", "{0,11:g}", "{0,12:g}"
      , "{0,13:g}", "{0,14:g}", "{0,15:g}", "{0,16:g}", "{0,17:g}", "{0,18:g}", "{0,19:g}", "{0,20:g}", "{0,21:g}", "{0,22:g}", "{0,23:g}", "{0,24:g}"
      , "{0,-1:g}", "{0,-2:g}", "{0,-3:g}", "{0,-4:g}", "{0,-5:g}", "{0,-6:g}", "{0,-7:g}", "{0,-8:g}", "{0,-9:g}", "{0,-10:g}", "{0,-11:g}"
      , "{0,-12:g}", "{0,-13:g}", "{0,-14:g}", "{0,-15:g}", "{0,-16:g}", "{0,-17:g}", "{0,-18:g}", "{0,-19:g}", "{0,-20:g}", "{0,-21:g}", "{0,-22:g}"
      , "{0,1:n}", "{0,2:n}", "{0,3:n}", "{0,4:n}", "{0,5:n}", "{0,6:n}", "{0,7:n}", "{0,8:n}", "{0,9:n}", "{0,10:n}", "{0,11:n}", "{0,12:n}"
      , "{0,13:n}", "{0,14:n}", "{0,15:n}", "{0,16:n}", "{0,17:n}", "{0,18:n}", "{0,19:n}", "{0,20:n}", "{0,21:n}", "{0,22:n}", "{0,23:n}", "{0,24:n}"
      , "{0,-1:n}", "{0,-2:n}", "{0,-3:n}", "{0,-4:n}", "{0,-5:n}", "{0,-6:n}", "{0,-7:n}", "{0,-8:n}", "{0,-9:n}", "{0,-10:n}", "{0,-11:n}"
      , "{0,-12:n}", "{0,-13:n}", "{0,-14:n}", "{0,-15:n}", "{0,-16:n}", "{0,-17:n}", "{0,-18:n}", "{0,-19:n}", "{0,-20:n}", "{0,-21:n}", "{0,-22:n}"
      , "{0,1:x}", "{0,2:x}", "{0,3:x}", "{0,4:x}", "{0,5:x}", "{0,6:x}", "{0,7:x}", "{0,8:x}", "{0,9:x}", "{0,10:x}", "{0,11:x}", "{0,12:x}"
      , "{0,13:x}", "{0,14:x}", "{0,15:x}", "{0,16:x}", "{0,17:x}", "{0,18:x}", "{0,19:x}", "{0,20:x}", "{0,21:x}", "{0,22:x}", "{0,23:x}", "{0,24:x}"
      , "{0,-1:x}", "{0,-2:x}", "{0,-3:x}", "{0,-4:x}", "{0,-5:x}", "{0,-6:x}", "{0,-7:x}", "{0,-8:x}", "{0,-9:x}", "{0,-10:x}", "{0,-11:x}"
      , "{0,-12:x}", "{0,-13:x}", "{0,-14:x}", "{0,-15:x}", "{0,-16:x}", "{0,-17:x}", "{0,-18:x}", "{0,-19:x}", "{0,-20:x}", "{0,-21:x}", "{0,-22:x}"
    ];

    public static List<string> RuntimeAppCustomFormatString = new();

    private static readonly string[] CommonDateTimeFormatString =
    [
        "{0:d}", "{0:D}", "{0:f}", "{0:f}", "{0:g}", "{0:M}", "{0:r}", "{0:s}", "{0:t}", "{0:u}", "{0:U}", "{0:Y}"
      , "{0:yyyy-MM-dd}", "{0:yyyy/MM/dd}", "{0:yyyyMMdd}", "{0:yyyy-MMM-dd}", "{0:yyyy/MMM/dd}", "{0:yyyyMMMdd}"
      , "{0:yyyy-MM-ddTHH:mm:ss.ffffff}", "{0:yyyy-MM-ddTHH:mm:ss.fff}", "{0:yyyy-MM-ddTHH:mm:ss}", "{0:yyyy-MM-ddTHH:mm}", "{0:yyyy-MM-ddTHH}"
      , "{0:yyyy-MM-dd HH:mm:ss.ffffff}", "{0:yyyy-MM-dd HH:mm:ss.fff}", "{0:yyyy-MM-dd HH:mm:ss}", "{0:yyyy-MM-dd HH:mm}", "{0:yyyy-MM-dd HH}"
      , "{0:yyyy/MM/ddTHH:mm:ss.ffffff}", "{0:yyyy/MM/ddTHH:mm:ss.fff}", "{0:yyyy/MM/ddTHH:mm:ss}", "{0:yyyy/MM/ddTHH:mm}", "{0:yyyy/MM/ddTHH}"
      , "{0:yyyy/MM/dd HH:mm:ss.ffffff}", "{0:yyyy/MM/dd HH:mm:ss.fff}", "{0:yyyy/MM/dd HH:mm:ss}", "{0:yyyy/MM/dd HH:mm}", "{0:yyyy/MM/dd HH}"
      , "{0:yyyyMMddTHH:mm:ss.ffffff}", "{0:yyyyMMddTHH:mm:ss.fff}", "{0:yyyyMMddTHH:mm:ss}", "{0:yyyyMMddTHH:mm}", "{0:yyyyMMddTHH}"
      , "{0:yyyyMMdd HH:mm:ss.ffffff}", "{0:yyyyMMdd HH:mm:ss.fff}", "{0:yyyyMMdd HH:mm:ss}", "{0:yyyyMMdd HH:mm}", "{0:yyyyMMdd HH}"
      , "{0:MM/dd/yyyy}", "{0:MM/dd/yyyyTHH:mm:ss}", "{0:MM/dd/yyyyTHH}", "{0:MM/dd/yyyyTHH:mm}"
      , "{0:MM/dd/yyyyThh:mm:ss tt}", "{0:MM/dd/yyyyThh:mm:sstt}"
      , "{0:MM/dd/yy}", "{0:MM/dd/yyTHH:mm:ss}", "{0:MM/dd/yyTHH}", "{0:MM/dd/yyTHH:mm}", "{0:MM/dd/yyThh:mm:ss tt}", "{0:MM/dd/yyThh:mm:sstt}"
      , "{0:dd/MM/yyyy}", "{0:dd/MM/yyyyTHH:mm:ss}", "{0:dd/MM/yyyyTHH}", "{0:dd/MM/yyyyTHH:mm}"
      , "{0:dd/MM/yyyyTHH:mm:ss.fff}", "{0:dd/MM/yyyyTHH:mm:ss.ffffff}"
      , "{0:dd/MM/yy}", "{0:dd/MM/yyTHH:mm:ss}", "{0:dd/MM/yyTHH}", "{0:dd/MM/yyTHH:mm}", "{0:dd/MM/yyTHH:mm:ss.fff}", "{0:dd/MM/yyTHH:mm:ss.ffffff}"
      , "{0:MMM/dd/yyyy}", "{0:MMM/dd/yyyyTHH:mm:ss}", "{0:MMM/dd/yyyyTHH}", "{0:MMM/dd/yyyyTHH:mm}"
      , "{0:MMM/dd/yyyyThh:mm:ss tt}", "{0:MMM/dd/yyyyThh:mm:sstt}"
      , "{0:MMM/dd/yy}", "{0:MMM/dd/yyTHH:mm:ss}", "{0:MMM/dd/yyTHH}", "{0:MMM/dd/yyTHH:mm}", "{0:MMM/dd/yyThh:mm:ss tt}", "{0:MMM/dd/yyThh:mm:sstt}"
      , "{0:dd/MMM/yyyy}", "{0:dd/MMM/yyyyTHH:mm:ss}", "{0:dd/MMM/yyyyTHH}", "{0:dd/MMM/yyyyTHH:mm}"
      , "{0:dd/MMM/yyyyThh:mm:ss tt}", "{0:dd/MMM/yyyyThh:mm:sstt}"
      , "{0:dd/MMM/yy}", "{0:dd/MMM/yyTHH:mm:ss}", "{0:dd/MMM/yyTHH}", "{0:dd/MMM/yyTHH:mm}", "{0:dd/MMM/yyThh:mm:ss tt}", "{0:dd/MMM/yyThh:mm:sstt}"
    ];

    private enum ReadFormattingTokenStage
    {
        ParamNumber
      , Padding
      , FormatParams
    }

    private struct FormatTokenState()
    {
        private bool isValidFormattingToken;
        public bool IsValidFormattingToken
        {
            readonly get => isValidFormattingToken;
            set
            {
                isValidFormattingToken = value;
                HasFinishParseAttempt  = value;
            }
        }

        public int Padding => PaddingSign * PaddingDigits;

        public bool HasFinishParseAttempt = false;

        public int ParamNumber   = -1;
        public int PaddingDigits = 0;
        public int PaddingSign   = 1;

        public Range? FormatTokenRange;

        public ReadFormattingTokenStage ParseStage = ReadFormattingTokenStage.ParamNumber;

        public FormatTypeFlags FormatType = FormatTypeFlags.Unknown;

        public string? FormatStringEquivalent = null;
    }

    public IStringBuilder BackingStringBuilder => sb;

    public FormatBuilder Initialize(string formattingString, IStringBuilder sbToFormat)
    {
        sb = sbToFormat;
        sb.Append(formattingString);
        formattedString = formattingString;
        return this;
    }

    public Range? ReplaceTokenWith(StringFormatTokenParams tokenToReplace, ICharSequence? withStringBuilderChars)
    {
        if (!stringFormatParams.Remove(tokenToReplace))
        {
            Console.Out.WriteLine("Token no longer exists in this string, check to ensure you are not trying to replace the same token twice");
            return null;
        }
        var originalLocation     = tokenToReplace.FormatStringLocation.Start.Value;
        var tokenSize            = tokenToReplace.FormatStringLocation.Length();
        var shift                = GetUpdatedTokenStart(originalLocation);
        var updatedTokenLocation = tokenToReplace.FormatStringLocation.Shift(shift);
        sb.ReplaceAtRange(updatedTokenLocation, withStringBuilderChars);
        var newLocation      = updatedTokenLocation.Start.Value;
        var replaceLength    = withStringBuilderChars?.Length ?? 0;
        var updatedSizeDelta = replaceLength - tokenSize;
        deltasApplied.Add(new ReplacedAt(originalLocation, updatedSizeDelta));
        return new Range(newLocation, newLocation + replaceLength);
    }

    private int GetUpdatedTokenStart(int originalLocation)
    {
        var deltaSum = 0;
        for (var i = 0; i < deltasApplied.Count && i < originalLocation; i++)
        {
            var previousReplacement = deltasApplied[i];
            if (previousReplacement.OriginalIndex < originalLocation)
            {
                deltaSum += previousReplacement.DeltaSize;
            }
        }
        return deltaSum;
    }

    public List<StringFormatTokenParams> RemainingTokens()
    {
        if (stringFormatParams.Any())
        {
            return stringFormatParams;
        }
        Span<char> tokenBuffer = (stackalloc char[ScratchCharBufferSize]).ResetMemory(ScratchCharBufferSize);

        for (var i = 0; i < formattedString.Length; i++)
        {
            var lookForOpeningBrace = formattedString[i];
            if (lookForOpeningBrace == '{')
            {
                tokenBuffer.ResetMemory(ScratchCharBufferSize);
                var tokenParseState = ParseTokenLoadCharBuffer(tokenBuffer, i);
                if (tokenParseState.IsValidFormattingToken)
                {
                    tokenParseState = FindOrCreateStringBuilderFormatString(tokenBuffer, tokenParseState);
                    if (tokenParseState is { FormatStringEquivalent: not null, FormatTokenRange: { } formatTokenRange })
                    {
                        var foundToken =
                            new StringFormatTokenParams
                                (tokenParseState.FormatStringEquivalent, tokenParseState.ParamNumber, tokenParseState.Padding
                               , formatTokenRange, tokenParseState.FormatType);
                        stringFormatParams.Add(foundToken);
                        i += formatTokenRange.End.Value - formatTokenRange.Start.Value;
                    }
                }
            }
        }
        return stringFormatParams;
    }

    private FormatTokenState FindOrCreateStringBuilderFormatString(Span<char> tokenBuffer, FormatTokenState tokenParseState)
    {
        for (int j = 0; j < CommonNumberFormatString.Length && tokenParseState.FormatStringEquivalent == null; j++)
        {
            var commonNumberFormat = CommonNumberFormatString[j];
            if (tokenBuffer.IsEndOf(commonNumberFormat))
            {
                tokenParseState.FormatStringEquivalent =  commonNumberFormat;
                tokenParseState.FormatType             =  commonNumberFormat.Contains(",") ? FormatTypeFlags.StringPadding : FormatTypeFlags.Unknown;
                tokenParseState.FormatType             |= commonNumberFormat.Contains(":") ? FormatTypeFlags.NumberFormat : FormatTypeFlags.Unknown;
            }
        }
        for (int j = 0; j < CommonDateTimeFormatString.Length && tokenParseState.FormatStringEquivalent == null; j++)
        {
            var commonDateTimeFormat = CommonDateTimeFormatString[j];
            if (tokenBuffer.IsEndOf(commonDateTimeFormat))
            {
                tokenParseState.FormatStringEquivalent = commonDateTimeFormat;
                tokenParseState.FormatType             = FormatTypeFlags.DateTime;
                tokenParseState.FormatType
                    |= commonDateTimeFormat.Contains(",") ? FormatTypeFlags.StringPadding : FormatTypeFlags.Unknown;
            }
        }
        for (int j = 0; j < RuntimeAppCustomFormatString.Count && tokenParseState.FormatStringEquivalent == null; j++)
        {
            var commonDateTimeFormat = RuntimeAppCustomFormatString[j];
            if (tokenBuffer.IsEndOf(commonDateTimeFormat))
            {
                tokenParseState.FormatStringEquivalent = commonDateTimeFormat;
            }
        }
        if (tokenParseState.FormatStringEquivalent == null && tokenParseState.FormatTokenRange != null)
        {
            var tokenSize            = BuildNewStringBuilderFormatToken(tokenBuffer, tokenParseState.FormatTokenRange.Value);
            var newAppUncommonFormat = tokenBuffer.ToString();
            if (tokenSize < 0)
            {
                Console.Out.WriteLine($"Failed to build format token {newAppUncommonFormat} properly");
            }
            else
            {
                Console.Out.WriteLine($"Constructed new formatting token {newAppUncommonFormat} properly");
                RuntimeAppCustomFormatString.Add(newAppUncommonFormat);
                tokenParseState.FormatStringEquivalent = newAppUncommonFormat;
            }
        }
        return tokenParseState;
    }

    private int BuildNewStringBuilderFormatToken(Span<char> tokenBuffer, Range formatRange)
    {
        tokenBuffer.ResetMemory();
        tokenBuffer.Append(formattedString[formatRange.Start]);
        var onePlusRange        = new Range(formatRange.Start.Value + 1, formatRange.End);
        var parseStage          = ReadFormattingTokenStage.ParamNumber;
        var hasAddedParamNumber = false;
        foreach (var check in formattedString[onePlusRange])
        {
            if (parseStage == ReadFormattingTokenStage.ParamNumber)
            {
                switch (check)
                {
                    case >= '0' and <= '9' when !hasAddedParamNumber:
                        tokenBuffer.Append('0');
                        hasAddedParamNumber = true;
                        break;
                    case >= '0' and <= '9': break;
                    case ',':
                        tokenBuffer.Append(check);
                        parseStage = ReadFormattingTokenStage.Padding;
                        break;
                    case ':':
                        tokenBuffer.Append(check);
                        parseStage = ReadFormattingTokenStage.FormatParams;
                        break;
                    case '}':
                        tokenBuffer.Append(check);
                        return tokenBuffer.PopulatedLength();
                    default: return -1;
                }
            }
            if (parseStage is ReadFormattingTokenStage.Padding or ReadFormattingTokenStage.FormatParams)
            {
                tokenBuffer.Append(check);
                switch (check)
                {
                    case '}': return tokenBuffer.PopulatedLength();
                    case '{': return -1;
                }
            }
        }
        return tokenBuffer.PopulatedLength();
    }

    private FormatTokenState ParseTokenLoadCharBuffer(Span<char> tokenBuffer, int i)
    {
        var formatTokenState = new FormatTokenState();
        var j                = i + 1;
        for (; j < formattedString.Length && !formatTokenState.HasFinishParseAttempt; j++)
        {
            var check = formattedString[i];
            if (formatTokenState.ParseStage == ReadFormattingTokenStage.ParamNumber)
            {
                formatTokenState = ParseParamNumber(check, formatTokenState);
            }
            if (formatTokenState.ParseStage == ReadFormattingTokenStage.Padding)
            {
                tokenBuffer.Append(check);
                formatTokenState = ParsePaddingSize(check, formatTokenState);
            }
            if (formatTokenState.ParseStage == ReadFormattingTokenStage.FormatParams)
            {
                tokenBuffer.Append(check);
                formatTokenState = ParseFormatDefinition(check, formatTokenState);
            }
        }
        formatTokenState.FormatTokenRange = new Range(i, j);
        return formatTokenState;
    }

    private static FormatTokenState ParseParamNumber(char check, FormatTokenState formatTokenState)
    {
        switch (check)
        {
            case >= '0' and <= '9' when formatTokenState.ParamNumber < 0: formatTokenState.ParamNumber = check - '0'; break;
            case >= '0' and <= '9':
                formatTokenState.ParamNumber *= 10;
                formatTokenState.ParamNumber += check - '0';
                break;
            case ',': formatTokenState.ParseStage             = ReadFormattingTokenStage.Padding; break;
            case ':': formatTokenState.ParseStage             = ReadFormattingTokenStage.FormatParams; break;
            case '}': formatTokenState.IsValidFormattingToken = true; break;
            default:  formatTokenState.HasFinishParseAttempt  = true; break;
        }
        return formatTokenState;
    }

    private static FormatTokenState ParsePaddingSize(char check, FormatTokenState formatTokenState)
    {
        switch (check)
        {
            case >= '0' and <= '9' when formatTokenState.PaddingDigits == 0: formatTokenState.PaddingDigits = check - '0'; break;
            case >= '0' and <= '9':
                formatTokenState.PaddingDigits *= 10;
                formatTokenState.PaddingDigits += check - '0';
                break;
            case '-': formatTokenState.PaddingSign            = -1; break;
            case ':': formatTokenState.ParseStage             = ReadFormattingTokenStage.FormatParams; break;
            case '}': formatTokenState.IsValidFormattingToken = true; break;
            default:  formatTokenState.HasFinishParseAttempt  = true; break;
        }
        return formatTokenState;
    }

    private static FormatTokenState ParseFormatDefinition(char check, FormatTokenState formatTokenState)
    {
        switch (check)
        {
            case '}': formatTokenState.IsValidFormattingToken = true; break;
            case ':':
            case '{':
            case ',':
                formatTokenState.HasFinishParseAttempt = true;
                break;
        }
        return formatTokenState;
    }

    public override void StateReset()
    {
        sb = null!;
        stringFormatParams.Clear();
        deltasApplied.Clear();
        formattedString = null!;

        base.StateReset();
    }

    public override FormatBuilder Clone() => Recycler?.Borrow<FormatBuilder>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FormatBuilder(this);

    public override FormatBuilder CopyFrom(FormatBuilder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        sb.Clear();
        sb.Append(source.sb);

        return this;
    }
}
