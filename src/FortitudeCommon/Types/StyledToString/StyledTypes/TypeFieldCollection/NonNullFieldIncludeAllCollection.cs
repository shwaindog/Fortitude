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
    public TExt  WhenNonNullAddAll(string fieldName, bool[]? value) => !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, bool?[]? value) => !stb.SkipBody &&  value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TToStyle, TStylerType>
        (string fieldName, TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, ICharSequence?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStyledObj>(string fieldName, TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject  => 
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAllMatch<T>
    (string fieldName, T[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        !stb.SkipBody && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TToStyle, TStylerType>
        (string fieldName, IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject  => 
        !stb.SkipBody && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAllMatch<T>
    (string fieldName, IReadOnlyList<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        !stb.SkipBody && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerable<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TStyledObj>(string fieldName, IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject  => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAllMatchEnumerate<T>
    (string fieldName, IEnumerable<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        !stb.SkipBody && value != null ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool?>? value) => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerator<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TStyledObj>(string fieldName, IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject => 
        !stb.SkipBody && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt  WhenNonNullAddAllMatchEnumerate<T>
    (string fieldName, IEnumerator<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class =>
        !stb.SkipBody && value != null ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
