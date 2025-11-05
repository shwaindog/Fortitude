// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
// ReSharper disable FormatStringProblem

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public static class BoolCollectionsTestData
{
    public static readonly Lazy<IOrderedListExpect[]> AllBoolCollectionExpectations = new (() =>
    [
        // bool Collections
        new OrderedListExpect<bool>(TestCollections.BoolList, "")
        {
            { new EK(AcceptsCollection | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ true, true, false, true, false, false, false, true, true, false, false, true, true, true ]" }
          , { new EK(OrderedCollectionType | AcceptsCollection  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[true,true,false,true,false,false,false,true,true,false,false,true,true,true]" }
          , { new EK(OrderedCollectionType | AcceptsCollection  | AcceptsStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  true,
                  true,
                  false,
                  true,
                  false,
                  false,
                  false,
                  true,
                  true,
                  false,
                  false,
                  true,
                  true,
                  true
                ]
                """ 
            }
        }
        
        // bool? Collections
      , new OrderedListExpect<bool?>(TestCollections.NullBoolList, "")
        {
            { new EK(AcceptsCollection | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ true, null, false, true, false, null, false, true, true, null, false, true, null, null ]" }
          , { new EK(OrderedCollectionType | AcceptsCollection  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                "[true,null,false,true,false,null,false,true,true,null,false,true,null,null]" }
          , { new EK(OrderedCollectionType | AcceptsCollection  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  true,
                  null,
                  false,
                  true,
                  false,
                  null,
                  false,
                  true,
                  true,
                  null,
                  false,
                  true,
                  null,
                  null
                ]
                """ 
            }
        }
    ]);
}
