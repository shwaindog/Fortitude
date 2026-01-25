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
    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllNullable(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllCharSeq(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllNullable(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchNull<TAny>(ReadOnlySpan<char> fieldName, Span<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllNullable(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllCharSeq(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllNullable(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value is { Length: > 0 }, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>
    (string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
    (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAll(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAll(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearer>(string fieldName, TBearer?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        WhenConditionMetRevealAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(
        string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        WhenConditionMetAddAllCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>(
        string fieldName
      , TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>
    (string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAll(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAll(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        WhenConditionMetRevealAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>
    (string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        WhenConditionMetAddAllCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAll
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAll(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>
    (string fieldName, IReadOnlyList<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatch(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        WhenConditionMetAddAllCharSeqEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchEnumerate<TAny>
    (string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatchEnumerate(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        WhenConditionMetAddAllCharSeqEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchEnumerate<TAny>
    (string fieldName, IEnumerator<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddAllMatchEnumerate(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);
}
