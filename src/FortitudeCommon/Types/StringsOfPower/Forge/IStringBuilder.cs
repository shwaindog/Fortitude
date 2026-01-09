// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

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

    public static IStringBuilder InsertAt(this IStringBuilder toExtend, ReadOnlySpan<char> source, int insertIndex = 0, int count = int.MaxValue)
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

    public static IStringBuilder OverwriteAt(this IStringBuilder toWriteTo, int insertIndex, ReadOnlySpan<char> source, int length)
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
        var oldLength = toMutate.Length;
        if (by < 0 || oldLength <= from) return toMutate;
        toMutate.EnsureCapacity(by);
        toMutate.Length = oldLength + by;
        for (var i = oldLength - 1 + by; i >= from + by; i--)
        {
            toMutate[i] = toMutate[i - by];
        }
        return toMutate;
    }
    
    
    public static IStringBuilder CopyAndMakeWhiteSpaceVisible(this IStringBuilder input, IStringBuilder destSb)
    {
        var sb = destSb;
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            switch (c)
            {
                case ' ' :  sb.Append('\u00B7'); break;
                case '\t' : sb.Append('\u21E5'); break;
                case '\r' :
                    sb.Append('\u00B6');
                    sb.Append('\r');
                    break;
                case '\n' : 
                    if (i - 1 >= 0)
                    {
                        var prev = input[i - 1];
                        if (prev == '\r' && sb.Length - 2 > 0 && sb[^2] == '\u00B6')
                        {
                            sb.Append('\n');
                        }
                        else
                        {
                            sb.Append('\u00B6');
                            sb.Append('\n');
                        }
                    }
                    else
                    {
                        sb.Append('\u00B6');
                        sb.Append('\n');
                    }
                    break;
                default:  sb.Append(c); break;
            }
        }
        return sb;
    }
    
    public static IStringBuilder IndentSubsequentLines(this IStringBuilder input, string indentChars = "  ")
    {
        Span<char> replace = stackalloc char[indentChars.Length + 1];
        replace.OverWriteAt(0, "\n");
        replace.OverWriteAt(1, indentChars);
        return input.Replace("\n", replace);
    }
    
    public static bool IsBrcBounded(this IStringBuilder input)
    {
        var hasTwoOpeningBraces  = input.Length > 1 && input[0] == '{';
        if(!hasTwoOpeningBraces) return false;
        var hasTwoClosingBraces = input[^1] == '}';
        if(!hasTwoClosingBraces) return false;
        for (int i = 1; i < input.Length && hasTwoOpeningBraces; i++)
        {
            var checkChar = input[i];
            if (checkChar == '{')
            {
                break;
            }
            if (checkChar == '}')
            {
                hasTwoOpeningBraces = false;
                break;
            }
        }
        for (int i = input.Length - 2; i >= 0 && hasTwoClosingBraces; i--)
        {
            var checkChar = input[i];
            if (checkChar == '}')
            {
                break;
            }
            if (checkChar == '{')
            {
                hasTwoClosingBraces = false;
                break;
            }
        }
        return hasTwoOpeningBraces && hasTwoClosingBraces;
    }
    
    public static bool HasAnyPairedBrc(this IStringBuilder input)
    {
        var foundOpeningBrace = input.Length > 1 && input[0] == '{';
        var foundClosingBrace = input.Length > 1 && input[^1] == '}';
        if (foundOpeningBrace && foundClosingBrace) return true;
        int i = 0;
        for (; i < input.Length && !foundOpeningBrace; i++)
        {
            var checkChar = input[i];
            if (checkChar == '{')
            {
                foundOpeningBrace = true;
                break;
            }
        }
        if (!foundOpeningBrace) return false;
        int j = input.Length - 1;
        for (; j > i && !foundClosingBrace; j--)
        {
            var checkChar = input[j];
            if (checkChar == '}')
            {
                foundClosingBrace = true;
                break;
            }
        }
        return foundOpeningBrace && foundClosingBrace;
    }
}