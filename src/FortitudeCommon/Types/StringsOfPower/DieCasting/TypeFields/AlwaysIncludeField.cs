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
    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendFormatted(value, formatString ?? "")
               .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendFormattedOrNull(value, formatString ?? "")
               .AddGoToNext();

    public TMold AlwaysAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormatted(value, formatString ?? "", formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendNullableFormattedOrNull(value, formatString ?? "", formatFlags)
                 .AddGoToNext();

    public TMold AlwaysReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendOrNull(value, palantírReveal, formatFlags)
               .AddGoToNext();

    public TMold AlwaysReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendOrNull(value, palantírReveal, formatFlags)
               .AddGoToNext();

    public TMold AlwaysReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendRevealBearerOrNull(value, formatFlags)
               .AddGoToNext();

    public TMold AlwaysReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendRevealBearerOrNull(value, formatFlags)
               .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNullOnZeroLength(value, formatString ?? "", formatFlags: formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNullOnZeroLength(value, formatString ?? "", formatFlags: formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", formatFlags: formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", startIndex, length, formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", formatFlags: formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", startIndex, length, formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", formatFlags: formatFlags)
                 .AddGoToNext();
    
    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", startIndex, length, formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields 
          ? stb.StyleTypeBuilder 
          : stb.FieldNameJoin(fieldName)
               .AppendFormattedOrNull(value, formatString ?? "", formatFlags: formatFlags)
               .AddGoToNext();

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendFormattedOrNull(value, formatString ?? "", startIndex, length, formatFlags)
                 .AddGoToNext();

    public TMold AlwaysAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendMatchFormattedOrNull(value, formatString ?? "", formatFlags)
                 .AddGoToNext();

    [CallsObjectToString]
    public TMold AlwaysAddObject(ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields
            ? stb.StyleTypeBuilder
            : stb.FieldNameJoin(fieldName)
                 .AppendMatchFormattedOrNull(value, formatString ?? "", formatFlags)
                 .AddGoToNext();
}
