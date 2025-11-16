// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public readonly record struct SeparatorPaddingRanges(Range? SeparatorRange = null, Range? PaddingRange = null)
{
    public Range? EntireRange
    {
        get
        {
            if (SeparatorRange == null && PaddingRange == null) return null;
            if (SeparatorRange != null && PaddingRange == null) return SeparatorRange;
            if (SeparatorRange == null && PaddingRange != null) return PaddingRange;
            return new Range(SeparatorRange!.Value.Start, PaddingRange!.Value.End);
        }
    }

    public static SeparatorPaddingRanges OnlyPadding(Range paddingRange)
    {
        return new SeparatorPaddingRanges(null, paddingRange);
    }

    public static SeparatorPaddingRanges OnlySeparator(Range separatorRange)
    {
        return new SeparatorPaddingRanges(separatorRange);
    }

    public int Length => EntireRange?.Length() ?? 0;
};

public readonly record struct ContentSeparatorRanges(FieldContentHandling PreviousFormatFlags, Range? ContentRange, SeparatorPaddingRanges? SeparatorPaddingRange = null)
{
    public static ContentSeparatorRanges None => new ();
};

public static class ContentSeparatorRangesExtensions
{
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContentSeparatorRanges ToContentSeparatorRanges(this Range contentRange, FieldContentHandling formatFlags) => 
        new (formatFlags, contentRange);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContentSeparatorRanges AddSeparatorPaddingRanges(this ContentSeparatorRanges contentRange
      , SeparatorPaddingRanges separatorPaddingRanges)
    {
        var updatedContentRange = contentRange.ContentRange;
        if (updatedContentRange != null)
        {
            var contRange = updatedContentRange.Value;
            if (separatorPaddingRanges.SeparatorRange != null)
            {
                var sepRange = separatorPaddingRanges.SeparatorRange.Value;
                updatedContentRange = new Range(new Index(contRange.Start.Value + sepRange.Start.Value, true)
                                               , new Index(sepRange.Start.Value, true));
            }
            else if (separatorPaddingRanges.PaddingRange != null)
            {
                var padRange = separatorPaddingRanges.PaddingRange.Value;
                updatedContentRange = new Range(new Index(contRange.Start.Value + padRange.Start.Value, true)
                                               , new Index(padRange.Start.Value, true));
            }
        }

        return  contentRange with {ContentRange = updatedContentRange, SeparatorPaddingRange = separatorPaddingRanges};
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContentSeparatorRanges NoContentSeparatorOrPadding(this IStringBuilder sb, ITypeMolderDieCast updateLast) => 
        updateLast.LastContentSeparatorPaddingRanges = ContentSeparatorRanges.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContentSeparatorRanges NoContentSeparatorOrPadding(this char lastContentChar, ITypeMolderDieCast updateLast) => 
        updateLast.LastContentSeparatorPaddingRanges = ContentSeparatorRanges.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char RemoveLastSeparatorAndPadding(this IStringBuilder sb, ContentSeparatorRanges contentRange)
    {
        if(contentRange.SeparatorPaddingRange == null) return sb.Length > 0 ? sb[^1] : '\0';
        var sepPaddingLen = contentRange.SeparatorPaddingRange?.Length ?? 0;
        sb.Length -= sepPaddingLen;
        return sb.Length > 0 ? sb[^1] : '\0';
    }
}
