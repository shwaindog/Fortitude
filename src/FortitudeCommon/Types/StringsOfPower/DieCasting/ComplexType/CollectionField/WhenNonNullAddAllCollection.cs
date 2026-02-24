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

    public TExt WhenNonNullAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool>? =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool?>? =>
        WhenConditionMetAddAllEnumerateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TEnumbl, TFmt>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmt>?
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullable<TEnumbl, TFmtStruct>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>
    (string fieldName, TEnumbl value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>
    (string fieldName, TEnumbl value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllEnumerateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllStringEnumerate<TEnumbl>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<string?>? =>
        WhenConditionMetAddAllStringEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeqEnumerate<TEnumbl, TCharSeq>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCharSeq>?
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllCharSeqEnumerate<TEnumbl, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllStringBuilderEnumerate<TEnumbl>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<StringBuilder?>? =>
        WhenConditionMetAddAllStringBuilderEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchEnumerate<TEnumbl, TAny>
    (string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TAny>? =>
        WhenConditionMetAddAllMatchEnumerate<TEnumbl, TAny>(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<object?>? =>
        WhenNonNullAddAllMatchEnumerate<TEnumbl, object?>(fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool>? =>
        WhenConditionMetAddAllIterate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool?>? =>
        WhenConditionMetAddAllIterateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr, TFmt>
    (string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TFmt>?
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllIterate<TEnumtr, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullable<TEnumtr, TFmtStruct>
    (string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllIterate<TEnumtr, TCloaked, TRevealBase>
    (string fieldName, TEnumtr value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllIterateNullable<TEnumtr, TCloakedStruct>
    (string fieldName, TEnumtr value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TCloakedStruct?>? 
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllIterateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TExt WhenNonNullRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllStringIterate<TEnumtr>
    (string fieldName, TEnumtr value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<string?>?  =>
        WhenConditionMetAddAllStringIterate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllCharSeqIterate<TEnumtr, TCharSeq>
    (string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TCharSeq>? 
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllCharSeqIterate<TEnumtr, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllStringBuilderIterate<TEnumtr>
    (string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<StringBuilder?>? =>
        WhenConditionMetAddAllStringBuilderIterate(value != null, fieldName, value, formatString, formatFlags);

    public TExt WhenNonNullAddAllMatchIterate<TEnumtr, TAny>
    (string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TAny>? =>
        WhenConditionMetAddAllMatchIterate<TEnumtr, TAny>(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<object?>? =>
        WhenNonNullAddAllMatchIterate<TEnumtr, object?>(fieldName, value, formatString, formatFlags);
}
