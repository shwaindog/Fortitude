// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags) =>
        !stb.SkipFields && value != defaultValue
            ? AlwaysAdd(fieldName, value, formatString)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd<TFmt>(ReadOnlySpan<char> fieldName, TFmt? value, TFmt? defaultValue = default(TFmt)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmt))
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd<TFmtStruct>(ReadOnlySpan<char> fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TFmtStruct))
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;


    public TMold WhenNonDefaultReveal<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedBase =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        !stb.SkipFields && !Equals(value, defaultValue ?? default(TCloakedStruct))
            ? AlwaysReveal(fieldName, value, palantírReveal, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer? value, TBearer? defaultValue = default(TBearer)
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value, TBearerStruct? defaultValue = null
    , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && !Equals(value, defaultValue)
            ? AlwaysReveal(fieldName, value, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && !value.SequenceMatches(defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value == null! || value != defaultValue)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, string value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && (value == null!
                                || (cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length 
                                 && !(((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue))))
            ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, char[] value, int startIndex, int count = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        var cappedEnd    = cappedStart + cappedLength;
        return !stb.SkipFields && (value == null!
                                || (cappedLength == 0 && defaultValue.Length > 0)
                                || (cappedStart < value.Length 
                                 && !((ReadOnlySpan<char>)value)[cappedStart..cappedEnd].SequenceMatches(defaultValue)))
          ? AlwaysAdd(fieldName, value, cappedStart, count, formatString, formatFlags)
          : stb.StyleTypeBuilder;
    }

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value == null! || !value.SequenceMatches(defaultValue)
            ? AlwaysAddCharSeq(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
        var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
        return !stb.SkipFields && value == null! 
            || (cappedLength == 0 && defaultValue.Length > 0)
            || (cappedStart < value!.Length 
             && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && !(value?.SequenceMatches(defaultValue) ?? false)
            ? AlwaysAdd(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;

    public TMold WhenNonDefaultAdd(ReadOnlySpan<char> fieldName, StringBuilder value, int startIndex, int count = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
      var cappedStart  = Math.Clamp(startIndex, 0, value?.Length ?? 0);
      var cappedLength = Math.Clamp(count, 0, (value?.Length ?? 0) - cappedStart);
      return !stb.SkipFields && value == null! 
          || (cappedLength == 0 && defaultValue.Length > 0)
          || (cappedStart < value!.Length 
           && !value.SequenceMatches(defaultValue, cappedStart, cappedLength))
        ? AlwaysAdd(fieldName, value, startIndex, count, formatString, formatFlags)
        : stb.StyleTypeBuilder;
    }


    public TMold WhenNonDefaultAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny? value, TAny? defaultValue = default(TAny)
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        
        if( stb.SkipFields) return  stb.StyleTypeBuilder;
            
        var anyIsNullable = typeof(TAny).IsNullable();
        var shouldProceed =  (anyIsNullable && value == null && defaultValue != null);
        if(!shouldProceed) shouldProceed =  anyIsNullable && !Equals(value, defaultValue ?? typeof(TAny).GetDefaultForUnderlyingNullableOrThis());
        if(!shouldProceed) shouldProceed = !anyIsNullable && !Equals(value, defaultValue ?? default(TAny))
                                         && !(defaultValue != null && value != null && value.IsStringBuilder() 
                                           && defaultValue.IsStringBuilder() && value.UnknownSequenceMatches(defaultValue));
        return shouldProceed 
            ? AlwaysAddMatch(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TMold WhenNonDefaultAddObject(ReadOnlySpan<char> fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && ((value != null || defaultValue != null)
                         && (value ?? defaultValue!).GetType().IsValueType
                         && !Equals(value, defaultValue ?? (value ?? defaultValue!).GetType().GetDefaultForUnderlyingNullableOrThis()))
     || ((value != null || defaultValue != null)
      && (!(value ?? defaultValue!).GetType().IsValueType
      && (!Equals(value, defaultValue) 
       && !(defaultValue != null && value != null && value.IsStringBuilder() && defaultValue.IsStringBuilder() 
         && value.UnknownSequenceMatches(defaultValue)))))
            ? AlwaysAddObject(fieldName, value, formatString, formatFlags)
            : stb.StyleTypeBuilder;
}
