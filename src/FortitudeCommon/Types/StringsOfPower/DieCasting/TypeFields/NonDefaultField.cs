// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != defaultValue
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmt))
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmt))
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmtStruct))
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmtStruct))
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, palantírReveal)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal
      , TCloaked? defaultValue = default(TCloaked), FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , TCloakedStruct? defaultValue = null)
        where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, palantírReveal)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, palantírReveal, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, TBearer? defaultValue = default(TBearer))
        where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, TBearer? defaultValue = default(TBearer)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultRevealAs<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysRevealAs(fieldName, value, flags)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value == null! || value != defaultValue)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value == null! || value != defaultValue)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && (value == null! || value[cappedStart..cappedEnd] != defaultValue)
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && (value == null! || value[cappedStart..cappedEnd] != defaultValue)
            ? AlwaysAddAs(fieldName, value, cappedStart, count, flags, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && (value == null! || !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && value == null! || !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)
            ? AlwaysAddAs(fieldName, value, cappedStart, count, flags, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value == null! || !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeq(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value == null! || !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeqAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        return !stb.SkipFields && value == null! || !value!.SequenceMatches(defaultValue, cappedStart, cappedLength)
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        return !stb.SkipFields && value == null! || !value!.SequenceMatches(defaultValue, cappedStart, cappedLength)
            ? AlwaysAddCharSeqAs(fieldName, value, startIndex, count, flags, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value?.Equals(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && !(value?.Equals(defaultValue) ?? false)
            ? AlwaysAddAs(fieldName, value, flags, formatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        return !stb.SkipFields && value == null! || !value!.SequenceMatches(defaultValue, cappedStart, cappedLength)
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddAs(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, value?.Length ?? 0 - cappedStart);
        return !stb.SkipFields && value == null! || !value!.SequenceMatches(defaultValue, cappedStart, cappedLength)
            ? AlwaysAddAs(fieldName, value, startIndex, count, flags, formatString)
            : stb.StyleTypeBuilder;
    }

    public TExt WhenNonDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default(TAny)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields
     && (typeof(TAny).IsNullable() && value == null
      || typeof(TAny).IsNullable() && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis())
      || typeof(TAny).IsNotNullable() && !Equals(value, defaultValue ?? default(TAny)!))
            ? AlwaysAddMatch(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && ((value != null || defaultValue != null)
                         && (value ?? defaultValue!).GetType().IsValueType
                         && !Equals(value, defaultValue ?? (value ?? defaultValue!).GetType().GetDefaultForUnderlyingNullableOrThis()))
     || ((value != null || defaultValue != null)
      && !(value ?? defaultValue!).GetType().IsValueType
      && !Equals(value, defaultValue))
            ? AlwaysAddObject(fieldName, value, formatString)
            : stb.StyleTypeBuilder;
}
