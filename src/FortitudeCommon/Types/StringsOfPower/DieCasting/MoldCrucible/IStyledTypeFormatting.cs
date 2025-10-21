// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public interface IStyledTypeFormatting : ICustomStringFormatter
{
    string Name { get; }

    IStringBuilder AppendValueTypeOpening(IStringBuilder sb, Type valueType, string? alternativeName = null);

    IStringBuilder AppendValueTypeClosing(IStringBuilder sb, Type valueType);

    IStringBuilder AppendComplexTypeOpening(IStringBuilder sb, Type complexType, string? alternativeName = null);

    IStringBuilder AppendFieldValueSeparator(IStringBuilder sb);

    IStringBuilder AddNextFieldSeparator(IStringBuilder sb);

    int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel);

    IStringBuilder AppendTypeClosing(IStringBuilder sb);

    IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType);

    IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount);

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null)
        where TMold : TypeMolder where TValue : TVBase;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler)
        where TMold : TypeMolder where TValue : TVBase where TKey : TKBase;

    IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber);

    IStringBuilder FormatCollectionStart(IStringBuilder sb, Type itemElementType, bool hasItems, Type collectionType);

    IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase;

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null);

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null);

    IStringBuilder CollectionNextItemFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount, string? formatString = null)  where TCharSeq : ICharSequence;

    IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null);

    IStringBuilder CollectionNextItemFormat(ITheOneString tos, IStringBearer? item, int retrieveCount);

    IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType, int totalItemCount);

    IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber);
    
    IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName);
    
    IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null);

    IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null);

    IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null);

    IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null) where TFmt : ISpanFormattable;

    IStringBuilder FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    IStringBuilder FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldName<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle, PalantírReveal<TCloakedBase> styler)
        where TCloaked : TCloakedBase;

    IStringBuilder FormatFieldName(ITheOneString tos, IStringBearer styledObj);

    IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null);

    IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null);

    IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null);

    IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null)
        where TFmt : ISpanFormattable;

    IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue);

    IStringBuilder FormatFieldContents<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle, PalantírReveal<TCloakedBase> styler)
        where TCloaked : TCloakedBase;

    IStringBuilder FormatFieldContents(ITheOneString tos, IStringBearer styledObj);
}

public static class StyleTypeFormattingExtensions
{
    
    public static IStringBuilder FieldEnd(this IStringBuilder sb, IStyledTypeFormatting stf) => stf.AppendFieldValueSeparator(sb);


    public static char RemoveLastWhiteSpacedCommaIfFound(this IStringBuilder sb)
    {
        if (sb.Length < 2) return sb.Length > 0 ? sb[0] : '\0';
        if (sb[^1] == ',')
        {
            sb.Length -= 1;
            return sb[^1];
        }
        if (sb[^2] == ',' && sb[^1] == ' ') sb.Length -= 2;
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
