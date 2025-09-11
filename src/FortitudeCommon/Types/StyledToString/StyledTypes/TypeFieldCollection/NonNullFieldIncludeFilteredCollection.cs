// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSequence<TCharSeq>
        (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate) where TCharSeq : ICharSequence =>
        !stb.SkipBody && value != null ? AlwaysAddFilteredCharSequence(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStyledObj>(string fieldName, TStyledObj[]? value
      , OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<T, TBase1, TBase2>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase1?> filterPredicate
      , CustomTypeStyler<TBase2?> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipBody && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSequence<TCharSeq>
        (string fieldName, IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate) 
        where TCharSeq : ICharSequence =>
        !stb.SkipBody && value != null ? AlwaysAddFilteredCharSequence(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) =>
        !stb.SkipBody && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredMatch<T>
    (string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        !stb.SkipBody && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
}
