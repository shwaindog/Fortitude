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
    private static PositionUpdatingList<IKeyedCollectionExpect>? allValueRevealerUnfilteredKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllValueRevealerUnfilteredDictExpectations =>
        allValueRevealerUnfilteredKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(ValueRevealerDictTestData))
            {
                // Version Collections (non null class - json as string)
                new ValueRevealerDictExpect<bool, int>
                    ([], () => TestDictionaries.Int_NegativeString_Reveal, DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<bool, int>
                    (null, () => TestDictionaries.Int_NegativeString_Reveal, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<bool, int>
                    (TestDictionaries.BoolIntMap.ToList(), () => TestDictionaries.Int_Money_Reveal, DefaultCallerTypeFlags
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
              , new ValueRevealerDictExpect<double, ICharSequence>([], () => TestDictionaries.CharSequenceMap_10Chars, DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>(null, () => TestDictionaries.CharSequenceMap_10Chars, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>
                    (TestDictionaries.DoubleCharSequenceMap.ToList(), () => TestDictionaries.CharSequenceMap_10Chars, DefaultCallerTypeFlags, "All_NoFilter")
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
