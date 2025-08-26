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

    public TExt AlwaysAdd<TFmtStruct>(string fieldName, TFmtStruct value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TFmtStruct>(string fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        formatString.IsNotNullOrEmpty() 
            ? AlwaysAddWithFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt AlwaysAdd<TStruct>(string fieldName, TStruct? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, customTypeStyler).AddGoToNext(stb);

    public TExt AlwaysAdd<TEnum>(string fieldName, TEnum? value) where TEnum : Enum =>
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value).AddGoToNext(stb);

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

    public TExt AlwaysAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, formatString,  stb);

    public TExt AlwaysAdd(string fieldName, char[]? value) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, formatString, stb);

    public TExt AlwaysAdd(string fieldName, IStyledToStringObject? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);
    
    public TExt AlwaysAdd<T, TBase>(string fieldName, T? value, CustomTypeStyler<TBase> overrideStyler) where T : class, TBase where TBase : class =>  
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, overrideStyler).AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, ICharSequence? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null) => 
         stb.FieldNameJoin(fieldName)
            .Append(value, startIndex, Math.Clamp(length, 0, (value?.Length ?? startIndex) - startIndex), formatString)
            .AddGoToNext(stb);

    public TExt AlwaysAdd(string fieldName, StringBuilder? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt AlwaysAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null) => 
        stb.FieldNameJoin(fieldName).Append(value, startIndex, Math.Clamp(length, 0, (value?.Length ?? startIndex) - startIndex), formatString)
           .AddGoToNext(stb);

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

    public TExt AlwaysAddWithFormatting(string fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt AlwaysAddWithFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);
}

