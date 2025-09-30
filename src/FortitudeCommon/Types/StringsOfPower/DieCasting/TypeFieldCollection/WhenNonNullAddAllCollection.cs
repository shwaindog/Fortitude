// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt  WhenNonNullAddAll(string fieldName, bool[]? value) => !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, bool?[]? value) => !stb.SkipFields &&  value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

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
    
    public TExt  WhenNonNullAddAllMatch<T>
    (string fieldName, T?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        WhenNonNullAddAllMatch(fieldName, value, formatString);

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool>? value) => 
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAll(string fieldName, IReadOnlyList<bool?>? value) => 
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

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
    
    public TExt  WhenNonNullAddAllMatch<T>
    (string fieldName, IReadOnlyList<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => WhenNonNullAddAllMatch(fieldName, value, formatString);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool>? value) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) => 
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
    
    public TExt  WhenNonNullAddAllMatchEnumerate<T>
    (string fieldName, IEnumerable<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddAllMatchEnumerate(fieldName, value, formatString);

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool>? value) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt  WhenNonNullAddAllEnumerate(string fieldName, IEnumerator<bool?>? value) => 
        !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

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
