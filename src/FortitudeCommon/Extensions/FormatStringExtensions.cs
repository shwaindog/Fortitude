namespace FortitudeCommon.Extensions;

public static class ExtendedSpanFormattableExtensions
{
    public static int LayoutStringRangeIndexers(this ReadOnlySpan<char> layout, out Range charRange)
    {
        charRange = Range.All;
        var foundAt = layout.ExtractRangeFromSliceExpression(out var nullableCharRange);

        if (foundAt >= 0)
        {
            charRange = nullableCharRange!.Value;
        }
        return foundAt;
    }

    public static int ExtractRangeFromSliceExpression(this ReadOnlySpan<char> layout, out Range? charRange)
    {
        Index startIndex = Index.Start;
        Index endIndex   = Index.End;

        var foundFromStringLength = layout.IndexOf("[");

        charRange = null;
        if (foundFromStringLength < 0)
        {
            return foundFromStringLength;
        }

        if (layout.Length > foundFromStringLength
         && (layout[foundFromStringLength + 1].IsDigit() || layout[foundFromStringLength + 1].IsCarat()))
        {
            
            var fromIndexSlice = layout.ExtractIndexSlice(foundFromStringLength + 1);
            if (fromIndexSlice.Length > 0)
            {
                startIndex = fromIndexSlice.ToIndex();
            }
        }
        var foundCapStringLength = layout.IndexOf("..");
        if (foundCapStringLength >= 0
         && layout.Length > foundCapStringLength
         && (layout[foundCapStringLength + 2].IsDigit() || layout[foundCapStringLength + 2].IsCarat()))
        {
            var toIndexSlice = layout.ExtractIndexSlice(foundCapStringLength + 2);
            if (toIndexSlice.Length > 0)
            {
                endIndex = toIndexSlice.ToIndex(true);
            }
        }
        charRange = new Range(startIndex, endIndex);
        return foundFromStringLength;
    }

    public static int PadAndAlign(this Span<char> destination, ReadOnlySpan<char> toInsert, ReadOnlySpan<char> layout)
    {
        if (layout.Length == 0)
        {
            destination.Append(toInsert);
            return  toInsert.Length;
        }
        var charsRange     = Range.All;
        var padding        = 0;
        var isLeftAligned  = layout[0].IsMinus();
        var isStringLength = layout[0].IsDigit() || isLeftAligned;
        if (isStringLength)
        {
            layout.LayoutStringRangeIndexers(out var range);
            charsRange = range.IsAllRange() 
                ? range 
                : new Range(
                            range.Start.IsFromEnd 
                                ? Index.FromEnd( Math.Clamp(range.Start.Value, 0,  toInsert.Length))
                                : Index.FromStart(Math.Clamp(range.Start.Value, 0,  toInsert.Length))
                          , range.Start.IsFromEnd 
                                ? Index.FromEnd(Math.Clamp(range.End.Value, 0,  toInsert.Length))
                                : Index.FromStart(Math.Clamp(range.End.Value, 0,  toInsert.Length)));
            
            int.TryParse(layout, out padding);
            padding = Math.Abs(padding);
        }
        var rangeCappedInsert = toInsert[charsRange];
        var rangeCappedSize  = rangeCappedInsert.Length;
        if (isLeftAligned)
        {
            destination.Append(rangeCappedInsert);
            for (int i = rangeCappedSize; i < padding; i++)
            {
                destination[i] = ' ';
            }
            return Math.Max(rangeCappedSize, padding);
        }
        var extraRequired = padding - rangeCappedSize;
        int addValueIndex = 0;
        for (var i = extraRequired - 1; i >= 0; i--)
        {
            destination[i] = ' ';
            addValueIndex++;
        }
        destination.OverWriteAt(addValueIndex, rangeCappedInsert);
        return Math.Max(rangeCappedSize, padding);
    }
    
    
}
