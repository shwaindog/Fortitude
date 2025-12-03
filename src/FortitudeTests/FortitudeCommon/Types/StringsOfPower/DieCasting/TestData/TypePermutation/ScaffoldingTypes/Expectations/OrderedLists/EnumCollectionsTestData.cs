// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class EnumCollectionsTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allEnumCollectionsExpectations;

    public static PositionUpdatingList<IOrderedListExpect> AllEnumCollectionsExpectations => allEnumCollectionsExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(EnumCollectionsTestData))
        {
            
            
            // float Collections (struct - json native)
            new OrderedListExpect<NoDefaultLongNoFlagsEnum>([],  "", name: "Empty")
            {
                { new EK(   OrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
               ,{ new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
               ,{ new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(null,  "")
            {
                { new EK( OrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnumList, "", name: "All_NoFilter")
            {
                { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
              , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                    "[\"NDLNFE_4\",\"NDLNFE_34\",8589934592,\"NDLNFE_1\",0,\"NDLNFE_13\",\"NDLNFE_2\"]" }
              , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
              , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
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
        };
}
