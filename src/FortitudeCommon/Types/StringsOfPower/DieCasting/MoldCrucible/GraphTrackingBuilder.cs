// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class GraphTrackingBuilder : ReusableObject<GraphTrackingBuilder>
{
    private static int globalInstanceId;

    protected int InstanceId = Interlocked.Increment(ref globalInstanceId);

    private ContentSeparatorPaddingRangeTracking currentSectionRanges;

    private StyleOptions? styleOptions;

    private int overWriteIndex       = -1;
    private int overWriteEndIndex    = -1;
    private int overWriteIndentLevel = -1;
    private int overWriteIndentSize  = -1;

    public GraphTrackingBuilder Initialize(StyleOptions options, IStringBuilder stringBuilder)
    {
        styleOptions = options;
        ((IRecyclableObject)options).IncrementRefCount();
        sb = stringBuilder;

        return this;
    }

    public GraphTrackingBuilder InitializeInsertBuilder(IStringBuilder writeBuffer, int indentLevel, int indentChars
      , IEncodingTransfer? overrideGraphEncoder, IEncodingTransfer? overrideParentGraphEncoder)
    {
        sb                   = writeBuffer;
        graphEncoder         = overrideGraphEncoder;
        parentGraphEncoder   = overrideParentGraphEncoder;
        overWriteIndentLevel = indentLevel;
        overWriteIndentSize  = indentChars;
        return this;
    }

    public GraphTrackingBuilder StartInsertAt(int insertAt, int insertShiftAmount)
    {
        overWriteIndex    = insertAt;
        overWriteEndIndex = insertAt + insertShiftAmount;
        sb.EnsureCapacity(insertShiftAmount);
        sb.ShiftRightAt(insertAt, insertShiftAmount);

        return this;
    }

    public ContentSeparatorPaddingRangeTracking CurrentSectionRanges
    {
        get => currentSectionRanges;
        set => currentSectionRanges = value;
    }

    public ContentSeparatorRanges SnapshotLastAppendSequence(FormatFlags formatFlags)
    {
        var checkAnyRanges = CurrentSectionRanges.ToContentSeparatorFromEndRanges(IsInModifyOverwriteMode ? overWriteIndex : sb.Length, formatFlags);
        if (checkAnyRanges.ContentRange != null || checkAnyRanges.SeparatorPaddingRange != null)
        {
            PenUltimateContentSeparatorPaddingRanges = LastContentSeparatorPaddingRanges;
            LastContentSeparatorPaddingRanges        = checkAnyRanges;
        }
        AllowEmptyContent = false;
        ResetCurrent(FormatFlags.DefaultCallerTypeFlags);
        return LastContentSeparatorPaddingRanges;
    }

    private bool IsInModifyOverwriteMode => overWriteIndex >= 0 && overWriteIndex < overWriteEndIndex;
    private bool IsInMarkOverwriteMode => overWriteIndex >= 0 && overWriteIndex <= overWriteEndIndex;

    public ContentSeparatorRanges LastContentSeparatorPaddingRanges { get; set; }

    public ContentSeparatorRanges? PenUltimateContentSeparatorPaddingRanges { get; set; }

    public int IndentLevel
    {
        get => styleOptions?.IndentLevel ?? overWriteIndentLevel;
        set
        {
            if (styleOptions != null) { styleOptions.IndentLevel = value; }
            overWriteIndentLevel = value;
        }
    }

    public int IndentSize => styleOptions?.IndentSize ?? overWriteIndentSize;

    public override void StateReset()
    {
        overWriteIndex       = -1;
        overWriteEndIndex    = -1;
        overWriteIndentLevel = -1;
        overWriteIndentSize  = -1;
        if (graphEncoder is not PassThroughEncodingTransfer)
        {
            graphEncoder?.DecrementRefCount();
            graphEncoder = null;
        }
        if (parentGraphEncoder is not PassThroughEncodingTransfer)
        {
            parentGraphEncoder?.DecrementRefCount();
            parentGraphEncoder = null;
        }
        LastContentSeparatorPaddingRanges = default;

        sb = null!;

        PenUltimateContentSeparatorPaddingRanges = null;
    }

    public IEncodingTransfer GraphEncoder
    {
        get => graphEncoder ??= Recycler.Borrow<PassThroughEncodingTransfer>();
        set
        {
            if (ReferenceEquals(parentGraphEncoder, value) && ReferenceEquals(graphEncoder, value)) return;
            if (graphEncoder != null && !ReferenceEquals(graphEncoder, value) && !ReferenceEquals(parentGraphEncoder, value))
            {
                value.IncrementRefCount();
            }
            if (parentGraphEncoder != null && !ReferenceEquals(parentGraphEncoder, graphEncoder) && !ReferenceEquals(parentGraphEncoder, value))
            {
                parentGraphEncoder.DecrementRefCount();
            }
            parentGraphEncoder = graphEncoder;
            graphEncoder       = value;
        }
    }

    public override IRecycler Recycler => base.Recycler ?? DataStructures.MemoryPools.Recycler.ThreadStaticRecycler;

    public IEncodingTransfer ParentGraphEncoder
    {
        get => parentGraphEncoder ??= Recycler.Borrow<PassThroughEncodingTransfer>();
        set => parentGraphEncoder = value;
    }

    private IStringBuilder sb = null!;

    private IEncodingTransfer? graphEncoder;
    private IEncodingTransfer? parentGraphEncoder;

    public bool HasCommitContent => CurrentSectionRanges.HasContent;

    public IStringBuilder Sb => sb;

    private bool AllowEmptyContent
    {
        get => currentSectionRanges.AllowEmptyContent;
        set => currentSectionRanges.AllowEmptyContent = value;
    }

    public void ResetCurrent(FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        currentSectionRanges.Reset();
        currentSectionRanges.StartedWithFormatFlags = formatFlags;
        currentSectionRanges.AllowEmptyContent      = allowEmptyContent;
    }

    public void AddHighWaterMark()
    {
        if (CurrentSectionRanges.HasContent || CurrentSectionRanges.HasSeparator || CurrentSectionRanges.HasPadding)
        {
            Complete(CurrentSectionRanges.StartedWithFormatFlags);
        }
        var highWaterMark = new ContentSeparatorRanges
            (FormatFlags.DefaultCallerTypeFlags, new Range(Index.End, Index.End)); // Empty content Range stops penultimate seperator/padding removal.
        PenUltimateContentSeparatorPaddingRanges = LastContentSeparatorPaddingRanges;
        LastContentSeparatorPaddingRanges        = highWaterMark;
    }

    public GraphTrackingBuilder StartNextContentSeparatorPaddingSequence(IStringBuilder writeBuffer, FormatFlags formatFlags
      , bool allowEmptyContent = false)
    {
        sb = writeBuffer;

        ResetCurrent(formatFlags, allowEmptyContent);
        return MarkContentStart();
    }

    public GraphTrackingBuilder MarkContentStart(int atIndex = -1)
    {
        currentSectionRanges.FromStartContentStart = IsInMarkOverwriteMode ? overWriteIndex : atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder MarkContentEnd(int atIndex = -1)
    {
        currentSectionRanges.FromStartContentEnd = IsInMarkOverwriteMode ? overWriteIndex : atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder StartAppendParentContent(string content, IStringBuilder writeBuffer
      , FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        sb = writeBuffer;

        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        if (IsInModifyOverwriteMode)
            overWriteIndex += ParentGraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            ParentGraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder StartAppendContent(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        sb = writeBuffer;

        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();

        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder StartAppendDelimiter(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FormatFlags formatFlags, bool allowEmptyContent = false) =>
        StartAppendParentContent(content, writeBuffer, formatFlags, allowEmptyContent);

    public GraphTrackingBuilder StartAppendContent(ReadOnlySpan<char> content, IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        sb = writeBuffer;

        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();

        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendContent(ReadOnlySpan<char> content)
    {
        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendDelimiter(ReadOnlySpan<char> content)
    {
        if (IsInModifyOverwriteMode)
            overWriteIndex += ParentGraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            ParentGraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendContent(string content)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }

        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendParentContent(string content)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }

        if (IsInModifyOverwriteMode)
            overWriteIndex += ParentGraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            ParentGraphEncoder.AppendTransfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendContent(char toRepeat, int repeatTimes)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        if (repeatTimes > 0)
        {
            Span<char> chars = stackalloc char[repeatTimes];
            chars.OverWriteRepatAt(0, toRepeat, repeatTimes);
            if (IsInModifyOverwriteMode)
                overWriteIndex += GraphEncoder.OverwriteTransfer(chars, sb, overWriteIndex);
            else
                GraphEncoder.AppendTransfer(chars, sb);
        }
        return MarkContentEnd();
    }

    public ContentSeparatorRanges StartAppendContentAndComplete(string content, IStringBuilder writeBuffer
      , FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        sb = writeBuffer;

        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        AppendContent(content);
        return SnapshotLastAppendSequence(formatFlags);
    }

    public GraphTrackingBuilder AppendPadding(string padding)
    {
        if (currentSectionRanges is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            MarkContentEnd();
            MarkSeparatorEnd();
        }
        else if (currentSectionRanges is { FromStartSeparatorEnd: null }) { MarkSeparatorEnd(); }

        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(padding, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(padding, sb);
        return TagPaddingEnd();
    }

    public GraphTrackingBuilder AppendPadding(char toRepeat, int repeatTimes)
    {
        if (currentSectionRanges is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            MarkContentEnd();
            MarkSeparatorEnd();
        }
        else if (currentSectionRanges is { FromStartSeparatorEnd: null }) { MarkSeparatorEnd(); }
        if (repeatTimes > 0)
        {
            Span<char> chars = stackalloc char[repeatTimes];
            chars.OverWriteRepatAt(0, toRepeat, repeatTimes);
            if (IsInModifyOverwriteMode)
                overWriteIndex += GraphEncoder.OverwriteTransfer(chars, sb, overWriteIndex);
            else
                GraphEncoder.AppendTransfer(chars, sb);
        }
        return TagPaddingEnd();
    }

    public GraphTrackingBuilder AppendSeparator(string separator)
    {
        if (currentSectionRanges is { FromStartContentEnd: null }) { MarkContentEnd(); }

        if (IsInModifyOverwriteMode)
            overWriteIndex += GraphEncoder.OverwriteTransfer(separator, sb, overWriteIndex);
        else
            GraphEncoder.AppendTransfer(separator, sb);
        return MarkSeparatorEnd();
    }

    public ContentSeparatorRanges AppendPaddingAndComplete(string content, FormatFlags formatFlags)
    {
        if (IsInModifyOverwriteMode)
            overWriteIndex += ParentGraphEncoder.OverwriteTransfer(content, sb, overWriteIndex);
        else
            ParentGraphEncoder.AppendTransfer(content, sb);
        return Complete(formatFlags);
    }

    public GraphTrackingBuilder MarkSeparatorEnd()
    {
        currentSectionRanges.FromStartSeparatorEnd = IsInMarkOverwriteMode ? overWriteIndex : sb.Length;
        return this;
    }

    protected GraphTrackingBuilder TagPaddingEnd()
    {
        currentSectionRanges.FromStartPaddingEnd = IsInMarkOverwriteMode ? overWriteIndex : sb.Length;
        return this;
    }

    public ContentSeparatorRanges ContentEndToRanges(FormatFlags formatFlags)
    {
        currentSectionRanges.FromStartSeparatorEnd = IsInMarkOverwriteMode ? overWriteIndex : sb.Length;
        return currentSectionRanges.ToContentSeparatorFromEndRanges(IsInModifyOverwriteMode ? overWriteIndex : sb.Length, formatFlags);
    }

    public GraphTrackingBuilder MarkPaddingEnd(int atIndex = -1)
    {
        currentSectionRanges.FromStartPaddingEnd = IsInMarkOverwriteMode ? overWriteIndex : atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public ContentSeparatorRanges Complete(FormatFlags formatFlags)
    {
        currentSectionRanges.FromStartPaddingEnd = IsInMarkOverwriteMode ? overWriteIndex : sb.Length;

        return SnapshotLastAppendSequence(formatFlags);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char RemoveLastSeparatorAndPadding()
    {
        if (sb == null!) return '\0';
        var lastRange        = LastContentSeparatorPaddingRanges;
        var penUltimateRange = PenUltimateContentSeparatorPaddingRanges;
        int sepPaddingLen;
        if (lastRange.SeparatorPaddingRange == null)
        {
            if (lastRange.ContentRange == null && penUltimateRange != null)
            {
                sepPaddingLen = penUltimateRange.Value.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
                if (sepPaddingLen > 0)
                {
                    if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
                    else
                        sb.Length -= sepPaddingLen;
                }
            }
            AddHighWaterMark();
            return sb.Length > 0
                ? (IsInModifyOverwriteMode ? sb[overWriteIndex - 1] : sb[^1])
                : '\0';
        }
        sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
        if (sepPaddingLen > 0)
        {
            if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
            else
                sb.Length -= sepPaddingLen;
        }
        if (lastRange.ContentRange == null && penUltimateRange != null)
        {
            sepPaddingLen = penUltimateRange.Value.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
            if (sepPaddingLen > 0)
            {
                if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
                else
                    sb.Length -= sepPaddingLen;
            }
        }
        AddHighWaterMark();
        return sb.Length > 0
            ? (IsInModifyOverwriteMode ? sb[Math.Max(0, overWriteIndex - 1)] : sb[^1])
            : '\0';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char RemoveLastSeparatorAndPadding(Span<char> destSpan, ref int destIndex)
    {
        var lastRange        = LastContentSeparatorPaddingRanges;
        var penUltimateRange = PenUltimateContentSeparatorPaddingRanges;
        int sepPaddingLen;
        if (lastRange.SeparatorPaddingRange == null)
        {
            if (lastRange.ContentRange == null && penUltimateRange != null)
            {
                sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
                if (sepPaddingLen > 0)
                {
                    if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
                    else
                        destIndex -= sepPaddingLen;
                }
            }
            return destIndex > 0 ? (IsInModifyOverwriteMode ? destSpan[Math.Max(0, overWriteIndex - 1)] : destSpan[destIndex - 1]) : '\0';
        }
        sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(destIndex) ?? 0;
        if (sepPaddingLen > 0)
        {
            if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
            else
                destIndex -= sepPaddingLen;
        }
        if (lastRange.ContentRange == null && penUltimateRange != null)
        {
            sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
            if (sepPaddingLen > 0)
            {
                if (IsInModifyOverwriteMode) { overWriteIndex -= sepPaddingLen; }
                else
                    destIndex -= sepPaddingLen;
            }
        }
        return destIndex > 0 ? (IsInModifyOverwriteMode ? destSpan[Math.Max(0, overWriteIndex - 1)] : destSpan[destIndex - 1]) : '\0';
    }

    public override GraphTrackingBuilder Clone()
    {
        var recycler = Recycler ?? DataStructures.MemoryPools.Recycler.ThreadStaticRecycler;
        var clone    = recycler.Borrow<GraphTrackingBuilder>();
        return clone.CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public override GraphTrackingBuilder CopyFrom(GraphTrackingBuilder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SetHistory(source);
        styleOptions         = source.styleOptions;
        AllowEmptyContent    = source.AllowEmptyContent;
        sb                   = source.Sb;
        graphEncoder         = source.GraphEncoder;
        parentGraphEncoder   = source.ParentGraphEncoder;

        overWriteIndex       = source.overWriteIndex;
        overWriteEndIndex    = source.overWriteEndIndex;
        overWriteIndentLevel = source.overWriteIndentLevel;
        overWriteIndentSize  = source.overWriteIndentSize;

        return this;
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {nameof(styleOptions)}: {styleOptions?.ToString() ?? "null"} }}";

    public void SetHistory(GraphTrackingBuilder graphBuilder)
    {
        currentSectionRanges = graphBuilder.CurrentSectionRanges;
        LastContentSeparatorPaddingRanges = graphBuilder.LastContentSeparatorPaddingRanges;
        PenUltimateContentSeparatorPaddingRanges = graphBuilder.PenUltimateContentSeparatorPaddingRanges;
    }
}
