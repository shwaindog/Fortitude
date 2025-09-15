// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public interface IStyledTypeFormatting : ICustomStringFormatter
{
    string Name { get; }

    IStyleTypeBuilderComponentAccess<TB> AppendValueTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type valueType, string? alternativeName = null) where TB : StyledTypeBuilder;
    
    IStyleTypeBuilderComponentAccess<TB> AppendComplexTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type complexType, string? alternativeName = null) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> AppendFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string fieldName) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> AppendFieldValueSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> AppendTypeClosing<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatCollectionStart<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType, bool hasItems, Type collectionType)
        where TB : StyledTypeBuilder;
    
    IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TCustStyle item, int retrieveCount
      , CustomTypeStyler<TCustBase> styler) where TB : StyledTypeBuilder where TCustStyle : TCustBase;
    
    IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string? item, int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder;
    
    IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCharSeq>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TCharSeq? item, int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder where TCharSeq : ICharSequence;
    
    IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder? item, int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder;
    
    IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject? item, int retrieveCount) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatCollectionEnd<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType, int totalItemCount)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> AddCollectionElementSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type elementType, int nextItemNumber)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> AddNextFieldSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldNameMatch<TB, T>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt source, string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : ISpanFormattable;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle, CustomTypeStyler<TBase> styler)
        where TB : StyledTypeBuilder where T : TBase;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject styledObj)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContentsMatch<TB, T>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source, string? formatString = null)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt source, string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : ISpanFormattable;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source, string? formatString = null)
        where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = Int32.MaxValue)
        where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle, CustomTypeStyler<TBase> styler)
        where TB : StyledTypeBuilder where T : TBase;

    IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject styledObj)
        where TB : StyledTypeBuilder;
}
