// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.OrderedCollectionFieldsTypes;

public class EnumCollectionsTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allEnumCollectionsExpectations;

    public static PositionUpdatingList<IOrderedListExpect> AllEnumCollectionsExpectations => allEnumCollectionsExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(EnumCollectionsTestData))
        {
            // NoDefaultLongNoFlagsEnum Collections
            new OrderedListExpect<NoDefaultLongNoFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLNFE_4\",\"NDLNFE_34\",8589934592,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592,
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13",
                      "NDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLNFE_4\",\"NDLNFE_34\",8589934592,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592,
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13",
                      "NDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", () => NoDefaultLongNoFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLNFE_4\",\"NDLNFE_34\",8589934592]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", () => NoDefaultLongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLNFE_4\",8589934592,0,\"NDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLNFE_4",
                      8589934592,
                      0,
                      "NDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "{0:d}", () => NoDefaultLongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.9223372036854775807,
                     NoDefaultLongNoFlagsEnum.-9223372036854775808,
                     NoDefaultLongNoFlagsEnum.13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[9223372036854775807,-9223372036854775808,13]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.9223372036854775807,
                      NoDefaultLongNoFlagsEnum.-9223372036854775808,
                      NoDefaultLongNoFlagsEnum.13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      9223372036854775807,
                      -9223372036854775808,
                      13
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", () => NoDefaultLongNoFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLNFE_1\",0,\"NDLNFE_13\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13"
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable NoDefaultLongNoFlagsEnum Collections
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     null,
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLNFE_4\",\"NDLNFE_34\",8589934592,null,null,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      null,
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592,
                      null,
                      null,
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13",
                      "NDLNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     null,
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLNFE_4\",\"NDLNFE_34\",8589934592,null,null,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      null,
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592,
                      null,
                      null,
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13",
                      "NDLNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, "", () => NullNoDefaultLongNoFlagsEnum_First_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_4,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLNFE_4\",\"NDLNFE_34\",8589934592,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_4,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLNFE_4",
                      "NDLNFE_34",
                      8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, ""
                                                           , () => NullNoDefaultLongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_34,
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLNFE_34\",null,\"NDLNFE_1\",\"NDLNFE_13\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_34,
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLNFE_34",
                      null,
                      "NDLNFE_1",
                      "NDLNFE_13",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, "{0:d}"
                                                           , () => NullNoDefaultLongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongNoFlagsEnum.4,
                     NoDefaultLongNoFlagsEnum.8589934592,
                     null,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[4,8589934592,null,0,2]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongNoFlagsEnum.4,
                      NoDefaultLongNoFlagsEnum.8589934592,
                      null,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      4,
                      8589934592,
                      null,
                      0,
                      2
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum?>(NullNoDefaultLongNoFlagsEnumList.Value, "", () => NullNoDefaultLongNoFlagsEnum_Second_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongNoFlagsEnum.NDLNFE_1,
                     NoDefaultLongNoFlagsEnum.0,
                     NoDefaultLongNoFlagsEnum.NDLNFE_13,
                     NoDefaultLongNoFlagsEnum.NDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongNoFlagsEnum.NDLNFE_1,
                      NoDefaultLongNoFlagsEnum.0,
                      NoDefaultLongNoFlagsEnum.NDLNFE_13,
                      NoDefaultLongNoFlagsEnum.NDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLNFE_1",
                      0,
                      "NDLNFE_13",
                      "NDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }


            // NoDefaultULongNoFlagsEnum Collections
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUNFE_4\",\"NDUNFE_34\",8589934592,\"NDUNFE_1\",0,\"NDUNFE_13\",\"NDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592,
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13",
                      "NDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUNFE_4\",\"NDUNFE_34\",8589934592,\"NDUNFE_1\",0,\"NDUNFE_13\",\"NDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592,
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13",
                      "NDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "", () => NoDefaultULongNoFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUNFE_4\",\"NDUNFE_34\",8589934592]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "", () => NoDefaultULongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUNFE_4\",8589934592,0,\"NDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUNFE_4",
                      8589934592,
                      0,
                      "NDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "{0:d}", () => NoDefaultULongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.18446744073709551615,
                     NoDefaultULongNoFlagsEnum.1,
                     NoDefaultULongNoFlagsEnum.13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[18446744073709551615,1,13]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.18446744073709551615,
                      NoDefaultULongNoFlagsEnum.1,
                      NoDefaultULongNoFlagsEnum.13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      18446744073709551615,
                      1,
                      13
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnumList, "", () => NoDefaultULongNoFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUNFE_1\",0,\"NDUNFE_13\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13"
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable NoDefaultULongNoFlagsEnum Collections
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     null,
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUNFE_4\",\"NDUNFE_34\",8589934592,null,null,\"NDUNFE_1\",0,\"NDUNFE_13\",\"NDUNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      null,
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592,
                      null,
                      null,
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13",
                      "NDUNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     null,
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUNFE_4\",\"NDUNFE_34\",8589934592,null,null,\"NDUNFE_1\",0,\"NDUNFE_13\",\"NDUNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      null,
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592,
                      null,
                      null,
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13",
                      "NDUNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, ""
                                                            , () => NullNoDefaultULongNoFlagsEnum_First_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_4,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUNFE_4\",\"NDUNFE_34\",8589934592,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_4,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUNFE_4",
                      "NDUNFE_34",
                      8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, ""
                                                            , () => NullNoDefaultULongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_34,
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUNFE_34\",null,\"NDUNFE_1\",\"NDUNFE_13\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_34,
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUNFE_34",
                      null,
                      "NDUNFE_1",
                      "NDUNFE_13",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, "{0:d}"
                                                            , () => NullNoDefaultULongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongNoFlagsEnum.4,
                     NoDefaultULongNoFlagsEnum.8589934592,
                     null,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[4,8589934592,null,0,2]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongNoFlagsEnum.4,
                      NoDefaultULongNoFlagsEnum.8589934592,
                      null,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      4,
                      8589934592,
                      null,
                      0,
                      2
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongNoFlagsEnum?>(NullNoDefaultULongNoFlagsEnumList.Value, ""
                                                            , () => NullNoDefaultULongNoFlagsEnum_Second_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongNoFlagsEnum.NDUNFE_1,
                     NoDefaultULongNoFlagsEnum.0,
                     NoDefaultULongNoFlagsEnum.NDUNFE_13,
                     NoDefaultULongNoFlagsEnum.NDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUNFE_1\",0,\"NDUNFE_13\",\"NDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongNoFlagsEnum.NDUNFE_1,
                      NoDefaultULongNoFlagsEnum.0,
                      NoDefaultULongNoFlagsEnum.NDUNFE_13,
                      NoDefaultULongNoFlagsEnum.NDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUNFE_1",
                      0,
                      "NDUNFE_13",
                      "NDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }

            // WithDefaultLongNoFlagsEnum Collections
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLNFE_4\",\"WDLNFE_34\",8589934592,\"WDLNFE_1\",\"Default\",\"WDLNFE_13\",\"WDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592,
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13",
                      "WDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLNFE_4\",\"WDLNFE_34\",8589934592,\"WDLNFE_1\",\"Default\",\"WDLNFE_13\",\"WDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592,
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13",
                      "WDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "", () => WithDefaultLongNoFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLNFE_4\",\"WDLNFE_34\",8589934592]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "", () => WithDefaultLongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLNFE_4\",8589934592,\"Default\",\"WDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLNFE_4",
                      8589934592,
                      "Default",
                      "WDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "{0:d}"
                                                            , () => WithDefaultLongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.9223372036854775807,
                     WithDefaultLongNoFlagsEnum.-9223372036854775808,
                     WithDefaultLongNoFlagsEnum.13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[9223372036854775807,-9223372036854775808,13]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.9223372036854775807,
                      WithDefaultLongNoFlagsEnum.-9223372036854775808,
                      WithDefaultLongNoFlagsEnum.13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      9223372036854775807,
                      -9223372036854775808,
                      13
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnumList, "", () => WithDefaultLongNoFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLNFE_1\",\"Default\",\"WDLNFE_13\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13"
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable WithDefaultLongNoFlagsEnum Collections
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     null,
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLNFE_4\",\"WDLNFE_34\",8589934592,null,null,\"WDLNFE_1\",\"Default\",\"WDLNFE_13\",\"WDLNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      null,
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592,
                      null,
                      null,
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13",
                      "WDLNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     null,
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLNFE_4\",\"WDLNFE_34\",8589934592,null,null,\"WDLNFE_1\",\"Default\",\"WDLNFE_13\",\"WDLNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      null,
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592,
                      null,
                      null,
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13",
                      "WDLNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, ""
                                                             , () => NullWithDefaultLongNoFlagsEnum_First_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_4,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLNFE_4\",\"WDLNFE_34\",8589934592,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_4,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLNFE_4",
                      "WDLNFE_34",
                      8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, ""
                                                             , () => NullWithDefaultLongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_34,
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLNFE_34\",null,\"WDLNFE_1\",\"WDLNFE_13\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_34,
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLNFE_34",
                      null,
                      "WDLNFE_1",
                      "WDLNFE_13",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, "{0:d}"
                                                             , () => NullWithDefaultLongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongNoFlagsEnum.4,
                     WithDefaultLongNoFlagsEnum.8589934592,
                     null,
                     WithDefaultLongNoFlagsEnum.0,
                     WithDefaultLongNoFlagsEnum.2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[4,8589934592,null,0,2]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongNoFlagsEnum.4,
                      WithDefaultLongNoFlagsEnum.8589934592,
                      null,
                      WithDefaultLongNoFlagsEnum.0,
                      WithDefaultLongNoFlagsEnum.2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      4,
                      8589934592,
                      null,
                      0,
                      2
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongNoFlagsEnum?>(NullWithDefaultLongNoFlagsEnumList.Value, ""
                                                             , () => NullWithDefaultLongNoFlagsEnum_Second_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongNoFlagsEnum.WDLNFE_1,
                     WithDefaultLongNoFlagsEnum.Default,
                     WithDefaultLongNoFlagsEnum.WDLNFE_13,
                     WithDefaultLongNoFlagsEnum.WDLNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLNFE_1\",\"Default\",\"WDLNFE_13\",\"WDLNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongNoFlagsEnum.WDLNFE_1,
                      WithDefaultLongNoFlagsEnum.Default,
                      WithDefaultLongNoFlagsEnum.WDLNFE_13,
                      WithDefaultLongNoFlagsEnum.WDLNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLNFE_1",
                      "Default",
                      "WDLNFE_13",
                      "WDLNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }


            // WithDefaultULongNoFlagsEnum Collections
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUNFE_4\",\"WDUNFE_34\",8589934592,\"WDUNFE_1\",\"Default\",\"WDUNFE_13\",\"WDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592,
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13",
                      "WDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUNFE_4\",\"WDUNFE_34\",8589934592,\"WDUNFE_1\",\"Default\",\"WDUNFE_13\",\"WDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592,
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13",
                      "WDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "", () => WithDefaultULongNoFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUNFE_4\",\"WDUNFE_34\",8589934592]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "", () => WithDefaultULongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUNFE_4\",8589934592,\"Default\",\"WDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUNFE_4",
                      8589934592,
                      "Default",
                      "WDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "{0:d}"
                                                             , () => WithDefaultULongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.18446744073709551615,
                     WithDefaultULongNoFlagsEnum.1,
                     WithDefaultULongNoFlagsEnum.13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[18446744073709551615,1,13]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.18446744073709551615,
                      WithDefaultULongNoFlagsEnum.1,
                      WithDefaultULongNoFlagsEnum.13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      18446744073709551615,
                      1,
                      13
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnumList, "", () => WithDefaultULongNoFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUNFE_1\",\"Default\",\"WDUNFE_13\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13"
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable WithDefaultULongNoFlagsEnum Collections
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     null,
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUNFE_4\",\"WDUNFE_34\",8589934592,null,null,\"WDUNFE_1\",\"Default\",\"WDUNFE_13\",\"WDUNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      null,
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592,
                      null,
                      null,
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13",
                      "WDUNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     null,
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUNFE_4\",\"WDUNFE_34\",8589934592,null,null,\"WDUNFE_1\",\"Default\",\"WDUNFE_13\",\"WDUNFE_2\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      null,
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592,
                      null,
                      null,
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13",
                      "WDUNFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, ""
                                                              , () => NullWithDefaultULongNoFlagsEnum_First_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_4,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUNFE_4\",\"WDUNFE_34\",8589934592,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_4,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUNFE_4",
                      "WDUNFE_34",
                      8589934592,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, ""
                                                              , () => NullWithDefaultULongNoFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_34,
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUNFE_34\",null,\"WDUNFE_1\",\"WDUNFE_13\",null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_34,
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUNFE_34",
                      null,
                      "WDUNFE_1",
                      "WDUNFE_13",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, "{0:d}"
                                                              , () => NullWithDefaultULongNoFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongNoFlagsEnum.4,
                     WithDefaultULongNoFlagsEnum.8589934592,
                     null,
                     WithDefaultULongNoFlagsEnum.0,
                     WithDefaultULongNoFlagsEnum.2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[4,8589934592,null,0,2]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongNoFlagsEnum.4,
                      WithDefaultULongNoFlagsEnum.8589934592,
                      null,
                      WithDefaultULongNoFlagsEnum.0,
                      WithDefaultULongNoFlagsEnum.2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      4,
                      8589934592,
                      null,
                      0,
                      2
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongNoFlagsEnum?>(NullWithDefaultULongNoFlagsEnumList.Value, ""
                                                              , () => NullWithDefaultULongNoFlagsEnum_Second_5)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongNoFlagsEnum.WDUNFE_1,
                     WithDefaultULongNoFlagsEnum.Default,
                     WithDefaultULongNoFlagsEnum.WDUNFE_13,
                     WithDefaultULongNoFlagsEnum.WDUNFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUNFE_1\",\"Default\",\"WDUNFE_13\",\"WDUNFE_2\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongNoFlagsEnum.WDUNFE_1,
                      WithDefaultULongNoFlagsEnum.Default,
                      WithDefaultULongNoFlagsEnum.WDUNFE_13,
                      WithDefaultULongNoFlagsEnum.WDUNFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUNFE_1",
                      "Default",
                      "WDUNFE_13",
                      "WDUNFE_2"
                    ]
                    """.Dos2Unix()
                }
            }

            // NoDefaultLongWithFlagsEnum Collections
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.NDLWFE_4,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                     | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                     NoDefaultLongWithFlagsEnum.NDLWFE_34,
                     NoDefaultLongWithFlagsEnum.9223372028264841216,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6
                     | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                     NoDefaultLongWithFlagsEnum.0,
                     NoDefaultLongWithFlagsEnum.NDLWFE_13,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4
                     | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                     NoDefaultLongWithFlagsEnum.-9223371972430266113,
                     NoDefaultLongWithFlagsEnum.NDLWFE_2,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["NDLWFE_4","NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8","NDLWFE_34",9223372028264841216,
                    "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",0,"NDLWFE_13","NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask",
                    "NDLWFE_First8Mask, NDLWFE_LastTwoMask",-9223371972430266113,"NDLWFE_2","NDLWFE_First4Mask"
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.NDLWFE_4,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                      NoDefaultLongWithFlagsEnum.NDLWFE_34,
                      NoDefaultLongWithFlagsEnum.9223372028264841216,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6 | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                      NoDefaultLongWithFlagsEnum.0,
                      NoDefaultLongWithFlagsEnum.NDLWFE_13,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                      NoDefaultLongWithFlagsEnum.-9223371972430266113,
                      NoDefaultLongWithFlagsEnum.NDLWFE_2,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLWFE_4",
                      "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8",
                      "NDLWFE_34",
                      9223372028264841216,
                      "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",
                      0,
                      "NDLWFE_13",
                      "NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask",
                      "NDLWFE_First8Mask, NDLWFE_LastTwoMask",
                      -9223371972430266113,
                      "NDLWFE_2",
                      "NDLWFE_First4Mask"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnumList, "", () => NoDefaultLongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.NDLWFE_4,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                     | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                     NoDefaultLongWithFlagsEnum.NDLWFE_34 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLWFE_4\",\"NDLWFE_First4Mask,·NDLWFE_5,·NDLWFE_7,·NDLWFE_8\",\"NDLWFE_34\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.NDLWFE_4,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                      NoDefaultLongWithFlagsEnum.NDLWFE_34
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLWFE_4",
                      "NDLWFE_First4Mask,·NDLWFE_5,·NDLWFE_7,·NDLWFE_8",
                      "NDLWFE_34"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnumList, "", () => NoDefaultLongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.NDLWFE_4,
                     NoDefaultLongWithFlagsEnum.NDLWFE_34,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6
                     | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                     NoDefaultLongWithFlagsEnum.NDLWFE_13,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                     NoDefaultLongWithFlagsEnum.NDLWFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["NDLWFE_4","NDLWFE_34","NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7","NDLWFE_13","NDLWFE_First8Mask, NDLWFE_LastTwoMask","NDLWFE_2"]
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.NDLWFE_4,
                      NoDefaultLongWithFlagsEnum.NDLWFE_34,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6 | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                      NoDefaultLongWithFlagsEnum.NDLWFE_13,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                      NoDefaultLongWithFlagsEnum.NDLWFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLWFE_4",
                      "NDLWFE_34",
                      "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",
                      "NDLWFE_13",
                      "NDLWFE_First8Mask, NDLWFE_LastTwoMask",
                      "NDLWFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnumList, "{0:d}"
                                                            , () => NoDefaultLongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.223,
                     NoDefaultLongWithFlagsEnum.9223372028264841216,
                     NoDefaultLongWithFlagsEnum.0,
                     NoDefaultLongWithFlagsEnum.253,
                     NoDefaultLongWithFlagsEnum.-9223371972430266113,
                     NoDefaultLongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[223,9223372028264841216,0,253,-9223371972430266113,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.223,
                      NoDefaultLongWithFlagsEnum.9223372028264841216,
                      NoDefaultLongWithFlagsEnum.0,
                      NoDefaultLongWithFlagsEnum.253,
                      NoDefaultLongWithFlagsEnum.-9223371972430266113,
                      NoDefaultLongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      223,
                      9223372028264841216,
                      0,
                      253,
                      -9223371972430266113,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnumList, "", () => NoDefaultLongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.9223372028264841216,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6
                     | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                     NoDefaultLongWithFlagsEnum.0 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[9223372028264841216,\"NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7\",0]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.9223372028264841216,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6 | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                      NoDefaultLongWithFlagsEnum.0
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      9223372028264841216,
                      "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",
                      0
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable NoDefaultLongWithFlagsEnum Collections
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(NullNoDefaultLongWithFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongWithFlagsEnum.NDLWFE_4,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                     | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                     NoDefaultLongWithFlagsEnum.NDLWFE_34,
                     null,
                     null,
                     NoDefaultLongWithFlagsEnum.9223372028264841216,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6
                     | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                     NoDefaultLongWithFlagsEnum.0,
                     NoDefaultLongWithFlagsEnum.NDLWFE_13,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4
                     | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                     null,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                     NoDefaultLongWithFlagsEnum.-9223371972430266113,
                     NoDefaultLongWithFlagsEnum.NDLWFE_2,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"NDLWFE_4","NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8","NDLWFE_34",null,null,9223372028264841216,
                    "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",0,"NDLWFE_13","NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask",
                    null,"NDLWFE_First8Mask, NDLWFE_LastTwoMask",-9223371972430266113,"NDLWFE_2","NDLWFE_First4Mask",null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongWithFlagsEnum.NDLWFE_4,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                      NoDefaultLongWithFlagsEnum.NDLWFE_34,
                      null,
                      null,
                      NoDefaultLongWithFlagsEnum.9223372028264841216,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_6 | NoDefaultLongWithFlagsEnum.NDLWFE_7,
                      NoDefaultLongWithFlagsEnum.0,
                      NoDefaultLongWithFlagsEnum.NDLWFE_13,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                      null,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                      NoDefaultLongWithFlagsEnum.-9223371972430266113,
                      NoDefaultLongWithFlagsEnum.NDLWFE_2,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLWFE_4",
                      "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8",
                      "NDLWFE_34",
                      null,
                      null,
                      9223372028264841216,
                      "NDLWFE_1, NDLWFE_2, NDLWFE_6, NDLWFE_7",
                      0,
                      "NDLWFE_13",
                      "NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask",
                      null,
                      "NDLWFE_First8Mask, NDLWFE_LastTwoMask",
                      -9223371972430266113,
                      "NDLWFE_2",
                      "NDLWFE_First4Mask",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(NullNoDefaultLongWithFlagsEnumList.Value, "", () => NullNoDefaultLongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongWithFlagsEnum.NDLWFE_4,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                     | NoDefaultLongWithFlagsEnum.NDLWFE_8 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDLWFE_4\",\"NDLWFE_First4Mask,·NDLWFE_5,·NDLWFE_7,·NDLWFE_8\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongWithFlagsEnum.NDLWFE_4,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLWFE_4",
                      "NDLWFE_First4Mask,·NDLWFE_5,·NDLWFE_7,·NDLWFE_8"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(NullNoDefaultLongWithFlagsEnumList.Value, "", () => NullNoDefaultLongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                     | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                     null,
                     NoDefaultLongWithFlagsEnum.9223372028264841216,
                     NoDefaultLongWithFlagsEnum.0,
                     NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                     NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                     NoDefaultLongWithFlagsEnum.NDLWFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8",null,9223372028264841216,0,
                    "NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask","NDLWFE_First8Mask, NDLWFE_LastTwoMask","NDLWFE_2",null]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8,
                      null,
                      NoDefaultLongWithFlagsEnum.9223372028264841216,
                      NoDefaultLongWithFlagsEnum.0,
                      NoDefaultLongWithFlagsEnum.NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask,
                      NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                      NoDefaultLongWithFlagsEnum.NDLWFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8",
                      null,
                      9223372028264841216,
                      0,
                      "NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask",
                      "NDLWFE_First8Mask, NDLWFE_LastTwoMask",
                      "NDLWFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(NullNoDefaultLongWithFlagsEnumList.Value, "{0:d}"
                                                            , () => NullNoDefaultLongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.8,
                     NoDefaultLongWithFlagsEnum.-9223372036854775808,
                     null,
                     NoDefaultLongWithFlagsEnum.99,
                     NoDefaultLongWithFlagsEnum.4096,
                     null,
                     NoDefaultLongWithFlagsEnum.-9223371972430266113,
                     NoDefaultLongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[8,-9223372036854775808,null,99,4096,null,-9223371972430266113,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.8,
                      NoDefaultLongWithFlagsEnum.-9223372036854775808,
                      null,
                      NoDefaultLongWithFlagsEnum.99,
                      NoDefaultLongWithFlagsEnum.4096,
                      null,
                      NoDefaultLongWithFlagsEnum.-9223371972430266113,
                      NoDefaultLongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      8,
                      -9223372036854775808,
                      null,
                      99,
                      4096,
                      null,
                      -9223371972430266113,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultLongWithFlagsEnum?>(NullNoDefaultLongWithFlagsEnumList.Value, "", () => NullNoDefaultLongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultLongWithFlagsEnum.NDLWFE_34,
                     null,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDLWFE_34\",null,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultLongWithFlagsEnum.NDLWFE_34,
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDLWFE_34",
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
            
            // NoDefaultULongWithFlagsEnum Collections
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.NDUWFE_4,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                     | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                     NoDefaultULongWithFlagsEnum.NDUWFE_34,
                     NoDefaultULongWithFlagsEnum.9223372028264841216,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6
                     | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                     NoDefaultULongWithFlagsEnum.0,
                     NoDefaultULongWithFlagsEnum.NDUWFE_13,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4
                     | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                     NoDefaultULongWithFlagsEnum.9223372101279285503,
                     NoDefaultULongWithFlagsEnum.NDUWFE_2,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["NDUWFE_4","NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8","NDUWFE_34",9223372028264841216,
                    "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",0,"NDUWFE_13","NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask",
                    "NDUWFE_First8Mask, NDUWFE_LastTwoMask",9223372101279285503,"NDUWFE_2","NDUWFE_First4Mask"
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.NDUWFE_4,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                      NoDefaultULongWithFlagsEnum.NDUWFE_34,
                      NoDefaultULongWithFlagsEnum.9223372028264841216,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6 | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                      NoDefaultULongWithFlagsEnum.0,
                      NoDefaultULongWithFlagsEnum.NDUWFE_13,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                      NoDefaultULongWithFlagsEnum.9223372101279285503,
                      NoDefaultULongWithFlagsEnum.NDUWFE_2,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUWFE_4",
                      "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8",
                      "NDUWFE_34",
                      9223372028264841216,
                      "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",
                      0,
                      "NDUWFE_13",
                      "NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask",
                      "NDUWFE_First8Mask, NDUWFE_LastTwoMask",
                      9223372101279285503,
                      "NDUWFE_2",
                      "NDUWFE_First4Mask"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnumList, "", () => NoDefaultULongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.NDUWFE_4,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                     | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                     NoDefaultULongWithFlagsEnum.NDUWFE_34 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUWFE_4\",\"NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8\",\"NDUWFE_34\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.NDUWFE_4,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                      NoDefaultULongWithFlagsEnum.NDUWFE_34
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUWFE_4",
                      "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8",
                      "NDUWFE_34"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnumList, "", () => NoDefaultULongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.NDUWFE_4,
                     NoDefaultULongWithFlagsEnum.NDUWFE_34,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6
                     | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                     NoDefaultULongWithFlagsEnum.NDUWFE_13,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                     NoDefaultULongWithFlagsEnum.NDUWFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["NDUWFE_4","NDUWFE_34","NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7","NDUWFE_13","NDUWFE_First8Mask, NDUWFE_LastTwoMask","NDUWFE_2"]
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.NDUWFE_4,
                      NoDefaultULongWithFlagsEnum.NDUWFE_34,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6 | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                      NoDefaultULongWithFlagsEnum.NDUWFE_13,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                      NoDefaultULongWithFlagsEnum.NDUWFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUWFE_4",
                      "NDUWFE_34",
                      "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",
                      "NDUWFE_13",
                      "NDUWFE_First8Mask, NDUWFE_LastTwoMask",
                      "NDUWFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnumList, "{0:d}"
                                                            , () => NoDefaultULongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.223,
                     NoDefaultULongWithFlagsEnum.9223372028264841216,
                     NoDefaultULongWithFlagsEnum.0,
                     NoDefaultULongWithFlagsEnum.253,
                     NoDefaultULongWithFlagsEnum.9223372101279285503,
                     NoDefaultULongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[223,9223372028264841216,0,253,9223372101279285503,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.223,
                      NoDefaultULongWithFlagsEnum.9223372028264841216,
                      NoDefaultULongWithFlagsEnum.0,
                      NoDefaultULongWithFlagsEnum.253,
                      NoDefaultULongWithFlagsEnum.9223372101279285503,
                      NoDefaultULongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      223,
                      9223372028264841216,
                      0,
                      253,
                      9223372101279285503,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnumList, "", () => NoDefaultULongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.9223372028264841216,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6
                     | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                     NoDefaultULongWithFlagsEnum.0 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[9223372028264841216,\"NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7\",0]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.9223372028264841216,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6 | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                      NoDefaultULongWithFlagsEnum.0
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      9223372028264841216,
                      "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",
                      0
                    ]
                    """.Dos2Unix()
                }
            }
            
            // Nullable NoDefaultULongWithFlagsEnum Collections
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(NullNoDefaultULongWithFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongWithFlagsEnum.NDUWFE_4,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                     | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                     NoDefaultULongWithFlagsEnum.NDUWFE_34,
                     null,
                     null,
                     NoDefaultULongWithFlagsEnum.9223372028264841216,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6
                     | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                     NoDefaultULongWithFlagsEnum.0,
                     NoDefaultULongWithFlagsEnum.NDUWFE_13,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4
                     | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                     null,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                     NoDefaultULongWithFlagsEnum.9223372101279285503,
                     NoDefaultULongWithFlagsEnum.NDUWFE_2,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"NDUWFE_4","NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8","NDUWFE_34",null,null,9223372028264841216,
                    "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",0,"NDUWFE_13","NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask",
                    null,"NDUWFE_First8Mask, NDUWFE_LastTwoMask",9223372101279285503,"NDUWFE_2","NDUWFE_First4Mask",null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongWithFlagsEnum.NDUWFE_4,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                      NoDefaultULongWithFlagsEnum.NDUWFE_34,
                      null,
                      null,
                      NoDefaultULongWithFlagsEnum.9223372028264841216,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_2 | NoDefaultULongWithFlagsEnum.NDUWFE_6 | NoDefaultULongWithFlagsEnum.NDUWFE_7,
                      NoDefaultULongWithFlagsEnum.0,
                      NoDefaultULongWithFlagsEnum.NDUWFE_13,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                      null,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                      NoDefaultULongWithFlagsEnum.9223372101279285503,
                      NoDefaultULongWithFlagsEnum.NDUWFE_2,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUWFE_4",
                      "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8",
                      "NDUWFE_34",
                      null,
                      null,
                      9223372028264841216,
                      "NDUWFE_1, NDUWFE_2, NDUWFE_6, NDUWFE_7",
                      0,
                      "NDUWFE_13",
                      "NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask",
                      null,
                      "NDUWFE_First8Mask, NDUWFE_LastTwoMask",
                      9223372101279285503,
                      "NDUWFE_2",
                      "NDUWFE_First4Mask",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(NullNoDefaultULongWithFlagsEnumList.Value, "", () => NullNoDefaultULongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongWithFlagsEnum.NDUWFE_4,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                     | NoDefaultULongWithFlagsEnum.NDUWFE_8 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"NDUWFE_4\",\"NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongWithFlagsEnum.NDUWFE_4,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUWFE_4",
                      "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(NullNoDefaultULongWithFlagsEnumList.Value, "", () => NullNoDefaultULongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                     | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                     null,
                     NoDefaultULongWithFlagsEnum.9223372028264841216,
                     NoDefaultULongWithFlagsEnum.0,
                     NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                     NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                     NoDefaultULongWithFlagsEnum.NDUWFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8",null,9223372028264841216,0,
                    "NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask","NDUWFE_First8Mask, NDUWFE_LastTwoMask","NDUWFE_2",null]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8,
                      null,
                      NoDefaultULongWithFlagsEnum.9223372028264841216,
                      NoDefaultULongWithFlagsEnum.0,
                      NoDefaultULongWithFlagsEnum.NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask,
                      NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask,
                      NoDefaultULongWithFlagsEnum.NDUWFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8",
                      null,
                      9223372028264841216,
                      0,
                      "NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask",
                      "NDUWFE_First8Mask, NDUWFE_LastTwoMask",
                      "NDUWFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(NullNoDefaultULongWithFlagsEnumList.Value, "{0:d}"
                                                            , () => NullNoDefaultULongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.8,
                     NoDefaultULongWithFlagsEnum.9223372036854775808,
                     null,
                     NoDefaultULongWithFlagsEnum.99,
                     NoDefaultULongWithFlagsEnum.4096,
                     null,
                     NoDefaultULongWithFlagsEnum.9223372101279285503,
                     NoDefaultULongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[8,9223372036854775808,null,99,4096,null,9223372101279285503,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.8,
                      NoDefaultULongWithFlagsEnum.9223372036854775808,
                      null,
                      NoDefaultULongWithFlagsEnum.99,
                      NoDefaultULongWithFlagsEnum.4096,
                      null,
                      NoDefaultULongWithFlagsEnum.9223372101279285503,
                      NoDefaultULongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      8,
                      9223372036854775808,
                      null,
                      99,
                      4096,
                      null,
                      9223372101279285503,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<NoDefaultULongWithFlagsEnum?>(NullNoDefaultULongWithFlagsEnumList.Value, "", () => NullNoDefaultULongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     NoDefaultULongWithFlagsEnum.NDUWFE_34,
                     null,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"NDUWFE_34\",null,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      NoDefaultULongWithFlagsEnum.NDUWFE_34,
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "NDUWFE_34",
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
            }

            // WithDefaultLongWithFlagsEnum Collections
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnumList, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.WDLWFE_4,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7
                     | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                     WithDefaultLongWithFlagsEnum.WDLWFE_34,
                     WithDefaultLongWithFlagsEnum.9223372028264841216,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6
                     | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                     WithDefaultLongWithFlagsEnum.Default,
                     WithDefaultLongWithFlagsEnum.WDLWFE_13,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4
                     | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                     WithDefaultLongWithFlagsEnum.-9223371972430266113,
                     WithDefaultLongWithFlagsEnum.WDLWFE_2,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["WDLWFE_4","WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8","WDLWFE_34",9223372028264841216,
                    "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7","Default","WDLWFE_13","WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask",
                    "WDLWFE_First8Mask, WDLWFE_LastTwoMask",-9223371972430266113,"WDLWFE_2","WDLWFE_First4Mask"
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.WDLWFE_4,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                      WithDefaultLongWithFlagsEnum.WDLWFE_34,
                      WithDefaultLongWithFlagsEnum.9223372028264841216,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6 | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                      WithDefaultLongWithFlagsEnum.Default,
                      WithDefaultLongWithFlagsEnum.WDLWFE_13,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                      WithDefaultLongWithFlagsEnum.-9223371972430266113,
                      WithDefaultLongWithFlagsEnum.WDLWFE_2,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLWFE_4",
                      "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8",
                      "WDLWFE_34",
                      9223372028264841216,
                      "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7",
                      "Default",
                      "WDLWFE_13",
                      "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask",
                      "WDLWFE_First8Mask, WDLWFE_LastTwoMask",
                      -9223371972430266113,
                      "WDLWFE_2",
                      "WDLWFE_First4Mask"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnumList, "", () => WithDefaultLongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.WDLWFE_4,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7
                     | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                     WithDefaultLongWithFlagsEnum.WDLWFE_34 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLWFE_4\",\"WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8\",\"WDLWFE_34\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.WDLWFE_4,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                      WithDefaultLongWithFlagsEnum.WDLWFE_34
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLWFE_4",
                      "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8",
                      "WDLWFE_34"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnumList, "", () => WithDefaultLongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.WDLWFE_4,
                     WithDefaultLongWithFlagsEnum.WDLWFE_34,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6
                     | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                     WithDefaultLongWithFlagsEnum.WDLWFE_13,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                     WithDefaultLongWithFlagsEnum.WDLWFE_2 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    ["WDLWFE_4","WDLWFE_34","WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7","WDLWFE_13","WDLWFE_First8Mask, WDLWFE_LastTwoMask","WDLWFE_2"]
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.WDLWFE_4,
                      WithDefaultLongWithFlagsEnum.WDLWFE_34,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6 | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                      WithDefaultLongWithFlagsEnum.WDLWFE_13,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                      WithDefaultLongWithFlagsEnum.WDLWFE_2
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLWFE_4",
                      "WDLWFE_34",
                      "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7",
                      "WDLWFE_13",
                      "WDLWFE_First8Mask, WDLWFE_LastTwoMask",
                      "WDLWFE_2"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnumList, "{0:d}"
                                                            , () => WithDefaultLongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.223,
                     WithDefaultLongWithFlagsEnum.9223372028264841216,
                     WithDefaultLongWithFlagsEnum.0,
                     WithDefaultLongWithFlagsEnum.253,
                     WithDefaultLongWithFlagsEnum.-9223371972430266113,
                     WithDefaultLongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[223,9223372028264841216,0,253,-9223371972430266113,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.223,
                      WithDefaultLongWithFlagsEnum.9223372028264841216,
                      WithDefaultLongWithFlagsEnum.0,
                      WithDefaultLongWithFlagsEnum.253,
                      WithDefaultLongWithFlagsEnum.-9223371972430266113,
                      WithDefaultLongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      223,
                      9223372028264841216,
                      0,
                      253,
                      -9223371972430266113,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnumList, "", () => WithDefaultLongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.9223372028264841216,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6
                     | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                     WithDefaultLongWithFlagsEnum.Default 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[9223372028264841216,\"WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7\",\"Default\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.9223372028264841216,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6 | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                      WithDefaultLongWithFlagsEnum.Default
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      9223372028264841216,
                      "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7",
                      "Default"
                    ]
                    """.Dos2Unix()
                }
            }

            // Nullable WithDefaultLongWithFlagsEnum Collections
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(NullWithDefaultLongWithFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongWithFlagsEnum.WDLWFE_4,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7
                     | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                     WithDefaultLongWithFlagsEnum.WDLWFE_34,
                     null,
                     null,
                     WithDefaultLongWithFlagsEnum.9223372028264841216,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6
                     | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                     WithDefaultLongWithFlagsEnum.Default,
                     WithDefaultLongWithFlagsEnum.WDLWFE_13,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4
                     | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                     null,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                     WithDefaultLongWithFlagsEnum.-9223371972430266113,
                     WithDefaultLongWithFlagsEnum.WDLWFE_2,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"WDLWFE_4","WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8","WDLWFE_34",null,null,9223372028264841216,
                    "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7","Default","WDLWFE_13","WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask",
                    null,"WDLWFE_First8Mask, WDLWFE_LastTwoMask",-9223371972430266113,"WDLWFE_2","WDLWFE_First4Mask",null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongWithFlagsEnum.WDLWFE_4,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                      WithDefaultLongWithFlagsEnum.WDLWFE_34,
                      null,
                      null,
                      WithDefaultLongWithFlagsEnum.9223372028264841216,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_6 | WithDefaultLongWithFlagsEnum.WDLWFE_7,
                      WithDefaultLongWithFlagsEnum.Default,
                      WithDefaultLongWithFlagsEnum.WDLWFE_13,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                      null,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                      WithDefaultLongWithFlagsEnum.-9223371972430266113,
                      WithDefaultLongWithFlagsEnum.WDLWFE_2,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLWFE_4",
                      "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8",
                      "WDLWFE_34",
                      null,
                      null,
                      9223372028264841216,
                      "WDLWFE_1, WDLWFE_2, WDLWFE_6, WDLWFE_7",
                      "Default",
                      "WDLWFE_13",
                      "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask",
                      null,
                      "WDLWFE_First8Mask, WDLWFE_LastTwoMask",
                      -9223371972430266113,
                      "WDLWFE_2",
                      "WDLWFE_First4Mask",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(NullWithDefaultLongWithFlagsEnumList.Value, "", () => NullWithDefaultLongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongWithFlagsEnum.WDLWFE_4,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7
                     | WithDefaultLongWithFlagsEnum.WDLWFE_8 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDLWFE_4\",\"WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongWithFlagsEnum.WDLWFE_4,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLWFE_4",
                      "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(NullWithDefaultLongWithFlagsEnumList.Value, "", () => NullWithDefaultLongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7
                     | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                     null,
                     WithDefaultLongWithFlagsEnum.9223372028264841216,
                     WithDefaultLongWithFlagsEnum.Default,
                     WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                     WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                     WithDefaultLongWithFlagsEnum.WDLWFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8",null,9223372028264841216,"Default",
                    "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask","WDLWFE_First8Mask, WDLWFE_LastTwoMask","WDLWFE_2",null]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8,
                      null,
                      WithDefaultLongWithFlagsEnum.9223372028264841216,
                      WithDefaultLongWithFlagsEnum.Default,
                      WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                      WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                      WithDefaultLongWithFlagsEnum.WDLWFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8",
                      null,
                      9223372028264841216,
                      "Default",
                      "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask",
                      "WDLWFE_First8Mask, WDLWFE_LastTwoMask",
                      "WDLWFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(NullWithDefaultLongWithFlagsEnumList.Value, "{0:d}"
                                                            , () => NullWithDefaultLongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.8,
                     WithDefaultLongWithFlagsEnum.-9223372036854775808,
                     null,
                     WithDefaultLongWithFlagsEnum.99,
                     WithDefaultLongWithFlagsEnum.4096,
                     null,
                     WithDefaultLongWithFlagsEnum.-9223371972430266113,
                     WithDefaultLongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[8,-9223372036854775808,null,99,4096,null,-9223371972430266113,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.8,
                      WithDefaultLongWithFlagsEnum.-9223372036854775808,
                      null,
                      WithDefaultLongWithFlagsEnum.99,
                      WithDefaultLongWithFlagsEnum.4096,
                      null,
                      WithDefaultLongWithFlagsEnum.-9223371972430266113,
                      WithDefaultLongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      8,
                      -9223372036854775808,
                      null,
                      99,
                      4096,
                      null,
                      -9223371972430266113,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultLongWithFlagsEnum?>(NullWithDefaultLongWithFlagsEnumList.Value, "", () => NullWithDefaultLongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultLongWithFlagsEnum.WDLWFE_34,
                     null,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDLWFE_34\",null,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultLongWithFlagsEnum.WDLWFE_34,
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDLWFE_34",
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
            
            
            // Nullable WithDefaultULongWithFlagsEnum Collections
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>([], "", name: "Empty")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(null, "")
            {
                { new EK(IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(NullWithDefaultULongWithFlagsEnumList.Value, "", name: "All_NoFilter")
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongWithFlagsEnum.WDUWFE_4,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7
                     | WithDefaultULongWithFlagsEnum.WDUWFE_8,
                     WithDefaultULongWithFlagsEnum.WDUWFE_34,
                     null,
                     null,
                     WithDefaultULongWithFlagsEnum.9223372028264841216,
                     WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2 | WithDefaultULongWithFlagsEnum.WDUWFE_6
                     | WithDefaultULongWithFlagsEnum.WDUWFE_7,
                     WithDefaultULongWithFlagsEnum.Default,
                     WithDefaultULongWithFlagsEnum.WDUWFE_13,
                     WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4
                     | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask,
                     null,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask,
                     WithDefaultULongWithFlagsEnum.9223372101279285503,
                     WithDefaultULongWithFlagsEnum.WDUWFE_2,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"WDUWFE_4","WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8","WDUWFE_34",null,null,9223372028264841216,
                    "WDUWFE_1, WDUWFE_2, WDUWFE_6, WDUWFE_7","Default","WDUWFE_13","WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask",
                    null,"WDUWFE_First8Mask, WDUWFE_LastTwoMask",9223372101279285503,"WDUWFE_2","WDUWFE_First4Mask",null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongWithFlagsEnum.WDUWFE_4,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7 | WithDefaultULongWithFlagsEnum.WDUWFE_8,
                      WithDefaultULongWithFlagsEnum.WDUWFE_34,
                      null,
                      null,
                      WithDefaultULongWithFlagsEnum.9223372028264841216,
                      WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_2 | WithDefaultULongWithFlagsEnum.WDUWFE_6 | WithDefaultULongWithFlagsEnum.WDUWFE_7,
                      WithDefaultULongWithFlagsEnum.Default,
                      WithDefaultULongWithFlagsEnum.WDUWFE_13,
                      WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask,
                      null,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask,
                      WithDefaultULongWithFlagsEnum.9223372101279285503,
                      WithDefaultULongWithFlagsEnum.WDUWFE_2,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUWFE_4",
                      "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8",
                      "WDUWFE_34",
                      null,
                      null,
                      9223372028264841216,
                      "WDUWFE_1, WDUWFE_2, WDUWFE_6, WDUWFE_7",
                      "Default",
                      "WDUWFE_13",
                      "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask",
                      null,
                      "WDUWFE_First8Mask, WDUWFE_LastTwoMask",
                      9223372101279285503,
                      "WDUWFE_2",
                      "WDUWFE_First4Mask",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(NullWithDefaultULongWithFlagsEnumList.Value, "", () => NullWithDefaultULongWithFlagsEnum_First_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongWithFlagsEnum.WDUWFE_4,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7
                     | WithDefaultULongWithFlagsEnum.WDUWFE_8 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[null,\"WDUWFE_4\",\"WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongWithFlagsEnum.WDUWFE_4,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7 | WithDefaultULongWithFlagsEnum.WDUWFE_8
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUWFE_4",
                      "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(NullWithDefaultULongWithFlagsEnumList.Value, "", () => NullWithDefaultULongWithFlagsEnum_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7
                     | WithDefaultULongWithFlagsEnum.WDUWFE_8,
                     null,
                     WithDefaultULongWithFlagsEnum.9223372028264841216,
                     WithDefaultULongWithFlagsEnum.Default,
                     WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask,
                     WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask,
                     WithDefaultULongWithFlagsEnum.WDUWFE_2,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [null,"WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8",null,9223372028264841216,"Default",
                    "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask","WDUWFE_First8Mask, WDUWFE_LastTwoMask","WDUWFE_2",null]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7 | WithDefaultULongWithFlagsEnum.WDUWFE_8,
                      null,
                      WithDefaultULongWithFlagsEnum.9223372028264841216,
                      WithDefaultULongWithFlagsEnum.Default,
                      WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask,
                      WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask,
                      WithDefaultULongWithFlagsEnum.WDUWFE_2,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8",
                      null,
                      9223372028264841216,
                      "Default",
                      "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask",
                      "WDUWFE_First8Mask, WDUWFE_LastTwoMask",
                      "WDUWFE_2",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(NullWithDefaultULongWithFlagsEnumList.Value, "{0:d}"
                                                            , () => NullWithDefaultULongWithFlagsEnum_Skip_Even_Index)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongWithFlagsEnum.8,
                     WithDefaultULongWithFlagsEnum.9223372036854775808,
                     null,
                     WithDefaultULongWithFlagsEnum.99,
                     WithDefaultULongWithFlagsEnum.4096,
                     null,
                     WithDefaultULongWithFlagsEnum.9223372101279285503,
                     WithDefaultULongWithFlagsEnum.15 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[8,9223372036854775808,null,99,4096,null,9223372101279285503,15]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongWithFlagsEnum.8,
                      WithDefaultULongWithFlagsEnum.9223372036854775808,
                      null,
                      WithDefaultULongWithFlagsEnum.99,
                      WithDefaultULongWithFlagsEnum.4096,
                      null,
                      WithDefaultULongWithFlagsEnum.9223372101279285503,
                      WithDefaultULongWithFlagsEnum.15
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      8,
                      9223372036854775808,
                      null,
                      99,
                      4096,
                      null,
                      9223372101279285503,
                      15
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<WithDefaultULongWithFlagsEnum?>(NullWithDefaultULongWithFlagsEnumList.Value, "", () => NullWithDefaultULongWithFlagsEnum_Second_3)
            {
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     WithDefaultULongWithFlagsEnum.WDUWFE_34,
                     null,
                     null 
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                  , "[\"WDUWFE_34\",null,null]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      WithDefaultULongWithFlagsEnum.WDUWFE_34,
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      "WDUWFE_34",
                      null,
                      null
                    ]
                    """.Dos2Unix()
                }
            }
        };
}
