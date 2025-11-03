// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAdd (ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd<TFmt> (ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags) 
            : stb.StyleTypeBuilder;


    public TMold WhenNonNullReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags) 
            : stb.StyleTypeBuilder;
    
    public TMold WhenNonNullAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TMold WhenNonNullAddObject(ReadOnlySpan<char> fieldName, object? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags) 
            : stb.StyleTypeBuilder;
}
