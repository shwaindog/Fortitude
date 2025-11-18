// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class GraphTrackingBuilder
{
    private ContentSeparatorPaddingRangeTracking currentNodeRanges;

    private Stack<ContentSeparatorPaddingRangeTracking> rangeHistory = new();

    public ContentSeparatorPaddingRangeTracking CurrentNodeRanges
    {
        get => currentNodeRanges;
        set => currentNodeRanges = value;
    }

    public ContentSeparatorRanges SnapshotLastAppendSequence(FieldContentHandling formatFlags)
    {
        PenUltimateContentSeparatorPaddingRanges = LastContentSeparatorPaddingRanges;
        LastContentSeparatorPaddingRanges        = CurrentNodeRanges.ToContentSeparatorFromEndRanges(sb, formatFlags);
        AllowEmptyContent                        = false;
        ResetCurrent(FieldContentHandling.DefaultCallerTypeFlags);
        return LastContentSeparatorPaddingRanges;
    }

    public ContentSeparatorRanges LastContentSeparatorPaddingRanges { get; set; }

    public ContentSeparatorRanges? PenUltimateContentSeparatorPaddingRanges { get; set; }

    public IEncodingTransfer GraphEncoder { get; set; } = new PassThroughEncodingTransfer();

    private IStringBuilder sb = null!;

    private IStyledTypeFormatting styledFormatting = null!;

    private bool AllowEmptyContent
    {
        get => currentNodeRanges.AllowEmptyContent;
        set => currentNodeRanges.AllowEmptyContent = value;
    }

    public void ResetCurrent(FieldContentHandling formatFlags, bool allowEmptyContent = false)
    {
        currentNodeRanges.Reset();
        currentNodeRanges.FormatFlags       = formatFlags;
        currentNodeRanges.AllowEmptyContent = allowEmptyContent;
    }

    public GraphTrackingBuilder StartNextContentSeparatorPaddingSequence(IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FieldContentHandling formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        
        ResetCurrent(formatFlags, allowEmptyContent);
        return MarkContentStart();
    }

    public GraphTrackingBuilder MarkContentStart(int atIndex = -1)
    {
        currentNodeRanges.FromStartContentStart = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder MarkContentEnd(int atIndex = -1)
    {
        currentNodeRanges.FromStartContentEnd = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public GraphTrackingBuilder StartAppendContent(string content, IStringBuilder writeBuffer, IStyledTypeFormatting styledTypeFormatting
      , FieldContentHandling formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        GraphEncoder.Transfer(styledFormatting, content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder StartAppendContent(ReadOnlySpan<char> content, IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FieldContentHandling formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        GraphEncoder.Transfer(styledFormatting, content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendContent(ReadOnlySpan<char> content)
    {
        GraphEncoder.Transfer(styledFormatting, content, sb);
        return MarkContentEnd();
    }
    
    public GraphTrackingBuilder AppendContent(string content)
    {
        if (currentNodeRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        GraphEncoder.Transfer(styledFormatting, content, sb);
        return MarkContentEnd();
    }

    public GraphTrackingBuilder AppendContent(char toRepeat, int repeatTimes)
    {
        if (currentNodeRanges is { FromStartContentStart: null }) { MarkContentStart(); }
        sb.Append(toRepeat, repeatTimes);
        return MarkContentEnd();
    }

    public ContentSeparatorRanges StartAppendContentAndComplete(string content, IStringBuilder writeBuffer
      , IStyledTypeFormatting styledTypeFormatting, FieldContentHandling formatFlags,  bool allowEmptyContent = false)
    {
        sb               = writeBuffer;
        styledFormatting = styledTypeFormatting;
        ResetCurrent(formatFlags, allowEmptyContent);
        MarkContentStart();
        AppendContent(content);
        return SnapshotLastAppendSequence(formatFlags);
    }

    public GraphTrackingBuilder AppendPadding(string separator)
    {
        if (currentNodeRanges is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            MarkContentEnd();
            MarkSeparatorEnd();
        }
        else if (currentNodeRanges is { FromStartSeparatorEnd: null }) { MarkSeparatorEnd(); }
        GraphEncoder.Transfer(styledFormatting, separator, sb);
        return TagPaddingEnd();
    }

    public GraphTrackingBuilder AppendPadding(char toRepeat, int repeatTimes)
    {
        if (currentNodeRanges is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            MarkContentEnd();
            MarkSeparatorEnd();
        }
        else if (currentNodeRanges is { FromStartSeparatorEnd: null }) { MarkSeparatorEnd(); }
        sb.Append(toRepeat, repeatTimes);
        return TagPaddingEnd();
    }

    public GraphTrackingBuilder AppendSeparator(string separator)
    {
        if (currentNodeRanges is { FromStartContentEnd: null }) { MarkContentEnd(); }
        GraphEncoder.Transfer(styledFormatting, separator, sb);
        return MarkSeparatorEnd();
    }

    public ContentSeparatorRanges AppendPaddingAndComplete(string content, FieldContentHandling formatFlags)
    {
        GraphEncoder.Transfer(styledFormatting, content, sb);
        return Complete(formatFlags);
    }

    public GraphTrackingBuilder MarkSeparatorEnd()
    {
        currentNodeRanges.FromStartSeparatorEnd = sb.Length;
        return this;
    }

    protected GraphTrackingBuilder TagPaddingEnd()
    {
        currentNodeRanges.FromStartPaddingEnd = sb.Length;
        return this;
    }

    public ContentSeparatorRanges ContentEndToRanges(FieldContentHandling formatFlags)
    {
        currentNodeRanges.FromStartSeparatorEnd = sb.Length;
        return currentNodeRanges.ToContentSeparatorFromEndRanges(sb, formatFlags);
    }

    public GraphTrackingBuilder MarkPaddingEnd(int atIndex = -1)
    {
        currentNodeRanges.FromStartPaddingEnd = atIndex < 0 ? sb.Length : atIndex;
        return this;
    }

    public ContentSeparatorRanges Complete(FieldContentHandling formatFlags)
    {
        currentNodeRanges.FromStartPaddingEnd = sb.Length;

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
                sepPaddingLen =  lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
                sb.Length     -= sepPaddingLen;
            }
            return sb.Length > 0 ? sb[^1] : '\0';
        }
        sepPaddingLen = lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
        sb.Length -= sepPaddingLen;
        if (lastRange.ContentRange == null && penUltimateRange != null)
        {
            sepPaddingLen =  lastRange.SeparatorPaddingRange?.Length(sb.Length) ?? 0;
            sb.Length     -= sepPaddingLen;
        }
        return sb.Length > 0 ? sb[^1] : '\0';
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
