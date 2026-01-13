// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyJsonTypeFormatting : CompactJsonTypeFormatting
{
    protected const string CmaSpc = ", ";
    protected const string ClnSpc = ": ";
    protected const string Spc    = " ";

    public override PrettyJsonTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        base.Initialize(graphTrackingBuilder, styleOptions);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (formatFlags.DoesNotHaveAsEmbeddedContentFlags())
        { 
            GraphBuilder.IndentLevel++;
            GraphBuilder.StartAppendContent(BrcOpn, sb, this, formatFlags);
        }
        if (formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                                   , StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
        }
        return GraphBuilder.Complete(formatFlags);
    }

    public override SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding( Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

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
        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;
        if(moldInternal.Master.CallerContext.FormatFlags.HasSuppressClosing())
        {
            return GraphBuilder.LastContentSeparatorPaddingRanges;
        }
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.IndentLevel--; 

        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, DefaultCallerTypeFlags);
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendContent(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendContent(StyleOptions.IndentChar, StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
        }
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlags())
        {
            GraphBuilder.AppendContent(BrcCls);
        }
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }


    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if(callerFormattingFlags.HasSuppressOpening())
        {
            return sb;
        }
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType);

        GraphBuilder.IndentLevel++;

        if (callerFormattingFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
                
            if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
        }
        GraphBuilder.Complete(callerFormattingFlags);
        return sb;
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if(callerFormattingFlags.HasSuppressClosing())
        {
            return sb;
        }
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.IndentLevel--;

        if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && callerFormattingFlags.CanAddNewLine())
        {
            GraphBuilder.AppendContent(StyleOptions.NewLineStyle);

            if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendContent(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(GraphBuilder.IndentLevel));
            }
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
        return CollectionStart(itemElementType, sb, hasItems.Value).ToStringBuilder(sb);
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        var currFmtFlags = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, currFmtFlags);
            if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            {
                GraphBuilder.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                GraphBuilder.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            GraphBuilder.IndentLevel++;
            if (elementType == typeof(KeyValuePair<string, JsonNode>)) { GraphBuilder.AppendContent(BrcOpn); }
            else { GraphBuilder.AppendContent(SqBrktOpn); }
            AddCollectionElementPadding(elementType, sb, 1, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        int charsAdded = 0;
        
        var currFmtFlags = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            GraphBuilder.ResetCurrent(currFmtFlags);
            if ((elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) || 
                (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String))
            {
                charsAdded = GraphBuilder.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destStartIndex);
                return charsAdded; 
            }
            GraphBuilder.IndentLevel++;
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                charsAdded = GraphBuilder.GraphEncoder.OverwriteTransfer(BrcOpn, destSpan, destStartIndex);
            }
            else { charsAdded = GraphBuilder.GraphEncoder.OverwriteTransfer(SqBrktOpn, destSpan, destStartIndex); }
            charsAdded += AddCollectionElementPadding(elementType, destSpan, destStartIndex + charsAdded, 1, formatSwitches);
        }

        return charsAdded;
    }
    
    public override ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return GraphBuilder.Complete(formatFlags); }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return GraphBuilder.Complete(formatFlags); }
        if (formatFlags.UseMainItemPadding() && formatFlags.CanAddNewLine())
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
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (fmtFlgs.UseMainFieldPadding() && fmtFlgs.CanAddNewLine())
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
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.MarkSeparatorEnd();
        var charsAdded = 0;
        if (fmtFlgs.UseMainFieldPadding() && fmtFlgs.CanAddNewLine())
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
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb         = moldInternal.Sb;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
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
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((itemElementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || formatFlags.HasAsStringContentFlag())))
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag())
                    GraphBuilder.AppendContent(DblQt);
                return sb;
            }
            else if (itemElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                GraphBuilder.AppendContent(DblQt);
                return sb;
            }
            sb.RemoveLastWhiteSpacedCommaIfFound();

            GraphBuilder.IndentLevel--;

            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, prevFmtFlags, true);
            if (totalItemCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(itemElementType, sb, resultsFoundCount ?? 0); }
            }
            if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
            {
                GraphBuilder.AppendContent(BrcCls);
                return sb;
            }
            GraphBuilder.AppendContent(SqBrktCls);
        }
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        
        var preAppendLen = sb.Length;
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((elementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.HasAsStringContentFlag())))
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag()) GraphBuilder.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            else if (elementType.IsByte()  && JsonOptions.ByteArrayWritesBase64String)
            {
                CompleteBase64Sequence(sb);
                GraphBuilder.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            GraphBuilder.RemoveLastSeparatorAndPadding();
            GraphBuilder.IndentLevel--;
            
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, prevFmtFlags, true);
            if (itemsCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine())
                {
                    AddCollectionElementPadding(elementType, sb, itemsCount);
                }
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                GraphBuilder.AppendContent(BrcCls);
            }
            else { GraphBuilder.AppendContent(SqBrktCls); }
        }
        return sb.Length - preAppendLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded        = 0;
        var originalDestIndex = destIndex;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if (prevFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
                {
                    charsAdded += GraphBuilder.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
                }
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            else if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded = CompleteBase64Sequence(destSpan, destIndex);
                destSpan.OverWriteAt(destIndex + charsAdded, DblQt);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            GraphBuilder.IndentLevel--;

            GraphBuilder.ResetCurrent(prevFmtFlags, true);
            GraphBuilder.MarkContentStart();
            if (itemsCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine())
                {
                    charsAdded += AddCollectionElementPadding(elementType, destSpan, destIndex, itemsCount);
                }
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                charsAdded += GraphBuilder.GraphEncoder.OverwriteTransfer(BrcCls, destSpan, destIndex);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
            }
            else
            {
                charsAdded += GraphBuilder.GraphEncoder.OverwriteTransfer(SqBrktCls, destSpan, destIndex);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
            }
        }
        return destIndex + charsAdded - originalDestIndex;
    }
}
