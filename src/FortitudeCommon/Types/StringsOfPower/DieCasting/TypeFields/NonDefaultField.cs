// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonDefaultAdd
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmt>
    (string fieldName, TFmt value, TFmt? defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmtStruct>
    (string fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TToStyle, TStylerType>
    (string fieldName, TToStyle value
      , StringBearerRevealState<TStylerType> stringBearerRevealState, TToStyle? defaultValue = default) where TToStyle : TStylerType =>
        !stb.SkipFields && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, stringBearerRevealState) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value.Length > 0 ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, string value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        !stb.SkipFields && length > 0 ? AlwaysAdd(fieldName, value, startIndex, length, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value) =>
        !stb.SkipFields && value.Length > 0 ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        !stb.SkipFields && length > 0 ? AlwaysAdd(fieldName, value, startIndex, length, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddStyled<TStyled>(string fieldName, TStyled? value, TStyled? defaultValue = default) 
        where TStyled : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, ICharSequence value, string defaultValue = "") =>
        !stb.SkipFields && !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, ICharSequence value, int startIndex, int count = int.MaxValue, string? formatString = null
      , string defaultValue = "") =>
        !stb.SkipFields && !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, StringBuilder value, string defaultValue = "") =>
        !stb.SkipFields && !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, StringBuilder value, int startIndex, int count = int.MaxValue, string? formatString = null, string defaultValue = "") =>
        !stb.SkipFields && !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddMatch<T>(string fieldName, T? value, T? defaultValue = default, string? formatString = null)  =>
        !stb.SkipFields && !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true) ? AlwaysAddMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonDefaultAddObject(string fieldName, object? value, object? defaultValue = null, string? formatString = null) =>
        !stb.SkipFields && !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true) ? AlwaysAddObject(fieldName, value, formatString) : stb.StyleTypeBuilder;

}
