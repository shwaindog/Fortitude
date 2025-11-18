// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyLogTypeFormatting : CompactLogTypeFormatting
{
    protected const char   BrcOpnChar = '{';
    protected const string BrcCls     = "}";

    public override PrettyLogTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        base.Initialize(graphTrackingBuilder, styleOptions);

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb              = moldInternal.Sb;
        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags);
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            buildingType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        StyleOptions.IndentLevel++;
        GraphBuilder.AppendContent(BrcOpn);
        if (formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder
                    .AppendPadding
                        (StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }

        return GraphBuilder.Complete(formatFlags);
    }

    public override SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

    public override SeparatorPaddingRanges AddNextFieldSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.AppendSeparator(Cma);
        if (formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        return GraphBuilder.Complete(formatFlags).SeparatorPaddingRange!.Value;
    }

    public override int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel)
    {
        var nlPadding      = options.IndentSize * indentLevel;
        var bufferSize     = nlPadding + 1 + options.NewLineStyle.Length;
        var nextFieldStart = stackalloc char[bufferSize].ResetMemory();
        nextFieldStart[0] = ',';
        nextFieldStart.Append(options.NewLineStyle);
        var spacesFrom                                                  = nextFieldStart.PopulatedLength();
        for (var i = spacesFrom; i < bufferSize; i++) nextFieldStart[i] = ' ';
        sb.InsertAt(nextFieldStart, atIndex);
        return bufferSize;
    }

    public override ContentSeparatorRanges AppendTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        StyleOptions.IndentLevel--;

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, DefaultCallerTypeFlags);
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendContent(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendContent(StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        GraphBuilder.AppendContent(BrcCls);
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType, formatFlags);

        StyleOptions.IndentLevel++;

        return sb.Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        StyleOptions.IndentLevel--;
        if (totalItemCount > 0)
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        return base.AppendKeyedCollectionEnd(sb, keyedCollectionType, keyType, valueType, totalItemCount, formatFlags);
    }

    public override IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType
      , bool? hasItems, Type collectionType, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        hasItems ??= false;
        if (!hasItems.Value) return sb;
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString) return sb.Append(DblQt);
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return sb.Append(DblQt);

        StyleOptions.IndentLevel++;

        return sb.Append(SqBrktOpn)
                 .Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) && StyleOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return 0;
        return sb.Append(CmaSpc).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) && StyleOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return 0;
        return destSpan.OverWriteAt(atIndex, CmaSpc);
    }

    public override IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType
      , int nextItemNumber, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.MarkContentEnd();
        var lastRange    = GraphBuilder.LastContentSeparatorPaddingRanges;
        var lastFmtFlags = lastRange.PreviousFormatFlags;
        base.AddCollectionElementSeparator(elementType, sb, nextItemNumber);
        GraphBuilder.MarkSeparatorEnd();
        if (elementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return sb;
        if (elementType == typeof(char) && StyleOptions.CharArrayWritesString) return sb;
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < sb.LineContentWidth)
            {
                sb.Length -= 1; // remove last Space
                GraphBuilder.AppendPadding(StyleOptions.NewLineStyle)
                            .AppendPadding(StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            if (lastFmtFlags.CanAddNewLine())
            {
                GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
                if (!lastFmtFlags.HasNoWhitespacesToNextFlag())
                {
                    GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                             , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
                }
            }
        }
        GraphBuilder.Complete(lastFmtFlags);
        return sb;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount
      , string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb         = moldInternal.Sb;
        var charsAdded = 0;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, this, formatFlags, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.HasAsStringContentFlag() && itemElementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                GraphBuilder.AppendContent(DblQt);
            }
            return sb;
        }
        else
        {
            sb.RemoveLastWhiteSpacedCommaIfFound();
        }

        if (prevFmtFlags.DoesNotHaveAsValueContentFlag())
        {
            StyleOptions.IndentLevel--;
        }
        
        if (totalItemCount > 0)
        {
            if (prevFmtFlags.CanAddNewLine())
            {
                GraphBuilder.AppendContent(StyleOptions.NewLineStyle);
                if (!prevFmtFlags.HasNoWhitespacesToNextFlag())
                {
                    GraphBuilder.AppendContent(StyleOptions.IndentChar
                                             , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
                }
            }
        }
        if (itemElementType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcCls);
            return sb;
        }
        GraphBuilder.AppendContent(SqBrktCls);
        return sb;
    }

    public override int CollectionEnd(Type collectionType, Span<char> destSpan, int destIndex, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var charsAdded        = 0;
        var originalDestIndex = destIndex;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.HasAsStringContentFlag() && collectionType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                charsAdded += GraphBuilder.GraphEncoder.Transfer(this, DblQt, destSpan, destIndex);
            }
            GraphBuilder.MarkContentEnd(destIndex + charsAdded);
            return charsAdded;
        }
        else
        {
            GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        }

        if (prevFmtFlags.DoesNotHaveAsValueContentFlag())
        {
            StyleOptions.IndentLevel--;
        }
        
        if (itemsCount > 0)
        {
            if (prevFmtFlags.CanAddNewLine())
            {
                charsAdded += GraphBuilder.GraphEncoder.Transfer(this, StyleOptions.NewLineStyle, destSpan, destIndex);
                GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                if (!prevFmtFlags.HasNoWhitespacesToNextFlag())
                {
                    charsAdded += destSpan.OverWriteRepatAt(destIndex + charsAdded, Spc, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
                    GraphBuilder.MarkContentEnd(destIndex + charsAdded);
                }
            }
        }
        if (collectionType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcCls);
        }
        else
        {
            GraphBuilder.AppendContent(SqBrktCls);
        }
        return destIndex + charsAdded - originalDestIndex;
    }
}
