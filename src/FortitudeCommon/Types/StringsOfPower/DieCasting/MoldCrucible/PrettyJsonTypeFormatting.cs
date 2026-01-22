// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
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

    public override PrettyJsonTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions, IStringBuilder sb)
    {
        base.Initialize(graphTrackingBuilder, styleOptions, sb);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (formatFlags.DoesNotHaveAsEmbeddedContentFlags())
        {
            Gb.IndentLevel++;
            Gb.StartAppendContent(BrcOpn, sb, this, formatFlags);
        }
        if (formatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendPadding(StyleOptions.IndentChar
                               , StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }
        return Gb.Complete(formatFlags);
    }

    public override int SizeFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        Gb.GraphEncoder.CalculateEncodedLength(ClnSpc);

    public override SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        Gb
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;


    public override int SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return 0;
        int nextPaddingSize;
        if (formatFlags.UseMainFieldPadding() && formatFlags.CanAddNewLine())
        {
            nextPaddingSize =  Gb.GraphEncoder.CalculateEncodedLength(StyleOptions.NewLineStyle);
            nextPaddingSize += StyleOptions.IndentRepeat(Gb.IndentLevel);
        }
        else { nextPaddingSize = Gb.GraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding); }
        return nextPaddingSize;
    }

    public override ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return Gb.Complete(formatFlags);
        if (formatFlags.UseMainFieldPadding() && formatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
        }
        else { Gb.AppendPadding(StyleOptions.AlternateFieldPadding); }
        return Gb.Complete(formatFlags);
    }

    public override ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (moldInternal.Master.CallerContext.FormatFlags.HasSuppressClosing()) { return Gb.LastContentSeparatorPaddingRanges; }
        var lastNonWhiteSpace = Gb.RemoveLastSeparatorAndPadding();
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlags()) Gb.IndentLevel--;

        var paddedCloseStartIndex = sb.Length;
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }
        Gb.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(paddedCloseStartIndex);
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlags()) { Gb.AppendContent(BrcCls); }
        Gb.MarkContentEnd(sb.Length);
        return Gb.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }


    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if (callerFormattingFlags.HasSuppressOpening()) { return sb; }
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType, callerFormattingFlags);

        Gb.IndentLevel++;

        if (callerFormattingFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);

            if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendPadding(StyleOptions.IndentChar
                               , StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }
        Gb.Complete(callerFormattingFlags);
        return sb;
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if (callerFormattingFlags.HasSuppressClosing()) { return sb; }
        var lastNonWhiteSpace = Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel--;

        var paddedCloseStartIndex = sb.Length;
        if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && callerFormattingFlags.CanAddNewLine())
        {
            Gb.AppendContent(StyleOptions.NewLineStyle);

            if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendContent(StyleOptions.IndentChar
                               , StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(paddedCloseStartIndex);
        Gb.AppendContent(BrcCls);
        Gb.MarkContentEnd(sb.Length);
        return sb;
    }

    public override IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType
      , bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!hasItems.HasValue)
        {
            Gb.MarkContentEnd();
            return sb;
        }
        return CollectionStart(itemElementType, sb, hasItems.Value).ToStringBuilder(sb);
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        var currFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, currFmtFlags);
            if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            {
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            Gb.IndentLevel++;
            Gb.AppendContent(elementType == typeof(KeyValuePair<string, JsonNode>) ? BrcOpn : SqBrktOpn);
            AddCollectionElementPadding(elementType, sb, 1, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        int charsAdded = 0;

        var currFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent(currFmtFlags);
            if ((elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) ||
                (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String))
            {
                charsAdded = Gb.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destStartIndex);
                return charsAdded;
            }
            Gb.IndentLevel++;
            charsAdded =
                Gb
                    .GraphEncoder
                    .OverwriteTransfer
                        ( elementType == typeof(KeyValuePair<string, JsonNode>) 
                             ? BrcOpn 
                             : SqBrktOpn, destSpan, destStartIndex);
            charsAdded += AddCollectionElementPadding(elementType, destSpan, destStartIndex + charsAdded, 1, formatSwitches);
        }

        return charsAdded;
    }

    public override ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return Gb.Complete(formatFlags); }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return Gb.Complete(formatFlags); }
        if (formatFlags.UseMainItemPadding() && formatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
        }
        else { Gb.AppendPadding(StyleOptions.AlternateFieldPadding); }
        return Gb.Complete(formatFlags);
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (fmtFlgs.HasNoFieldPaddingFlag()) return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (fmtFlgs.UseMainFieldPadding() && fmtFlgs.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
        }
        else { Gb.AppendPadding(StyleOptions.AlternateFieldPadding); }
        return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (fmtFlgs.HasNoFieldPaddingFlag()) return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.MarkSeparatorEnd();
        var charsAdded = 0;
        if (fmtFlgs.UseMainFieldPadding() && fmtFlgs.CanAddNewLine())
        {
            charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
            charsAdded += destSpan.OverWriteRepatAt(atIndex + charsAdded, StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
        }
        else { charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.AlternateFieldPadding); }
        Gb.MarkPaddingEnd(atIndex + charsAdded).Complete(fmtFlgs);
        return charsAdded;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
                Gb.Complete(formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            return sb;
        }
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((itemElementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || formatFlags.HasAsStringContentFlag())))
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                return sb;
            }
            else if (itemElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt);
                return sb;
            }
            sb.RemoveLastWhiteSpacedCommaIfFound();

            Gb.IndentLevel--;

            Gb.StartNextContentSeparatorPaddingSequence(sb, prevFmtFlags, true);
            if (totalItemCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(itemElementType, sb, resultsFoundCount ?? 0); }
            }
            if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
            {
                Gb.AppendContent(BrcCls);
                return sb;
            }
            Gb.AppendContent(SqBrktCls);
        }
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var preAppendLen = sb.Length;
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((elementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.HasAsStringContentFlag())))
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                    prevFmtFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            else if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                CompleteBase64Sequence(sb);
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            Gb.RemoveLastSeparatorAndPadding();
            Gb.IndentLevel--;

            Gb.StartNextContentSeparatorPaddingSequence(sb, prevFmtFlags, true);
            if (itemsCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(elementType, sb, itemsCount); }
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>)) { Gb.AppendContent(BrcCls); }
            else { Gb.AppendContent(SqBrktCls); }
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
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if (prevFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
            {
                if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
                {
                    charsAdded += Gb.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
                }
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            else if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded = CompleteBase64Sequence(destSpan, destIndex);
                destSpan.OverWriteAt(destIndex + charsAdded, DblQt);
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            Gb.IndentLevel--;

            Gb.ResetCurrent(prevFmtFlags, true);
            Gb.MarkContentStart();
            if (itemsCount > 0)
            {
                if (prevFmtFlags.CanAddNewLine()) { charsAdded += AddCollectionElementPadding(elementType, destSpan, destIndex, itemsCount); }
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                charsAdded += Gb.GraphEncoder.OverwriteTransfer(BrcCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
            }
            else
            {
                charsAdded += Gb.GraphEncoder.OverwriteTransfer(SqBrktCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
            }
        }
        return destIndex + charsAdded - originalDestIndex;
    }
    
    public override PrettyJsonTypeFormatting Clone()
    {
        return Recycler.Borrow<PrettyJsonTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        return CopyFrom((PrettyJsonTypeFormatting)source, copyMergeFlags);
    }

    public override PrettyJsonTypeFormatting CopyFrom(CompactJsonTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }
}
