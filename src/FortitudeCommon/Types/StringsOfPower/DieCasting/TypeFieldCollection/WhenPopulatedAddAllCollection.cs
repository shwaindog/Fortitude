// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllNullable<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, Span<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer?> value) where TBearer : class, IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeqNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatchNull<TAny>(ReadOnlySpan<char> fieldName, Span<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllNullable<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value) where TBearer : class, IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeqNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, TBearer?[]? value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, TAny?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value)
        where TBearer : IStringBearer =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value)
        where TBearer : IStringBearer =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool>, bool>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool?>, bool?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearer>(value!);
            while (hasValue)
            {
                eoctb.AddBearerElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearerStruct>(value!);
            while (hasValue)
            {
                eoctb.AddBearerElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<string>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?>(value!);
            while (hasValue)
            {
                eoctb.AddCharSequenceElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<StringBuilder>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TAny>(value!);
            while (hasValue)
            {
                eoctb.AddMatchElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString);
}
