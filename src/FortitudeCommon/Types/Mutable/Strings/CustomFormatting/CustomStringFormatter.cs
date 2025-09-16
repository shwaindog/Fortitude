// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

public abstract class CustomStringFormatter : RecyclableObject, ICustomStringFormatter
{
    public const string DefaultNullString = "null";

    protected const string NoFormatFormatString = "{0}";

    protected const char TokenClose = '}';
    protected const char TokenOpen  = '{';

    protected const string DefaultTrueString  = "true";
    protected const string DefaultFalseString = "false";

    protected const string SqBrktOpn = "[";
    protected const char SqBrktOpnChar = '[';
    protected const string SqBrktCls = "]";
    protected const char SqBrktClsChar = ']';

    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomSpanFormattableProviders = new();

    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomStyledToStringFormattableProviders = new();

    protected static readonly string[] RawByteHex;
    protected static readonly string[] DotNetStringByteHex;

    protected static readonly char[] Base64LookupTable =
    [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b'
      , 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3'
      , '4', '5', '6', '7', '8', '9', '+', '/'
    ];

    static CustomStringFormatter()
    {
        RawByteHex          = new string[256];
        DotNetStringByteHex = new string[256];
        for (int i = 0; i < 256; i++)
        {
            RawByteHex[i]          = $"{i:X2}";
            DotNetStringByteHex[i] = $"\\0x{i:X2}";
        }
    }

    protected MutableString? CharSpanCollectionScratchBuffer { get; set; }

    public string ItemSeparator { get; set; } = ", ";

    public bool IgnoreNullableValues { get; set; } = false;

    public bool EmptyCollectionWritesNull { get; set; } = false;

    public bool IgnoreEmptyCollection { get; set; } = false;

    public string NullString { get; set; } = DefaultNullString;

    public string True { get; set; } = DefaultTrueString;
    public string False { get; set; } = DefaultFalseString;

