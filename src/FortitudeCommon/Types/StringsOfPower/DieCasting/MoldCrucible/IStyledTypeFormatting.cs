// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public interface IStyledTypeFormatting : ICustomStringFormatter
{
    string Name { get; }

    public StyleOptions StyleOptions { get; set; }
    public GraphTrackingBuilder? GraphBuilder { get; set; }
    public GraphTrackingBuilder Gb { get; set; }
    
    IStyledTypeFormatting? PreviousContext { get; set; }
    IStyledTypeFormatting PreviousContextOrThis { get; }
    
    bool AddedContextOnThisCall { get; set; }

    IStyledTypeFormatting Initialize(ITheOneString theOneString);
    IStyledTypeFormatting ContextStartPushToNext(StyleOptions withStyleOptions);
    IStyledTypeFormatting ContextCompletePopToPrevious();

    FormatFlags ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FormatFlags callerFormattingFlags
      , string formatString = "", bool isFieldName = false);

    FormatFlags ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags callerFlags = DefaultCallerTypeFlags);

    FormatFlags ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags callerFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast moldInternal, FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast moldInternal, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast moldInternal);

    ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast moldInternal, FormatFlags formatFlags = DefaultCallerTypeFlags);

    SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);

    FormatFlags          GetFormatterContentHandlingFlags<T>(ITheOneString tos, T forValue, Type actualType, WriteMethodType proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    FormatFlags          GetFormatterContentHandlingFlags(ITheOneString tos, Type actualType, WriteMethodType proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    int                    SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    Range?                 AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    int                    SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal);

    WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : struct, TVRevealBase
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber, FormatFlags valueFlags = DefaultCallerTypeFlags);

    IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType, bool? hasItems, Type collectionType
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(IStringBuilder sb, bool item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(IStringBuilder sb, bool? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?;

    WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags CollectionNextItemFormat<TCloaked, TCloakedBase>(ISecretStringOfPower tos, TCloaked? item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull;

    WrittenAsFlags CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?;

    WrittenAsFlags CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextStringBearerFormat<TBearer>(ISecretStringOfPower tos, TBearer item, int retrieveCount, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?;

    IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType, int? totalItemCount
      , string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags);

    int InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, Type actualType, int refId, int indexToInsertAt, WriteMethodType writeMethod
    , FormatFlags createTypeFlags, int currentEnd = -1, ITypeMolderDieCast? liveMoldInternal = null);
    
    int AppendExistingReferenceId(ITypeMolderDieCast moldInternal, int refId, WriteMethodType writeMethod, FormatFlags createTypeFlags);

    int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags);

    int AppendInstanceInfoField(ITypeMolderDieCast moldInternal, string fieldName, ReadOnlySpan<char> description
    , WriteMethodType writeMethod, FormatFlags createTypeFlags);
    
    WrittenAsFlags AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName<TFmt>(IStringBuilder sb, TFmt source, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    StateExtractStringRange FormatFieldName<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    StateExtractStringRange FormatFieldName<TBearer>(ISecretStringOfPower tos, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?;

    WrittenAsFlags FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    StateExtractStringRange FormatFieldContents<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    StateExtractStringRange FormatFieldContents<TBearer>(ISecretStringOfPower tos, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?;

    new IStyledTypeFormatting Clone();
}

public static class StyleTypeFormattingExtensions
{
    public static SeparatorPaddingRanges FieldEnd(this IStringBuilder _, IStyledTypeFormatting stf
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        stf.AppendFieldValueSeparator(formatFlags);


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
        var i = sb.Length - 1;
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
        if (destSpan[destIndex - 1] == ',')
        {
            destIndex -= 1;
            return destSpan[destIndex - 1];
        }
        if (destSpan[destIndex - 2] == ',' && destSpan[destIndex - 1] == ' ')
        {
            destIndex -= 2;
            return destSpan[destIndex - 1];
        }
        var i = destIndex - 1;
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
