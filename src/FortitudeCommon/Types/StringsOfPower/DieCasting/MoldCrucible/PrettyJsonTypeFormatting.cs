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

    public override ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        if (formatFlags.DoesNotHaveAsEmbeddedContentFlags())
        {
            Gb.IndentLevel++;
            Gb.StartAppendContent(BrcOpn, sb, this, formatFlags.RemoveContentTreatmentFlags());
        }
        if (formatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
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

    public override ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToOpen, IMoldWriteState mdc, WrittenAsFlags openingAs)
    {
        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        if (mdc.Master.CallerContext.FormatFlags.HasSuppressClosing()) { return Gb.LastContentSeparatorPaddingRanges; }
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlags()) Gb.IndentLevel -= mdc.CloseDepthDecrementBy;

        return base.AppendComplexTypeClosing(mdc.InstanceOrType, mdc, mdc.CurrentWriteMethod);
    }
    
    public override ContentSeparatorRanges StartKeyedCollectionOpen(IMoldWriteState mws
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if (callerFormattingFlags.HasSuppressOpening())
        {
            return ContentSeparatorRanges.None;
        }
        
        Gb.IndentLevel++;
        return base.StartKeyedCollectionOpen(mws, keyType, valueType, callerFormattingFlags);
    }
    
    public override ContentSeparatorRanges FinishKeyedCollectionOpen(IMoldWriteState mws)
    {
        return ContentSeparatorRanges.None;
    }

    public override ContentSeparatorRanges AppendKeyedCollectionClose(IMoldWriteState mws
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        if (callerFormattingFlags.HasSuppressClosing()) 
        { 
            return ContentSeparatorRanges.None; 
        }
        if (callerFormattingFlags.DoesNotHaveAsEmbeddedContentFlags()) Gb.IndentLevel -= mws.CloseDepthDecrementBy;

        return base.AppendKeyedCollectionClose(mws, keyType, valueType, totalItemCount, callerFormattingFlags);
    }

    public override ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType
      , bool? hasItems, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (hasItems != true || mws.WroteInnerTypeOpen) { return ContentSeparatorRanges.None; }

        var  reg                    = mws.Master.ActiveGraphRegistry;
        var  hasWroteCollectionOpen = false;
        int? firstOpenIndex         = null;

        IMoldWriteState? initialDc = mws;

        if (mws.MoldGraphVisit.HasRegisteredVisit)
        {
            GraphNodeVisit derivedMold = reg[mws.MoldGraphVisit.VisitId.VisitIndex];
            if (mws.MoldGraphVisit.IsBaseOfInitial)
            {
                var            checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;
                GraphNodeVisit checkMold      = reg[checkMoldIndex];
                do
                {
                    derivedMold            =   checkMold;
                    hasWroteCollectionOpen |=  checkMold.MoldState?.WroteInnerTypeOpen ?? false;
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
                    if (openDc != null) openDc.WroteInnerTypeClose = true;
                }
                mws.WroteInnerTypeClose = false;
                return ContentSeparatorRanges.None;
            }
        }
        var sb = mws.Sb;
        mws.WroteInnerTypeClose    = false;
        mws.WroteInnerTypeOpen     = true;
        var inheritedFmtFlags = mws.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        if (mws is ICollectionMoldWriteState { IsSimple: true } scmdc)
        {
            if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, inheritedFmtFlags); }
            else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
            {
                if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
                {
                    AppendInstanceValuesFieldName(mws.TypeBeingBuilt, inheritedFmtFlags);
                }
            }
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)inheritedFmtFlags | (FormatSwitches)mws.CreateMoldFormatFlags);
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
            Gb.Complete(inheritedFmtFlags);
            Gb.AddHighWaterMark();
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
                LayoutEncoder.OverwriteTransfer
                    ( elementType == typeof(KeyValuePair<string, JsonNode>)
                             ? BrcOpn
                             : SqBrktOpn, destSpan, destStartIndex);
            Gb.Complete(inheritedFmtFlags);
            Gb.AddHighWaterMark();
            charsAdded += AddCollectionElementPadding(elementType, destSpan, destStartIndex + charsAdded, 1, formatSwitches);
        }

        return charsAdded;
    }

    public override int AppendCloseCollection(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var inheritedFmtFlags    = mws.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        var inheritedFmtSwitches = (FormatSwitches)inheritedFmtFlags;
        var preAppendAt          = sb.Length;
        if (mws.SkipBody || mws.WroteInnerTypeClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(inheritedFmtFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeClose = true;
        if (!(totalItemCount > 0))
        {
            if (totalItemCount.HasValue || StyleOptions.NullWritesEmpty)
            {
                Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
                if (!mws.WroteInnerTypeOpen)
                {
                    CollectionStart(itemElementType, sb, true, inheritedFmtSwitches);
                    if (mws.CurrentWriteMethod.HasAsCollectionFlag() && mws.CloseDepthDecrementBy > 1)
                    {
                        Gb.IndentLevel -= mws.CloseDepthDecrementBy - 1;
                    }
                }
                else if (mws.CurrentWriteMethod.HasAsCollectionFlag())
                {
                    Gb.IndentLevel -= mws.CloseDepthDecrementBy;
                }
                else { Gb.IndentLevel--; }
                Gb.RemoveLastSeparatorAndPadding();
                CollectionEnd(itemElementType, sb, 0, inheritedFmtSwitches);
                Gb.Complete(inheritedFmtFlags);
            }
            else
            {
                AppendFormattedNull(sb, formatString, inheritedFmtFlags);
                if (mws.WroteInnerTypeOpen)
                {
                    if (mws.CurrentWriteMethod.HasAsCollectionFlag())  Gb.IndentLevel -= mws.CloseDepthDecrementBy; 
                    else  Gb.IndentLevel--; 
                }
            }
            mws.WroteInnerTypeOpen  = false;
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            Gb.MarkContentStart(preAppendAt);
            Gb.MarkContentEnd();
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeOpen  = false;

        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            Gb.MarkContentStart(preAppendAt);
            if ((itemElementType.IsChar() && (JsonOptions.CharBufferWritesAsCharCollection || inheritedFmtFlags.HasAsStringContentFlag())))
            {
                if (inheritedFmtFlags.DoesNotHaveAsValueContentFlag() || inheritedFmtFlags.HasAsStringContentFlag()) Gb.AppendContent(DblQt);
                return sb.Length - preAppendAt;
            }
            if (itemElementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt);
                return sb.Length - preAppendAt;
            }
            Gb.RemoveLastSeparatorAndPadding();

            if (mws.CurrentWriteMethod.HasAsCollectionFlag())  Gb.IndentLevel -= mws.CloseDepthDecrementBy; 
            else  Gb.IndentLevel--; 

            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
            if (totalItemCount > 0)
            {
                if (inheritedFmtFlags.CanAddNewLine()) { AddCollectionElementPadding(itemElementType, sb, resultsFoundCount ?? 0); }
            }
            if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
            {
                Gb.AppendContent(BrcCls);
                return sb.Length - preAppendAt;
            }
            Gb.AppendContent(SqBrktCls);
        }
        return sb.Length - preAppendAt;
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
            Gb.Complete(inheritedFmtFlags);
            Gb.AddHighWaterMark();
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

    public override ContentSeparatorRanges AddCollectionElementPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
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
