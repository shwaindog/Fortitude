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
    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext();

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAddAs<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendNullableFormattedOrNull(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAddAs<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendNullableFormattedOrNull(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysRevealAs<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysRevealAs<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value) where TBearer : IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendRevealBearerOrNull(value).AddGoToNext();

    public TExt AlwaysRevealAs<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TBearer : IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendRevealBearerOrNull(value).AddGoToNext();

    public TExt AlwaysReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value) where TBearerStruct : struct, IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendRevealBearerOrNull(value).AddGoToNext();

    public TExt AlwaysRevealAs<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TBearerStruct : struct, IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendRevealBearerOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, Span<char> value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, string? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, string? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, char[]? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddCharSeqAs<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, StringBuilder? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddAs(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : (formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext());

    public TExt AlwaysAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendMatchFormattedOrNull(value, formatString ?? "").AddGoToNext();

    [CallsObjectToString]
    public TExt AlwaysAddObject(ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendMatchFormattedOrNull(value, formatString ?? "").AddGoToNext();
}
