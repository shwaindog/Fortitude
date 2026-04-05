// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloaked, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmt>(string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable? =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloaked, TRevealBase>(string fieldName, TCloaked[]? value, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearer>(string fieldName, TBearer[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCharSeq : ICharSequence? =>
        value is { Length: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllMatch<TAny>(string fieldName, TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable? =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TFmtStruct : struct, ISpanFormattable =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloaked, TRevealBase>(string fieldName, IReadOnlyList<TCloaked>? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TCloakedStruct>(string fieldName, IReadOnlyList<TCloakedStruct?>? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TMold WhenPopulatedRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        value is { Count: > 0 }
            ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCharSeq : ICharSequence? =>
        value is { Count: > 0 }
            ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TMold WhenPopulatedAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Count: > 0 }
            ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenPopulatedAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenPopulatedAddAllMatch(fieldName, value, formatString, formatFlags);
}
