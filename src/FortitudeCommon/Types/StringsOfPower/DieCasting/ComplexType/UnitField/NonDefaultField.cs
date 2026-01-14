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
        !stb.HasSkipField<bool>(typeof(bool), fieldName, formatFlags) && value != defaultValue
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.WasSkipped<bool>(typeof(bool), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.HasSkipField<TFmt>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TFmt))
            ? AlwaysAdd(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped<TFmt?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.HasSkipField<TFmtStruct>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TFmtStruct))
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TFmtStruct?>(value?.GetType(), fieldName, formatFlags);


    public TMold WhenNonDefaultReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, TCloaked defaultValue = default(TCloaked)!
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        !stb.HasSkipField<TCloaked?>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value!, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped<TCloaked?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.HasSkipField<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue ?? default(TCloakedStruct))
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value, TBearer defaultValue = default(TBearer)!
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        !stb.HasSkipField<TBearer?>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped<TBearer?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.HasSkipField<TBearerStruct?>(value?.GetType(), fieldName, formatFlags) && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<TBearerStruct?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, fieldName, formatFlags) && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags) 
     && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<string>(value?.GetType(), fieldName, formatFlags) && (value == null! || value != defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.HasSkipField<string>(value?.GetType(), fieldName, formatFlags)
            && (value == null!
             || (cappedLength == 0 && defaultValue.Length > 0)
             || (cappedStart < value.Length
              && !(((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue))))
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
            : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<string>(value?.GetType(), fieldName, formatFlags) && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<string>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.HasSkipField<char[]>(value?.GetType(), fieldName, formatFlags)
            && (value == null!
             || (cappedLength == 0 && defaultValue.Length > 0)
             || (cappedStart < value.Length
              && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)))
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
            : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.HasSkipField<char[]>(value?.GetType(), fieldName, formatFlags)
     && value == null! || !(value?.SequenceMatches(defaultValue) ?? true)
            ? AlwaysAddCharSeq(fieldName, value!, formatString, formatFlags)
            : stb.WasSkipped<char[]>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.HasSkipField<TCharSeq?>(value?.GetType(), fieldName, formatFlags)
            && value == null!
            || (cappedLength == 0 && defaultValue.Length > 0)
            || (cappedStart < value!.Length
             && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
            ? AlwaysAddCharSeq(fieldName, value!, startIndex, count, formatString, formatFlags)
            : stb.WasSkipped<TCharSeq?>(value.GetType(), fieldName, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags)
     && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<StringBuilder?>(value?.GetType(), fieldName, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.HasSkipField<StringBuilder?>(value?.GetType(), fieldName, formatFlags)
            && value == null!
            || (cappedLength == 0 && defaultValue.Length > 0)
            || (cappedStart < value!.Length
             && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.WasSkipped<StringBuilder?>(value.GetType(), fieldName, formatFlags);
    }


    public TMold WhenNonDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default(TAny)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipField<TAny?>(value?.GetType(), fieldName, formatFlags)) 
          return stb.WasSkipped<TAny?>(value?.GetType(), fieldName, formatFlags);

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
            : stb.WasSkipped<TAny?>(value?.GetType(), fieldName, formatFlags);
    }

    [CallsObjectToString]
    public TMold WhenNonDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.HasSkipField<object?>(value?.GetType(), fieldName, formatFlags)
     && ((value != null || defaultValue != null)
      && (value ?? defaultValue!).GetType().IsValueType
      && !Equals(value, defaultValue ?? (value ?? defaultValue!).GetType().GetDefaultForUnderlyingNullableOrThis()))
     || ((value != null || defaultValue != null)
      && (!(value ?? defaultValue!).GetType().IsValueType
       && (!Equals(value, defaultValue)
        && !(defaultValue != null && value != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
          && value.UnknownSequenceMatches(defaultValue)))))
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped<object?>(value?.GetType(), fieldName, formatFlags);
}
