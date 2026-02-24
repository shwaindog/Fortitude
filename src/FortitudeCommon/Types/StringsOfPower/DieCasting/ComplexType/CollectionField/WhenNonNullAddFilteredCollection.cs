// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredNullable(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredCharSeq(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredNullable(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredMatch(value is { Length: > 0 }, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TFilterBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TFilterBase? =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);


    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence, TCharSeqBase =>
        WhenConditionMetAddFilteredCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>
    (string fieldName, TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase =>
        WhenConditionMetAddFilteredMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, IReadOnlyList<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase =>
        WhenConditionMetAddFilteredCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>
    (string fieldName, IReadOnlyList<TAny>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TEnumbl>(string fieldName, TEnumbl value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerateNullable<TEnumbl>(string fieldName, TEnumbl value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool?>?  =>
        WhenConditionMetAddFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmt?>?
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked?>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TBearer?>? 
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>
    (string fieldName
    , TEnumbl value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TBearerStruct?>? 
      where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredStringEnumerate<TEnumbl>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<string?>? =>
        WhenConditionMetAddFilteredStringEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredStringBuilderEnumerate<TEnumbl>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<StringBuilder?>? =>
        WhenConditionMetAddFilteredStringBuilderEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>? 
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectEnumerate<TEnumbl>
    (string fieldName, TEnumbl value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<object?>? =>
        WhenNonNullAddFilteredMatchEnumerate<TEnumbl, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredIterate<TEnumtr>(string fieldName, TEnumtr value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool>? =>
        WhenConditionMetAddFilteredIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredIterateNullable<TEnumtr>(string fieldName, TEnumtr value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool?>?  =>
        WhenConditionMetAddFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredIterate<TEnumtr, TFmt, TFmtBase>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<TFmt?>? 
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredIterateNullable<TEnumtr, TFmtStruct>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCloaked?>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TBearer?>?
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
      string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
       where TEnumtr : IEnumerator<TBearerStruct?>?
       where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredStringIterate<TEnumtr>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<string?>? =>
        WhenConditionMetAddFilteredStringIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeqIterate<TEnumtr, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq?>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredCharSeqIterate<TEnumtr, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredStringBuilderIterate<TEnumtr>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<StringBuilder?>? =>
        WhenConditionMetAddFilteredStringBuilderIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumtr : IEnumerator<TAny>?
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectIterate<TEnumtr>
    (string fieldName, TEnumtr value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<object?>?
      => WhenNonNullAddFilteredMatchIterate<TEnumtr, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);
}
