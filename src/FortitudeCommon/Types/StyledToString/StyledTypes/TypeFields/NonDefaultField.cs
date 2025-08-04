// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AddWhenNonDefault
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault<TNum>
    (string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        !Equals(value, defaultValue) ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault<TStruct>
    (string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct =>
        !Equals(value, defaultValue) ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault
    (string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value.Length > 0 ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault
    (string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault(string fieldName, string value, int startIndex, int length) =>
        length > 0 ? AddAlways(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault(string fieldName, char[] value, int startIndex, int length) =>
        length > 0 ? AddAlways(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) =>
        Equals(value, defaultValue) ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault
    (string fieldName, IFrozenString value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault
    (string fieldName, IStringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonDefault
    (string fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt AddWhenNonDefault(string fieldName, object? value, object? defaultValue = null, string? formatString = null) =>
        !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

}
