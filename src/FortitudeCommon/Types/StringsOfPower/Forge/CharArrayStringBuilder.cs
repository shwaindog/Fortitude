using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Framework.System;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

// ReSharper disable NonReadonlyMemberInGetHashCode

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public class CharArrayStringBuilder : ReusableObject<CharArrayStringBuilder>, IScopeDelimitedStringBuilder
{
    private static readonly IRecycler GuaranteedRecycler = new Recycler();

    private RecyclingCharArray ca;

    private MutableString? fallBackFormatter;

    private RecyclingCharArray CharArray(int requiredLength)
    {
        var oldCa = ca;
        ca = ca.EnsureCapacity(requiredLength);
        if (!ReferenceEquals(oldCa, ca)) { this.EnsureRecyclerMatchesCurrentCapacity(); }
        return ca;
    }

    public CharArrayStringBuilder() => ca = 128.SourceRecyclingCharArray();

    public CharArrayStringBuilder(IMutableString initialString)
    {
        var initialSize = Math.Max(initialString.Length, 128);
        ca = initialSize.SourceRecyclingCharArray();
        Append(initialString);
    }

    public CharArrayStringBuilder(string? initialString)
    {
        var initialSize = Math.Max(initialString?.Length ?? 128, 128);
        ca = initialSize.SourceRecyclingCharArray();
        Append(initialString);
    }

    public CharArrayStringBuilder(int initialCapacity) => ca = Math.Max(initialCapacity, 128).SourceRecyclingCharArray();

    public CharArrayStringBuilder(RecyclingCharArray initializedBuilder) => ca = initializedBuilder;

    public CharArrayStringBuilder(CharArrayStringBuilder toClone)
    {
        ca = toClone.Length.SourceRecyclingCharArray();
        ca.CopyFrom(toClone.ca);
    }

    protected IRecycler MyRecycler
    {
        get => base.Recycler ?? GuaranteedRecycler;
        set => base.Recycler = value;
    }


    public CharArrayStringBuilder EnsureIsAtSize(int size)
    {
        if (ca != null && ca.Capacity != size)
        {
            throw new
                ArgumentException($"Expected the array to already be initialized at size {size} or be empty and ready to be initialized, but it was {ca.Capacity}");
        }
        ca ??= size.SourceRecyclingCharArray();
        return this;
    }

    Action<IScopeDelimitedStringBuilder>? IScopeDelimitedStringBuilder.OnScopeEndedAction { get; set; }

    public int Capacity
    {
        get => ca.Capacity;
        set => throw new NotImplementedException();
    }
    public int MaxCapacity => throw new NotImplementedException();

    public int Length
    {
        get => ca.Count;
        set
        {
            if (value >= ca.Capacity)
            {
                CharArray(value - ca.Length);
            }
            ca.Count = value;
        }
    }

    public char this[int index]
    {
        get => ca[index];
        set => ca[index] = value;
    }

    public int LineChars
    {
        get
        {
            var charCount = 0;
            for (int i = ca.Length - 1; i >= 0; i--)
            {
                var checkChar = ca[i];
                if (checkChar is '\n' or '\r') return charCount;
                charCount++;
            }
            return ca.Length;
        }
    }

    public int LineContentStartColumn
    {
        get
        {
            var columnCount = 0;
            for (var i = 0; i < ca.Length - LineChars; i++)
            {
                var checkChar = ca[i];
                if (!checkChar.IsWhiteSpace()) return columnCount;
                columnCount++;
            }
            return -1;
        }
    }

    public int LineContentWidth
    {
        get
        {
            var contentStartColumn = LineContentStartColumn;
            if (contentStartColumn < 0) return 0;
            var lineContentFromEnd = 0;
            for (var i = ca.Length - 1; i >= 0; i--)
            {
                var checkChar = ca[i];
                if (checkChar is '\n' or '\r' || !checkChar.IsWhiteSpace())
                {
                    lineContentFromEnd = ca.Length - i;
                    break;
                }
            }
            return Math.Max(0, LineChars - contentStartColumn - lineContentFromEnd);
        }
    }

    public Span<char> WrittenAsSpan()   => ca.WrittenAsSpan();
    public Span<char> RemainingAsSpan() => ca.RemainingAsSpan();
    public CharArrayRange AsCharArrayRange => ca.AsCharArrayRange;

    public void CopyTo(char[] array, int arrayIndex, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex; i < array.Length && myIndex < Length && myIndex < myEnd; i++) { array[i] = ca[myIndex++]; }
    }

    public void CopyTo(RecyclingCharArray array, int? arrayIndex = null, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex ?? array.Count; i < array.Count && myIndex < Length && myIndex < myEnd; i++) { array[i] = ca[myIndex++]; }
    }

    public void CopyTo(Span<char> charSpan, int spanIndex, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = spanIndex; i < charSpan.Length && myIndex < Length && myIndex < myEnd; i++) { charSpan[i] = ca[myIndex++]; }
    }

    public bool Contains(string subStr) => IndexOf(subStr) >= 0;

    public bool Contains(ICharSequence subStr) => IndexOf(subStr) >= 0;

    public int IndexOf(string subStr) => IndexOf(subStr, 0);

    public int IndexOf(ICharSequence subStr) => IndexOf(subStr, 0);

    public int IndexOf(string subStr, int fromThisPos)
    {
        var thisLen  = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var startPos  = fromThisPos < 0 ? 0 : fromThisPos;
        var firstChar = subStr[0];
        var max       = thisLen - otherLen;
        for (var i = startPos; i <= max; i++)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (++i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j   = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int IndexOf(ICharSequence subStr, int fromThisPos)
    {
        var thisLen  = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var startPos  = fromThisPos < 0 ? 0 : fromThisPos;
        var firstChar = subStr[0];
        var max       = thisLen - otherLen;
        for (var i = startPos; i <= max; i++)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (++i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j   = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int IndexOf(StringBuilder subStr, int fromThisPos)
    {
        var thisLen  = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var startPos  = fromThisPos < 0 ? 0 : fromThisPos;
        var firstChar = subStr[0];
        var max       = thisLen - otherLen;
        for (var i = startPos; i <= max; i++)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (++i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j   = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int LastIndexOf(string subStr) => LastIndexOf(subStr, subStr.Length);

    public int LastIndexOf(ICharSequence subStr) => LastIndexOf(subStr, subStr.Length);

    public int LastIndexOf(string subStr, int fromThisPos)
    {
        var thisLen  = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var firstChar = subStr[0];
        var max       = thisLen - otherLen;
        var startPos  = fromThisPos > max ? max : fromThisPos;
        for (var i = startPos; i >= 0; i--)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (--i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j   = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int LastIndexOf(ICharSequence subStr, int fromThisPos)
    {
        var thisLen  = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var firstChar = subStr[0];
        var max       = thisLen - otherLen;
        var startPos  = fromThisPos > max ? max : fromThisPos;
        for (var i = startPos; i >= 0; i--)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (--i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j   = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int CompareTo(string other)
    {
        var thisLen  = Length;
        var otherLen = other.Length;
        var minLen   = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = ca[i] - other[i];
            if (cmp != 0) return cmp;
        }

        if (thisLen < otherLen) return -1;
        if (thisLen > otherLen) return 1;

        return 0;
    }

    public int CompareTo(ICharSequence other)
    {
        var thisLen  = Length;
        var otherLen = other.Length;
        if (thisLen < otherLen) return -1;
        if (thisLen > otherLen) return 1;
        var minLen = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = ca[i] - other[i];
            if (cmp != 0) return cmp;
        }

        return 0;
    }

    public int CompareTo(StringBuilder other)
    {
        var thisLen  = Length;
        var otherLen = other.Length;
        if (thisLen < otherLen) return -1;
        if (thisLen > otherLen) return 1;
        var minLen = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = ca[i] - other[i];
            if (cmp != 0) return cmp;
        }

        return 0;
    }

    public bool EquivalentTo(string other)
    {
        if (ReferenceEquals(null, other)) return false;
        return 0 == CompareTo(other);
    }

    public CharArrayStringBuilder Append(bool value, ICustomStringFormatter? customStringFormatter)
    {
        customStringFormatter ??= ICustomStringFormatter.DefaultBufferFormatter;
        CharArray(5).Add(value ? customStringFormatter.Options.True : customStringFormatter.Options.False);
        return this;
    }

    public CharArrayStringBuilder Append(byte value)
    {
        value.TryFormat(CharArray(3).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(char value)
    {
        CharArray(1).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(char value, int repeatCount)
    {
        CharArray(repeatCount);
        for (int i = 0; i < repeatCount; i++) { ca.Add(value); }
        return this;
    }

    public CharArrayStringBuilder Append(char[]? value
      , ICustomStringFormatter? customStringFormatter = null)
    {
        if (value == null) return this;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, 0, charArraySpan, 0, value.Length);
            return this;
        }
        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;
        var cappedLength = Math.Clamp(length, 0, value.Length - startIndex);
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (formatString == null)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString, cappedLength, formatFlags);
            return this;
        }
        if (formatString == null) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            CharArray(cappedLength + formatString.Length);
            var charArraySpan = ca.WrittenAsSpan();
            var size          = ca.Length;
            var endIndex      = startIndex + cappedLength;
            var toAddSpan     = value[startIndex..endIndex];
            charArraySpan.SingleTokenFormatAt(size, toAddSpan, formatString);
        }
        return this;
    }

    public CharArrayStringBuilder Append(char[]? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;
        var cappedLength = Math.Clamp(length, 0, value.Length - startIndex);
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (formatString == null)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString, cappedLength, formatFlags);
            return this;
        }
        if (formatString == null) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            CharArray(cappedLength + formatString.Length);
            var charArraySpan = ca.WrittenAsSpan();
            var size          = ca.Length;
            var endIndex      = startIndex + cappedLength;
            var toAddSpan     = value[startIndex..endIndex];
            charArraySpan.SingleTokenFormatAt(size, toAddSpan, formatString);
        }
        return this;
    }

    public unsafe CharArrayStringBuilder Append(char* value, int valueCount)
    {
        if (value == null) return this;

        CharArray(valueCount).Add(value, valueCount);
        return this;
    }

    public CharArrayStringBuilder Append(decimal value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(double value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(short value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(int value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(long value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(StringBuilder? value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (value == null) return this;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.ContentEncoder.Transfer(value, 0, charArraySpan, 0, value.Length);
            return this;
        }

        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;

        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString, cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString, 0, cappedLength);
        }
        return this;
    }

    public CharArrayStringBuilder Append(StringBuilder? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;

        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsEmpty || formatString.SequenceMatches(CustomStringFormatter.NoFormatFormatString);
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString, cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString, 0, cappedLength);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;

        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, startIndex
                                                                         , charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ICharSequence? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;

        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsEmpty || formatString.SequenceMatches(CustomStringFormatter.NoFormatFormatString);
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, startIndex
                                                                         , charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ICharSequence? value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (value == null) return this;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.ContentEncoder.Transfer(customStringFormatter, value, 0, charArraySpan, 0, value.Length);
            return this;
        }

        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(object? value)
    {
        if (value == null) return this;
        var wasSuccessful = ICustomStringFormatter.DefaultBufferFormatter.TryFormat(value, this, "");
        if (wasSuccessful != 0) return this;
        var objString = value.ToString();
        if (objString == null) return this;
        CharArray(objString.Length).Add(objString);
        return this;
    }

    public CharArrayStringBuilder Append(sbyte value)
    {
        value.TryFormat(CharArray(3).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(float value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(string? value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (value == null) return this;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length +=
                customStringFormatter
                    .ContentEncoder
                    .OverwriteTransfer(value, 0, charArraySpan, 0, value.Length);
            return this;
        }
        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(string? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;
        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length +=
                    customStringFormatter
                        .ContentEncoder
                        .OverwriteTransfer(value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(string? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        if (value == null) return this;
        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsEmpty || formatString.SequenceMatches(CustomStringFormatter.NoFormatFormatString);
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length +=
                    customStringFormatter
                        .ContentEncoder
                        .OverwriteTransfer(value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append<TFmt>(TFmt arg0, ICustomStringFormatter? customStringFormatter = null)
        where TFmt : ISpanFormattable?
    {
        customStringFormatter ??= ICustomStringFormatter.DefaultBufferFormatter;
        EnsureCapacity(32);
        var charArraySpan = ca.RemainingAsSpan();
        ca.Length += customStringFormatter.Format(arg0, charArraySpan, 0, "".AsSpan());
        return this;
    }

    public CharArrayStringBuilder Append(Span<char> value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length +=
                customStringFormatter
                    .ContentEncoder
                    .OverwriteTransfer(value, 0, charArraySpan, 0, value.Length);
            return this;
        }
        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(Span<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length +=
                    customStringFormatter
                        .ContentEncoder
                        .OverwriteTransfer(value, startIndex, charArraySpan, 0, length);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , length, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(cappedLength).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString, 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ReadOnlySpan<char> value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length +=
                customStringFormatter
                    .ContentEncoder
                    .OverwriteTransfer(value, 0, charArraySpan, 0, value.Length);
            return this;
        }
        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(ReadOnlySpan<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length +=
                    customStringFormatter
                        .ContentEncoder
                        .OverwriteTransfer(value, startIndex, charArraySpan, 0, cappedLength);
            else
                ca.Length += customStringFormatter.Format(value, startIndex, this, formatString
                                                        , cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(value.Length).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(value, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ReadOnlyMemory<char> value, ICustomStringFormatter? customStringFormatter = null)
    {
        if (customStringFormatter != null)
        {
            var asSpan        = value.Span;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter
                         .ContentEncoder
                         .OverwriteTransfer(asSpan, 0, charArraySpan, 0, asSpan.Length);
            return this;
        }
        CharArray(value.Length).Add(value);
        return this;
    }

    public CharArrayStringBuilder Append(ReadOnlyMemory<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        var cappedLength             = Math.Clamp(length, 0, value.Length - startIndex);
        var asSpan                   = value.Span;
        var noFormatStringFormatting = formatString.IsNullOrEmpty() || formatString == CustomStringFormatter.NoFormatFormatString;
        if (customStringFormatter != null)
        {
            var charArraySpan = ca.RemainingAsSpan();
            if (noFormatStringFormatting)
                ca.Length +=
                    customStringFormatter
                        .ContentEncoder
                        .OverwriteTransfer(asSpan, startIndex, charArraySpan, 0, cappedLength);
            else
                customStringFormatter.Format(asSpan, startIndex, this, formatString, cappedLength, formatFlags);
            return this;
        }
        if (noFormatStringFormatting) { CharArray(value.Length).Add(value, startIndex, cappedLength); }
        else
        {
            customStringFormatter = ICustomStringFormatter.DefaultBufferFormatter;
            var charArraySpan = ca.RemainingAsSpan();
            ca.Length += customStringFormatter.Format(asSpan, startIndex, charArraySpan, formatString
                                                    , 0, cappedLength, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder Append(ushort value)
    {
        value.TryFormat(CharArray(6).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(uint value)
    {
        value.TryFormat(CharArray(12).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder Append(ulong value)
    {
        value.TryFormat(CharArray(20).BackingArray, out var addToCount);
        ca.Count += addToCount;
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TFmt arg0
      , FormatSwitches formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?
    {
        EnsureCapacity(32);
        var charArraySpan = ca.RemainingAsSpan();
        ca.Length += customStringFormatter.Format(arg0, charArraySpan, 0, format, formatFlags);
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, TFmt arg0
      , FormatSwitches formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?
    {
        EnsureCapacity(32);
        var charArraySpan = ca.RemainingAsSpan();
        ca.Length += customStringFormatter.Format(arg0, charArraySpan, 0, format, formatFlags);
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TFmt>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TFmt arg0)
        where TFmt : ISpanFormattable?
    {
        EnsureCapacity(32);
        return AppendFormat(format.AsSpan(), arg0);
    }

    public CharArrayStringBuilder AppendFormat<TFmt>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, TFmt arg0)
        where TFmt : ISpanFormattable?
    {
        EnsureCapacity(32);
        var charArraySpan = ca.RemainingAsSpan();
        ca.Length += ICustomStringFormatter.DefaultBufferFormatter.Format(arg0, charArraySpan, 0, format);
        return this;
    }

    public CharArrayStringBuilder AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, string arg0) =>
        AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, format, arg0);

    public CharArrayStringBuilder AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, ReadOnlySpan<char> arg0)
    {
        format.AsSpan().ExtractExtendedStringFormatStages
            (out var prefix, out _, out var extendedLengthRange
           , out var layout, out _, out _, out var suffix);

        if (prefix.Length > 0) Append(prefix);
        if (extendedLengthRange.IsAllRange())
        {
            if (layout.Length == 0) { Append(arg0); }
            else { ca.Count += ca.RemainingAsSpan().PadAndAlign(arg0, layout); }
        }
        else
        {
            extendedLengthRange = extendedLengthRange.BoundRangeToLength(arg0.Length);
            if (layout.Length == 0) { Append(arg0[extendedLengthRange]); }
            else { ca.Count += ca.RemainingAsSpan().PadAndAlign(arg0[extendedLengthRange], layout); }
        }
        if (suffix.Length > 0) Append(suffix);

        return this;
    }

    public CharArrayStringBuilder AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, ReadOnlySpan<char> arg0)
    {
        format.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendedLengthRange
                                               , out var layout, out _, out _, out var suffix);
        if (prefix.Length > 0) Append(prefix);
        if (extendedLengthRange.IsAllRange())
        {
            if (layout.Length == 0) { Append(arg0); }
            else { ca.Count += ca.RemainingAsSpan().PadAndAlign(arg0, layout); }
        }
        else
        {
            extendedLengthRange = extendedLengthRange.BoundRangeToLength(arg0.Length);
            if (layout.Length == 0) { Append(arg0[extendedLengthRange]); }
            else { ca.Count += ca.RemainingAsSpan().PadAndAlign(arg0[extendedLengthRange], layout); }
        }
        if (suffix.Length > 0) Append(suffix);

        return this;
    }

    public CharArrayStringBuilder AppendFormat(ICustomStringFormatter customStringFormatter, string format, object? arg0
      , FormatSwitches formatFlags = EncodeInnerContent)
    {
        var wasSuccessful = customStringFormatter.TryFormat(arg0, this, format);
        if (wasSuccessful != 0) return this;

        var preAppendLen = Length;
        if (format.IsValidFormatString())
            AppendFormatHelper(null, format, new ReadOnlySpan<object?>(in arg0));
        else
            Append(arg0);
        if (preAppendLen < Length)
        {
            var addedChars = Length - preAppendLen;
            var encodedSize      = customStringFormatter.ContentEncoder.CalculateEncodedLength(this, preAppendLen, addedChars);
            EnsureCapacity(encodedSize);
            ca.Length += customStringFormatter.ProcessAppendedRange(ca.BackingArray.AsSpan(), preAppendLen, addedChars, formatFlags);
        }
        return this;
    }

    public CharArrayStringBuilder AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        return AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, format, arg0);
    }

    public CharArrayStringBuilder AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        TwoObjects two = new TwoObjects(arg0, arg1);
        return AppendFormatHelper(null, format, MemoryMarshal.CreateReadOnlySpan(ref two.Arg0, 2));
    }

    public CharArrayStringBuilder AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        ThreeObjects three = new ThreeObjects(arg0, arg1, arg2);
        EnsureCapacity(512);
        return AppendFormatHelper(null, format, MemoryMarshal.CreateReadOnlySpan(ref three.Arg0, 3));
    }

    public CharArrayStringBuilder AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        if (args is null)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            throw new ArgumentNullException(format is null ? nameof(format) : nameof(args));
        }

        return AppendFormatHelper(null, format, args);
    }

    public CharArrayStringBuilder AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        return AppendFormatHelper(provider, format, new ReadOnlySpan<object?>(in arg0));
    }

    public CharArrayStringBuilder AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        TwoObjects two = new TwoObjects(arg0, arg1);
        return AppendFormatHelper(provider, format, MemoryMarshal.CreateReadOnlySpan(ref two.Arg0, 2));
    }

    public CharArrayStringBuilder AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        ThreeObjects three = new ThreeObjects(arg0, arg1, arg2);
        return AppendFormatHelper(provider, format, MemoryMarshal.CreateReadOnlySpan(ref three.Arg0, 3));
    }

    public CharArrayStringBuilder AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        if (args is null)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            throw new ArgumentNullException(format is null ? nameof(format) : nameof(args));
        }

        return AppendFormatHelper(provider, format, args);
    }

    public CharArrayStringBuilder AppendFormat(IFormatProvider? provider, CompositeFormat format, params object?[] args)
    {
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(args);
        return AppendFormat(provider, format, (ReadOnlySpan<object?>)args);
    }

    public CharArrayStringBuilder AppendFormat(IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args)
    {
        fallBackFormatter ??= MyRecycler.Borrow<MutableString>();
        fallBackFormatter.Clear();
        fallBackFormatter.AppendFormat(provider, format, args);
        CharArray(fallBackFormatter.Length).Add(fallBackFormatter);
        fallBackFormatter.DecrementRefCount();
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0)
    {
        fallBackFormatter ??= MyRecycler.Borrow<MutableString>();
        fallBackFormatter.Clear();
        fallBackFormatter.AppendFormat(provider, format, arg0);
        CharArray(fallBackFormatter.Length).Add(fallBackFormatter);
        fallBackFormatter.DecrementRefCount();
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TParam, TParam1>(IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1)
    {
        fallBackFormatter ??= MyRecycler.Borrow<MutableString>();
        fallBackFormatter.Clear();
        fallBackFormatter.AppendFormat(provider, format, arg0, arg1);
        CharArray(fallBackFormatter.Length).Add(fallBackFormatter);
        fallBackFormatter.DecrementRefCount();
        return this;
    }

    public CharArrayStringBuilder AppendFormat<TParam, TParam1, TParam2>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2)
    {
        fallBackFormatter ??= MyRecycler.Borrow<MutableString>();
        fallBackFormatter.Clear();
        fallBackFormatter.AppendFormat(provider, format, arg0, arg1, arg2);
        CharArray(fallBackFormatter.Length).Add(fallBackFormatter);
        fallBackFormatter.DecrementRefCount();
        return this;
    }

    public CharArrayStringBuilder AppendJoin(string? separator, params object?[] values)
    {
        separator ??= string.Empty;
        return AppendJoinCore(separator, values);
    }

    public CharArrayStringBuilder AppendJoin(string? separator, params string?[] values)
    {
        separator ??= string.Empty;
        return AppendJoinCore(separator, values);
    }

    public CharArrayStringBuilder AppendJoin(char separator, params string?[] values)
    {
        var separatorSpan = stackalloc char[1].ResetMemory();
        separatorSpan[0] = separator;
        return AppendJoinCore(separatorSpan, values);
    }

    public CharArrayStringBuilder AppendJoin<TParam>(string? separator, IEnumerable<TParam> values)
    {
        separator ??= string.Empty;
        return AppendJoinCore(separator, values);
    }

    public CharArrayStringBuilder AppendJoin<TParam>(char separator, IEnumerable<TParam> values)
    {
        var separatorSpan = stackalloc char[1].ResetMemory();
        separatorSpan[0] = separator;
        return AppendJoinCore(separatorSpan, values);
    }

    public CharArrayStringBuilder AppendLine()
    {
        return Append(Environment.NewLine);
    }

    public CharArrayStringBuilder AppendLine(string? value)
    {
        Append(value);
        return Append(Environment.NewLine);
    }

    public CharArrayStringBuilder AppendLine(StringBuilder value)
    {
        Append(value);
        return Append(Environment.NewLine);
    }

    public CharArrayStringBuilder AppendLine(ICharSequence value)
    {
        Append(value);
        return Append(Environment.NewLine);
    }

    public CharArrayStringBuilder Clear()
    {
        ca.Clear();
        return this;
    }

    public CharArrayStringBuilder CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
        ca.CopyTo(sourceIndex, destination, destinationIndex, count);
        return this;
    }

    public CharArrayStringBuilder CopyTo(int sourceIndex, Span<char> destination, int count)
    {
        ca.CopyTo(sourceIndex, destination, count);
        return this;
    }

    public int EnsureCapacity(int requiredRemainingCapacity)
    {
        ca =  CharArray(requiredRemainingCapacity);
        return ca.RemainingCapacity;
    }

    public CharArrayStringBuilder Insert(int atIndex, bool value)
    {
        CharArray(5).Insert(atIndex, value ? "true" : "false");
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, byte value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, char value)
    {
        CharArray(1).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, char[]? value)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, decimal value)
    {
        var scratchSpan = stackalloc char[24].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, double value)
    {
        var scratchSpan = stackalloc char[24].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, short value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, int value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, long value)
    {
        var scratchSpan = stackalloc char[24].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, object? value)
    {
        if (value == null) return this;
        var objString = value.ToString();
        if (objString == null) return this;
        CharArray(objString.Length).Insert(atIndex, objString);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, sbyte value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, float value)
    {
        var scratchSpan = stackalloc char[24].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, string? value)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, char[]? value, int startIndex, int length)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value, startIndex, length);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, ushort value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, uint value)
    {
        var scratchSpan = stackalloc char[12].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, ulong value)
    {
        var scratchSpan = stackalloc char[24].ResetMemory();
        value.TryFormat(scratchSpan, out var charsLen);
        CharArray(charsLen).Insert(atIndex, scratchSpan.Slice(charsLen));
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, IFrozenString? value)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, StringBuilder? value)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, string? value, int count)
    {
        if (value == null) return this;
        CharArray(value.Length).Insert(atIndex, value, count);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, Span<char> value)
    {
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, Span<char> value, int startIndex, int length)
    {
        CharArray(value.Length).Insert(atIndex, value, startIndex, length);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, ReadOnlySpan<char> value)
    {
        CharArray(value.Length).Insert(atIndex, value);
        return this;
    }

    public CharArrayStringBuilder Insert(int atIndex, ReadOnlySpan<char> value, int startIndex, int length)
    {
        CharArray(value.Length).Insert(atIndex, value, startIndex, length);
        return this;
    }

    public int Overwrite(int atIndex, bool value)
    {
        // bool returns cached strings
        return Overwrite(atIndex, value.ToString());
    }

    public int Overwrite(int atIndex, byte value)
    {
        Span<char> populated = stackalloc char[3];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, char value)
    {
        if (atIndex < Length) { ca[atIndex] = value; }
        else { ca.Add(value); }
        return 1;
    }

    public int Overwrite(int atIndex, char[]? value)
    {
        return Overwrite(atIndex, value.AsSpan());
    }

    public int Overwrite(int atIndex, decimal value)
    {
        Span<char> populated = stackalloc char[30];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, double value)
    {
        Span<char> populated = stackalloc char[24];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, short value)
    {
        Span<char> populated = stackalloc char[6];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, int value)
    {
        Span<char> populated = stackalloc char[11];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, long value)
    {
        Span<char> populated = stackalloc char[20];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, object? value)
    {
        // bool returns cached strings
        return Overwrite(atIndex, value?.ToString() ?? "");
    }

    public int Overwrite(int atIndex, sbyte value)
    {
        Span<char> populated = stackalloc char[4];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, float value)
    {
        Span<char> populated = stackalloc char[15];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, string? value)
    {
        return Overwrite(atIndex, (value ?? "").AsSpan());
    }

    public int Overwrite(int atIndex, char[]? value, int startIndex, int length)
    {
        return Overwrite(atIndex, value.AsSpan(), startIndex, length);
    }

    public int Overwrite(int atIndex, ushort value)
    {
        Span<char> populated = stackalloc char[5];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, uint value)
    {
        Span<char> populated = stackalloc char[10];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, ulong value)
    {
        Span<char> populated = stackalloc char[19];
        if (value.TryFormat(populated, out var charsWritten, ReadOnlySpan<char>.Empty)) { populated = populated[..charsWritten]; }
        return Overwrite(atIndex, populated);
    }

    public int Overwrite(int atIndex, ICharSequence? value)
    {
        var sbLen = Length;
        if (value == null) return 0;
        var overWriteLen = value.Length;
        if (atIndex < sbLen)
        {
            Length = Math.Max(Length, atIndex + overWriteLen);
            for (int i = 0; i < overWriteLen && i < sbLen; i++) { ca[i + atIndex] = value[i]; }
        }
        else { ca.Add(value); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, StringBuilder? value)
    {
        var sbLen = Length;
        if (value == null) return 0;
        var overWriteLen = value.Length;
        if (atIndex < sbLen)
        {
            Length = Math.Max(sbLen, atIndex + overWriteLen);
            for (int i = 0; i < overWriteLen && i < sbLen; i++) { ca[i + atIndex] = value[i]; }
        }
        else { ca.Add(value); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, string? value, int count)
    {
        var sbLen = Length;
        if (value == null) return 0;
        var overWriteLen = Math.Min(count, value.Length);
        if (atIndex < sbLen)
        {
            Length = Math.Max(sbLen, atIndex + overWriteLen);
            for (int i = 0; i < overWriteLen && i < sbLen; i++) { ca[i + atIndex] = value[i]; }
        }
        else { ca.Add(value); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, Span<char> value)
    {
        var sbLen        = Length;
        var overWriteLen = value.Length;
        if (atIndex < sbLen)
        {
            Length = Math.Max(sbLen, atIndex + overWriteLen);
            for (int i = 0; i < overWriteLen && i < sbLen; i++) { ca[i + atIndex] = value[i]; }
        }
        else { ca.Add(value); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, Span<char> value, int startIndex, int length)
    {
        var sbLen        = Length;
        var cappedStart  = Math.Clamp(startIndex, 0, value.Length);
        var overWriteLen = Math.Clamp(length, 0, value.Length - startIndex);
        if (atIndex < sbLen)
        {
            Length = Math.Max(sbLen, atIndex + overWriteLen);
            var destOffset = atIndex - cappedStart;
            for (int i = cappedStart; i < overWriteLen && i < sbLen; i++) { ca[destOffset + i] = value[i]; }
        }
        else { ca.Add(value[cappedStart..]); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, ReadOnlySpan<char> value)
    {
        var sbLen        = Length;
        var overWriteLen = value.Length;
        if (atIndex < sbLen)
        {
            Length = Math.Max(Length, atIndex + overWriteLen);
            for (int i = 0; i < overWriteLen && i < sbLen; i++) { ca[i + atIndex] = value[i]; }
        }
        else { ca.Add(value); }
        return overWriteLen;
    }

    public int Overwrite(int atIndex, ReadOnlySpan<char> value, int startIndex, int length)
    {
        var sbLen        = Length;
        var cappedStart  = Math.Clamp(startIndex, 0, value.Length);
        var overWriteLen = Math.Clamp(length, 0, value.Length - startIndex);
        if (atIndex < sbLen)
        {
            Length = Math.Max(Length, atIndex + overWriteLen);
            var destOffset = atIndex - cappedStart;
            for (int i = cappedStart; i < overWriteLen && i < sbLen; i++) { ca[destOffset + i] = value[i]; }
        }
        else { ca.Add(value[cappedStart..]); }
        return overWriteLen;
    }

    public CharArrayStringBuilder Remove(int startIndex, int length)
    {
        ca.RemoveAt(startIndex, length);
        return this;
    }

    public CharArrayStringBuilder Replace(char find, char replace)
    {
        ca.Replace(find, replace);
        return this;
    }

    public CharArrayStringBuilder Replace(char find, char replace, int startIndex, int length)
    {
        ca.Replace(find, replace, startIndex, length);
        return this;
    }

    public CharArrayStringBuilder Replace(string find, string? replace)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var count      = searchSpan.SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, (replace?.Length ?? 0) - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        var findSpan    = find.AsSpan();
        var replaceSpan = replace.AsSpan();
        ca.Replace(findSpan, replaceSpan);
        return this;
    }

    public CharArrayStringBuilder Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var cappedFrom = 0;
        var cappedTo   = ca.Length;
        var count      = searchSpan[cappedFrom..cappedTo].SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, replace.Length - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        ca.Replace(find, replace, cappedFrom, cappedTo + requiredIncreaseSize);
        return this;
    }

    public CharArrayStringBuilder Replace(string find, string replace, int startIndex, int length)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var cappedFrom = Math.Clamp(startIndex, 0, searchSpan.Length);
        var cappedTo   = Math.Clamp(startIndex + length, cappedFrom, searchSpan.Length);
        var count      = searchSpan[cappedFrom..cappedTo].SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, replace.Length - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        var findSpan    = find.AsSpan();
        var replaceSpan = replace.AsSpan();
        ca.Replace(findSpan, replaceSpan, startIndex, length + requiredIncreaseSize);
        return this;
    }

    public CharArrayStringBuilder Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex, int length)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var cappedFrom = Math.Clamp(startIndex, 0, searchSpan.Length);
        var cappedTo   = Math.Clamp(startIndex + length, cappedFrom, searchSpan.Length);
        var count      = searchSpan[cappedFrom..cappedTo].SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, replace.Length - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        ca.Replace(find, replace, startIndex, length + requiredIncreaseSize);
        return this;
    }

    public CharArrayStringBuilder Replace(ICharSequence find, ICharSequence replace) => Replace(find, replace, 0, Length);

    public CharArrayStringBuilder Replace(ICharSequence find, ICharSequence replace, int startIndex, int length)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var cappedFrom = Math.Clamp(startIndex, 0, searchSpan.Length);
        var cappedTo   = Math.Clamp(startIndex + length, cappedFrom, searchSpan.Length);
        var count      = searchSpan[cappedFrom..cappedTo].SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, replace.Length - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        ca.Replace(find, replace, startIndex, length + requiredIncreaseSize);
        return this;
    }

    public CharArrayStringBuilder Replace(StringBuilder find, StringBuilder replace) => Replace(find, replace, 0, Length);

    public CharArrayStringBuilder Replace(StringBuilder find, StringBuilder replace, int startIndex, int length)
    {
        var searchSpan = ((ReadOnlySpan<char>)ca.WrittenAsSpan());
        var cappedFrom = Math.Clamp(startIndex, 0, searchSpan.Length);
        var cappedTo   = Math.Clamp(startIndex + length, cappedFrom, searchSpan.Length);
        var count      = searchSpan[cappedFrom..cappedTo].SubSequenceOccurenceCount(find);

        var requiredIncreaseSize = count * Math.Max(0, replace.Length - find.Length);
        EnsureCapacity(requiredIncreaseSize);

        ca.Replace(find, replace, startIndex, length + requiredIncreaseSize);
        return this;
    }

    public CharArrayStringBuilder Remove(int startIndex)
    {
        ca.RemoveAt(startIndex);
        return this;
    }

    public CharArrayStringBuilder ToUpper()
    {
        ca.ToUpper();
        return this;
    }

    public CharArrayStringBuilder ToLower()
    {
        ca.ToLower();
        return this;
    }

    public CharArrayStringBuilder Trim()
    {
        var arraySpan          = ca.BackingArray.AsSpan();
        var arrayLen           = ca.Count;
        var countEndWhiteSpace = arraySpan.CountWhiteSpaceBackwardsFrom(arrayLen);
        if (countEndWhiteSpace > 0)
        {
            Remove(arrayLen - 1 - countEndWhiteSpace, countEndWhiteSpace);
            arrayLen = ca.Count -= countEndWhiteSpace;
        }
        var countStartWhiteSpace = arraySpan.CountWhiteSpaceFrom(0, arrayLen);
        if (countStartWhiteSpace > 0)
        {
            Remove(arrayLen - 1 - countStartWhiteSpace, countStartWhiteSpace);
            ca.Count -= countStartWhiteSpace;
        }
        return this;
    }


    public CharArrayStringBuilder Substring(int startIndex)
    {
        var subStringSize = ca.Count - startIndex;
        var subStringCopy = subStringSize.SourceCharArrayStringBuilder();

        subStringCopy.ca.Add(ca, startIndex, ca.Count - startIndex);

        return subStringCopy;
    }

    public CharArrayStringBuilder Substring(int startIndex, int length)
    {
        var subStringSize = ca.Count - startIndex;
        var subStringCopy = subStringSize.SourceCharArrayStringBuilder();

        subStringCopy.ca.Add(ca, startIndex, length);

        return subStringCopy;
    }

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ICharSequence? value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    unsafe IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char* value, int valueCount) => Append(value, valueCount);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ulong value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ushort value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char[]? value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char[]? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append<TFmt>(TFmt arg0, ICustomStringFormatter? customStringFormatter) =>
        Append(arg0, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlyMemory<char> value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlyMemory<char> value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(Span<char> value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(Span<char> value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlySpan<char> value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlySpan<char> value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(string? value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(string? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(string? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(float value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(sbyte value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(object? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(uint value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(int value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(long value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(StringBuilder? value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(StringBuilder? value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(StringBuilder? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ICharSequence? value, int startIndex, int length, string? formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ICharSequence? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter, FormatSwitches formatFlags) =>
        Append(value, startIndex, length, formatString, customStringFormatter, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(byte value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(bool value, ICustomStringFormatter? customStringFormatter) =>
        Append(value, customStringFormatter);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char[]? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(decimal value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(double value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(short value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char value, int repeatCount) => Append(value, repeatCount);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter, string format
      , TFmt arg0, FormatSwitches formatFlags) =>
        AppendFormat(customStringFormatter, format, arg0, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter
      , ReadOnlySpan<char> format, TFmt arg0, FormatSwitches formatFlags) =>
        AppendFormat(customStringFormatter, format, arg0, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0) =>
        AppendFormat(provider, format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TFmt>(string format, TFmt arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TFmt>(ReadOnlySpan<char> format, TFmt arg0) =>
        AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, string arg0) => AppendFormat(format, arg0);


    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam, TParam1>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam, TParam1, TParam2>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(IFormatProvider? provider, CompositeFormat format
      , params object?[] args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, ReadOnlySpan<char> arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(ReadOnlySpan<char> format, ReadOnlySpan<char> arg0) =>
        AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(ICustomStringFormatter customStringFormatter, string format
      , object? arg0, FormatSwitches formatFlags) =>
        AppendFormat(customStringFormatter, format, arg0, formatFlags);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, object? arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, object? arg0, object? arg1) => AppendFormat(format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, params object?[] args) => AppendFormat(format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, string format, object? arg0) =>
        AppendFormat(provider, format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, string format, object? arg0, object? arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, string format, params object?[] args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(string separator, params object?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin<TParam>
        (string separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(string separator, params string?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin<TParam>
        (char separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(char separator, params string?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(StringBuilder value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(ICharSequence value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine() => AppendLine();

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(string? value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Clear() => Clear();

    IStringBuilder IStringBuilder.Clone() => Clone();

    IStringBuilder IStringBuilder.CopyFrom(string source) => CopyFrom(source);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.CopyTo(int sourceIndex, Span<char> destination, int count) =>
        CopyTo(sourceIndex, destination, count);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) =>
        CopyTo(sourceIndex, destination, destinationIndex, count);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, bool value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int index, string? value, int count) => Insert(index, value, count);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, StringBuilder? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ulong value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ushort value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char[]? value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, Span<char> value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, Span<char> value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ReadOnlySpan<char> value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ReadOnlySpan<char> value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, string? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, float value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ICharSequence? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, object? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, sbyte value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, byte value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char[]? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, uint value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, double value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, short value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, int value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, long value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, decimal value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Remove(int startIndex, int length) => Remove(startIndex, length);

    IStringBuilder IStringBuilder.Remove(int startIndex) => Remove(startIndex);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(StringBuilder find, StringBuilder replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ICharSequence find, ICharSequence replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(StringBuilder find, StringBuilder replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(string find, string replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(char find, char replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(char find, char replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ICharSequence find, ICharSequence replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(string find, string? replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace) => Replace(find, replace);

    IStringBuilder IStringBuilder.Substring(int startIndex) => Substring(startIndex);

    IStringBuilder IStringBuilder.Substring(int startIndex, int length) => Substring(startIndex, length);

    IStringBuilder IStringBuilder.ToLower() => ToLower();

    IStringBuilder IStringBuilder.ToUpper() => ToUpper();

    IStringBuilder IStringBuilder.Trim() => Trim();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator() => ca.GetEnumerator();

    public override void StateReset()
    {
        ca.Clear();
        base.StateReset();
    }

    public void Dispose()
    {
        var onEndAction = ((IScopeDelimitedStringBuilder)this).OnScopeEndedAction;
        if (onEndAction != null)
        {
            onEndAction(this);
            ((IScopeDelimitedStringBuilder)this).OnScopeEndedAction = null;
        }
    }

    IStringBuilder ICloneable<IStringBuilder>.Clone() => Clone();

    public override CharArrayStringBuilder Clone() => Recycler?.Borrow<CharArrayStringBuilder>() ?? new CharArrayStringBuilder(this);

    object ICloneable.Clone() => Clone();

    public CharArrayStringBuilder CopyFrom(string source)
    {
        Clear();
        Append(source);
        return this;
    }

    public override CharArrayStringBuilder CopyFrom(CharArrayStringBuilder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Clear();
        ca.CopyFrom(source.ca);
        return this;
    }

    private bool Equals(CharArrayStringBuilder? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return 0 == CompareTo(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is MutableString s && Equals(s);
    }

    public bool Equals(string? toCompare)
    {
        if (toCompare == null) return false;
        var compareSpan = toCompare.AsSpan();
        return Equals(compareSpan);
    }

    public bool Equals([NotNullWhen(true)] StringBuilder? otherSb)
    {
        if (otherSb == null) return false;
        return 0 == CompareTo(otherSb);
    }

    public bool Equals(ReadOnlySpan<char> span)
    {
        var thisLen  = Length;
        var otherLen = span.Length;
        if (thisLen < otherLen) return false;
        if (thisLen > otherLen) return false;
        var minLen = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = ca[i] - span[i];
            if (cmp != 0) return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var len  = ca.Count;
        var hash = 0;

        for (var i = 0; i < len; i++) hash = (31 * hash) ^ ca[i];
        return hash;
    }

    public override string ToString() => ca.ToString();

    public string ToString(int startIndex, int length) => ca.ToString(startIndex, length);


    public static implicit operator CharArrayStringBuilder(string? initial) => initial != null ? new(initial) : null!;

    public static bool operator ==(string lhs, CharArrayStringBuilder rhs)
    {
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return rhs.Equals((ReadOnlySpan<char>)lhs);
    }

    public static bool operator !=(string lhs, CharArrayStringBuilder rhs) => !(lhs == rhs);

    public static bool operator ==(CharArrayStringBuilder? lhs, string? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return lhs.Equals((ReadOnlySpan<char>)rhs);
    }

    public static bool operator !=(CharArrayStringBuilder? lhs, string? rhs) => !(lhs == rhs);

    public static bool operator ==(CharArrayStringBuilder? lhs, CharArrayStringBuilder? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        return lhs.Equals(rhs);
    }

    public static bool operator !=(CharArrayStringBuilder? lhs, CharArrayStringBuilder? rhs) => !(lhs == rhs);

    // Copied from Microsoft StringBuilder 
    internal CharArrayStringBuilder AppendFormatHelper(IFormatProvider? provider, string format, ReadOnlySpan<object?> args)
    {
        ArgumentNullException.ThrowIfNull(format);

        // Undocumented exclusive limits on the range for Argument Hole Index and Argument Hole Alignment.
        const int indexLimit = 1_000_000; // Note:            0 <= ArgIndex < IndexLimit
        const int widthLimit = 1_000_000; // Note:  -WidthLimit <  ArgAlign < WidthLimit

        // Query the provider (if one was supplied) for an ICustomFormatter.  If there is one,
        // it needs to be used to transform all arguments.
        var cf = (ICustomFormatter?)provider?.GetFormat(typeof(ICustomFormatter));

        // Repeatedly find the next hole and process it.
        var pos = 0;
        while (true)
        {
            // Skip until either the end of the input or the first unescaped opening brace, whichever comes first.
            // Along the way we need to also unescape escaped closing braces.
            char ch;
            while (true)
            {
                // Find the next brace.  If there isn't one, the remainder of the input is text to be appended, and we're done.
                if ((uint)pos >= (uint)format.Length) { return this; }

                ReadOnlySpan<char> remainder           = format.AsSpan(pos);
                int                countUntilNextBrace = remainder.IndexOfAny('{', '}');
                if (countUntilNextBrace < 0)
                {
                    Append(remainder);
                    return this;
                }

                // Append the text until the brace.
                Append(remainder.Slice(0, countUntilNextBrace));
                pos += countUntilNextBrace;

                // Get the brace.  It must be followed by another character, either a copy of itself in the case of being
                // escaped, or an arbitrary character that's part of the hole in the case of an opening brace.
                char brace = format[pos];
                ch = MoveNext(format, ref pos);
                if (brace == ch)
                {
                    Append(ch);
                    pos++;
                    continue;
                }

                // This wasn't an escape, so it must be an opening brace.
                if (brace != '{') { throw new FormatException($"Unexpected Close brace at {pos}"); }

                // Proceed to parse the hole.
                break;
            }

            // We're now positioned just after the opening brace of an argument hole, which consists of
            // an opening brace, an index, an optional width preceded by a comma, and an optional format
            // preceded by a colon, with arbitrary amounts of spaces throughout.
            int                width          = 0;
            bool               leftJustify    = false;
            ReadOnlySpan<char> itemFormatSpan = default; // used if itemFormat is null

            // First up is the index parameter, which is of the form:
            //     at least on digit
            //     optional any number of spaces
            // We've already read the first digit into ch.
            Debug.Assert(format[pos - 1] == '{');
            Debug.Assert(ch != '{');
            int index = ch - '0';
            if ((uint)index >= 10u) { throw new FormatException($"Expected Ascii digit at {pos}"); }

            // Common case is a single digit index followed by a closing brace.  If it's not a closing brace,
            // proceed to finish parsing the full hole format.
            ch = MoveNext(format, ref pos);
            if (ch != '}')
            {
                // Continue consuming optional additional digits.
                while (char.IsAsciiDigit(ch) && index < indexLimit)
                {
                    index = index * 10 + ch - '0';
                    ch    = MoveNext(format, ref pos);
                }

                // Consume optional whitespace.
                while (ch == ' ') { ch = MoveNext(format, ref pos); }

                // Parse the optional alignment, which is of the form:
                //     comma
                //     optional any number of spaces
                //     optional -
                //     at least one digit
                //     optional any number of spaces
                if (ch == ',')
                {
                    // Consume optional whitespace.
                    do { ch = MoveNext(format, ref pos); } while (ch == ' ');

                    // Consume an optional minus sign indicating left alignment.
                    if (ch == '-')
                    {
                        leftJustify = true;
                        ch          = MoveNext(format, ref pos);
                    }

                    // Parse alignment digits. The read character must be a digit.
                    width = ch - '0';
                    if ((uint)width >= 10u) { throw new FormatException($"Expected Ascii digit at {pos}"); }
                    ch = MoveNext(format, ref pos);
                    while (char.IsAsciiDigit(ch) && width < widthLimit)
                    {
                        width = width * 10 + ch - '0';
                        ch    = MoveNext(format, ref pos);
                    }

                    // Consume optional whitespace
                    while (ch == ' ') { ch = MoveNext(format, ref pos); }
                }

                // The next character needs to either be a closing brace for the end of the hole,
                // or a colon indicating the start of the format.
                if (ch != '}')
                {
                    if (ch != ':')
                    {
                        // Unexpected character
                        throw new FormatException($"Expected either '}}' or ':' at {pos}");
                    }

                    // Search for the closing brace; everything in between is the format,
                    // but opening braces aren't allowed.
                    int startingPos = pos;
                    while (true)
                    {
                        ch = MoveNext(format, ref pos);

                        if (ch == '}')
                        {
                            // Argument hole closed
                            break;
                        }

                        if (ch == '{')
                        {
                            // Braces inside the argument hole are not supported
                            throw new FormatException($"Expected either '}}' at {pos}");
                        }
                    }

                    startingPos++;
                    itemFormatSpan = format.AsSpan(startingPos, pos - startingPos);
                }
            }

            // Construct the output for this arg hole.
            Debug.Assert(format[pos] == '}');
            pos++;
            string? s          = null;
            string? itemFormat = null;

            if ((uint)index >= (uint)args.Length) { throw new FormatException($"Argument index out of Range for identifier {{{index}}}"); }
            object? arg = args[index];

            if (cf != null)
            {
                if (!itemFormatSpan.IsEmpty) { itemFormat = new string(itemFormatSpan); }

                s = cf.Format(itemFormat, arg, provider);
            }

            if (s == null)
            {
                // If arg is ISpanFormattable and the beginning doesn't need padding,
                // try formatting it into the remaining current chunk.
                if ((leftJustify || width == 0) &&
                    arg is ISpanFormattable spanFormattableArg &&
                    spanFormattableArg.TryFormat(ca.BackingArray, out int charsWritten, itemFormatSpan, provider))
                {
                    if ((uint)charsWritten > (uint)ca.RemainingCapacity)
                    {
                        // Untrusted ISpanFormattable implementations might return an erroneous charsWritten value,
                        // and m_ChunkLength might end up being used in Unsafe code, so fail if we get back an
                        // out-of-range charsWritten value.
                        throw new FormatException($"Invalid String");
                    }

                    ca.Count += charsWritten;

                    // Pad the end, if needed.
                    if (leftJustify && width > charsWritten) { Append(' ', width - charsWritten); }

                    // Continue to parse other characters.
                    continue;
                }

                // Otherwise, fallback to trying IFormattable or calling ToString.
                if (arg is IFormattable formattableArg)
                {
                    if (itemFormatSpan.Length != 0) { itemFormat ??= new string(itemFormatSpan); }
                    s = formattableArg.ToString(itemFormat, provider);
                }
                else { s = arg?.ToString(); }

                s ??= string.Empty;
            }

            // Append it to the final output of the Format String.
            if (width <= s.Length) { Append(s); }
            else if (leftJustify)
            {
                Append(s);
                Append(' ', width - s.Length);
            }
            else
            {
                Append(' ', width - s.Length);
                Append(s);
            }

            // Continue parsing the rest of the format string.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char MoveNext(string format, ref int pos)
        {
            pos++;
            if ((uint)pos >= (uint)format.Length) { throw new FormatException($"Invalid String. Unclosed Format token"); }
            return format[pos];
        }
    }

    private CharArrayStringBuilder AppendJoinCore<T>(ReadOnlySpan<char> separator, IEnumerable<T> values)
    {
        Debug.Assert(separator.Length >= 0);

        if (values == null) { throw new ArgumentException("Did not expect values to be null"); }

        Debug.Assert(values != null);
        using (IEnumerator<T> en = values.GetEnumerator())
        {
            if (!en.MoveNext()) { return this; }

            T value = en.Current;
            if (value != null) { Append(value.ToString()); }

            while (en.MoveNext())
            {
                Append(separator);
                value = en.Current;
                if (value != null) { Append(value.ToString()); }
            }
        }
        return this;
    }


    private CharArrayStringBuilder AppendJoinCore<T>(ReadOnlySpan<char> separator, T[] values)
    {
        if (values == null) { throw new ArgumentException("Did not expect values to be null"); }

        Debug.Assert(values != null);
        if (values.Length == 0) { return this; }

        if (values[0] != null) { Append(values[0]!.ToString()); }

        for (int i = 1; i < values.Length; i++)
        {
            Append(separator);
            if (values[i] != null) { Append(values[i]!.ToString()); }
        }
        return this;
    }
}
