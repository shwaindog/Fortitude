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
    public TExt WhenNonDefaultAdd(string fieldName, bool value, bool defaultValue = false) =>
        !stb.SkipFields && value != defaultValue
            ? AlwaysAdd(fieldName, value)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmt>(string fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs<TFmt>(string fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmtStruct>(string fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs<TFmtStruct>(string fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TCloaked, TCloakedBase>(string fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, palantírReveal)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TCloaked, TCloakedBase>(string fieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , TCloaked? defaultValue = default(TCloaked), FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TCloakedStruct>(string fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null)
        where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, palantírReveal)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TCloakedStruct>(string fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TBearer>(string fieldName, TBearer? value, TBearer? defaultValue = default(TBearer)) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TBearer>(string fieldName, TBearer? value, TBearer? defaultValue = default(TBearer)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TBearerStruct>(string fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TBearerStruct>(string fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && value != defaultValue
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, string value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && value != defaultValue
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, string value, int startIndex, int length = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != defaultValue
            ? AlwaysAdd(fieldName, value, startIndex, length, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, string value, int startIndex, int length = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != defaultValue
            ? AlwaysAddAs(fieldName, value, startIndex, length, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, char[] value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && !value.SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, char[] value, int startIndex, int length = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, startIndex, length, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, char[] value, int startIndex, int length = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null! && !value.SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, startIndex, length, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeq<TCharSeq>(string fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddCharSeq(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeq<TCharSeq>(string fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddCharSeqAs(fieldName, value, startIndex, count, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, StringBuilder value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(string fieldName, StringBuilder value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(string fieldName, StringBuilder value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddMatch<T>(string fieldName, T? value, T? defaultValue = default(T)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value == null! || value.Equals(defaultValue))
            ? AlwaysAddMatch(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonDefaultAddObject(string fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true)
            ? AlwaysAddObject(fieldName, value, formatString)
            : stb.StyleTypeBuilder;
}
