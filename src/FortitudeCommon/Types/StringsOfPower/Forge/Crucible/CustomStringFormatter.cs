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
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

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
    protected IEncodingTransfer?  StringEncoderTransfer;
    protected IEncodingTransfer?  JoinEncoderTransfer;


    static CustomStringFormatter() { }

    public virtual FormatStyle FormattingStyle => FormatStyle.None;

    protected MutableString? CharSpanCollectionScratchBuffer { get; set; }

    protected int LastFormatExceptionBuilderLength { get; set; }

    public virtual IFormattingOptions Options
    {
        get => FormatOptions ??= new FormattingOptions.FormattingOptions();
        set { FormatOptions = value; }
    }

    public virtual IEncodingTransfer ContentEncoder
    {
        get => StringEncoderTransfer ??= PassThroughEncodingTransfer.Instance;
        set => StringEncoderTransfer = value;
    }

    public virtual IEncodingTransfer LayoutEncoder
    {
        get => JoinEncoderTransfer ??= PassThroughEncodingTransfer.Instance;
        set => JoinEncoderTransfer = value;
    }

    protected virtual IEncodingTransfer ResolveContentEncoderFor<T>(T toEncode, FormatSwitches formatSwitches) =>
        (formatSwitches.DoesNotHaveEncodeInnerContent() ? LayoutEncoder : ContentEncoder);

    protected virtual IEncodingTransfer ResolveContentEncoderFor(ReadOnlySpan<char> toEncode, FormatSwitches formatSwitches) =>
        (formatSwitches.DoesNotHaveEncodeInnerContent() ? LayoutEncoder : ContentEncoder);

    protected virtual IEncodingTransfer ResolveBoundsEncoder(FormatSwitches formatSwitches) => ContentEncoder;

    public virtual FormatSwitches ResolveStringFormattingFlags<T>(char lastNonWhiteSpace, T input
      , FormatSwitches callerFormattingFlags, string formatString = "") =>
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
            return ContentEncoder.StringValueDelimiter(sb);
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
            return ContentEncoder.StringValueDelimiter(sb);
        }
        return 0;
    }

    public virtual int AppendDelimiterStart(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray()) { return destSpan.OverWriteAt(fromIndex, SqBrktOpn); }
            return ContentEncoder.StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }

    public virtual int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex)
    {
        if (maybeDelimited.IsAnyTypeHoldingChars())
        {
            if (maybeDelimited.IsCharArray()) { return destSpan.OverWriteAt(fromIndex, SqBrktCls); }
            return ContentEncoder.StringValueDelimiter(destSpan, fromIndex);
        }
        return 0;
    }


    public virtual int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        var preAppendLen = sb.Length;
        sb.Append(formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return sb.Length - preAppendLen;
    }

    public virtual int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemPaddingFlag()) return 0;
        var preAppendLen = sb.Length;
        sb.Append(formatSwitches.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return sb.Length - preAppendLen;
    }

    public virtual int AddCollectionElementSeparatorAndPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        AddCollectionElementSeparator(collectionType, sb, nextItemNumber, formatSwitches);
        return AddCollectionElementPadding(collectionType, sb, nextItemNumber, formatSwitches);
    }

    public virtual int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        var charsAdded = 0;
        charsAdded
            += destSpan.OverWriteAt(atIndex, formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return charsAdded;
    }

    public virtual int AddCollectionElementPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemPaddingFlag()) return 0;
        var charsAdded = 0;
        charsAdded += destSpan.OverWriteAt(atSpanOffset
                                         , formatSwitches.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return charsAdded;
    }

    public virtual int AddCollectionElementSeparatorAndPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var charsAdded = AddCollectionElementSeparator(collectionType, destSpan, atSpanOffset, nextItemNumber, formatSwitches);
        return AddCollectionElementPadding(collectionType, destSpan, atSpanOffset + charsAdded, nextItemNumber, formatSwitches) + charsAdded;
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
      , int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var extendLengthRange
           , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded +=
                ResolveBoundsEncoder(formatSwitches)
                    .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, sb);
        source = source[sourceFrom..(sourceFrom + cappedLength)];

        extendLengthRange = extendLengthRange.BoundRangeToLength(cappedLength);
        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            charsAdded +=
                ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(source[extendLengthRange], 0, sb, maxTransferCount: cappedLength);
            if (suffix.Length > 0)
                charsAdded +=
                    ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResultSpan, source, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize    =  padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize    =  Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { source = source[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, alignedLength);
                charsAdded +=
                    ResolveContentEncoderFor(source, formatSwitches)
                        .Transfer(padSpan[..padSize], sb);
            }

            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResult, source, ResolveContentEncoderFor(source, formatSwitches)
                                  , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize    =  Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { source = source[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, cappedLength);
                charsAdded +=
                    ResolveContentEncoderFor(source, formatSwitches)
                        .Transfer(padSpan[..padSize], sb);
            }

            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded +=
                    ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches).Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        return Format(((ReadOnlySpan<char>)source)[sourceFrom..], 0, sb, formatString, maxTransferCount, formatSwitches);
    }

    public virtual int Format(StringBuilder source, int sourceFrom, IStringBuilder sb
      , ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, sourceFrom, sb, sb.Length, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded +=
                ResolveBoundsEncoder(formatSwitches)
                    .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, sb);

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
            charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, sourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResultSpan, sourceInSpan, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize    =  padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize    =  Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize    =  padSpan.PadAndAlign(sourceInSpan, layout);
                padSize    =  Math.Min(padSize, alignedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
            }

            if (suffix.Length > 0)
                charsAdded +=
                    ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResult, sourceInSpan, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize    =  Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize    =  padSpan.PadAndAlign(sourceInSpan, layout);
                padSize    =  Math.Min(padSize, cappedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
            }

            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded +=
                    ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString
      , int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches).Transfer(this, source, sourceFrom, sb, maxTransferCount: cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded += ResolveBoundsEncoder(formatSwitches)
                .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, sb);

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
            charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(this, source, rawSourceFrom, sb, maxTransferCount: rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResultSpan, sourceInSpan, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];

                padSize    =  padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize    =  Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                padSize    =  padSpan.PadAndAlign(sourceInSpan, layout);
                padSize    =  Math.Min(padSize, alignedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
            }

            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResult, sourceInSpan, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize    =  Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer.Instance.Transfer(padSpan[..padSize], sb);
            }
            else
            {
                padSize    =  padSpan.PadAndAlign(sourceInSpan, layout);
                padSize    =  Math.Min(padSize, cappedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
            }

            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded +=
                    ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var extendLengthRange
           , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded += ResolveBoundsEncoder(formatSwitches)
                .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough
                              , prefix, destCharSpan, destStartIndex);

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
            charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, 0, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResultSpan, source, ResolveContentEncoderFor(source, formatSwitches)
                                  , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];

                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, alignedLength);

                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange
                    .ApplySplitJoin(splitJoinResult, source, ResolveContentEncoderFor(source, formatSwitches)
                    , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                padSize = padSpan.PadAndAlign(source, layout);
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            padAlignBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0 && formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString)) return 0;
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(this, source, sourceFrom, destCharSpan, destStartIndex, cappedLength);

        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        return Format(((ReadOnlySpan<char>)source)[sourceFrom..], 0, destCharSpan, formatString, destStartIndex
                    , cappedLength, formatSwitches);
    }

    public virtual int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches).Transfer(source, destCharSpan, destStartIndex);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var extendLengthRange
           , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded += ResolveBoundsEncoder(formatSwitches)
                .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough
                              , prefix, destCharSpan, destStartIndex);

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
            charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange.ApplySplitJoin
                    (splitJoinResultSpan, sourceInSpan
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResultSpan = splitJoinResultSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange.ApplySplitJoin
                    (splitJoinResult, sourceInSpan
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                if (!extendLengthRange.IsAllRange()) { splitJoinResult = splitJoinResult[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                if (!extendLengthRange.IsAllRange()) { sourceInSpan = sourceInSpan[extendLengthRange]; }
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        sourceFrom       = Math.Clamp(sourceFrom, 0, source.Length);
        maxTransferCount = Math.Clamp(maxTransferCount, 0, source.Length);
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (formatString.Length == 0 || formatString.SequenceMatches(NoFormatFormatString))
            return ResolveContentEncoderFor(source, formatSwitches).Transfer(this, source, destCharSpan, destStartIndex, maxTransferCount);

        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var extendLengthRange
                                                     , out var layout, out var splitJoinRange, out _, out var suffix);
        if (prefix.Length > 0)
            charsAdded += ResolveBoundsEncoder(formatSwitches)
                .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough
                              , prefix, destCharSpan, destStartIndex);

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
            charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                .Transfer(this, source, rawSourceFrom, destCharSpan, destStartIndex + charsAdded, rawCappedLength);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange.ApplySplitJoin
                    (splitJoinResultSpan, sourceInSpan, ResolveContentEncoderFor(source, formatSwitches)
                   , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResultSpan = splitJoinResultSpan[..size];

                padSize = padSpan.PadAndAlign(splitJoinResultSpan, layout);
                padSize = Math.Min(padSize, alignedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, alignedLength);

                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
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

                var size = splitJoinRange.ApplySplitJoin
                    (splitJoinResult, sourceInSpan
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , formatSwitches.DoesNotHaveReformatMultiLineFlag() ? ContentEncoder : LayoutEncoder);
                splitJoinResult = splitJoinResult[..size];
                padSize         = padSpan.PadAndAlign(splitJoinResult, layout);
                splitJoinBuffer.DecrementRefCount();
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += PassThroughEncodingTransfer
                              .Instance
                              .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }
            else
            {
                padSize = padSpan.PadAndAlign(sourceInSpan, layout);
                padSize = Math.Min(padSize, cappedLength);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                    .Transfer(padSpan[..padSize], destCharSpan, destStartIndex + charsAdded);
            }

            padAlignBuffer.DecrementRefCount();
            sourceBuffer.DecrementRefCount();
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
    }

    public virtual int Format<TFmt>(TFmt source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        int charsWritten;
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            var charSpan = stackalloc char[2048].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString
                                   , ResolveContentEncoderFor(source, formatSwitches)
                                   , LayoutEncoder, null, formatSwitches);
            PassThroughEncodingTransfer.Instance.Transfer(charSpan[..charsWritten], sb);
            return charsWritten;
        }
        if (source is Enum sourceEnum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateEnumFormatProvider(source.GetType());
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);

            var charSpan               = stackalloc char[2048].ResetMemory();
            var spanEnumFormatProvider = enumFormatProvider.AsSpanFormattableEnumFormatProvider<TFmt>();

            Span<char> formatStringModified = stackalloc char[formatString.Length + 2];
            if (Options.EnumsDefaultAsNumber && !formatString.FormatStringHasFormatSequence())
            {
                formatStringModified = formatStringModified.InjectFormatSequenceIntoExistingFormatString(formatString, "d");
            }
            else
            {
                formatStringModified.Append(formatString);
                formatStringModified = formatStringModified[..formatString.Length];
            }
            if (spanEnumFormatProvider != null)
            {
                charsWritten = spanEnumFormatProvider.StringBearerSpanFormattable
                    (source, charSpan, formatStringModified
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , formatSwitches.HasContentTreatmentFlags() ? ContentEncoder : LayoutEncoder, null, formatSwitches);
            }
            else
            {
                var asEnumFormatter = enumFormatProvider.AsEnumFormatProvider()!;
                charsWritten = asEnumFormatter.StringBearerSpanFormattable
                    (sourceEnum, charSpan, formatStringModified
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , LayoutEncoder, null, formatSwitches);
            }
            PassThroughEncodingTransfer.Instance.Transfer(charSpan[..charsWritten], sb);
            return charsWritten;
        }
        try
        {
            var charSpan   = stackalloc char[2048].ResetMemory();
            var charsAdded = 0;
            formatString.ExtractExtendedStringFormatStages(out var prefix, out _, out var outputSubRange
                                                         , out var layout, out _, out var format, out var suffix);
            if (prefix.Length > 0)
                charsAdded = ResolveBoundsEncoder(formatSwitches)
                    .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, sb);
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
                    charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(toTransfer, sb);
                    if (suffix.Length > 0)
                        charsAdded += ResolveBoundsEncoder(formatSwitches)
                            .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
                    return charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
                if (suffix.Length > 0)
                    charsAdded += ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
                return charsAdded;
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
                    LastFormatExceptionBuilderLength = -1;
                }
            }
            charsWritten = sb.Length - sbPreAppendLen;
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable?
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString);
        int charsWritten;
        var charSpan = stackalloc char[2048].ResetMemory();
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            charsWritten = formatter
                (source, charSpan, formatString
               , ResolveContentEncoderFor(source, formatSwitches)
               , LayoutEncoder, null, formatSwitches);
            return PassThroughEncodingTransfer.Instance.Transfer(charSpan[..charsWritten], destCharSpan, destStartIndex);
        }
        if (source is Enum sourceEnum)
        {
            var enumType           = source.GetType();
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateEnumFormatProvider(enumType);
            TryAddCustomSpanFormattableProvider(enumType, enumFormatProvider);
            var        spanEnumFormatProver = enumFormatProvider.AsSpanFormattableEnumFormatProvider<TFmt>();
            Span<char> formatStringModified = stackalloc char[formatString.Length + 2];
            if (Options.EnumsDefaultAsNumber && !formatString.FormatStringHasFormatSequence())
            {
                formatStringModified = formatStringModified.InjectFormatSequenceIntoExistingFormatString(formatString, "d");
            }
            else
            {
                formatStringModified.Append(formatString);
                formatStringModified = formatStringModified[..formatString.Length];
            }
            if (spanEnumFormatProver != null)
            {
                charsWritten = spanEnumFormatProver.StringBearerSpanFormattable
                    (source, charSpan, formatStringModified
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , LayoutEncoder, null, formatSwitches);
            }
            else
            {
                var asEnumFormatter = enumFormatProvider.AsEnumFormatProvider()!;
                charsWritten = asEnumFormatter.StringBearerSpanFormattable
                    (sourceEnum, charSpan, formatStringModified
                   , ResolveContentEncoderFor(source, formatSwitches)
                   , LayoutEncoder, null, formatSwitches);
            }
            return PassThroughEncodingTransfer.Instance.Transfer(charSpan[..charsWritten], destCharSpan, destStartIndex);
        }
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var outputSubRange
           , out var layout, out _, out var format, out var suffix);
        try
        {
            var charsAdded = 0;
            if (prefix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferPrefix
                        (formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, destCharSpan
                       , destStartIndex);
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
                    charsAdded += ResolveContentEncoderFor(source, formatSwitches)
                        .Transfer(toTransfer, destCharSpan, destStartIndex + charsAdded);

                    if (suffix.Length > 0)
                        charsAdded += ResolveBoundsEncoder(formatSwitches).TransferSuffix
                            (suffix, destCharSpan, destStartIndex + charsAdded, formatSwitches.HasEncodeBoundsFlag());
                    return charsAdded;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);

                charsAdded += ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], destCharSpan, destStartIndex);

                if (suffix.Length > 0)
                    charsAdded += ResolveBoundsEncoder(formatSwitches)
                        .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                      , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
                return charsAdded;
            }
        }
        catch (FormatException) { }
        if (source.TryFormat(charSpan, out charsWritten, "", null))
        {
            try
            {
                var toTransfer = charSpan[..charsWritten];
                if (!outputSubRange.IsAllRange())
                {
                    outputSubRange.BoundRangeToLength(toTransfer.Length);
                    toTransfer = toTransfer[outputSubRange];
                }
                if (layout.Length == 0)
                {
                    return ResolveContentEncoderFor(source, formatSwitches)
                        .Transfer(toTransfer, destCharSpan, destStartIndex);
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(toTransfer, layout);
                charsWritten = ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], destCharSpan, destStartIndex);
            }
            catch (FormatException) { }
        }
        return charsWritten;
    }

    public virtual int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : struct, ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return Format(source.Value, sb, formatString, formatSwitches);
    }

    public virtual int Format<TFmt>(TFmt? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : struct, ISpanFormattable
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return Format(source.Value, destCharSpan, destStartIndex, formatString, formatSwitches);
    }

    public int Format(bool source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var charSpan   = stackalloc char[16].ResetMemory();
        var charsAdded = 0;
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var outputSubRange
           , out var layout, out _, out _, out var suffix);

        if (prefix.Length > 0)
            charsAdded += ResolveBoundsEncoder(formatSwitches)
                .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough, prefix, sb);
        var charsWritten = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);
        var toTransfer   = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            ResolveContentEncoderFor(source, formatSwitches).Transfer(toTransfer, sb);
            if (suffix.Length > 0)
                charsAdded += ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
            return charsWritten + charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], sb);
        if (suffix.Length > 0)
            charsAdded +=
                ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, sb, formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
        return padSize + charsAdded;
    }

    public int Format(bool source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var charSpan = stackalloc char[16].ResetMemory();
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var outputSubRange
           , out var layout, out _, out _, out var suffix);
        var charsAdded = 0;
        if (prefix.Length > 0)
            charsAdded +=
                ResolveBoundsEncoder(formatSwitches)
                    .TransferPrefix(formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough
                                  , prefix, destCharSpan, destStartIndex);
        var charsWritten = charSpan.AppendReturnAddCount(source ? Options.True : Options.False);

        var toTransfer = charSpan[..charsWritten];
        if (!outputSubRange.IsAllRange())
        {
            outputSubRange.BoundRangeToLength(toTransfer.Length);
            toTransfer = toTransfer[outputSubRange];
        }
        if (layout.Length == 0)
        {
            ResolveContentEncoderFor(source, formatSwitches).Transfer(toTransfer, destCharSpan, destStartIndex + charsAdded);
            charsAdded += charsWritten;
            if (suffix.Length > 0) charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, suffix);
            return charsAdded;
        }
        var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
        var padSize = padSpan.PadAndAlign(toTransfer, layout);
        ResolveContentEncoderFor(source, formatSwitches).Transfer(padSpan[..padSize], destCharSpan, destStartIndex);
        charsAdded += padSize;
        if (suffix.Length > 0)
            charsAdded +=
                ResolveBoundsEncoder(formatSwitches)
                    .TransferSuffix(suffix, destCharSpan, destStartIndex + charsAdded
                                  , formatSwitches.HasEncodeBoundsFlag() || LayoutEncoder.Type != EncodingType.PassThrough);
        return charsAdded;
    }

    public int Format(bool? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return Format(source.Value, sb, formatString, formatSwitches);
    }

    public int Format(bool? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (source == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return Format(source.Value, destCharSpan, destStartIndex, formatString, formatSwitches);
    }

    public int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString,
        FormatSwitches formatSwitches = EncodeInnerContent)
    {
        int NeverZero(int check) => check == 0 ? -1 : check;

        if (source == null) return 0;
        if (source is bool boolSource) { return NeverZero(Format(boolSource, sb, formatString)); }
        if (source is ISpanFormattable formattable) { return NeverZero(Format(formattable, sb, formatString)); }
        var type                     = source.GetType();
        var maybeIterableElementType = type.GetIterableElementType();
        if (maybeIterableElementType == null) return 0;
        if (!((maybeIterableElementType.IsSpanFormattableOrNullable() || maybeIterableElementType.IsBoolOrNullable()))) return 0;
        if (source is String stringSource) { return NeverZero(Format(stringSource, 0, sb, formatString, formatSwitches: formatSwitches)); }
        if (source is char[] charArraySource) { return NeverZero(Format(charArraySource, 0, sb, formatString, formatSwitches: formatSwitches)); }
        if (source is StringBuilder stringBuilderSource)
        {
            return NeverZero(Format(stringBuilderSource, 0, sb, formatString, formatSwitches: formatSwitches));
        }
        if (source is ICharSequence charSeqSource) { return NeverZero(Format(charSeqSource, 0, sb, formatString)); }
        if (type.IsArray) { return NeverZero(CheckIsKnownSpanFormattableArray(source, sb, formatString, type, formatSwitches)); }
        if (type.IsReadOnlyList()) { return NeverZero(CheckIsKnownSpanFormattableList(source, sb, formatString, type, formatSwitches)); }
        if (type.IsEnumerable()) { return NeverZero(CheckIsKnownSpanFormattableEnumerable(source, sb, formatString, type, formatSwitches)); }
        if (type.IsEnumerator()) { return NeverZero(CheckIsKnownSpanFormattableEnumerator(source, sb, formatString, type, formatSwitches)); }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerator<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var enumeratorElementType = type.IfEnumeratorGetElementType()!;

        if (enumeratorElementType.IsSpanFormattable() || enumeratorElementType.IsBool())
        {
            switch (source)
            {
                case IEnumerable<bool?> boolEnumerator: return FormatBoolEnumerator(boolEnumerator.GetEnumerator(), sb, formatString, formatSwitches);
                case IEnumerator<byte> byteEnumerator: return FormatEnumerator(byteEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<sbyte> sbyteEnumerator: return FormatEnumerator(sbyteEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<char> charEnumerator: return FormatEnumerator(charEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<short> shortEnumerator: return FormatEnumerator(shortEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<ushort> uShortEnumerator: return FormatEnumerator(uShortEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Half> halfFloatEnumerator: return FormatEnumerator(halfFloatEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<int> intEnumerator: return FormatEnumerator(intEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<uint> uIntEnumerator: return FormatEnumerator(uIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<nint> nIntEnumerator: return FormatEnumerator(nIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<float> floatEnumerator: return FormatEnumerator(floatEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<long> longEnumerator: return FormatEnumerator(longEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<ulong> uLongEnumerator: return FormatEnumerator(uLongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<double> doubleEnumerator: return FormatEnumerator(doubleEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<decimal> decimalEnumerator: return FormatEnumerator(decimalEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Int128> veryLongEnumerator: return FormatEnumerator(veryLongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<UInt128> veryUlongEnumerator: return FormatEnumerator(veryUlongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<BigInteger> bigIntEnumerator: return FormatEnumerator(bigIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Complex> complexNumEnumerator: return FormatEnumerator(complexNumEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<DateTime> dateTimeEnumerator: return FormatEnumerator(dateTimeEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<DateOnly> dateOnlyEnumerator: return FormatEnumerator(dateOnlyEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<TimeSpan> timeSpanEnumerator: return FormatEnumerator(timeSpanEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<TimeOnly> timeOnlyEnumerator: return FormatEnumerator(timeOnlyEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Rune> runeEnumerator: return FormatEnumerator(runeEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Guid> guidEnumerator: return FormatEnumerator(guidEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<IPNetwork> ipNetworkEnumerator: return FormatEnumerator(ipNetworkEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Enum> enumEnumerator: return FormatEnumerator(enumEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Version> versionEnumerator: return FormatEnumerator(versionEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<IPAddress> ipAddressEnumerator: return FormatEnumerator(ipAddressEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Uri> uriEnumerator: return FormatEnumerator(uriEnumerator, sb, formatString, formatSwitches);
            }
        }
        else if (enumeratorElementType is { IsValueType: true, IsGenericType: true } &&
                 enumeratorElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<bool?> nullBoolEnumerator:
                    return FormatBoolEnumerator(nullBoolEnumerator.GetEnumerator(), sb, formatString, formatSwitches);
                case IEnumerator<byte?> nullByteEnumerator:       return FormatEnumerator(nullByteEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<sbyte?> nullSbyteEnumerator:     return FormatEnumerator(nullSbyteEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<char?> nullCharEnumerator:       return FormatEnumerator(nullCharEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<short?> nullShortEnumerator:     return FormatEnumerator(nullShortEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<ushort?> nullUShortEnumerator:   return FormatEnumerator(nullUShortEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Half?> nullHalfFloatEnumerator:  return FormatEnumerator(nullHalfFloatEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<int?> nullIntEnumerator:         return FormatEnumerator(nullIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<uint?> nullUIntEnumerator:       return FormatEnumerator(nullUIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<nint?> nullNIntEnumerator:       return FormatEnumerator(nullNIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<float?> nullFloatEnumerator:     return FormatEnumerator(nullFloatEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<long?> nullLongEnumerator:       return FormatEnumerator(nullLongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<ulong?> nullULongEnumerator:     return FormatEnumerator(nullULongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<double?> nullDoubleEnumerator:   return FormatEnumerator(nullDoubleEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<decimal?> nullDecimalEnumerator: return FormatEnumerator(nullDecimalEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Int128?> nullVeryLongEnumerator: return FormatEnumerator(nullVeryLongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<UInt128?> nullVeryUlongEnumerator:
                    return FormatEnumerator(nullVeryUlongEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<BigInteger?> nullBigIntEnumerator: return FormatEnumerator(nullBigIntEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Complex?> nullComplexNumEnumerator:
                    return FormatEnumerator(nullComplexNumEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<DateTime?> nullDateTimeEnumerator: return FormatEnumerator(nullDateTimeEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<DateOnly?> nullDateOnlyEnumerator: return FormatEnumerator(nullDateOnlyEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<TimeSpan?> nullTimeSpanEnumerator: return FormatEnumerator(nullTimeSpanEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<TimeOnly?> nullTimeOnlyEnumerator: return FormatEnumerator(nullTimeOnlyEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Rune?> nullRuneEnumerator:         return FormatEnumerator(nullRuneEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<Guid?> nullGuidEnumerator:         return FormatEnumerator(nullGuidEnumerator, sb, formatString, formatSwitches);
                case IEnumerator<IPNetwork?> nullIpNetworkEnumerator:
                    return FormatEnumerator(nullIpNetworkEnumerator, sb, formatString, formatSwitches);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableEnumerable<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var enumerableElementType = type.IfEnumerableGetElementType()!;

        if (enumerableElementType.IsSpanFormattable() || enumerableElementType.IsBool())
        {
            switch (source)
            {
                case IEnumerable<bool?> boolEnumerable: return FormatBoolEnumerator(boolEnumerable.GetEnumerator(), sb, formatString, formatSwitches);
                case IEnumerable<byte> byteEnumerable: return FormatEnumerable(byteEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<sbyte> sbyteEnumerable: return FormatEnumerable(sbyteEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<char> charEnumerable: return FormatEnumerable(charEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<short> shortEnumerable: return FormatEnumerable(shortEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<ushort> uShortEnumerable: return FormatEnumerable(uShortEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Half> halfFloatEnumerable: return FormatEnumerable(halfFloatEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<int> intEnumerable: return FormatEnumerable(intEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<uint> uIntEnumerable: return FormatEnumerable(uIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<nint> nIntEnumerable: return FormatEnumerable(nIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<float> floatEnumerable: return FormatEnumerable(floatEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<long> longEnumerable: return FormatEnumerable(longEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<ulong> uLongEnumerable: return FormatEnumerable(uLongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<double> doubleEnumerable: return FormatEnumerable(doubleEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<decimal> decimalEnumerable: return FormatEnumerable(decimalEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Int128> veryLongEnumerable: return FormatEnumerable(veryLongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<UInt128> veryULongEnumerable: return FormatEnumerable(veryULongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<BigInteger> bigIntEnumerable: return FormatEnumerable(bigIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Complex> complexNumEnumerable: return FormatEnumerable(complexNumEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<DateTime> dateTimeEnumerable: return FormatEnumerable(dateTimeEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<DateOnly> dateOnlyEnumerable: return FormatEnumerable(dateOnlyEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<TimeSpan> timeSpanEnumerable: return FormatEnumerable(timeSpanEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<TimeOnly> timeOnlyEnumerable: return FormatEnumerable(timeOnlyEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Rune> runeEnumerable: return FormatEnumerable(runeEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Guid> guidEnumerable: return FormatEnumerable(guidEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<IPNetwork> ipNetworkEnumerable: return FormatEnumerable(ipNetworkEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Enum> enumEnumerable: return FormatEnumerable(enumEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Version> versionEnumerable: return FormatEnumerable(versionEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<IPAddress> ipAddressEnumerable: return FormatEnumerable(ipAddressEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Uri> uriEnumerable: return FormatEnumerable(uriEnumerable, sb, formatString, formatSwitches);
            }
        }
        else if (enumerableElementType is { IsValueType: true, IsGenericType: true } &&
                 enumerableElementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IEnumerable<bool?> nullBoolEnumerable:
                    return FormatBoolEnumerator(nullBoolEnumerable.GetEnumerator(), sb, formatString, formatSwitches);
                case IEnumerable<byte?> nullByteEnumerable:       return FormatEnumerable(nullByteEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<sbyte?> nullSbyteEnumerable:     return FormatEnumerable(nullSbyteEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<char?> nullCharEnumerable:       return FormatEnumerable(nullCharEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<short?> nullShortEnumerable:     return FormatEnumerable(nullShortEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<ushort?> nullUShortEnumerable:   return FormatEnumerable(nullUShortEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Half?> nullHalfFloatEnumerable:  return FormatEnumerable(nullHalfFloatEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<int?> nullIntEnumerable:         return FormatEnumerable(nullIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<uint?> nullUIntEnumerable:       return FormatEnumerable(nullUIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<nint?> nullNIntEnumerable:       return FormatEnumerable(nullNIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<float?> nullFloatEnumerable:     return FormatEnumerable(nullFloatEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<long?> nullLongEnumerable:       return FormatEnumerable(nullLongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<ulong?> nullULongEnumerable:     return FormatEnumerable(nullULongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<double?> nullDoubleEnumerable:   return FormatEnumerable(nullDoubleEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<decimal?> nullDecimalEnumerable: return FormatEnumerable(nullDecimalEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Int128?> nullVeryLongEnumerable: return FormatEnumerable(nullVeryLongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<UInt128?> nullVeryULongEnumerable:
                    return FormatEnumerable(nullVeryULongEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<BigInteger?> nullBigIntEnumerable: return FormatEnumerable(nullBigIntEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Complex?> nullComplexNumEnumerable:
                    return FormatEnumerable(nullComplexNumEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<DateTime?> nullDateTimeEnumerable: return FormatEnumerable(nullDateTimeEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<DateOnly?> nullDateOnlyEnumerable: return FormatEnumerable(nullDateOnlyEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<TimeSpan?> nullTimeSpanEnumerable: return FormatEnumerable(nullTimeSpanEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<TimeOnly?> nullTimeOnlyEnumerable: return FormatEnumerable(nullTimeOnlyEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Rune?> nullRuneEnumerable:         return FormatEnumerable(nullRuneEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<Guid?> nullGuidEnumerable:         return FormatEnumerable(nullGuidEnumerable, sb, formatString, formatSwitches);
                case IEnumerable<IPNetwork?> nullIpNetworkEnumerable:
                    return FormatEnumerable(nullIpNetworkEnumerable, sb, formatString, formatSwitches);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableList<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var listType = type.GetListElementType()!;

        if (listType.IsSpanFormattable() || listType.IsBool())
        {
            switch (source)
            {
                case IReadOnlyList<bool> boolList:           return FormatBoolEnumerator(boolList.GetEnumerator(), sb, formatString, formatSwitches);
                case IReadOnlyList<byte> byteList:           return FormatList(byteList, sb, formatString, formatSwitches);
                case IReadOnlyList<sbyte> sbyteList:         return FormatList(sbyteList, sb, formatString, formatSwitches);
                case IReadOnlyList<char> charList:           return FormatList(charList, sb, formatString, formatSwitches);
                case IReadOnlyList<short> shortList:         return FormatList(shortList, sb, formatString, formatSwitches);
                case IReadOnlyList<ushort> uShortList:       return FormatList(uShortList, sb, formatString, formatSwitches);
                case IReadOnlyList<Half> halfFloatList:      return FormatList(halfFloatList, sb, formatString, formatSwitches);
                case IReadOnlyList<int> intList:             return FormatList(intList, sb, formatString, formatSwitches);
                case IReadOnlyList<uint> uIntList:           return FormatList(uIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<nint> nIntList:           return FormatList(nIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<float> floatList:         return FormatList(floatList, sb, formatString, formatSwitches);
                case IReadOnlyList<long> longList:           return FormatList(longList, sb, formatString, formatSwitches);
                case IReadOnlyList<ulong> uLongList:         return FormatList(uLongList, sb, formatString, formatSwitches);
                case IReadOnlyList<double> doubleList:       return FormatList(doubleList, sb, formatString, formatSwitches);
                case IReadOnlyList<decimal> decimalList:     return FormatList(decimalList, sb, formatString, formatSwitches);
                case IReadOnlyList<Int128> veryLongList:     return FormatList(veryLongList, sb, formatString, formatSwitches);
                case IReadOnlyList<UInt128> veryULongList:   return FormatList(veryULongList, sb, formatString, formatSwitches);
                case IReadOnlyList<BigInteger> bigIntList:   return FormatList(bigIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<Complex> complexNumList:  return FormatList(complexNumList, sb, formatString, formatSwitches);
                case IReadOnlyList<DateTime> dateTimeList:   return FormatList(dateTimeList, sb, formatString, formatSwitches);
                case IReadOnlyList<DateOnly> dateOnlyList:   return FormatList(dateOnlyList, sb, formatString, formatSwitches);
                case IReadOnlyList<TimeSpan> timeSpanList:   return FormatList(timeSpanList, sb, formatString, formatSwitches);
                case IReadOnlyList<TimeOnly> timeOnlyList:   return FormatList(timeOnlyList, sb, formatString, formatSwitches);
                case IReadOnlyList<Rune> runeList:           return FormatList(runeList, sb, formatString, formatSwitches);
                case IReadOnlyList<Guid> guidList:           return FormatList(guidList, sb, formatString, formatSwitches);
                case IReadOnlyList<IPNetwork> ipNetworkList: return FormatList(ipNetworkList, sb, formatString, formatSwitches);
                case IReadOnlyList<Enum> enumList:           return FormatList(enumList, sb, formatString, formatSwitches);
                case IReadOnlyList<Version> versionList:     return FormatList(versionList, sb, formatString, formatSwitches);
                case IReadOnlyList<IPAddress> ipAddressList: return FormatList(ipAddressList, sb, formatString, formatSwitches);
                case IReadOnlyList<Uri> uriList:             return FormatList(uriList, sb, formatString, formatSwitches);
            }
        }
        else if (listType is { IsValueType: true, IsGenericType: true } && listType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case IReadOnlyList<bool?> nullBoolList: return FormatBoolEnumerator(nullBoolList.GetEnumerator(), sb, formatString, formatSwitches);
                case IReadOnlyList<byte?> nullByteList: return FormatList(nullByteList, sb, formatString, formatSwitches);
                case IReadOnlyList<sbyte?> nullSbyteList: return FormatList(nullSbyteList, sb, formatString, formatSwitches);
                case IReadOnlyList<char?> nullCharList: return FormatList(nullCharList, sb, formatString, formatSwitches);
                case IReadOnlyList<short?> nullShortList: return FormatList(nullShortList, sb, formatString, formatSwitches);
                case IReadOnlyList<ushort?> nullUShortList: return FormatList(nullUShortList, sb, formatString, formatSwitches);
                case IReadOnlyList<Half?> nullHalfFloatList: return FormatList(nullHalfFloatList, sb, formatString, formatSwitches);
                case IReadOnlyList<int?> nullIntList: return FormatList(nullIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<uint?> nullUIntList: return FormatList(nullUIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<nint?> nullNIntList: return FormatList(nullNIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<float?> nullFloatList: return FormatList(nullFloatList, sb, formatString, formatSwitches);
                case IReadOnlyList<long?> nullLongList: return FormatList(nullLongList, sb, formatString, formatSwitches);
                case IReadOnlyList<ulong?> nullULongList: return FormatList(nullULongList, sb, formatString, formatSwitches);
                case IReadOnlyList<double?> nullDoubleList: return FormatList(nullDoubleList, sb, formatString, formatSwitches);
                case IReadOnlyList<decimal?> nullDecimalList: return FormatList(nullDecimalList, sb, formatString, formatSwitches);
                case IReadOnlyList<Int128?> nullVeryLongList: return FormatList(nullVeryLongList, sb, formatString, formatSwitches);
                case IReadOnlyList<UInt128?> nullVeryULongList: return FormatList(nullVeryULongList, sb, formatString, formatSwitches);
                case IReadOnlyList<BigInteger?> nullBigIntList: return FormatList(nullBigIntList, sb, formatString, formatSwitches);
                case IReadOnlyList<Complex?> nullComplexNumList: return FormatList(nullComplexNumList, sb, formatString, formatSwitches);
                case IReadOnlyList<DateTime?> nullDateTimeList: return FormatList(nullDateTimeList, sb, formatString, formatSwitches);
                case IReadOnlyList<DateOnly?> nullDateOnlyList: return FormatList(nullDateOnlyList, sb, formatString, formatSwitches);
                case IReadOnlyList<TimeSpan?> nullTimeSpanList: return FormatList(nullTimeSpanList, sb, formatString, formatSwitches);
                case IReadOnlyList<TimeOnly?> nullTimeOnlyList: return FormatList(nullTimeOnlyList, sb, formatString, formatSwitches);
                case IReadOnlyList<Rune?> nullRuneList: return FormatList(nullRuneList, sb, formatString, formatSwitches);
                case IReadOnlyList<Guid?> nullGuidList: return FormatList(nullGuidList, sb, formatString, formatSwitches);
                case IReadOnlyList<IPNetwork?> nullIpNetworkList: return FormatList(nullIpNetworkList, sb, formatString, formatSwitches);
            }
        }
        return 0;
    }

    private int CheckIsKnownSpanFormattableArray<TAny>([DisallowNull] TAny source, IStringBuilder sb, string formatString, Type type
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var elementType = type.GetElementType()!;
        if (elementType.IsSpanFormattable() || elementType.IsBool())
        {
            switch (source)
            {
                case bool[] boolArray: return FormatBoolEnumerator((IEnumerator<bool>)boolArray.GetEnumerator(), sb, formatString, formatSwitches);
                case byte[] byteArray: return FormatArray(byteArray, sb, formatString, formatSwitches);
                case sbyte[] sbyteArray: return FormatArray(sbyteArray, sb, formatString, formatSwitches);
                case char[] charArray: return FormatArray(charArray, sb, formatString, formatSwitches);
                case short[] shortArray: return FormatArray(shortArray, sb, formatString, formatSwitches);
                case ushort[] uShortArray: return FormatArray(uShortArray, sb, formatString, formatSwitches);
                case Half[] halfFloatArray: return FormatArray(halfFloatArray, sb, formatString, formatSwitches);
                case int[] intArray: return FormatArray(intArray, sb, formatString, formatSwitches);
                case uint[] uIntArray: return FormatArray(uIntArray, sb, formatString, formatSwitches);
                case nint[] nIntArray: return FormatArray(nIntArray, sb, formatString, formatSwitches);
                case float[] floatArray: return FormatArray(floatArray, sb, formatString, formatSwitches);
                case long[] longArray: return FormatArray(longArray, sb, formatString, formatSwitches);
                case ulong[] uLongArray: return FormatArray(uLongArray, sb, formatString, formatSwitches);
                case double[] doubleArray: return FormatArray(doubleArray, sb, formatString, formatSwitches);
                case decimal[] decimalArray: return FormatArray(decimalArray, sb, formatString, formatSwitches);
                case Int128[] veryLongArray: return FormatArray(veryLongArray, sb, formatString, formatSwitches);
                case UInt128[] veryULongArray: return FormatArray(veryULongArray, sb, formatString, formatSwitches);
                case BigInteger[] bigIntArray: return FormatArray(bigIntArray, sb, formatString, formatSwitches);
                case Complex[] complexNumArray: return FormatArray(complexNumArray, sb, formatString, formatSwitches);
                case DateTime[] dateTimeArray: return FormatArray(dateTimeArray, sb, formatString, formatSwitches);
                case DateOnly[] dateOnlyArray: return FormatArray(dateOnlyArray, sb, formatString, formatSwitches);
                case TimeSpan[] timeSpanArray: return FormatArray(timeSpanArray, sb, formatString, formatSwitches);
                case TimeOnly[] timeOnlyArray: return FormatArray(timeOnlyArray, sb, formatString, formatSwitches);
                case Rune[] runeArray: return FormatArray(runeArray, sb, formatString, formatSwitches);
                case Guid[] guidArray: return FormatArray(guidArray, sb, formatString, formatSwitches);
                case IPNetwork[] ipNetworkArray: return FormatArray(ipNetworkArray, sb, formatString, formatSwitches);
                case Enum[] enumArray: return FormatArray(enumArray, sb, formatString, formatSwitches);
                case Version[] versionArray: return FormatArray(versionArray, sb, formatString, formatSwitches);
                case IPAddress[] ipAddressArray: return FormatArray(ipAddressArray, sb, formatString, formatSwitches);
                case Uri[] uriArray: return FormatArray(uriArray, sb, formatString, formatSwitches);
            }
        }
        else if (elementType is { IsValueType: true, IsGenericType: true } && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            switch (source)
            {
                case bool?[] nullBoolArray:
                    return FormatBoolEnumerator((IEnumerator<bool?>)(nullBoolArray.GetEnumerator()), sb, formatString, formatSwitches);
                case byte?[] nullByteArray:           return FormatArray(nullByteArray, sb, formatString, formatSwitches);
                case sbyte?[] nullSbyteArray:         return FormatArray(nullSbyteArray, sb, formatString, formatSwitches);
                case char?[] nullCharArray:           return FormatArray(nullCharArray, sb, formatString, formatSwitches);
                case short?[] nullShortArray:         return FormatArray(nullShortArray, sb, formatString, formatSwitches);
                case ushort?[] nullUShortArray:       return FormatArray(nullUShortArray, sb, formatString, formatSwitches);
                case Half?[] nullHalfFloatArray:      return FormatArray(nullHalfFloatArray, sb, formatString, formatSwitches);
                case int?[] nullIntArray:             return FormatArray(nullIntArray, sb, formatString, formatSwitches);
                case uint?[] nullUIntArray:           return FormatArray(nullUIntArray, sb, formatString, formatSwitches);
                case nint?[] nullNIntArray:           return FormatArray(nullNIntArray, sb, formatString, formatSwitches);
                case float?[] nullFloatArray:         return FormatArray(nullFloatArray, sb, formatString, formatSwitches);
                case long?[] nullLongArray:           return FormatArray(nullLongArray, sb, formatString, formatSwitches);
                case ulong?[] nullULongArray:         return FormatArray(nullULongArray, sb, formatString, formatSwitches);
                case double?[] nullDoubleArray:       return FormatArray(nullDoubleArray, sb, formatString, formatSwitches);
                case decimal?[] nullDecimalArray:     return FormatArray(nullDecimalArray, sb, formatString, formatSwitches);
                case Int128?[] nullVeryLongArray:     return FormatArray(nullVeryLongArray, sb, formatString, formatSwitches);
                case UInt128?[] nullVeryULongArray:   return FormatArray(nullVeryULongArray, sb, formatString, formatSwitches);
                case BigInteger?[] nullBigIntArray:   return FormatArray(nullBigIntArray, sb, formatString, formatSwitches);
                case Complex?[] nullComplexNumArray:  return FormatArray(nullComplexNumArray, sb, formatString, formatSwitches);
                case DateTime?[] nullDateTimeArray:   return FormatArray(nullDateTimeArray, sb, formatString, formatSwitches);
                case DateOnly?[] nullDateOnlyArray:   return FormatArray(nullDateOnlyArray, sb, formatString, formatSwitches);
                case TimeSpan?[] nullTimeSpanArray:   return FormatArray(nullTimeSpanArray, sb, formatString, formatSwitches);
                case TimeOnly?[] nullTimeOnlyArray:   return FormatArray(nullTimeOnlyArray, sb, formatString, formatSwitches);
                case Rune?[] nullRuneArray:           return FormatArray(nullRuneArray, sb, formatString, formatSwitches);
                case Guid?[] nullGuidArray:           return FormatArray(nullGuidArray, sb, formatString, formatSwitches);
                case IPNetwork?[] nullIpNetworkArray: return FormatArray(nullIpNetworkArray, sb, formatString, formatSwitches);
            }
        }
        return 0;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatSwitches);
        }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0)
                    addedChars += AddCollectionElementSeparatorAndPadding
                        (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatSwitches);
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
                        (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
        }
        return addedChars;
    }

    public virtual int FormatBoolEnumerator(IEnumerator<bool?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
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
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(bool?);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatSwitches);
        }
        var itemCount = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0)
                    addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount
                                                                        , formatSwitches);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatSwitches);
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
                    addedChars += AddCollectionElementSeparatorAndPadding(elementType, destCharSpan, destStartIndex + addedChars, itemCount
                                                                        , formatSwitches);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
        }
        return addedChars;
    }

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    public abstract int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    public abstract int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent);

    public abstract int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent);

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?;

    public abstract int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?;

    public abstract int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public abstract int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    public virtual int CollectionNextItemFormat(bool nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) => Format(nextItem, sb, formatString);

    public virtual int CollectionNextItemFormat(bool nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) =>
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public int CollectionNextItemFormat(bool? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (nextItem == null) return Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        return CollectionNextItemFormat(nextItem.Value, retrieveCount, sb, formatString);
    }

    public int CollectionNextItemFormat(bool? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (nextItem == null) return Options.NullWritesEmpty ? 0 : destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        return CollectionNextItemFormat(nextItem.Value, retrieveCount, destCharSpan, destStartIndex, formatString);
    }

    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormatSwitches formatSwitches = EncodeInnerContent);

    public abstract int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , FormatSwitches formatSwitches = EncodeInnerContent);

    public abstract int CollectionEnd(Type elementType, IStringBuilder sb, int totalItemCount
      , FormatSwitches formatSwitches = EncodeInnerContent);

    public abstract int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int totalItemCount
      , FormatSwitches formatSwitches = EncodeInnerContent);


    public override void StateReset()
    {
        (StringEncoderTransfer as IRecyclableObject)?.DecrementRefCount();
        base.StateReset();
    }
}
