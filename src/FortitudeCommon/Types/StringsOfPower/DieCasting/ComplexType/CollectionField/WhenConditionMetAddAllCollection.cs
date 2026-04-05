// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmt>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearer>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearer> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearerStruct?> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);


    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<object?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmt>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloakedStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearer>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearer> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearerStruct?> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<object?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmt>(
        bool condition
      , string fieldName
      , TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , string fieldName
      , TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TCloaked[]? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloakedStruct>(
        bool condition
      , string fieldName
      , TCloakedStruct?[]? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearer>(
        bool condition
      , string fieldName
      , TBearer[]? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , string fieldName
      , TBearerStruct?[]? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , string fieldName
      , TCharSeq[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , string fieldName
      , TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObject(
        bool condition
      , string fieldName
      , object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(condition, fieldName, value, formatString, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmt>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCloaked>? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TCloakedStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCloakedStruct?>? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearer>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearer>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer>), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearerStruct?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCharSeq>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , string fieldName
      , IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllObject(
        bool condition
      , string fieldName
      , IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(condition, fieldName, value, formatString, formatFlags);

}
