// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
        )
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, TBearer?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value is { Count: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmt?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloaked?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearer?>), fieldName, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        (value?.Any() ?? false)
            ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<string?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        (value?.Any() ?? false)
            ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eocm = stb.Master.StartExplicitCollectionType<IEnumerator<bool>, bool>(value!);
            while (hasValue)
            {
                count++;
                eocm.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eocm.TotalCount = count; 
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool?>, bool?>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName
      , IEnumerator<TBearer?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearer>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddBearerElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName
      , IEnumerator<TBearerStruct?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearerStruct>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddBearerElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<string>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddCharSequenceElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<StringBuilder>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);

        var hasValue = value?.MoveNext() ?? false;
        var count    = 0;
        if (hasValue)
        {
            formatString ??= "";
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TAny>(value!);
            while (hasValue)
            {
                count++;
                eoctb.AddMatchElementAndGoToNextElement(value!.Current, formatString, formatFlags);
                hasValue = value.MoveNext();
            }
            eoctb.TotalCount = count; 
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext(true);
        }
        return stb.Mold;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);
}
