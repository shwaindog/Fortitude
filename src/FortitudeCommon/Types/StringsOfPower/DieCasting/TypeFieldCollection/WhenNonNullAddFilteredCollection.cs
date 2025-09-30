// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
        (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate) where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, IReadOnlyList<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
        (string fieldName, IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate) 
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, IReadOnlyList<T?>? value, OrderedCollectionPredicate<TBase?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
}
