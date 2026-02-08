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

public class PrettyLogTypeFormatting : CompactLogTypeFormatting
{
    public override PrettyLogTypeFormatting Initialize(ITheOneString theOneString)
    {
        base.Initialize(theOneString);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                = mdc.Sb;
        var alternativeName   = mdc.InstanceName;
        var buildingType      = mdc.TypeBeingBuilt;
        var buildTypeFullName = buildingType.FullName ?? "";

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);

        var mergedFlags = formatFlags | mdc.CreateMoldFormatFlags;

        if (mergedFlags.HasSuppressOpening()) return Gb.Complete(formatFlags);

        if (mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag() 
         && (mergedFlags.DoesNotHaveLogSuppressTypeNamesFlag()
          && (mergedFlags.HasAddTypeNameFieldFlag()
           || !StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildTypeFullName.StartsWith(s)))))
        {
            var isComplexContentType = mdc.CurrentWriteMethod.HasAsContentFlag();
            if (isComplexContentType)
            {
                Gb.AppendContent(RndBrktOpn);
            }
            buildingType.AppendShortNameInCSharpFormat(sb);
            mdc.WroteTypeName = true;
            if (isComplexContentType)
            {
                Gb.AppendContent(RndBrktCls);
            } 
            sb.Append(Spc);
        }
        Gb.IndentLevel++;
        Gb.AppendContent(BrcOpn);
        if (formatFlags.CanAddNewLine())
        {
            Gb.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                Gb
                    .AppendPadding
                        (StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
            }
        }

        return Gb.Complete(formatFlags);
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

    public override ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel--;

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

    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        Gb.IndentLevel++;
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType, callerFormattingFlags);
        Gb.Complete(callerFormattingFlags);
        return sb;
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var lastNonWhiteSpace = Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel--;

        var paddedCloseStartIndex = sb.Length;
        if (callerFormattingFlags.UseMainFieldPadding())
        {
            if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && callerFormattingFlags.CanAddNewLine())
            {
                Gb.AppendPadding(StyleOptions.NewLineStyle);

                if (!callerFormattingFlags.HasNoWhitespacesToNextFlag())
                {
                    Gb.AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(Gb.IndentLevel));
                }
            }
        }
        else { Gb.AppendPadding(StyleOptions.AlternateFieldPadding); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(paddedCloseStartIndex);
        Gb.AppendContent(BrcCls);
        Gb.MarkContentEnd(sb.Length);
        return sb;
    }

    public override IStringBuilder FormatCollectionStart(ITypeMolderDieCast mdc, Type itemElementType
      , bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!hasItems.HasValue)
        {
            Gb.MarkContentEnd();
            return sb;
        }

        // Log always shows each collection and field name
        // if (formatFlags.DoesNotHaveSuppressOpening())
        // {
        if ((itemElementType == typeof(char) && StyleOptions.CharBufferWritesAsCharCollection)
         || (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String))
        {
            Gb.AppendContent(DblQt);
            return sb;
        }
        Gb.IndentLevel++;

        base.FormatCollectionStart(mdc, itemElementType, hasItems, collectionType, formatFlags);
        return sb;
    }

    public override ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
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

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast mdc, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
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
            return sb;
        }
        Gb.RemoveLastSeparatorAndPadding();
        Gb.IndentLevel--;

        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (totalItemCount > 0) { AddCollectionElementPadding(mdc, itemElementType, totalItemCount.Value, formatFlags); }
        if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
        {
            Gb.AppendContent(BrcCls);
            return sb;
        }
        Gb.AppendContent(SqBrktCls);
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
