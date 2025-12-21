using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Extensions;

[Flags]
public enum FormatStringType
{
    None                   = 0x00
  , Prefix                 = 0x01
  , Identifier             = 0x02
  , ExtendedLimitLength    = 0x04
  , Layout                 = 0x08
  , ExtendedSplitJoinRange = 0x10
  , Format                 = 0x20
  , Suffix                 = 0x40
}

[InlineArray(128)]
public struct DoubleCacheLineChars
{
    private char element;
}

public readonly struct SplitJoinRange : ISpanFormattable
{
    private readonly DoubleCacheLineChars splitChars;
    private readonly DoubleCacheLineChars joinChars;
    private readonly Range     splitElementsRange;

    private readonly byte splitLength;
    private readonly byte joinLength;

    public SplitJoinRange(ReadOnlySpan<char> splitChars, ReadOnlySpan<char> joinChars, Range splitElementsRange)
    {
        switch (splitChars.Length)
        {
            case 0:
                splitLength = 0;
                return;
            case 1: this.splitChars[0] = splitChars[0]; break;
            case 2:
                this.splitChars[0] = splitChars[0];
                this.splitChars[1] = splitChars[1];
                break;
            case 3:
                this.splitChars[0] = splitChars[0];
                this.splitChars[1] = splitChars[1];
                this.splitChars[2] = splitChars[2];
                break;
            default:
                this.splitChars[0] = splitChars[0];
                this.splitChars[1] = splitChars[1];
                this.splitChars[2] = splitChars[2];
                this.splitChars[3] = splitChars[3];
                break;
        }
        switch (joinChars.Length)
        {
            case 0: break;
            case 1: this.joinChars[0] = joinChars[0]; break;
            case 2:
                this.joinChars[0] = joinChars[0];
                this.joinChars[1] = joinChars[1];
                break;
            case 3:
                this.joinChars[0] = joinChars[0];
                this.joinChars[1] = joinChars[1];
                this.joinChars[2] = joinChars[2];
                break;
            default:
                this.joinChars[0] = joinChars[0];
                this.joinChars[1] = joinChars[1];
                this.joinChars[2] = joinChars[2];
                this.joinChars[3] = joinChars[3];
                break;
        }
        splitLength = (byte)Math.Min(4, splitChars.Length);
        joinLength  = (byte)Math.Min(4, joinChars.Length);
        if (splitChars == joinChars && splitElementsRange.IsAllRange())
        {
            splitLength = 0;
        }
        this.splitElementsRange = splitElementsRange;
    }

    public bool IsNoSplitJoin => splitLength == 0;

    public byte SplitLength => splitLength;
    public byte JoinLength => joinLength;

    public int WriteSplitOn(Span<char> destination, int fromDestIndex)
    {
        for (int i = 0; i < splitLength; i++)
        {
            destination[fromDestIndex + i] = splitChars[i];
        }
        return splitLength;
    }

    public int WriteJoinOn(Span<char> destination, int fromDestIndex)
    {
        for (int i = 0; i < joinLength; i++)
        {
            destination[fromDestIndex + i] = joinChars[i];
        }
        return joinLength;
    }

    public Range SplitElementsRange => splitElementsRange;

    public int ApplySplitJoin(Span<char> bufferWritten, ReadOnlySpan<char> original, IEncodingTransfer contentEncoder, IEncodingTransfer joinEncoder)
    {
        var splitSpan           = splitChars[..splitLength];
        var splitOccurenceCount = -1;
        if (!IsNoSplitJoin)
        {
            splitOccurenceCount = original.SubSequenceOccurenceCount(splitSpan);
        }
        if (IsNoSplitJoin || splitOccurenceCount < 1)
        {
            return contentEncoder.Transfer(original, bufferWritten, 0);
        }
        Span<Range> splitRanges    = stackalloc Range[splitOccurenceCount+1];
        var         numberOfRanges = original.Split(splitRanges, splitSpan);
        splitRanges = splitRanges[..numberOfRanges];
        var boundRange = splitElementsRange.BoundRangeToLength(splitRanges.Length);
        splitRanges = splitRanges[boundRange];

        var joinSpan = joinChars[..joinLength];
        var bufi     = 0;
        for (int i = 0; i < splitRanges.Length; i++)
        {
            var toCopy = original[splitRanges[i]];
            bufi += contentEncoder.Transfer(toCopy, bufferWritten, bufi);
            if (i < splitRanges.Length - 1)
            {
                bufi += joinEncoder.Transfer(joinSpan, bufferWritten, bufi);
            }
        }
        return bufi;
    }

    public int ApplySplitJoin(IStringBuilder sb, ReadOnlySpan<char> original, IEncodingTransfer contentEncoder, IEncodingTransfer joinEncoder)
    {
        var splitSpan           = splitChars[..splitLength];
        var splitOccurenceCount = -1;
        if (!IsNoSplitJoin)
        {
            splitOccurenceCount = original.SubSequenceOccurenceCount(splitSpan);
        }
        if (IsNoSplitJoin || splitOccurenceCount < 1)
        {
            return contentEncoder.Transfer(original, sb);
        }
        Span<Range> splitRanges    = stackalloc Range[splitOccurenceCount+1];
        var         numberOfRanges = original.Split(splitRanges, splitSpan);
        splitRanges = splitRanges[..numberOfRanges];
        var boundRange = splitElementsRange.BoundRangeToLength(splitRanges.Length);
        splitRanges = splitRanges[boundRange];

        var joinSpan = joinChars[..joinLength];
        var bufi     = 0;
        for (int i = 0; i < splitRanges.Length; i++)
        {
            var toCopy = original[splitRanges[i]];
            bufi += contentEncoder.Transfer(toCopy, sb);
            if (i < splitRanges.Length - 1)
            {
                bufi += joinEncoder.Transfer(joinSpan, sb);
            }
        }
        return bufi;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> temp         = stackalloc char[32];
        var        charsWritten = temp.WriteSplitJoinFormatting(0, this);
        return new string(temp[..charsWritten]);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = destination.WriteSplitJoinFormatting(0, this);
        return true;
    }

    public static readonly SplitJoinRange NoSplitJoin = new();
}

