// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullOrDefaultAdd
    (ReadOnlySpan<char> fieldName, bool? value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null && value != defaultValue, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, TFmt defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        WhenConditionMetAdd(value != null && !Equals(value, defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAdd(value != null && !Equals(value, defaultValue ?? default(TFmtStruct)), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, TCloaked defaultValue = default(TCloaked)!
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull =>
        WhenConditionMetReveal(value != null && !Equals(value, defaultValue), fieldName, value, palantírReveal,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetReveal(value != null && !Equals(value, defaultValue ?? default(TCloakedStruct)), fieldName, value
                             , palantírReveal,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value, TBearer defaultValue = default!
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer? =>
        WhenConditionMetReveal(value != null && !Equals(value, defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetReveal(value != null && !Equals(value, defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value.Length > 0 && !value.SequenceMatches(defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value.Length > 0 && !value.SequenceMatches(defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, string? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null && value != defaultValue, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        var shouldProceed =
            value != null
         && ((cappedLength == 0 && defaultValue.Length > 0)
          || (cappedStart < value.Length
           && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)));

        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, char[]? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      WhenConditionMetAdd((value != null && !value.SequenceMatches(defaultValue)), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        var shouldProceed =
          value != null
            && ((cappedLength == 0 && defaultValue.Length > 0)
             || (cappedStart < value.Length
              && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)));

        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count,  formatString, formatFlags);
    }

    public TMold WhenNonNullOrDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
      WhenConditionMetAddCharSeq(value != null && !value.SequenceMatches(defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var shouldProceed =
          value != null
       && ((cappedLength == 0 && defaultValue.Length > 0)
        || (cappedStart < value.Length
         && !value.SequenceMatches(defaultValue, cappedStart, cappedLength)));

        return WhenConditionMetAddCharSeq(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      WhenConditionMetAdd(value != null && !value.SequenceMatches(defaultValue), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullOrDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var shouldProceed =
          value != null
       && ((cappedLength == 0 && defaultValue.Length > 0)
        || (cappedStart < value.Length
         && !value.SequenceMatches(defaultValue, cappedStart, cappedLength)));

        return WhenConditionMetAdd(shouldProceed, fieldName, value, cappedStart, count, formatString, formatFlags);
    }

    public TMold WhenNonNullOrDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny defaultValue = default!
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
      var shouldProceed =
        value != null
          && (typeof(TAny).IsNullable() && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis())
           || typeof(TAny).IsNotNullable()
           && (!Equals(value, defaultValue ?? default(TAny))
             & !(defaultValue != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
              && value.UnknownSequenceMatches(defaultValue))));

      return WhenConditionMetAddMatch(shouldProceed, fieldName, value,  formatString, formatFlags);
    }

    [CallsObjectToString]
    public TMold WhenNonNullOrDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
      var shouldProceed =
        value != null
     && ((value.GetType().IsValueType
       && !Equals(value, defaultValue ?? value.GetType().GetDefaultForUnderlyingNullableOrThis()))
      || (!value.GetType().IsValueType &&
          (!Equals(value, defaultValue)
        && !(defaultValue != null && value.IsStringBuilder() && defaultValue.IsStringBuilder()
          && value.UnknownSequenceMatches(defaultValue)))));
      
      return WhenConditionMetAddObject(shouldProceed, fieldName, value,  formatString, formatFlags);
    }
}
