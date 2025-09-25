using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Extensions;

public static class CharSpanExtensions
{
    public const char Terminator = '\0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToLowerChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return AppendLowestByteAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToUpperChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToLowerChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return AppendLowestShortAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToUpperChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsLowerHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 8) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsLowerHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 7] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 6] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 5] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset + 4] = (value >> 12).BottomNibbleToLowerChar();
        buffer[bufferOffset + 3] = (value >> 16).BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 20).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 24).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 28).BottomNibbleToLowerChar();
        return 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsUpperHex(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return AppendLowestShortAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsUpperHexUnchecked(this Span<char> buffer, int value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 7] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 6] = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 5] = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset + 4] = (value >> 12).BottomNibbleToUpperChar();
        buffer[bufferOffset + 3] = (value >> 16).BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 20).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 24).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 28).BottomNibbleToLowerChar();
        return 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset++] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToLowerChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return AppendLowestByteAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset++] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToUpperChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToLowerChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return AppendLowestShortAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToUpperChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsLowerHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 8) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsLowerHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 7] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 6] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 5] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset + 4] = (value >> 12).BottomNibbleToLowerChar();
        buffer[bufferOffset + 3] = (value >> 16).BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 20).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 24).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 28).BottomNibbleToLowerChar();
        return 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsUpperHex(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return AppendLowestShortAsUpperHexUnchecked(buffer, (long)value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendAsUpperHexUnchecked(this Span<char> buffer, uint value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 7] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 6] = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 5] = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset + 4] = (value >> 12).BottomNibbleToUpperChar();
        buffer[bufferOffset + 3] = (value >> 16).BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 20).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 24).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 28).BottomNibbleToLowerChar();
        return 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHex(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHexUnchecked(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToLowerChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHex(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsUpperHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHexUnchecked(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToUpperChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHex(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHexUnchecked(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToLowerChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHex(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestShortAsUpperHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHexUnchecked(this Span<char> buffer, long value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToUpperChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendShortByteAsLowerHex(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsLowerHexUnchecked(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToLowerChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHex(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestByteAsUpperHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestByteAsUpperHexUnchecked(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 1] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 4).BottomNibbleToUpperChar();
        return 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHex(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 4) return 0;
        return buffer.AppendLowestShortAsLowerHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsLowerHexUnchecked(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToLowerChar();
        buffer[bufferOffset + 2] = (value >> 4).BottomNibbleToLowerChar();
        buffer[bufferOffset + 1] = (value >> 8).BottomNibbleToLowerChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToLowerChar();
        return 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHex(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        if (buffer.Length - bufferOffset < 2) return 0;
        return buffer.AppendLowestShortAsUpperHexUnchecked(value, bufferOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AppendLowestShortAsUpperHexUnchecked(this Span<char> buffer, ulong value, int bufferOffset = 0)
    {
        buffer[bufferOffset + 3] = value.BottomNibbleToUpperChar();
        buffer[bufferOffset + 2]   = (value >> 4).BottomNibbleToUpperChar();
        buffer[bufferOffset + 1]   = (value >> 8).BottomNibbleToUpperChar();
        buffer[bufferOffset]   = (value >> 12).BottomNibbleToUpperChar();
        return 4;
    }

    public static Span<char> ResetMemory(this Span<char> toClear, int from = 0, int cappedResetLength = int.MaxValue)
    {
        var length = Math.Min(toClear.Length, cappedResetLength);
        for (int i = from; i < length; i++)
        {
            toClear[i] = Terminator;
        }
        return toClear;
    }

    public static int IndexOf(this Span<char> subject, char toFind = Terminator, int startingAtIndex = 0)
    {
        var length = subject.Length;
        int i      = startingAtIndex;
        for (; i < length; i++)
        {
            var checkChar = subject[i];
            if (checkChar == toFind) return i;
        }
        return -1;
    }

    public static int IndexOf(this Span<char> subject, ICharSequence toFind, int startingAtIndex = 0)
    {
        var searchLength = subject.Length - toFind.Length;
        int i            = startingAtIndex;

        var lastFindIndex = toFind.Length - 1;
        for (; i <= searchLength; i++)
        {
            for (var j = 0; j < toFind.Length; j++)
            {
                var checkChar   = subject[i + j];
                var compareChar = toFind[j];
                if (checkChar != compareChar) break;
                if (j == lastFindIndex) return i;
            }
        }
        return -1;
    }

    public static int IndexOf(this ReadOnlySpan<char> subject, string toFind, int startingAtIndex)
    {
        var searchLength = subject.Length - toFind.Length;
        int i            = startingAtIndex;

        var lastFindIndex = toFind.Length - 1;
        for (; i <= searchLength; i++)
        {
            for (var j = 0; j < toFind.Length; j++)
            {
                var checkChar   = subject[i + j];
                var compareChar = toFind[j];
                if (checkChar != compareChar) break;
                if (j == lastFindIndex) return i;
            }
        }
        return -1;
    }

    public static int IndexOf(this Span<char> subject, StringBuilder toFind, int startingAtIndex = 0)
    {
        var searchLength = subject.Length - toFind.Length;
        int i            = startingAtIndex;

        var lastFindIndex = toFind.Length - 1;
        for (; i <= searchLength; i++)
        {
            for (var j = 0; j < toFind.Length; j++)
            {
                var checkChar   = subject[i + j];
                var compareChar = toFind[j];
                if (checkChar != compareChar) break;
                if (j == lastFindIndex) return i;
            }
        }
        return -1;
    }

    public static int CountWhiteSpaceBackwardsFrom(this Span<char> searchSpace, int fromIndex)
    {
        var startIndex      = Math.Min(searchSpace.Length, fromIndex);
        var countWhiteSpace = 0;
        for (int i = startIndex; i >= 0; i--)
        {
            if (!searchSpace[i].IsWhiteSpace()) break;
        }
        return countWhiteSpace;
    }

    public static int CountWhiteSpaceFrom(this Span<char> searchSpace, int searchLength, int fromIndex)
    {
        var startIndex      = fromIndex;
        var stopIndex       = Math.Min(searchSpace.Length, searchLength);
        var countWhiteSpace = 0;
        for (int i = startIndex; i < stopIndex; i++)
        {
            if (!searchSpace[i].IsWhiteSpace()) break;
        }
        return countWhiteSpace;
    }

    public static int SplitCount(this ReadOnlySpan<char> stringAsSpan, char splitChar)
    {
        var length        = stringAsSpan.Length;
        int lastCharIndex = length - 1;
        int countSplits   = 1;
        for (var i = 0; i < lastCharIndex; i++)
        {
            var checkChar = stringAsSpan[i];
            if (checkChar == splitChar)
            {
                countSplits++;
            }
        }
        return countSplits;
    }

    public static ReadOnlySpan<char> SplitAt(this ReadOnlySpan<char> stringAsSpan, char splitChar, int splitIndex)
    {
        var length          = stringAsSpan.Length;
        var lastCharIndex   = length - 1;
        var countSplits     = 0;
        var startedCountEnd = false;

        var splitStartCharIndex = 0;
        var splitEndCharIndex   = length;

        for (var i = 0; i < lastCharIndex; i++)
        {
            if (!startedCountEnd && splitIndex == countSplits)
            {
                splitStartCharIndex = i;
                startedCountEnd     = true;
            }
            var checkChar = stringAsSpan[i];
            if (checkChar == splitChar)
            {
                if (startedCountEnd)
                {
                    splitEndCharIndex = i;
                    break;
                }
                countSplits++;
            }
        }
        return stringAsSpan[splitStartCharIndex..splitEndCharIndex];
    }

    public static ReadOnlySpan<char> LastSplitFrom(this ReadOnlySpan<char> stringAsSpan, char splitChar)
    {
        var length = stringAsSpan.Length;
        int i      = length - 1;
        for (; i >= 0; i--)
        {
            var checkChar = stringAsSpan[i];
            if (checkChar == splitChar)
            {
                var newLength = length - i + 1;
                return stringAsSpan.Slice(i + 1, newLength);
            }
        }
        return stringAsSpan;
    }

    public static int ReplaceCapped(this Span<char> searchSpace, int searchPopLength, string find, string replace, int occurences = int.MaxValue)
    {
        var findSpan    = find.AsSpan();
        var replaceSpan = replace.AsSpan();
        return ReplaceCapped(searchSpace, searchPopLength, findSpan, replaceSpan, occurences);
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, ICharSequence find, ICharSequence replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int ReplaceCapped
        (this Span<char> searchSpace, int searchPopLength, StringBuilder find, StringBuilder replace, int occurences = int.MaxValue)
    {
        int lengthChange    = 0;
        var fromToDeltaSize = find.Length - replace.Length;

        var found = 0;

        int indexOfFind;
        do
        {
            indexOfFind = searchSpace.IndexOf(find);
            if (indexOfFind >= 0)
            {
                if (fromToDeltaSize > 0)
                {
                    var startIndex = Math.Min(searchSpace.Length - 1, searchPopLength + fromToDeltaSize);
                    var stopIndex  = indexOfFind + find.Length;
                    for (int i = startIndex; i > stopIndex; i--)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                else if (fromToDeltaSize < 0)
                {
                    var startIndex = indexOfFind + find.Length;
                    var stopIndex  = Math.Min(searchSpace.Length - 1, searchPopLength);
                    for (int i = startIndex; i < stopIndex; i++)
                    {
                        searchSpace[i] = searchSpace[i - fromToDeltaSize];
                    }
                    lengthChange    += fromToDeltaSize;
                    searchPopLength += fromToDeltaSize;
                }
                found++;
                searchSpace.OverWriteAt(indexOfFind, replace);
                searchSpace = searchSpace.Slice(indexOfFind + replace.Length);
            }
        } while (indexOfFind >= 0 && found < occurences);
        return lengthChange;
    }

    public static int PopulatedLength(this Span<char> subject)
    {
        var terminatingIndex = IndexOf(subject);
        return terminatingIndex >= 0 ? terminatingIndex : subject.Length;
    }

    public static Span<char> OverWriteAt(this Span<char> toUpdate, int spanIndex, char item)
    {
        toUpdate[spanIndex] = item;
        return toUpdate;
    }

    public static int OverWriteAt(this Span<char> toUpdate, int spanIndex, ReadOnlySpan<char> toAppend)
    {
        var charsWritten = 0;
        for (int i = 0; i < toAppend.Length && spanIndex < toUpdate.Length; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
            charsWritten++;
        }
        return charsWritten;
    }

    public static int OverWriteAt
        (this Span<char> toUpdate, int spanIndex, ICharSequence toAppend, int buildIndex = 0, int builderMaxLength = int.MaxValue)
    {
        var spanLength = toUpdate.Length;
        builderMaxLength = Math.Min(builderMaxLength, toAppend.Length - buildIndex);
        var charsWritten = 0;
        for (int i = buildIndex; i < builderMaxLength && spanIndex < spanLength; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
            charsWritten++;
        }
        return charsWritten;
    }

    public static int OverWriteAt
        (this Span<char> toUpdate, int spanIndex, StringBuilder toAppend, int buildIndex = 0, int builderMaxLength = int.MaxValue)
    {
        var spanLength = toUpdate.Length;
        builderMaxLength = Math.Min(builderMaxLength, toAppend.Length - buildIndex);
        var charsWritten = 0;
        for (int i = buildIndex; i < builderMaxLength && spanIndex < spanLength; i++)
        {
            toUpdate[spanIndex++] = toAppend[i];
            charsWritten++;
        }
        return charsWritten;
    }

    public static Span<char> Append(this Span<char> toUpdate, char toAppend, ref int populatedLength)
    {
        toUpdate[populatedLength++] = toAppend;
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, char toAppend)
    {
        var updateLength = toUpdate.PopulatedLength();
        toUpdate[updateLength] = toAppend;
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, string toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static int AppendReturnAddCount(this Span<char> toUpdate, string toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        int addedChars   = 0;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
            addedChars++;
        }
        return addedChars;
    }

    public static Span<char> Append(this Span<char> toUpdate, ReadOnlySpan<char> toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, Span<char> toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, char[] toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, ICharSequence toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static Span<char> Append(this Span<char> toUpdate, StringBuilder toAppend, int startIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Clamp(count, 0, toAppend.Length - startIndex);
        var startAt      = toUpdate.PopulatedLength();
        cappedLength = Math.Clamp(cappedLength, 0, toUpdate.Length - startAt);
        var endIndex     = startIndex + cappedLength;
        var updateLength = startAt;
        for (int i = startIndex; i < endIndex; i++)
        {
            toUpdate[updateLength++] = toAppend[i];
        }
        return toUpdate;
    }

    public static int AppendInt(this Span<char> toUpdate, int toFormatAppend)
    {
        var startAt      = toUpdate.PopulatedLength();
        if (startAt != 0)
        {
            toUpdate = toUpdate[startAt..];
        }
        if (toFormatAppend.TryFormat(toUpdate, out var written))
        {
            return written;
        }
        return 0;
    }

    public static int AppendEnum<T>(this Span<char> toUpdate, T toFormatAppend) where T : Enum, ISpanFormattable
    {
        var startAt      = toUpdate.PopulatedLength();
        if (startAt != 0)
        {
            toUpdate = toUpdate[startAt..];
        }
        if (toFormatAppend.TryFormat(toUpdate, out var written, "F", null))
        {
            return written;
        }
        return 0;
    }

    public static int AppendRange(this Span<char> toUpdate, Range toConvert) 
    {
        var startAt      = toUpdate.PopulatedLength();
        return toUpdate.WriteRangeAsSlice(startAt, toConvert);
    }

    public static bool IsEndOf(this Span<char> subject, string checkSameChars, int subjectPopulatedLength = -1)
    {
        if (subjectPopulatedLength < 0)
        {
            subjectPopulatedLength = subject.PopulatedLength();
        }
        var compareIndex = checkSameChars.Length - 1;
        for (int i = subjectPopulatedLength - 1; i >= 0 && compareIndex >= 0; i--)
        {
            var bufferChar = subject[i];
            var checkChar  = checkSameChars[compareIndex--];
            if (bufferChar != checkChar)
            {
                return false;
            }
        }
        return true;
    }

    public static bool SequenceMatches(this ReadOnlySpan<char> toCheck, string matchWith, int fromIndex = 0)
    {
        var matchLength = matchWith.Length;
        var matchIndex  = 0;
        for (var i = fromIndex; i < toCheck.Length && matchIndex < matchLength; i++)
        {
            if (toCheck[i] != matchWith[matchIndex++]) return false;
        }
        return matchIndex == matchLength;
    }

    public static bool SequenceMatches(this Span<char> toCheck, string matchWith, int fromIndex = 0)
    {
        var matchLength = matchWith.Length;
        var matchIndex  = 0;
        for (var i = fromIndex; i < toCheck.Length && matchIndex < matchLength; i++)
        {
            if (toCheck[i] != matchWith[matchIndex++]) return false;
        }
        return matchIndex == matchLength;
    }

    public static bool SequenceMatches(this ReadOnlySpan<char> toCheck, ReadOnlySpan<char> matchWith, int fromIndex = 0)
    {
        var matchLength = matchWith.Length;
        var matchIndex  = 0;
        for (var i = fromIndex; i < toCheck.Length && matchIndex < matchLength; i++)
        {
            if (toCheck[i] != matchWith[matchIndex++]) return false;
        }
        return matchIndex == matchLength;
    }

    public static int SequenceIndexOfAnyOf(this ReadOnlySpan<char> toCheck, string[] matchCandidates, int fromIndex = 0)
    {
        for (var i = 0; i < matchCandidates.Length; i++)
        {
            if (toCheck.SequenceMatches(matchCandidates[i], fromIndex)) return i;
        }
        return -1;
    }

    public static string? SequenceIndexOfAnyOf(this ReadOnlySpan<char> toCheck, (string Open, string Close) matchCandidates, int fromIndex = 0)
    {
        if (toCheck.SequenceMatches(matchCandidates.Open, fromIndex)) return matchCandidates.Open;
        if (toCheck.SequenceMatches(matchCandidates.Close, fromIndex)) return matchCandidates.Close;
        return null;
    }

    public static readonly (string Open, string Close) DefaultTokenStartEndDelimiter = ("{", "}");

    public static bool ContainsTokens(this ReadOnlySpan<char> maybeTokenFormatting, (string Open, string Close)? tokenDelimiter = null)
    {
        tokenDelimiter ??= DefaultTokenStartEndDelimiter;
        var openClose    = tokenDelimiter ?? DefaultTokenStartEndDelimiter;
        var foundOpening = false;
        var foundClosing = false;
        for (int i = 0; i < maybeTokenFormatting.Length; i++)
        {
            var delimiterMatch = maybeTokenFormatting.SequenceIndexOfAnyOf(openClose, i);
            if (delimiterMatch == openClose.Open)
            {
                foundOpening = true;
            }
            if (delimiterMatch == openClose.Close && foundOpening)
            {
                foundClosing = true;
            }
        }
        return foundOpening && foundClosing;
    }
    //
    // public static Range TokenNextTokenOpenMatchingEnd(this ReadOnlySpan<char> tokenisedFormatting, (string Open, string Close) openClose)
    // {
    //     var atIndex       = tokenisedFormatting.Length;
    //     var nextTokenOpen = tokenisedFormatting.IndexOf(openClose.Open);
    //     if (nextTokenOpen >= 0)
    //     {
    //         atIndex = nextTokenOpen + openClose.Open.Length;
    //         var remainingString        = tokenisedFormatting[atIndex..];
    //         var tokenOpenRange         = new Range(Index.FromStart(nextTokenOpen), Index.FromStart(atIndex));
    //         var subTokenPlusCloseRange = TokenNextTokenOpenMatchingEnd(remainingString, 1, openClose);
    //         var totalRange             = tokenOpenRange.ConcatLength(subTokenPlusCloseRange);
    //         return totalRange;
    //     }
    //     return new Range(tokenisedFormatting.Length, tokenisedFormatting.Length);
    // }

    public static Range TokenNextTokenOpenMatchingEnd
        (this ReadOnlySpan<char> tokenisedFormatting, (string Open, string Close) openClose, int depth = 0)
    {
        Range? sumRange = null;

        var clsLen         = openClose.Close.Length;
        var firstTokenOpen = tokenisedFormatting.IndexOf(openClose.Open, 0);
        var nextTokenClose = tokenisedFormatting.IndexOf(openClose.Close, 0);
        var remaining      = tokenisedFormatting;

        var nextTokenOpen = firstTokenOpen;
        while (nextTokenOpen >= 0 && nextTokenClose > nextTokenOpen)
        {
            var tokenLength  = nextTokenClose - nextTokenOpen;
            var addOpenRange = new Range(Index.FromStart(nextTokenOpen), Index.FromStart(nextTokenOpen));

            sumRange = sumRange?.ConcatLength(addOpenRange) ?? addOpenRange;

            var subTokenStart = nextTokenOpen == 0
                ? remaining.IndexOf(openClose.Open, nextTokenOpen + openClose.Open.Length)
                : -1;

            if (subTokenStart >= 0 && subTokenStart < nextTokenClose ||
                (nextTokenOpen > 0 && (depth > 0 || (firstTokenOpen == nextTokenOpen))))
            {
                if (subTokenStart >= 0 && nextTokenOpen == 0) sumRange = sumRange.Value.ConcatLength(new Range(0, subTokenStart - nextTokenOpen));

                subTokenStart = nextTokenOpen > 0 ? nextTokenOpen : subTokenStart;
                remaining     = remaining[subTokenStart..];
                var subTokenRange = TokenNextTokenOpenMatchingEnd(remaining, openClose, depth + 1);
                sumRange = sumRange.Value.ConcatLength(subTokenRange);
                if (depth == 0 && nextTokenOpen > 0) return sumRange.Value;
                remaining = remaining[subTokenRange.End..];
            }
            else
            {
                if (remaining.Length < tokenLength)
                {
                    Debugger.Break();
                }
                sumRange  = sumRange.Value.ConcatLength(new Range(0, Math.Clamp(tokenLength, 0, remaining.Length)));
                remaining = remaining[nextTokenClose..];
            }
            nextTokenOpen  = remaining.IndexOf(openClose.Open);
            nextTokenClose = remaining.IndexOf(openClose.Close);
        }
        if (nextTokenClose < 0)
        {
            return new Range(Index.FromStart(tokenisedFormatting.Length), Index.FromStart(tokenisedFormatting.Length));
        }
        var subTokenPostCloseStart = nextTokenClose + openClose.Close.Length;
        var addCloseRange          = new Range(Index.FromStart(nextTokenClose), Index.FromStart(subTokenPostCloseStart));
        sumRange = sumRange?.ConcatLength(addCloseRange) ?? addCloseRange;
        return sumRange.Value;
    }

    private static Range ConcatLength(this Range keepStart, Range addLength)
    {
        var origStartIndex = new Index(keepStart.Start.Value);
        var sumEndIndex    = new Index(keepStart.End.Value + addLength.End.Value);
        return new Range(origStartIndex, sumEndIndex);
    }

    public static List<string> TokenSplit
    (this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;
        var result    = new List<string>();

        var openTokenLength  = openClose.Open.Length;
        var closeTokenLength = openClose.Close.Length;

        bool hasReplace = replaceDelimiter != null;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value > 0)
            {
                var stringPart = new String(remainingTokenString[..nextTokenStartRange.Start]);
                result.Add(stringPart);
            }
            if (nextTokenStartRange.Start.Value < remainingTokenString.Length && nextTokenStartRange.Length() > 0)
            {
                if (hasReplace)
                {
                    var subTokenSpan   = remainingTokenString[nextTokenStartRange];
                    var from           = subTokenSpan.StartsWith(openClose.Open) ? Index.FromStart(openTokenLength) : Index.Start;
                    var to             = subTokenSpan.EndsWith(openClose.Close) ? Index.FromEnd(closeTokenLength) : Index.End;
                    var subTokenString = subTokenSpan[from..to];
                    var replaceOc      = replaceDelimiter!.Value;
                    scratchSpace.Append(replaceOc.ReplaceOpen);
                    scratchSpace.Append(subTokenString);
                    scratchSpace.Append(replaceOc.ReplaceClose);
                    var stringPart = new String(scratchSpace[..scratchSpace.PopulatedLength()]);
                    result.Add(stringPart);
                }
                else
                {
                    var stringPart = new String(remainingTokenString[nextTokenStartRange]);
                    result.Add(stringPart);
                }
            }
            var nextSliceStart = nextTokenStartRange.End.Value;
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return result;
    }

    public static Range NextTokenSplit
    (this ReadOnlySpan<char> tokenisedFormatting
      , int startIndex
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;

        var remainingTokenString = tokenisedFormatting[startIndex..];
        var scratchSpace         = stackalloc char[256].ResetMemory();

        scratchSpace.ResetMemory();
        var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

        return new Range(startIndex + nextTokenStartRange.Start.Value, startIndex + nextTokenStartRange.End.Value);
    }

    public static int SingleTokenFormatAt
    (this Span<char> destination, int destStartIndex, ReadOnlySpan<char> tokenisedFormatting, ReadOnlySpan<char> toFormatOver, int startIndex = 0
      , int count = int.MaxValue)
    {
        var cappedFrom          = Math.Clamp(startIndex, 0, toFormatOver.Length);
        var cappedLength        = Math.Clamp(count, 0, toFormatOver.Length - startIndex);
        var nextWriteAt         = destStartIndex;
        var lastRange           = new Range(0, 0);
        var nextTokenStartRange = tokenisedFormatting.NextTokenSplit(0);
        while (lastRange.End.Value < nextTokenStartRange.End.Value)
        {
            if (nextTokenStartRange.Start.Value > 0)
            {
                var charsAdded = nextTokenStartRange.Start.Value - lastRange.End.Value;
                destination.OverWriteAt(nextWriteAt, tokenisedFormatting[lastRange.End..nextTokenStartRange.Start]);
                nextWriteAt += charsAdded;
            }
            if (nextTokenStartRange.Start.Value < tokenisedFormatting.Length && nextTokenStartRange.Length() > 0)
            {
                destination.OverWriteAt(nextWriteAt, toFormatOver[cappedFrom..(cappedFrom + cappedLength)]);
                nextWriteAt += toFormatOver.Length;
            }
            lastRange           = nextTokenStartRange;
            nextTokenStartRange = tokenisedFormatting.NextTokenSplit(lastRange.End.Value);
        }
        return nextWriteAt - destStartIndex;
    }

    public static int SingleTokenFormatAt
    (this Span<char> destination, int destStartIndex, ReadOnlySpan<char> tokenisedFormatting, StringBuilder toFormatOver, int startIndex = 0
      , int count = int.MaxValue)
    {
        var cappedFrom          = Math.Clamp(startIndex, 0, toFormatOver.Length);
        var cappedLength        = Math.Clamp(count, 0, toFormatOver.Length - startIndex);
        var nextWriteAt         = destStartIndex;
        var lastRange           = new Range(0, 0);
        var nextTokenStartRange = tokenisedFormatting.NextTokenSplit(0);
        while (lastRange.End.Value < nextTokenStartRange.End.Value)
        {
            if (nextTokenStartRange.Start.Value > 0)
            {
                var charsAdded = nextTokenStartRange.Start.Value - lastRange.End.Value;
                destination.OverWriteAt(nextWriteAt, tokenisedFormatting[lastRange.End..nextTokenStartRange.Start]);
                nextWriteAt += charsAdded;
            }
            if (nextTokenStartRange.Start.Value < tokenisedFormatting.Length && nextTokenStartRange.Length() > 0)
            {
                destination.OverWriteAt(nextWriteAt, toFormatOver, cappedFrom, cappedLength);
                nextWriteAt += toFormatOver.Length;
            }
            lastRange           = nextTokenStartRange;
            nextTokenStartRange = tokenisedFormatting.NextTokenSplit(lastRange.End.Value);
        }
        return nextWriteAt - destStartIndex;
    }

    public static int SingleTokenFormatAt
    (this Span<char> destination, int destStartIndex, ReadOnlySpan<char> tokenisedFormatting, ICharSequence toFormatOver, int startIndex = 0
      , int count = int.MaxValue)
    {
        var cappedFrom          = Math.Clamp(startIndex, 0, toFormatOver.Length);
        var cappedLength        = Math.Clamp(count, 0, toFormatOver.Length - startIndex);
        var nextWriteAt         = destStartIndex;
        var lastRange           = new Range(0, 0);
        var nextTokenStartRange = tokenisedFormatting.NextTokenSplit(0);
        while (lastRange.End.Value < nextTokenStartRange.End.Value)
        {
            if (nextTokenStartRange.Start.Value > 0)
            {
                var charsAdded = nextTokenStartRange.Start.Value - lastRange.End.Value;
                destination.OverWriteAt(nextWriteAt, tokenisedFormatting[lastRange.End..nextTokenStartRange.Start]);
                nextWriteAt += charsAdded;
            }
            if (nextTokenStartRange.Start.Value < tokenisedFormatting.Length && nextTokenStartRange.Length() > 0)
            {
                destination.OverWriteAt(nextWriteAt, toFormatOver, cappedFrom, cappedLength);
                nextWriteAt += toFormatOver.Length;
            }
            lastRange           = nextTokenStartRange;
            nextTokenStartRange = tokenisedFormatting.NextTokenSplit(lastRange.End.Value);
        }
        return nextWriteAt - destStartIndex;
    }

    public static int NonTokenCharCount
    (this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;

        int nonTokenCharLength = 0;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value > 0)
            {
                nonTokenCharLength += nextTokenStartRange.Start.Value;
            }
            var nextSliceStart = nextTokenStartRange.Start.Value + nextTokenStartRange.Length();
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return nonTokenCharLength;
    }

    public static int TokenCount(this ReadOnlySpan<char> tokenisedFormatting
      , (string Open, string Close)? tokenDelimiter = null
      , (string ReplaceOpen, string ReplaceClose)? replaceDelimiter = null)
    {
        var openClose = tokenDelimiter ?? DefaultTokenStartEndDelimiter;

        int tokenCount = 0;

        var remainingTokenString = tokenisedFormatting;
        var scratchSpace         = stackalloc char[256].ResetMemory();

        while (remainingTokenString.Length > 0)
        {
            scratchSpace.ResetMemory();
            var nextTokenStartRange = TokenNextTokenOpenMatchingEnd(remainingTokenString, openClose);

            if (nextTokenStartRange.Start.Value < remainingTokenString.Length && nextTokenStartRange.Length() > 0)
            {
                tokenCount += nextTokenStartRange.Start.Value;
            }
            var nextSliceStart = nextTokenStartRange.Start.Value + nextTokenStartRange.Length();
            if (nextSliceStart < remainingTokenString.Length)
            {
                remainingTokenString = remainingTokenString[nextSliceStart..];
            }
            else
            {
                break;
            }
        }

        return tokenCount;
    }

    private static string WrapToString(string wrapPrefix, ReadOnlySpan<char> bodyRange, string wrapPostfix)
    {
        var stringSize = wrapPrefix.Length + bodyRange.Length + wrapPostfix.Length;
        var buildChars = stackalloc char[stringSize].ResetMemory();
        buildChars.Append(wrapPrefix);
        buildChars.Append(bodyRange);
        buildChars.Append(wrapPostfix);

        var wrappedString = new String(buildChars);
        return wrappedString;
    }

    public static string ToString(this Span<char> subject, int startingAt = 0, int subjectPopulatedLength = -1)
    {
        if (subjectPopulatedLength < 0)
        {
            subjectPopulatedLength = subject.PopulatedLength();
        }
        var restrictedSpan = (ReadOnlySpan<char>)subject.Slice(startingAt, subjectPopulatedLength);
        return new String(restrictedSpan);
    }

    public static int CountOccurences(this ReadOnlySpan<char> source, int startingAt, char find, char terminateAt1 = '\0', char terminateAt2 = '\0')
    {
        if (startingAt > source.Length) return 0;
        var count = 0;
        for (int i = startingAt; i < source.Length; i++)
        {
            var checkChar = source[i];
            if (checkChar == terminateAt1 || checkChar == terminateAt2) return count;
            if (checkChar == find) count++;
        }
        return count;
    }


    public static int WriteIntToSpan(this Span<char> writeTo, int toWrite, int startIndex)
    {
        if (startIndex != 0)
        {
            writeTo = writeTo[startIndex..];
        }
        if (toWrite.TryFormat(writeTo, out var written))
        {
            return written;
        }
        return 0;
    }


    public static int CalculateIntToSpanLength(this int toWrite)
    {
        int signSize = 0;
        if (toWrite < 0)
        {
            signSize++; // '-' negative sign
            toWrite               = Math.Abs(toWrite);
        }
        var remainingSize = 0;
        if (toWrite >= 10)
        {
            var divide10 = toWrite / 10;
            remainingSize = divide10.CalculateIntToSpanLength();
        }
        return 1 + signSize + remainingSize;
    }

    public static string BuildStringBuilderFormatting(this ReadOnlySpan<char> identifier, Range extendedLimitLengthRange
      , ReadOnlySpan<char> layout, SplitJoinRange extendedSplitJoinRange, ReadOnlySpan<char> format)
    {
        var stringLength = identifier.Length + 2;
        stringLength += layout.Length > 0 ? layout.Length + 1 : 0;
        stringLength += format.Length > 0 ? format.Length + 1 : 0;
        stringLength += extendedLimitLengthRange.CalculateRangeAsSliceStringSize();
        stringLength += extendedSplitJoinRange.CalculateSplitJoinStringSize() + (layout.Length > 0 ? 0 : 1);
        var buildChars = stackalloc char[stringLength].ResetMemory();
        var index      = 0;
        buildChars[index++] = '{';
        for (int i = 0; i < identifier.Length; i++)
        {
            buildChars[index++] = identifier[i];
        }
        if (!extendedLimitLengthRange.IsAllRange())
        {
            index += buildChars.WriteRangeAsSlice(index, extendedLimitLengthRange);
        }
        if (layout.Length > 0 || !extendedSplitJoinRange.IsNoSplitJoin)
        {
            buildChars[index++] = ',';
            for (int i = 0; i < layout.Length; i++)
            {
                buildChars[index++] = layout[i];
            }
            if (!extendedSplitJoinRange.IsNoSplitJoin)
            {
                index += buildChars.WriteSplitJoinFormatting(index, extendedSplitJoinRange);
            }
        }
        if (format.Length > 0)
        {
            buildChars[index++] = ':';
            for (int i = 0; i < format.Length; i++)
            {
                buildChars[index++] = format[i];
            }
        }
        buildChars[index] = '}';
        return buildChars[..buildChars.PopulatedLength()].ToString();
    }

    public static ReadOnlySpan<char> ExtractDigitsSlice(this ReadOnlySpan<char> maybeDigitsSpan, int startingFrom)
    {
        for (var i = startingFrom; i < maybeDigitsSpan.Length; i++)
        {
            var checkChar = maybeDigitsSpan[i];
            if (checkChar is < '0' or > '9') return maybeDigitsSpan[startingFrom..i];
        }
        return maybeDigitsSpan[startingFrom..];
    }

    public static Index ExtractIndexSlice(this ReadOnlySpan<char> maybeIndexSpan, int startingFrom, bool isEnd = false)
    {
        for (var i = startingFrom; i < maybeIndexSpan.Length; i++)
        {
            var checkChar = maybeIndexSpan[i];
            if (checkChar is < '0' or > '9' and not '^') return maybeIndexSpan[startingFrom..i].ToIndex(isEnd);
        }
        return isEnd ? Index.End : Index.Start;
    }

    public static int? ExtractInt(this ReadOnlySpan<char> maybeDigitsSpan, int startingFrom = 0)
    {
        var  sign  = 1;
        int? value = null;

        var foundNonWhiteSpace = false;
        for (var i = startingFrom; i < maybeDigitsSpan.Length; i++)
        {
            var checkChar = maybeDigitsSpan[i];
            if (!foundNonWhiteSpace && checkChar.IsWhiteSpace()) continue;
            if (checkChar.IsMinus()) sign = -1;
            if (checkChar is < '0' or > '9')
            {
                value ??= 0;
                value *=  10;
                value +=  checkChar - '0';
            }
            else
            {
                break;
            }
        }
        return value * sign;
    }
}
