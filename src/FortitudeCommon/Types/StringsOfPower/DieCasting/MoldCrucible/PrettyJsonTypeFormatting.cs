// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyJsonTypeFormatting : CompactJsonTypeFormatting
{
    protected const string CmaSpc = ", ";
    protected const string ClnSpc = ": ";
    protected const string Spc    = " ";

    public override PrettyJsonTypeFormatting Initialize(ITheOneString theOneString)
    {
        base.Initialize(theOneString);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
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
        LayoutEncoder.CalculateEncodedLength(ClnSpc);

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
            nextPaddingSize =  LayoutEncoder.CalculateEncodedLength(StyleOptions.NewLineStyle);
            nextPaddingSize += StyleOptions.IndentRepeat(Gb.IndentLevel);
        }
        else { nextPaddingSize = LayoutEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding); }
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

    public override ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast mdc)
    {
        var sb = mdc.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (mdc.Master.CallerContext.FormatFlags.HasSuppressClosing()) { return Gb.LastContentSeparatorPaddingRanges; }
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
        Gb.StartNextContentSeparatorPaddingSequence(mdc.Sb, DefaultCallerTypeFlags);
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

    public override ContentSeparatorRanges FinishFormatCollectionOpen(ITypeMolderDieCast mdc, Type itemElementType
      , bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (hasItems != true || mdc.WroteCollectionOpen) { return ContentSeparatorRanges.None; }

        var  reg                    = mdc.Master.ActiveGraphRegistry;
        var  hasWroteCollectionOpen = false;
        int? firstOpenIndex         = null;

        ITypeMolderDieCast? initialDc = mdc;

        if (mdc.MoldGraphVisit.HasRegisteredVisit)
        {
            GraphNodeVisit derivedMold = reg[mdc.MoldGraphVisit.VisitId.VisitIndex];
            if (mdc.MoldGraphVisit.IsBaseOfInitial)
            {
                var            checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;
                GraphNodeVisit checkMold      = reg[checkMoldIndex];
                do
                {
                    derivedMold            =   checkMold;
                    hasWroteCollectionOpen |=  checkMold.MoldState?.WroteCollectionOpen ?? false;
                    firstOpenIndex         ??= hasWroteCollectionOpen ? checkMoldIndex : null;
                    checkMoldIndex         =   reg[checkMoldIndex].ParentVisitId.VisitIndex;
                    checkMold              =   reg[Math.Max(0, checkMoldIndex)];
                } while (checkMoldIndex >= 0
                      && (checkMold.MoldState?.MoldGraphVisit.IsBaseOfInitial ?? false)
                      && ReferenceEquals(checkMold.VisitedInstance, derivedMold.VisitedInstance));
            }
            initialDc = derivedMold.MoldState;
            if (hasWroteCollectionOpen)
            {
                if (firstOpenIndex != null)
                {
                    var openDc                                      = reg[firstOpenIndex.Value].MoldState;
                    if (openDc != null) openDc.WroteCollectionClose = true;
                }
                mdc.WroteCollectionClose = false;
                return ContentSeparatorRanges.None;
            }
        }
        var sb = mdc.Sb;
        mdc.SuppressCollectionClose = false;
        mdc.WroteCollectionClose    = false;
        mdc.WroteCollectionOpen     = true;
        var inheritedFmtFlags = mdc.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        if (mdc is ICollectionMolderDieCast { IsSimple: true } scmdc)
        {
            if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mdc.TypeBeingBuilt, inheritedFmtFlags); }
            else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
            {
                if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
                {
                    AppendInstanceValuesFieldName(mdc.TypeBeingBuilt, inheritedFmtFlags);
                }
            }
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)inheritedFmtFlags | (FormatSwitches)mdc.CreateMoldFormatFlags);
        return Gb.LastContentSeparatorPaddingRanges;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var preAppendLen         = sb.Length;
        var inheritedFmtFlags    = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;
        var inheritedFmtSwitches = (FormatSwitches)inheritedFmtFlags;
        if (inheritedFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
            if ((elementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || inheritedFmtFlags.HasAsStringContentFlag())))
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag()) Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            Gb.IndentLevel++;
            Gb.AppendContent(elementType == typeof(KeyValuePair<string, JsonNode>) ? BrcOpn : SqBrktOpn);
            AddCollectionElementPadding(elementType, sb, 1, inheritedFmtSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        int charsAdded = 0;

        var inheritedFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;
        if (inheritedFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent(inheritedFmtFlags);
            if ((elementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || inheritedFmtFlags.HasAsStringContentFlag())))
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag())
                    charsAdded = LayoutEncoder.OverwriteTransfer(DblQt, destSpan, destStartIndex);
                return charsAdded;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded = LayoutEncoder.OverwriteTransfer(DblQt, destSpan, destStartIndex);
                return charsAdded;
            }
            Gb.IndentLevel++;
            charsAdded =
                LayoutEncoder
                    .OverwriteTransfer
                        (elementType == typeof(KeyValuePair<string, JsonNode>)
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
        if (elementType.IsChar() && JsonOptions.CharBufferWritesAsCharCollection) { return Gb.Complete(formatFlags); }
        if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String) { return Gb.Complete(formatFlags); }
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
        if (collectionElementType.IsChar() && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
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
        if (collectionElementType.IsChar() && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
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

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast mdc, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var inheritedFmtFlags    = mdc.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        var inheritedFmtSwitches = (FormatSwitches)inheritedFmtFlags;
        if (mdc.SkipBody || mdc.WroteCollectionClose || mdc.SuppressCollectionClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(inheritedFmtFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
            Gb.MarkContentStart(sb.Length - 1);
            Gb.MarkContentEnd(sb.Length);
            return sb;
        }
        mdc.WroteCollectionClose = true;
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
                if (!mdc.WroteCollectionOpen)
                    CollectionStart(itemElementType, sb, false, inheritedFmtSwitches);
                else
                    Gb.IndentLevel--;
                CollectionEnd(itemElementType, sb, 0, inheritedFmtSwitches);
                Gb.Complete(inheritedFmtFlags);
            }
            else
            {
                AppendFormattedNull(sb, formatString, inheritedFmtFlags);
                if (mdc.WroteCollectionOpen) Gb.IndentLevel--;
            }
            mdc.WroteCollectionOpen  = false;
            return sb;
        }
        mdc.WroteCollectionOpen  = false;

        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((itemElementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || inheritedFmtFlags.HasAsStringContentFlag())))
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag()) Gb.AppendContent(DblQt);
                return sb;
            }
            if (itemElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt);
                return sb;
            }
            Gb.RemoveLastSeparatorAndPadding();

            Gb.IndentLevel--;

            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
            if (totalItemCount > 0)
            {
                if (inheritedFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(itemElementType, sb, resultsFoundCount ?? 0); }
            }
            if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
            {
                Gb.AppendContent(BrcCls);
                return sb;
            }
            Gb.AppendContent(SqBrktCls);
        }
        Gb.Complete(inheritedFmtFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
        Gb.MarkContentStart(sb.Length - 1);
        Gb.MarkContentEnd(sb.Length);
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var preAppendLen = sb.Length;
        var inheritedFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags | (FormatFlags)formatSwitches;
        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if ((elementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || formatSwitches.HasAsStringContentFlag())))
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag()) Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                CompleteBase64Sequence(sb);
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendLen;
            }
            Gb.RemoveLastSeparatorAndPadding();
            Gb.IndentLevel--;

            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
            if (itemsCount > 0)
            {
                if (inheritedFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(elementType, sb, itemsCount); }
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
        var inheritedFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags | (FormatFlags)formatSwitches;

        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            if (inheritedFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag())
                {
                    charsAdded += LayoutEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
                }
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            else if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded = CompleteBase64Sequence(destSpan, destIndex);
                destSpan.OverWriteAt(destIndex + charsAdded, DblQt);
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            Gb.IndentLevel--;

            Gb.ResetCurrent(inheritedFmtFlags, true);
            Gb.MarkContentStart();
            if (itemsCount > 0)
            {
                if (inheritedFmtFlags.CanAddNewLine()) { charsAdded += AddCollectionElementPadding(elementType, destSpan, destIndex, itemsCount); }
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                charsAdded += LayoutEncoder.OverwriteTransfer(BrcCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
            }
            else
            {
                charsAdded += LayoutEncoder.OverwriteTransfer(SqBrktCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
            }
        }
        return destIndex + charsAdded - originalDestIndex;
    }

    public override PrettyJsonTypeFormatting Clone()
    {
        return AlwaysRecycler.Borrow<PrettyJsonTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public override PrettyJsonTypeFormatting CopyFrom(CompactJsonTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }
}
