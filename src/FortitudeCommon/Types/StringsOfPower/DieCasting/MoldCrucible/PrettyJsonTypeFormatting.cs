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

    public override ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (formatFlags.DoesNotHaveAsEmbeddedContentFlag())
        { 
            StyleOptions.IndentLevel++;
            GraphBuilder.StartAppendContent(BrcOpn, sb, this, formatFlags);
        }
        if (formatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                                   , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        return GraphBuilder.Complete(formatFlags);
    }

    public override SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding( Spc)
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
        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = GraphBuilder.RemoveLastSeparatorAndPadding();
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlag()) { StyleOptions.IndentLevel--; }

        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, this, DefaultCallerTypeFlags);
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            GraphBuilder.AppendContent(StyleOptions.NewLineStyle);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendContent(StyleOptions.IndentChar
                                                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlag())
        {
            GraphBuilder.AppendContent(BrcCls);
        }
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }


    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FieldContentHandling callerFormattingFlags = DefaultCallerTypeFlags)
    {
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType);

        StyleOptions.IndentLevel++;

        return sb.Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FieldContentHandling callerFormattingFlags = DefaultCallerTypeFlags)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        StyleOptions.IndentLevel--;
        if (totalItemCount > 0)
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        return base.AppendKeyedCollectionEnd(sb, keyedCollectionType, keyType, valueType, totalItemCount);
    }

    public override IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType
      , bool? hasItems, Type collectionType, FieldContentHandling callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        hasItems ??= false;
        return !hasItems.Value ? sb : CollectionStart(itemElementType, sb, hasItems.Value).ToStringBuilder(sb);
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString)
        {
            GraphBuilder.AppendContent(DblQt);
            return sb.Length - preAppendLen; 
        }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            GraphBuilder.AppendContent(DblQt);
            return sb.Length - preAppendLen;
        }
        var currFmtFlags = GraphBuilder.CurrentNodeRanges.FormatFlags;
        if (!currFmtFlags.HasAsEmbeddedContentFlag()) { StyleOptions.IndentLevel++; }
        if (elementType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcOpn);
        } 
        else 
        {
            GraphBuilder.AppendContent(SqBrktOpn);
        }
        if (currFmtFlags.CanAddNewLine())
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            if (!currFmtFlags.HasNoWhitespacesToNextFlag())
            {
                GraphBuilder.AppendPadding(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        GraphBuilder.Complete(currFmtFlags);
        return sb.Length - preAppendLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        int charsAdded = 0;
        if ((elementType == typeof(char) && JsonOptions.CharArrayWritesString) || 
            (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String))
        {
            charsAdded = GraphBuilder.GraphEncoder.Transfer(this, DblQt, destSpan, destStartIndex);
            return charsAdded; 
        }
        
        var currFmtFlags = GraphBuilder.CurrentNodeRanges.FormatFlags;
        if (!currFmtFlags.HasAsEmbeddedContentFlag()) { StyleOptions.IndentLevel++; }
        if (elementType == typeof(KeyValuePair<string, JsonNode>))
        {
            charsAdded = GraphBuilder.GraphEncoder.Transfer(this, BrcOpn, destSpan, destStartIndex);
        }
        else
        {
            charsAdded = GraphBuilder.GraphEncoder.Transfer(this, SqBrktOpn, destSpan, destStartIndex);
        }
        if (currFmtFlags.CanAddNewLine())
        {
            charsAdded += GraphBuilder.GraphEncoder.Transfer(this, StyleOptions.NewLineStyle, destSpan
                                                          ,  destStartIndex + charsAdded);
            GraphBuilder.MarkPaddingEnd(destStartIndex + charsAdded);
            if (!currFmtFlags.HasNoWhitespacesToNextFlag())
            {
                charsAdded += destSpan.OverWriteRepatAt(destStartIndex + charsAdded, Spc
                                                      , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
                GraphBuilder.MarkPaddingEnd(destStartIndex + charsAdded);
            }
        }

        return charsAdded;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var currFmtFlags = GraphBuilder.CurrentNodeRanges.FormatFlags;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString)
        {
            GraphBuilder.Complete(currFmtFlags);
            return 0;
        }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            GraphBuilder.Complete(currFmtFlags);
            return 0;
        }
        var preAppendLen = sb.Length;
        GraphBuilder.AppendSeparator(Cma);
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < sb.LineChars)
            {
                GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
                GraphBuilder.AppendPadding( StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            GraphBuilder.AppendPadding( StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        GraphBuilder.Complete(currFmtFlags);
        return sb.Length - preAppendLen;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var currFmtFlags = GraphBuilder.CurrentNodeRanges.FormatFlags;
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString)
        {
            GraphBuilder.Complete(currFmtFlags);
            return 0;
        }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            GraphBuilder.Complete(currFmtFlags);
            return 0;
        }
        
        GraphBuilder.MarkContentEnd();
        var addedChars = destSpan.OverWriteAt(atIndex, Cma);
        GraphBuilder.MarkSeparatorEnd();
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < destSpan.FindLineLengthFrom(atIndex))
            {
                
                addedChars += GraphBuilder.GraphEncoder.Transfer(this, StyleOptions.NewLineStyle, destSpan, atIndex);
                addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                      , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
                
            }
            GraphBuilder.MarkPaddingEnd(atIndex + addedChars);
        }
        else
        {
            addedChars += GraphBuilder.GraphEncoder.Transfer(this, StyleOptions.NewLineStyle, destSpan, atIndex);
            addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            GraphBuilder.MarkPaddingEnd(atIndex + addedChars);
        }
        GraphBuilder.Complete(currFmtFlags);
        return addedChars;
    }

    public override IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType
      , int nextItemNumber, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString)
        {
            GraphBuilder.Complete(formatFlags);
            return sb;
        }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            GraphBuilder.Complete(formatFlags);
            return sb;
        }
        GraphBuilder.AppendSeparator(Cma);
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < sb.LineChars)
            {
                GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
                GraphBuilder.AppendPadding( StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            GraphBuilder.AppendPadding(StyleOptions.NewLineStyle);
            GraphBuilder.AppendPadding( StyleOptions.IndentChar, StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        GraphBuilder.Complete(formatFlags);
        return sb;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount
      , string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb         = moldInternal.Sb;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        GraphBuilder.ResetCurrent(formatFlags, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if ((itemElementType.IsChar() && (JsonOptions.CharArrayWritesString || formatFlags.HasAsStringContentFlag())))
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() ||
                prevFmtFlags.HasAsStringContentFlag()) GraphBuilder.AppendContent(DblQt);
            return sb;
        }
        else if (itemElementType.IsByte()  && JsonOptions.ByteArrayWritesBase64String)
        {
            GraphBuilder.AppendContent(DblQt);
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

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        
        var preAppendLen = sb.Length;
        GraphBuilder.ResetCurrent((FieldContentHandling)formatFlags, true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if ((elementType.IsChar() && (JsonOptions.CharArrayWritesString || formatFlags.HasAsStringContentFlag())))
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
        sb.RemoveLastWhiteSpacedCommaIfFound();

        if (prevFmtFlags.DoesNotHaveAsValueContentFlag())
        {
            StyleOptions.IndentLevel--;
        }
        
        if (itemsCount > 0)
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
        if (elementType == typeof(KeyValuePair<string, JsonNode>))
        {
            GraphBuilder.AppendContent(BrcCls);
        }
        else { GraphBuilder.AppendContent(SqBrktCls); }
        return sb.Length - preAppendLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        var charsAdded        = 0;
        var originalDestIndex = destIndex;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var currFmtFlags = GraphBuilder.CurrentNodeRanges.FormatFlags;
        GraphBuilder.ResetCurrent(currFmtFlags,  true);
        var prevFmtFlags = GraphBuilder.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.HasAsStringContentFlag() && elementType.IsCharArray())
        {
            if (prevFmtFlags.DoesNotHaveAsValueContentFlag() || prevFmtFlags.HasAsStringContentFlag())
            {
                charsAdded += GraphBuilder.GraphEncoder.Transfer(this, DblQt, destSpan, destIndex);
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
        if (elementType == typeof(KeyValuePair<string, JsonNode>))
        {
            charsAdded += GraphBuilder.GraphEncoder.Transfer(this, BrcCls, destSpan, destIndex);
            GraphBuilder.MarkContentEnd(destIndex + charsAdded);
        }
        else
        {
            charsAdded += GraphBuilder.GraphEncoder.Transfer(this, SqBrktCls, destSpan, destIndex);
            GraphBuilder.MarkContentEnd(destIndex + charsAdded);
        }
        return destIndex + charsAdded - originalDestIndex;
    }
}
