// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAdd (ReadOnlySpan<char> fieldName, bool? value) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TFmt> (ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs<TFmt> (ReadOnlySpan<char> fieldName, TFmt? value
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) 
        where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) 
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, Span<char> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, Span<char> value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value.Length > 0 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, string? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, char[]? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeqAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, StringBuilder? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddMatch(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddObject(ReadOnlySpan<char> fieldName, object? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddObject(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;
}
