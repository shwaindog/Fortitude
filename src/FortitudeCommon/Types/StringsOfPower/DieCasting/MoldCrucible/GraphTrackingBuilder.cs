// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class GraphTrackingBuilder
{
    private ContentSeparatorPaddingRangeTracking currentSectionRanges;

    private Stack<ContentSeparatorPaddingRangeTracking> rangeHistory = new();

    public ContentSeparatorPaddingRangeTracking CurrentSectionRanges
    {
        get => currentSectionRanges;
        set => currentSectionRanges = value;
    }

    public ContentSeparatorRanges SnapshotLastAppendSequence(FormatFlags formatFlags)
    {
        var checkAnyRanges        = CurrentSectionRanges.ToContentSeparatorFromEndRanges(sb, formatFlags);
        if (checkAnyRanges.ContentRange != null || checkAnyRanges.SeparatorPaddingRange != null)
        {
            PenUltimateContentSeparatorPaddingRanges = LastContentSeparatorPaddingRanges;
            LastContentSeparatorPaddingRanges        = checkAnyRanges;
        }
        AllowEmptyContent                        = false;
        ResetCurrent(FormatFlags.DefaultCallerTypeFlags);
        return LastContentSeparatorPaddingRanges;
    }

    public ContentSeparatorRanges LastContentSeparatorPaddingRanges { get; set; }

    public ContentSeparatorRanges? PenUltimateContentSeparatorPaddingRanges { get; set; }

    public IEncodingTransfer GraphEncoder
    {
        get => graphEncoder;
        set
        {
            parentGraphEncoder = graphEncoder;
            graphEncoder       = value;
        }
    }

    public IEncodingTransfer ParentGraphEncoder
    {
        get => parentGraphEncoder ??= new PassThroughEncodingTransfer();
        set => parentGraphEncoder = value;
    }

    private IStringBuilder sb = null!;

    private IStyledTypeFormatting styledFormatting = null!;
    private IEncodingTransfer     graphEncoder     = new PassThroughEncodingTransfer();
    private IEncodingTransfer?    parentGraphEncoder;

    public bool HasCommitContent => CurrentSectionRanges.HasContent;
    
    private bool AllowEmptyContent
    {
        get => currentSectionRanges.AllowEmptyContent;
        set => currentSectionRanges.AllowEmptyContent = value;
    }

    public void ResetCurrent(FormatFlags formatFlags, bool allowEmptyContent = false)
    {
        currentSectionRanges.Reset();
        currentSectionRanges.StartedWithFormatFlags       = formatFlags;
        currentSectionRanges.AllowEmptyContent = allowEmptyContent;
    }

    public void AddHighWaterMark()
    {
        if (CurrentSectionRanges.HasContent || CurrentSectionRanges.HasSeparator || CurrentSectionRanges.HasPadding)
        {
            Complete(CurrentSectionRanges.StartedWithFormatFlags);
        }
        var highWaterMark = new ContentSeparatorRanges
            (FormatFlags.DefaultCallerTypeFlags
           , new Range(Index.End, Index.End)); // Empty content Range stops penultimate seperator/padding removal.
        PenUltimateContentSeparatorPaddingRanges = LastContentSeparatorPaddingRanges;
        LastContentSeparatorPaddingRanges        = highWaterMark;
    }

    public GraphTrackingBuilder StartNextContentSeparatorPaddingSequence(IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FormatFlags formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        
        ResetCurrent(formatFlags, allowEmptyContent);
        return MarkContentStart();
    }

    public GraphTrackingBuilder MarkContentStart(int atIndex = -1)
    {
        currentSectionRanges.FromStartContentStart = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder MarkContentEnd(int atIndex = -1)
    {
        currentSectionRanges.FromStartContentEnd = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder StartAppendParentContent(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FormatFlags formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        ParentGraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder StartAppendContent(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FormatFlags formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        GraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder StartAppendDelimiter(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FormatFlags formatFlags, bool allowEmptyContent = false) =>
        StartAppendParentContent(content, writeBuffer, styledTypeFormatting, formatFlags, allowEmptyContent);

    public GraphTrackingBuilder StartAppendContent(ReadOnlySpan<char> content, IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FormatFlags formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        GraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendContent(ReadOnlySpan<char> content)
    {
        GraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendDelimiter(ReadOnlySpan<char> content)
    {
        ParentGraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendContent(string content)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        GraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendParentContent(string content)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        ParentGraphEncoder.Transfer(content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendContent(char toRepeat, int repeatTimes)
    {
        if (currentSectionRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        sb.Append(toRepeat, repeatTimes);
        return MarkContentEnd();
    }

    public ContentSeparatorRanges StartAppendContentAndComplete(string content, IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FormatFlags formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
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
        GraphEncoder.Transfer(padding, sb);
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
        sb.Append(toRepeat, repeatTimes);
        return TagPaddingEnd();
    }

    public GraphTrackingBuilder AppendSeparator(string separator)
    {
        if (currentSectionRanges is { FromStartContentEnd: null }) { MarkContentEnd(); }
        GraphEncoder.Transfer(separator, sb);
        return MarkSeparatorEnd();
    }

    public ContentSeparatorRanges AppendPaddingAndComplete(string content, FormatFlags formatFlags)
    {
        GraphEncoder.Transfer(content, sb);
        return Complete(formatFlags);
    }

    public GraphTrackingBuilder MarkSeparatorEnd()
    {
        currentSectionRanges.FromStartSeparatorEnd = sb.Length;
        return this;
    }

    protected GraphTrackingBuilder TagPaddingEnd()
    {
        currentSectionRanges.FromStartPaddingEnd = sb.Length;
        return this;
    }

    public ContentSeparatorRanges ContentEndToRanges(FormatFlags formatFlags)
    {
        currentSectionRanges.FromStartSeparatorEnd = sb.Length;
        return currentSectionRanges.ToContentSeparatorFromEndRanges(sb, formatFlags);
    }

    public GraphTrackingBuilder MarkPaddingEnd(int atIndex = -1)
    {
        currentSectionRanges.FromStartPaddingEnd = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public ContentSeparatorRanges Complete(FormatFlags formatFlags)
    {
        currentSectionRanges.FromStartPaddingEnd = sb.Length;

        return SnapshotLastAppendSequence(formatFlags);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char RemoveLastSeparatorAndPadding()
    {
        var lastRange        = LastContentSeparatorPaddingRanges;
        var penUltimateRange = PenUltimateContentSeparatorPaddingRanges;
        int sepPaddingLen;
        if (lastRange.SeparatorPaddingRange == null)
        {
            if (lastRange.ContentRange == null && penUltimateRange != null)
            {
                sepPaddingLen =  penUltimateRange?.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
                sb.Length     -= sepPaddingLen;
            }
            AddHighWaterMark();
            return (sb?.Length ?? 0) > 0 ? sb![^1] : '\0';
        }
        sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
        sb.Length -= sepPaddingLen;
        if (lastRange.ContentRange == null && penUltimateRange != null)
        {
            sepPaddingLen =  penUltimateRange?.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
            sb.Length     -= sepPaddingLen;
        }
        AddHighWaterMark();
        return (sb?.Length ?? 0) > 0 ? sb![^1] : '\0';
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
                sepPaddingLen =  lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
                destIndex     -= sepPaddingLen;
            }
            return destIndex > 0 ? destSpan[destIndex - 1] : '\0';
        }
        sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(destIndex) ?? 0;
        destIndex -= sepPaddingLen;
        if (lastRange.ContentRange == null && penUltimateRange != null)
        {
            sepPaddingLen =  lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
            destIndex     -= sepPaddingLen;
        }
        return destIndex > 0 ? destSpan[destIndex - 1] : '\0';
    }
}
