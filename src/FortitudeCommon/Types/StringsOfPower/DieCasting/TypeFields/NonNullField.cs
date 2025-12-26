// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAdd (ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) =>
        !stb.SkipField<bool>(typeof(bool), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.WasSkipped<bool>(typeof(bool), fieldName, formatFlags);

    public TMold WhenNonNullAdd<TFmt> (ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.SkipField<TFmt?>(value?.GetType(), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TFmt?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipField<TFmtStruct?>(value?.GetType(), fieldName, formatFlags) 
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TFmtStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase 
        where TRevealBase : notnull =>
        !stb.SkipField<TCloaked?>(value?.GetType(), fieldName, formatFlags)
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags) 
            : stb.WasSkipped<TCloaked?>(value?.GetType(), fieldName, formatFlags);


    public TMold WhenNonNullReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.SkipField<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags) 
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags) 
            : stb.WasSkipped<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        !stb.SkipField<TBearer?>(value?.GetType(), fieldName, formatFlags)
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TBearer?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipField<TBearerStruct?>(value?.GetType(), fieldName, formatFlags) 
     && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TBearerStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, fieldName, formatFlags) && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags)
     && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<string>(value?.GetType(), fieldName, formatFlags)
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<string>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<char[]>(value?.GetType(), fieldName, formatFlags) 
     && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<char[]>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipField<TCharSeq>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAddCharSeq(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TCharSeq>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipField<TCharSeq?>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped<TCharSeq?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        !stb.SkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<StringBuilder?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        !stb.SkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.WasSkipped<StringBuilder?>(value?.GetType(), fieldName, formatFlags);
    
    public TMold WhenNonNullAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<TAny?>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<TAny?>(value?.GetType(), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TMold WhenNonNullAddObject(ReadOnlySpan<char> fieldName, object? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipField<object?>(value?.GetType(), fieldName, formatFlags) && value != null 
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags) 
            : stb.WasSkipped<object?>(value?.GetType(), fieldName, formatFlags);
}
