// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public struct ContentSeparatorPaddingRangeTracking
{
    public int? FromStartContentStart;
    public int? FromStartContentEnd;
    public int? FromStartSeparatorEnd;
    public int? FromStartPaddingEnd;

    public void Reset()
    {
        FromStartContentStart = null;
        FromStartContentEnd = null;
        FromStartSeparatorEnd = null;
        FromStartPaddingEnd = null;
    }
}

public static class ContentSeparatorPaddingRangeTrackingExtensions
{
    public static IStringBuilder MarkContentStart(this IStringBuilder sb, IStyledTypeFormatting styledFormatting)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartContentStart = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return sb;
    }
    
    public static IStringBuilder MarkContentEnd(this IStringBuilder sb, IStyledTypeFormatting styledFormatting)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartContentEnd = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return sb;
    }
    
    public static IStringBuilder StartAppendContentExpectMore(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting)
    {
        styledFormatting.ContentSeparatorPaddingTracking.Reset();
        sb.MarkContentStart(styledFormatting);
        styledFormatting.ContentEncoder.Transfer(styledFormatting, content, sb);
        return sb;
    }
    
    public static IStringBuilder StartAppendLastContent(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting)
    {
        styledFormatting.ContentSeparatorPaddingTracking.Reset();
        sb.MarkContentStart(styledFormatting);
        return sb.AppendLastContent(content, styledFormatting);
    }
    
    public static IStringBuilder AppendContentExpectMore
        (this IStringBuilder sb, string separator, IStyledTypeFormatting styledFormatting)
    {
        var tracking = styledFormatting.ContentSeparatorPaddingTracking;
        if (tracking is { FromStartContentStart: null })
        {
            sb.MarkContentStart(styledFormatting);
        }
        styledFormatting.ContentEncoder.Transfer(styledFormatting, separator, sb);
        return sb;
    }
    
    public static IStringBuilder AppendContentExpectMore
        (this IStringBuilder sb, char toRepeat, int repeatTimes, IStyledTypeFormatting styledFormatting)
    {
        var tracking = styledFormatting.ContentSeparatorPaddingTracking;
        if (tracking is { FromStartContentStart: null })
        {
            sb.MarkContentStart(styledFormatting);
        }
        sb.Append(toRepeat, repeatTimes);
        return sb;
    }
    
    public static IStringBuilder ContinueAppendContentExpectMore(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting)
    {
        styledFormatting.ContentEncoder.Transfer(styledFormatting, content, sb);
        return sb;
    }
    
    public static ContentSeparatorRanges JustAppendContentAndComplete(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting
      , ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags)
    {
        styledFormatting.ContentSeparatorPaddingTracking.Reset();
        sb.MarkContentStart(styledFormatting);
        sb.AppendLastContent(content, styledFormatting);
        return styledFormatting.ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, formatFlags);
    }
    
    public static IStringBuilder AppendLastContent(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting)
    {
        styledFormatting.ContentEncoder.Transfer(styledFormatting, content, sb);
        return sb.MarkContentEnd(styledFormatting);
    }
    
    public static IStringBuilder AppendSeparatorExpectMore
        (this IStringBuilder sb, string separator, IStyledTypeFormatting styledFormatting)
    {
        if (styledFormatting.ContentSeparatorPaddingTracking.FromStartContentEnd == null)
        {
            sb.MarkContentEnd(styledFormatting);
        }
        styledFormatting.ContentEncoder.Transfer(styledFormatting, separator, sb);
        return sb;
    }
    
    public static IStringBuilder AppendPaddingExpectMore
        (this IStringBuilder sb, string separator, IStyledTypeFormatting styledFormatting)
    {
        var tracking = styledFormatting.ContentSeparatorPaddingTracking;
        if (tracking is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            sb.MarkContentEnd(styledFormatting);
            sb.MarkSeparatorEnd(styledFormatting);
        } else if (tracking is { FromStartSeparatorEnd: null })
        {
            sb.MarkSeparatorEnd(styledFormatting);
        }
        styledFormatting.ContentEncoder.Transfer(styledFormatting, separator, sb);
        return sb;
    }
    
    public static IStringBuilder AppendPaddingExpectMore
        (this IStringBuilder sb, char toRepeat, int repeatTimes, IStyledTypeFormatting styledFormatting)
    {
        var tracking = styledFormatting.ContentSeparatorPaddingTracking;
        if (tracking is { FromStartContentStart: not null, FromStartContentEnd: null })
        {
            sb.MarkContentEnd(styledFormatting);
            sb.MarkSeparatorEnd(styledFormatting);
        } else if (tracking is { FromStartSeparatorEnd: null })
        {
            sb.MarkSeparatorEnd(styledFormatting);
        }
        sb.Append(toRepeat, repeatTimes);
        return sb;
    }
    
    public static IStringBuilder AppendLastSeparator(this IStringBuilder sb, string separator, IStyledTypeFormatting styledFormatting)
    {
        styledFormatting.ContentEncoder.Transfer(styledFormatting, separator, sb);
        return sb.MarkSeparatorEnd(styledFormatting);
    }
    
    public static ContentSeparatorRanges AppendLastPadding(this IStringBuilder sb, string content, IStyledTypeFormatting styledFormatting
      , ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags)
    {
        styledFormatting.ContentEncoder.Transfer(styledFormatting, content, sb);
        sb.MarkPaddingEnd(styledFormatting);
        return moldInternal.LastContentSeparatorPaddingRanges = 
            styledFormatting.ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, formatFlags);
    }
    
    public static IStringBuilder MarkPaddingStart(this IStringBuilder sb, IStyledTypeFormatting styledFormatting)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartContentEnd          = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return sb;
    }
    
    public static IStringBuilder MarkSeparatorEnd(this IStringBuilder sb, IStyledTypeFormatting styledFormatting)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartSeparatorEnd        = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return sb;
    }
    
    public static ContentSeparatorRanges ContentEndToRanges(this IStringBuilder sb
      , IStyledTypeFormatting styledFormatting, ITypeMolderDieCast toUpdate, FieldContentHandling formatFlags)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartSeparatorEnd        = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return toUpdate.LastContentSeparatorPaddingRanges = contentSepPadRanges.ToContentSeparatorFromEndRanges(toUpdate, formatFlags);
    }
    
    public static IStringBuilder MarkPaddingEnd(this IStringBuilder sb, IStyledTypeFormatting styledFormatting)
    {
        var contentSepPadRanges = styledFormatting.ContentSeparatorPaddingTracking;
        contentSepPadRanges.FromStartPaddingEnd          = sb.Length;
        styledFormatting.ContentSeparatorPaddingTracking = contentSepPadRanges;
        return sb;
    }

    public static ContentSeparatorRanges ToContentSeparatorFromEndRanges
        (this ContentSeparatorPaddingRangeTracking tracking, ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags)
    {
        var sb       = moldInternal.Sb;
        var sbLength = sb.Length;
        
        Range? contentRange = null;
        if (tracking is { FromStartContentStart: not null, FromStartContentEnd: not null })
        {
            var contentStartIndexFromEnd = new Index(Math.Abs( sbLength - tracking.FromStartContentStart.Value), true);
            var contentEndIndexFromEnd = new Index(Math.Abs( sbLength - tracking.FromStartContentEnd.Value), true);
            if(contentEndIndexFromEnd.Value > contentStartIndexFromEnd.Value)
                contentRange = new Range(contentStartIndexFromEnd, contentEndIndexFromEnd);
        }
        Range? separatorRange = null;
        if (tracking is { FromStartContentEnd: not null, FromStartSeparatorEnd: not null })
        {
            var separatorStartIndexFromEnd = new Index(Math.Abs( sbLength - tracking.FromStartContentEnd.Value), true);
            var separatorEndIndexFromEnd   = new Index(Math.Abs( sbLength - tracking.FromStartSeparatorEnd.Value), true);
            if(separatorEndIndexFromEnd.Value > separatorStartIndexFromEnd.Value)
                separatorRange = new Range(separatorStartIndexFromEnd, separatorEndIndexFromEnd);
        }
        Range? paddingRange = null;
        if (tracking is { FromStartPaddingEnd: not null } and ({ FromStartSeparatorEnd: not null} or {FromStartContentEnd : not null})  )
        {
            var sepEndOrContentEnd       = tracking.FromStartSeparatorEnd ?? tracking.FromStartContentEnd;
            var paddingStartIndexFromEnd = new Index(Math.Abs( sbLength - sepEndOrContentEnd!.Value), true);
            var paddingEndIndexFromEnd   = new Index(Math.Abs( sbLength - tracking.FromStartPaddingEnd.Value), true);
            if(paddingEndIndexFromEnd.Value > paddingStartIndexFromEnd.Value)
                paddingRange = new Range(paddingStartIndexFromEnd, paddingEndIndexFromEnd);
        }
        var hasSeparatorOrPadding = separatorRange is not null && paddingRange is not null;
        
        return moldInternal.LastContentSeparatorPaddingRanges = 
            new ContentSeparatorRanges(formatFlags, contentRange, hasSeparatorOrPadding ? new( separatorRange, paddingRange ) : null);
    }
}
