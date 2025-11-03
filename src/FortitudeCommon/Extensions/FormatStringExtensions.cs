using System.Runtime.CompilerServices;

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

[InlineArray(4)]
public struct FourChars
{
    private char element;
}

public readonly struct SplitJoinRange : ISpanFormattable
{
    private readonly FourChars splitChars;
    private readonly FourChars joinChars;
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

    public int ApplySplitJoin(Span<char> bufferWritten, ReadOnlySpan<char> original)
    {
        var noSplitFound = true;
        var splitSpan    = splitChars[..splitLength];
        if (!IsNoSplitJoin)
        {
            var splitAt = original.IndexOf(splitSpan);
            noSplitFound = splitAt < 0;
        }
        if (IsNoSplitJoin || noSplitFound)
        {
            var end = Math.Min(bufferWritten.Length, original.Length);
            for (int i = 0; i < end; i++)
            {
                bufferWritten[i] = original[i];
            }
            return end;
        }
        Span<Range> splitRanges    = stackalloc Range[16];
        var         numberOfRanges = original.Split(splitRanges, splitSpan);
        splitRanges = splitRanges[..numberOfRanges];
        var boundRange = splitElementsRange.BoundRangeToLength(splitRanges.Length);
        splitRanges = splitRanges[boundRange];

        var joinSpan = joinChars[..joinLength];
        var bufi     = 0;
        for (int i = 0; i < splitRanges.Length; i++)
        {
            var toCopy = original[splitRanges[i]];
            var end    = Math.Min(bufferWritten.Length - bufi, toCopy.Length);
            for (int j = 0; j < end; j++)
            {
                bufferWritten[bufi++] = toCopy[j];
            }
            if (i < splitRanges.Length - 1)
            {
                for (int j = 0; j < joinSpan.Length && bufi < bufferWritten.Length; j++)
                {
                    bufferWritten[bufi++] = joinSpan[j];
                }
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

    public static int PrefixSuffixLength(this ReadOnlySpan<char> toCheck)
    {
        var indexOfBrcOpn = toCheck.IndexOf('{');
        if (indexOfBrcOpn < 0) return 0;
        var indexOfBrcCls = toCheck.IndexOf('}');
        if (indexOfBrcCls < 0) return 0;
        return indexOfBrcOpn + toCheck.Length - 1 - indexOfBrcCls;
        
    }

    public static FormatStringType ExtractExtendedStringFormatStages
    (this ReadOnlySpan<char> toCheck, out ReadOnlySpan<char> formatPrefix, out ReadOnlySpan<char> identifier, out Range extendedSliceLengthRange
      , out ReadOnlySpan<char> layout, out ReadOnlySpan<char> format, out ReadOnlySpan<char> formatSuffix
      , int fromIndex = 0)
    {
        formatPrefix             = toCheck.Slice(0, 0);
        identifier               = toCheck.Slice(0, 0);
        extendedSliceLengthRange = Range.All;
        layout                   = toCheck.Slice(0, 0);
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
                    var (nextStage, foundStages, consumedSpan, lengthSliceRange) = IdentifierAndSlice(remainingSpan, out identifier);

                    stage                    =  nextStage;
                    foundFormatStages        |= foundStages;
                    remainingSpan            =  remainingSpan[consumedSpan..];
                    extendedSliceLengthRange =  lengthSliceRange;
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

    public static int CalculatePaddedAlignedLength(this ReadOnlySpan<char> toInsert, ReadOnlySpan<char> layout)
    {
        if (layout.Length == 0)
        {
            return toInsert.Length;
        }
        var isLeftAligned  = layout[0].IsMinus();
        var isStringLength = layout[0].IsDigit() || isLeftAligned;
        if (isStringLength)
        {
            layout.LayoutStringRangeIndexers(out var range);

            int.TryParse(layout, out var padding);
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
        toBuild.Append("{0");
        
        toExtractLayout.ExtractExtendedStringFormatStages
            (out _, out _, out _
            , out var layout, out _, out _, out _);

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
        if (padding > 0)
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
}
