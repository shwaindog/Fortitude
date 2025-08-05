// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public IStringBuilder Sb => stb.Sb;

    public TExt AlwaysAdd(string fieldName, bool value) => 
        stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, bool? value) => 
        stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString) where TStruct : struct =>
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, structToString).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value ?? "null").AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, string? value, int startIndex, int length) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, stb);

    public TExt AlwaysAdd(string fieldName, char[]? value, int startIndex, int length) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, stb);

    public TExt AlwaysAdd(string fieldName, IStyledToStringObject? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    [CallsObjectToString]
    public TExt AlwaysAdd(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAddWithFormatting<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);
}

