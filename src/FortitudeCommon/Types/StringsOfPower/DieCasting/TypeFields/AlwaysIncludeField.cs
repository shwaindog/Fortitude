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
    public TExt AlwaysAdd(string fieldName, bool value) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, bool? value) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd<TFmt>(string fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAddAs<TFmt>(string fieldName, TFmt? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAdd<TFmtStruct>(string fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendNullableFormattedOrNull(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysAddAs<TFmtStruct>(string fieldName, TFmtStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendNullableFormattedOrNull(value, formatString ?? "").AddGoToNext();

    public TExt AlwaysReveal<TCloaked, TCloakedBase>(string fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysRevealAs<TCloaked, TCloakedBase>(string fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloaked : TCloakedBase =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysReveal<TCloakedStruct>(string fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysRevealAs<TCloakedStruct>(string fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) where TCloakedStruct : struct =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value, palantírReveal).AddGoToNext();

    public TExt AlwaysReveal<TBearer>(string fieldName, TBearer? value) where TBearer : IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysRevealAs<TBearer>(string fieldName, TBearer? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TBearer : IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysReveal<TBearerStruct>(string fieldName, TBearerStruct? value) where TBearerStruct : struct, IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysRevealAs<TBearerStruct>(string fieldName, TBearerStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType)
        where TBearerStruct : struct, IStringBearer =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, ReadOnlySpan<char> value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNullOnZeroLength(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, string? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, string? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, char[]? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddCharSeqAs<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddCharSeq<TCharSeq>(string fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, StringBuilder? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields ? stb.StyleTypeBuilder : stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext();

    public TExt AlwaysAddAs(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString, startIndex, length).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext();

    public TExt AlwaysAddMatch<T>(string fieldName, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendMatchFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendMatchOrNull(value).AddGoToNext();

    [CallsObjectToString]
    public TExt AlwaysAddObject(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : formatString.IsNotNullOrEmpty()
                ? stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext()
                : stb.FieldNameJoin(fieldName).AppendMatchOrNull(value).AddGoToNext();
}
