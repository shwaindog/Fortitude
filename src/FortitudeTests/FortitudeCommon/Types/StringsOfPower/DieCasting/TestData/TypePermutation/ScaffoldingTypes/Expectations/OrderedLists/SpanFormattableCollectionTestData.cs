// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class SpanFormattableCollectionTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allSpanFormattableCollectionExpectations;  
    
    public static PositionUpdatingList<IOrderedListExpect> AllSpanFormattableCollectionExpectations => allSpanFormattableCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(SpanFormattableCollectionTestData))
        {
        // float Collections
        new OrderedListExpect<float>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsSpanFormattable), "[]" }
          , { new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<float>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsSpanFormattable | AlwaysWrites), "[]" }
          , { new EK(  AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<float>(TestCollections.FloatList, "")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 3.1415927, 2.7182817, 6.2831855, 5.4365635, 12.566371, 10.873127, 18.849556, 16.30969, 25.132742, 21.746254 ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[3.1415927,2.7182817,6.2831855,5.4365635,12.566371,10.873127,18.849556,16.30969,25.132742,21.746254]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
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
                """ 
            }
        }
      , new OrderedListExpect<float>(TestCollections.FloatList, null, () => TestCollections.Float_First_5)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 3.1415927, 2.7182817, 6.2831855, 5.4365635, 12.566371 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[3.1415927,2.7182817,6.2831855,5.4365635,12.566371]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  3.1415927,
                  2.7182817,
                  6.2831855,
                  5.4365635,
                  12.566371,
                ]
                """ 
            }
        }
      , new OrderedListExpect<float>(TestCollections.FloatList, "{0:F3}", () => TestCollections.Float_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 3.142, 2.718 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[3.142,2.718]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  3.142,
                  2.718
                ]
                """ 
            }
        }
      , new OrderedListExpect<float>(TestCollections.FloatList, "", () => TestCollections.Float_First_Gt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 12.566371 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[12.566371]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  12.566371
                ]
                """ 
            }
        }
        
        // float? Collections
      , new OrderedListExpect<float?>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsSpanFormattable), "[]" }
         ,  { new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable |  AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable |  AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<float?>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsSpanFormattable), "[]" }
          , { new EK(  AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AlwaysWrites, CompactJson), "null" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<float?>(TestCollections.NullFloatList, "")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                   |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.1415927, 2.7182817, null, null, 9.424778, null, null, null, 8.154845, 12.566371, 10.873127, null, 15.707964, null, 13.591409 ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[null,3.1415927,2.7182817,null,null,9.424778,null,null,null,8.154845,12.566371,10.873127,null,15.707964,null,13.591409]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  3.1415927,
                  2.7182817,
                  5.4365635,
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
                  15.707964
                  null,
                  13.591409]
                ]
                """ 
            }
        }
      , new OrderedListExpect<float?>(TestCollections.NullFloatList, null, () => TestCollections.NullFloat_First_5)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.1415927, 2.7182817, null, null ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[null,3.1415927,2.7182817,null,null]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  3.1415927,
                  2.7182817,
                  null,
                  null
                ]
                """ 
            }
        }
      , new OrderedListExpect<float?>(TestCollections.NullFloatList, "{0:F3}", () => TestCollections.NullFloat_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ null, 3.142 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[null,3.142]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  3.142
                ]
                """ 
            }
        }
      , new OrderedListExpect<float?>(TestCollections.NullFloatList, "", () => TestCollections.NullFloat_First_Gt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 12.566371 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[12.566371]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  12.566371
                ]
                """ 
            }
        }
    };
}
