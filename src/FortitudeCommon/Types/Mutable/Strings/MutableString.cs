// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;

#endregion

namespace FortitudeCommon.Types.Mutable.Strings;

[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public sealed class MutableString : ReusableObject<IMutableString>, IMutableString, ITransferState<MutableString>
{
    private static readonly Recycler EnumeratorPool = new();

    internal static readonly char[] WhiteSpaceChars = [' ', '\t', '\r', '\n'];

    private readonly StringBuilder sb;

    private bool throwOnMutateAttempt = true;

    public MutableString() => sb = new StringBuilder();

    public MutableString(IMutableString initialString)
    {
        sb = new StringBuilder();
        Append(initialString);
    }

    public MutableString(string? initialString)
    {
        sb = new StringBuilder();
        sb.Append(initialString);
    }

    public MutableString(int initialCapacity) => sb = new StringBuilder(initialCapacity);

    public MutableString(StringBuilder initializedBuilder) => sb = initializedBuilder;

    IMaybeFrozen IFreezable.Freeze => Freeze;

    public IFrozenString Freeze
    {
        get
        {
            if (!IsFrozen)
            {
                IsFrozen = true;
            }
            return this;
        }
    }

    public bool ThrowOnMutateAttempt
    {
        get => throwOnMutateAttempt;
        set
        {
            if (!IsFrozen)
                throwOnMutateAttempt = value;
            else
                ShouldThrow();
        }
    }

    public bool IsFrozen { get; private set; }

    public int Capacity
    {
        get => sb.Capacity;
        set => sb.Capacity = value;
    }
    public int MaxCapacity => sb.MaxCapacity;

    private MutableString ShouldThrow() =>
        !ThrowOnMutateAttempt ? this : throw new ModifyFrozenObjectAttempt("Attempted to modify a frozen MutableString");


    IMutableString IMutableStringBuilder<IMutableString>.Append(IFrozenString? value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(StringBuilder? value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append
        (StringBuilder? value, int startIndex, int length) =>
        Append(value, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Append(bool value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(byte value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char value, int repeat) => Append(value, repeat);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char[]? value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char[]? value, int startIndex, int length) => Append(value);

    unsafe IMutableString IMutableStringBuilder<IMutableString>.Append(char* value, int valueCount) => Append(value, valueCount);

    IMutableString IMutableStringBuilder<IMutableString>.Append(decimal value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(double value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(short value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(int value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(long value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(object? value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(sbyte value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(float value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(string? value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(string? value, int startIndex, int length) => Append(value, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ReadOnlySpan<char> value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ReadOnlyMemory<char> value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ushort value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(uint value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ulong value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) => AppendFormat(format, arg0);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) => AppendFormat(format, arg0, arg1);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(format, arg0, arg1, arg2);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) => AppendFormat(format, args);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat(IFormatProvider? provider, CompositeFormat format, params object?[] args) =>
        AppendFormat(provider, format, args);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) =>
        AppendFormat(provider, format, arg0);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        AppendFormat(provider, format, args);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat
        (IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args) =>
        AppendFormat(provider, format, args);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0) =>
        AppendFormat(provider, format, arg0);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat<TParam, TParam1>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IMutableString IMutableStringBuilder<IMutableString>.AppendFormat<TParam, TParam1, TParam2>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IMutableString IMutableStringBuilder<IMutableString>.AppendJoin(string? separator, params object?[] values) => AppendJoin(separator, values);

    IMutableString IMutableStringBuilder<IMutableString>.AppendJoin(string? separator, params string?[] values) => AppendJoin(separator, values);

    IMutableString IMutableStringBuilder<IMutableString>.AppendJoin(char separator, params string?[] values) => AppendJoin(separator, values);

    IMutableString IMutableStringBuilder<IMutableString>.AppendJoin<TParam>(string? separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IMutableString IMutableStringBuilder<IMutableString>.AppendJoin<TParam>(char separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine() => AppendLine();

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine(string? value) => AppendLine(value);

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine(IFrozenString value) => AppendLine(value);

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine(StringBuilder value) => AppendLine(value);

    IMutableString IMutableStringBuilder<IMutableString>.Clear() => Clear();

    IMutableString IMutableStringBuilder<IMutableString>.CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) =>
        CopyTo(sourceIndex, destination, destinationIndex, count);

    IMutableString IMutableStringBuilder<IMutableString>.CopyTo(int sourceIndex, Span<char> destination, int count) =>
        CopyTo(sourceIndex, destination, count);

    int IMutableStringBuilder<IMutableString>.EnsureCapacity(int capacity) => EnsureCapacity(capacity);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, bool value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, byte value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[]? value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, decimal value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, double value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, short value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, int value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, long value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, object? value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, sbyte value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, float value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, string? value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, IFrozenString? value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, StringBuilder? value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Remove(int startIndex, int length) => Remove(startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[]? value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ushort value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, uint value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ulong value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, string? value, int count) => Insert(atIndex, value, count);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ReadOnlySpan<char> value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace) => Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string? replace) => Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(IFrozenString find, IFrozenString replace) => Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(IFrozenString find, IFrozenString replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, Length);

    IMutableString IStringBuilder<IMutableString>.Remove(int startIndex) => Remove(startIndex);

    IMutableString IStringBuilder<IMutableString>.ToUpper() => ToUpper();

    IMutableString IStringBuilder<IMutableString>.ToLower() => ToLower();

    IMutableString IStringBuilder<IMutableString>.Trim() => Trim();

    IMutableString IStringBuilder<IMutableString>.Substring(int startIndex) => Substring(startIndex);

    IMutableString IStringBuilder<IMutableString>.Substring(int startIndex, int length) => Substring(startIndex, length);


    public int Length
    {
        get => sb.Length;
        set
        {
            if (IsFrozen)
            {
                ShouldThrow();
                return;
            }
            sb.Length = value;
        }
    }

    public char this[int index]
    {
        get => sb[index];
        set
        {
            if (IsFrozen)
            {
                ShouldThrow();
                return;
            }
            sb[index] = value;
        }
    }

    public void CopyTo(char[] array, int arrayIndex, int fromMyIndex = 0, int myLength = int.MaxValue)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex; i < array.Length && myIndex < Length && myIndex < myEnd; i++)
        {
            array[i] = sb[myIndex++];
        }
    }

    public void CopyTo(RecyclingCharArray array, int? arrayIndex = null, int fromMyIndex = 0, int myLength = int.MaxValue)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex ?? array.Count; i < array.Count && myIndex < Length && myIndex < myEnd; i++)
        {
            array[i] = sb[myIndex++];
        }
    }

    public void CopyTo(Span<char> charSpan, int spanIndex, int fromMyIndex = 0, int myLength = int.MaxValue)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = spanIndex; i < charSpan.Length && myIndex < Length && myIndex < myEnd; i++)
        {
            charSpan[i] = sb[myIndex++];
        }
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

    public StringBuilder BackingStringBuilder => sb;

    public int CompareTo(string other)
    {
        var thisLen  = Length;
        var otherLen = other.Length;
        var minLen   = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = sb[i] - other[i];
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
            var cmp = sb[i] - other[i];
            if (cmp != 0) return cmp;
        }

        return 0;
    }

    public bool EquivalentTo(string other)
    {
        if (ReferenceEquals(null, other)) return false;
        return 0 == CompareTo(other);
    }

    public MutableString Append(IMutableString? value)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        for (var i = 0; i < value.Length; i++) Append(value[i]);
        return this;
    }

    public MutableString Append(bool value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(byte value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(char value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(char value, int repeatCount)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value, repeatCount);
        return this;
    }

    public MutableString Append(char[]? value)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(char[]? value, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value, startIndex, length);
        return this;
    }

    public unsafe MutableString Append(char* value, int valueCount)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value, valueCount);
        return this;
    }

    public MutableString Append(decimal value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(double value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(short value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(int value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(long value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(StringBuilder? value)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(StringBuilder? value, int startIndex, int length)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.Append(value, startIndex, length);
        return this;
    }

    public MutableString Append(IFrozenString? value)
    {
        if(value == null) return this;
        if (IsFrozen) return ShouldThrow();
        for (int i = 0; i < value.Length; i++)
        {
            sb.Append(value[i]);
        }
        return this;
    }

    public MutableString Append(object? value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(sbyte value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(float value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(string? value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(string? value, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value, startIndex, length);
        return this;
    }

    public MutableString Append(ReadOnlySpan<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(ReadOnlyMemory<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(ushort value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(uint value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(ulong value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(format, arg0);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(format, arg0, arg1);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(format, arg0, arg1, arg2);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(format, args);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1, arg2);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, args);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, CompositeFormat format, params object?[] args)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, args);
        return this;
    }

    public MutableString AppendFormat(IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, args);
        return this;
    }

    public MutableString AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0);
        return this;
    }

    public MutableString AppendFormat<TParam, TParam1>(IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1);
        return this;
    }

    public MutableString AppendFormat<TParam, TParam1, TParam2>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1, arg2);
        return this;
    }

    public MutableString AppendJoin(string? separator, params object?[] values)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendJoin(separator, values);
        return this;
    }

    public MutableString AppendJoin(string? separator, params string?[] values)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendJoin(separator, values);
        return this;
    }

    public MutableString AppendJoin(char separator, params string?[] values)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendJoin(separator, values);
        return this;
    }

    public MutableString AppendJoin<TParam>(string? separator, IEnumerable<TParam> values)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendJoin(separator, values);
        return this;
    }

    public MutableString AppendJoin<TParam>(char separator, IEnumerable<TParam> values)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendJoin(separator, values);
        return this;
    }

    public MutableString AppendLine()
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendLine();
        return this;
    }

    public MutableString AppendLine(string? value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendLine(value);
        return this;
    }

    public MutableString AppendLine(StringBuilder value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendRange(value).AppendLine();
        return this;
    }

    public MutableString AppendLine(IFrozenString value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value).AppendLine();
        return this;
    }

    public MutableString Clear()
    {
        if (IsFrozen) return ShouldThrow();
        sb.Clear();
        return this;
    }

    public MutableString CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
        sb.CopyTo(sourceIndex, destination, destinationIndex, count);
        return this;
    }

    public MutableString CopyTo(int sourceIndex, Span<char> destination, int count)
    {
        sb.CopyTo(sourceIndex, destination, count);
        return this;
    }

    public int EnsureCapacity(int capacity)
    {
        return sb.EnsureCapacity(capacity);
    }

    public MutableString Insert(int atIndex, bool value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, byte value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char[]? value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, decimal value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, double value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, short value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, int value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, long value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, object? value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, sbyte value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, float value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, string? value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char[]? value, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value, startIndex, length);
        return this;
    }

    public MutableString Insert(int atIndex, ushort value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, uint value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, ulong value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, IFrozenString? value)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.InsertAt(value, atIndex);
        return this;
    }

    public MutableString Insert(int atIndex, StringBuilder? value)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.InsertAt(value, atIndex);
        return this;
    }

    public MutableString Insert(int index, string? value, int count)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        sb.Insert(index, value, count);
        return this;
    }

    public MutableString Insert(int atIndex, ReadOnlySpan<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Remove(int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Remove(startIndex, length);
        return this;
    }

    public MutableString Replace(char find, char replace)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Replace(find, replace);
        return this;
    }

    public MutableString Replace(char find, char replace, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Replace(find, replace, startIndex, length);
        return this;
    }

    public MutableString Replace(string find, string? replace)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Replace(find, replace);
        return this;
    }

    public MutableString Replace(string find, string replace, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Replace(find, replace, startIndex, length);
        return this;
    }

    public MutableString Replace(IFrozenString find, IFrozenString replace) => Replace(find, replace, 0, Length);

    public MutableString Replace(IFrozenString find, IFrozenString replace, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        var fromIndex = startIndex;
        int indexOfFind;
        while ((indexOfFind = IndexOf(find, fromIndex)) >= 0
            && fromIndex < startIndex + length)
        {
            var remainingIndexOf  = 0;
            var highestMatchIndex = indexOfFind;
            for (fromIndex = 0; fromIndex < indexOfFind + find.Length; fromIndex++)
                if (remainingIndexOf < replace.Length)
                {
                    highestMatchIndex = remainingIndexOf++;
                    sb[fromIndex]     = replace[highestMatchIndex];
                }
                else
                {
                    Remove(highestMatchIndex + 1);
                }

            for (; remainingIndexOf < replace.Length; fromIndex++) sb.Insert(remainingIndexOf++, replace[highestMatchIndex]);
        }

        return this;
    }

    public IMutableString Replace(StringBuilder find, StringBuilder replace) => Replace(find, replace, 0, Length);

    public IMutableString Replace(StringBuilder find, StringBuilder replace, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        var fromIndex = startIndex;
        int indexOfFind;
        while ((indexOfFind = IndexOf(find, fromIndex)) >= 0
            && fromIndex < startIndex + length)
        {
            var remainingIndexOf  = 0;
            var highestMatchIndex = indexOfFind;
            for (fromIndex = 0; fromIndex < indexOfFind + find.Length; fromIndex++)
                if (remainingIndexOf < replace.Length)
                {
                    highestMatchIndex = remainingIndexOf++;
                    sb[fromIndex]     = replace[highestMatchIndex];
                }
                else
                {
                    Remove(highestMatchIndex + 1);
                }

            for (; remainingIndexOf < replace.Length; fromIndex++) sb.Insert(remainingIndexOf++, replace[highestMatchIndex]);
        }

        return this;
    }

    public MutableString Remove(int startIndex)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Remove(startIndex, 1);
        return this;
    }

    public MutableString ToUpper()
    {
        if (IsFrozen) return ShouldThrow();
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar   = sb[i];
            var upperChar = char.ToUpperInvariant(oldChar);

            if (upperChar != oldChar) sb[i] = upperChar;
        }

        return this;
    }

    public MutableString ToLower()
    {
        if (IsFrozen) return ShouldThrow();
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar   = sb[i];
            var upperChar = char.ToLowerInvariant(oldChar);

            if (upperChar != oldChar) sb[i] = upperChar;
        }

        return this;
    }

    public MutableString Trim()
    {
        if (IsFrozen) return ShouldThrow();
        for (var i = 0; i < sb.Length; i++)
        {
            var checkChar = sb[i];
            if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0) sb.Remove(i--, 1);
        }

        for (var i = sb.Length - 1; i >= 0; i--)
        {
            var checkChar = sb[i];
            if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0) sb.Remove(i++, 1);
        }

        return this;
    }


    public MutableString Substring(int startIndex)
    {
        if (IsFrozen) return ShouldThrow();
        if (IsFrozen)
        {
            return ShouldThrow();
        }
        sb.Remove(0, startIndex);
        return this;
    }

    public MutableString Substring(int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Remove(0, startIndex);
        sb.Remove(length, Length - length);
        return this;
    }

    public IMutableString SourceThawed => IsFrozen ? Clone() : this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator() => sb.RecycledEnumerator(EnumeratorPool);

    public override void StateReset()
    {
        IsFrozen = false;
        sb.Clear();
        base.StateReset();
    }

    // ReSharper disable once UnusedMember.Global
    public IFrozenString NewFrozenString => new FrozenMutableStringWrapper((MutableString)(Clone().Freeze));

    IStringBuilder<IMutableString> ICloneable<IStringBuilder<IMutableString>>.Clone() => Clone();

    public override MutableString Clone() => Recycler?.Borrow<MutableString>() ?? new MutableString(this);

    object ICloneable.Clone() => Clone();

    IMutableString ICloneable<IMutableString>.Clone() => Clone();

    // ReSharper disable once OptionalParameterHierarchyMismatch
    public override IMutableString CopyFrom(IMutableString source, CopyMergeFlags copyMergeFlags)
    {
        if (IsFrozen)
        {
            return ShouldThrow();
        }
        Clear();
        sb.AppendRange(source);

        return this;
    }

    public MutableString CopyFrom(MutableString source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (IsFrozen) return ShouldThrow();
        Clear();
        sb.AppendRange(source);
        return this;
    }

    public IFrozenString CopyFrom(IFrozenString source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Clear(); // does FrozenCheck
        Append(source);

        return this;
    }

    public IMutableString CopyFrom(string source)
    {
        Clear(); // does FrozenCheck
        Append(source);

        return this;
    }

    private bool Equals(MutableString? other)
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

    public bool Equals([NotNullWhen(true)] StringBuilder? otherSb)
    {
        return sb.Equals(otherSb);
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
            var cmp = sb[i] - span[i];
            if (cmp != 0) return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var len  = sb.Length;
        var hash = 0;

        for (var i = 0; i < len; i++) hash = (31 * hash) ^ sb[i];
        return hash;
    }

    public override string ToString() => sb.ToString();

    public string ToString(int startIndex, int length) => sb.ToString(startIndex, length);


    public static implicit operator MutableString(string? initial) => initial != null ? new(initial) : null!;

    public static bool operator ==(string lhs, MutableString rhs)
    {
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return rhs.Equals((ReadOnlySpan<char>)lhs);
    }

    public static bool operator !=(string lhs, MutableString rhs) => !(lhs == rhs);

    public static bool operator ==(MutableString? lhs, string? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return lhs.Equals((ReadOnlySpan<char>)rhs);
    }

    public static bool operator !=(MutableString? lhs, string? rhs) => !(lhs == rhs);

    public static bool operator ==(MutableString? lhs, MutableString? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        return lhs.Equals(rhs);
    }

    public static bool operator !=(MutableString? lhs, MutableString? rhs) => !(lhs == rhs);


    private class FrozenMutableStringWrapper(MutableString ms) : IFrozenString
    {
        public bool AutoRecycleAtRefCountZero
        {
            get => ms.AutoRecycleAtRefCountZero;
            set => ms.AutoRecycleAtRefCountZero = value;
        }

        public bool IsFrozen => ms.IsFrozen;

        public int CompareTo(ICharSequence other) => ms.CompareTo(other);

        public int CompareTo(string other) => ms.CompareTo(other);

        public bool Contains(ICharSequence subStr) => ms.Contains(subStr);

        public bool Contains(string subStr) => ms.Contains(subStr);

        public void CopyTo
            (Span<char> charSpan, int spanIndex, int fromMyIndex = 0, int myLength = Int32.MaxValue) =>
            ms.CopyTo(charSpan, spanIndex, fromMyIndex, myLength);

        public void CopyTo
            (RecyclingCharArray array, int? arrayIndex = null, int fromMyIndex = 0, int myLength = Int32.MaxValue) =>
            ms.CopyTo(array, arrayIndex, fromMyIndex, myLength);

        public void CopyTo
            (char[] array, int arrayIndex, int fromMyIndex = 0, int myLength = Int32.MaxValue) =>
            ms.CopyTo(array, arrayIndex, fromMyIndex, myLength);

        public int DecrementRefCount() => ms.DecrementRefCount();

        public bool EquivalentTo(string other) => ms.EquivalentTo(other);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<char> GetEnumerator() => ms.GetEnumerator();

        public int IncrementRefCount() => ms.IncrementRefCount();

        public int IndexOf(ICharSequence subStr, int fromThisPos) => ms.IndexOf(subStr, fromThisPos);

        public int IndexOf(string subStr) => ms.IndexOf(subStr);

        public int IndexOf(ICharSequence subStr) => ms.IndexOf(subStr);

        public int IndexOf(string subStr, int fromThisPos) => ms.IndexOf(subStr, fromThisPos);

        public bool IsInRecycler
        {
            get => ms.IsInRecycler;
            set => throw new NotImplementedException("This should never get set as this is always attached to a MutableString");
        }

        public char this[int index] => ms[index];

        public int LastIndexOf(ICharSequence subStr) => ms.LastIndexOf(subStr);

        public int LastIndexOf(string subStr, int fromThisPos) => ms.LastIndexOf(subStr, fromThisPos);

        public int LastIndexOf(ICharSequence subStr, int fromThisPos) => ms.LastIndexOf(subStr, fromThisPos);

        public int LastIndexOf(string subStr) => ms.LastIndexOf(subStr);

        public int Length => ms.Length;

        public bool Recycle() => ms.Recycle();

        public IRecycler? Recycler
        {
            get => ms.Recycler;
            set => ms.Recycler = value;
        }
        public int RefCount => ms.RefCount;

        public IMutableString SourceThawed => ms.SourceThawed;

        public void StateReset() { }

        public override string ToString() => ms.ToString();
    }
}

