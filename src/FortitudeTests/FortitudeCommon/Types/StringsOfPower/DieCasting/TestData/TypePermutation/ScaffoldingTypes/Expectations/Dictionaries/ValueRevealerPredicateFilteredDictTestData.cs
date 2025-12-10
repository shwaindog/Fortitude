// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.Dictionaries;

public partial class ValueRevealerDictTestData
{
    
    private static PositionUpdatingList<IKeyedCollectionExpect>? allPredicateFilteredSimpleKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllPredicateFilteredDictExpectations =>
        allPredicateFilteredSimpleKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(ValueRevealerDictTestData))
            {
                // Version Collections (non null class - json as string)
                new ValueRevealerDictExpect<bool, int>([],() => TestDictionaries.Int_NegativeString_Reveal
                                                                , () => TestDictionaries.BoolIntMap_First_1, name: "Empty_Filtered")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<bool, int>(null,() => TestDictionaries.Int_NegativeString_Reveal,
                                                                  () => TestDictionaries.BoolIntMap_First_1)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<bool, int>(TestDictionaries.BoolIntMap.ToList(), () => TestDictionaries.Int_Money_Reveal
                                                                , "{0[..1]}" , () => TestDictionaries.BoolIntMap_First_1)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ t: $1.00 }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"t\":$1.00}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          t: $1.00 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                        """
                        {
                          "t": $1.00
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<bool, int>
                    (TestDictionaries.BoolIntMap.ToList(), () => TestDictionaries.Int_NegativeString_Reveal
                   , "\"{0}\"", () => TestDictionaries.BoolIntMap_Second_1)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ \"false\": \"0\" }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"false\":\"0\"}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                        """
                        {
                          "false": "0"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                        """
                        {
                          "false": "0"
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>
                    ( TestDictionaries.DoubleCharSequenceMap.ToList(), () => TestDictionaries.CharSequenceMap_Last10Chars, "N1"
                   , () => TestDictionaries.DoubleCharSequenceMap_First_4)
                {
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                          , """
                            {
                             3.1: " of two pi",
                             6.3: "eel like 2",
                             2.7: "re number.",
                             5.4: "re number." 
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                          , """
                            {
                            "3.1":" of two pi",
                            "6.3":"eel like 2",
                            "2.7":"re number.",
                            "5.4":"re number."
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                          , """
                            {
                              3.1: " of two pi",
                              6.3: "eel like 2",
                              2.7: "re number.",
                              5.4: "re number."
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                          , """
                            {
                              "3.1":" of two pi",
                              "6.3":"eel like 2",
                              "2.7":"re number.",
                              "5.4":"re number."
                            }
                            """.RemoveLineEndings()
                        }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>(TestDictionaries.DoubleCharSequenceMap.ToList()
                                                                            , () => TestDictionaries.CharSequenceMap_Last10Chars, "{0,17}"
                                                                            , () => TestDictionaries.DoubleCharSequenceMap_Second_4)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         8.539734222673566: "nt things.",
                                         1: "e for all.",
                                        -1: "if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "8.539734222673566":"nt things.",
                        "                1":"e for all.",
                        "               -1":"if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          8.539734222673566: "nt things.",
                                          1: "e for all.",
                                         -1: "if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "8.539734222673566":"nt things.",
                          "                1":"e for all.",
                          "               -1":"if you try"
                        }
                        """.RemoveLineEndings()
                    }
                }
            };
}
