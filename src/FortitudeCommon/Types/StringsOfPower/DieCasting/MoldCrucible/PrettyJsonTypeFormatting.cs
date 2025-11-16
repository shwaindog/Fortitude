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

    public override PrettyJsonTypeFormatting Initialize(StyleOptions styleOptions)
    {
        base.Initialize(styleOptions);

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
            sb.StartAppendLastContent(BrcOpn, this);
        }
        if (formatFlags.CanAddNewLine())
        {
            sb.AppendPaddingExpectMore(StyleOptions.NewLineStyle, this);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                sb.AppendPaddingExpectMore(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel), this);
            }
        }

        return ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, formatFlags);
    }

    public override SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        return sb.AppendLastSeparator(Cln, this)
                 .AppendLastPadding(Spc, this, moldInternal, formatFlags)
                 .SeparatorPaddingRange!.Value;
    }

    public override SeparatorPaddingRanges AddNextFieldSeparator(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        sb.AppendLastSeparator(Cma, this);
        if (formatFlags.CanAddNewLine())
        {
            sb.AppendPaddingExpectMore(StyleOptions.NewLineStyle, this);
            if (!formatFlags.HasNoWhitespacesToNextFlag())
            {
                sb.AppendPaddingExpectMore(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel), this);
            }
        }
        return ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, formatFlags)
                                              .SeparatorPaddingRange!.Value;
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

        var previousContentPadSpacing = moldInternal.LastContentSeparatorPaddingRanges;
        var lastNonWhiteSpace         = sb.RemoveLastSeparatorAndPadding(previousContentPadSpacing);
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlag()) { StyleOptions.IndentLevel--; }

        ContentSeparatorPaddingTracking.Reset();
        if (lastNonWhiteSpace != BrcOpnChar && previousContentPadSpacing.PreviousFormatFlags.CanAddNewLine())
        {
            sb.AppendContentExpectMore(StyleOptions.NewLineStyle, this);
            if (!previousContentPadSpacing.PreviousFormatFlags.HasNoWhitespacesToNextFlag())
            {
                sb.AppendContentExpectMore(StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel), this);
            }
        }
        if (previousContentPadSpacing.PreviousFormatFlags.DoesNotHaveAsEmbeddedContentFlag())
        {
            sb.AppendLastContent(BrcCls, this);
        }
        return ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, DefaultCallerTypeFlags);
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
        StyleOptions.IndentLevel++;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcOpn).ReturnCharCount(1);
        var originalLen = sb.Length;
        sb.Append(SqBrktOpn)
          .Append(StyleOptions.NewLineStyle)
          .Append(StyleOptions.IndentChar
                , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));

        return sb.Length - originalLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        StyleOptions.IndentLevel++;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(destStartIndex, BrcOpn);
        var addedChars = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        addedChars += destSpan.OverWriteAt(destStartIndex + addedChars, StyleOptions.NewLineStyle);
        addedChars += destSpan.OverWriteRepatAt(destStartIndex + addedChars, StyleOptions.IndentChar
                                              , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));

        return addedChars;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        var originalLen = sb.Length;
        sb.Append(Cma);
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < sb.LineContentWidth)
            {
                sb.Length -= 1; // remove last Space
                sb.Append(StyleOptions.NewLineStyle)
                  .Append(StyleOptions.IndentChar
                        , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return sb.Length - originalLen;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        var addedChars = destSpan.OverWriteAt(atIndex, Cma);
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < destSpan.FindLineLengthFrom(atIndex))
            {
                addedChars += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
                addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                      , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            addedChars += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
            addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return addedChars;
    }

    public override IStringBuilder AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType
      , int nextItemNumber, FieldContentHandling callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        return AddCollectionElementSeparator(elementType, sb, nextItemNumber).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        StyleOptions.IndentLevel--;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(sb);
            return sb.Append(DblQt).ReturnCharCount(1 + addedChars);
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcCls).ReturnCharCount(1);
        var originalLen = sb.Length;
        sb.RemoveLastWhiteSpacedCommaIfFound();
        if (itemsCount > 0)
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        return sb.Append(SqBrktCls).ReturnCharCount(sb.Length - originalLen);
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int index, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        StyleOptions.IndentLevel--;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(index, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var charsAdded = CompleteBase64Sequence(destSpan, index);
            return destSpan.OverWriteAt(index + charsAdded, DblQt) + charsAdded;
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(index, BrcCls);
        var addedChars = destSpan.RemoveLastWhiteSpacedCommaIfFound(index);
        if (itemsCount > 0)
        {
            addedChars += destSpan.OverWriteAt(index + addedChars, StyleOptions.NewLineStyle);
            addedChars += destSpan.OverWriteRepatAt(index + addedChars, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return addedChars;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FieldContentHandling callerFormattingFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        return CollectionEnd(itemElementType, sb, totalItemCount ?? 0).ToStringBuilder(sb);
    }
}
