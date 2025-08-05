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
    public TExt WhenNonNullAdd (string fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TNum> (string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TStruct>(string fieldName, TStruct? value, StructStyler<TStruct> structToString)
        where TStruct : struct =>
        !Equals(value, null) ? AlwaysAdd(fieldName, value.Value, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value, int startIndex, int length) =>
        value != null ? AlwaysAdd(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, char[]? value, int startIndex, int length) =>
        value != null ? AlwaysAdd(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, IStyledToStringObject? value) => 
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, IStringBuilder? value, string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, StringBuilder? value, string? formatString = null) => 
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenNonNullAdd
    (string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;
}