public static class ExtendedSpanFormattableExtensions
{
    public static FormatStringType ExtractExtendedStringFormatStages
    (this ReadOnlySpan<char> toCheck, out ReadOnlySpan<char> formatPrefix, out ReadOnlySpan<char> identifier, out Range extendedSliceLengthRange
      , out ReadOnlySpan<char> layout, out SplitJoinRange extendedSplitJoinRange, out ReadOnlySpan<char> format, out ReadOnlySpan<char> formatSuffix
      , int fromIndex = 0)
    {
        formatPrefix             = toCheck.Slice(0, 0);
        identifier               = toCheck.Slice(0, 0);
        extendedSliceLengthRange = Range.All;
        layout                   = toCheck.Slice(0, 0);
        extendedSplitJoinRange   = SplitJoinRange.NoSplitJoin;
        format                   = toCheck.Slice(0, 0);
        formatSuffix             = toCheck.Slice(0, 0);

        var foundFormatStages = FormatStringType.None;
        if (toCheck.Length == 0) return foundFormatStages;
        ReadOnlySpan<char> remainingSpan = toCheck[fromIndex..];

        var indexOfBrcOpn = remainingSpan.IndexOf('{');

        int stage = 0;
        if (indexOfBrcOpn >= 0)
        {
            if (indexOfBrcOpn > 0)
            {
                foundFormatStages |= FormatStringType.Prefix;
                formatPrefix      =  remainingSpan[..indexOfBrcOpn];
                remainingSpan     =  remainingSpan[indexOfBrcOpn..];
            }
            var indexOfBrcCls = remainingSpan.IndexOf('}');
            if (indexOfBrcCls == -1)
            {
                throw new FormatException("Invalid format string expected matching closing braces from " + remainingSpan.ToString());
            }
            formatSuffix = remainingSpan[(indexOfBrcCls + 1)..];
            if (indexOfBrcCls < remainingSpan.Length - 1)
            {
                foundFormatStages |= FormatStringType.Suffix;
            }
            remainingSpan = remainingSpan[1..indexOfBrcCls];
        }
        else
        { // just format string
            identifier = "0";
            layout     = "0";
            format     = remainingSpan;
            stage      = 3;
        }
        while (remainingSpan.Length > 0)
        {
            switch (stage)
            {
                case 0:
                    var (nextStage, foundStages, consumedSpan, lengthSliceRange) = IdentifierAndSlice(remainingSpan, out identifier);

                    stage                    =  nextStage;
                    foundFormatStages        |= foundStages;
                    remainingSpan            =  remainingSpan[consumedSpan..];
                    extendedSliceLengthRange =  lengthSliceRange;
                    break;
                case 1:
                    var (next, foundParts, nextSpanIndex, splitJoinRange) = LayoutAndSplitJoin(remainingSpan, out layout);

                    stage                  =  next;
                    foundFormatStages      |= foundParts;
                    remainingSpan          =  remainingSpan[nextSpanIndex..];
                    extendedSplitJoinRange =  splitJoinRange;
                    break;
                case 2:
                    stage             =  3;
                    format            =  remainingSpan;
                    foundFormatStages |= FormatStringType.Format;
                    break;
                default: return foundFormatStages;
            }
        }
        return foundFormatStages;
    }

