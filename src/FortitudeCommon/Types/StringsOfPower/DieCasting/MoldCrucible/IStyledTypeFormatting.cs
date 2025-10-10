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

    ITypeMolderDieCast<TMold> AppendValueTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold, Type valueType, string? alternativeName = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendValueTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold, Type valueType) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendComplexTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold, Type complexType, string? alternativeName = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ReadOnlySpan<char> fieldName) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendFieldValueSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AddNextFieldSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder;

    int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel);

    ITypeMolderDieCast<TMold> AppendTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyedCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyedCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null)
        where TMold : TypeMolder where TValue : TVBase;

    ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler)
        where TMold : TypeMolder where TValue : TVBase where TKey : TKBase;

    ITypeMolderDieCast<TMold> AppendKeyedCollectionNextItem<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType, bool hasItems
      , Type collectionType)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked item
      , int retrieveCount
      , PalantírReveal<TCloakedBase> styler) where TMold : TypeMolder where TCloaked : TCloakedBase;

    ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, string? item, int retrieveCount
      , string? formatString = null) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold, TCharSeq>(ITypeMolderDieCast<TMold> typeMold, TCharSeq? item, int retrieveCount
      , string? formatString = null) where TMold : TypeMolder where TCharSeq : ICharSequence;

    ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder? item, int retrieveCount
      , string? formatString = null) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer? item, int retrieveCount)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType, int totalItemCount)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> AddCollectionElementSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold, Type elementType, int nextItemNumber)
        where TMold : TypeMolder;

    IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null);

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source, string? formatString = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source, string? formatString = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source, string? formatString = null)
        where TMold : TypeMolder where TFmt : ISpanFormattable;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmtStruct>(ITypeMolderDieCast<TMold> typeMold, TFmtStruct? source, string? formatString = null)
        where TMold : TypeMolder where TFmtStruct : struct, ISpanFormattable;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler)
        where TMold : TypeMolder where TCloaked : TCloakedBase;

    ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer styledObj)
        where TMold : TypeMolder;

    IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null);

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source, string? formatString = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source, string? formatString = null)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source, string? formatString = null)
        where TMold : TypeMolder where TFmt : ISpanFormattable;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source, string? formatString = null)
        where TMold : TypeMolder where TFmt : struct, ISpanFormattable;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue)
        where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler)
        where TMold : TypeMolder where TCloaked : TCloakedBase;

    ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer styledObj)
        where TMold : TypeMolder;
}

public static class StyleTypeFormattingExtensions
{
    public static IStringBuilder RemoveLastWhiteSpacedCommaIfFound(this IStringBuilder sb)
    {
        if (sb.Length < 2) return sb;
        if (sb[^1] == ',')
        {
            sb.Length -= 1;
            return sb;
        }
        if (sb[^2] == ',' && sb[^1] == ' ') sb.Length -= 2;
        var i                                         = sb.Length - 1;
        for (; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (sb[i] == ',')
            {
                sb.Length = i;
                return sb;
            }
        sb.Length = i + 1;
        return sb;
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