public static class MutableStringExtensions
{
    public static IMutableString? TransferOrCreate(this IMutableString? possibleExisting, string? possibleNew)
    {
        var result = possibleExisting;
        if (result != null)
        {
            result.Clear();
            if (possibleNew != null) result.Append(possibleNew);
        }
        {
            result = (MutableString?)possibleNew;
        }
        return result;
    }

    // ReSharper disable once ReturnTypeCanBeNotNullable
    public static MutableString? TransferOrCreate(this MutableString? possibleExisting, string? possibleNew)
    {
        var result = possibleExisting;
        if (result != null)
        {
            result.Clear();
            if (possibleNew != null) result.Append(possibleNew);
        }
        {
            result = possibleNew;
        }
        return result;
    }

    public static IMutableString? TransferOrReplace(this IMutableString? possibleExisting, IMutableString? possibleNew)
    {
        if (ReferenceEquals(possibleExisting, possibleNew)) return possibleExisting;
        var result = possibleExisting;
        if (result != null)
        {
            result.Clear();
            if (possibleNew != null) result.Append(possibleNew);
        }
        {
            result = possibleNew;
        }
        return result;
    }

    public static MutableString? TransferOrReplace(this MutableString? possibleExisting, IMutableString? possibleNew)
    {
        if (ReferenceEquals(possibleExisting, possibleNew)) return possibleExisting;
        var result = possibleExisting;
        if (result != null)
        {
            result.Clear();
            if (possibleNew != null) result.Append(possibleNew);
        }
        {
            result = (MutableString?)possibleNew;
        }
        return result;
    }
}
