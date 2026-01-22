// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAdd (ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(bool), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.WasSkipped(typeof(bool), fieldName, formatFlags);

    public TMold WhenNonNullAdd<TFmt> (ReadOnlySpan<char> fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags);

    public TMold WhenNonNullAdd<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.HasSkipField(typeof(TFmtStruct?), fieldName, formatFlags) 
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(TFmtStruct?), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value, PalantírReveal<TRevealBase> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase? 
        where TRevealBase : notnull =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags)
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags);


    public TMold WhenNonNullReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.HasSkipField(typeof(TCloakedStruct?), fieldName, formatFlags) 
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags) 
            : stb.WasSkipped(typeof(TCloakedStruct?), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags)
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.HasSkipField(typeof(TBearerStruct?), fieldName, formatFlags) 
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(TBearerStruct?), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(Span<char>), fieldName, formatFlags) && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(Span<char>), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(ReadOnlySpan<char>), fieldName, formatFlags)
     && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(ReadOnlySpan<char>), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(string), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(string), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(char[]), fieldName, formatFlags) 
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(char[]), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags) && value != null 
            ? AlwaysAddCharSeq(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags) && value != null 
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);
    
    public TMold WhenNonNullAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TAny), fieldName, formatFlags) && value != null 
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TAny), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TMold WhenNonNullAddObject(ReadOnlySpan<char> fieldName, object? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(value?.GetType() ?? typeof(object), fieldName, formatFlags) && value != null 
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(object), fieldName, formatFlags);
}