    public static bool HasFormatStringPadding(this string toCheck) => ((ReadOnlySpan<char>)toCheck).HasFormatStringPadding();


    public static bool HasFormatStringPadding(this ReadOnlySpan<char> toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return false;
        var remainingSpan = toCheck[indexOfBrcOpn..];
        var indexOfBrcCls = remainingSpan.IndexOf('}');
        if (indexOfBrcCls < 0) return false;
        remainingSpan = toCheck[..indexOfBrcCls];
        var expectIdentifier         = true;
        var expectLayoutSign         = true;
        var expectLayout        = false;
        for (var i = 1; i < remainingSpan.Length; i++)
        {
            var checkChar = remainingSpan[i];
            if (expectIdentifier)
            {
                if(checkChar.IsDigit()) continue;
                if (checkChar.IsComma())
                {
                    expectIdentifier = false;
                    expectLayout = true;
                    continue;
                }
                else { return false; }
            }
            if (expectLayout)
            {
                if (checkChar.IsMinus() && expectLayoutSign)
                {
                    expectLayoutSign = false;
                    continue;
                }
                if(checkChar.IsDigit()) return true;
                else { return false; }
            }
        }
        return false;
    }

    public static bool FormatStringHasFormatSequence(this string formatString) => 
        ((ReadOnlySpan<char>)formatString).HasFormatStringPadding();

    public static bool FormatStringHasFormatSequence(this ReadOnlySpan<char> toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return toCheck.Length > 0;
        var remainingSpan = toCheck[indexOfBrcOpn..];
        var indexOfBrcCls = remainingSpan.IndexOf('}');
        if (indexOfBrcCls < 0) return false;
        remainingSpan = toCheck[..indexOfBrcCls];
        var foundColon         = false;
        var countAfterColon        = 0;
        for (var i = 1; i < remainingSpan.Length; i++)
        {
            var checkChar = remainingSpan[i];
            if (!foundColon)
            {
                if(!checkChar.IsColon()) continue;
                foundColon = true;
            }
            countAfterColon++;
        }
        return foundColon && countAfterColon > 0;
    }

    public static ReadOnlySpan<char> ExtractFormatStringFormatSequence(this ReadOnlySpan<char> toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return toCheck;
        var remainingSpan = toCheck[indexOfBrcOpn..];
        var indexOfBrcCls = remainingSpan.IndexOf('}');
        if (indexOfBrcCls < 0) return toCheck;
        remainingSpan = toCheck[..indexOfBrcCls];
        var indexAfterColon = -1;
        for (var i = 1; i < remainingSpan.Length; i++)
        {
            var checkChar = remainingSpan[i];
            if(!checkChar.IsColon()) continue;
            indexAfterColon = i + 1;
            break;
        }
        
        return indexAfterColon > 0 ? remainingSpan[indexAfterColon..] : "";
    }

    public static Span<char> InjectFormatSequenceIntoExistingFormatString(this Span<char> toBuild, ReadOnlySpan<char> existing, ReadOnlySpan<char> fmtSequence)
    {
        existing.ExtractExtendedStringFormatStages
            (out var prefix, out var identifier, out var sliceLength
           , out var layout, out var splitJoinRange, out _, out var suffix);

        var charsAdded = 0;
        if (prefix.Length > 0)
        {
            charsAdded = toBuild.OverWriteAt(0, prefix);
        }
        charsAdded += toBuild.AddIdentifierAndSliceLength(charsAdded, identifier, sliceLength);
        charsAdded += toBuild.AddPaddingAndSplitJoin(charsAdded, layout, splitJoinRange);
        charsAdded += toBuild.AddFormatSequence(charsAdded, !identifier.IsEmpty, fmtSequence);
        if (!identifier.IsEmpty)
        {
            toBuild[charsAdded++] = '}';
        }
        
        if (suffix.Length > 0)
        {
            charsAdded += toBuild.OverWriteAt(charsAdded, suffix);
        }
        return toBuild[..charsAdded];
    }

