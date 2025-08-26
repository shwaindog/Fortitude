using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Extensions;

public static class RangeExtensions
{

    public static Range Shift(this Range toShift, int delta)
    {
        var start = toShift.Start.Value + delta;
        var end = toShift.Start.Value + delta;

        if (start < 0)
        {
            throw new ArgumentException("Did not expect to shift a range to a negative index");
        }
        return new Range(new Index(start), new Index(end));
    }

    public static int Length(this Range toShift)
    {
        var start = toShift.Start.Value;
        var end = toShift.End.Value;
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
}