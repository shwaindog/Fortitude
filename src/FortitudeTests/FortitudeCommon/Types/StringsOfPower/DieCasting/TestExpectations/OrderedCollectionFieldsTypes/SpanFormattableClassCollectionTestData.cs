// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

public class SpanFormattableClassCollectionTestData
{
    
    private static PositionUpdatingList<IOrderedListExpect>? spanFormattableClassCollectionsExpectations;

    public static PositionUpdatingList<IOrderedListExpect> SpanFormattableClassCollectionsExpectations => spanFormattableClassCollectionsExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(SpanFormattableClassCollectionTestData))
        {
            
        // Version Collections (non null class - json as string)
        new OrderedListExpect<Version>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<Version>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<Version>(VersionsList, "", name: "All_NoFilter")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 0.0, 0.1.1, 1.1.1.1, 2.1.123456, 4.2.25, 8.3.3.3, 0.4, 16.0.0, 32.2563.1000000.1 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"0.0\",\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"4.2.25\",\"8.3.3.3\",\"0.4\",\"16.0.0\",\"32.2563.1000000.1\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  0.0,
                  0.1.1,
                  1.1.1.1,
                  2.1.123456,
                  4.2.25,
                  8.3.3.3,
                  0.4,
                  16.0.0,
                  32.2563.1000000.1
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
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
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version>(VersionsList, null, () => Version_First_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 0.0, 0.1.1, 1.1.1.1, 2.1.123456, 4.2.25 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"0.0\",\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"4.2.25\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                    [
                      0.0,
                      0.1.1,
                      1.1.1.1,
                      2.1.123456,
                      4.2.25
                    ]
                    """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  "0.0",
                  "0.1.1",
                  "1.1.1.1",
                  "2.1.123456",
                  "4.2.25"
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version>(VersionsList, "\"{0,-10}\"", () => Version_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ \"0.0       \", \"0.1.1     \" ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"0.0       \",\"0.1.1     \"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog | Json),
                """
                [
                  "0.0       ",
                  "0.1.1     "
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version>(VersionsList, "", () => Version_First_MjrGt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 16.0.0 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"16.0.0\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  16.0.0
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  "16.0.0"
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version>(VersionsList, "", () => Version_Second_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 8.3.3.3, 0.4, 16.0.0, 32.2563.1000000.1 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"8.3.3.3\",\"0.4\",\"16.0.0\",\"32.2563.1000000.1\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  8.3.3.3,
                  0.4,
                  16.0.0,
                  32.2563.1000000.1
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  "8.3.3.3",
                  "0.4",
                  "16.0.0",
                  "32.2563.1000000.1"
                ]
                """.Dos2Unix()
            }
        }
        
        // Version Collections ( null class - json as string)
      , new OrderedListExpect<Version?>([],  "", name: "Empty")
        {
            { new EK(   IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "[]" }
           ,{ new EK(   AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<Version?>(null,  "")
        {
            { new EK( IsOrderedCollectionType | AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "", name: "All_NoFilter")
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog),
                "[ null, 0.0, null, 0.1.1, 1.1.1.1, 2.1.123456, 8.3.3.3, null, null, null, null, 16.0.0, 32.2563.1000000.1, null, null, null ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,\"0.0\",null,\"0.1.1\",\"1.1.1.1\",\"2.1.123456\",\"8.3.3.3\",null,null,null,null,\"16.0.0\",\"32.2563.1000000.1\",null,null,null]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  null,
                  0.0,
                  null,
                  0.1.1,
                  1.1.1.1,
                  2.1.123456,
                  8.3.3.3,
                  null,
                  null,
                  null,
                  null,
                  16.0.0,
                  32.2563.1000000.1,
                  null,
                  null,
                  null
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
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
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, null, () => NullVersion_First_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ null, 0.0, null, 0.1.1, 1.1.1.1 ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[null,\"0.0\",null,\"0.1.1\",\"1.1.1.1\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                    [
                      null,
                      0.0,
                      null,
                      0.1.1,
                      1.1.1.1
                    ]
                    """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  null,
                  "0.0",
                  null,
                  "0.1.1",
                  "1.1.1.1"
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "\'{0,10}\'", () => NullVersion_First_2)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ '      null', '       0.0' ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                    [
                      '      null',
                      '       0.0'
                    ]
                    """.Dos2Unix()
            }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "['      null',\"'       0.0'\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  '      null',
                  "'       0.0'"
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "", () => NullVersion_First_MjrGt_10)
        {
            { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask, CompactLog),
                "[ 16.0.0 ]" }
          , { new EK(AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"16.0.0\"]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  16.0.0
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  "16.0.0"
                ]
                """.Dos2Unix()
            }
        }
      , new OrderedListExpect<Version?>(NullVersionsList, "", () => NullVersion_Second_5)
        {
            { new EK(  AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan 
                    |  AllOutputConditionsMask, CompactLog),
                "[ 2.1.123456, 8.3.3.3, null, null, null ]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson),
                "[\"2.1.123456\",\"8.3.3.3\",null,null,null]" }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog),
                """
                [
                  2.1.123456,
                  8.3.3.3,
                  null,
                  null,
                  null
                ]
                """.Dos2Unix()
            }
          , { new EK( AcceptsSpanFormattable | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson),
                """
                [
                  "2.1.123456",
                  "8.3.3.3",
                  null,
                  null,
                  null
                ]
                """.Dos2Unix()
            }
        }
        };
}
