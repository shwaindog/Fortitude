// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonDefaultAdd
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmt>
    (string fieldName, TFmt value, TFmt? defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TToStyle, TStylerType>
    (string fieldName, TToStyle value
      , CustomTypeStyler<TStylerType> customTypeStyler, TToStyle? defaultValue = default) where TToStyle : TStylerType =>
        !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value.Length > 0 ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, string value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        length > 0 ? AlwaysAdd(fieldName, value, startIndex, length, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value) =>
        value.Length > 0 ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        length > 0 ? AlwaysAdd(fieldName, value, startIndex, length, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) =>
        !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, ICharSequence value, string defaultValue = "") =>
        !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, ICharSequence value, int startIndex, int count = int.MaxValue, string? formatString = null
      , string defaultValue = "") =>
        !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, StringBuilder value, string defaultValue = "") =>
        !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd
    (string fieldName, StringBuilder value, int startIndex, int count = int.MaxValue, string? formatString = null, string defaultValue = "") =>
        !value.Equals(defaultValue) ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonDefaultAdd(string fieldName, object? value, object? defaultValue = null, string? formatString = null) =>
        !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

}
