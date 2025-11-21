// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public interface IStyledTypeFormatting : ICustomStringFormatter
{
    string Name { get; }
    
    public GraphTrackingBuilder GraphBuilder { get; }

    FieldContentHandling ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FieldContentHandling callerFormattingFlags
    , string formatString = "", bool isFieldName = false);
    
    FieldContentHandling ResolveContentAsValueFormattingFlags<T>(T input, bool hasFallbackValue);
    
    FieldContentHandling ResolveContentAsStringFormattingFlags<T>(T input, bool hasFallbackValue);

    ContentSeparatorRanges AppendValueTypeOpening(ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendValueTypeClosing(ITypeMolderDieCast moldInternal);

    ContentSeparatorRanges AppendComplexTypeOpening(ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    SeparatorPaddingRanges AppendFieldValueSeparator(ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    SeparatorPaddingRanges AddNextFieldSeparator(ITypeMolderDieCast moldInternal, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel);

    ContentSeparatorRanges AppendTypeClosing(ITypeMolderDieCast moldInternal);

    IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(ITypeMolderDieCast<TMold> typeMold
    , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVRevealBase> valueStyler
    , PalantírReveal<TKRevealBase> keyStyler, FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber);

    IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType, bool? hasItems, Type collectionType
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
      where TFmt: ISpanFormattable?;

    IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item
      , int retrieveCount, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
      where TFmtStruct: struct, ISpanFormattable;

    IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked? item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler)
      where TCloaked : TCloakedBase?
      where TCloakedBase : notnull;

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  where TCharSeq : ICharSequence;

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder CollectionNextStringBearerFormat<TBearer>(ITheOneString tos, TBearer? item, int retrieveCount) 
      where TBearer : IStringBearer;

    IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType, int? totalItemCount
    , string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags);
    
    IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName);
    
    IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable;

    IStringBuilder FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    IStringBuilder FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ITheOneString tos, TCloaked value, PalantírReveal<TRevealBase> valueRevealer)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?;

    IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable;

    IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatFieldContents<TCloaked, TRevealBase>(ITheOneString tos, TCloaked value, PalantírReveal<TRevealBase> valueRevealer)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer?;
}

public static class StyleTypeFormattingExtensions
{
    
    public static SeparatorPaddingRanges FieldEnd(this IStringBuilder _, ITypeMolderDieCast moldInternal, IStyledTypeFormatting stf
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) => 
      stf.AppendFieldValueSeparator(moldInternal, formatFlags);


    public static char RemoveLastWhiteSpacedCommaIfFound(this IStringBuilder sb)
    {
        if (sb.Length < 2) return sb.Length > 0 ? sb[0] : '\0';
        if (sb[^1] == ',')
        {
            sb.Length -= 1;
            return sb[^1];
        }
        if (sb[^2] == ',' && sb[^1] == ' ')
        {
          sb.Length -= 2;
          return sb[^1];
        }
        var i                                         = sb.Length - 1;
        for (; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (sb[i] == ',')
            {
                sb.Length = i;
                return sb[^1];
            }
        sb.Length = i + 1;
        return sb[^1];
    }
    
    public static char RemoveLastWhiteSpacedCommaIfFound(this Span<char> destSpan, ref int destIndex)
    {
        if (destIndex < 2) return destIndex > 0 ? destSpan[0] : '\0';
        if (destSpan[destIndex -1] == ',')
        {
            destIndex -= 1;
            return destSpan[destIndex - 1];
        }
        if (destSpan[destIndex - 2] == ',' && destSpan[destIndex - 1] == ' ')
        {
          destIndex -= 2;
          return destSpan[destIndex - 1];
        }
        var i   = destIndex - 1;
        for (; i > 0 && destSpan[i] is ' ' or '\r' or '\n' or ','; i--)
            if (destSpan[i] == ',')
            {
                destIndex = i;
                return destSpan[destIndex - 1];
            }
        destIndex = i + 1;
        return destSpan[destIndex - 1];
    }

    public static int RemoveLastWhiteSpacedCommaIfFound(this Span<char> destSpan, int destStartIndex)
    {
        if (destStartIndex < 2) return 0;
        if (destSpan[destStartIndex - 1] == ',')
        {
            destSpan[destStartIndex - 1] = '\0';
            return -1;
        }
        if (destSpan[destStartIndex - 2] == ',' && destSpan[destStartIndex - 1] == ' ')
        {
            destSpan[destStartIndex - 2] = '\0';
            destSpan[destStartIndex - 1] = '\0';
            return -2;
        }
        var i = destStartIndex - 1;
        for (; i > 0 && destSpan[i] is ' ' or '\r' or '\n' or ','; i--)
        {
            if (destSpan[i] == ',')
            {
                destSpan[i] = '\0';
                return i - destStartIndex;
            }
            destSpan[i] = '\0';
        }
        return -destStartIndex;
    }
}
