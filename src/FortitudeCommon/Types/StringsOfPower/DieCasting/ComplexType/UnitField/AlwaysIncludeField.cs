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
      !stb.HasSkipField(typeof(bool), fieldName, formatFlags) 
        ? stb.AppendBooleanField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(bool), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(bool?), fieldName, formatFlags) 
        ? stb.AppendNullableBooleanField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(bool?), fieldName, formatFlags);

    public TMold AlwaysAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
      !stb.HasSkipField(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags) 
        ? stb.AppendFormattableField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags);

    public TMold AlwaysAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
      !stb.HasSkipField(typeof(TFmtStruct?), fieldName, formatFlags) 
        ? stb.AppendFormattableField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(TFmtStruct?), fieldName, formatFlags);

    public TMold AlwaysReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags) 
          ? stb.RevealCloakedBearerField(fieldName, value, palantírReveal, formatString, formatFlags).AddGoToNext()
          : stb.WasSkipped(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags);

    public TMold AlwaysReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TCloakedStruct : struct =>
      !stb.HasSkipField(typeof(TCloakedStruct?), fieldName, formatFlags) 
        ? stb.RevealNullableCloakedBearerField(fieldName, value, palantírReveal, formatString, formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(TCloakedStruct?), fieldName, formatFlags);

    public TMold AlwaysReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
      !stb.HasSkipField(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags) 
        ? stb.RevealStringBearerField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags);

    public TMold AlwaysReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
      !stb.HasSkipField(typeof(TBearerStruct?), fieldName, formatFlags) 
        ? stb.RevealNullableStringBearerField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(TBearerStruct?), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(Span<char>), fieldName, formatFlags) 
        ? stb.AppendReadOnlySpanField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(Span<char>), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(ReadOnlySpan<char>), fieldName, formatFlags) 
        ? stb.AppendReadOnlySpanField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(ReadOnlySpan<char>), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(string), fieldName, formatFlags) 
        ? stb.AppendStringField(fieldName, value,  formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(string), fieldName, formatFlags) 
        ? stb.AppendStringField(fieldName, value, formatString ?? "", startIndex, length, formatFlags)
             .AddGoToNext()
        : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(char[]), fieldName, formatFlags)
        ? stb.AppendCharArrayField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(char[]), fieldName, formatFlags)
        ? stb.AppendCharArrayField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
      !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags)
        ? stb.AppendCharSequenceField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);
    
    public TMold AlwaysAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
      !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags)
        ? stb.AppendCharSequenceField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags)
        ? stb.AppendStringBuilderField(fieldName, value, formatString ?? "", formatFlags: formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);

    public TMold AlwaysAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags)
        ? stb.AppendStringBuilderField(fieldName, value, formatString ?? "", startIndex, length, formatFlags).AddGoToNext()
        : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);

    public TMold AlwaysAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(value?.GetType() ?? typeof(TAny), fieldName, formatFlags)
        ? stb.AppendMatchField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(TAny), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold AlwaysAddObject(ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.HasSkipField(value?.GetType() ?? typeof(object), fieldName, formatFlags)
        ? stb.AppendObjectField(fieldName, value, formatString ?? "", formatFlags).AddGoToNext()
        : stb.WasSkipped(value?.GetType() ?? typeof(object), fieldName, formatFlags);
}
