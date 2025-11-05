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

    protected const string NoFormatFormatString = "{0}";

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
        set
        {
            
            FormatOptions = value;
        }
    }

    public virtual IEncodingTransfer TransferEncoder
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
            if (maybeDelimited.IsCharArray())
            {
                return destSpan.OverWriteAt(fromIndex, SqBrktOpn);
            }
            return StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }

    public virtual int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray())
            {
                return destSpan.OverWriteAt(fromIndex, SqBrktCls);
            }
            return StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }

    public int StringValueDelimiter(IStringBuilder sb) => sb.Append(DblQt).ReturnCharCount(1);

    public int StringValueDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public int StringFieldDelimiter(IStringBuilder sb) => sb.Append(DblQt).ReturnCharCount(1);

    public int StringFieldDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);


    public virtual int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) =>
        sb.Append(Options.ItemSeparator).ReturnCharCount(Options.ItemSeparator.Length);

    public virtual int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) =>
        destSpan.OverWriteAt(atIndex, Options.ItemSeparator);

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

    protected virtual bool TryGetCachedCustomStyledToStringFormatter<T>([NotNullWhen(true)] out PalantírReveal<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomStyledToStringFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportStyleToString && formattableProvider is IStringBearerRevelStateProvider<T> spanFormattableProvider)
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
        var cappedLength = Math.Clamp(maxTransferCount,0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return TransferEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        source       =  source[sourceFrom..(sourceFrom + cappedLength)];

        extendLengthRange = extendLengthRange.BoundRangeToLength(cappedLength);
        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded += TransferEncoder.Transfer(this, source[extendLengthRange], 0, sb, maxTransferCount: cappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
        var alignedLength = source.CalculatePaddedAlignedLength(layout) + 256;
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom   =  Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount,0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return TransferEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        return Format(((ReadOnlySpan<char>)source)[sourceFrom..], 0, sb, formatString, maxTransferCount, formatFlags);
    }

    public virtual int Format(StringBuilder source, int sourceFrom, IStringBuilder sb
      , ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount,0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return TransferEncoder.Transfer(this,  source, sourceFrom, sb, sb.Length, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        
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
            charsAdded += TransferEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount,0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return TransferEncoder.Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        
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
            charsAdded += TransferEncoder.Transfer(this, source, rawSourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], sb);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString
      , int destStartIndex = 0 , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        sourceFrom   =  Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return TransferEncoder.Transfer(this, source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
        
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
            charsAdded += TransferEncoder.Transfer(this, source, 0, destCharSpan, destStartIndex + charsAdded
                                                          , rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
            return TransferEncoder.Transfer(this, source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

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
            return TransferEncoder.Transfer(this, source, destCharSpan, destStartIndex);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
        
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
            charsAdded += TransferEncoder.Transfer(this, source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
            return TransferEncoder.Transfer(this, source, destCharSpan, destStartIndex, maxTransferCount);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
        
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
            charsAdded += TransferEncoder.Transfer(this, source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
            }

            charsAdded += TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
            return charsAdded;
        }
    }

    public virtual int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        int charsWritten;
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            var charSpan = stackalloc char[2048].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            TransferEncoder.Transfer(this, charSpan[..charsWritten], sb);
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
            TransferEncoder.Transfer(this, charSpan[..charsWritten], sb);
            return charsWritten;
        }
        try
        {
            var charSpan   = stackalloc char[2048].ResetMemory();
            var charsAdded = 0;
            formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var outputSubRange
                                                         , out var layout, out _, out var format, out var suffix);
            if (prefix.Length > 0)
                charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
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
                    TransferEncoder.Transfer(this, toTransfer, sb);
                    if (suffix.Length > 0) charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
                    return charsWritten + charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                TransferEncoder.Transfer(this, padSpan[..padSize], sb);
                if (suffix.Length > 0)
                    charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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
                    if (sbPreAppendLen == sb.Length)
                    {
                        sb.Append(source);
                    }
                    else
                    {
                        LastFormatExceptionBuilderLength = -1;
                    }
                }
            }
            charsWritten = sb.Length - sbPreAppendLen;
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString);
        int charsWritten;
        var charSpan = stackalloc char[2048].ResetMemory();
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            charsWritten = formatter(source, charSpan, formatString, null);
            return TransferEncoder.Transfer(this, charSpan[..charsWritten], destCharSpan, destStartIndex);
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
            return TransferEncoder.Transfer(this, charSpan[..charsWritten], destCharSpan, destStartIndex);
        }
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var outputSubRange
                                                     , out var layout, out _, out var format, out var suffix);
        try
        {
            var charsAdded = 0;
            if (prefix.Length > 0)
                charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
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
                    TransferEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex + charsAdded);
                    charsAdded += charsWritten;
                    if (suffix.Length > 0) charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
                    return charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
                charsAdded += padSize;
                if (suffix.Length > 0)
                    charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
                    if (layout.Length == 0) { return TransferEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex); }
                    var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                    var padSize = padSpan.PadAndAlign(toTransfer, layout);
                    charsWritten = TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
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

        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, sb);
        var charsWritten                  = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);
        var toTransfer                    = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            TransferEncoder.Transfer(this, toTransfer, sb);
            if (suffix.Length > 0) charsAdded += sb.Append(suffix).ReturnCharCount(suffix.Length);
            return charsWritten + charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        TransferEncoder.Transfer(this, padSpan[..padSize], sb);
        if (suffix.Length > 0) charsAdded += TransferEncoder.TransferSuffix(suffix, sb, formatFlags.HasEncodeBoundsFlag());
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
        if (prefix.Length > 0) charsAdded += TransferEncoder.TransferPrefix(formatFlags.HasEncodeBoundsFlag(), prefix, destCharSpan, destStartIndex);
        var charsWritten                  = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);

        var toTransfer = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            TransferEncoder.Transfer(this, toTransfer, destCharSpan, destStartIndex + charsAdded);
            charsAdded += charsWritten;
            if (suffix.Length > 0) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, suffix);
            return charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        TransferEncoder.Transfer(this, padSpan[..padSize], destCharSpan, destStartIndex);
        charsAdded += padSize;
        if (suffix.Length > 0)
            charsAdded += TransferEncoder.TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded, formatFlags.HasEncodeBoundsFlag());
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
        if (!((maybeIterableElementType.IsSpanFormattable()) || (maybeIterableElementType.IsNullableSpanFormattable()))) return 0;
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

        if (enumeratorElementType.IsSpanFormattable())
        {
            switch (source)
            {
                case IEnumerator<byte> byteArray:           return FormatEnumerator(byteArray, sb, formatString, formatFlags);
                case IEnumerator<sbyte> sbyteArray:         return FormatEnumerator(sbyteArray, sb, formatString, formatFlags);
                case IEnumerator<char> charArray:           return FormatEnumerator(charArray, sb, formatString, formatFlags);
                case IEnumerator<short> shortArray:         return FormatEnumerator(shortArray, sb, formatString, formatFlags);
                case IEnumerator<ushort> uShortArray:       return FormatEnumerator(uShortArray, sb, formatString, formatFlags);
                case IEnumerator<Half> halfFloatArray:      return FormatEnumerator(halfFloatArray, sb, formatString, formatFlags);
                case IEnumerator<int> intArray:             return FormatEnumerator(intArray, sb, formatString, formatFlags);
                case IEnumerator<uint> uIntArray:           return FormatEnumerator(uIntArray, sb, formatString, formatFlags);
                case IEnumerator<nint> nIntArray:           return FormatEnumerator(nIntArray, sb, formatString, formatFlags);
                case IEnumerator<float> floatArray:         return FormatEnumerator(floatArray, sb, formatString, formatFlags);
                case IEnumerator<long> longArray:           return FormatEnumerator(longArray, sb, formatString, formatFlags);
                case IEnumerator<ulong> uLongArray:         return FormatEnumerator(uLongArray, sb, formatString, formatFlags);
                case IEnumerator<double> doubleArray:       return FormatEnumerator(doubleArray, sb, formatString, formatFlags);
                case IEnumerator<decimal> decimalArray:     return FormatEnumerator(decimalArray, sb, formatString, formatFlags);
                case IEnumerator<Int128> veryLongArray:     return FormatEnumerator(veryLongArray, sb, formatString, formatFlags);
                case IEnumerator<UInt128> veryUlongArray:   return FormatEnumerator(veryUlongArray, sb, formatString, formatFlags);
                case IEnumerator<BigInteger> bigIntArray:   return FormatEnumerator(bigIntArray, sb, formatString, formatFlags);
                case IEnumerator<Complex> complexNumArray:  return FormatEnumerator(complexNumArray, sb, formatString, formatFlags);
                case IEnumerator<DateTime> dateTimeArray:   return FormatEnumerator(dateTimeArray, sb, formatString, formatFlags);
                case IEnumerator<DateOnly> dateOnlyArray:   return FormatEnumerator(dateOnlyArray, sb, formatString, formatFlags);
                case IEnumerator<TimeSpan> timeSpanArray:   return FormatEnumerator(timeSpanArray, sb, formatString, formatFlags);
                case IEnumerator<TimeOnly> timeOnlyArray:   return FormatEnumerator(timeOnlyArray, sb, formatString, formatFlags);
                case IEnumerator<Rune> runeArray:           return FormatEnumerator(runeArray, sb, formatString, formatFlags);
                case IEnumerator<Guid> guidArray:           return FormatEnumerator(guidArray, sb, formatString, formatFlags);
                case IEnumerator<IPNetwork> ipNetworkArray: return FormatEnumerator(ipNetworkArray, sb, formatString, formatFlags);
                case IEnumerator<Enum> enumArray:           return FormatEnumerator(enumArray, sb, formatString, formatFlags);
                case IEnumerator<Version> versionArray:     return FormatEnumerator(versionArray, sb, formatString, formatFlags);
                case IEnumerator<IPAddress> ipAddressArray: return FormatEnumerator(ipAddressArray, sb, formatString, formatFlags);
                case IEnumerator<Uri> uriArray:             return FormatEnumerator(uriArray, sb, formatString, formatFlags);
            }
        }
        else if (enumeratorElementType is { IsValueType: true, IsGenericType: true } &&
                 enumeratorElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerator<byte?> nullByteArray:           return FormatEnumerator(nullByteArray, sb, formatString, formatFlags);
                case IEnumerator<sbyte?> nullSbyteArray:         return FormatEnumerator(nullSbyteArray, sb, formatString, formatFlags);
                case IEnumerator<char?> nullCharArray:           return FormatEnumerator(nullCharArray, sb, formatString, formatFlags);
                case IEnumerator<short?> nullShortArray:         return FormatEnumerator(nullShortArray, sb, formatString, formatFlags);
                case IEnumerator<ushort?> nullUShortArray:       return FormatEnumerator(nullUShortArray, sb, formatString, formatFlags);
                case IEnumerator<Half?> nullHalfFloatArray:      return FormatEnumerator(nullHalfFloatArray, sb, formatString, formatFlags);
                case IEnumerator<int?> nullIntArray:             return FormatEnumerator(nullIntArray, sb, formatString, formatFlags);
                case IEnumerator<uint?> nullUIntArray:           return FormatEnumerator(nullUIntArray, sb, formatString, formatFlags);
                case IEnumerator<nint?> nullNIntArray:           return FormatEnumerator(nullNIntArray, sb, formatString, formatFlags);
                case IEnumerator<float?> nullFloatArray:         return FormatEnumerator(nullFloatArray, sb, formatString, formatFlags);
                case IEnumerator<long?> nullLongArray:           return FormatEnumerator(nullLongArray, sb, formatString, formatFlags);
                case IEnumerator<ulong?> nullULongArray:         return FormatEnumerator(nullULongArray, sb, formatString, formatFlags);
                case IEnumerator<double?> nullDoubleArray:       return FormatEnumerator(nullDoubleArray, sb, formatString, formatFlags);
                case IEnumerator<decimal?> nullDecimalArray:     return FormatEnumerator(nullDecimalArray, sb, formatString, formatFlags);
                case IEnumerator<Int128?> nullVeryLongArray:     return FormatEnumerator(nullVeryLongArray, sb, formatString, formatFlags);
                case IEnumerator<UInt128?> nullVeryUlongArray:   return FormatEnumerator(nullVeryUlongArray, sb, formatString, formatFlags);
                case IEnumerator<BigInteger?> nullBigIntArray:   return FormatEnumerator(nullBigIntArray, sb, formatString, formatFlags);
                case IEnumerator<Complex?> nullComplexNumArray:  return FormatEnumerator(nullComplexNumArray, sb, formatString, formatFlags);
                case IEnumerator<DateTime?> nullDateTimeArray:   return FormatEnumerator(nullDateTimeArray, sb, formatString, formatFlags);
                case IEnumerator<DateOnly?> nullDateOnlyArray:   return FormatEnumerator(nullDateOnlyArray, sb, formatString, formatFlags);
                case IEnumerator<TimeSpan?> nullTimeSpanArray:   return FormatEnumerator(nullTimeSpanArray, sb, formatString, formatFlags);
                case IEnumerator<TimeOnly?> nullTimeOnlyArray:   return FormatEnumerator(nullTimeOnlyArray, sb, formatString, formatFlags);
                case IEnumerator<Rune?> nullRuneArray:           return FormatEnumerator(nullRuneArray, sb, formatString, formatFlags);
                case IEnumerator<Guid?> nullGuidArray:           return FormatEnumerator(nullGuidArray, sb, formatString, formatFlags);
                case IEnumerator<IPNetwork?> nullIpNetworkArray: return FormatEnumerator(nullIpNetworkArray, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerable<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var enumerableElementType = type.IfEnumerableGetElementType()!;

        if (enumerableElementType.IsSpanFormattable())
        {
            switch (source)
            {
                case IEnumerable<byte> byteArray:           return FormatEnumerable(byteArray, sb, formatString, formatFlags);
                case IEnumerable<sbyte> sbyteArray:         return FormatEnumerable(sbyteArray, sb, formatString, formatFlags);
                case IEnumerable<char> charArray:           return FormatEnumerable(charArray, sb, formatString, formatFlags);
                case IEnumerable<short> shortArray:         return FormatEnumerable(shortArray, sb, formatString, formatFlags);
                case IEnumerable<ushort> uShortArray:       return FormatEnumerable(uShortArray, sb, formatString, formatFlags);
                case IEnumerable<Half> halfFloatArray:      return FormatEnumerable(halfFloatArray, sb, formatString, formatFlags);
                case IEnumerable<int> intArray:             return FormatEnumerable(intArray, sb, formatString, formatFlags);
                case IEnumerable<uint> uIntArray:           return FormatEnumerable(uIntArray, sb, formatString, formatFlags);
                case IEnumerable<nint> nIntArray:           return FormatEnumerable(nIntArray, sb, formatString, formatFlags);
                case IEnumerable<float> floatArray:         return FormatEnumerable(floatArray, sb, formatString, formatFlags);
                case IEnumerable<long> longArray:           return FormatEnumerable(longArray, sb, formatString, formatFlags);
                case IEnumerable<ulong> uLongArray:         return FormatEnumerable(uLongArray, sb, formatString, formatFlags);
                case IEnumerable<double> doubleArray:       return FormatEnumerable(doubleArray, sb, formatString, formatFlags);
                case IEnumerable<decimal> decimalArray:     return FormatEnumerable(decimalArray, sb, formatString, formatFlags);
                case IEnumerable<Int128> veryLongArray:     return FormatEnumerable(veryLongArray, sb, formatString, formatFlags);
                case IEnumerable<UInt128> veryULongArray:   return FormatEnumerable(veryULongArray, sb, formatString, formatFlags);
                case IEnumerable<BigInteger> bigIntArray:   return FormatEnumerable(bigIntArray, sb, formatString, formatFlags);
                case IEnumerable<Complex> complexNumArray:  return FormatEnumerable(complexNumArray, sb, formatString, formatFlags);
                case IEnumerable<DateTime> dateTimeArray:   return FormatEnumerable(dateTimeArray, sb, formatString, formatFlags);
                case IEnumerable<DateOnly> dateOnlyArray:   return FormatEnumerable(dateOnlyArray, sb, formatString, formatFlags);
                case IEnumerable<TimeSpan> timeSpanArray:   return FormatEnumerable(timeSpanArray, sb, formatString, formatFlags);
                case IEnumerable<TimeOnly> timeOnlyArray:   return FormatEnumerable(timeOnlyArray, sb, formatString, formatFlags);
                case IEnumerable<Rune> runeArray:           return FormatEnumerable(runeArray, sb, formatString, formatFlags);
                case IEnumerable<Guid> guidArray:           return FormatEnumerable(guidArray, sb, formatString, formatFlags);
                case IEnumerable<IPNetwork> ipNetworkArray: return FormatEnumerable(ipNetworkArray, sb, formatString, formatFlags);
                case IEnumerable<Enum> enumArray:           return FormatEnumerable(enumArray, sb, formatString, formatFlags);
                case IEnumerable<Version> versionArray:     return FormatEnumerable(versionArray, sb, formatString, formatFlags);
                case IEnumerable<IPAddress> ipAddressArray: return FormatEnumerable(ipAddressArray, sb, formatString, formatFlags);
                case IEnumerable<Uri> uriArray:             return FormatEnumerable(uriArray, sb, formatString, formatFlags);
            }
        }
        else if (enumerableElementType is { IsValueType: true, IsGenericType: true } &&
                 enumerableElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<byte?> nullByteArray:           return FormatEnumerable(nullByteArray, sb, formatString, formatFlags);
                case IEnumerable<sbyte?> nullSbyteArray:         return FormatEnumerable(nullSbyteArray, sb, formatString, formatFlags);
                case IEnumerable<char?> nullCharArray:           return FormatEnumerable(nullCharArray, sb, formatString, formatFlags);
                case IEnumerable<short?> nullShortArray:         return FormatEnumerable(nullShortArray, sb, formatString, formatFlags);
                case IEnumerable<ushort?> nullUShortArray:       return FormatEnumerable(nullUShortArray, sb, formatString, formatFlags);
                case IEnumerable<Half?> nullHalfFloatArray:      return FormatEnumerable(nullHalfFloatArray, sb, formatString, formatFlags);
                case IEnumerable<int?> nullIntArray:             return FormatEnumerable(nullIntArray, sb, formatString, formatFlags);
                case IEnumerable<uint?> nullUIntArray:           return FormatEnumerable(nullUIntArray, sb, formatString, formatFlags);
                case IEnumerable<nint?> nullNIntArray:           return FormatEnumerable(nullNIntArray, sb, formatString, formatFlags);
                case IEnumerable<float?> nullFloatArray:         return FormatEnumerable(nullFloatArray, sb, formatString, formatFlags);
                case IEnumerable<long?> nullLongArray:           return FormatEnumerable(nullLongArray, sb, formatString, formatFlags);
                case IEnumerable<ulong?> nullULongArray:         return FormatEnumerable(nullULongArray, sb, formatString, formatFlags);
                case IEnumerable<double?> nullDoubleArray:       return FormatEnumerable(nullDoubleArray, sb, formatString, formatFlags);
                case IEnumerable<decimal?> nullDecimalArray:     return FormatEnumerable(nullDecimalArray, sb, formatString, formatFlags);
                case IEnumerable<Int128?> nullVeryLongArray:     return FormatEnumerable(nullVeryLongArray, sb, formatString, formatFlags);
                case IEnumerable<UInt128?> nullVeryULongArray:   return FormatEnumerable(nullVeryULongArray, sb, formatString, formatFlags);
                case IEnumerable<BigInteger?> nullBigIntArray:   return FormatEnumerable(nullBigIntArray, sb, formatString, formatFlags);
                case IEnumerable<Complex?> nullComplexNumArray:  return FormatEnumerable(nullComplexNumArray, sb, formatString, formatFlags);
                case IEnumerable<DateTime?> nullDateTimeArray:   return FormatEnumerable(nullDateTimeArray, sb, formatString, formatFlags);
                case IEnumerable<DateOnly?> nullDateOnlyArray:   return FormatEnumerable(nullDateOnlyArray, sb, formatString, formatFlags);
                case IEnumerable<TimeSpan?> nullTimeSpanArray:   return FormatEnumerable(nullTimeSpanArray, sb, formatString, formatFlags);
                case IEnumerable<TimeOnly?> nullTimeOnlyArray:   return FormatEnumerable(nullTimeOnlyArray, sb, formatString, formatFlags);
                case IEnumerable<Rune?> nullRuneArray:           return FormatEnumerable(nullRuneArray, sb, formatString, formatFlags);
                case IEnumerable<Guid?> nullGuidArray:           return FormatEnumerable(nullGuidArray, sb, formatString, formatFlags);
                case IEnumerable<IPNetwork?> nullIpNetworkArray: return FormatEnumerable(nullIpNetworkArray, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableList<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var listType = type.GetListElementType()!;

        if (listType.IsSpanFormattable())
        {
            switch (source)
            {
                case IReadOnlyList<byte> byteArray:           return FormatList(byteArray, sb, formatString, formatFlags);
                case IReadOnlyList<sbyte> sbyteArray:         return FormatList(sbyteArray, sb, formatString, formatFlags);
                case IReadOnlyList<char> charArray:           return FormatList(charArray, sb, formatString, formatFlags);
                case IReadOnlyList<short> shortArray:         return FormatList(shortArray, sb, formatString, formatFlags);
                case IReadOnlyList<ushort> uShortArray:       return FormatList(uShortArray, sb, formatString, formatFlags);
                case IReadOnlyList<Half> halfFloatArray:      return FormatList(halfFloatArray, sb, formatString, formatFlags);
                case IReadOnlyList<int> intArray:             return FormatList(intArray, sb, formatString, formatFlags);
                case IReadOnlyList<uint> uIntArray:           return FormatList(uIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<nint> nIntArray:           return FormatList(nIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<float> floatArray:         return FormatList(floatArray, sb, formatString, formatFlags);
                case IReadOnlyList<long> longArray:           return FormatList(longArray, sb, formatString, formatFlags);
                case IReadOnlyList<ulong> uLongArray:         return FormatList(uLongArray, sb, formatString, formatFlags);
                case IReadOnlyList<double> doubleArray:       return FormatList(doubleArray, sb, formatString, formatFlags);
                case IReadOnlyList<decimal> decimalArray:     return FormatList(decimalArray, sb, formatString, formatFlags);
                case IReadOnlyList<Int128> veryLongArray:     return FormatList(veryLongArray, sb, formatString, formatFlags);
                case IReadOnlyList<UInt128> veryULongArray:   return FormatList(veryULongArray, sb, formatString, formatFlags);
                case IReadOnlyList<BigInteger> bigIntArray:   return FormatList(bigIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<Complex> complexNumArray:  return FormatList(complexNumArray, sb, formatString, formatFlags);
                case IReadOnlyList<DateTime> dateTimeArray:   return FormatList(dateTimeArray, sb, formatString, formatFlags);
                case IReadOnlyList<DateOnly> dateOnlyArray:   return FormatList(dateOnlyArray, sb, formatString, formatFlags);
                case IReadOnlyList<TimeSpan> timeSpanArray:   return FormatList(timeSpanArray, sb, formatString, formatFlags);
                case IReadOnlyList<TimeOnly> timeOnlyArray:   return FormatList(timeOnlyArray, sb, formatString, formatFlags);
                case IReadOnlyList<Rune> runeArray:           return FormatList(runeArray, sb, formatString, formatFlags);
                case IReadOnlyList<Guid> guidArray:           return FormatList(guidArray, sb, formatString, formatFlags);
                case IReadOnlyList<IPNetwork> ipNetworkArray: return FormatList(ipNetworkArray, sb, formatString, formatFlags);
                case IReadOnlyList<Enum> enumArray:           return FormatList(enumArray, sb, formatString, formatFlags);
                case IReadOnlyList<Version> versionArray:     return FormatList(versionArray, sb, formatString, formatFlags);
                case IReadOnlyList<IPAddress> ipAddressArray: return FormatList(ipAddressArray, sb, formatString, formatFlags);
                case IReadOnlyList<Uri> uriArray:             return FormatList(uriArray, sb, formatString, formatFlags);
            }
        }
        else if (listType is { IsValueType: true, IsGenericType: true } && listType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IReadOnlyList<byte?> nullByteArray:           return FormatList(nullByteArray, sb, formatString, formatFlags);
                case IReadOnlyList<sbyte?> nullSbyteArray:         return FormatList(nullSbyteArray, sb, formatString, formatFlags);
                case IReadOnlyList<char?> nullCharArray:           return FormatList(nullCharArray, sb, formatString, formatFlags);
                case IReadOnlyList<short?> nullShortArray:         return FormatList(nullShortArray, sb, formatString, formatFlags);
                case IReadOnlyList<ushort?> nullUShortArray:       return FormatList(nullUShortArray, sb, formatString, formatFlags);
                case IReadOnlyList<Half?> nullHalfFloatArray:      return FormatList(nullHalfFloatArray, sb, formatString, formatFlags);
                case IReadOnlyList<int?> nullIntArray:             return FormatList(nullIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<uint?> nullUIntArray:           return FormatList(nullUIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<nint?> nullNIntArray:           return FormatList(nullNIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<float?> nullFloatArray:         return FormatList(nullFloatArray, sb, formatString, formatFlags);
                case IReadOnlyList<long?> nullLongArray:           return FormatList(nullLongArray, sb, formatString, formatFlags);
                case IReadOnlyList<ulong?> nullULongArray:         return FormatList(nullULongArray, sb, formatString, formatFlags);
                case IReadOnlyList<double?> nullDoubleArray:       return FormatList(nullDoubleArray, sb, formatString, formatFlags);
                case IReadOnlyList<decimal?> nullDecimalArray:     return FormatList(nullDecimalArray, sb, formatString, formatFlags);
                case IReadOnlyList<Int128?> nullVeryLongArray:     return FormatList(nullVeryLongArray, sb, formatString, formatFlags);
                case IReadOnlyList<UInt128?> nullVeryULongArray:   return FormatList(nullVeryULongArray, sb, formatString, formatFlags);
                case IReadOnlyList<BigInteger?> nullBigIntArray:   return FormatList(nullBigIntArray, sb, formatString, formatFlags);
                case IReadOnlyList<Complex?> nullComplexNumArray:  return FormatList(nullComplexNumArray, sb, formatString, formatFlags);
                case IReadOnlyList<DateTime?> nullDateTimeArray:   return FormatList(nullDateTimeArray, sb, formatString, formatFlags);
                case IReadOnlyList<DateOnly?> nullDateOnlyArray:   return FormatList(nullDateOnlyArray, sb, formatString, formatFlags);
                case IReadOnlyList<TimeSpan?> nullTimeSpanArray:   return FormatList(nullTimeSpanArray, sb, formatString, formatFlags);
                case IReadOnlyList<TimeOnly?> nullTimeOnlyArray:   return FormatList(nullTimeOnlyArray, sb, formatString, formatFlags);
                case IReadOnlyList<Rune?> nullRuneArray:           return FormatList(nullRuneArray, sb, formatString, formatFlags);
                case IReadOnlyList<Guid?> nullGuidArray:           return FormatList(nullGuidArray, sb, formatString, formatFlags);
                case IReadOnlyList<IPNetwork?> nullIpNetworkArray: return FormatList(nullIpNetworkArray, sb, formatString, formatFlags);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableArray<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var elementType = type.GetElementType()!;
        if (elementType.IsSpanFormattable())
        {
            switch (source)
            {
                case byte[] byteArray:           return FormatArray(byteArray, sb, formatString, formatFlags);
                case sbyte[] sbyteArray:         return FormatArray(sbyteArray, sb, formatString, formatFlags);
                case char[] charArray:           return FormatArray(charArray, sb, formatString, formatFlags);
                case short[] shortArray:         return FormatArray(shortArray, sb, formatString, formatFlags);
                case ushort[] uShortArray:       return FormatArray(uShortArray, sb, formatString, formatFlags);
                case Half[] halfFloatArray:      return FormatArray(halfFloatArray, sb, formatString, formatFlags);
                case int[] intArray:             return FormatArray(intArray, sb, formatString, formatFlags);
                case uint[] uIntArray:           return FormatArray(uIntArray, sb, formatString, formatFlags);
                case nint[] nIntArray:           return FormatArray(nIntArray, sb, formatString, formatFlags);
                case float[] floatArray:         return FormatArray(floatArray, sb, formatString, formatFlags);
                case long[] longArray:           return FormatArray(longArray, sb, formatString, formatFlags);
                case ulong[] uLongArray:         return FormatArray(uLongArray, sb, formatString, formatFlags);
                case double[] doubleArray:       return FormatArray(doubleArray, sb, formatString, formatFlags);
                case decimal[] decimalArray:     return FormatArray(decimalArray, sb, formatString, formatFlags);
                case Int128[] veryLongArray:     return FormatArray(veryLongArray, sb, formatString, formatFlags);
                case UInt128[] veryULongArray:   return FormatArray(veryULongArray, sb, formatString, formatFlags);
                case BigInteger[] bigIntArray:   return FormatArray(bigIntArray, sb, formatString, formatFlags);
                case Complex[] complexNumArray:  return FormatArray(complexNumArray, sb, formatString, formatFlags);
                case DateTime[] dateTimeArray:   return FormatArray(dateTimeArray, sb, formatString, formatFlags);
                case DateOnly[] dateOnlyArray:   return FormatArray(dateOnlyArray, sb, formatString, formatFlags);
                case TimeSpan[] timeSpanArray:   return FormatArray(timeSpanArray, sb, formatString, formatFlags);
                case TimeOnly[] timeOnlyArray:   return FormatArray(timeOnlyArray, sb, formatString, formatFlags);
                case Rune[] runeArray:           return FormatArray(runeArray, sb, formatString, formatFlags);
                case Guid[] guidArray:           return FormatArray(guidArray, sb, formatString, formatFlags);
                case IPNetwork[] ipNetworkArray: return FormatArray(ipNetworkArray, sb, formatString, formatFlags);
                case Enum[] enumArray:           return FormatArray(enumArray, sb, formatString, formatFlags);
                case Version[] versionArray:     return FormatArray(versionArray, sb, formatString, formatFlags);
                case IPAddress[] ipAddressArray: return FormatArray(ipAddressArray, sb, formatString, formatFlags);
                case Uri[] uriArray:             return FormatArray(uriArray, sb, formatString, formatFlags);
            }
        }
        else if (elementType is { IsValueType: true, IsGenericType: true } && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
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

    public abstract int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

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
    
    public abstract int CollectionEnd(Type collectionType, Span<char> destSpan, int index, int totalItemCount
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);


    public override void StateReset()
    {
        (EncoderTransfer as IRecyclableObject)?.DecrementRefCount();
        base.StateReset();
    }
}
