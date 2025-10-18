﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[Flags]
public enum ScaffoldingStringBuilderInvokeFlags : ulong
{
    None                             = 0x00_00_00_00_00_00
  , AlwaysWrites                     = 0x00_00_00_00_00_01
  , NonEmptyWrites                   = 0x00_00_00_00_00_02
  , NonNullWrites                    = 0x00_00_00_00_00_04
  , NonNullAndPopulatedWrites        = 0x00_00_00_00_00_08
  , OutputConditionMask              = 0x00_00_00_00_00_0F
  , SimpleType                       = 0x00_00_00_00_00_10
  , ComplexType                      = 0x00_00_00_00_00_20
  , OrderedCollectionType            = 0x00_00_00_00_00_40
  , KeyedCollectionType              = 0x00_00_00_00_00_80
  , AcceptsCollection                = 0x00_00_00_00_01_00
  , AcceptsKeyValueCollection        = 0x00_00_00_00_02_00
  , AcceptsSingleValue               = 0x00_00_00_00_04_00
  , AcceptsStruct                    = 0x00_00_00_00_08_00
  , AcceptsClass                     = 0x00_00_00_00_10_00
  , AcceptsNullableStruct            = 0x00_00_00_00_20_00
  , AcceptsNullableClass             = 0x00_00_00_00_40_00
  , AcceptsChars                     = 0x00_00_00_00_80_00
  , AcceptsCharArray                 = 0x00_00_00_01_80_00
  , AcceptsString                    = 0x00_00_00_02_80_00
  , AcceptsCharSequence              = 0x00_00_00_04_80_00
  , AcceptsStringBuilder             = 0x00_00_00_08_80_00
  , AcceptsSpanFormattable           = 0x00_00_00_10_00_00
  , AcceptsIntegerNumber             = 0x00_00_00_30_00_00
  , AcceptsDecimalNumber             = 0x00_00_00_50_00_00
  , AcceptsDateTimeLike              = 0x00_00_00_90_00_00
  , AcceptsStringBearer              = 0x00_00_01_00_00_00
  , AcceptsArray                     = 0x00_00_02_00_00_00
  , AcceptsList                      = 0x00_00_04_00_00_00
  , AcceptsDictionary                = 0x00_00_08_00_00_00
  , AcceptsEnumerable                = 0x00_00_10_00_00_00
  , AcceptsEnumerator                = 0x00_00_20_00_00_00
  , AcceptsAnyGeneric                = 0x00_00_3F_FF_F8_00
  , CallsAsSpan                      = 0x00_00_40_00_00_00
  , CallsAsReadOnlySpan              = 0x00_00_80_00_00_00
  , SubSpanCallMask                  = 0x00_00_C0_00_00_00
  , FilterPredicate                  = 0x00_01_00_00_00_00
  , SubsetListFilter                 = 0x00_02_00_00_00_00
  , StatefulFilter                   = 0x00_04_00_00_00_00
  , SupportsValueFormatString        = 0x00_10_00_00_00_00
  , SupportsKeyFormatString          = 0x00_20_00_00_00_00
  , SupportsValueRevealer            = 0x00_40_00_00_00_00
  , SupportsKeyRevealer              = 0x00_80_00_00_00_00
  , SupportsCustomHandling           = 0x01_00_00_00_00_00
  , SupportsIndexSubRanges           = 0x02_00_00_00_00_00
  , SupportsSettingDefaultValue      = 0x04_00_00_00_00_00
  , SupportsCreatingPalantirRevealer = 0x08_00_00_00_00_00
}

public static class ScaffoldingStringBuilderInvokeFlagsExtensions
{
    public static bool IsNullableSpanFormattableOnly(this ScaffoldingStringBuilderInvokeFlags flags) =>
        flags.IsNotAcceptsAnyGeneric() && flags.HasAcceptsNullableStruct() && flags.HasAcceptsSpanFormattable();

    public static bool IsAcceptsAnyGeneric(this ScaffoldingStringBuilderInvokeFlags flags)              => (flags & AcceptsAnyGeneric) == AcceptsAnyGeneric;
    public static bool IsNotAcceptsAnyGeneric(this ScaffoldingStringBuilderInvokeFlags flags)           => !flags.HasAllOf(AcceptsAnyGeneric);
    public static bool HasAcceptsNullableStruct(this ScaffoldingStringBuilderInvokeFlags flags)  => (flags & AcceptsNullableStruct) > 0;
    public static bool HasAcceptsStruct(this ScaffoldingStringBuilderInvokeFlags flags)          => (flags & AcceptsStruct) > 0;
    public static bool DoesNotAcceptsStruct(this ScaffoldingStringBuilderInvokeFlags flags)      => !flags.HasAcceptsStruct();
    public static bool HasAcceptsSpanFormattable(this ScaffoldingStringBuilderInvokeFlags flags) => (flags & AcceptsSpanFormattable) > 0;

    public static bool HasAllOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllAreSet) =>
        (flags & checkAllAreSet) == checkAllAreSet;

    public static bool HasNoneOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkNonAreSet) =>
        (flags & checkNonAreSet) == 0;

    public static bool HasAnyOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAnyAreFound) =>
        (flags & checkAnyAreFound) > 0;

    public static bool IsExactly(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllFound) =>
        flags == checkAllFound;
}
