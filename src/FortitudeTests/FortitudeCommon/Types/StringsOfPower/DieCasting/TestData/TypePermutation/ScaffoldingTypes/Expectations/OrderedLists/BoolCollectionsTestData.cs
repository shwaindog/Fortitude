// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
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
        new OrderedListExpect<bool>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsStruct), "[]" }
          , { new EK(   AcceptsStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<bool>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsStruct | AlwaysWrites), "[]" }
          , { new EK(  AcceptsStruct | AlwaysWrites), "null" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
          , { new EK(  AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<bool>(TestCollections.BoolList, "")
        {
            { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ true, true, false, true, false, false, false, true, true, false, false, true, true, true ]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[true,true,false,true,false,false,false,true,true,false,false,true,true,true]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
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
      , new OrderedListExpect<bool>(TestCollections.BoolList, "", TestCollections.Bool_First_8)
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ true, true, false, true, false, false, false, true ]" }
          , { new EK(CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[true,true,false,true,false,false,false,true]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
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
                ]
                """ 
            }
        }
      , new OrderedListExpect<bool>(TestCollections.BoolList, "", TestCollections.Bool_First_False)
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ false ]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[false]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  false
                ]
                """ 
            }
        }
      , new OrderedListExpect<bool>(TestCollections.BoolList, "\"{0,10}\"")
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                """
                [ "      true", "      true", "     false", "      true", "     false", "     false", "     false", "      true", 
                "      true", "     false", "     false", "      true", "      true", "      true" ]
                """.RemoveLineEndings()
            }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                """
                ["      true","      true","     false","      true","     false","     false","     false","      true","      true",
                "     false","     false","      true","      true","      true"]
                """.ReplaceLineEndings()
            }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  "      true",
                  "      true",
                  "     false",
                  "      true",
                  "     false",
                  "     false",
                  "     false",
                  "      true",
                  "      true",
                  "     false",
                  "     false",
                  "      true",
                  "      true",
                  "      true"
                ]
                """.Dos2Unix() 
            }
        }
        
        // bool? Collections
      , new OrderedListExpect<bool?>([],  "")
        {
            { new EK(  OrderedCollectionType | AcceptsNullableStruct), "[]" }
         ,  { new EK(   AcceptsNullableStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
          , { new EK(   AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct |  AlwaysWrites | NonNullWrites, CompactJson), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct |  AlwaysWrites | NonNullWrites, Pretty), "[]" }
        }
      , new OrderedListExpect<bool?>(null,  "")
        {
            { new EK( OrderedCollectionType | AcceptsNullableStruct), "[]" }
          , { new EK(  AcceptsNullableStruct | AlwaysWrites), "null" }
          , { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AlwaysWrites, CompactJson), "null" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AlwaysWrites, Pretty), "null" }
        }
      , new OrderedListExpect<bool?>(TestCollections.NullBoolList, "")
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ true, null, false, true, false, null, false, true, true, null, false, true, null, null ]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                "[true,null,false,true,false,null,false,true,true,null,false,true,null,null]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
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
      , new OrderedListExpect<bool?>(TestCollections.NullBoolList, "", TestCollections.NullBool_Skip_Odd_Index)
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ true, false, false, false, true, false, null ]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                "[true,false,false,false,true,false,null]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  true,
                  false,
                  false,
                  false,
                  true,
                  false,
                  null
                ]
                """ 
            }
        }
      , new OrderedListExpect<bool?>(TestCollections.NullBoolList, "", TestCollections.NullBool_First_False)
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                "[ false ]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                "[false]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  false
                ]
                """ 
            }
        }
      , new OrderedListExpect<bool?>(TestCollections.NullBoolList, "\"{0,10}\"")
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan 
                  |  AllOutputConditionsMask, CompactLog),
                """
                [ "      true", "      null", "     false", "      true", "     false", "      null", "     false", "      true", 
                "      true", "      null", "     false", "      true", "      null", "      null" ]
                """.RemoveLineEndings()
            }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                """
                ["      true","      null","     false","      true","     false","     null","     false","      true","      true",
                "     false","      null","      true","      null","      null"]
                """.ReplaceLineEndings()
            }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  "      true",
                  "      null",
                  "     false",
                  "      true",
                  "     false",
                  "      null",
                  "     false",
                  "      true",
                  "      true",
                  "      null",
                  "     false",
                  "      true",
                  "      null",
                  "      null"
                ]
                """.Dos2Unix() 
            }
        }
    ]);
}
