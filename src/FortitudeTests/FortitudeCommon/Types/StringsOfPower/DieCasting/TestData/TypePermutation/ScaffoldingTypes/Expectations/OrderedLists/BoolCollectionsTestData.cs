// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

// ReSharper disable FormatStringProblem

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public static class BoolCollectionsTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allBoolCollectionExpectations;  
    
    public static PositionUpdatingList<IOrderedListExpect> AllBoolCollectionExpectations => allBoolCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(BoolCollectionsTestData))
        {
        // bool Collections
        new OrderedListExpect<bool>([],  "", name: "Empty")
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
      , new OrderedListExpect<bool>(BoolList, "", name: "All_NoFilter")
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
      , new OrderedListExpect<bool>(BoolList, "", () => Bool_First_8)
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
      , new OrderedListExpect<bool>(BoolList, "", () => Bool_First_False)
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
      , new OrderedListExpect<bool>(BoolList, "\"{0,10}\"", name: "PadAndDelimited")
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
      , new OrderedListExpect<bool>(BoolList, "", () => Bool_Second_5)
        {
            { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ false, false, true, true, false ]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson),
                "[false,false,true,true,false]" }
          , { new EK( CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  false,
                  false,
                  true,
                  true,
                  false
                ]
                """ 
            }
        }
        
        // bool? Collections
      , new OrderedListExpect<bool?>([],  "", name: "Empty")
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
      , new OrderedListExpect<bool?>(NullBoolList, "", name: "All_NoFilter")
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
      , new OrderedListExpect<bool?>(NullBoolList, "", () => NullBool_Skip_Odd_Index)
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
      , new OrderedListExpect<bool?>(NullBoolList, "", () => NullBool_First_False)
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
      , new OrderedListExpect<bool?>(NullBoolList, "\"{0,10}\"", name: "PadAndDelimited")
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
      , new OrderedListExpect<bool?>(NullBoolList, "", () => NullBool_Second_5)
        {
            { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ null, false, true, true, null ]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, CompactJson),
                "[null,false,true,true,null]" }
          , { new EK( CollectionCardinality  | AcceptsNullableStruct | AllOutputConditionsMask, Pretty),
                """
                [
                  null,
                  false,
                  true,
                  true,
                  null
                ]
                """ 
            }
        }
    };
}
