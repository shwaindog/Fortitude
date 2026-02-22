// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
//
// public enum Complexity
// {
//     SimpleUnitContent
//   , ComplexUnitContent
//   , ComplexUnitObject
//   , SimpleCollection
//   , ComplexCollection
//   , ComplexMapCollection  
// }

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

    ContentSeparatorRanges StartSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs, FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges FinishSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openAs, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendSimpleTypeClosing<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags closeAs);

    ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags closeAs, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges FinishComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags closeAs, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags closeAs);

    SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);

    (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags<T>(ITheOneString tos, T forValue, Type actualType, WrittenAsFlags proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType, VisitResult visitResult, FormatFlags formatFlags);
    int                    SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    Range?                 AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags);
    int                    SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges StartKeyedCollectionOpen(IMoldWriteState mws, Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags);
    ContentSeparatorRanges FinishKeyedCollectionOpen(IMoldWriteState mws);

    ContentSeparatorRanges AppendKeyedCollectionClose(IMoldWriteState mws, Type keyType, Type valueType, int totalItemCount
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue>(IMoldWriteState<TMold> mws, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder;

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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

    IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
    //
    // ContentSeparatorRanges AppendCollectionTypeOpen(IMoldWriteState mws, Type itemElementType, bool? hasItems, Type collectionType
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags);
    //
    //
    // ContentSeparatorRanges AppendCollectionTypeClose(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType, int? totalItemCount
    // , string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);
    
    ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType, bool? hasItems
    , FormatFlags formatFlags = DefaultCallerTypeFlags);

    int AppendCloseCollection(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType, int? totalItemCount
    , string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextItemFormat<TFmt>(IMoldWriteState mws, TFmt item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?;

    WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(IMoldWriteState mws, TFmtStruct? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(IMoldWriteState mws, TCloaked? item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull;

    AppendSummary CollectionNextItemFormat(IMoldWriteState mws, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextItemFormat(IMoldWriteState mws, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextCharSeqFormat<TCharSeq>(IMoldWriteState mws, TCharSeq item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?;

    AppendSummary CollectionNextItemFormat(IMoldWriteState mws, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary CollectionNextStringBearerFormat<TBearer>(IMoldWriteState mws, TBearer item, int retrieveCount, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?;

    Range? AddCollectionElementSeparator(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags);

    (int, int) InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex, WrittenAsFlags writtenAs
    , int firstFieldIndex, FormatFlags createTypeFlags, int contentLength = -1, IMoldWriteState? liveMoldInternal = null);
    
    int AppendExistingReferenceId(IMoldWriteState mws, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags);

    int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags);

    int AppendInstanceInfoField(IMoldWriteState mws, string fieldName, ReadOnlySpan<char> description
    , WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags);
    
    WrittenAsFlags AppendFieldName(IMoldWriteState mws, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldNameMatch<T>(IMoldWriteState mws, T source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName<TFmt>(IMoldWriteState mws, TFmt source, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldName<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, ICharSequence source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldName(IMoldWriteState mws, StringBuilder source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary FormatFieldName<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    AppendSummary FormatBearerFieldName<TBearer>(IMoldWriteState mws, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?;

    WrittenAsFlags FormatFieldContentsMatch<TAny>(IMoldWriteState mws, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents<TFmt>(IMoldWriteState mws, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?;

    WrittenAsFlags FormatFieldContents<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable;

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFallbackFieldContents<TAny>(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, char[] source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    WrittenAsFlags FormatFieldContents(IMoldWriteState mws, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags);

    AppendSummary FormatFieldContents<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value, PalantírReveal<TRevealBase> valueRevealer
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull;

    AppendSummary FormatBearerFieldContents<TBearer>(IMoldWriteState mws, TBearer styledObj, string? callerFormatString = null
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
