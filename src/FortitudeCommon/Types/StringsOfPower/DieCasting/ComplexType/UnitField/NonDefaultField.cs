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
        WhenConditionMetAdd(value != defaultValue, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        WhenConditionMetAdd(!Equals(value, defaultValue ?? default(TFmt)), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAdd(!Equals(value, defaultValue ?? default(TFmtStruct)), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, TCloaked defaultValue = default(TCloaked)!
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetReveal(!Equals(value, defaultValue), fieldName, value!, palantírReveal, formatString, formatFlags);

    public TMold WhenNonDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetReveal(!Equals(value, defaultValue ?? default(TCloakedStruct)), fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value, TBearer defaultValue = default(TBearer)!
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        WhenConditionMetReveal(!Equals(value, defaultValue), fieldName, value!, formatString, formatFlags);

    public TMold WhenNonDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetReveal(!Equals(value, defaultValue), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(!value.SequenceMatches(defaultValue), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(!value.SequenceMatches(defaultValue), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value == null! || value != defaultValue, fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        var shouldProceed =
            (value == null!
          || (cappedLength == 0 && defaultValue.Length > 0)
          || (cappedStart < value.Length
           && !(((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue))));
        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(!(value?.SequenceMatches(defaultValue) ?? false), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        var shouldProceed =
            (value == null!
          || (cappedLength == 0 && defaultValue.Length > 0)
          || (cappedStart < value.Length
           && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)));
        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        WhenConditionMetAddCharSeq(value == null || !(value.SequenceMatches(defaultValue)), fieldName, value!, formatString, formatFlags);

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var shouldProceed =
            value == null!
         || (cappedLength == 0 && defaultValue.Length > 0)
         || (cappedStart < value!.Length
          && !value.SequenceMatches(defaultValue, cappedStart, cappedLength));

        return WhenConditionMetAddCharSeq(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(!(value?.SequenceMatches(defaultValue) ?? false), fieldName, value, formatString, formatFlags);

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var shouldProceed =
            value == null!
         || (cappedLength == 0 && defaultValue.Length > 0)
         || (cappedStart < value!.Length
          && !value.SequenceMatches(defaultValue, cappedStart, cappedLength));

        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }


    public TMold WhenNonDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default(TAny)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var anyIsNullable = typeof(TAny).IsNullable();
        var shouldProceed = (anyIsNullable && value == null && defaultValue != null);

        if (!shouldProceed) shouldProceed = anyIsNullable && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis());
        if (!shouldProceed)
            shouldProceed =
                !anyIsNullable
             && !Equals(value, defaultValue ?? default(TAny))
             && !(defaultValue != null && value != null && value.IsStringBuilder()
               && defaultValue.IsStringBuilder() && value.UnknownSequenceMatches(defaultValue));

        return WhenConditionMetAddMatch(shouldProceed, fieldName, value, formatString, formatFlags);
    }

    [CallsObjectToString]
    public TMold WhenNonDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var shouldProceed =
            ((value != null || defaultValue != null)
          && (value ?? defaultValue!).GetType().IsValueType
          && !Equals(value, defaultValue ?? (value ?? defaultValue!).GetType().GetDefaultForUnderlyingNullableOrThis()))
         || ((value != null || defaultValue != null)
          && (!(value ?? defaultValue!).GetType().IsValueType
           && (!Equals(value, defaultValue)
            && !(defaultValue != null && value != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
              && value.UnknownSequenceMatches(defaultValue)))));

        return WhenConditionMetAddObject(shouldProceed, fieldName, value, formatString, formatFlags);
    }
}
