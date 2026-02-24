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
    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool> value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool?> value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmtStruct?> value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCloaked> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName, Span<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        condition
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool> value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool?> value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmt> value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmtStruct?> value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloaked> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearer, TFilterBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TFilterBase? =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        condition
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);


    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , bool[]? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , bool?[]? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TFmt?[]? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmtStruct>(
        bool condition
      , string fieldName
      , TFmtStruct?[]? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TCloaked?[]? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , string fieldName
      , TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearerStruct>(
        bool condition
      , string fieldName
      , TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , string?[]? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TCharSeq[]? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        condition
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , StringBuilder?[]? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TAny?[]? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObject(
        bool condition
      , string fieldName
      , object?[]? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<bool>? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<bool?>? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmt?>? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TFmtStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFiltered<TBearerStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<string?>? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TAny>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObject(
        bool condition
      , string fieldName
      , IReadOnlyList<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<bool?>? =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt?>? 
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmt?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>? 
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked?>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer?>?
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredStringEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredStringBuilderEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<StringBuilder?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TAny?>? 
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObjectEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?  =>
        WhenConditionMetAddFilteredMatchEnumerate<TEnumbl, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?  =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<bool?>?  =>
        condition
            ? AlwaysAddFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmt?>?
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmt?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked?>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer?>?
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredStringIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredCharSeqIterate<TEnumtr, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq?>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredCharSeqIterate<TEnumtr, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredStringBuilderIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny?>?
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredObjectIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>? =>
        WhenConditionMetAddFilteredMatchIterate<TEnumtr, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);
}
