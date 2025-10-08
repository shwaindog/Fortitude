// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[Flags]
public enum ScaffoldingStringBuilderInvokeFlags : ulong
{
    None                        = 0x00_00_00_00_00
  , SimpleType                  = 0x00_00_00_00_01
  , ComplexType                 = 0x00_00_00_00_02
  , CollectionType              = 0x00_00_00_00_04
  , KeyedCollectionType         = 0x00_00_00_00_08
  , AcceptsCollection           = 0x00_00_00_00_10
  , AcceptsKeyValueCollection   = 0x00_00_00_00_20
  , AcceptsSingleValue          = 0x00_00_00_00_40
  , AcceptsStruct               = 0x00_00_00_00_80
  , AcceptsClass                = 0x00_00_00_01_00
  , AcceptsNullableStruct       = 0x00_00_00_02_00
  , AcceptsNullableClass        = 0x00_00_00_04_00
  , AcceptsChars                = 0x00_00_00_08_00
  , AcceptsSpanFormattable      = 0x00_00_00_10_00
  , AcceptsIntegerNumber        = 0x00_00_00_30_00
  , AcceptsDecimalNumber        = 0x00_00_00_50_00
  , AcceptsDateTimeLike         = 0x00_00_00_90_00
  , AcceptsStringBearer         = 0x00_00_01_00_00
  , AcceptsArray                = 0x00_00_02_00_00
  , AcceptsList                 = 0x00_00_04_00_00
  , AcceptsDictionary           = 0x00_00_08_00_00
  , AcceptsEnumerable           = 0x00_00_10_00_00
  , AcceptsEnumerator           = 0x00_00_20_00_00
  , AcceptsAny                  = 0x00_00_2F_FF_80
  , CallsAsSpan                 = 0x00_00_40_00_00
  , CallsAsReadOnlySpan         = 0x00_00_80_00_00
  , AcceptsMask                 = 0x00_00_FF_FF_F0
  , AlwaysWrites                = 0x00_01_00_00_00
  , OnlyPopulatedWrites         = 0x00_02_00_00_00
  , NonNullWrites               = 0x00_04_00_00_00
  , NonNullAndPopulatedWrites   = 0x00_08_00_00_00
  , OutputConditionMask         = 0x00_0F_00_00_00
  , FilterPredicate             = 0x00_10_00_00_00
  , SubsetListFilter            = 0x00_20_00_00_00
  , StatefulFilter              = 0x00_40_00_00_00
  , SupportsValueFormatString   = 0x01_00_00_00_00
  , SupportsKeyFormatString     = 0x02_00_00_00_00
  , SupportsValueRevealer       = 0x04_00_00_00_00
  , SupportsKeyRevealer         = 0x08_00_00_00_00
  , SupportsCustomHandling      = 0x10_00_00_00_00
  , SupportsIndexSubRanges      = 0x20_00_00_00_00
  , SupportsSettingDefaultValue = 0x40_00_00_00_00
}

public static class ScaffoldingStringBuilderInvokeFlagsExtensions
{
    

    public static bool HasAllOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllAreSet)  => (flags & checkAllAreSet) == checkAllAreSet;
    public static bool HasNoneOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllFound)   => flags == checkAllFound;
}