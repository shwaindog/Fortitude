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

    FormatFlags ResolveContentFormatFlags<T>(IStringBuilder sb, T input, FormatFlags callerFormattingFlags
      , string formatString = "", bool isFieldName = false);

    FormatFlags ResolveContentAsValueFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags callerFlags = DefaultCallerTypeFlags);

    FormatFlags ResolveContentAsStringFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags callerFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast mdc, FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast mdc, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast mdc);

    ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast mdc, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast mdc, FormatFlags formatFlags = DefaultCallerTypeFlags);

    SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);

    (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags<T>(ITheOneString tos, T forValue, Type actualType, WrittenAsFlags proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    int                    SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    Range?                 AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    int                    SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast mdc);

    WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges StartKeyedCollectionOpen(ITypeMolderDieCast mdc, Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges FinishKeyedCollectionOpen(ITypeMolderDieCast mdc);

    ContentSeparatorRanges AppendKeyedCollectionClose(ITypeMolderDieCast mdc, Type keyType, Type valueType, int totalItemCount
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        ITypeMolderDieCast<TMold> mdc
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
        ITypeMolderDieCast<TMold> mdc
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
        ITypeMolderDieCast<TMold> mdc
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
        ITypeMolderDieCast<TMold> mdc
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
        ITypeMolderDieCast<TMold> mdc
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

    ContentSeparatorRanges StartFormatCollectionOpen(ITypeMolderDieCast mdc, Type itemElementType, bool? hasItems, Type collectionType
      , FormatFlags formatFlags = DefaultCallerTypeFlags);
    
    ContentSeparatorRanges FinishFormatCollectionOpen(ITypeMolderDieCast mdc, Type itemElementType, bool? hasItems
    , Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextItemFormat<TFmt>(ITypeMolderDieCast mdc, TFmt item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?;

    WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(ITypeMolderDieCast mdc, TCloaked? item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull;

    AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextCharSeqFormat<TCharSeq>(ITypeMolderDieCast mdc, TCharSeq item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?;

    AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextStringBearerFormat<TBearer>(ITypeMolderDieCast mdc, TBearer item, int retrieveCount, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?;

    IStringBuilder FormatCollectionEnd(ITypeMolderDieCast mdc, int? resultsFoundCount, Type itemElementType, int? totalItemCount
      , string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags);

    (int, int) InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex, WrittenAsFlags writtenAs
    , int firstFieldIndex, FormatFlags createTypeFlags, int contentLength = -1, ITypeMolderDieCast? liveMoldInternal = null);
    
    int AppendExistingReferenceId(ITypeMolderDieCast mdc, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags);

    int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags);

    int AppendInstanceInfoField(ITypeMolderDieCast mdc, string fieldName, ReadOnlySpan<char> description
    , WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags);
    
    WrittenAsFlags AppendFieldName(ITypeMolderDieCast mdc, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldNameMatch<T>(ITypeMolderDieCast mdc, T source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName<TFmt>(ITypeMolderDieCast mdc, TFmt source, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldName<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, ICharSequence source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, StringBuilder source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary FormatFieldName<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    AppendSummary FormatBearerFieldName<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?;

    WrittenAsFlags FormatFieldContentsMatch<TAny>(ITypeMolderDieCast mdc, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents<TFmt>(ITypeMolderDieCast mdc, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldContents<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFallbackFieldContents<TAny>(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, char[] source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary FormatFieldContents<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    AppendSummary FormatBearerFieldContents<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj, string? callerFormatString = null
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
