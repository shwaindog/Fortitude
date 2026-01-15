// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyLogTypeFormatting : CompactLogTypeFormatting
{
    public override PrettyLogTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        base.Initialize(graphTrackingBuilder, styleOptions);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb              = moldInternal.Sb;
        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            buildingType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        GraphBuilder.IndentLevel++;
        GraphBuilder.AppendContent(BrcOpn);
        if (formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder
                    .AppendPadding
                        (StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
        }

        return GraphBuilder.Complete(formatFlags);
    }

    public override int SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return 0;
        var nextPaddingSize = 0;
        if (formatFlags.UseMainFieldPadding() && formatFlags.CanAddNewLine())
        {
            nextPaddingSize =  GraphBuilder.GraphEncoder.CalculateEncodedLength(StyleOptions.NewLineStyle);
            nextPaddingSize += StyleOptions.IndentRepeat(GraphBuilder.IndentLevel);
        }
        else
        {
            nextPaddingSize =  GraphBuilder.GraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding);
        }
        return nextPaddingSize;
    }

    
    public override ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        if (formatFlags.UseMainFieldPadding() && formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            GraphBuilder.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.AlternateFieldPadding);
        }
        return GraphBuilder.Complete(formatFlags);
    }

    public override ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.IndentLevel--;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendContent(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendContent(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
        }
        GraphBuilder.AppendContent(BrcCls);
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.IndentLevel++;
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType, callerFormattingFlags);
        GraphBuilder.Complete(callerFormattingFlags);
        return sb;
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.IndentLevel--;

        if (callerFormattingFlags.UseMainFieldPadding())
        {
            if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && callerFormattingFlags.CanAddNewLine())
            {
                GraphBuilder.AppendContent(StyleOptions.NewLineStyle);

                if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
                {
                    GraphBuilder.AppendContent(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
                }
            }
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.AlternateFieldPadding);
        }
        GraphBuilder.AppendContent(BrcCls);
        return sb;
    }
    
    public override IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType
      , bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!hasItems.HasValue)
        {
            GraphBuilder.MarkContentEnd();
            return sb;
        }
    
        // Log always shows each collection and field name
        // if (formatFlags.DoesNotHaveSuppressOpening())
        // {
        if ((itemElementType == typeof(char) && StyleOptions.CharBufferWritesAsCharCollection)
            || (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String))
        {
            GraphBuilder.AppendContent(DblQt);
            return sb;
        }
        GraphBuilder.IndentLevel++;

        base.FormatCollectionStart(moldInternal, itemElementType, hasItems, collectionType, formatFlags);
        return sb;
    }

    public override ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        if (formatFlags.UseMainItemPadding())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            GraphBuilder.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.AlternateFieldPadding);
        }
        return GraphBuilder.Complete(formatFlags);
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent) 
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (fmtFlgs.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (fmtFlgs.UseMainFieldPadding())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            GraphBuilder.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.AlternateFieldPadding);
        }
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent) 
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (fmtFlgs.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.MarkSeparatorEnd();
        var charsAdded = 0;
        if (fmtFlgs.UseMainFieldPadding())
        {
            charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
            charsAdded += destSpan.OverWriteRepatAt(atIndex + charsAdded, StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
        }
        else
        {
            charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.AlternateFieldPadding);
        }
        GraphBuilder.MarkPaddingEnd(atIndex + charsAdded).Complete(fmtFlgs);
        return charsAdded;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount , string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
                GraphBuilder.Complete(formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            return sb;
        }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        // Log always shows each collection and field name
        // if (prevFmtFlags.DoesNotHaveSuppressClosing())
        // {
        if (prevFmtFlags.HasAsStringContentFlag() && itemElementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                GraphBuilder.AppendContent(DblQt);
            }
            return sb;
        }
        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.IndentLevel--;
    
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (totalItemCount > 0)
        {
            AddCollectionElementPadding(moldInternal, itemElementType, totalItemCount.Value, formatFlags);
        }
        if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcCls);
            return sb;
        }
        GraphBuilder.AppendContent(SqBrktCls);
        // }
        return sb;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded        = 0;
        var originalDestIndex = destIndex;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        GraphBuilder.ResetCurrent((FormatFlags)formatSwitches, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        
        // Log always shows each collection and field name
        //if (prevFmtFlags.DoesNotHaveSuppressClosing())
        //{
        if (prevFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                charsAdded += GraphBuilder.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
            }
            GraphBuilder.MarkContentEnd(destIndex + charsAdded);
            return charsAdded;
        }
        GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        GraphBuilder.IndentLevel--;
    
        GraphBuilder.ResetCurrent(prevFmtFlags, true);
        GraphBuilder.MarkContentStart();
        if (itemsCount > 0)
        {
            charsAdded += AddCollectionElementPadding(elementType, destSpan, destIndex, itemsCount,  formatSwitches);
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcCls);
        }
        else
        {
            GraphBuilder.AppendContent(SqBrktCls);
        }
        GraphBuilder.MarkContentEnd();
        //}
        return destIndex + charsAdded - originalDestIndex;
    }
}
