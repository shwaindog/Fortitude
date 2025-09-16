// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;

namespace FortitudeCommon.Types.Mutable.Strings;

public interface IStringBuilder : ICharSequence, IMutableStringBuilder<IStringBuilder>, ICloneable<IStringBuilder>
{
    new int Length { get; set; }
    new char this[int index] { get; set; }

    IStringBuilder Substring(int startIndex);
    IStringBuilder Substring(int startIndex, int length);
    IStringBuilder Trim();
    IStringBuilder ToLower();
    IStringBuilder Remove(int startIndex);
    IStringBuilder ToUpper();
    
    int LineChars { get; }
    int LineContentStartColumn { get; }
    int LineContentWidth { get; }

    IStringBuilder CopyFrom(string source);

    new IStringBuilder Clone();
}

public interface IScopeDelimitedStringBuilder : IStringBuilder, IDisposable
{
    Action<IScopeDelimitedStringBuilder>? OnScopeEndedAction { get; set; }
}

public static class IStringBuilderExtensions
{
    public static IStringBuilder ReplaceAtRange(this IStringBuilder toMutate, Range rangeToReplace, ICharSequence? replaceWith)
    {
        var replaceStart     = rangeToReplace.Start.Value;
        var toReplaceSize    = rangeToReplace.End.Value - replaceStart;
        var replaceLength    = replaceWith?.Length ?? 0;
        var updatedSizeDelta = replaceLength - toReplaceSize;
        if (updatedSizeDelta > 0)
        {
            toMutate.Length += updatedSizeDelta;
            var endOfToken = replaceStart + toReplaceSize + updatedSizeDelta;
            for (var i = toMutate.Length - 1; i >= endOfToken; i--)
            {
                toMutate[i] = toMutate[i - updatedSizeDelta];
            }
        }
        var endAt   = replaceStart + replaceLength;
        var sbIndex = 0;
        for (var i = replaceStart; i < endAt && sbIndex < replaceLength; i++)
        {
            toMutate[i] = replaceWith![sbIndex++];
        }
        if (updatedSizeDelta < 0)
        {
            for (var i = endAt - updatedSizeDelta; i < toMutate.Length; i++)
            {
                toMutate[i + updatedSizeDelta] = toMutate[i];
            }
            toMutate.Length += updatedSizeDelta;
        }
        return toMutate;
    }

    public static IStringBuilder AppendRange(this IStringBuilder toAppendTo, StringBuilder source)
    {
        for (var i = 0; i < source.Length; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static IStringBuilder AppendRange(this IStringBuilder toAppendTo, ICharSequence source)
    {
        for (var i = 0; i < source.Length; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static IStringBuilder AppendRange(this IStringBuilder toAppendTo, ICharSequence source, int startAt, int length)
    {
        var endAt = startAt + length;
        for (var i = startAt; i < source.Length & i < endAt; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static IStringBuilder InsertAt(this IStringBuilder toExtend, string source, int insertIndex = 0)
    {
        toExtend.ShiftRightAt(insertIndex, source.Length);
        toExtend.OverwriteAt(insertIndex, source);
        return toExtend;
    }

    public static IStringBuilder InsertAt(this IStringBuilder toExtend, StringBuilder source, int insertIndex = 0)
    {
        toExtend.ShiftRightAt(insertIndex, source.Length);
        toExtend.OverwriteAt(insertIndex, source);
        return toExtend;
    }

    public static IStringBuilder InsertAt(this IStringBuilder toExtend, ICharSequence source, int insertIndex = 0)
    {
        toExtend.ShiftRightAt(insertIndex, source.Length);
        toExtend.OverwriteAt(insertIndex, source);
        return toExtend;
    }

    public static IStringBuilder InsertAt(this IStringBuilder toExtend, Span<char> source, int insertIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length, count);
        toExtend.ShiftRightAt(insertIndex, cappedLength);
        toExtend.OverwriteAt(insertIndex, source, cappedLength);
        return toExtend;
    }

    public static IStringBuilder OverwriteAt(this IStringBuilder toWriteTo, int insertIndex, string source)
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

    public static IStringBuilder OverwriteAt(this IStringBuilder toWriteTo, int insertIndex, Span<char> source, int length)
    {
        var endSizeDiff = source.Length - (toWriteTo.Length - insertIndex);
        if (endSizeDiff > 0)
        {
            toWriteTo.Length += endSizeDiff;
        }
        for (var i = 0; i < source.Length && i < length; i++)
        {
            toWriteTo[insertIndex + i] = source[i];
        }
        return toWriteTo;
    }

    public static IStringBuilder OverwriteAt(this IStringBuilder toWriteTo, int insertIndex, ICharSequence source)
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

    public static IStringBuilder OverwriteAt(this IStringBuilder toWriteTo, int insertIndex, StringBuilder source)
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

    public static IStringBuilder ShiftRightAt(this IStringBuilder toMutate, int from, int by)
    {
        if (by < 0) return toMutate;
        var oldLength = toMutate.Length;
        toMutate.EnsureCapacity(by);

        for (var i = oldLength - 1 + by; i >= from + by; i--)
        {
            toMutate[i] = toMutate[i - by];
        }
        return toMutate;
    }
}