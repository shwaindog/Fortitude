// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullOrDefaultAdd
        (string fieldName, bool? value, bool? defaultValue = false) =>
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd<TFmt>(string fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs<TFmt>(string fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAddAs(fieldName, value, flags, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd<TFmtStruct>(string fieldName, TFmtStruct? value, TFmtStruct defaultValue = default(TFmtStruct)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs<TFmtStruct>(string fieldName, TFmtStruct? value, TFmtStruct defaultValue = default(TFmtStruct)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAddAs(fieldName, value, flags, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultReveal<TCloaked, TCloakedBase>(string fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysReveal(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultRevealAs<TCloaked, TCloakedBase>(string fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , TCloaked? defaultValue = default(TCloaked), FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultReveal<TCloakedStruct>(string fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null) where TCloakedStruct : struct =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysReveal(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultRevealAs<TCloakedStruct>(string fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null, FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TCloakedStruct : struct =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysRevealAs(fieldName, value, palantírReveal, flags) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultReveal<TBearer>(string fieldName, TBearer? value, TBearer? defaultValue = default(TBearer?))
        where TBearer : IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysReveal(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultRevealAs<TBearer>(string fieldName, TBearer? value, TBearer? defaultValue = default(TBearer?)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TBearer : IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysRevealAs(fieldName, value, flags) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultReveal<TBearerStruct>(string fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysReveal(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultRevealAs<TBearerStruct>(string fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysRevealAs(fieldName, value, flags) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, string? value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAddAs(fieldName, value, flags, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, string? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } && !value.SequenceMatches(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, char[]? value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } && !value.SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType, 
      [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeq(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAddCharSeq(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, StringBuilder? value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue) ? AlwaysAddAs(fieldName, value, flags, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddAs(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue, startIndex, length)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddMatch<T>(string fieldName, T? value, T? defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && value.Equals(defaultValue) ? AlwaysAddMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullOrDefaultAddObject(string fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && value.Equals(defaultValue) ? AlwaysAddObject(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
