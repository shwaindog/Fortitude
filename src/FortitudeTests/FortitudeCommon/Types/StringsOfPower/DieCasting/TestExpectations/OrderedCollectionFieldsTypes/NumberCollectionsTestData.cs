// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

public class NumberCollectionsTestData
{
    
    
    private static PositionUpdatingList<IOrderedListExpect>? allNumberCollectionsExpectations;

    public static PositionUpdatingList<IOrderedListExpect> AllNumberCollectionsExpectations => allNumberCollectionsExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(NumberCollectionsTestData))
        {
            
        // float Collections (struct - json native)
        new OrderedListExpect<float>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<float>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<float>(FloatList, "", name: "All_NoFilter")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ 3.1415927, 2.7182817, 6.2831855, 5.4365635, 12.566371, 10.873127, 18.849556, 16.30969, 25.132742, 21.746254 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[3.1415927,2.7182817,6.2831855,5.4365635,12.566371,10.873127,18.849556,16.30969,25.132742,21.746254]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  3.1415927,
                  2.7182817,
                  6.2831855,
                  5.4365635,
                  12.566371,
                  10.873127,
                  18.849556,
                  16.30969,
                  25.132742,
                  21.746254
                ]
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<float>(FloatList, null, () => Float_First_5)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 3.1415927, 2.7182817, 6.2831855, 5.4365635, 12.566371 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[3.1415927,2.7182817,6.2831855,5.4365635,12.566371]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  3.1415927,
                  2.7182817,
                  6.2831855,
                  5.4365635,
                  12.566371
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float>(FloatList, "|{0,-10:F4}|",() => Float_Skip_Odd_Index)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ |3.1416    |, |6.2832    |, |12.5664   |, |18.8496   |, |25.1327   | ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[|3.1416    |,|6.2832    |,|12.5664   |,|18.8496   |,|25.1327   |]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  |3.1416    |,
                  |6.2832    |,
                  |12.5664   |,
                  |18.8496   |,
                  |25.1327   |
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float>(FloatList, "{0:F3}", () => Float_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ 3.142, 2.718 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[3.142,2.718]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  3.142,
                  2.718
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float>(FloatList, "", () => Float_First_Gt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 12.566371 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[12.566371]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  12.566371
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float>(FloatList, "", () => Float_Second_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 10.873127, 18.849556, 16.30969, 25.132742, 21.746254 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[10.873127,18.849556,16.30969,25.132742,21.746254]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  10.873127,
                  18.849556,
                  16.30969,
                  25.132742,
                  21.746254
                ]
                """.Dos2Unix()
            }
        }
        
        // float? Collections (nullable struct - json native)
      , new OrderedListExpect<float?>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<float?>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<float?>(NullFloatList, "", name: "All_NoFilter")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                   |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.1415927, 2.7182817, null, null, 9.424778, null, null, null, 8.154845, 12.566371, 10.873127, null, 15.707964, null, 13.591409 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,3.1415927,2.7182817,null,null,9.424778,null,null,null,8.154845,12.566371,10.873127,null,15.707964,null,13.591409]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  null,
                  3.1415927,
                  2.7182817,
                  null,
                  null,
                  9.424778,
                  null,
                  null,
                  null,
                  8.154845,
                  12.566371,
                  10.873127,
                  null,
                  15.707964,
                  null,
                  13.591409
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float?>(NullFloatList, null, () => NullFloat_First_5)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.1415927, 2.7182817, null, null ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,3.1415927,2.7182817,null,null]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  null,
                  3.1415927,
                  2.7182817,
                  null,
                  null
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float?>(NullFloatList, "|{0,10:F4}|",() => NullFloat_Skip_Odd_Index)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ |      null|, |    2.7183|, |      null|, |      null|, |      null|, |   12.5664|, |      null|, |      null| ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[|      null|,|    2.7183|,|      null|,|      null|,|      null|,|   12.5664|,|      null|,|      null|]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  |      null|,
                  |    2.7183|,
                  |      null|,
                  |      null|,
                  |      null|,
                  |   12.5664|,
                  |      null|,
                  |      null|
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float?>(NullFloatList, "{0:F3}", () => NullFloat_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.142 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,3.142]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  null,
                  3.142
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float?>(NullFloatList, "", () => NullFloat_First_Gt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 12.566371 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[12.566371]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  12.566371
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<float?>(NullFloatList, "", () => NullFloat_Second_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 9.424778, null, null, null, 8.154845 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[9.424778,null,null,null,8.154845]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  9.424778,
                  null,
                  null,
                  null,
                  8.154845
                ]
                """.Dos2Unix()
            }
        }
        };
}
