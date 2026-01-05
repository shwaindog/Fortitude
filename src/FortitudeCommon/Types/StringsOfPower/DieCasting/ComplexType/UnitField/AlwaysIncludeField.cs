// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<bool>(typeof(bool), fieldName, formatFlags) 
        ? stb.AppendBooleanField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<bool>(typeof(bool), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<bool?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.AppendNullableBooleanField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<bool?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
      !stb.SkipField<TFmt?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.AppendFormattableField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<TFmt?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
      !stb.SkipField<TFmtStruct?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.AppendFormattableField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<TFmtStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull =>
        !stb.SkipField<TCloaked?>(value?.GetType(), fieldName, formatFlags) 
          ? stb.RevealCloakedBearerField(fieldName, value, palantírReveal, formatString, formatFlags).AddGoToNext()
          : stb.WasSkipped<TCloaked?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TCloakedStruct : struct =>
      !stb.SkipField<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.RevealNullableCloakedBearerField(fieldName, value, palantírReveal, formatString, formatFlags).AddGoToNext()
        : stb.WasSkipped<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
      !stb.SkipField<TBearer?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.RevealStringBearerField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<TBearer?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
      !stb.SkipField<TBearerStruct?>(value?.GetType(), fieldName, formatFlags) 
        ? stb.RevealNullableStringBearerField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<TBearerStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, fieldName, formatFlags) 
        ? stb.AppendReadOnlySpanField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<Memory<char>>(value.Length > 0 ?typeof(Span<char>) : null, fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags) 
        ? stb.AppendReadOnlySpanField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<string>(value?.GetType(), fieldName, formatFlags) 
        ? stb.AppendStringField(fieldName, value,  formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<string>(value?.GetType(), fieldName, formatFlags) 
        ? stb.AppendStringField(fieldName, value, formatString ?? "", startIndex, length, formatFlags)
             .AddGoToNext()
        : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<char[]>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendCharArrayField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<char[]>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendCharArrayField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
      !stb.SkipField<TCharSeq>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendCharSequenceField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<TCharSeq>(value?.GetType(), fieldName, formatFlags);
    
    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
      !stb.SkipField<TCharSeq?>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendCharSequenceField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped<TCharSeq?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendStringBuilderField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped<StringBuilder?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendStringBuilderField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped<StringBuilder?>(value?.GetType(), fieldName, formatFlags);

    public TMold AlwaysAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<TAny?>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendMatchField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<TAny?>(value?.GetType(), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold AlwaysAddObject(ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipField<object?>(value?.GetType(), fieldName, formatFlags)
        ? stb.AppendObjectField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped<object?>(value?.GetType(), fieldName, formatFlags);
}
