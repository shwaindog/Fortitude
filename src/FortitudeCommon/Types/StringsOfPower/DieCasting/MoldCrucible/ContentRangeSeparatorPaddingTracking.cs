// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public struct ContentSeparatorPaddingRangeTracking
{
    public FieldContentHandling StartedWithFormatFlags;

    public bool AllowEmptyContent;
    public int? FromStartContentStart;
    public int? FromStartContentEnd;
    public int? FromStartSeparatorEnd;
    public int? FromStartPaddingEnd;

    public Range? CurrentContentRange
    {
        get
        {
            
            if (this is { FromStartContentStart: not null, FromStartContentEnd: not null })
            {
                var contentStart = new Index(Math.Abs(FromStartContentStart.Value));
                var contentEnd   = new Index(Math.Abs(FromStartContentEnd.Value));
                if (contentStart.Value < contentEnd.Value || (AllowEmptyContent && contentStart.Value == contentEnd.Value))
                    return new Range(contentStart, contentEnd);
            }
            return null;
        }  
    } 
    public Range? CurrentSeparatorRange
    {
        get
        {
            
            if (this is { FromStartContentEnd: not null, FromStartSeparatorEnd: not null })
            {
                var separatorStart = new Index(Math.Abs(FromStartContentEnd.Value));
                var separatorEnd   = new Index(Math.Abs(FromStartSeparatorEnd.Value));
                if (separatorStart.Value < separatorEnd.Value || (AllowEmptyContent && separatorStart.Value == separatorEnd.Value))
                    return new Range(separatorStart, separatorEnd);
            }
            return null;
        }  
    } 
    public Range? CurrentPaddingRange
    {
        get
        {
            
            if (this is { FromStartPaddingEnd: not null } and ({ FromStartSeparatorEnd: not null } or { FromStartContentEnd : not null }))
            {
                var sepEndOrContentEnd = FromStartSeparatorEnd ?? FromStartContentEnd!.Value;
                var paddingStart     = new Index(Math.Abs(sepEndOrContentEnd));
                var paddingEnd       = new Index(Math.Abs(FromStartPaddingEnd.Value));
                if (paddingStart.Value < paddingEnd.Value || (AllowEmptyContent && paddingStart.Value == paddingEnd.Value))
                    return new Range(paddingStart, paddingEnd);
            }
            return null;
        }  
    }

    public bool HasContent => this is { FromStartContentStart: not null, FromStartContentEnd: not null };
    public bool HasSeparator => this is { FromStartContentEnd: not null, FromStartSeparatorEnd: not null };
    public bool HasPadding => this is  { FromStartPaddingEnd: not null } 
                                   and ({ FromStartSeparatorEnd: not null } 
                                     or { FromStartContentEnd : not null });

    public void Reset()
    {
        StartedWithFormatFlags = FieldContentHandling.DefaultCallerTypeFlags;

        AllowEmptyContent     = false;
        FromStartContentStart = null;
        FromStartContentEnd   = null;
        FromStartSeparatorEnd = null;
        FromStartPaddingEnd   = null;
    }
}

public static class ContentSeparatorPaddingRangeTrackingExtensions
{
    public static ContentSeparatorRanges ToContentSeparatorFromEndRanges
        (this ContentSeparatorPaddingRangeTracking tracking, IStringBuilder sb, FieldContentHandling formatFlags)
    {
        var sbLength = sb.Length;

        Range? contentRange = null;
        if (tracking.HasContent)
        {
            var contentStartIndexFromEnd = new Index(Math.Abs(sbLength - tracking.FromStartContentStart.Value), true);
            var contentEndIndexFromEnd   = new Index(Math.Abs(sbLength - tracking.FromStartContentEnd.Value), true);
            if (contentEndIndexFromEnd.Value < contentStartIndexFromEnd.Value ||
                (tracking.AllowEmptyContent && contentEndIndexFromEnd.Value == contentStartIndexFromEnd.Value))
                contentRange = new Range(contentStartIndexFromEnd, contentEndIndexFromEnd);
        }
        Range? separatorRange = null;
        if (tracking.HasSeparator)
        {
            var separatorStartIndexFromEnd = new Index(Math.Abs(sbLength - tracking.FromStartContentEnd.Value), true);
            var separatorEndIndexFromEnd   = new Index(Math.Abs(sbLength - tracking.FromStartSeparatorEnd.Value), true);
            if (separatorEndIndexFromEnd.Value < separatorStartIndexFromEnd.Value)
                separatorRange = new Range(separatorStartIndexFromEnd, separatorEndIndexFromEnd);
        }
        Range? paddingRange = null;
        if (tracking.HasPadding)
        {
            var sepEndOrContentEnd       = tracking.FromStartSeparatorEnd ?? tracking.FromStartContentEnd;
            var paddingStartIndexFromEnd = new Index(Math.Abs(sbLength - sepEndOrContentEnd!.Value), true);
            var paddingEndIndexFromEnd   = new Index(Math.Abs(sbLength - tracking.FromStartPaddingEnd.Value), true);
            if (paddingEndIndexFromEnd.Value < paddingStartIndexFromEnd.Value)
                paddingRange = new Range(paddingStartIndexFromEnd, paddingEndIndexFromEnd);
        }
        var hasSeparatorOrPadding = separatorRange is not null || paddingRange is not null;

        return new ContentSeparatorRanges(formatFlags, contentRange, hasSeparatorOrPadding
                                              ? new(separatorRange, paddingRange)
                                              : null);
    }
}
