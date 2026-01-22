// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 }
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 }
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TBearerBase? =>
        value is { Length: > 0 }
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 }
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        value is { Length: > 0 }
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 }
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        value is { Length: > 0 }
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase? =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TFilterBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TFilterBase? =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        value != null
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        value != null
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);


    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence, TCharSeqBase =>
        value != null
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>
    (string fieldName, TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase =>
        value != null
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmt, TFmtBase>
    (string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, IReadOnlyList<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value != null
            ? AlwaysRevealFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatch<TAny, TAnyBase>
    (string fieldName, IReadOnlyList<TAny>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        value != null
            ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IReadOnlyList<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TFmt, TFmtBase>
    (string fieldName, IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmt?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloaked?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TCloakedStruct>(string fieldName, IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName
      , IEnumerable<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TBearerBase? =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearer?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>
    (string fieldName, IEnumerable<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredCharSeqEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatchEnumerate<TAny, TAnyBase>
    (string fieldName, IEnumerable<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerable<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectEnumerate
    (string fieldName, IEnumerable<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TFmt, TFmtBase>
    (string fieldName, IEnumerator<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable, TFmtBase =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmt?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloaked?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TCloakedStruct>(string fieldName, IEnumerator<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCloakedStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName
      , IEnumerator<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearer?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealFilteredEnumerate<TBearerStruct>(string fieldName
      , IEnumerator<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value != null
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TBearerStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>
    (string fieldName, IEnumerator<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredCharSeqEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TCharSeq?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredMatchEnumerate<TAny, TAnyBase>
    (string fieldName, IEnumerator<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(IEnumerator<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectEnumerate
    (string fieldName, IEnumerator<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString, formatFlags);
}
