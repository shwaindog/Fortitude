// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

// ReSharper disable FormatStringProblem

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.OrderedCollectionFieldsTypes;

public static class BoolCollectionsTestData
{
    private static PositionUpdatingList<IOrderedListExpect>? allBoolCollectionExpectations;  
    
    public static PositionUpdatingList<IOrderedListExpect> AllBoolCollectionExpectations => allBoolCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(BoolCollectionsTestData))
        {
        // bool Collections
        new OrderedListExpect<bool>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsStruct | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<bool>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
          , { new EK(AcceptsStruct | AlwaysWrites), "null" }
          , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<bool>(BoolList, "", name: "All_NoFilter")
        {
            { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ true, true, false, true, false, false, false, true, true, false, false, true, true, true ]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[true,true,false,true,false,false,false,true,true,false,false,true,true,true]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
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
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool>(BoolList, "", () => Bool_First_8)
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ true, true, false, true, false, false, false, true ]" }
          , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[true,true,false,true,false,false,false,true]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  true,
                  true,
                  false,
                  true,
                  false,
                  false,
                  false,
                  true
                ]
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool>(BoolList, "", () => Bool_First_False)
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ false ]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[false]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  false
                ]
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool>(BoolList, "\"{0,10}\"", name: "PadAndDelimited")
        {
            { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                """
                [ "      true", "      true", "     false", "      true", "     false", "     false", "     false", "      true", 
                "      true", "     false", "     false", "      true", "      true", "      true" ]
                """.RemoveLineEndings()
            }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                """
                ["      true","      true","     false","      true","     false","     false","     false","      true","      true",
                "     false","     false","      true","      true","      true"]
                """.RemoveLineEndings()
            }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
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
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[false,false,true,true,false]" }
          , { new EK( AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  false,
                  false,
                  true,
                  true,
                  false
                ]
                """.Dos2Unix() 
            }
        }
        
        // bool? Collections
      , new OrderedListExpect<bool?>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsNullableStruct | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsNullableStruct  | CallsAsSpan | CallsAsReadOnlySpan| AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<bool?>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
          , { new EK(AcceptsNullableStruct | AlwaysWrites), "null" }
          , { new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<bool?>(NullBoolList, "", name: "All_NoFilter")
        {
            { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ true, null, false, true, false, null, false, true, true, null, false, true, null, null ]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[true,null,false,true,false,null,false,true,true,null,false,true,null,null]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
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
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool?>(NullBoolList, "", () => NullBool_Skip_Odd_Index)
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ true, false, false, false, true, false, null ]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[true,false,false,false,true,false,null]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
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
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool?>(NullBoolList, "", () => NullBool_First_False)
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ false ]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[false]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  false
                ]
                """.Dos2Unix() 
            }
        }
      , new OrderedListExpect<bool?>(NullBoolList, "\"{0,10}\"", name: "PadAndDelimited")
        {
            { new EK(  AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                """
                [ "      true", "      null", "     false", "      true", "     false", "      null", "     false", "      true", 
                "      true", "      null", "     false", "      true", "      null", "      null" ]
                """.RemoveLineEndings()
            }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                """
                ["      true","      null","     false","      true","     false","      null","     false","      true",
                "      true","      null","     false","      true","      null","      null"]
                """.RemoveLineEndings()
            }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
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
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,false,true,true,null]" }
          , { new EK( AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  null,
                  false,
                  true,
                  true,
                  null
                ]
                """.Dos2Unix() 
            }
        }
    };
}
