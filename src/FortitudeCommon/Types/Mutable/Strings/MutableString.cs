// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;

// ReSharper disable MemberCanBePrivate.Global

#endregion

namespace FortitudeCommon.Types.Mutable.Strings;

[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public sealed class MutableString : ReusableObject<IMutableString>, IMutableString, ITransferState<MutableString>, IScopeDelimitedStringBuilder
{
    private static readonly Recycler                                               EnumeratorPool                           = new();
    
    private static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> CustomSpanFormattableProviders           = new ();
    private static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> CustomStyledToStringFormattableProviders = new ();

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

    Action<IScopeDelimitedStringBuilder>? IScopeDelimitedStringBuilder.OnScopeEndedAction { get; set; }

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

    private bool TryGetCachedCustomSpanFormatter<T>([NotNullWhen(true)] out CustomSpanFormattable<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (CustomSpanFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportSpanFormattable && formattableProvider is ICustomSpanFormattableProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomSpanFormattable;
                return true;
            }
        }
        return false;
    }
    
    private bool TryGetCachedCustomStyledToStringFormatter<T>([NotNullWhen(true)] out CustomTypeStyler<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (CustomStyledToStringFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportStyleToString && formattableProvider is ICustomTypeStylerProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomTypeStyler;
                return true;
            }
        }
        return false;
    }

    private MutableString ShouldThrow() =>
        !ThrowOnMutateAttempt ? this : throw new ModifyFrozenObjectAttempt("Attempted to modify a frozen MutableString");


    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ICharSequence? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(StringBuilder? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append
        (ICharSequence? value, int startIndex, int length, string? formatString) =>
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append
        (StringBuilder? value, int startIndex, int length, string? formatString) =>
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(bool value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(byte value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char value, int repeat) => Append(value, repeat);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char[]? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char[]? value, int startIndex, int length, string? formatString) => 
        Append(value, startIndex, length, formatString);

    unsafe IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(char* value, int valueCount) => Append(value, valueCount);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(decimal value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(double value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(short value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(int value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(long value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(object? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(sbyte value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(float value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(string? value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(string? value, int startIndex, int length, string? formatString) => 
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append<TStruct>(TStruct arg0) => Append(arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append<TStruct>(TStruct? arg0) => Append(arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(Span<char> value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(Span<char> value, int startIndex, int length, string? formatString) => 
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlySpan<char> value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlySpan<char> value, int startIndex, int length, string? formatString) =>
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlyMemory<char> value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ReadOnlyMemory<char> value, int startIndex, int length, string? formatString) =>
        Append(value, startIndex, length, formatString);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ushort value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(uint value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Append(ulong value) => Append(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendSpanFormattable<TClass>(string format, TClass? arg0) 
        where TClass : class => AppendSpanFormattable(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TStruct>(string format, TStruct arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TStruct>(string format, TStruct? arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(string format, string arg0) => AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, ReadOnlySpan<char> arg0) =>
        AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) =>
        AppendFormat(format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) =>
        AppendFormat(format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        ([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        AppendFormat(format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat(IFormatProvider? provider, CompositeFormat format, params object?[] args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) =>
        AppendFormat(provider, format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat
        (IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args) =>
        AppendFormat(provider, format, args);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0) =>
        AppendFormat(provider, format, arg0);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam, TParam1>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1) =>
        AppendFormat(provider, format, arg0, arg1);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendFormat<TParam, TParam1, TParam2>
        (IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2) =>
        AppendFormat(provider, format, arg0, arg1, arg2);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(string? separator, params object?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(string? separator, params string?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin(char separator, params string?[] values) => AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin<TParam>(string? separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendJoin<TParam>(char separator, IEnumerable<TParam> values) =>
        AppendJoin(separator, values);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine() => AppendLine();

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(string? value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(ICharSequence value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.AppendLine(StringBuilder value) => AppendLine(value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Clear() => Clear();

    IStringBuilder IMutableStringBuilder<IStringBuilder>.CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) =>
        CopyTo(sourceIndex, destination, destinationIndex, count);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.CopyTo(int sourceIndex, Span<char> destination, int count) =>
        CopyTo(sourceIndex, destination, count);

    int IMutableStringBuilder<IStringBuilder>.EnsureCapacity(int capacity) => EnsureCapacity(capacity);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, bool value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, byte value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char[]? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, decimal value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, double value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, short value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, int value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, long value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, object? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, sbyte value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, float value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, string? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ICharSequence? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, StringBuilder? value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Remove(int startIndex, int length) => Remove(startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, char[]? value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ushort value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, uint value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ulong value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, string? value, int count) => Insert(atIndex, value, count);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, Span<char> value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, Span<char> value, int startIndex, int length) => 
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ReadOnlySpan<char> value) => Insert(atIndex, value);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Insert(int atIndex, ReadOnlySpan<char> value, int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(char find, char replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(char find, char replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(string find, string? replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(string find, string replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ICharSequence find, ICharSequence replace) => Replace(find, replace);

    IStringBuilder IMutableStringBuilder<IStringBuilder>.Replace(ICharSequence find, ICharSequence replace, int startIndex, int length) =>
        Replace(find, replace, startIndex, Length);

    IStringBuilder IStringBuilder.Remove(int startIndex) => Remove(startIndex);

    IStringBuilder IStringBuilder.ToUpper() => ToUpper();

    IStringBuilder IStringBuilder.ToLower() => ToLower();

    IStringBuilder IStringBuilder.Trim() => Trim();

    IStringBuilder IStringBuilder.Substring(int startIndex) => Substring(startIndex);

    IStringBuilder IStringBuilder.Substring(int startIndex, int length) => Substring(startIndex, length);


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

    public void CopyTo(char[] array, int arrayIndex, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex; i < array.Length && myIndex < Length && myIndex < myEnd; i++)
        {
            array[i] = sb[myIndex++];
        }
    }

    public void CopyTo(RecyclingCharArray array, int? arrayIndex = null, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex = fromMyIndex;
        var myEnd   = myLength != int.MaxValue ? fromMyIndex + myLength : myLength;
        for (var i = arrayIndex ?? array.Count; i < array.Count && myIndex < Length && myIndex < myEnd; i++)
        {
            array[i] = sb[myIndex++];
        }
    }

    public void CopyTo(Span<char> charSpan, int spanIndex, int myLength = int.MaxValue, int fromMyIndex = 0)
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

    public MutableString Append(char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (IsFrozen) return ShouldThrow();
        if (value == null) return this;
        if (formatString == null)
        {
            sb.Append(value, startIndex, length);
        }
        else
        {
            var cappedLength    = Math.Clamp(length, 256, 256 + value.Length - startIndex);
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
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

    public MutableString Append(StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        if (formatString == null)
        {
            sb.Append(value, startIndex, length);
        }
        else
        {
            var cappedLength    = Math.Clamp(length, 256, 256 + value.Length - startIndex);
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
        return this;
    }

    public MutableString Append(ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (value == null) return this;
        if (IsFrozen) return ShouldThrow();
        if (formatString == null)
        {
            sb.AppendRange(value, startIndex, length);
        }
        else
        {
            var cappedLength       = Math.Clamp(length, 256, 256 + value.Length - startIndex);
            var maxTransferSize    = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan        = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
            
        }
        return this;
    }

    public MutableString Append(IFrozenString? value)
    {
        if (value == null) return this;
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

    public MutableString Append(string? value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (IsFrozen) return ShouldThrow();
        if (value == null) return this;
        if (formatString == null)
        {
            sb.Append(value, startIndex, length);
        }
        else
        {
            var cappedLength    = Math.Clamp(length, 256, 256 + value.Length - startIndex);
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
        return this;
    }

    public MutableString Append<TStruct>(TStruct arg0) 
        where TStruct : struct, ISpanFormattable
    {
        var charSpan     = stackalloc char[256].ResetMemory();
        if (arg0.TryFormat(charSpan, out var charsWritten, format: default, provider: null))
        {
            sb.Append(charSpan[..charsWritten]);
        }

        return this;
    }

    public MutableString Append<TStruct>(TStruct? arg0) 
        where TStruct : struct, ISpanFormattable
    {
        if (arg0 == null) return this;

        Append(arg0.Value);
        return this;
    }

    public MutableString Append(Span<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(Span<char> value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (IsFrozen) return ShouldThrow();
        
        if (formatString == null)
        {
            var cappedLength = Math.Clamp(length, 0, value.Length - startIndex);
            var endIndex     = startIndex + cappedLength;
            for (int i = startIndex; i < endIndex; i++)
            {
                sb.Append(value[i]);
            }
        }
        else
        {
            var cappedLength = Math.Clamp(length, 256, 256 + value.Length - startIndex);
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
        return this;
    }

    public MutableString Append(ReadOnlySpan<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(ReadOnlySpan<char> value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (IsFrozen) return ShouldThrow();
        var cappedLength = Math.Clamp(length, 256, 256 + value.Length - startIndex);
        var endIndex     = startIndex + cappedLength;
        if (formatString == null)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                sb.Append(value[i]);
            }
        }
        else
        {
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, value, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
        return this;
    }

    public MutableString Append(ReadOnlyMemory<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Append(value);
        return this;
    }

    public MutableString Append(ReadOnlyMemory<char> value, int startIndex, int length = int.MaxValue, string? formatString = null)
    {
        if (IsFrozen) return ShouldThrow();
        var asSpan       = value.Span;
        var cappedLength = Math.Clamp(length, 256, 256 + value.Length - startIndex);
        var endIndex     = startIndex + cappedLength;
        if (formatString == null)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                sb.Append(asSpan[i]);
            }
        }
        else
        {
            var maxTransferSize = Math.Min(cappedLength, 512*1024);
            var rangeAsSpan     = stackalloc char[maxTransferSize].ResetMemory();
            
            var memoryAsSpan = value.Span;
            
            rangeAsSpan.SingleTokenFormatAt(0, formatString, memoryAsSpan, startIndex, length);
            var size = rangeAsSpan.PopulatedLength();
            sb.Append(rangeAsSpan[..size]);
        }
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

    public MutableString AppendSpanFormattable<TClass>(string format, TClass? arg0) where TClass : class, ISpanFormattable
    {
        try
        {
            var charSpan     = stackalloc char[2048].ResetMemory();
            if (arg0?.TryFormat(charSpan, out var charsWritten, format: format, provider: null) ?? false)
            {
                sb.Append(charSpan[..charsWritten]);
            }
        }
        catch (FormatException)
        {
            AppendFormat(format, arg0.ToString()!);
        }

        return this;
    }

    public MutableString AppendFormat<TStruct>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TStruct arg0) 
        where TStruct : struct, ISpanFormattable
    {
        if (TryGetCachedCustomSpanFormatter<TStruct>(out var formatter))
        {
            var charSpan     = stackalloc char[256].ResetMemory();
            var charsWritten = formatter(arg0, charSpan, format, null);
            sb.Append(charSpan[..charsWritten]);
            return this;
        }
        if (arg0 is Enum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateStructEnumFormatProvider<TStruct>();
            CustomSpanFormattableProviders.TryAdd(typeof(TStruct), enumFormatProvider);
            formatter =  enumFormatProvider.CustomSpanFormattable;
            var charSpan     = stackalloc char[1024].ResetMemory();
            var charsWritten = formatter(arg0, charSpan, format, null);
            sb.Append(charSpan[..charsWritten]);
            return this;
        }
        try
        {
            var charSpan = stackalloc char[1024].ResetMemory();
            if (arg0.TryFormat(charSpan, out var charsWritten, format: format, provider: null))
            {
                sb.Append(charSpan[..charsWritten]);
            }
        }
        catch (FormatException)
        {
            AppendFormat(format, arg0.ToString()!);
        }
        return this;
    }

    public MutableString AppendFormat<TStruct>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TStruct? arg0) 
        where TStruct : struct, ISpanFormattable
    {
        if (arg0 == null) return this;

        AppendFormat(format, arg0.Value);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, string arg0)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(format, arg0);
        return this;
    }

    public MutableString AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, ReadOnlySpan<char> arg0)
    {
        if (IsFrozen) return ShouldThrow();
        format.AsSpan().ExtractStringFormatStages(out var _, out var layout, out var formatting);
        if(layout.Length == 0) return Append(arg0);
        var cappedSize   = Math.Min(4096, arg0.Length + 256);
        var charSpan     = stackalloc char[cappedSize].ResetMemory();
        var charsWritten = charSpan.PadAndAlign(arg0, layout);
        sb.Append(charSpan[..charsWritten]);
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

    public MutableString AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1);
        return this;
    }

    public MutableString AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        if (IsFrozen) return ShouldThrow();
        sb.AppendFormat(provider, format, arg0, arg1, arg2);
        return this;
    }

    public MutableString AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
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

    public MutableString AppendLine(ICharSequence value)
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

    public MutableString Insert(int atIndex, ICharSequence? value)
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

    public MutableString Insert(int atIndex, Span<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, Span<char> value, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        var cappedFrom = Math.Min(value.Length, startIndex);
        var cappedEnd  = Math.Min(value.Length, startIndex + length);

        sb.Insert(atIndex, value[cappedFrom..cappedEnd]);
        return this;
    }

    public MutableString Insert(int atIndex, ReadOnlySpan<char> value)
    {
        if (IsFrozen) return ShouldThrow();
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, ReadOnlySpan<char> value, int startIndex, int length)
    {
        if (IsFrozen) return ShouldThrow();
        var cappedFrom = Math.Min(value.Length, startIndex);
        var cappedEnd  = Math.Min(value.Length, startIndex + length);

        sb.Insert(atIndex, value[cappedFrom..cappedEnd]);
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

    public MutableString Replace(ICharSequence find, ICharSequence replace) => Replace(find, replace, 0, Length);

    public MutableString Replace(ICharSequence find, ICharSequence replace, int startIndex, int length)
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

    public IStringBuilder Replace(StringBuilder find, StringBuilder replace) => Replace(find, replace, 0, Length);

    public IStringBuilder Replace(StringBuilder find, StringBuilder replace, int startIndex, int length)
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

    IStringBuilder IStringBuilder.Clone() => Clone();

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

    public IStringBuilder CopyFrom(string source)
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

    public bool Equals(string? toCompare)
    {
        if (toCompare == null) return false;
        var compareSpan = toCompare.AsSpan();
        return Equals(compareSpan);
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
            (Span<char> charSpan, int spanIndex, int myLength = int.MaxValue, int fromMyIndex = 0) =>
            ms.CopyTo(charSpan, spanIndex, myLength: myLength, fromMyIndex: fromMyIndex);

        public void CopyTo
            (RecyclingCharArray array, int? arrayIndex = null, int myLength = int.MaxValue, int fromMyIndex = 0) =>
            ms.CopyTo(array, arrayIndex, myLength: myLength, fromMyIndex: fromMyIndex);

        public void CopyTo
            (char[] array, int arrayIndex, int myLength = int.MaxValue, int fromMyIndex = 0) =>
            ms.CopyTo(array, arrayIndex, myLength: myLength, fromMyIndex: fromMyIndex);

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

        public bool Equals(string? toCompare) => ms.Equals(toCompare);

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