    public static int PrefixSuffixLength(this string toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return 0;
        var indexOfBrcCls = toCheck.IndexOf('}');
        if (indexOfBrcCls < 0) return 0;
        return indexOfBrcOpn + toCheck.Length - 1 - indexOfBrcCls;
        
    }

    public static int PrefixSuffixLength(this ReadOnlySpan<char> toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return 0;
        var indexOfBrcCls = toCheck.IndexOf('}');
        if (indexOfBrcCls < 0) return 0;
        return indexOfBrcOpn + toCheck.Length - 1 - indexOfBrcCls;
        
    }

    public static FormatStringType ExtractStandardStringFormatStages
    (this ReadOnlySpan<char> toCheck, out ReadOnlySpan<char> formatPrefix, out ReadOnlySpan<char> identifier
      , out ReadOnlySpan<char> layout, out ReadOnlySpan<char> format, out ReadOnlySpan<char> formatSuffix
      , int fromIndex = 0)
    {
        formatPrefix = toCheck.Slice(0, 0);
        identifier   = toCheck.Slice(0, 0);
        layout       = toCheck.Slice(0, 0);
        format       = toCheck.Slice(0, 0);
        formatSuffix = toCheck.Slice(0, 0);

        var foundFormatStages = FormatStringType.None;
        if (toCheck.Length == 0) return foundFormatStages;
        ReadOnlySpan<char> remainingSpan = toCheck[fromIndex..];

        var indexOfBrcOpn = remainingSpan.IndexOf('{');

        int stage = 0;
        if (indexOfBrcOpn >= 0)
        {
            if (indexOfBrcOpn > 0)
            {
                foundFormatStages |= FormatStringType.Prefix;
                formatPrefix      =  remainingSpan[..fromIndex];
                remainingSpan     =  remainingSpan[fromIndex..];
            }
            var indexOfBrcCls = remainingSpan.IndexOf('}');
            if (indexOfBrcCls == -1)
            {
                throw new FormatException("Invalid format string expected matching closing braces from " + remainingSpan.ToString());
            }
            formatSuffix = remainingSpan[(indexOfBrcCls + 1)..];
            if (indexOfBrcCls < remainingSpan.Length - 1)
            {
                foundFormatStages |= FormatStringType.Suffix;
            }
            remainingSpan = remainingSpan[1..indexOfBrcCls];
        }
        else
        { // just format string
            identifier = "0";
            layout     = "0";
            stage      = 3;
        }
        while (remainingSpan.Length > 0)
        {
            switch (stage)
            {
                case 0:
                    var (nextStage, foundStages, consumedSpan) = IdentifierOnly(remainingSpan, out identifier);

                    stage             =  nextStage;
                    foundFormatStages |= foundStages;
                    remainingSpan     =  remainingSpan[consumedSpan..];
                    break;
                case 1:
                    var (next, foundParts, nextSpanIndex) = LayoutOnly(remainingSpan, out layout);

                    stage             =  next;
                    foundFormatStages |= foundParts;
                    remainingSpan     =  remainingSpan[nextSpanIndex..];
                    break;
                case 2:
                    stage             =  3;
                    format            =  remainingSpan;
                    foundFormatStages |= FormatStringType.Format;
                    break;
                default: return foundFormatStages;
            }
        }
        return foundFormatStages;
    }

    private static (int, FormatStringType, int) IdentifierOnly(ReadOnlySpan<char> remainingSpan, out ReadOnlySpan<char> identifier)
    {
        var stageEnd     = remainingSpan.Length;
        var consumeExtra = 0;
        var nextStage    = 3;
        var colonIndex   = remainingSpan.IndexOf(":");
        if (colonIndex > 0)
        {
            nextStage = 2;
            consumeExtra = 1;
            stageEnd  = colonIndex;
        }
        var commaIndex = remainingSpan.IndexOf(",");
        if (commaIndex > 0 && (commaIndex < colonIndex || colonIndex < 0))
        {
            nextStage    = 1;
            consumeExtra = 1;
            stageEnd     = commaIndex;
        }
        FormatStringType foundFormatStages = FormatStringType.Identifier;
        identifier = remainingSpan[..stageEnd];
        return (nextStage, foundFormatStages, stageEnd + consumeExtra);
    }

