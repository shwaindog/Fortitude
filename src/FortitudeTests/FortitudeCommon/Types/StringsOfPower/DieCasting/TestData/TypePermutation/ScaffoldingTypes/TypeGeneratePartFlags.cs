// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[Flags]
public enum ScaffoldingStringBuilderInvokeFlags : ulong
{
    None                             = 0x00_00_00_00_00_00_00
  , AlwaysWrites                     = 0x00_00_00_00_00_00_01
  , NonDefaultWrites                 = 0x00_00_00_00_00_00_02
  , NonNullWrites                    = 0x00_00_00_00_00_00_04
  , NonNullAndPopulatedWrites        = 0x00_00_00_00_00_00_08
  , AllOutputConditionsMask          = 0x00_00_00_00_00_00_0F
  , SimpleType                       = 0x00_00_00_00_00_00_10
  , ComplexType                      = 0x00_00_00_00_00_00_20
  , OrderedCollectionType            = 0x00_00_00_00_00_00_40
  , KeyedCollectionType              = 0x00_00_00_00_00_00_80
  , MoldTypeConditionMask            = 0x00_00_00_00_00_00_F0
  , CollectionCardinality            = 0x00_00_00_00_00_01_00
  , KeyValueCardinality              = 0x00_00_00_00_00_02_00
  , SingleValueCardinality           = 0x00_00_00_00_00_04_00
  , CardinalityMask                  = 0x00_00_00_00_00_07_00 
  , AcceptsStruct                    = 0x00_00_00_00_00_08_00
  , AcceptsClass                     = 0x00_00_00_00_00_10_00
  , AcceptsNullableStruct            = 0x00_00_00_00_00_20_00
  , AcceptsNullableClass             = 0x00_00_00_00_00_40_00
  , AcceptsChars                     = 0x00_00_00_00_00_80_00
  , AcceptsCharArray                 = 0x00_00_00_00_01_80_00
  , AcceptsString                    = 0x00_00_00_00_02_80_00
  , AcceptsCharSequence              = 0x00_00_00_00_04_80_00
  , AcceptsStringBuilder             = 0x00_00_00_00_08_80_00
  , AcceptsSpanFormattable           = 0x00_00_00_00_10_00_00
  , AcceptsIntegerNumber             = 0x00_00_00_00_30_00_00
  , AcceptsDecimalNumber             = 0x00_00_00_00_50_00_00
  , AcceptsDateTimeLike              = 0x00_00_00_00_90_00_00
  , AcceptsStringBearer              = 0x00_00_00_01_00_00_00
  , AcceptsAnyGeneric                = 0x00_00_00_01_FF_F8_00
  , AcceptsArray                     = 0x00_00_00_02_00_00_00
  , AcceptsList                      = 0x00_00_00_04_00_00_00
  , AcceptsDictionary                = 0x00_00_00_08_00_00_00
  , AcceptsEnumerable                = 0x00_00_00_10_00_00_00
  , AcceptsEnumerator                = 0x00_00_00_20_00_00_00
  , CallsAsSpan                      = 0x00_00_00_40_00_00_00
  , CallsAsReadOnlySpan              = 0x00_00_00_80_00_00_00
  , SubSpanCallMask                  = 0x00_00_00_C0_00_00_00
  , AcceptsUnknownObject             = 0x00_00_01_00_00_00_00
  , FilterPredicate                  = 0x00_00_02_00_00_00_00
  , SubsetListFilter                 = 0x00_00_04_00_00_00_00
  , StatefulFilter                   = 0x00_00_08_00_00_00_00
  , SupportsValueFormatString        = 0x00_00_10_00_00_00_00
  , SupportsKeyFormatString          = 0x00_00_20_00_00_00_00
  , SupportsValueRevealer            = 0x00_00_40_00_00_00_00
  , SupportsKeyRevealer              = 0x00_00_80_00_00_00_00
  , SupportsIndexSubRanges           = 0x00_01_00_00_00_00_00
  , SupportsSettingDefaultValue      = 0x00_02_00_00_00_00_00
  , SupportsCreatingPalantirRevealer = 0x00_04_00_00_00_00_00
  , SupportsCustomHandling           = 0x00_08_00_00_00_00_00
  , DefaultTreatedAsStringOut        = 0x00_10_00_00_00_00_00
  , DefaultTreatedAsValueOut         = 0x00_20_00_00_00_00_00
  , OutputTreatedMask                = 0x00_30_00_00_00_00_00
  , DefaultBecomesZero               = 0x00_40_00_00_00_00_00
  , DefaultBecomesNull               = 0x00_80_00_00_00_00_00
  , DefaultBecomesEmpty              = 0x01_00_00_00_00_00_00
  , DefaultBecomesFallback           = 0x02_00_00_00_00_00_00
  , EmptyBecomesNull                 = 0x04_00_00_00_00_00_00
  , OutputBecomesMask                = 0x07_C0_00_00_00_00_00
  , OutputMask                       = 0x07_F0_00_00_00_00_00
}

