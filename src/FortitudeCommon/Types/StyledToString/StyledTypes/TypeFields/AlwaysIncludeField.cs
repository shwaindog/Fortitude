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
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, bool? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TFmt>(string fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TFmt>(string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : struct, ISpanFormattable =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TToStyle, TStylerType>(string fieldName, TToStyle? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, customTypeStyler).AddGoToNext(stb);

    public TExt AlwaysAdd<TEnum>(string fieldName, TEnum? value) where TEnum : Enum =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb)
                : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName,stb).AppendOrNull(value ?? "null").AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, formatString, stb);

    public TExt AlwaysAdd(string fieldName, char[]? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, formatString, stb);

    public TExt AlwaysAdd(string fieldName, IStyledToStringObject? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, ICharSequence? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .Append(value, startIndex, Math.Clamp(length, 0, (value?.Length ?? startIndex) - startIndex), formatString)
                 .AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, StringBuilder? value) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).Append(value, startIndex, Math.Clamp(length, 0, (value?.Length ?? startIndex) - startIndex), formatString)
                 .AddGoToNext(stb);
    
    public TExt AlwaysAddMatch<T>(string fieldName, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddMatchWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    [CallsObjectToString]
    public TExt AlwaysAddObject(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipBody
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? AlwaysAddObjectWithFormatting(fieldName, value, formatString)
                : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAddWithFormatting<TFmt>(string fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TFmt : ISpanFormattable =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting<TFmt>(string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TFmt : struct, ISpanFormattable =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddObjectWithFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddMatchWithFormatting<T>(string fieldName, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.SkipBody ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);
}