    private static (int, FormatStringType, int, Range) IdentifierAndSlice(ReadOnlySpan<char> remainingSpan, out ReadOnlySpan<char> identifier)
    {
        var stageEnd     = remainingSpan.Length;
        var consumeExtra = 0;
        var nextStage    = 3;
        var colonIndex   = remainingSpan.IndexOf(":");
        if (colonIndex > 0)
        {
            nextStage    = 2;
            consumeExtra = 1;
            stageEnd     = colonIndex;
        }
        var commaIndex = remainingSpan.IndexOf(",");
        if (commaIndex > 0 && (commaIndex < colonIndex || colonIndex < 0))
        {
            nextStage    = 1;
            consumeExtra = 1;
            stageEnd     = commaIndex;
        }
        var foundFormatStages = FormatStringType.Identifier;
        var sourceLengthRange = Range.All;
        var squareOpenIndex   = remainingSpan.IndexOf("[");
        if (squareOpenIndex > 0 && (squareOpenIndex < colonIndex || colonIndex < 0)
                                && (squareOpenIndex < commaIndex || commaIndex < 0))
        {
            identifier = remainingSpan[..squareOpenIndex];
            var squareCloseIndex = remainingSpan.IndexOf("]");
            if (squareCloseIndex < 0) throw new FormatException("Invalid format string expected ']' after '[' " + remainingSpan[squareOpenIndex..].ToString());
            var dotdotIndex = remainingSpan.IndexOf("..");
            if (dotdotIndex < 0)
                throw new FormatException("Invalid format string expected \"..\" between '[' and '] " +
                                          remainingSpan[squareOpenIndex..].ToString());
            var rangeFirstChar = remainingSpan[squareOpenIndex + 1];
            var start          = Index.Start;
            if (rangeFirstChar.IsCarat() || rangeFirstChar.IsDigit())
            {
                start = remainingSpan.ExtractIndexSlice(squareOpenIndex + 1);
            }
            remainingSpan  = remainingSpan[(dotdotIndex + 2)..];
            var rangeEndChar = remainingSpan[0];
            var end          = Index.End;
            if (rangeEndChar.IsCarat() || rangeEndChar.IsDigit())
            {
                end = remainingSpan.ExtractIndexSlice(0, true);
            }
            foundFormatStages |= FormatStringType.ExtendedLimitLength;
            sourceLengthRange =  new Range(start, end);
        }
        else
        {
            identifier = remainingSpan[..stageEnd];
        }
        return (nextStage, foundFormatStages, stageEnd + consumeExtra, sourceLengthRange);
    }

    private static (int, FormatStringType, int) LayoutOnly(
        ReadOnlySpan<char> remainingSpan, out ReadOnlySpan<char> layout)
    {
        int colonIndex;
        var stageEnd     = remainingSpan.Length;
        var consumeExtra = 0;
        var nextStage    = 3;
        colonIndex = remainingSpan.IndexOf(":");
        if (colonIndex > 0)
        {
            nextStage    = 2;
            consumeExtra = 1;
            stageEnd     = colonIndex;
        }
        var foundFormatStages = FormatStringType.Layout;
        layout = remainingSpan[..stageEnd];
        return (nextStage, foundFormatStages, stageEnd + consumeExtra);
    }

