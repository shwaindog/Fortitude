// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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

    public override PrettyLogTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb              = moldInternal.Sb;
        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;
        
        StartNewContentSeparatorPaddingTracking(sb);
        if (alternativeName.IsNotNullOrEmpty())
            sb.Append(alternativeName);
        else
            buildingType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        StyleOptions.IndentLevel++;
        sb.AppendLastContent(BrcOpn, this);
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
        StyleOptions.IndentLevel--; 

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
        sb.AppendLastContent(BrcCls, this);
        return ContentSeparatorPaddingTracking.ToContentSeparatorFromEndRanges(moldInternal, DefaultCallerTypeFlags);
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

    public override IStringBuilder AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType
      , int nextItemNumber, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        base.AddCollectionElementSeparator(elementType, sb, nextItemNumber);
        if (elementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return sb;
        if (elementType == typeof(char) && StyleOptions.CharArrayWritesString) return sb;
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
        return sb;
    }

    public override IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType, int? totalItemCount
      , string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            sb.Append(StyleOptions.NullString);
            return sb;
        }
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString)
        {
            return CollectionEnd(itemElementType, sb, totalItemCount.Value).ToStringBuilder(sb);
        }
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String)
        {
            return CollectionEnd(itemElementType, sb, totalItemCount.Value).ToStringBuilder(sb);
        }

        sb.RemoveLastWhiteSpacedCommaIfFound();
        if (totalItemCount > 0)
        {
            StyleOptions.IndentLevel--;
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return sb.Append(SqBrktCls);
    }
}
