// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public struct ContentSeparatorPaddingRangeTracking
{
    public FieldContentHandling FormatFlags;

    public bool AllowEmptyContent;
    public int? FromStartContentStart;
    public int? FromStartContentEnd;
    public int? FromStartSeparatorEnd;
    public int? FromStartPaddingEnd;

    public void Reset()
    {
        FormatFlags = FieldContentHandling.DefaultCallerTypeFlags;

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
        if (tracking is { FromStartContentStart: not null, FromStartContentEnd: not null })
        {
            var contentStartIndexFromEnd = new Index(Math.Abs(sbLength - tracking.FromStartContentStart.Value), true);
            var contentEndIndexFromEnd   = new Index(Math.Abs(sbLength - tracking.FromStartContentEnd.Value), true);
            if (contentEndIndexFromEnd.Value < contentStartIndexFromEnd.Value ||
                (tracking.AllowEmptyContent && contentEndIndexFromEnd.Value == contentStartIndexFromEnd.Value))
                contentRange = new Range(contentStartIndexFromEnd, contentEndIndexFromEnd);
        }
        Range? separatorRange = null;
        if (tracking is { FromStartContentEnd: not null, FromStartSeparatorEnd: not null })
        {
            var separatorStartIndexFromEnd = new Index(Math.Abs(sbLength - tracking.FromStartContentEnd.Value), true);
            var separatorEndIndexFromEnd   = new Index(Math.Abs(sbLength - tracking.FromStartSeparatorEnd.Value), true);
            if (separatorEndIndexFromEnd.Value < separatorStartIndexFromEnd.Value)
                separatorRange = new Range(separatorStartIndexFromEnd, separatorEndIndexFromEnd);
        }
        Range? paddingRange = null;
        if (tracking is { FromStartPaddingEnd: not null } and ({ FromStartSeparatorEnd: not null } or { FromStartContentEnd : not null }))
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
