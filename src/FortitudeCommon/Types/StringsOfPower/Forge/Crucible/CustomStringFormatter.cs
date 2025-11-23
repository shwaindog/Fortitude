// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public abstract class CustomStringFormatter : RecyclableObject, ICustomStringFormatter
{
    protected const string DblQt         = "\"";
    protected const char   DblQtChar     = '"';
    protected const string SglQt         = "'";
    protected const char   SglQtChar     = '\'';
    protected const string SqBrktOpn     = "[";
    protected const char   SqBrktOpnChar = '[';
    protected const string SqBrktCls     = "]";
    protected const char   SqBrktClsChar = ']';

    public const string NoFormatFormatString = "{0}";

    protected static readonly ConcurrentDictionary<Type, IStringBearerFormattableProvider> GlobalCustomSpanFormattableProviders = new();

    protected static readonly ConcurrentDictionary<Type, IStringBearerFormattableProvider> GlobalCustomStyledToStringFormattableProviders = new();

    protected static readonly char[] Base64LookupTable =
    [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b'
      , 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3'
      , '4', '5', '6', '7', '8', '9', '+', '/'
    ];
    protected IFormattingOptions? FormatOptions;
    protected IEncodingTransfer?  EncoderTransfer;


    static CustomStringFormatter() { }

    protected MutableString? CharSpanCollectionScratchBuffer { get; set; }

    protected int LastFormatExceptionBuilderLength { get; set; }

    public virtual IFormattingOptions Options
    {
        get => FormatOptions ??= new FormattingOptions.FormattingOptions();
        set { FormatOptions = value; }
    }

    public virtual IEncodingTransfer StringEncoder
    {
        get => EncoderTransfer ??= PassThroughEncodingTransfer.Instance;
        set => EncoderTransfer = value;
    }

    public virtual FormattingHandlingFlags ResolveStringFormattingFlags<T>(char lastNonWhiteSpace, T input
      , FormattingHandlingFlags callerFormattingFlags, string formatString = "") =>
        callerFormattingFlags;

    public virtual int AppendDelimiterStart(Type maybeDelimited, IStringBuilder sb)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray())
            {
                sb.Append(SqBrktOpn);
                return 1;
            }
            return StringValueDelimiter(sb);
        }
        return 0;
    }

    public virtual int AppendDelimiterEnd(Type maybeDelimited, IStringBuilder sb)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray())
            {
                sb.Append(SqBrktCls);
                return 1;
            }
            return StringValueDelimiter(sb);
        }
        return 0;
    }

    public virtual int AppendDelimiterStart(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray()) { return destSpan.OverWriteAt(fromIndex, SqBrktOpn); }
            return StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }

    public virtual int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray()) { return destSpan.OverWriteAt(fromIndex, SqBrktCls); }
            return StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }

    public int StringValueDelimiter(IStringBuilder sb) => sb.Append(DblQt).ReturnCharCount(1);

    public int StringValueDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public int StringFieldDelimiter(IStringBuilder sb) => sb.Append(DblQt).ReturnCharCount(1);

    public int StringFieldDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);


    public virtual int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return 0;
        var preAppendLen = sb.Length;
        sb.Append(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return sb.Length - preAppendLen;
    }

    public virtual int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return 0;
        var preAppendLen = sb.Length;
        sb.Append(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return sb.Length - preAppendLen;
    }

    public virtual int AddCollectionElementSeparatorAndPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        AddCollectionElementSeparator(collectionType, sb, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(collectionType, sb, nextItemNumber, formatFlags);
    }

    public virtual int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return 0;
        var charsAdded = 0;
        charsAdded += destSpan.OverWriteAt(atIndex, formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return charsAdded;
    }

    public virtual int AddCollectionElementPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return 0;
        var charsAdded = 0;
        charsAdded += destSpan.OverWriteAt(atSpanOffset, formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return charsAdded;
    }

    public virtual int AddCollectionElementSeparatorAndPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var charsAdded = AddCollectionElementSeparator(collectionType, destSpan, atSpanOffset, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(collectionType, destSpan, atSpanOffset + charsAdded, nextItemNumber, formatFlags) + charsAdded;
    }

    protected virtual bool TryGetCachedCustomSpanFormatter<T>([NotNullWhen(true)] out StringBearerSpanFormattable<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomSpanFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportSpanFormattable && formattableProvider is IStringBearerSpanFormattableProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.StringBearerSpanFormattable;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryGetCachedCustomStyledToStringFormatter<TCloaked>
        ([NotNullWhen(true)] out PalantírReveal<TCloaked>? maybeFormatter) where TCloaked : notnull
    {
        maybeFormatter = null;
        if (GlobalCustomStyledToStringFormattableProviders.TryGetValue(typeof(TCloaked), out var formattableProvider))
        {
            if (formattableProvider.SupportStyleToString && formattableProvider is IStringBearerRevelStateProvider<TCloaked> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.EnumPalantír;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryAddCustomSpanFormattableProvider(Type registeredType, IStringBearerFormattableProvider toRegister) =>
        GlobalCustomSpanFormattableProviders.TryAdd(registeredType, toRegister);

    protected virtual bool TryAddCustomStyledToStringProvider(Type registeredType, IStringBearerFormattableProvider toRegister) =>
        GlobalCustomStyledToStringFormattableProviders.TryAdd(registeredType, toRegister);

    public abstract int ProcessAppendedRange(IStringBuilder sb, int fromIndex);
    public abstract int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length);

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        source = source[sourceFrom..(sourceFrom + cappedLength)];

        extendLengthRange = extendLengthRange.BoundRangeToLength(cappedLength);
        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source[extendLengthRange], 0, sb, maxTransferCount: cappedLength);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = source.CalculatePaddedAlignedFormatStringLength(layout) + 256;
        if (alignedLength < 4096)
        {
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, source);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { source = source[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, source);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { source = source[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        return Format(((ReadOnlySpan<char>)source)[sourceFrom..], 0, sb, formatString, maxTransferCount, formatFlags);
    }

    public virtual int Format(StringBuilder source, int sourceFrom, IStringBuilder sb
      , ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, sb, sb.Length, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);

        var rawSourceTo     = Math.Clamp(sourceFrom + cappedLength, 0, source.Length);
        var rawCappedLength = Math.Min(cappedLength, rawSourceTo - sourceFrom);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(source.Length - sourceFrom);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                sourceFrom = start.IsFromEnd
                    ? Math.Max(sourceFrom, source.Length - start.Value)
                    : Math.Min(source.Length, sourceFrom + start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = end.IsFromEnd
                    ? Math.Max(sourceFrom, rawSourceTo - end.Value)
                    : Math.Min(source.Length, sourceFrom + end.Value);
            }
            rawCappedLength = Math.Min(cappedLength, rawSourceTo - sourceFrom);
        }

        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = rawCappedLength.CalculatePaddedAlignedLength(layout) + 256;
        if (alignedLength < 4096)
        {
            var sourceInSpan = stackalloc char[rawCappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, rawCappedLength);
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, sourceInSpan);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var sourceBuffer = rawCappedLength.SourceRecyclingCharArray();
            sourceBuffer.Add(source, sourceFrom, rawCappedLength);
            var sourceInSpan   = sourceBuffer.WrittenAsSpan();
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, sourceInSpan);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);

        var rawSourceFrom   = sourceFrom;
        var rawSourceTo     = Math.Clamp(rawSourceFrom + cappedLength, 0, source.Length);
        var rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(rawCappedLength);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                rawSourceFrom = start.IsFromEnd
                    ? Math.Max(rawSourceFrom, source.Length - start.Value)
                    : Math.Min(source.Length, rawSourceFrom + start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = end.IsFromEnd
                    ? Math.Max(rawSourceFrom, rawSourceTo - end.Value)
                    : Math.Min(source.Length, rawSourceFrom + end.Value);
            }
            rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom);
        }

        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source, rawSourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = rawCappedLength.CalculatePaddedAlignedLength(layout) + 256;
        if (alignedLength < 4096)
        {
            Span<char> sourceInSpan = stackalloc char[rawCappedLength];
            sourceInSpan.Append(source, rawSourceFrom, rawCappedLength);
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, sourceInSpan);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                padSize             = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize             = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var sourceBuffer = alignedLength.SourceRecyclingCharArray();
            sourceBuffer.Add(source, rawSourceFrom, rawCappedLength);
            var sourceInSpan   = sourceBuffer.WrittenAsSpan();
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, sourceInSpan);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);

        var rawSourceFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var rawSourceTo     = Math.Clamp(rawSourceFrom + cappedLength, 0, source.Length);
        var rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(source.Length - rawSourceFrom);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                rawSourceFrom = start.IsFromEnd
                    ? Math.Max(rawSourceFrom, source.Length - start.Value)
                    : Math.Min(source.Length, rawSourceFrom + start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = end.IsFromEnd
                    ? Math.Max(rawSourceFrom, rawSourceTo - end.Value)
                    : Math.Min(source.Length, rawSourceFrom + end.Value);
            }
            rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom);
        }
        source = source[rawSourceFrom..(rawSourceFrom + rawCappedLength)];

        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source, 0, destCharSpan, destStartIndex + charsAdded
                                               , rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = rawCappedLength.CalculatePaddedAlignedLength(layout) + 256;
        if (alignedLength < 4096)
        {
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, source);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                padSize             = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize             = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, source);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0 && formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString)) return 0;
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        return Format(((ReadOnlySpan<char>)source)[sourceFrom..], 0, destCharSpan, formatString, destStartIndex
                    , cappedLength, formatFlags);
    }

    public virtual int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, destCharSpan, destStartIndex);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);

        var rawSourceFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var rawSourceTo     = Math.Clamp(rawSourceFrom + cappedLength, 0, source.Length);
        var rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom - prefix.Length - suffix.Length);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(source.Length - rawSourceFrom);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                rawSourceFrom = start.IsFromEnd
                    ? Math.Max(rawSourceFrom, source.Length - start.Value)
                    : Math.Min(source.Length, rawSourceFrom + start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = start.IsFromEnd
                    ? Math.Max(rawSourceFrom, source.Length - end.Value)
                    : Math.Min(source.Length, rawSourceFrom + end.Value);
            }
            rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom - prefix.Length - suffix.Length);
        }

        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = cappedLength.CalculatePaddedAlignedLength(layout) + 256;
        if (alignedLength < 4096)
        {
            var sourceInSpan = stackalloc char[cappedLength].ResetMemory();
            sourceInSpan.Append(source, sourceFrom, cappedLength);
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, sourceInSpan);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var sourceBuffer = cappedLength.SourceRecyclingCharArray();
            sourceBuffer.Add(source, sourceFrom, cappedLength);
            var sourceInSpan   = sourceBuffer.WrittenAsSpan();
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, sourceInSpan);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return StringEncoder.Transfer(this, source, destCharSpan, destStartIndex, maxTransferCount);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);

        var rawSourceFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var rawSourceTo     = Math.Clamp(rawSourceFrom + cappedLength, 0, source.Length);
        var rawCappedLength = Math.Min(cappedLength, rawSourceTo - rawSourceFrom);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(source.Length - rawSourceFrom);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                rawSourceFrom = start.IsFromEnd
                    ? Math.Max(rawSourceFrom, source.Length - start.Value)
                    : Math.Min(source.Length, rawSourceFrom + start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = end.IsFromEnd
                    ? Math.Max(rawSourceFrom, rawSourceTo - end.Value)
                    : Math.Min(rawSourceTo, rawSourceFrom + end.Value);
            }
            rawCappedLength = Math.Clamp(rawSourceTo - rawSourceFrom, 0, rawCappedLength);
        }

        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += StringEncoder.Transfer(this, source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = rawCappedLength.CalculatePaddedAlignedLength(layout) + 256;
        if (alignedLength < 4096)
        {
            Span<char> sourceInSpan = stackalloc char[rawCappedLength];
            sourceInSpan.Append(source, rawSourceFrom, rawCappedLength);
            Span<char> padSpan = stackalloc char[alignedLength];
            int        padSize;
            if (!splitJoinRange.IsNoSplitJoin)
            {
                Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

                var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, sourceInSpan);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                padSize             = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize             = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        else
        {
            var sourceBuffer = alignedLength.SourceRecyclingCharArray();
            sourceBuffer.Add(source, rawSourceFrom, rawCappedLength);
            var sourceInSpan   = sourceBuffer.WrittenAsSpan();
            var padAlignBuffer = (alignedLength).SourceRecyclingCharArray();
            var padSpan        = padAlignBuffer.RemainingAsSpan();
            int padSize;

            if (!splitJoinRange.IsNoSplitJoin)
            {
                var splitJoinBuffer = (alignedLength + 256).SourceRecyclingCharArray();
                var splitJoinResult = splitJoinBuffer.RemainingAsSpan();

                var size = splitJoinRange.ApplySplitJoin(splitJoinResult, sourceInSpan);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format<TFmt>(TFmt source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        int charsWritten;
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            var charSpan = stackalloc char[2048].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            StringEncoder.Transfer(this, charSpan[..charsWritten], sb);
            return charsWritten;
        }
        if (source is Enum sourceEnum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateEnumFormatProvider(source.GetType());
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            var charSpan             = stackalloc char[2048].ResetMemory();
            var spanEnumFormatProver = enumFormatProvider.AsSpanFormattableEnumFormatProvider<TFmt>();
            if (spanEnumFormatProver != null)
            {
                charsWritten = spanEnumFormatProver.StringBearerSpanFormattable(source, charSpan, formatString, null);
            }
            else
            {
                var asEnumFormatter = enumFormatProvider.AsEnumFormatProvider()!;
                charsWritten = asEnumFormatter.StringBearerSpanFormattable(sourceEnum, charSpan, formatString, null);
            }
            StringEncoder.Transfer(this, charSpan[..charsWritten], sb);
            return charsWritten;
        }
        try
        {
            var charSpan   = stackalloc char[2048].ResetMemory();
            var charsAdded = 0;
            formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var outputSubRange
                                                         , out var layout, out _, out var format, out var suffix);
            if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
            if (source.TryFormat(charSpan, out charsWritten, format, null))
            {
                var toTransfer = charSpan[..charsWritten];
                if (!outputSubRange.IsAllRange())
                {
                    outputSubRange = outputSubRange.BoundRangeToLength(toTransfer.Length);
                    outputSubRange.BoundRangeToLength(toTransfer.Length);
                    toTransfer = toTransfer[outputSubRange];
                }
                if (layout.Length == 0)
                {
                    StringEncoder.Transfer(this, toTransfer, sb);
                    if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
                    return charsWritten + charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                StringEncoder.Transfer(this, padSpan[..padSize], sb);
                if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
                return padSize + charsAdded;
            }
        }
        catch (FormatException)
        {
            var sbPreAppendLen = sb.Length;
            if (LastFormatExceptionBuilderLength != sbPreAppendLen)
            {
                LastFormatExceptionBuilderLength = sbPreAppendLen;
                try { sb.AppendFormat(formatString.Length > 0 ? formatString : NoFormatFormatString.AsSpan(), source); }
                catch (FormatException) { }
                finally
                {
                    if (sbPreAppendLen == sb.Length) { sb.Append(source); }
                    else { LastFormatExceptionBuilderLength = -1; }
                }
            }
            charsWritten = sb.Length - sbPreAppendLen;
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable?
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString);
        int charsWritten;
        var charSpan = stackalloc char[2048].ResetMemory();
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            charsWritten = formatter(source, charSpan, formatString, null);
            return StringEncoder.Transfer(this, charSpan[..charsWritten], destCharSpan, destStartIndex);
        }
        if (source is Enum sourceEnum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateEnumFormatProvider(source.GetType());
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            var spanEnumFormatProver = enumFormatProvider.AsSpanFormattableEnumFormatProvider<TFmt>();
            if (spanEnumFormatProver != null)
            {
                charsWritten = spanEnumFormatProver.StringBearerSpanFormattable(source, charSpan, formatString, null);
            }
            else
            {
                var asEnumFormatter = enumFormatProvider.AsEnumFormatProvider()!;
                charsWritten = asEnumFormatter.StringBearerSpanFormattable(sourceEnum, charSpan, formatString, null);
            }
            return StringEncoder.Transfer(this, charSpan[..charsWritten], destCharSpan, destStartIndex);
        }
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var outputSubRange
                                                     , out var layout, out _, out var format, out var suffix);
        try
        {
            var charsAdded = 0;
            if (prefix.Length > 0)
                charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
            if (source.TryFormat(charSpan, out charsWritten, format, null))
            {
                var toTransfer = charSpan[..charsWritten];
                if (!outputSubRange.IsAllRange())
                {
                    outputSubRange = outputSubRange.BoundRangeToLength(toTransfer.Length);
                    outputSubRange.BoundRangeToLength(toTransfer.Length);
                    toTransfer = toTransfer[outputSubRange];
                }
                if (layout.Length == 0)
                {
                    StringEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex + charsAdded);
                    charsAdded += charsWritten;
                    if (suffix.Length > 0)
                        charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                                                 , formatFlags.HasEncodeBoundsFlag());
                    return charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
                charsAdded += padSize;
                if (suffix.Length > 0)
                    charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
                return charsAdded;
            }
        }
        catch (FormatException) { }
        if (source.TryFormat(charSpan, out charsWritten, "", null))
        {
            var sbPreAppendLen = destStartIndex + charsWritten;
            if (LastFormatExceptionBuilderLength != sbPreAppendLen)
            {
                LastFormatExceptionBuilderLength = sbPreAppendLen;
                try
                {
                    var toTransfer = charSpan[..charsWritten];
                    if (!outputSubRange.IsAllRange())
                    {
                        outputSubRange.BoundRangeToLength(toTransfer.Length);
                        toTransfer = toTransfer[outputSubRange];
                    }
                    if (layout.Length == 0) { return StringEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex); }
                    var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                    var padSize = padSpan.PadAndAlign(toTransfer, layout);
                    charsWritten = StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
                }
                catch (FormatException) { }
            }
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : struct, ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return Format(source.Value, sb, formatString);
    }

    public virtual int Format<TFmt>(TFmt? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : struct, ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return Format(source.Value, destCharSpan, destStartIndex, formatString);
    }

    public int Format(bool source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var charSpan   = stackalloc char[16].ResetMemory();
        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var outputSubRange
           , out var layout, out _, out _, out var suffix);

        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        var charsWritten                  = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);
        var toTransfer                    = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            StringEncoder.Transfer(this, toTransfer, sb);
            if (suffix.Length > 0) charsAdded += sb.Append(suffix).ReturnCharCount(suffix.Length);
            return charsWritten + charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        StringEncoder.Transfer(this, padSpan[..padSize], sb);
        if (suffix.Length > 0) charsAdded += StringEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
        return padSize + charsAdded;
    }

    public int Format(bool source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var charSpan = stackalloc char[16].ResetMemory();
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var outputSubRange
           , out var layout, out _, out _, out var suffix);
        var charsAdded                    = 0;
        if (prefix.Length > 0) charsAdded += StringEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
        var charsWritten                  = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);

        var toTransfer = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            StringEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex + charsAdded);
            charsAdded += charsWritten;
            if (suffix.Length > 0) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, suffix);
            return charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        StringEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
        charsAdded += padSize;
        if (suffix.Length > 0)
            charsAdded += StringEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
        return charsAdded;
    }

    public int Format(bool? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return Format(source.Value, sb, formatString, formatFlags);
    }

    public int Format(bool? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return Format(source.Value, destCharSpan, destStartIndex, formatString, formatFlags);
    }


    public int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString,
        FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        int NeverZero(int check) => check == 0 ? -1 : check;

        if (source == null) return 0;
        if (source is bool boolSource) { return NeverZero(Format(boolSource, sb, formatString)); }
        if (source is ISpanFormattable formattable) { return NeverZero(Format(formattable, sb, formatString)); }
        var type                     = source.GetType();
        var maybeIterableElementType = type.GetIterableElementType();
        if (maybeIterableElementType == null) return 0;
        if (!((maybeIterableElementType.IsSpanFormattableOrNullable() || maybeIterableElementType.IsBoolOrNullable()))) return 0;
        if (source is String stringSource) { return NeverZero(Format(stringSource, 0, sb, formatString)); }
        if (source is char[] charArraySource) { return NeverZero(Format(charArraySource, 0, sb, formatString)); }
        if (source is StringBuilder stringBuilderSource) { return NeverZero(Format(stringBuilderSource, 0, sb, formatString)); }
        if (source is ICharSequence charSeqSource) { return NeverZero(Format(charSeqSource, 0, sb, formatString)); }
        if (type.IsArray) { return NeverZero(CheckIsKnownSpanFormattableArray(source, sb, formatString, type)); }
        if (type.IsReadOnlyList()) { return NeverZero(CheckIsKnownSpanFormattableList(source, sb, formatString, type)); }
        if (type.IsEnumerable()) { return NeverZero(CheckIsKnownSpanFormattableEnumerable(source, sb, formatString, type)); }
        if (type.IsEnumerator()) { return NeverZero(CheckIsKnownSpanFormattableEnumerator(source, sb, formatString, type)); }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerator<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var enumeratorElementType = type.IfEnumeratorGetElementType()!;

        if (enumeratorElementType.IsSpanFormattable() || enumeratorElementType.IsBool())
        {
            switch (source)
            {
                case IEnumerable<bool?> boolEnumerator: return FormatBoolEnumerator(boolEnumerator.GetEnumerator(), sb, formatString, formatFlags);
                case IEnumerator<byte> byteEnumerator: return FormatEnumerator(byteEnumerator, sb, formatString, formatFlags);
                case IEnumerator<sbyte> sbyteEnumerator: return FormatEnumerator(sbyteEnumerator, sb, formatString, formatFlags);
                case IEnumerator<char> charEnumerator: return FormatEnumerator(charEnumerator, sb, formatString, formatFlags);
                case IEnumerator<short> shortEnumerator: return FormatEnumerator(shortEnumerator, sb, formatString, formatFlags);
                case IEnumerator<ushort> uShortEnumerator: return FormatEnumerator(uShortEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Half> halfFloatEnumerator: return FormatEnumerator(halfFloatEnumerator, sb, formatString, formatFlags);
                case IEnumerator<int> intEnumerator: return FormatEnumerator(intEnumerator, sb, formatString, formatFlags);
                case IEnumerator<uint> uIntEnumerator: return FormatEnumerator(uIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<nint> nIntEnumerator: return FormatEnumerator(nIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<float> floatEnumerator: return FormatEnumerator(floatEnumerator, sb, formatString, formatFlags);
                case IEnumerator<long> longEnumerator: return FormatEnumerator(longEnumerator, sb, formatString, formatFlags);
                case IEnumerator<ulong> uLongEnumerator: return FormatEnumerator(uLongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<double> doubleEnumerator: return FormatEnumerator(doubleEnumerator, sb, formatString, formatFlags);
                case IEnumerator<decimal> decimalEnumerator: return FormatEnumerator(decimalEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Int128> veryLongEnumerator: return FormatEnumerator(veryLongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<UInt128> veryUlongEnumerator: return FormatEnumerator(veryUlongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<BigInteger> bigIntEnumerator: return FormatEnumerator(bigIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Complex> complexNumEnumerator: return FormatEnumerator(complexNumEnumerator, sb, formatString, formatFlags);
                case IEnumerator<DateTime> dateTimeEnumerator: return FormatEnumerator(dateTimeEnumerator, sb, formatString, formatFlags);
                case IEnumerator<DateOnly> dateOnlyEnumerator: return FormatEnumerator(dateOnlyEnumerator, sb, formatString, formatFlags);
                case IEnumerator<TimeSpan> timeSpanEnumerator: return FormatEnumerator(timeSpanEnumerator, sb, formatString, formatFlags);
                case IEnumerator<TimeOnly> timeOnlyEnumerator: return FormatEnumerator(timeOnlyEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Rune> runeEnumerator: return FormatEnumerator(runeEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Guid> guidEnumerator: return FormatEnumerator(guidEnumerator, sb, formatString, formatFlags);
                case IEnumerator<IPNetwork> ipNetworkEnumerator: return FormatEnumerator(ipNetworkEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Enum> enumEnumerator: return FormatEnumerator(enumEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Version> versionEnumerator: return FormatEnumerator(versionEnumerator, sb, formatString, formatFlags);
                case IEnumerator<IPAddress> ipAddressEnumerator: return FormatEnumerator(ipAddressEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Uri> uriEnumerator: return FormatEnumerator(uriEnumerator, sb, formatString, formatFlags);
            }
        }
        else if (enumeratorElementType is { IsValueType: true, IsGenericType: true } &&
                 enumeratorElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<bool?> nullBoolEnumerator:
                    return FormatBoolEnumerator(nullBoolEnumerator.GetEnumerator(), sb, formatString, formatFlags);
                case IEnumerator<byte?> nullByteEnumerator: return FormatEnumerator(nullByteEnumerator, sb, formatString, formatFlags);
                case IEnumerator<sbyte?> nullSbyteEnumerator: return FormatEnumerator(nullSbyteEnumerator, sb, formatString, formatFlags);
                case IEnumerator<char?> nullCharEnumerator: return FormatEnumerator(nullCharEnumerator, sb, formatString, formatFlags);
                case IEnumerator<short?> nullShortEnumerator: return FormatEnumerator(nullShortEnumerator, sb, formatString, formatFlags);
                case IEnumerator<ushort?> nullUShortEnumerator: return FormatEnumerator(nullUShortEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Half?> nullHalfFloatEnumerator: return FormatEnumerator(nullHalfFloatEnumerator, sb, formatString, formatFlags);
                case IEnumerator<int?> nullIntEnumerator: return FormatEnumerator(nullIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<uint?> nullUIntEnumerator: return FormatEnumerator(nullUIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<nint?> nullNIntEnumerator: return FormatEnumerator(nullNIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<float?> nullFloatEnumerator: return FormatEnumerator(nullFloatEnumerator, sb, formatString, formatFlags);
                case IEnumerator<long?> nullLongEnumerator: return FormatEnumerator(nullLongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<ulong?> nullULongEnumerator: return FormatEnumerator(nullULongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<double?> nullDoubleEnumerator: return FormatEnumerator(nullDoubleEnumerator, sb, formatString, formatFlags);
                case IEnumerator<decimal?> nullDecimalEnumerator: return FormatEnumerator(nullDecimalEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Int128?> nullVeryLongEnumerator: return FormatEnumerator(nullVeryLongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<UInt128?> nullVeryUlongEnumerator: return FormatEnumerator(nullVeryUlongEnumerator, sb, formatString, formatFlags);
                case IEnumerator<BigInteger?> nullBigIntEnumerator: return FormatEnumerator(nullBigIntEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Complex?> nullComplexNumEnumerator: return FormatEnumerator(nullComplexNumEnumerator, sb, formatString, formatFlags);
                case IEnumerator<DateTime?> nullDateTimeEnumerator: return FormatEnumerator(nullDateTimeEnumerator, sb, formatString, formatFlags);
                case IEnumerator<DateOnly?> nullDateOnlyEnumerator: return FormatEnumerator(nullDateOnlyEnumerator, sb, formatString, formatFlags);
                case IEnumerator<TimeSpan?> nullTimeSpanEnumerator: return FormatEnumerator(nullTimeSpanEnumerator, sb, formatString, formatFlags);
                case IEnumerator<TimeOnly?> nullTimeOnlyEnumerator: return FormatEnumerator(nullTimeOnlyEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Rune?> nullRuneEnumerator: return FormatEnumerator(nullRuneEnumerator, sb, formatString, formatFlags);
                case IEnumerator<Guid?> nullGuidEnumerator: return FormatEnumerator(nullGuidEnumerator, sb, formatString, formatFlags);
                case IEnumerator<IPNetwork?> nullIpNetworkEnumerator: return FormatEnumerator(nullIpNetworkEnumerator, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerable<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var enumerableElementType = type.IfEnumerableGetElementType()!;

        if (enumerableElementType.IsSpanFormattable() || enumerableElementType.IsBool())
        {
            switch (source)
            {
                case IEnumerable<bool?> boolEnumerable: return FormatBoolEnumerator(boolEnumerable.GetEnumerator(), sb, formatString, formatFlags);
                case IEnumerable<byte> byteEnumerable: return FormatEnumerable(byteEnumerable, sb, formatString, formatFlags);
                case IEnumerable<sbyte> sbyteEnumerable: return FormatEnumerable(sbyteEnumerable, sb, formatString, formatFlags);
                case IEnumerable<char> charEnumerable: return FormatEnumerable(charEnumerable, sb, formatString, formatFlags);
                case IEnumerable<short> shortEnumerable: return FormatEnumerable(shortEnumerable, sb, formatString, formatFlags);
                case IEnumerable<ushort> uShortEnumerable: return FormatEnumerable(uShortEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Half> halfFloatEnumerable: return FormatEnumerable(halfFloatEnumerable, sb, formatString, formatFlags);
                case IEnumerable<int> intEnumerable: return FormatEnumerable(intEnumerable, sb, formatString, formatFlags);
                case IEnumerable<uint> uIntEnumerable: return FormatEnumerable(uIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<nint> nIntEnumerable: return FormatEnumerable(nIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<float> floatEnumerable: return FormatEnumerable(floatEnumerable, sb, formatString, formatFlags);
                case IEnumerable<long> longEnumerable: return FormatEnumerable(longEnumerable, sb, formatString, formatFlags);
                case IEnumerable<ulong> uLongEnumerable: return FormatEnumerable(uLongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<double> doubleEnumerable: return FormatEnumerable(doubleEnumerable, sb, formatString, formatFlags);
                case IEnumerable<decimal> decimalEnumerable: return FormatEnumerable(decimalEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Int128> veryLongEnumerable: return FormatEnumerable(veryLongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<UInt128> veryULongEnumerable: return FormatEnumerable(veryULongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<BigInteger> bigIntEnumerable: return FormatEnumerable(bigIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Complex> complexNumEnumerable: return FormatEnumerable(complexNumEnumerable, sb, formatString, formatFlags);
                case IEnumerable<DateTime> dateTimeEnumerable: return FormatEnumerable(dateTimeEnumerable, sb, formatString, formatFlags);
                case IEnumerable<DateOnly> dateOnlyEnumerable: return FormatEnumerable(dateOnlyEnumerable, sb, formatString, formatFlags);
                case IEnumerable<TimeSpan> timeSpanEnumerable: return FormatEnumerable(timeSpanEnumerable, sb, formatString, formatFlags);
                case IEnumerable<TimeOnly> timeOnlyEnumerable: return FormatEnumerable(timeOnlyEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Rune> runeEnumerable: return FormatEnumerable(runeEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Guid> guidEnumerable: return FormatEnumerable(guidEnumerable, sb, formatString, formatFlags);
                case IEnumerable<IPNetwork> ipNetworkEnumerable: return FormatEnumerable(ipNetworkEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Enum> enumEnumerable: return FormatEnumerable(enumEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Version> versionEnumerable: return FormatEnumerable(versionEnumerable, sb, formatString, formatFlags);
                case IEnumerable<IPAddress> ipAddressEnumerable: return FormatEnumerable(ipAddressEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Uri> uriEnumerable: return FormatEnumerable(uriEnumerable, sb, formatString, formatFlags);
            }
        }
        else if (enumerableElementType is { IsValueType: true, IsGenericType: true } &&
                 enumerableElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<bool?> nullBoolEnumerable:
                    return FormatBoolEnumerator(nullBoolEnumerable.GetEnumerator(), sb, formatString, formatFlags);
                case IEnumerable<byte?> nullByteEnumerable: return FormatEnumerable(nullByteEnumerable, sb, formatString, formatFlags);
                case IEnumerable<sbyte?> nullSbyteEnumerable: return FormatEnumerable(nullSbyteEnumerable, sb, formatString, formatFlags);
                case IEnumerable<char?> nullCharEnumerable: return FormatEnumerable(nullCharEnumerable, sb, formatString, formatFlags);
                case IEnumerable<short?> nullShortEnumerable: return FormatEnumerable(nullShortEnumerable, sb, formatString, formatFlags);
                case IEnumerable<ushort?> nullUShortEnumerable: return FormatEnumerable(nullUShortEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Half?> nullHalfFloatEnumerable: return FormatEnumerable(nullHalfFloatEnumerable, sb, formatString, formatFlags);
                case IEnumerable<int?> nullIntEnumerable: return FormatEnumerable(nullIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<uint?> nullUIntEnumerable: return FormatEnumerable(nullUIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<nint?> nullNIntEnumerable: return FormatEnumerable(nullNIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<float?> nullFloatEnumerable: return FormatEnumerable(nullFloatEnumerable, sb, formatString, formatFlags);
                case IEnumerable<long?> nullLongEnumerable: return FormatEnumerable(nullLongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<ulong?> nullULongEnumerable: return FormatEnumerable(nullULongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<double?> nullDoubleEnumerable: return FormatEnumerable(nullDoubleEnumerable, sb, formatString, formatFlags);
                case IEnumerable<decimal?> nullDecimalEnumerable: return FormatEnumerable(nullDecimalEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Int128?> nullVeryLongEnumerable: return FormatEnumerable(nullVeryLongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<UInt128?> nullVeryULongEnumerable: return FormatEnumerable(nullVeryULongEnumerable, sb, formatString, formatFlags);
                case IEnumerable<BigInteger?> nullBigIntEnumerable: return FormatEnumerable(nullBigIntEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Complex?> nullComplexNumEnumerable: return FormatEnumerable(nullComplexNumEnumerable, sb, formatString, formatFlags);
                case IEnumerable<DateTime?> nullDateTimeEnumerable: return FormatEnumerable(nullDateTimeEnumerable, sb, formatString, formatFlags);
                case IEnumerable<DateOnly?> nullDateOnlyEnumerable: return FormatEnumerable(nullDateOnlyEnumerable, sb, formatString, formatFlags);
                case IEnumerable<TimeSpan?> nullTimeSpanEnumerable: return FormatEnumerable(nullTimeSpanEnumerable, sb, formatString, formatFlags);
                case IEnumerable<TimeOnly?> nullTimeOnlyEnumerable: return FormatEnumerable(nullTimeOnlyEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Rune?> nullRuneEnumerable: return FormatEnumerable(nullRuneEnumerable, sb, formatString, formatFlags);
                case IEnumerable<Guid?> nullGuidEnumerable: return FormatEnumerable(nullGuidEnumerable, sb, formatString, formatFlags);
                case IEnumerable<IPNetwork?> nullIpNetworkEnumerable: return FormatEnumerable(nullIpNetworkEnumerable, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableList<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var listType = type.GetListElementType()!;

        if (listType.IsSpanFormattable() || listType.IsBool())
        {
            switch (source)
            {
                case IReadOnlyList<bool> boolList:           return FormatBoolEnumerator(boolList.GetEnumerator(), sb, formatString, formatFlags);
                case IReadOnlyList<byte> byteList:           return FormatList(byteList, sb, formatString, formatFlags);
                case IReadOnlyList<sbyte> sbyteList:         return FormatList(sbyteList, sb, formatString, formatFlags);
                case IReadOnlyList<char> charList:           return FormatList(charList, sb, formatString, formatFlags);
                case IReadOnlyList<short> shortList:         return FormatList(shortList, sb, formatString, formatFlags);
                case IReadOnlyList<ushort> uShortList:       return FormatList(uShortList, sb, formatString, formatFlags);
                case IReadOnlyList<Half> halfFloatList:      return FormatList(halfFloatList, sb, formatString, formatFlags);
                case IReadOnlyList<int> intList:             return FormatList(intList, sb, formatString, formatFlags);
                case IReadOnlyList<uint> uIntList:           return FormatList(uIntList, sb, formatString, formatFlags);
                case IReadOnlyList<nint> nIntList:           return FormatList(nIntList, sb, formatString, formatFlags);
                case IReadOnlyList<float> floatList:         return FormatList(floatList, sb, formatString, formatFlags);
                case IReadOnlyList<long> longList:           return FormatList(longList, sb, formatString, formatFlags);
                case IReadOnlyList<ulong> uLongList:         return FormatList(uLongList, sb, formatString, formatFlags);
                case IReadOnlyList<double> doubleList:       return FormatList(doubleList, sb, formatString, formatFlags);
                case IReadOnlyList<decimal> decimalList:     return FormatList(decimalList, sb, formatString, formatFlags);
                case IReadOnlyList<Int128> veryLongList:     return FormatList(veryLongList, sb, formatString, formatFlags);
                case IReadOnlyList<UInt128> veryULongList:   return FormatList(veryULongList, sb, formatString, formatFlags);
                case IReadOnlyList<BigInteger> bigIntList:   return FormatList(bigIntList, sb, formatString, formatFlags);
                case IReadOnlyList<Complex> complexNumList:  return FormatList(complexNumList, sb, formatString, formatFlags);
                case IReadOnlyList<DateTime> dateTimeList:   return FormatList(dateTimeList, sb, formatString, formatFlags);
                case IReadOnlyList<DateOnly> dateOnlyList:   return FormatList(dateOnlyList, sb, formatString, formatFlags);
                case IReadOnlyList<TimeSpan> timeSpanList:   return FormatList(timeSpanList, sb, formatString, formatFlags);
                case IReadOnlyList<TimeOnly> timeOnlyList:   return FormatList(timeOnlyList, sb, formatString, formatFlags);
                case IReadOnlyList<Rune> runeList:           return FormatList(runeList, sb, formatString, formatFlags);
                case IReadOnlyList<Guid> guidList:           return FormatList(guidList, sb, formatString, formatFlags);
                case IReadOnlyList<IPNetwork> ipNetworkList: return FormatList(ipNetworkList, sb, formatString, formatFlags);
                case IReadOnlyList<Enum> enumList:           return FormatList(enumList, sb, formatString, formatFlags);
                case IReadOnlyList<Version> versionList:     return FormatList(versionList, sb, formatString, formatFlags);
                case IReadOnlyList<IPAddress> ipAddressList: return FormatList(ipAddressList, sb, formatString, formatFlags);
                case IReadOnlyList<Uri> uriList:             return FormatList(uriList, sb, formatString, formatFlags);
            }
        }
        else if (listType is { IsValueType: true, IsGenericType: true } && listType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IReadOnlyList<bool?> nullBoolList: return FormatBoolEnumerator(nullBoolList.GetEnumerator(), sb, formatString, formatFlags);
                case IReadOnlyList<byte?> nullByteList: return FormatList(nullByteList, sb, formatString, formatFlags);
                case IReadOnlyList<sbyte?> nullSbyteList: return FormatList(nullSbyteList, sb, formatString, formatFlags);
                case IReadOnlyList<char?> nullCharList: return FormatList(nullCharList, sb, formatString, formatFlags);
                case IReadOnlyList<short?> nullShortList: return FormatList(nullShortList, sb, formatString, formatFlags);
                case IReadOnlyList<ushort?> nullUShortList: return FormatList(nullUShortList, sb, formatString, formatFlags);
                case IReadOnlyList<Half?> nullHalfFloatList: return FormatList(nullHalfFloatList, sb, formatString, formatFlags);
                case IReadOnlyList<int?> nullIntList: return FormatList(nullIntList, sb, formatString, formatFlags);
                case IReadOnlyList<uint?> nullUIntList: return FormatList(nullUIntList, sb, formatString, formatFlags);
                case IReadOnlyList<nint?> nullNIntList: return FormatList(nullNIntList, sb, formatString, formatFlags);
                case IReadOnlyList<float?> nullFloatList: return FormatList(nullFloatList, sb, formatString, formatFlags);
                case IReadOnlyList<long?> nullLongList: return FormatList(nullLongList, sb, formatString, formatFlags);
                case IReadOnlyList<ulong?> nullULongList: return FormatList(nullULongList, sb, formatString, formatFlags);
                case IReadOnlyList<double?> nullDoubleList: return FormatList(nullDoubleList, sb, formatString, formatFlags);
                case IReadOnlyList<decimal?> nullDecimalList: return FormatList(nullDecimalList, sb, formatString, formatFlags);
                case IReadOnlyList<Int128?> nullVeryLongList: return FormatList(nullVeryLongList, sb, formatString, formatFlags);
                case IReadOnlyList<UInt128?> nullVeryULongList: return FormatList(nullVeryULongList, sb, formatString, formatFlags);
                case IReadOnlyList<BigInteger?> nullBigIntList: return FormatList(nullBigIntList, sb, formatString, formatFlags);
                case IReadOnlyList<Complex?> nullComplexNumList: return FormatList(nullComplexNumList, sb, formatString, formatFlags);
                case IReadOnlyList<DateTime?> nullDateTimeList: return FormatList(nullDateTimeList, sb, formatString, formatFlags);
                case IReadOnlyList<DateOnly?> nullDateOnlyList: return FormatList(nullDateOnlyList, sb, formatString, formatFlags);
                case IReadOnlyList<TimeSpan?> nullTimeSpanList: return FormatList(nullTimeSpanList, sb, formatString, formatFlags);
                case IReadOnlyList<TimeOnly?> nullTimeOnlyList: return FormatList(nullTimeOnlyList, sb, formatString, formatFlags);
                case IReadOnlyList<Rune?> nullRuneList: return FormatList(nullRuneList, sb, formatString, formatFlags);
                case IReadOnlyList<Guid?> nullGuidList: return FormatList(nullGuidList, sb, formatString, formatFlags);
                case IReadOnlyList<IPNetwork?> nullIpNetworkList: return FormatList(nullIpNetworkList, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableArray<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var elementType = type.GetElementType()!;
        if (elementType.IsSpanFormattable() || elementType.IsBool())
        {
            switch (source)
            {
                case bool[] boolArray: return FormatBoolEnumerator((IEnumerator<bool>)boolArray.GetEnumerator(), sb, formatString, formatFlags);
                case byte[] byteArray: return FormatArray(byteArray, sb, formatString, formatFlags);
                case sbyte[] sbyteArray: return FormatArray(sbyteArray, sb, formatString, formatFlags);
                case char[] charArray: return FormatArray(charArray, sb, formatString, formatFlags);
                case short[] shortArray: return FormatArray(shortArray, sb, formatString, formatFlags);
                case ushort[] uShortArray: return FormatArray(uShortArray, sb, formatString, formatFlags);
                case Half[] halfFloatArray: return FormatArray(halfFloatArray, sb, formatString, formatFlags);
                case int[] intArray: return FormatArray(intArray, sb, formatString, formatFlags);
                case uint[] uIntArray: return FormatArray(uIntArray, sb, formatString, formatFlags);
                case nint[] nIntArray: return FormatArray(nIntArray, sb, formatString, formatFlags);
                case float[] floatArray: return FormatArray(floatArray, sb, formatString, formatFlags);
                case long[] longArray: return FormatArray(longArray, sb, formatString, formatFlags);
                case ulong[] uLongArray: return FormatArray(uLongArray, sb, formatString, formatFlags);
                case double[] doubleArray: return FormatArray(doubleArray, sb, formatString, formatFlags);
                case decimal[] decimalArray: return FormatArray(decimalArray, sb, formatString, formatFlags);
                case Int128[] veryLongArray: return FormatArray(veryLongArray, sb, formatString, formatFlags);
                case UInt128[] veryULongArray: return FormatArray(veryULongArray, sb, formatString, formatFlags);
                case BigInteger[] bigIntArray: return FormatArray(bigIntArray, sb, formatString, formatFlags);
                case Complex[] complexNumArray: return FormatArray(complexNumArray, sb, formatString, formatFlags);
                case DateTime[] dateTimeArray: return FormatArray(dateTimeArray, sb, formatString, formatFlags);
                case DateOnly[] dateOnlyArray: return FormatArray(dateOnlyArray, sb, formatString, formatFlags);
                case TimeSpan[] timeSpanArray: return FormatArray(timeSpanArray, sb, formatString, formatFlags);
                case TimeOnly[] timeOnlyArray: return FormatArray(timeOnlyArray, sb, formatString, formatFlags);
                case Rune[] runeArray: return FormatArray(runeArray, sb, formatString, formatFlags);
                case Guid[] guidArray: return FormatArray(guidArray, sb, formatString, formatFlags);
                case IPNetwork[] ipNetworkArray: return FormatArray(ipNetworkArray, sb, formatString, formatFlags);
                case Enum[] enumArray: return FormatArray(enumArray, sb, formatString, formatFlags);
                case Version[] versionArray: return FormatArray(versionArray, sb, formatString, formatFlags);
                case IPAddress[] ipAddressArray: return FormatArray(ipAddressArray, sb, formatString, formatFlags);
                case Uri[] uriArray: return FormatArray(uriArray, sb, formatString, formatFlags);
            }
        }
        else if (elementType is { IsValueType: true, IsGenericType: true } && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case bool?[] nullBoolArray:
                    return FormatBoolEnumerator((IEnumerator<bool?>)(nullBoolArray.GetEnumerator()), sb, formatString, formatFlags);
                case byte?[] nullByteArray:           return FormatArray(nullByteArray, sb, formatString, formatFlags);
                case sbyte?[] nullSbyteArray:         return FormatArray(nullSbyteArray, sb, formatString, formatFlags);
                case char?[] nullCharArray:           return FormatArray(nullCharArray, sb, formatString, formatFlags);
                case short?[] nullShortArray:         return FormatArray(nullShortArray, sb, formatString, formatFlags);
                case ushort?[] nullUShortArray:       return FormatArray(nullUShortArray, sb, formatString, formatFlags);
                case Half?[] nullHalfFloatArray:      return FormatArray(nullHalfFloatArray, sb, formatString, formatFlags);
                case int?[] nullIntArray:             return FormatArray(nullIntArray, sb, formatString, formatFlags);
                case uint?[] nullUIntArray:           return FormatArray(nullUIntArray, sb, formatString, formatFlags);
                case nint?[] nullNIntArray:           return FormatArray(nullNIntArray, sb, formatString, formatFlags);
                case float?[] nullFloatArray:         return FormatArray(nullFloatArray, sb, formatString, formatFlags);
                case long?[] nullLongArray:           return FormatArray(nullLongArray, sb, formatString, formatFlags);
                case ulong?[] nullULongArray:         return FormatArray(nullULongArray, sb, formatString, formatFlags);
                case double?[] nullDoubleArray:       return FormatArray(nullDoubleArray, sb, formatString, formatFlags);
                case decimal?[] nullDecimalArray:     return FormatArray(nullDecimalArray, sb, formatString, formatFlags);
                case Int128?[] nullVeryLongArray:     return FormatArray(nullVeryLongArray, sb, formatString, formatFlags);
                case UInt128?[] nullVeryULongArray:   return FormatArray(nullVeryULongArray, sb, formatString, formatFlags);
                case BigInteger?[] nullBigIntArray:   return FormatArray(nullBigIntArray, sb, formatString, formatFlags);
                case Complex?[] nullComplexNumArray:  return FormatArray(nullComplexNumArray, sb, formatString, formatFlags);
                case DateTime?[] nullDateTimeArray:   return FormatArray(nullDateTimeArray, sb, formatString, formatFlags);
                case DateOnly?[] nullDateOnlyArray:   return FormatArray(nullDateOnlyArray, sb, formatString, formatFlags);
                case TimeSpan?[] nullTimeSpanArray:   return FormatArray(nullTimeSpanArray, sb, formatString, formatFlags);
                case TimeOnly?[] nullTimeOnlyArray:   return FormatArray(nullTimeOnlyArray, sb, formatString, formatFlags);
                case Rune?[] nullRuneArray:           return FormatArray(nullRuneArray, sb, formatString, formatFlags);
                case Guid?[] nullGuidArray:           return FormatArray(nullGuidArray, sb, formatString, formatFlags);
                case IPNetwork?[] nullIpNetworkArray: return FormatArray(nullIpNetworkArray, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(bool);
        if (!hasNext || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(bool);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatFlags);
        }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0)
                    addedChars += AddCollectionElementSeparatorAndPadding
                        (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatFlags);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0)
                    addedChars += AddCollectionElementSeparatorAndPadding
                        (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatFlags);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
        }
        return addedChars;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(bool?);
        if (!hasNext || !Options.IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItemFormat(item, itemCount, sb, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(bool?);
        if (!hasNext || !Options.IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatFlags); }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatFlags);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatFlags);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatFlags);
        }
        return addedChars;
    }

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    public abstract int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;

    public abstract int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public virtual int CollectionNextItemFormat(bool nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) => Format(nextItem, sb, formatString);

    public virtual int CollectionNextItemFormat(bool nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) =>
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public int CollectionNextItemFormat(bool? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (nextItem == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return CollectionNextItemFormat(nextItem.Value, retrieveCount, sb, formatString);
    }

    public int CollectionNextItemFormat(bool? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (nextItem == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return CollectionNextItemFormat(nextItem.Value, retrieveCount, destCharSpan, destStartIndex, formatString);
    }

    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    public abstract int CollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    public abstract int CollectionEnd(Type collectionType, Span<char> destSpan, int destIndex, int totalItemCount
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);


    public override void StateReset()
    {
        (EncoderTransfer as IRecyclableObject)?.DecrementRefCount();
        base.StateReset();
    }
}
