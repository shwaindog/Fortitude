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
}