    private static (int, FormatStringType, int, SplitJoinRange) LayoutAndSplitJoin(
        ReadOnlySpan<char> remainingSpan, out ReadOnlySpan<char> layout)
    {
        int colonIndex;
        var stageEnd     = remainingSpan.Length;
        var consumeExtra = 0;
        var nextStage    = 3;
        colonIndex = remainingSpan.IndexOf(":");
        if (colonIndex > 0)
        {
            nextStage = 2;
            consumeExtra = 1;
            stageEnd  = colonIndex;
        }
        var foundFormatStages      = FormatStringType.Layout;
        var extendedSplitJoinRange = SplitJoinRange.NoSplitJoin;
        var countSlashesIfAny      = remainingSpan.CountOccurences(0, '/', ':', '}');
        if (countSlashesIfAny == 3)
        {
            var firstFwdSlashIndex = remainingSpan.IndexOf('/');
            layout = remainingSpan[..firstFwdSlashIndex];

            remainingSpan = remainingSpan[(firstFwdSlashIndex + 1)..];
            var secondFwdSlashIndex = remainingSpan.IndexOf('/');
            var splitChars          = remainingSpan[..secondFwdSlashIndex];
            remainingSpan = remainingSpan[(secondFwdSlashIndex + 1)..];
            var thirdFwdSlashIndex       = remainingSpan.IndexOf('/');
            var joinChars                = remainingSpan[..thirdFwdSlashIndex];
            var extendedSplitSourceRange = Range.All;
            var squareLayoutOpenIndex    = remainingSpan.IndexOf("[");

            if (squareLayoutOpenIndex > 0 && (squareLayoutOpenIndex < colonIndex || colonIndex < 0))
            {
                var squareCloseIndex = remainingSpan.IndexOf("]");
                if (squareCloseIndex < 0)
                    throw new FormatException("Invalid format string expected ']' after '[' " +
                                              remainingSpan[squareLayoutOpenIndex..].ToString());
                var dotdotIndex = remainingSpan.IndexOf("..");
                if (dotdotIndex < 0)
                    throw new FormatException("Invalid format string expected \"..\" between '[' and '] " +
                                              remainingSpan[squareLayoutOpenIndex..].ToString());
                var rangeFirstChar = remainingSpan[squareLayoutOpenIndex + 1];
                var start          = Index.Start;
                if (rangeFirstChar.IsCarat() || rangeFirstChar.IsDigit())
                {
                    start = remainingSpan.ExtractIndexSlice(squareLayoutOpenIndex + 1);
                }
                remainingSpan  = remainingSpan[(dotdotIndex + 2)..];
                var rangeEndChar = remainingSpan[0];
                var end = Index.End;
                if (rangeEndChar.IsCarat() || rangeEndChar.IsDigit())
                {
                    end = remainingSpan.ExtractIndexSlice(0, true);
                }
                foundFormatStages        |= FormatStringType.ExtendedSplitJoinRange;
                extendedSplitSourceRange =  new Range(start, end);
            }
            extendedSplitJoinRange = new SplitJoinRange(splitChars, joinChars, extendedSplitSourceRange);
        }
        else
        {
            layout = remainingSpan[..stageEnd];
        }
        return (nextStage, foundFormatStages, stageEnd + consumeExtra, extendedSplitJoinRange);
    }

    public static int WriteSplitJoinFormatting(this Span<char> destination, int fromDestIndex, SplitJoinRange splitJoinRange)
    {
        if (splitJoinRange.IsNoSplitJoin) return 0;
        var index = fromDestIndex;
        destination[index++] =  '/';
        index                += splitJoinRange.WriteSplitOn(destination, index);
        destination[index++] =  '/';
        index                += splitJoinRange.WriteJoinOn(destination, index);
        destination[index++] =  '/';
        index                += destination.WriteRangeAsSlice(index, splitJoinRange.SplitElementsRange);
        return index - fromDestIndex;
    }

    public static int CalculateSplitJoinStringSize(this SplitJoinRange splitJoinRange)
    {
        if (splitJoinRange.IsNoSplitJoin) return 0;
        var size = 0;

        size++; //  '/';
        size += splitJoinRange.SplitLength;
        size++; //  '/';
        size += splitJoinRange.JoinLength;
        size++; //  '/';

        size += splitJoinRange.SplitElementsRange.CalculateRangeAsSliceStringSize();
        return size;
    }

    public static ReadOnlySpan<char> ExtractStringFormatStageOnly(this ReadOnlySpan<char> toCheck, int fromIndex = 0)
    {
        var remainingSpan = toCheck[fromIndex..];
        if (remainingSpan[0] != '{') throw new ArgumentException($"toCheck {toCheck} at {fromIndex} was expected to be '{{");
        remainingSpan = toCheck[1..^1];
        var indexOfColon = remainingSpan.IndexOf(":");
        remainingSpan = remainingSpan[(indexOfColon + 1)..];
        return remainingSpan;
    }

    public static int LayoutStringRangeIndexers(this ReadOnlySpan<char> layout, out Range charRange)
    {
        charRange = Range.All;
        var foundAt = layout.ExtractRangeFromSliceExpression(out var nullableCharRange);

        if (foundAt >= 0)
        {
            charRange = nullableCharRange!.Value;
        }
        return foundAt;
    }

    public static int ExtractRangeFromSliceExpression(this ReadOnlySpan<char> layout, out Range? charRange)
    {
        Index startIndex = Index.Start;
        Index endIndex   = Index.End;

        var foundFromStringLength = layout.IndexOf("[");

        charRange = null;
        if (foundFromStringLength < 0)
        {
            return foundFromStringLength;
        }

        if (layout.Length > foundFromStringLength
         && (layout[foundFromStringLength + 1].IsDigit() || layout[foundFromStringLength + 1].IsCarat()))
        {
            startIndex = layout.ExtractIndexSlice(foundFromStringLength + 1);
        }
        var foundCapStringLength = layout.IndexOf("..");
        if (foundCapStringLength >= 0
         && layout.Length > foundCapStringLength
         && (layout[foundCapStringLength + 2].IsDigit() || layout[foundCapStringLength + 2].IsCarat()))
        {
            endIndex = layout.ExtractIndexSlice(foundCapStringLength + 2, true);
        }
        charRange = new Range(startIndex, endIndex);
        return foundFromStringLength;
    }

