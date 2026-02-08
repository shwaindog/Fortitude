// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.InputClassFlags;

namespace FortitudeCommon.Types.StringsOfPower.Options;

[Flags]
public enum InputClassFlags : byte
{
    None                 = 0
  , SpanFormattableClass = 1
  , StringClass          = 2
  , CharArrayClass       = 4
  , CharSequenceClass    = 8
  , StringBuilderClass   = 16
  , ObjectToStringClass  = 32
  , AllInputClasses      = 64
  , AsStringClasses      = 128
}

public static class InputClassFlagsExtensions
{
    private static readonly ConcurrentDictionary<Type, bool> TypeIsInputConstructionTypeCache = new();

    public static bool HasSpanFormattableClassFlag(this InputClassFlags flags) => (flags & SpanFormattableClass) > 0;
    public static bool HasStringClassFlag(this InputClassFlags flags)          => (flags & StringClass) > 0;
    public static bool HaCharArrayClassFlag(this InputClassFlags flags)        => (flags & CharArrayClass) > 0;
    public static bool HasCharSequenceClassFlag(this InputClassFlags flags)    => (flags & CharSequenceClass) > 0;
    public static bool HasStringBuilderClassFlag(this InputClassFlags flags)   => (flags & StringBuilderClass) > 0;
    public static bool HasObjectToStringClassFlag(this InputClassFlags flags)  => (flags & ObjectToStringClass) > 0;
    public static bool HasAllInputClassesFlag(this InputClassFlags flags)      => (flags & AllInputClasses) > 0;

    public static bool HasAsStringClassesFlag(this InputClassFlags flags) => (flags & AsStringClasses) > 0;


    public static bool IsSpanFormattableClassActive(this InputClassFlags flags) => (flags & (SpanFormattableClass | AllInputClasses)) > 0;
    public static bool IsStringClassActive(this InputClassFlags flags)          => (flags & (StringClass | AllInputClasses)) > 0;
    public static bool IsCharArrayClassActive(this InputClassFlags flags)       => (flags & (CharArrayClass | AllInputClasses)) > 0;
    public static bool IsCharSequenceClassActive(this InputClassFlags flags)    => (flags & (CharSequenceClass | AllInputClasses)) > 0;
    public static bool IsStringBuilderClassActive(this InputClassFlags flags)   => (flags & (StringBuilderClass | AllInputClasses)) > 0;
    public static bool IsObjectToStringClassActive(this InputClassFlags flags)  => (flags & ObjectToStringClass) > 0;


    public static bool? IsSpanFormattableClassActive(this InputClassFlags? flags) => flags?.IsSpanFormattableClassActive();
    public static bool? IsStringClassActive(this InputClassFlags? flags)          => flags?.IsStringClassActive();
    public static bool? IsCharArrayClassActive(this InputClassFlags? flags)       => flags?.IsCharArrayClassActive();
    public static bool? IsCharSequenceClassActive(this InputClassFlags? flags)    => flags?.IsCharSequenceClassActive();
    public static bool? IsStringBuilderClassActive(this InputClassFlags? flags)   => flags?.IsStringBuilderClassActive();
    public static bool? IsObjectToStringClassActive(this InputClassFlags? flags)  => flags?.IsObjectToStringClassActive();
    public static bool? IsAllInputClassesActive(this InputClassFlags? flags)      => flags?.HasAllInputClassesFlag();
    public static bool? IsAsStringClassesActive(this InputClassFlags? flags)      => flags?.HasAsStringClassesFlag();

    public static bool IsInputConstructionType(this Type checkIsInputType) =>
        checkIsInputType.IsSpanFormattableOrNullableCached()
     || checkIsInputType.IsStringBearerOrNullableCached()
     || checkIsInputType.IsBoolOrNullable()
     || checkIsInputType.IsString()
     || checkIsInputType.IsCharArray()
     || checkIsInputType.IsCharSequence()
     || checkIsInputType.IsStringBuilder();

    public static bool IsInputConstructionTypeCached(this Type? checkIsInputType) =>
        checkIsInputType != null && TypeIsInputConstructionTypeCache.GetOrAdd(checkIsInputType, t => t.IsInputConstructionType());

    public static InputClassFlags Unset(this InputClassFlags flags, InputClassFlags toUnset) => flags & ~toUnset;

    public static InputClassFlags SetTo(this InputClassFlags flags, InputClassFlags toToggle, bool setOrUnsetValue) =>
        setOrUnsetValue ? flags | toToggle : flags & ~toToggle;

    public static InputClassFlags? SetTo(this InputClassFlags? flags, InputClassFlags toToggle, bool setOrUnsetValue) =>
        flags == null
            ? setOrUnsetValue ? toToggle : None
            : setOrUnsetValue
                ? flags | toToggle
                : flags & ~toToggle;

    public static bool HasAllOf(this InputClassFlags flags, InputClassFlags checkAllFound) => (flags & checkAllFound) == checkAllFound;

    public static bool HasNoneOf(this InputClassFlags flags, InputClassFlags checkNonAreSet) => (flags & checkNonAreSet) == 0;

    public static bool HasAnyOf(this InputClassFlags flags, InputClassFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;

    public static bool IsExactly(this InputClassFlags flags, InputClassFlags checkAllFound) => flags == checkAllFound;
}
