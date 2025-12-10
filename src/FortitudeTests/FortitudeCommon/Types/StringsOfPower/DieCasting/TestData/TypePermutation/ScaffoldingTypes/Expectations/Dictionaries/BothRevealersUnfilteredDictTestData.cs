// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestDictionaries;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.Dictionaries;

public partial class BothRevealersDictTestData
{
    private static PositionUpdatingList<IKeyedCollectionExpect>? allBothRevealersUnfilteredKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllBothRevealersUnfilteredDictExpectations =>
        allBothRevealersUnfilteredKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(BothRevealersDictTestData))
            {
                // Version Collections (non null class - json as string)
                new BothRevealersDictExpect<bool, int>
                    ([], () => Int_NegativeString_Reveal, () => Bool_Reveal,
                     DefaultCallerTypeFlags, name: "Empty")
                    {
                        { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                      , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                      , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                    }
              , new BothRevealersDictExpect<bool, int>
                    (null, () => Int_NegativeString_Reveal, () => Bool_Reveal, DefaultCallerTypeFlags)
                    {
                        { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                      , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                      , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                    }
              , new BothRevealersDictExpect<bool, int>
                    (BoolIntMap.ToList(), () => Int_Money_Reveal, () => Bool_Reveal, DefaultCallerTypeFlags
                   , name: "All_NoFilter")
                    {
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                          , "{ true: $1.00, false: $0.00 }"
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                          , "{\"true\":$1.00,\"false\":$0.00}"
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                          , """
                            {
                              true: $1.00,
                              false: $0.00
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                          , """
                            {
                              "true": $1.00,
                              "false": $0.00
                            }
                            """.Dos2Unix()
                        }
                    }
              , new BothRevealersDictExpect<double, ICharSequence>
                    ([], () => CharSequenceMap_10Chars, () => Double_Reveal
                   , DefaultCallerTypeFlags, name: "Empty")
                    {
                        { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                      , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                      , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                    }
              , new BothRevealersDictExpect<double, ICharSequence>
                    (null, () => CharSequenceMap_10Chars, () => Double_Reveal, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new BothRevealersDictExpect<double, ICharSequence>
                    (DoubleCharSequenceMap.ToList(), () => CharSequenceMap_10Chars, () => Double_Reveal
                   , DefaultCallerTypeFlags, "All_NoFilter")
                    {
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                          , """
                            {
                             3.141592653589793: "Eating the",
                             6.283185307179586: "You have n",
                             2.718281828459045: "One doesn'",
                             5.43656365691809: "One doesn'",
                             8.539734222673566: "Oiler and ",
                             1: "All for on",
                             -1: "Imagine th" 
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                          , """
                            {
                            "3.141592653589793":"Eating the,
                            "6.283185307179586":"You have n",
                            "2.718281828459045":"One doesn'",
                            "5.43656365691809":"One doesn'",
                            "8.539734222673566":"Oiler and ",
                            "1":"All for on",
                            "-1":"Imagine th"
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                          , """
                            {
                              3.141592653589793: "Eating the",
                              6.283185307179586: "You have n",
                              2.718281828459045: "One doesn'",
                              5.43656365691809: "One doesn'",
                              8.539734222673566: "Oiler and ",
                              1: "All for on",
                              -1: "Imagine th" 
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                          , """
                            {
                              "3.141592653589793":"Eating the",
                              "6.283185307179586":"You have n",
                              "2.718281828459045":"One doesn'",
                              "5.43656365691809":"One doesn'",
                              "8.539734222673566":"Oiler and ",
                              "1":"All for on",
                              "-1":"Imagine th"
                            }
                            """.RemoveLineEndings()
                        }
                    }
            };
}
