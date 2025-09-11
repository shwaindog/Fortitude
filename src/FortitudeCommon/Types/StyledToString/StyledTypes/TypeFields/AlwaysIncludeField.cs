// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public IStringBuilder Sb => stb.Sb;

    public TExt AlwaysAdd(string fieldName, bool value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, bool? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd<TFmt>(string fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendValue(value).AddGoToNext();

    public TExt AlwaysAdd<TFmt>(string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : struct, ISpanFormattable =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd<TToStyle, TStylerType>(string fieldName, TToStyle? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, customTypeStyler).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendOrNull(value ?? "null").AddGoToNext();

    public TExt AlwaysAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, char[]? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, IStyledToStringObject? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, ICharSequence? value) =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, StringBuilder? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty() 
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();
    
    public TExt AlwaysAddMatch<T>(string fieldName, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddMatchWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendMatchOrNull(value).AddGoToNext();

    [CallsObjectToString]
    public TExt AlwaysAddObject(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddObjectWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendMatchOrNull(value).AddGoToNext();

    public TExt AlwaysAddWithFormatting<TFmt>(string fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TFmt : ISpanFormattable =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName)
                 .StyleFormatter.Format(value, stb.Sb, formatString)
                 .AnyToTypeBuilder(stb);

    public TExt AlwaysAddWithFormatting<TFmt>(string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TFmt : struct, ISpanFormattable =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendNullableFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddWithFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext();

    public TExt AlwaysAddWithFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody 
            ? stb.StyleTypeBuilder 
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, 0).AddGoToNext();

    public TExt AlwaysAddWithFormatting(string fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, 0).AddGoToNext();

    public TExt AlwaysAddWithFormatting(string fieldName, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, 0).AddGoToNext();

    public TExt AlwaysAddWithFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, 0).AddGoToNext();

    public TExt AlwaysAddObjectWithFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddMatchWithFormatting<T>(string fieldName, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendMatchFormattedOrNull(value, formatString).AddGoToNext();
}
