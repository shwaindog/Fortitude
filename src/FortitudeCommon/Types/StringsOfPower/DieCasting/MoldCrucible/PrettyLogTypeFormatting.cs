// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyLogTypeFormatting : CompactLogTypeFormatting
{
    public override PrettyLogTypeFormatting Initialize(ITheOneString theOneString)
    {
        base.Initialize(theOneString);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                = mws.Sb;
        var buildingType         = instanceToOpen is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceToOpen is Type asType 
                ? asType 
                : (instanceToOpen?.GetType() ?? typeof(T)));
        var buildTypeFullName = buildingType.FullName ?? "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);

        var mergedFlags = formatFlags | mws.CreateMoldFormatFlags;

        if (mergedFlags.HasSuppressOpening()) return Gb.Complete(mergedFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag())
        {
            var showTypeName = false;

            showTypeName |= (openAs.HasAnyOf(AsContent | AsObject)
                          && !(StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildTypeFullName.StartsWith(s))));

            if (!showTypeName)
            {
                var elementType         = buildingType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? buildingType;
                var elementTypeFullName = elementType.FullName ?? "";
                showTypeName |= (openAs.HasAsCollectionFlag() &&
                                 !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => buildTypeFullName.StartsWith(s))
                                && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => elementTypeFullName.StartsWith(s)))
                              || (mws.MoldGraphVisit.IsARevisit && mws is ICollectionMoldWriteState { IsSimple: true }));
            }

            if (showTypeName)
            {
                var isComplexContentType = mws.CurrentWriteMethod.HasAnyOf(AsContent | AsSimple | WrittenAsFlags.AsCollection)
                                        || mws.CurrentWriteMethod.HasAllOf(AsRaw | AsObject);
                if (isComplexContentType)
                {
                    Gb.AppendContent(RndBrktOpn);
                }
                buildingType.AppendShortNameInCSharpFormat(sb);
                if (isComplexContentType)
                {
                    Gb.AppendContent(RndBrktCls);
                } 
                mws.WroteOuterTypeName = true;
                Gb.AppendContent(Spc);
            }
        }

        Gb.IndentLevel++;
        Gb.AppendContent(BrcOpn);

        return AddNextFieldPadding(formatFlags);
    }

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

    public override ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToClose, IMoldWriteState mdc, WrittenAsFlags closingAs)
    {
        var sb = mdc.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel -= mdc.CloseDepthDecrementBy;

        var paddedCloseStartIndex = sb.Length;
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(paddedCloseStartIndex);
        Gb.AppendContent(BrcCls);
        Gb.MarkContentEnd(sb.Length);
        return Gb.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public override ContentSeparatorRanges StartKeyedCollectionOpen(IMoldWriteState mws
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        Gb.IndentLevel++;
        return base.StartKeyedCollectionOpen(mws, keyType, valueType, callerFormattingFlags);
    }

    public override ContentSeparatorRanges AppendKeyedCollectionClose(IMoldWriteState mws
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        
        var lastNonWhiteSpace = Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel -= mws.CloseDepthDecrementBy;

        if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && callerFormattingFlags.CanAddNewLine())
        {
            AddNextFieldPadding(callerFormattingFlags);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.AppendContent(BrcCls);
        var range = Gb.Complete(callerFormattingFlags);
        Gb.AddHighWaterMark();
        return range;
    }
    
    public override ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType, bool? hasItems
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (hasItems != true || mws.WroteInnerTypeOpen)
        {
            return ContentSeparatorRanges.None;
        }
        mws.WroteInnerTypeClose = false;
    
        // Log always shows each collection and field name
        // if (formatFlags.DoesNotHaveSuppressOpening())
        // {
        if (!mws.SkipBody)
        {
            var sb = mws.Sb;
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            if ((itemElementType == typeof(char) && StyleOptions.CharBufferWritesAsCharCollection)
             || (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String))
            {
                Gb.AppendContent(DblQt);
                return Gb.Complete(formatFlags);
            }
            Gb.IndentLevel++;
        }
    
        return base.AppendOpenCollection(mws, itemElementType, hasItems, formatFlags);
    }

    public override int AppendCloseCollection(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb          = mws.Sb;
        var preAppendAt = sb.Length;
        if (mws.SkipBody || mws.WroteInnerTypeClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            // return Gb.AddHighWaterMark();
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeClose = true;
        if (!(totalItemCount > 0))
        {
            if (totalItemCount.HasValue || StyleOptions.NullWritesEmpty)
            {
                Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
                if (!mws.WroteInnerTypeOpen)
                {
                    CollectionStart(itemElementType, sb, true, (FormatSwitches)formatFlags);
                    if (mws.CurrentWriteMethod.HasAsCollectionFlag() && mws.CloseDepthDecrementBy > 1)
                    {
                        Gb.IndentLevel -= mws.CloseDepthDecrementBy - 1;
                    }
                }
                else if (mws.CurrentWriteMethod.HasAsCollectionFlag())  Gb.IndentLevel -= mws.CloseDepthDecrementBy; 
                else  Gb.IndentLevel--;
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
                // Gb.Complete(formatFlags);
            }
            else
            {
                AppendFormattedNull(sb, formatString, formatFlags);
                if (mws.WroteInnerTypeOpen)
                {
                    if (mws.CurrentWriteMethod.HasAsCollectionFlag())  Gb.IndentLevel -= mws.CloseDepthDecrementBy; 
                    else  Gb.IndentLevel--; 
                }
            }
            mws.WroteInnerTypeOpen = false;
            Gb.MarkContentEnd();
            // Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            // Gb.MarkContentStart(preAppendAt);
            // Gb.MarkContentEnd();
            // return Gb.Complete(formatFlags);
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeOpen = false;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        // Log always shows each collection and field name
        // if (prevFmtFlags.DoesNotHaveSuppressClosing())
        // {
        if (prevFmtFlags.HasAsStringContentFlag() && itemElementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag()) { Gb.AppendContent(DblQt); }
            // return Gb.Complete(formatFlags);
            return sb.Length - preAppendAt;
        }
        Gb.RemoveLastSeparatorAndPadding();
        if (mws.CurrentWriteMethod.HasAsCollectionFlag())  Gb.IndentLevel -= mws.CloseDepthDecrementBy; 
        else  Gb.IndentLevel--; 

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (totalItemCount > 0) { AddCollectionElementPadding(mws, itemElementType, totalItemCount.Value, formatFlags); }
        if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
        {
            Gb.AppendContent(BrcCls);
        }
        // Gb.StartAppendContentAndComplete(SqBrktCls, sb, formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.AppendContent(SqBrktCls);
        return sb.Length - preAppendAt;
        // }
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded        = 0;
        var originalDestIndex = destIndex;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        Gb.ResetCurrent((FormatFlags)formatSwitches, true);
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        // Log always shows each collection and field name
        //if (prevFmtFlags.DoesNotHaveSuppressClosing())
        //{
        if (prevFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                charsAdded += LayoutEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
            }
            Gb.MarkContentEnd(destIndex + charsAdded);
            return charsAdded;
        }
        Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        Gb.IndentLevel--;

        Gb.ResetCurrent(prevFmtFlags, true);
        Gb.MarkContentStart();
        if (itemsCount > 0) { charsAdded += AddCollectionElementPadding(elementType, destSpan, destIndex, itemsCount, formatSwitches); }
        Gb.AppendContent(elementType == typeof(KeyValuePair<string, JsonNode>) ? BrcCls : SqBrktCls);
        Gb.MarkContentEnd();
        //}
        return destIndex + charsAdded - originalDestIndex;
    }

    public override ContentSeparatorRanges AddCollectionElementPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        if (formatFlags.UseMainItemPadding())
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
        if (fmtFlgs.UseMainFieldPadding())
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
        Gb.MarkSeparatorEnd();
        var charsAdded = 0;
        if (fmtFlgs.UseMainFieldPadding())
        {
            charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
            charsAdded += destSpan.OverWriteRepatAt(atIndex + charsAdded, StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
        }
        else { charsAdded += destSpan.OverWriteAt(atIndex, StyleOptions.AlternateFieldPadding); }
        Gb.MarkPaddingEnd(atIndex + charsAdded).Complete(fmtFlgs);
        return charsAdded;
    }
    
    public override PrettyLogTypeFormatting Clone()
    {
        return AlwaysRecycler.Borrow<PrettyLogTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public override PrettyLogTypeFormatting CopyFrom(CompactLogTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }
}