public static class ScaffoldingStringBuilderInvokeFlagsExtensions
{
    public static bool IsNullableSpanFormattableOnly(this ScaffoldingStringBuilderInvokeFlags flags) =>
        flags.IsNotAcceptsAnyGeneric() && flags.HasAcceptsNullableStruct() && flags.HasAcceptsSpanFormattable();

    public static bool HasComplexTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & ComplexType) > 0;

    public static bool HasSimpleTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & SimpleType) > 0;

    public static bool HasOrderedCollectionTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & OrderedCollectionType) > 0;

    public static bool DoesNotHaveOrderedCollectionTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & OrderedCollectionType) == 0;

    public static bool HasKeyedCollectionTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & KeyedCollectionType) > 0;

    public static bool DoesNotHaveKeyedCollectionTypeFlag(this ScaffoldingStringBuilderInvokeFlags flags) =>
        (flags & KeyedCollectionType) == 0;

    public static bool IsAcceptsBool(this ScaffoldingStringBuilderInvokeFlags flags) =>
        flags.HasAnyOf(AcceptsStruct | AcceptsNullableStruct)
     && (flags.HasNoneOf(AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable | SupportsValueRevealer | AcceptsStringBearer)
      || flags.IsAcceptsAnyGeneric());

    public static bool IsAcceptsAnyGeneric(this ScaffoldingStringBuilderInvokeFlags flags)        => (flags & AcceptsAnyGeneric) == AcceptsAnyGeneric;
    public static bool IsNotAcceptsAnyGeneric(this ScaffoldingStringBuilderInvokeFlags flags)     => !flags.HasAllOf(AcceptsAnyGeneric);
    public static bool HasAcceptsNullableStruct(this ScaffoldingStringBuilderInvokeFlags flags)   => (flags & AcceptsNullableStruct) > 0;
    public static bool HasAcceptsNullableClass(this ScaffoldingStringBuilderInvokeFlags flags)   => (flags & AcceptsNullableClass) > 0;
    
    public static bool HasAcceptsNullables(this ScaffoldingStringBuilderInvokeFlags flags)   =>
        flags.HasAcceptsNullableStruct() || flags.HasAcceptsNullableClass();
    
    public static bool HasAcceptsStruct(this ScaffoldingStringBuilderInvokeFlags flags)           => (flags & AcceptsStruct) > 0;
    public static bool DoesNotAcceptsStruct(this ScaffoldingStringBuilderInvokeFlags flags)       => !flags.HasAcceptsStruct();
    public static bool HasAcceptsSpanFormattable(this ScaffoldingStringBuilderInvokeFlags flags)  => (flags & AcceptsSpanFormattable) > 0;
    public static bool HasAcceptsArray(this ScaffoldingStringBuilderInvokeFlags flags)            => (flags & AcceptsArray) > 0;
    public static bool HasAcceptsList(this ScaffoldingStringBuilderInvokeFlags flags)             => (flags & AcceptsList) > 0;
    public static bool HasAcceptsEnumerable(this ScaffoldingStringBuilderInvokeFlags flags)       => (flags & AcceptsEnumerable) > 0;
    public static bool HasAcceptsEnumerator(this ScaffoldingStringBuilderInvokeFlags flags)       => (flags & AcceptsEnumerator) > 0;
    public static bool HasFilterPredicate(this ScaffoldingStringBuilderInvokeFlags flags)         => (flags & FilterPredicate) > 0;
    public static bool DoesNotHaveFilterPredicate(this ScaffoldingStringBuilderInvokeFlags flags) => (flags & FilterPredicate) == 0;
    public static bool HasAcceptsUnknownObject(this ScaffoldingStringBuilderInvokeFlags flags)    => (flags & FilterPredicate) > 0;
    public static bool DoesAcceptsUnknownObject(this ScaffoldingStringBuilderInvokeFlags flags)   => (flags & FilterPredicate) == 0;

    public static bool HasAllOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllAreSet) =>
        (flags & checkAllAreSet) == checkAllAreSet;

    public static bool HasNoneOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkNonAreSet) =>
        (flags & checkNonAreSet) == 0;

    public static bool HasAnyOf(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAnyAreFound) =>
        (flags & checkAnyAreFound) > 0;

    public static ScaffoldingStringBuilderInvokeFlags Unset(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags toRemove) =>
        flags & ~toRemove;

    public static bool IsExactly(this ScaffoldingStringBuilderInvokeFlags flags, ScaffoldingStringBuilderInvokeFlags checkAllFound) =>
        flags == checkAllFound;
}
