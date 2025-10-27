// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullOrDefaultAdd
    (ReadOnlySpan<char> fieldName, bool? value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) 
          ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
      where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue ?? default(TFmtStruct)) 
          ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
          : stb.StyleTypeBuilder;
    
    public TMold WhenNonNullOrDefaultReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) 
          ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue ?? default(TCloakedStruct)) 
          ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, TBearer? defaultValue = default(TBearer?)
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) 
          ? AlwaysReveal(fieldName, value, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) 
          ? AlwaysReveal(fieldName, value, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields || value is { Length: 0 } || value.SequenceMatches(defaultValue)
            ? stb.StyleTypeBuilder
            : AlwaysAdd( fieldName, value, formatString ?? "", formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        stb.SkipFields || value is { Length: 0 } || value.SequenceMatches(defaultValue)
            ? stb.StyleTypeBuilder
            : AlwaysAdd(fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, string? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null && value != defaultValue 
          ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && value != null
                               && ((cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length
                                 && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, char[]? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value != null && !value.SequenceMatches(defaultValue))
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && value != null
                               && ((cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length
                                 && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonNullOrDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeq(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.SkipFields && value != null
                               && ((cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length
                                 && !value.SequenceMatches(defaultValue, cappedStart, cappedLength)))
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != null && !value.SequenceMatches(defaultValue) 
          ? AlwaysAdd(fieldName, value, formatString, formatFlags) 
          : stb.StyleTypeBuilder;

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.SkipFields && value != null
                               && ((cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length
                                 && !value.SequenceMatches(defaultValue, cappedStart, cappedLength)))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonNullOrDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields
     && value != null
     && (typeof(TAny).IsNullable() && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis())
      || typeof(TAny).IsNotNullable()
      && (!Equals(value, defaultValue ?? default(TAny))
       & !(defaultValue != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
        && value.UnknownSequenceMatches(defaultValue))))
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TMold WhenNonNullOrDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields
     && value != null
     && ((value.GetType().IsValueType
       && !Equals(value, defaultValue ?? value.GetType().GetDefaultForUnderlyingNullableOrThis()))
      || (!value.GetType().IsValueType &&
          (!Equals(value, defaultValue) 
        && !(defaultValue != null && value.IsStringBuilder() && defaultValue.IsStringBuilder() 
          && value.UnknownSequenceMatches(defaultValue)))))
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;
}
