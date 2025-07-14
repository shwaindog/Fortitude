using System.Text;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendRange(this StringBuilder toAppendTo, StringBuilder source)
    {
        for (var i = 0; i < source.Length; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static StringBuilder AppendRange(this StringBuilder toAppendTo, IFrozenString source)
    {
        for (var i = 0; i < source.Length; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static StringBuilder InsertAt(this StringBuilder toExtend, StringBuilder source, int insertIndex = 0)
    {
        toExtend.ShiftRightAt(insertIndex, source.Length);
        toExtend.OverwriteAt(insertIndex, source);
        return toExtend;
    }

    public static StringBuilder InsertAt(this StringBuilder toExtend, IFrozenString source, int insertIndex = 0)
    {
        toExtend.ShiftRightAt(insertIndex, source.Length);
        toExtend.OverwriteAt(insertIndex, source);
        return toExtend;
    }

    public static StringBuilder OverwriteAt(this StringBuilder toWriteTo, int insertIndex, StringBuilder source)
    {
        var endSizeDiff = source.Length - (toWriteTo.Length - insertIndex);
        if (endSizeDiff > 0)
        {
            toWriteTo.Length += endSizeDiff;
        }
        for (var i = 0; i < source.Length; i++)
        {
            toWriteTo[insertIndex + i] = source[i];
        }
        return toWriteTo;
    }

    public static StringBuilder OverwriteAt(this StringBuilder toWriteTo, int insertIndex, IFrozenString source)
    {
        var endSizeDiff = source.Length - (toWriteTo.Length - insertIndex);
        if (endSizeDiff > 0)
        {
            toWriteTo.Length += endSizeDiff;
        }
        for (var i = 0; i < source.Length; i++)
        {
            toWriteTo[insertIndex + i] = source[i];
        }
        return toWriteTo;
    }

    public static StringBuilder ShiftRightAt(this StringBuilder toMutate, int from, int by)
    {
        if (by < 0) return toMutate;

        toMutate.Length += by;
        for (var i = toMutate.Length - 1 + by; i >= from; i--)
        {
            toMutate[i] = toMutate[i - by];
        }
        return toMutate;
    }

    public static StringBuilder ReplaceAtRange(this StringBuilder toMutate, Range rangeToReplace, StringBuilder replaceWith)
    {
        var replaceStart     = rangeToReplace.Start.Value;
        var toReplaceSize    = rangeToReplace.End.Value - replaceStart;
        var replaceLength    = replaceWith.Length;
        var updatedSizeDelta = replaceLength - toReplaceSize;
        if (updatedSizeDelta > 0)
        {
            toMutate.Length += updatedSizeDelta;
            var endOfToken = replaceStart + replaceLength;
            for (var i = toMutate.Length - 1 + updatedSizeDelta; i > endOfToken; i--)
            {
                toMutate[i] = toMutate[i - updatedSizeDelta];
            }
        }
        var endAt   = replaceStart + replaceLength;
        var sbIndex = 0;
        for (var i = replaceStart; i < endAt && sbIndex < replaceLength; i++)
        {
            toMutate[i] = replaceWith[sbIndex];
        }
        if (updatedSizeDelta < 0)
        {
            var lastReplace = replaceStart + replaceLength;
            for (var i = lastReplace; i < toMutate.Length; i++)
            {
                toMutate[i] = toMutate[i - updatedSizeDelta];
            }
            toMutate.Length += updatedSizeDelta;
        }
        return toMutate;
    }

}
