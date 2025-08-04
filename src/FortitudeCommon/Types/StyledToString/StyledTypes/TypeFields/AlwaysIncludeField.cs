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

    public TExt AddAlways(string fieldName, bool value) => 
        stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AddAlways(string fieldName, bool? value) => 
        stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AddAlways<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AddAlways<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AddAlways<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString) where TStruct : struct =>
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, structToString).AddGoToNext(stb);

    public TExt AddAlways(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AddAlways(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value ?? "null").AddGoToNext(stb);

    public TExt AddAlways(string fieldName, string? value, int startIndex, int length) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, stb);

    public TExt AddAlways(string fieldName, char[]? value, int startIndex, int length) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, stb);

    public TExt AddAlways(string fieldName, IStyledToStringObject? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AddAlways(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AddAlways(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AddAlways(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    [CallsObjectToString]
    public TExt AddAlways(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? AddAlwaysAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AddAlwaysAndFormatting<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AddAlwaysAndFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);
}

