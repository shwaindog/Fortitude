// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmt>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloaked, TRevealBase>(
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

    public TExt WhenConditionMetRevealAll<TCloakedStruct>(
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

    public TExt WhenConditionMetRevealAll<TBearer>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearer> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TBearerStruct?> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatchNull<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<object?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmt>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloaked, TRevealBase>(
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

    public TExt WhenConditionMetRevealAll<TCloakedStruct>(
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

    public TExt WhenConditionMetRevealAll<TBearer>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearer> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TBearerStruct?> value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatchNullable<TAny>(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObject(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObjectNullable(
        bool condition
      , ReadOnlySpan<char> fieldName
      , ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<object?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmt>(
        bool condition
      , string fieldName
      , TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , string fieldName
      , TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TCloaked?[]? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloakedStruct>(
        bool condition
      , string fieldName
      , TCloakedStruct?[]? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TBearer>(
        bool condition
      , string fieldName
      , TBearer?[]? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , string fieldName
      , TBearerStruct?[]? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , string fieldName
      , TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , string fieldName
      , TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObject(
        bool condition
      , string fieldName
      , object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(condition, fieldName, value, formatString, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmt>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TFmtStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCloaked?>? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TCloakedStruct>(
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

    public TExt WhenConditionMetRevealAll<TBearer>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearer?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAll<TBearerStruct>(
        bool condition
      , string fieldName
      , IReadOnlyList<TBearerStruct?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeq<TCharSeq>(
        bool condition
      , string fieldName
      , IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence =>
        condition
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll(
        bool condition
      , string fieldName
      , IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatch<TAny>(
        bool condition
      , string fieldName
      , IReadOnlyList<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObject(
        bool condition
      , string fieldName
      , IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(condition, fieldName, value, formatString, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        condition
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>? =>
        condition
            ? AlwaysAddAllEnumerateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllEnumerate<TEnumbl, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmt?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllEnumerateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(
        bool condition
      , string fieldName
      , TEnumbl value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllStringEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>? =>
        condition
            ? AlwaysAddAllStringEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeqEnumerate<TEnumbl, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeqEnumerate<TEnumbl, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllStringBuilderEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>? =>
        condition
            ? AlwaysAddAllStringBuilderEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatchEnumerate<TEnumbl, TAny>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>? =>
        condition
            ? AlwaysAddAllMatchEnumerate<TEnumbl, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObjectEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>? =>
        WhenConditionMetAddAllMatchEnumerate<TEnumbl, object?>(condition, fieldName, value, formatString, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>? =>
        condition
            ? AlwaysAddAllIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>? =>
        condition
            ? AlwaysAddAllIterateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr, TFmt>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllIterate<TEnumtr, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmt?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloaked?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCloakedStruct?>? 
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllIterateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(
        bool condition
      , string fieldName
      , TEnumtr value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearer?>), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllStringIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>?  =>
        condition
            ? AlwaysAddAllStringIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<string?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllCharSeqIterate<TEnumtr, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCharSeq>? 
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllCharSeqIterate<TEnumtr, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllStringBuilderIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>? =>
        condition
            ? AlwaysAddAllStringBuilderIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllMatchIterate<TEnumtr, TAny>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>? =>
        condition
            ? AlwaysAddAllMatchIterate<TEnumtr, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllObjectIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>? =>
        WhenConditionMetAddAllMatchIterate<TEnumtr, object?>(condition, fieldName, value, formatString, formatFlags);
}
