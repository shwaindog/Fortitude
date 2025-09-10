// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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

    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomSpanFormattableProviders = new();

    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomStyledToStringFormattableProviders = new();

    protected static readonly string[] RawByteHex;
    protected static readonly string[] DotNetStringByteHex;

    protected static readonly char[] Base64LookupTable = [
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',  'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',  'Y', 'Z',
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',  'q', 'r', 's', 't', 'u', 'v', 'w', 'x',  'y', 'z',
    '0',  '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/' ];

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

    public string ItemSeparator { get; set; } = ", ";

    public bool IgnoreNullableValues { get; set; } = false;

    public bool EmptyCollectionWritesNull { get; set; } = false;

    public bool IgnoreEmptyCollection { get; set; } = false;
    
    public string NullString { get; set; } = DefaultNullString;

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
    public abstract int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ReadOnlySpan<char> source, IStringBuilder sb);
    public abstract int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(StringBuilder source, IStringBuilder sb);
    public abstract int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ICharSequence source, IStringBuilder sb);
    public abstract int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int maxTransferCount = Int32.MaxValue);
    public abstract int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = Int32.MaxValue);
    
    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue)
    {
        if (source.Length == 0) return 0;
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sourceFrom, sb, cappedLength);

        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
                ;
            }
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
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
            return charsAdded;
        }
        var charBuffer    = (alignedLength).SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source[sourceFrom..end], layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
        {
            charsAdded += Transfer(formatString[afterClosingBracket..], sb);
        }
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
            charsAdded = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
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
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
            return charsAdded;
        }
        var charBuffer      = (alignedLength).SourceRecyclingCharArray();
        var padBufferSpan   = charBuffer.RemainingAsSpan();
        var sourceFromRange = source[sourceFrom..end];
        var paBufferSize    = padBufferSpan.PadAndAlign(sourceFromRange, layout);
        charsAdded += Transfer(padBufferSpan[..paBufferSize], sb);
        charBuffer.DecrementRefCount();
        if (afterClosingBracket < formatString.Length)
        {
            charsAdded += Transfer(formatString[afterClosingBracket..], sb);
        }
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
            charsAdded = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length)
            {
                Transfer(formatString[afterClosingBracket..], sb);
            }
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
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
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
        if (afterClosingBracket < formatString.Length)
        {
            charsAdded += Transfer(formatString[afterClosingBracket..], sb);
        }
        return charsAdded;
    }

    public virtual int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString == NoFormatFormatString) return Transfer(source, sb);
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf(TokenOpen);
        if (indexOfOpenBracket > 0)
        {
            charsAdded = Transfer(formatString[..indexOfOpenBracket], sb);
            formatString = formatString[indexOfOpenBracket..];
        }
        formatString.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatString.IndexOf(TokenClose) + 1;
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sourceFrom, sb, cappedLength);
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
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
            if (afterClosingBracket < formatString.Length)
            {
                charsAdded += Transfer(formatString[afterClosingBracket..], sb);
            }
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
        if (afterClosingBracket < formatString.Length)
        {
            charsAdded += Transfer(formatString[afterClosingBracket..], sb);
        }
        return charsAdded;
    }

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
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
            charsAdded += Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
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
            charsAdded = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
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
            charsAdded = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
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
            charsAdded = Transfer(formatString[..indexOfOpenBracket], destCharSpan, destStartIndex);
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
                if (layout.Length == 0)
                {
                    return Transfer(charSpan[..charsWritten], destination, destStartIndex);
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
                return Transfer(padSpan[..padSize], destination, destStartIndex);
            }
        }
        catch (FormatException) { }
        if (source.TryFormat(charSpan, out charsWritten, "", null))
        {
            if (layout.Length == 0)
            {
                return Transfer(charSpan[..charsWritten], destination, destStartIndex);
            }
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
    
    public abstract int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(TFmt[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollectionStart(Type collectionType, IStringBuilder sb, bool hasItems);
    public abstract int FormatCollectionStart(Type collectionType, Span<char> destination, int destStartIndex, bool hasItems);
    public abstract int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString) where TFmt : ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString) where TFmt : ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb) where TFmt : ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex) where TFmt : ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex) where TFmt : struct, ISpanFormattable;
    public abstract int FormatCollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount);
    public abstract int FormatCollectionEnd(Type collectionType, Span<char> destination, int index, int totalItemCount);
}