    public virtual int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber) =>
        sb.Append(ItemSeparator).ReturnCharCount(ItemSeparator.Length);

    public virtual int AddCollectionElementSeparator(Type collectionElementType, Span<char> charSpan, int atIndex, int nextItemNumber) =>
        charSpan.OverWriteAt(atIndex, ItemSeparator);

    protected virtual bool TryGetCachedCustomSpanFormatter<T>([NotNullWhen(true)] out CustomSpanFormattable<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomSpanFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportSpanFormattable && formattableProvider is ICustomSpanFormattableProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomSpanFormattable;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryGetCachedCustomStyledToStringFormatter<T>([NotNullWhen(true)] out CustomTypeStyler<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomStyledToStringFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportStyleToString && formattableProvider is ICustomTypeStylerProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomTypeStyler;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryAddCustomSpanFormattableProvider(Type registeredType, ICustomFormattableProvider toRegister) =>
        GlobalCustomSpanFormattableProviders.TryAdd(registeredType, toRegister);

    protected virtual bool TryAddCustomStyledToStringProvider(Type registeredType, ICustomFormattableProvider toRegister) =>
        GlobalCustomStyledToStringFormattableProviders.TryAdd(registeredType, toRegister);

    public abstract int Transfer(char[] source, IStringBuilder sb);
    public abstract int Transfer(char[] source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(ReadOnlySpan<char> source, IStringBuilder sb);
    public abstract int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(StringBuilder source, IStringBuilder sb);
    public abstract int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(ICharSequence source, IStringBuilder sb);
    public abstract int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);

    public abstract int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue);

    public abstract int ProcessAppendedRange(IStringBuilder sb, int fromIndex);
    public abstract int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length);

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue)
    {
        if (source.Length == 0) return 0;
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sourceFrom, sb, cappedLength);

        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var alignedLength = source[..cappedLength].CalculatePaddedAlignedLength(layout);
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        var end = sourceFrom + cappedLength;
        if (alignedLength < 4096)
        {
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(source[sourceFrom..end], layout);
            charsAdded += Transfer(padSpan[..padSize], sb);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var charBuffer    = (alignedLength).SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source[sourceFrom..end], layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
        return charsAdded;
    }

    public virtual int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sourceFrom, sb, cappedLength);
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);
        var end           = sourceFrom + cappedLength;

        if (alignedLength < 4096)
        {
            var padSpan        = stackalloc char[alignedLength].ResetMemory();
            var sourceFromSpan = source[sourceFrom..end];
            var padSize        = padSpan.PadAndAlign(sourceFromSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], sb);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var charBuffer      = (alignedLength).SourceRecyclingCharArray();
        var padBufferSpan   = charBuffer.RemainingAsSpan();
        var sourceFromRange = source[sourceFrom..end];
        var paBufferSize    = padBufferSpan.PadAndAlign(sourceFromRange, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
        return charsAdded;
    }

    public virtual int Format(StringBuilder source, int sourceFrom, IStringBuilder sb
      , ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sb);
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length) { Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);

        if (alignedLength < 4096 && cappedLength < 4096)
        {
            var sourceInSpan = stackalloc char[cappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, cappedLength);
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], sb);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var sourceBuffer = cappedLength.SourceRecyclingCharArray();
        sourceBuffer.Add(source, sourceFrom, cappedLength);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
        return charsAdded;
    }

    public virtual int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sb);
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }

        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);

        if (alignedLength < 4096 && cappedLength < 4096)
        {
            var sourceInSpan = stackalloc char[cappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, cappedLength);
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], sb);
            if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
            return charsAdded;
        }
        var sourceBuffer = cappedLength.SourceRecyclingCharArray();
        sourceBuffer.Add(source, sourceFrom, cappedLength);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length) { charsAdded += Transfer(formatString[afterClosingBracket..], sb); }
        return charsAdded;
    }

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString
      , int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (source.Length == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, destCharSpan, destStartIndex);
        var charsAdded = 0;

        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   += Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
            formatString =  formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, destCharSpan, destStartIndex + charsAdded, cappedLength);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }

        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);

        var end = sourceFrom + cappedLength;
        if (alignedLength < 4096 && cappedLength < 4096)
        {
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(source[sourceFrom..end], layout);
            charsAdded += Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }
        var charBuffer    = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source[sourceFrom..end], layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], destCharSpan, destStartIndex + charsAdded);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
            charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
        return charsAdded;
    }

    public virtual int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, destCharSpan, destStartIndex);
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, destCharSpan, destStartIndex + charsAdded, cappedLength);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }

        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);
        var end           = sourceFrom + cappedLength;

        if (alignedLength < 4096)
        {
            var padSpan    = stackalloc char[alignedLength].ResetMemory();
            var sourceSpan = source[sourceFrom..end];
            var padSize    = padSpan.PadAndAlign(sourceSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }
        var charBuffer      = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan   = charBuffer.RemainingAsSpan();
        var sourceLargeSpan = source[sourceFrom..end];
        var paBufferSize    = padBufferSpan.PadAndAlign(sourceLargeSpan, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], destCharSpan, destStartIndex + charsAdded);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
            charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
        return charsAdded;
    }

    public virtual int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, destCharSpan, destStartIndex);
        var charsAdded = 0;

        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, destCharSpan, destStartIndex + charsAdded, cappedLength);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }

        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);

        if (alignedLength < 4096 && cappedLength < 4096)
        {
            var sourceInSpan = stackalloc char[cappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, cappedLength);
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }
        var sourceBuffer = cappedLength.SourceRecyclingCharArray();
        sourceBuffer.Add(source, sourceFrom, cappedLength);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], destCharSpan, destStartIndex + charsAdded);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
            charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
        return charsAdded;
    }

    public virtual int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, destCharSpan, destStartIndex);
        var charsAdded = 0;

        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded   = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, destCharSpan, destStartIndex + charsAdded, cappedLength);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }

        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout);

        if (alignedLength < 4096 && cappedLength < 4096)
        {
            var sourceInSpan = stackalloc char[cappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, cappedLength);
            var padSpan = stackalloc char[alignedLength].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatString.Length)
                charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
            return charsAdded;
        }
        var sourceBuffer = cappedLength.SourceRecyclingCharArray();
        sourceBuffer.Add(source, sourceFrom, cappedLength);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = alignedLength.SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], destCharSpan, destStartIndex + charsAdded);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
            charsAdded += Transfer(formatString[afterClosingBracket..], destCharSpan, destStartIndex + charsAdded);
        return charsAdded;
    }

    public virtual int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmt : ISpanFormattable
    {
        if (source == null) return IgnoreNullableValues ? 0 : sb.Append(NullString).ReturnCharCount(NullString.Length);
        int charsWritten;
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            var charSpan = stackalloc char[256].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            Transfer(charSpan[..charsWritten], sb);
            return charsWritten;
        }
        if (source is Enum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateStructEnumFormatProvider<TFmt>();
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            formatter = enumFormatProvider.CustomSpanFormattable;
            var charSpan = stackalloc char[1024].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            Transfer(charSpan[..charsWritten], sb);
            return charsWritten;
        }
        try
        {
            var charSpan = stackalloc char[1024].ResetMemory();
            formatString.ExtractStringFormatStages(out _, out var layout, out var formatting);
            if (source.TryFormat(charSpan, out charsWritten, formatting, null))
            {
                if (layout.Length == 0)
                {
                    Transfer(charSpan[..charsWritten], sb);
                    return charsWritten;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
                Transfer(padSpan[..padSize], sb);
                return padSize;
            }
        }
        catch (FormatException)
        {
            var sbPreAppendLen = sb.Length;
            sb.AppendFormat(formatString.Length > 0 ? formatString : NoFormatFormatString.AsSpan(), source.ToString()!);
            charsWritten = sb.Length - sbPreAppendLen;
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, ReadOnlySpan<char> formatString)
        where TFmt : ISpanFormattable
    {
        if (source == null) return IgnoreNullableValues ? 0 : destination.AppendReturnAddCount(NullString);
        int charsWritten;
        var charSpan = stackalloc char[1024].ResetMemory();
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            charsWritten = formatter(source, charSpan, formatString, null);
            return Transfer(charSpan[..charsWritten], destination, destStartIndex);
        }
        if (source is Enum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateStructEnumFormatProvider<TFmt>();
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            formatter    = enumFormatProvider.CustomSpanFormattable;
            charsWritten = formatter(source, charSpan, formatString, null);
            return Transfer(charSpan[..charsWritten], destination, destStartIndex);
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out var formatting);
        try
        {
            if (source.TryFormat(charSpan, out charsWritten, formatting, null))
            {
                if (layout.Length == 0) { return Transfer(charSpan[..charsWritten], destination, destStartIndex); }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
                return Transfer(padSpan[..padSize], destination, destStartIndex);
            }
        }
        catch (FormatException) { }
        if (source.TryFormat(charSpan, out charsWritten, "", null))
        {
            if (layout.Length == 0) { return Transfer(charSpan[..charsWritten], destination, destStartIndex); }
            var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
            return Transfer(padSpan[..padSize], destination, destStartIndex);
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmt : struct, ISpanFormattable
    {
        if (source == null) return IgnoreNullableValues ? 0 : sb.Append(NullString).ReturnCharCount(NullString.Length);
        return Format(source.Value, sb, formatString);
    }

    public virtual int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, ReadOnlySpan<char> formatString)
        where TFmt : struct, ISpanFormattable
    {
        if (source == null) return IgnoreNullableValues ? 0 : destination.OverWriteAt(destStartIndex, NullString);
        return Format(source.Value, destination, destStartIndex, formatString);
    }

    public int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString)
    {
        if (source == null) return 0;
        var type                     = source.GetType();
        var maybeIterableElementType = type.GetIterableElementType();
        if (maybeIterableElementType == null) return 0;
        if (!((maybeIterableElementType.IsSpanFormattable()) || (maybeIterableElementType.IsNullableSpanFormattable()))) return 0;
        if (type.IsArray) { return CheckIsKnownSpanFormattableArray(source, sb, formatString, type); }
        if (type.IsReadOnlyList()) { return CheckIsKnownSpanFormattableList(source, sb, formatString, type); }
        if (type.IsEnumerable()) { return CheckIsKnownSpanFormattableEnumerable(source, sb, formatString, type); }
        if (type.IsEnumerator()) { return CheckIsKnownSpanFormattableEnumerator(source, sb, formatString, type); }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerator<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type)
    {
        var enumeratorElementType = type.IfEnumeratorGetElementType()!;

        if (enumeratorElementType.IsSpanFormattable())
        {
            switch (source)
            {
                case IEnumerator<byte> byteArray:           return FormatEnumerator(byteArray, sb, formatString);
                case IEnumerator<sbyte> sbyteArray:         return FormatEnumerator(sbyteArray, sb, formatString);
                case IEnumerator<char> charArray:           return FormatEnumerator(charArray, sb, formatString);
                case IEnumerator<short> shortArray:         return FormatEnumerator(shortArray, sb, formatString);
                case IEnumerator<ushort> uShortArray:       return FormatEnumerator(uShortArray, sb, formatString);
                case IEnumerator<Half> halfFloatArray:      return FormatEnumerator(halfFloatArray, sb, formatString);
                case IEnumerator<int> intArray:             return FormatEnumerator(intArray, sb, formatString);
                case IEnumerator<uint> uIntArray:           return FormatEnumerator(uIntArray, sb, formatString);
                case IEnumerator<nint> nIntArray:           return FormatEnumerator(nIntArray, sb, formatString);
                case IEnumerator<float> floatArray:         return FormatEnumerator(floatArray, sb, formatString);
                case IEnumerator<long> longArray:           return FormatEnumerator(longArray, sb, formatString);
                case IEnumerator<ulong> uLongArray:         return FormatEnumerator(uLongArray, sb, formatString);
                case IEnumerator<double> doubleArray:       return FormatEnumerator(doubleArray, sb, formatString);
                case IEnumerator<decimal> decimalArray:     return FormatEnumerator(decimalArray, sb, formatString);
                case IEnumerator<Int128> xtralongArray:     return FormatEnumerator(xtralongArray, sb, formatString);
                case IEnumerator<UInt128> xtraUlongArray:   return FormatEnumerator(xtraUlongArray, sb, formatString);
                case IEnumerator<BigInteger> bigIntArray:   return FormatEnumerator(bigIntArray, sb, formatString);
                case IEnumerator<Complex> complexNumArray:  return FormatEnumerator(complexNumArray, sb, formatString);
                case IEnumerator<DateTime> dateTimeArray:   return FormatEnumerator(dateTimeArray, sb, formatString);
                case IEnumerator<DateOnly> dateOnlyArray:   return FormatEnumerator(dateOnlyArray, sb, formatString);
                case IEnumerator<TimeSpan> timeSpanArray:   return FormatEnumerator(timeSpanArray, sb, formatString);
                case IEnumerator<TimeOnly> timeOnlyArray:   return FormatEnumerator(timeOnlyArray, sb, formatString);
                case IEnumerator<Rune> runeArray:           return FormatEnumerator(runeArray, sb, formatString);
                case IEnumerator<Guid> guidArray:           return FormatEnumerator(guidArray, sb, formatString);
                case IEnumerator<IPNetwork> ipNetworkArray: return FormatEnumerator(ipNetworkArray, sb, formatString);
                case IEnumerator<Enum> enumArray:           return FormatEnumerator(enumArray, sb, formatString);
                case IEnumerator<Version> versionArray:     return FormatEnumerator(versionArray, sb, formatString);
                case IEnumerator<IPAddress> ipAddressArray: return FormatEnumerator(ipAddressArray, sb, formatString);
                case IEnumerator<Uri> uriArray:             return FormatEnumerator(uriArray, sb, formatString);
            }
        }
        else if (enumeratorElementType is { IsValueType: true, IsGenericType: true } &&
                 enumeratorElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerator<byte?> nullByteArray:           return FormatEnumerator(nullByteArray, sb, formatString);
                case IEnumerator<sbyte?> nullSbyteArray:         return FormatEnumerator(nullSbyteArray, sb, formatString);
                case IEnumerator<char?> nullCharArray:           return FormatEnumerator(nullCharArray, sb, formatString);
                case IEnumerator<short?> nullShortArray:         return FormatEnumerator(nullShortArray, sb, formatString);
                case IEnumerator<ushort?> nullUShortArray:       return FormatEnumerator(nullUShortArray, sb, formatString);
                case IEnumerator<Half?> nullHalfFloatArray:      return FormatEnumerator(nullHalfFloatArray, sb, formatString);
                case IEnumerator<int?> nullIntArray:             return FormatEnumerator(nullIntArray, sb, formatString);
                case IEnumerator<uint?> nullUIntArray:           return FormatEnumerator(nullUIntArray, sb, formatString);
                case IEnumerator<nint?> nullNIntArray:           return FormatEnumerator(nullNIntArray, sb, formatString);
                case IEnumerator<float?> nullFloatArray:         return FormatEnumerator(nullFloatArray, sb, formatString);
                case IEnumerator<long?> nullLongArray:           return FormatEnumerator(nullLongArray, sb, formatString);
                case IEnumerator<ulong?> nullULongArray:         return FormatEnumerator(nullULongArray, sb, formatString);
                case IEnumerator<double?> nullDoubleArray:       return FormatEnumerator(nullDoubleArray, sb, formatString);
                case IEnumerator<decimal?> nullDecimalArray:     return FormatEnumerator(nullDecimalArray, sb, formatString);
                case IEnumerator<Int128?> nullXtralongArray:     return FormatEnumerator(nullXtralongArray, sb, formatString);
                case IEnumerator<UInt128?> nullXtraUlongArray:   return FormatEnumerator(nullXtraUlongArray, sb, formatString);
                case IEnumerator<BigInteger?> nullBigIntArray:   return FormatEnumerator(nullBigIntArray, sb, formatString);
                case IEnumerator<Complex?> nullComplexNumArray:  return FormatEnumerator(nullComplexNumArray, sb, formatString);
                case IEnumerator<DateTime?> nullDateTimeArray:   return FormatEnumerator(nullDateTimeArray, sb, formatString);
                case IEnumerator<DateOnly?> nullDateOnlyArray:   return FormatEnumerator(nullDateOnlyArray, sb, formatString);
                case IEnumerator<TimeSpan?> nullTimeSpanArray:   return FormatEnumerator(nullTimeSpanArray, sb, formatString);
                case IEnumerator<TimeOnly?> nullTimeOnlyArray:   return FormatEnumerator(nullTimeOnlyArray, sb, formatString);
                case IEnumerator<Rune?> nullRuneArray:           return FormatEnumerator(nullRuneArray, sb, formatString);
                case IEnumerator<Guid?> nullGuidArray:           return FormatEnumerator(nullGuidArray, sb, formatString);
                case IEnumerator<IPNetwork?> nullIpNetworkArray: return FormatEnumerator(nullIpNetworkArray, sb, formatString);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerable<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type)
    {
        var enumerableElementType = type.IfEnumerableGetElementType()!;

        if (enumerableElementType.IsSpanFormattable())
        {
            switch (source)
            {
                case IEnumerable<byte> byteArray:           return FormatEnumerable(byteArray, sb, formatString);
                case IEnumerable<sbyte> sbyteArray:         return FormatEnumerable(sbyteArray, sb, formatString);
                case IEnumerable<char> charArray:           return FormatEnumerable(charArray, sb, formatString);
                case IEnumerable<short> shortArray:         return FormatEnumerable(shortArray, sb, formatString);
                case IEnumerable<ushort> uShortArray:       return FormatEnumerable(uShortArray, sb, formatString);
                case IEnumerable<Half> halfFloatArray:      return FormatEnumerable(halfFloatArray, sb, formatString);
                case IEnumerable<int> intArray:             return FormatEnumerable(intArray, sb, formatString);
                case IEnumerable<uint> uIntArray:           return FormatEnumerable(uIntArray, sb, formatString);
                case IEnumerable<nint> nIntArray:           return FormatEnumerable(nIntArray, sb, formatString);
                case IEnumerable<float> floatArray:         return FormatEnumerable(floatArray, sb, formatString);
                case IEnumerable<long> longArray:           return FormatEnumerable(longArray, sb, formatString);
                case IEnumerable<ulong> uLongArray:         return FormatEnumerable(uLongArray, sb, formatString);
                case IEnumerable<double> doubleArray:       return FormatEnumerable(doubleArray, sb, formatString);
                case IEnumerable<decimal> decimalArray:     return FormatEnumerable(decimalArray, sb, formatString);
                case IEnumerable<Int128> xtralongArray:     return FormatEnumerable(xtralongArray, sb, formatString);
                case IEnumerable<UInt128> xtraUlongArray:   return FormatEnumerable(xtraUlongArray, sb, formatString);
                case IEnumerable<BigInteger> bigIntArray:   return FormatEnumerable(bigIntArray, sb, formatString);
                case IEnumerable<Complex> complexNumArray:  return FormatEnumerable(complexNumArray, sb, formatString);
                case IEnumerable<DateTime> dateTimeArray:   return FormatEnumerable(dateTimeArray, sb, formatString);
                case IEnumerable<DateOnly> dateOnlyArray:   return FormatEnumerable(dateOnlyArray, sb, formatString);
                case IEnumerable<TimeSpan> timeSpanArray:   return FormatEnumerable(timeSpanArray, sb, formatString);
                case IEnumerable<TimeOnly> timeOnlyArray:   return FormatEnumerable(timeOnlyArray, sb, formatString);
                case IEnumerable<Rune> runeArray:           return FormatEnumerable(runeArray, sb, formatString);
                case IEnumerable<Guid> guidArray:           return FormatEnumerable(guidArray, sb, formatString);
                case IEnumerable<IPNetwork> ipNetworkArray: return FormatEnumerable(ipNetworkArray, sb, formatString);
                case IEnumerable<Enum> enumArray:           return FormatEnumerable(enumArray, sb, formatString);
                case IEnumerable<Version> versionArray:     return FormatEnumerable(versionArray, sb, formatString);
                case IEnumerable<IPAddress> ipAddressArray: return FormatEnumerable(ipAddressArray, sb, formatString);
                case IEnumerable<Uri> uriArray:             return FormatEnumerable(uriArray, sb, formatString);
            }
        }
        else if (enumerableElementType is { IsValueType: true, IsGenericType: true } &&
                 enumerableElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<byte?> nullByteArray:           return FormatEnumerable(nullByteArray, sb, formatString);
                case IEnumerable<sbyte?> nullSbyteArray:         return FormatEnumerable(nullSbyteArray, sb, formatString);
                case IEnumerable<char?> nullCharArray:           return FormatEnumerable(nullCharArray, sb, formatString);
                case IEnumerable<short?> nullShortArray:         return FormatEnumerable(nullShortArray, sb, formatString);
                case IEnumerable<ushort?> nullUShortArray:       return FormatEnumerable(nullUShortArray, sb, formatString);
                case IEnumerable<Half?> nullHalfFloatArray:      return FormatEnumerable(nullHalfFloatArray, sb, formatString);
                case IEnumerable<int?> nullIntArray:             return FormatEnumerable(nullIntArray, sb, formatString);
                case IEnumerable<uint?> nullUIntArray:           return FormatEnumerable(nullUIntArray, sb, formatString);
                case IEnumerable<nint?> nullNIntArray:           return FormatEnumerable(nullNIntArray, sb, formatString);
                case IEnumerable<float?> nullFloatArray:         return FormatEnumerable(nullFloatArray, sb, formatString);
                case IEnumerable<long?> nullLongArray:           return FormatEnumerable(nullLongArray, sb, formatString);
                case IEnumerable<ulong?> nullULongArray:         return FormatEnumerable(nullULongArray, sb, formatString);
                case IEnumerable<double?> nullDoubleArray:       return FormatEnumerable(nullDoubleArray, sb, formatString);
                case IEnumerable<decimal?> nullDecimalArray:     return FormatEnumerable(nullDecimalArray, sb, formatString);
                case IEnumerable<Int128?> nullXtralongArray:     return FormatEnumerable(nullXtralongArray, sb, formatString);
                case IEnumerable<UInt128?> nullXtraUlongArray:   return FormatEnumerable(nullXtraUlongArray, sb, formatString);
                case IEnumerable<BigInteger?> nullBigIntArray:   return FormatEnumerable(nullBigIntArray, sb, formatString);
                case IEnumerable<Complex?> nullComplexNumArray:  return FormatEnumerable(nullComplexNumArray, sb, formatString);
                case IEnumerable<DateTime?> nullDateTimeArray:   return FormatEnumerable(nullDateTimeArray, sb, formatString);
                case IEnumerable<DateOnly?> nullDateOnlyArray:   return FormatEnumerable(nullDateOnlyArray, sb, formatString);
                case IEnumerable<TimeSpan?> nullTimeSpanArray:   return FormatEnumerable(nullTimeSpanArray, sb, formatString);
                case IEnumerable<TimeOnly?> nullTimeOnlyArray:   return FormatEnumerable(nullTimeOnlyArray, sb, formatString);
                case IEnumerable<Rune?> nullRuneArray:           return FormatEnumerable(nullRuneArray, sb, formatString);
                case IEnumerable<Guid?> nullGuidArray:           return FormatEnumerable(nullGuidArray, sb, formatString);
                case IEnumerable<IPNetwork?> nullIpNetworkArray: return FormatEnumerable(nullIpNetworkArray, sb, formatString);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableList<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type)
    {
        var listType = type.GetListElementType()!;

        if (listType.IsSpanFormattable())
        {
            switch (source)
            {
                case IReadOnlyList<byte> byteArray:           return FormatList(byteArray, sb, formatString);
                case IReadOnlyList<sbyte> sbyteArray:         return FormatList(sbyteArray, sb, formatString);
                case IReadOnlyList<char> charArray:           return FormatList(charArray, sb, formatString);
                case IReadOnlyList<short> shortArray:         return FormatList(shortArray, sb, formatString);
                case IReadOnlyList<ushort> uShortArray:       return FormatList(uShortArray, sb, formatString);
                case IReadOnlyList<Half> halfFloatArray:      return FormatList(halfFloatArray, sb, formatString);
                case IReadOnlyList<int> intArray:             return FormatList(intArray, sb, formatString);
                case IReadOnlyList<uint> uIntArray:           return FormatList(uIntArray, sb, formatString);
                case IReadOnlyList<nint> nIntArray:           return FormatList(nIntArray, sb, formatString);
                case IReadOnlyList<float> floatArray:         return FormatList(floatArray, sb, formatString);
                case IReadOnlyList<long> longArray:           return FormatList(longArray, sb, formatString);
                case IReadOnlyList<ulong> uLongArray:         return FormatList(uLongArray, sb, formatString);
                case IReadOnlyList<double> doubleArray:       return FormatList(doubleArray, sb, formatString);
                case IReadOnlyList<decimal> decimalArray:     return FormatList(decimalArray, sb, formatString);
                case IReadOnlyList<Int128> xtralongArray:     return FormatList(xtralongArray, sb, formatString);
                case IReadOnlyList<UInt128> xtraUlongArray:   return FormatList(xtraUlongArray, sb, formatString);
                case IReadOnlyList<BigInteger> bigIntArray:   return FormatList(bigIntArray, sb, formatString);
                case IReadOnlyList<Complex> complexNumArray:  return FormatList(complexNumArray, sb, formatString);
                case IReadOnlyList<DateTime> dateTimeArray:   return FormatList(dateTimeArray, sb, formatString);
                case IReadOnlyList<DateOnly> dateOnlyArray:   return FormatList(dateOnlyArray, sb, formatString);
                case IReadOnlyList<TimeSpan> timeSpanArray:   return FormatList(timeSpanArray, sb, formatString);
                case IReadOnlyList<TimeOnly> timeOnlyArray:   return FormatList(timeOnlyArray, sb, formatString);
                case IReadOnlyList<Rune> runeArray:           return FormatList(runeArray, sb, formatString);
                case IReadOnlyList<Guid> guidArray:           return FormatList(guidArray, sb, formatString);
                case IReadOnlyList<IPNetwork> ipNetworkArray: return FormatList(ipNetworkArray, sb, formatString);
                case IReadOnlyList<Enum> enumArray:           return FormatList(enumArray, sb, formatString);
                case IReadOnlyList<Version> versionArray:     return FormatList(versionArray, sb, formatString);
                case IReadOnlyList<IPAddress> ipAddressArray: return FormatList(ipAddressArray, sb, formatString);
                case IReadOnlyList<Uri> uriArray:             return FormatList(uriArray, sb, formatString);
            }
        }
        else if (listType is { IsValueType: true, IsGenericType: true } && listType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IReadOnlyList<byte?> nullByteArray:           return FormatList(nullByteArray, sb, formatString);
                case IReadOnlyList<sbyte?> nullSbyteArray:         return FormatList(nullSbyteArray, sb, formatString);
                case IReadOnlyList<char?> nullCharArray:           return FormatList(nullCharArray, sb, formatString);
                case IReadOnlyList<short?> nullShortArray:         return FormatList(nullShortArray, sb, formatString);
                case IReadOnlyList<ushort?> nullUShortArray:       return FormatList(nullUShortArray, sb, formatString);
                case IReadOnlyList<Half?> nullHalfFloatArray:      return FormatList(nullHalfFloatArray, sb, formatString);
                case IReadOnlyList<int?> nullIntArray:             return FormatList(nullIntArray, sb, formatString);
                case IReadOnlyList<uint?> nullUIntArray:           return FormatList(nullUIntArray, sb, formatString);
                case IReadOnlyList<nint?> nullNIntArray:           return FormatList(nullNIntArray, sb, formatString);
                case IReadOnlyList<float?> nullFloatArray:         return FormatList(nullFloatArray, sb, formatString);
                case IReadOnlyList<long?> nullLongArray:           return FormatList(nullLongArray, sb, formatString);
                case IReadOnlyList<ulong?> nullULongArray:         return FormatList(nullULongArray, sb, formatString);
                case IReadOnlyList<double?> nullDoubleArray:       return FormatList(nullDoubleArray, sb, formatString);
                case IReadOnlyList<decimal?> nullDecimalArray:     return FormatList(nullDecimalArray, sb, formatString);
                case IReadOnlyList<Int128?> nullXtralongArray:     return FormatList(nullXtralongArray, sb, formatString);
                case IReadOnlyList<UInt128?> nullXtraUlongArray:   return FormatList(nullXtraUlongArray, sb, formatString);
                case IReadOnlyList<BigInteger?> nullBigIntArray:   return FormatList(nullBigIntArray, sb, formatString);
                case IReadOnlyList<Complex?> nullComplexNumArray:  return FormatList(nullComplexNumArray, sb, formatString);
                case IReadOnlyList<DateTime?> nullDateTimeArray:   return FormatList(nullDateTimeArray, sb, formatString);
                case IReadOnlyList<DateOnly?> nullDateOnlyArray:   return FormatList(nullDateOnlyArray, sb, formatString);
                case IReadOnlyList<TimeSpan?> nullTimeSpanArray:   return FormatList(nullTimeSpanArray, sb, formatString);
                case IReadOnlyList<TimeOnly?> nullTimeOnlyArray:   return FormatList(nullTimeOnlyArray, sb, formatString);
                case IReadOnlyList<Rune?> nullRuneArray:           return FormatList(nullRuneArray, sb, formatString);
                case IReadOnlyList<Guid?> nullGuidArray:           return FormatList(nullGuidArray, sb, formatString);
                case IReadOnlyList<IPNetwork?> nullIpNetworkArray: return FormatList(nullIpNetworkArray, sb, formatString);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableArray<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type)
    {
        var elementType = type.GetElementType()!;
        if (elementType.IsSpanFormattable())
        {
            switch (source)
            {
                case byte[] byteArray:           return FormatArray(byteArray, sb, formatString);
                case sbyte[] sbyteArray:         return FormatArray(sbyteArray, sb, formatString);
                case char[] charArray:           return FormatArray(charArray, sb, formatString);
                case short[] shortArray:         return FormatArray(shortArray, sb, formatString);
                case ushort[] uShortArray:       return FormatArray(uShortArray, sb, formatString);
                case Half[] halfFloatArray:      return FormatArray(halfFloatArray, sb, formatString);
                case int[] intArray:             return FormatArray(intArray, sb, formatString);
                case uint[] uIntArray:           return FormatArray(uIntArray, sb, formatString);
                case nint[] nIntArray:           return FormatArray(nIntArray, sb, formatString);
                case float[] floatArray:         return FormatArray(floatArray, sb, formatString);
                case long[] longArray:           return FormatArray(longArray, sb, formatString);
                case ulong[] uLongArray:         return FormatArray(uLongArray, sb, formatString);
                case double[] doubleArray:       return FormatArray(doubleArray, sb, formatString);
                case decimal[] decimalArray:     return FormatArray(decimalArray, sb, formatString);
                case Int128[] xtralongArray:     return FormatArray(xtralongArray, sb, formatString);
                case UInt128[] xtraUlongArray:   return FormatArray(xtraUlongArray, sb, formatString);
                case BigInteger[] bigIntArray:   return FormatArray(bigIntArray, sb, formatString);
                case Complex[] complexNumArray:  return FormatArray(complexNumArray, sb, formatString);
                case DateTime[] dateTimeArray:   return FormatArray(dateTimeArray, sb, formatString);
                case DateOnly[] dateOnlyArray:   return FormatArray(dateOnlyArray, sb, formatString);
                case TimeSpan[] timeSpanArray:   return FormatArray(timeSpanArray, sb, formatString);
                case TimeOnly[] timeOnlyArray:   return FormatArray(timeOnlyArray, sb, formatString);
                case Rune[] runeArray:           return FormatArray(runeArray, sb, formatString);
                case Guid[] guidArray:           return FormatArray(guidArray, sb, formatString);
                case IPNetwork[] ipNetworkArray: return FormatArray(ipNetworkArray, sb, formatString);
                case Enum[] enumArray:           return FormatArray(enumArray, sb, formatString);
                case Version[] versionArray:     return FormatArray(versionArray, sb, formatString);
                case IPAddress[] ipAddressArray: return FormatArray(ipAddressArray, sb, formatString);
                case Uri[] uriArray:             return FormatArray(uriArray, sb, formatString);
            }
        }
        else if (elementType is { IsValueType: true, IsGenericType: true } && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case byte?[] nullByteArray:           return FormatArray(nullByteArray, sb, formatString);
                case sbyte?[] nullSbyteArray:         return FormatArray(nullSbyteArray, sb, formatString);
                case char?[] nullCharArray:           return FormatArray(nullCharArray, sb, formatString);
                case short?[] nullShortArray:         return FormatArray(nullShortArray, sb, formatString);
                case ushort?[] nullUShortArray:       return FormatArray(nullUShortArray, sb, formatString);
                case Half?[] nullHalfFloatArray:      return FormatArray(nullHalfFloatArray, sb, formatString);
                case int?[] nullIntArray:             return FormatArray(nullIntArray, sb, formatString);
                case uint?[] nullUIntArray:           return FormatArray(nullUIntArray, sb, formatString);
                case nint?[] nullNIntArray:           return FormatArray(nullNIntArray, sb, formatString);
                case float?[] nullFloatArray:         return FormatArray(nullFloatArray, sb, formatString);
                case long?[] nullLongArray:           return FormatArray(nullLongArray, sb, formatString);
                case ulong?[] nullULongArray:         return FormatArray(nullULongArray, sb, formatString);
                case double?[] nullDoubleArray:       return FormatArray(nullDoubleArray, sb, formatString);
                case decimal?[] nullDecimalArray:     return FormatArray(nullDecimalArray, sb, formatString);
                case Int128?[] nullXtralongArray:     return FormatArray(nullXtralongArray, sb, formatString);
                case UInt128?[] nullXtraUlongArray:   return FormatArray(nullXtraUlongArray, sb, formatString);
                case BigInteger?[] nullBigIntArray:   return FormatArray(nullBigIntArray, sb, formatString);
                case Complex?[] nullComplexNumArray:  return FormatArray(nullComplexNumArray, sb, formatString);
                case DateTime?[] nullDateTimeArray:   return FormatArray(nullDateTimeArray, sb, formatString);
                case DateOnly?[] nullDateOnlyArray:   return FormatArray(nullDateOnlyArray, sb, formatString);
                case TimeSpan?[] nullTimeSpanArray:   return FormatArray(nullTimeSpanArray, sb, formatString);
                case TimeOnly?[] nullTimeOnlyArray:   return FormatArray(nullTimeOnlyArray, sb, formatString);
                case Rune?[] nullRuneArray:           return FormatArray(nullRuneArray, sb, formatString);
                case Guid?[] nullGuidArray:           return FormatArray(nullGuidArray, sb, formatString);
                case IPNetwork?[] nullIpNetworkArray: return FormatArray(nullIpNetworkArray, sb, formatString);
            }
        }
        return 0;
    }

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    public abstract int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems);
    public abstract int CollectionStart(Type collectionType, Span<char> destination, int destStartIndex, bool hasItems);

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString) where TFmt : ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : struct, ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString) where TFmt : struct, ISpanFormattable;

    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb);
    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destination, int destStartIndex);
    public abstract int CollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount);
    public abstract int CollectionEnd(Type collectionType, Span<char> destination, int index, int totalItemCount);
}
