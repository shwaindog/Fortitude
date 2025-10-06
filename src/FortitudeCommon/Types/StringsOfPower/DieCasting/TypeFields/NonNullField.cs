// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAdd (string fieldName, bool? value) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TFmt> (string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs<TFmt> (string fieldName, TFmt? value
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd<TFmtStruct> (string fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs<TFmtStruct> (string fieldName, TFmtStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TCloaked, TCloakedBase>(string fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TCloaked, TCloakedBase>(string fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TCloakedStruct>(string fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value, palantírReveal) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TCloakedStruct>(string fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TBearer>(string fieldName, TBearer? value) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TBearer>(string fieldName, TBearer? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) 
        where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullReveal<TBearerStruct>(string fieldName, TBearerStruct? value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysReveal(fieldName, value) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealAs<TBearerStruct>(string fieldName, TBearerStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) 
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, null) 
            ? AlwaysRevealAs(fieldName, value, flags) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, string? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, string? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, char[]? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null 
            ? AlwaysAddCharSeqAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, StringBuilder? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, StringBuilder? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, flags, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAdd(string fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAs(string fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null 
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString) 
            : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddMatch<T>(string fieldName, T? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddMatch(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddObject(string fieldName, object? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null 
            ? AlwaysAddObject(fieldName, value, formatString) 
            : stb.StyleTypeBuilder;
}
