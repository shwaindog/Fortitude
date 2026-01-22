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
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(Span<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(Span<TFmt>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(Span<TFmtStruct?>), fieldName, formatFlags);
    
    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
        (ReadOnlySpan<char> fieldName, Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
        , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
        , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TCloaked>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
        , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null 
        , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TCloakedStruct>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TBearer>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TBearerStruct>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<string>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 } 
          ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TCharSeq>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<StringBuilder>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<StringBuilder?>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)  
          : stb.WasSkipped(typeof(Span<TAny>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllMatchNull<TAny>(ReadOnlySpan<char> fieldName, Span<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(Span<object?>), fieldName, formatFlags);
    
    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<bool>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)  
          : stb.WasSkipped(typeof(ReadOnlySpan<bool?>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TFmt>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TFmtStruct?>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TCloaked, TRevealBase>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
        , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
        , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TCloaked>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TCloakedStruct>
        (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
        , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
         value is { Length: > 0 } 
           ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
           : stb.WasSkipped(typeof(ReadOnlySpan<TCloakedStruct?>), fieldName, formatFlags);

    public TExt            WhenNonNullRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TBearer>), fieldName, formatFlags);

    public TExt WhenNonNullRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        value is { Length: > 0 } 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TBearerStruct>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<string>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<string?>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        value is { Length: > 0 } 
          ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TCharSeq>), fieldName, formatFlags);

    public TExt WhenNonNullAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllNullable(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<StringBuilder>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TAny>), fieldName, formatFlags);

    public TExt WhenNonNullAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<TAny?>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<object>), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value is { Length: > 0 } 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(ReadOnlySpan<object?>), fieldName, formatFlags);
    
    public TExt  WhenNonNullAddAll(string fieldName, bool[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(bool[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll(string fieldName, bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(bool?[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(TFmt?[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(TFmtStruct?[]), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TCloaked, TRevealBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
         , string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value != null 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(TCloaked?[]), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(TCloakedStruct?[]), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TBearer>(string fieldName, TBearer?[]? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer  => 
        value != null 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(TBearer?[]), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer  => 
        value != null 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(TBearerStruct?[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(string?[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllCharSeq<TCharSeq>
    (string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value != null 
          ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(TCharSeq?[]), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(StringBuilder?[]), fieldName, formatFlags);
    
    public TExt  WhenNonNullAddAllMatch<TAny>
    (string fieldName, TAny[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(TAny[]), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        WhenNonNullAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IReadOnlyList<bool>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<bool?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll<TFmt>
    (string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TFmt>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TFmtStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TCloaked, TRevealBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
        , string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value != null 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TCloaked?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null 
          ? AlwaysRevealAll(fieldName, value, palantírReveal, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TCloakedStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer  => 
        value != null 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TBearer?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer  => 
        value != null 
          ? AlwaysRevealAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TBearerStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IReadOnlyList<string?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllCharSeq<TCharSeq>
    (string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value != null 
          ? AlwaysAddAllCharSeq(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TCharSeq?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAll
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAll(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<StringBuilder?>), fieldName, formatFlags);
    
    public TExt  WhenNonNullAddAllMatch<TAny>
    (string fieldName, IReadOnlyList<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value != null 
          ? AlwaysAddAllMatch(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IReadOnlyList<TAny?>), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
      WhenNonNullAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<bool>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<bool?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerable<TFmt?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerable<TFmtStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TCloaked, TRevealBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<TCloaked?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<TCloakedStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer  => 
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<TBearer?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer  => 
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<TBearerStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<string?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        value != null 
          ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerable<TCharSeq?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerable<StringBuilder?>), fieldName, formatFlags);
    
    public TExt  WhenNonNullAddAllMatchEnumerate<TAny>
    (string fieldName, IEnumerable<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerable<TAny?>), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerator<bool>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<bool?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate<TFmt>
    (string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TFmt?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
          : stb.WasSkipped(typeof(IEnumerator<TFmtStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TCloaked, TRevealBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TCloaked?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TCloakedStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer => 
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TBearer?>), fieldName, formatFlags);

    public TExt  WhenNonNullRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer => 
        value != null 
          ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TBearerStruct?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<string?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllCharSeqEnumerate<TCharSeq>
    (string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq :  ICharSequence =>
        value != null 
          ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TCharSeq?>), fieldName, formatFlags);

    public TExt  WhenNonNullAddAllEnumerate
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<StringBuilder?>), fieldName, formatFlags);
    
    public TExt  WhenNonNullAddAllMatchEnumerate<TAny>
    (string fieldName, IEnumerator<TAny?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        value != null 
          ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString, formatFlags) 
          : stb.WasSkipped(typeof(IEnumerator<TAny?>), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)  =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString, formatFlags);
}
