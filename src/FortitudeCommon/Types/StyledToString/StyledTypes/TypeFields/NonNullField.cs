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

    public TExt WhenNonNullAdd<TFmtStruct> (string fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        value != null ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TStruct>(string fieldName, TStruct? value, StructStyler<TStruct> structToString)
        where TStruct : struct =>
        !Equals(value, null) ? AlwaysAdd(fieldName, value.Value, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value, int startIndex, int count = int.MaxValue, string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, char[]? value) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, char[]? value, int startIndex, int count = int.MaxValue, string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, IStyledToStringObject? value) => 
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, ICharSequence? value) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, ICharSequence? value, int startIndex, int count = int.MaxValue, string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, StringBuilder? value) => 
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue, string? formatString = null) => 
        value != null ? AlwaysAdd(fieldName, value, startIndex, count, formatString) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenNonNullAdd
    (string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;
}
