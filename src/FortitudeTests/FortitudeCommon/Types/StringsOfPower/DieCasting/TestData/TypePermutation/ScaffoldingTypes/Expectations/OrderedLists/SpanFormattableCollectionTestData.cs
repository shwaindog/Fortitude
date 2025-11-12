// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class SpanFormattableCollectionTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allSpanFormattableCollectionExpectations;  
    
    public static PositionUpdatingList<IOrderedListExpect> AllSpanFormattableCollectionExpectations => allSpanFormattableCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(SpanFormattableCollectionTestData))
        {
        // float Collections (struct - json native)
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
      , new OrderedListExpect<float>(FloatList, "")
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
      , new OrderedListExpect<float>(FloatList, null, () => Float_First_5)
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
      , new OrderedListExpect<float>(FloatList, "{0:F3}", () => Float_First_2)
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
      , new OrderedListExpect<float>(FloatList, "", () => Float_First_Gt_10)
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
        
        // float? Collections (nullable struct - json native)
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
      , new OrderedListExpect<float?>(NullFloatList, "")
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
      , new OrderedListExpect<float?>(NullFloatList, null, () => NullFloat_First_5)
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
      , new OrderedListExpect<float?>(NullFloatList, "{0:F3}", () => NullFloat_First_2)
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
      , new OrderedListExpect<float?>(NullFloatList, "", () => NullFloat_First_Gt_10)
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
        
        // Version Collections (non null class - json as string)
      , new OrderedListExpect<Version>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsSpanFormattable), "[]" }
          , { new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<Version>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsSpanFormattable | AlwaysWrites), "[]" }
          , { new EK(  AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<Version>(VersionsList, "")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 0.0, 0.1.1, 1.1.1.1, 2.1.123456, 4.2.25, 8.3.3.3, 0.4, 16.0.0, 32.2563.1000000.1 ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[\"0.0\",\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"4.2.25\",\"8.3.3.3\",\"0.4\",\"16.0.0\",\"32.2563.1000000.1\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  "0.0",
                  "0.1.1",
                  "1.1.1.1",
                  "2.1.123456",
                  "4.2.25",
                  "8.3.3.3",
                  "0.4",
                  "16.0.0",
                  "32.2563.1000000.1"
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version>(VersionsList, null, () => Version_First_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 0.0, 0.1.1, 1.1.1.1, 2.1.123456, 4.2.25 ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[\"0.0\",\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"4.2.25\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  "0.0",
                  "0.1.1",
                  "1.1.1.1",
                  "2.1.123456",
                  "4.2.25"
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version>(VersionsList, "\"{0,-10}\"", () => Version_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ \"0.0       \", \"0.1.1     \" ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[\"0.0       \",\"0.1.1     \"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  "0.0       ",
                  "0.1.1     "
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version>(VersionsList, "", () => Version_First_MjrGt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 16.0.0 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[\"16.0.0\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  "16.0.0"
                ]
                """ 
            }
        }
        
        // Version Collections ( null class - json as string)
      , new OrderedListExpect<Version?>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsSpanFormattable), "[]" }
          , { new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<Version?>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsSpanFormattable | AlwaysWrites), "[]" }
          , { new EK(  AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
          , { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ null, 0.0, null, 0.1.1, 1.1.1.1, 2.1.123456, 8.3.3.3, null, null, null, null, 16.0.0, 32.2563.1000000.1, null, null, null ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[null,\"0.0\",null,\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"8.3.3.3\",null,null,null,null,\"16.0.0\",\"32.2563.1000000.1\",null,null,null]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  "0.0",
                  null,
                  "0.1.1",
                  "1.1.1.1",
                  "2.1.123456",
                  "8.3.3.3",
                  null,
                  null,
                  null,
                  null,
                  "16.0.0",
                  "32.2563.1000000.1",
                  null,
                  null,
                  null
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, null, () => NullVersion_First_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ null, 0.0, null, 0.1.1, 1.1.1.1 ]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[null,\"0.0\",null,\"0.1.1\",\"1.1.1.1\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  "0.0",
                  null,
                  "0.1.1",
                  "1.1.1.1"
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "\'{0,10}\'", () => NullVersion_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ '      null', '       0.0' ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "['      null',\"'       0.0'\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  '      null',
                  "'       0.0'"
                ]
                """ 
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "", () => NullVersion_First_MjrGt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 16.0.0 ]" }
          , { new EK(CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson),
                "[\"16.0.0\"]" }
          , { new EK( CollectionCardinality  | AcceptsSpanFormattable | AllOutputConditionsMask, Pretty),
                """
                [
                  "16.0.0"
                ]
                """ 
            }
        }
    };
}