    public static int PadAndAlign(this Span<char> destination, ReadOnlySpan<char> toInsert, ReadOnlySpan<char> layout)
    {
        if (layout.Length == 0)
        {
            destination.Append(toInsert);
            return toInsert.Length;
        }
        var charsRange     = Range.All;
        var padding        = 0;
        var isLeftAligned  = layout[0].IsMinus();
        var isStringLength = layout[0].IsDigit() || isLeftAligned;
        if (isStringLength)
        {
            layout.LayoutStringRangeIndexers(out var range);
            charsRange = range.BoundRangeToLength(toInsert.Length);

            int.TryParse(layout, out padding);
            padding = Math.Abs(padding);
        }
        var rangeCappedInsert = toInsert[charsRange];
        var rangeCappedSize   = rangeCappedInsert.Length;
        if (isLeftAligned)
        {
            destination.Append(rangeCappedInsert);
            for (int i = rangeCappedSize; i < padding; i++)
            {
                destination[i] = ' ';
            }
            return Math.Max(rangeCappedSize, padding);
        }
        var extraRequired = padding - rangeCappedSize;
        int addValueIndex = 0;
        for (var i = extraRequired - 1; i >= 0; i--)
        {
            destination[i] = ' ';
            addValueIndex++;
        }
        destination.OverWriteAt(addValueIndex, rangeCappedInsert);
        return Math.Max(rangeCappedSize, padding);
    }

    public static int CalculatePaddedAlignedFormatStringLength(this ReadOnlySpan<char> toInsert, ReadOnlySpan<char> toExtractJustLayout)
    {
        if (toExtractJustLayout.Length == 0)
        {
            return toInsert.Length;
        }
        var isLeftAligned  = toExtractJustLayout[0].IsMinus();
        var isStringLength = toExtractJustLayout[0].IsDigit() || isLeftAligned;
        if (isStringLength)
        {
            toExtractJustLayout.LayoutStringRangeIndexers(out var range);

            int.TryParse(toExtractJustLayout, out var padding);
            padding = Math.Abs(padding);

            var finalLength = range.IsAllRange()
                ? Math.Max(padding, toInsert.Length)
                : Math.Max(padding, toInsert[range].Length);
            return finalLength;
        }
        return toInsert.Length;
    }

    public static int CalculatePaddedAlignedLength(this int toInsertLength, ReadOnlySpan<char> layout)
    {
        if (layout.Length == 0)
        {
            return toInsertLength;
        }
        var isLeftAligned  = layout[0].IsMinus();
        var isStringLength = layout[0].IsDigit() || isLeftAligned;
        if (isStringLength)
        {
            layout.LayoutStringRangeIndexers(out var range);

            int.TryParse(layout, out var padding);
            padding = Math.Abs(padding);

            if (range.IsAllRange())
            {
                return Math.Max(padding, toInsertLength);
            }
            var deduct = range.Start.IsFromEnd
                ? Math.Max(0, Math.Min(toInsertLength, toInsertLength - range.Start.Value))
                : range.Start.Value;
            if (range.End.IsFromEnd)
            {
                deduct += Math.Max(0, Math.Min(toInsertLength, range.End.Value));
            }
            else
            {
                deduct += Math.Min(toInsertLength, range.End.Value);
            }

            return Math.Max(padding, toInsertLength - deduct);
        }
        return toInsertLength;
    }

    public static Span<char> ToLayoutOnlyFormatString(this Span<char> toBuild, ReadOnlySpan<char> toExtractLayout)
    {
        toExtractLayout.ExtractExtendedStringFormatStages
            (out _, out _, out _
            , out var layout, out _, out _, out _);

        return MakeLayoutOnlyFormatString(toBuild, layout);
    }

