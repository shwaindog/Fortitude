using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Extensions;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool WithinRange(this (int StartIncl, int EndExcl) range, int checkIndex)
    {
        return checkIndex >= range.StartIncl && checkIndex < range.EndExcl;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool WithinRange(this Range range, int checkIndex, int rangeLength)
    {
        var (rangeStart, rangeEnd) = range.BoundRangeToIndexValues(rangeLength);
        if (checkIndex >= rangeStart && checkIndex <= rangeEnd) return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range BoundRangeToLength(this Range formatRange, int length)
    {
        return formatRange.IsAllRange()
            ? formatRange
            : new Range(
                        formatRange.Start.IsFromEnd
                            ? Index.FromEnd(Math.Clamp(formatRange.Start.Value, 0, length))
                            : Index.FromStart(Math.Clamp(formatRange.Start.Value, 0, length))
                      , formatRange.End.IsFromEnd
                            ? Index.FromEnd(Math.Clamp(formatRange.End.Value, 0, length))
                            : Index.FromStart(Math.Clamp(formatRange.End.Value, 0, length)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int StartIncl, int EndExcl) BoundRangeToUnicodeIndexValues(this Range formatRange)
    {
        return formatRange.BoundRangeToIndexValues(CharExtensions.UnicodeCodePoints);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int StartIncl, int EndExcl) BoundRangeToIndexValues(this Range formatRange, int length)
    {
        var start = formatRange.Start.IsFromEnd ? length - formatRange.Start.Value : formatRange.Start.Value;
        var end = formatRange.End.IsFromEnd ? length - formatRange.End.Value : formatRange.End.Value;
        
        return (start, end);
    }

    public static Range Shift(this Range toShift, int delta)
    {
        var start = toShift.Start.Value + delta;
        var end = toShift.End.Value + delta;

        if (start < 0)
        {
            throw new ArgumentException("Did not expect to shift a range to a negative index");
        }
        return new Range(new Index(start), new Index(end));
    }

    public static int Length(this Range calculateIndexes, int entireRangeLength = 0)
    {
        var start = calculateIndexes.Start.IsFromEnd ? entireRangeLength - calculateIndexes.Start.Value: calculateIndexes.Start.Value;
        var end   = calculateIndexes.End.IsFromEnd ? entireRangeLength -  calculateIndexes.End.Value : calculateIndexes.End.Value;
        return end - start;
    }
    
    public static bool IsAllRange(this Range checkRange)
    {
        if (checkRange.Start.IsFromEnd || checkRange.Start.Value > 0) return false;
        if (!checkRange.End.IsFromEnd || checkRange.End.Value > 0) return false;
        return true;
    }

    public static Index ToIndex(this ReadOnlySpan<char> indexOffsetSpan, bool isEndRange = false)
    {
        if (indexOffsetSpan.Length == 0)
        {
            return isEndRange ? Index.End : Index.Start;
        }
        var isFromEnd = indexOffsetSpan[0].IsCarat();
        if (isFromEnd)
        {
            indexOffsetSpan = indexOffsetSpan[1..];
        }
        int digits     = int.TryParse(indexOffsetSpan, out var attempt) ? attempt : 0;
        return isFromEnd ? Index.FromEnd(digits) : Index.FromStart(digits);
    }

    public static int WriteRangeAsSlice(this Span<char> destination, int fromDestIndex, Range toConvert)
    {
        var startIndex = fromDestIndex;
        destination[fromDestIndex++] = '[';
        var start = toConvert.Start;
        if (start.IsFromEnd)
        {
            destination[fromDestIndex++] =  '^';
            fromDestIndex                += destination.WriteIntToSpan(start.Value, fromDestIndex);
        }
        else if (start.Value > 0)
        {
            fromDestIndex += destination.WriteIntToSpan(start.Value, fromDestIndex);
        }
        destination[fromDestIndex++] = '.';
        destination[fromDestIndex++] = '.';
        var end = toConvert.End;
        if (end is { IsFromEnd: true, Value: > 0 })
        {
            destination[fromDestIndex++] =  '^';
            fromDestIndex                += destination.WriteIntToSpan(end.Value, fromDestIndex);
        }
        else if (end is { IsFromEnd : false })
        {
            fromDestIndex += destination.WriteIntToSpan(end.Value, fromDestIndex);
        }
        destination[fromDestIndex++] = ']';
        return fromDestIndex - startIndex;
    }

    public static int CalculateRangeAsSliceStringSize(this Range toConvert)
    {
        var size = 0;
        if (toConvert.IsAllRange()) return 0;
        size++; // '['
        var start = toConvert.Start;
        if (start.IsFromEnd)
        {
            size++; // '^'
            size +=  start.Value.CalculateIntToSpanLength();
        }
        else if (start.Value > 0)
        {
            size +=  start.Value.CalculateIntToSpanLength();
        }
        size++; // '.'
        size++; // '.'
        var end = toConvert.End;
        if (end is { IsFromEnd: true, Value: > 0 })
        {
            size++; // '^'
            size +=  end.Value.CalculateIntToSpanLength();
        }
        else if (end is { IsFromEnd : false })
        {
            size +=  end.Value.CalculateIntToSpanLength();
        }
        size++; // ']'
        return size;
    }
}