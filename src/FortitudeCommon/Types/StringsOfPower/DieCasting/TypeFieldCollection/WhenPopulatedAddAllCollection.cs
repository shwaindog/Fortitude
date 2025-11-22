// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<bool>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<bool?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TFmt>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TFmtStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<Memory<TCloaked>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<Memory<TCloakedStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<Memory<TBearer>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<Memory<TBearerStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<string>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<string?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TCharSeq>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<StringBuilder?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<StringBuilder?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TAny>>(null, fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<bool>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<bool?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TFmt>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TFmtStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<Memory<TCloaked>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<Memory<TCloakedStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<Memory<TBearer>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<Memory<TBearerStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<string>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<string?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TCharSeq>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<StringBuilder>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<StringBuilder?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<TAny>>(null, fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<bool[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<bool?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TFmt?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TFmtStruct?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<TCloaked?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<TCloakedStruct?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, TBearer?[]? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<TBearer?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<TBearerStruct?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<string?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TCharSeq?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<StringBuilder?[]>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TAny[]>(null, fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<bool>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<bool?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TFmt>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TFmtStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TCloaked?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TCloakedStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TBearer?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TBearerStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<string?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value is { Count: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TCharSeq?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<StringBuilder?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<TAny>>(null, fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<bool>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<bool?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<TFmt?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<TFmtStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<IEnumerable<TCloaked?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatFlags)
            : stb.WasSkipped<IEnumerable<TCloakedStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, formatFlags)
            : stb.WasSkipped<IEnumerable<TBearer?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, formatFlags)
            : stb.WasSkipped<IEnumerable<TBearerStruct?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<string?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        (value?.Any() ?? false)
            ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<TCharSeq?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<StringBuilder?>>(null, fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<IEnumerable<TAny?>>(null, fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<bool>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<bool>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool>, bool>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<bool?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<bool?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool?>, bool?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipField<IEnumerator<TFmt?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TFmt?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipField<IEnumerator<TFmtStruct?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TFmtStruct?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<TCloaked?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TCloaked?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipField<IEnumerator<TCloakedStruct?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TCloakedStruct?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (stb.SkipField<IEnumerator<TBearer?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TBearer?>>(null, fieldName, formatFlags);

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

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipField<IEnumerator<TBearerStruct?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TBearerStruct?>>(null, fieldName, formatFlags);

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
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<string?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<string?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<string>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence
    {
        if (stb.SkipField<IEnumerator<TCharSeq?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TCharSeq?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?>(value!);
            while (hasValue)
            {
                eoctb.AddCharSequenceElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<StringBuilder?>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<StringBuilder?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<StringBuilder>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<TAny>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<TAny?>>(null, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TAny>(value!);
            while (hasValue)
            {
                eoctb.AddMatchElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);
}
