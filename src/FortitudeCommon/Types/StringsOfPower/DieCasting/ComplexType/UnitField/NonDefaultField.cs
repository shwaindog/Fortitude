// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(bool), fieldName, formatFlags) && value != defaultValue
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.WasSkipped(typeof(bool), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TFmt))
            ? AlwaysAdd(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.HasSkipField(typeof(TFmtStruct?), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TFmtStruct))
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TFmtStruct?), fieldName, formatFlags);


    public TMold WhenNonDefaultReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, TCloaked defaultValue = default(TCloaked)!
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value!, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.HasSkipField(typeof(TCloakedStruct?), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TCloakedStruct))
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TCloakedStruct?), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value, TBearer defaultValue = default(TBearer)!
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.HasSkipField(typeof(TBearerStruct?), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TBearerStruct?), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(Span<char>), fieldName, formatFlags) && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(Span<char>), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(ReadOnlySpan<char>), fieldName, formatFlags) 
     && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(ReadOnlySpan<char>), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(string), fieldName, formatFlags) && (value == null! || value != defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.HasSkipField(typeof(string), fieldName, formatFlags)
            && (value == null!
             || (cappedLength == 0 && defaultValue.Length > 0)
             || (cappedStart < value.Length
              && !(((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue))))
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(char[]), fieldName, formatFlags) && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.HasSkipField(typeof(char[]), fieldName, formatFlags)
            && (value == null!
             || (cappedLength == 0 && defaultValue.Length > 0)
             || (cappedStart < value.Length
              && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)))
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags)
     && value == null! || !(value?.SequenceMatches(defaultValue) ?? true)
            ? AlwaysAddCharSeq(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.HasSkipField(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags)
            && value == null!
            || (cappedLength == 0 && defaultValue.Length > 0)
            || (cappedStart < value!.Length
             && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
            ? AlwaysAddCharSeq(fieldName, value!, startIndex, count, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags)
     && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.HasSkipField(typeof(StringBuilder), fieldName, formatFlags)
            && value == null!
            || (cappedLength == 0 && defaultValue.Length > 0)
            || (cappedStart < value!.Length
             && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.WasSkipped(value.GetType(), fieldName, formatFlags);
    }


    public TMold WhenNonDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default(TAny)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipField(value?.GetType() ?? typeof(TAny), fieldName, formatFlags)) 
          return stb.WasSkipped(value?.GetType() ?? typeof(TAny), fieldName, formatFlags);

        var anyIsNullable = typeof(TAny).IsNullable();
        var shouldProceed = (anyIsNullable && value == null && defaultValue != null);

        if (!shouldProceed) shouldProceed = anyIsNullable && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis());
        if (!shouldProceed)
            shouldProceed =
                !anyIsNullable
             && !Equals(value, defaultValue ?? default(TAny))
             && !(defaultValue != null && value != null && value.IsStringBuilder()
               && defaultValue.IsStringBuilder() && value.UnknownSequenceMatches(defaultValue));
        return shouldProceed
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(TAny), fieldName, formatFlags);
    }

    [CallsObjectToString]
    public TMold WhenNonDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField(value?.GetType() ?? typeof(object), fieldName, formatFlags)
     && ((value != null || defaultValue != null)
      && (value ?? defaultValue!).GetType().IsValueType
      && !Equals(value, defaultValue ?? (value ?? defaultValue!).GetType().GetDefaultForUnderlyingNullableOrThis()))
     || ((value != null || defaultValue != null)
      && (!(value ?? defaultValue!).GetType().IsValueType
       && (!Equals(value, defaultValue)
        && !(defaultValue != null && value != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
          && value.UnknownSequenceMatches(defaultValue)))))
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(object), fieldName, formatFlags);
}
