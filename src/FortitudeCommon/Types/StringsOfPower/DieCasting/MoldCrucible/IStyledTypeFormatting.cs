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

    ITypeMolderDieCast<TB> AppendValueTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder, Type valueType, string? alternativeName = null) where TB : TypeMolder;

    ITypeMolderDieCast<TB> AppendValueTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder, Type valueType) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> AppendComplexTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder, Type complexType, string? alternativeName = null) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> AppendFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, ReadOnlySpan<char> fieldName) where TB : TypeMolder;

    ITypeMolderDieCast<TB> AppendFieldValueSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder) where TB : TypeMolder;

    ITypeMolderDieCast<TB> AddNextFieldSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder;

    int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel);

    ITypeMolderDieCast<TB> AppendTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder) where TB : TypeMolder;

    ITypeMolderDieCast<TB> AppendKeyedCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> AppendKeyedCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TVBase>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null) 
        where TB : TypeMolder where TValue : TVBase;
    
    ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) 
        where TB : TypeMolder where TValue : TVBase where TKey : TKBase;
    
    ITypeMolderDieCast<TB> AppendKeyedCollectionNextItem<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType, bool hasItems, Type collectionType)
        where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(ITypeMolderDieCast<TB> typeBuilder, TCustStyle item, int retrieveCount
      , PalantírReveal<TCustBase> styler) where TB : TypeMolder where TCustStyle : TCustBase;
    
    ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, string? item, int retrieveCount
      , string? formatString = null) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCharSeq>(ITypeMolderDieCast<TB> typeBuilder, TCharSeq? item, int retrieveCount
      , string? formatString = null) where TB : TypeMolder where TCharSeq : ICharSequence;
    
    ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder? item, int retrieveCount
      , string? formatString = null) where TB : TypeMolder;
    
    ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer? item, int retrieveCount) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType, int totalItemCount)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> AddCollectionElementSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder, Type elementType, int nextItemNumber)
        where TB : TypeMolder;

    IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source, string? formatString = null);

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source, string? formatString = null)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source, string? formatString = null)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : TypeMolder where TFmt : ISpanFormattable;

    ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : TypeMolder where TFmt : struct, ISpanFormattable;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldName<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle, PalantírReveal<TBase> styler)
        where TB : TypeMolder where T : TBase;

    ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer styledObj)
        where TB : TypeMolder;

    IStringBuilder FormatFieldContentsMatch<T>(IStringBuilder sb, T source, string? formatString = null);

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source, string? formatString = null)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source, string? formatString = null)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : TypeMolder where TFmt : ISpanFormattable;

    ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : TypeMolder where TFmt : struct, ISpanFormattable;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue)
        where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder;

    ITypeMolderDieCast<TB> FormatFieldContents<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle, PalantírReveal<TBase> styler)
        where TB : TypeMolder where T : TBase;

    ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer styledObj)
        where TB : TypeMolder;
}


public static class StyleTypeFormattingExtensions
{
    public static IStringBuilder RemoveLastWhiteSpacedCommaIfFound(this IStringBuilder sb)
    {
        if(sb.Length < 2) return sb;
        if (sb[^1] == ',')
        {
            sb.Length -= 1;
            return sb;
        }
        if (sb[^2] == ',' && sb[^1] == ' ')
        {
            sb.Length -= 2;
        }
        var i = sb.Length - 1;
        for (; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (sb[i] == ',')
            {
                sb.Length = i;
                return sb;
            }
        sb.Length = i+1;
        return sb;
    }
    
    public static int RemoveLastWhiteSpacedCommaIfFound(this Span<char> destSpan, int destStartIndex)
    {
        if(destStartIndex < 2) return 0;
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