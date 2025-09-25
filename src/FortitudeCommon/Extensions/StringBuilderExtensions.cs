using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

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

    public static StringBuilder AppendRange(this StringBuilder toAppendTo, ICharSequence source)
    {
        for (var i = 0; i < source.Length; i++)
        {
            toAppendTo.Append(source[i]);
        }
        return toAppendTo;
    }

    public static StringBuilder AppendRange(this StringBuilder toAppendTo, ICharSequence source, int startAt, int length)
    {
        var endAt = startAt + length;
        for (var i = startAt; i < source.Length & i < endAt; i++)
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

    public static StringBuilder InsertAt(this StringBuilder toExtend, ICharSequence source, int insertIndex = 0)
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

    public static StringBuilder OverwriteAt(this StringBuilder toWriteTo, int insertIndex, ICharSequence source)
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

    public static string BuildStringBuilderFormatting(this int param, int padding = 0, string formatting = "")
    {
        var stringLength = param.NumOfDigits() + 2;
        stringLength += padding > 0 ? padding.NumOfDigits() + 1 : 0;
        stringLength += formatting.Length > 0 ? formatting.Length + 1 : 0;
        var buildChars = stackalloc char[stringLength].ResetMemory();
        var index      = 0;
        buildChars[index++] = '{';
        for (int i = 0; i < 1 || param > 0; i++)
        {
            var lowestDigit = param % 10;
            var digitToAdd  = (char)('0' + lowestDigit); 
            buildChars[index++] = digitToAdd;
            param /= 10;
        }
        if (padding > 0)
        {
            buildChars[index++] = ',';
            for (int i = 0; i < padding.NumOfDigits(); i++)
            {
                var lowestDigit = padding % 10;
                var digitToAdd  = (char)('0' + lowestDigit); 
                buildChars[index++] = digitToAdd;
                padding /= 10;
            }
        }
        if (formatting.Length > 0)
        {
            buildChars[index++] = ',';
            for (int i = 0; i < formatting.Length; i++)
            {
                buildChars[index++] = formatting[i];
            }
        }
        buildChars[index++] = '}';
        return buildChars.ToString();
    }
    
    public static bool IsEquivalentTo(this StringBuilder search, string checkIsSame, int fromIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(count, checkIsSame.Length - fromIndex);
        if(checkIsSame.Length == cappedLength) return false;
        for (int i = 0; i < search.Length; i++)
        {
            var checkChar   = search[fromIndex + i];
            var compareChar = checkIsSame[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }

    public static IEnumerator<char> RecycledEnumerator(this StringBuilder sb, IRecycler recycler) =>
         Recycler.ThreadStaticRecycler.Borrow<RecyclingStringBuilderEnumerator>().Initialize(sb);


    
    private class RecyclingStringBuilderEnumerator : RecyclableObject, IEnumerator<char>
    {
        private StringBuilder? sb;

        private int currentPosition = -1;

        public RecyclingStringBuilderEnumerator Initialize(StringBuilder sBuilder)
        {
            sb = sBuilder;

            currentPosition = -1;

            return this;
        }

        public void Dispose()
        {
            Reset();
            sb = null;
            DecrementRefCount();
        }

        public bool MoveNext() => ++currentPosition < sb!.Length;

        public void Reset()
        {
            currentPosition = -1;
        }

        public char Current => sb![currentPosition];

        object IEnumerator.Current => Current;
    }

}
