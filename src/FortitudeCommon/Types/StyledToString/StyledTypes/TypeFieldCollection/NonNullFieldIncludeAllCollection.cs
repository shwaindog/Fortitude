// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt  WhenNonNullAddAll(string fieldName, bool[]? value) => value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, bool?[]? value) => value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>(string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IStyledToStringObject?[]? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IFrozenString?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IStringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAll
    (string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<IStyledToStringObject?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerable<bool>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerable<bool?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerable<IStyledToStringObject?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerable<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerable<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerator<bool>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerator<bool?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TNum>
    (string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddAll(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IEnumerator<IStyledToStringObject?>? value) => 
        value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerator<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerator<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAll
    (string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
