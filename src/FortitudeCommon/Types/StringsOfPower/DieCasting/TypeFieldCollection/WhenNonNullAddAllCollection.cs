// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAllNullable<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, Span<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer?> value) where TBearer : class, IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeqNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllMatchNull<TAny>(ReadOnlySpan<char> fieldName, Span<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAllNullable<TCloaked, TCloakedBase>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value) where TBearer : class, IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAllNullable(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeqNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllNullable(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt  WhenNonNullAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields &&  value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TCloaked, TCloakedBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TBearer>(string fieldName, TBearer?[]? value)
        where TBearer : IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value)
        where TBearerStruct : struct, IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllCharSeq<TCharSeq>
    (string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt  WhenNonNullAddAllMatch<TAny>
    (string fieldName, TAny?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        WhenNonNullAddAllMatch(fieldName, value, formatString);

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TCloaked, TCloakedBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value)
        where TBearer : IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllCharSeq<TCharSeq>
    (string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt  WhenNonNullAddAllMatch<TAny>
    (string fieldName, IReadOnlyList<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => WhenNonNullAddAllMatch(fieldName, value, formatString);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TCloaked>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TCloaked> palantírReveal) where TCloaked : struct =>
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value)
        where TBearer : IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value)
        where TBearer : struct, IStringBearer  => 
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt  WhenNonNullAddAllMatchEnumerate<TAny>
    (string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value)
        where TBearer : IStringBearer => 
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer => 
        !stb.SkipFields && value != null ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq :  ICharSequence =>
        !stb.SkipFields && value != null ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt  WhenNonNullAddAllMatchEnumerate<T>
    (string fieldName, IEnumerator<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString);
}