    private static Span<char> MakeLayoutOnlyFormatString(this Span<char> toBuild, ReadOnlySpan<char> layout)
    {
        toBuild.Append("{0");

        if (layout.IsEmpty)
        {
            toBuild.Append("}");
            return toBuild[..3];
        }
        var isLeftAligned  = layout[0].IsMinus();
        var isStringLength = layout[0].IsDigit() || isLeftAligned;
        int padding        = 0;
        if (isStringLength)
        {
            int.TryParse(layout, out padding);
        }
        if (padding != 0)
        {
            toBuild.OverWriteAt(2, ",");
            var insertPadSpan = toBuild[3..];
            padding.TryFormat(insertPadSpan, out var charsWritten, "");
            insertPadSpan.OverWriteAt(charsWritten, "}");
            return toBuild[..(charsWritten + 4)];
        }
        toBuild.Append("}");
        return toBuild[..3];
    }

    private static int AddIdentifierAndSliceLength(this Span<char> toBuild, int atOffset, ReadOnlySpan<char> identifierSpan, Range sliceLengthRange)
    {
        var charsAdded = 0;
        charsAdded += toBuild.OverWriteAt(atOffset, "{");
        charsAdded +=  toBuild.OverWriteAt(atOffset + charsAdded, identifierSpan);

        if (sliceLengthRange.IsAllRange())
        {
            return charsAdded; 
        }
        charsAdded += toBuild.WriteRangeAsSlice(charsAdded, sliceLengthRange);
        return charsAdded;
    }

    private static int AddPaddingAndSplitJoin(this Span<char> toBuild, int atOffset, ReadOnlySpan<char> paddingSpan, SplitJoinRange splitJoinRange)
    {
        if (splitJoinRange.IsNoSplitJoin && paddingSpan.IsEmpty) return 0;
        
        var charsAdded = 0;
        charsAdded += toBuild.OverWriteAt(atOffset, ",");
        if (paddingSpan.Length > 0)
        {
            charsAdded += toBuild.OverWriteAt(atOffset + charsAdded, paddingSpan);
        }
        if (splitJoinRange.IsNoSplitJoin)
        {
            return charsAdded;
        }
        charsAdded += toBuild.WriteSplitJoinAsSlashSearchSlashReplace(atOffset + charsAdded, splitJoinRange);
        
        return charsAdded;
    }

    private static int AddFormatSequence(this Span<char> toBuild, int atOffset, bool isComposite, ReadOnlySpan<char> formatSequenceSpan)
    {
        if (formatSequenceSpan.IsEmpty)
        {
            return 0;
        }
        
        var charsAdded = 0;
        if(isComposite) charsAdded += toBuild.OverWriteAt(atOffset, ":");
        charsAdded += toBuild.OverWriteAt(atOffset + charsAdded, formatSequenceSpan);
        
        return charsAdded;
    }

    public static int CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(this int toInsertLength, ReadOnlySpan<char> formatStringToExtract)
    {
        formatStringToExtract.ExtractExtendedStringFormatStages
            (out var prefix, out _, out _
           , out var layout, out _, out _, out var suffix);
        if (prefix.Length == 0 && layout.Length == 0 && suffix.Length == 0)
        {
            return toInsertLength;
        }
        return CalculatePrefixPaddedAlignedAndSuffixLength(toInsertLength, prefix, layout, suffix);;
    }

    private static int CalculatePrefixPaddedAlignedAndSuffixLength(this int toInsertLength, ReadOnlySpan<char> prefix
      , ReadOnlySpan<char> layout, ReadOnlySpan<char> suffix)
    {
        if (prefix.Length == 0 && layout.Length == 0 && suffix.Length == 0)
        {
            return toInsertLength;
        }
        Span<char> layoutOnlyFormatString = stackalloc char[12];
        layoutOnlyFormatString = layoutOnlyFormatString.MakeLayoutOnlyFormatString(layout);
        return layoutOnlyFormatString.Length + prefix.Length + suffix.Length;;
    }

    public static Span<char> ToPrefixLayoutSuffixOnlyFormatString(this Span<char> toBuild, ReadOnlySpan<char> toExtractLayout)
    {
        toExtractLayout.ExtractExtendedStringFormatStages
            (out var prefix, out _, out _
            , out var layout, out _, out _, out var suffix);
        
        if (prefix.Length > 0)
        {
            toBuild.Append(prefix);
            var layoutFormatBuild = toBuild[prefix.Length..];
            layoutFormatBuild.MakeLayoutOnlyFormatString(layout);
        }
        else
        {
            toBuild.MakeLayoutOnlyFormatString(layout);
        }
        toBuild.Append(suffix);
        var length = toBuild.PopulatedLength();
        return toBuild[..length];
    }
}
