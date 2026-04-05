// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool> value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool?> value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmt> value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmtStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
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

    public TMold WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
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

    public TMold WhenConditionMetRevealFiltered<TBearerStruct>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName, Span<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
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
    public TMold WhenConditionMetAddFilteredObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddFilteredObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool> value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool?> value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmt, TFmtBase>(
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

    public TMold WhenConditionMetAddFiltered<TFmtStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
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

    public TMold WhenConditionMetRevealFiltered<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TBearer, TFilterBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TBearer : IStringBearer?, TFilterBase? =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TBearerStruct>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddFilteredObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);


    [CallsObjectToString]
    public TMold WhenConditionMetAddFilteredObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , bool[]? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , bool?[]? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TFmt[]? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmtStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TCloaked[]? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TCloakedStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TBearer[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TBearerStruct>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , string?[]? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , StringBuilder?[]? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TAny[]? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddFilteredObject(
        bool condition
      , string fieldName
      , object?[]? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<bool>? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<bool?>? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmt>? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered<TFmtStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TCloaked, TFilterBase, TRevealBase>(
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
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TCloakedStruct>(
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

    public TMold WhenConditionMetRevealFiltered<TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealFiltered<TBearerStruct>(
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

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<string?>? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredCharSeq<TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFiltered(
        bool condition
      , string fieldName
      , IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddFilteredMatch<TAny, TAnyBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        condition
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddFilteredObject(
        bool condition
      , string fieldName
      , IReadOnlyList<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddFilteredMatch(condition, fieldName, value, filterPredicate, formatString, formatFlags);
}
