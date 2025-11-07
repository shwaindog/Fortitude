// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class SpanFormattableCollectionTestData
{
    public static readonly Lazy<IOrderedListExpect[]> AllSpanFormattableCollectionExpectations = new (() =>
    [
        // float Collections
        new OrderedListExpect<float>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsStruct), "[]" }
          , { new EK(   AcceptsStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<float>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsStruct | AlwaysWrites), "[]" }
          , { new EK(  AcceptsStruct | AlwaysWrites), "null" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<float>(TestCollections.FloatList, "")
        {
            { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan 
                   |  AllOutputConditionsMask, CompactLog),
                "[ 3.1415927, 2.7182817, 6.2831855, 5.4365635, 12.566371, 10.873127, 18.849556, 16.30969, 25.132742, 21.746254 ]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[3.1415927,2.7182817,6.2831855,5.4365635,12.566371,10.873127,18.849556,16.30969,25.132742,21.746254]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
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
        
        // float? Collections
      , new OrderedListExpect<float?>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsNullableStruct), "[]" }
         ,  { new EK(   AcceptsNullableStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct |  AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct |  AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<float?>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsNullableStruct), "[]" }
          , { new EK(  AcceptsNullableStruct | AlwaysWrites), "null" }
          , { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AlwaysWrites, CompactJson), "null" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AlwaysWrites, Pretty), "null" }
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
    ]);
}
