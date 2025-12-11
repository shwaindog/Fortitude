// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestDictionaries;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.Dictionaries;

public partial class SimpleDictTestData
{
    private static PositionUpdatingList<IKeyedCollectionExpect>? allSubListFilteredSimpleKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllSubListFilteredDictExpectations =>
        allSubListFilteredSimpleKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(SimpleDictTestData))
            {
                // Version Collections (non null class - json as string)
                new KeyedSubListDictionaryExpect<bool, int>([], () => Bool_True_SubList, name: "Empty_Filtered")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<bool, int>(null, () => Bool_False_SubList)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<bool, int>(BoolIntMap.ToList(), "0x{0}", "{0[..1]}", name: "AllKeysSubList")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ t: 0x1, f: 0x0 }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"t\":0x1,\"f\":0x0}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          t: 0x1,
                          f: 0x0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "t": 0x1,
                          "f": 0x0
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<bool?, int?>(NullBoolNullIntKvpList.ToList(), "0x{0:000#}", "'{0[..1]}'"
                                                            , () => Bool_NullOrFalse_SubList, name: "AllKeysSubList")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ 'null': 0x0000, 'f': null }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"'null'\": 0x0000, \"'f'\": null}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          'null': 0x0000,
                          'f': null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "'null'": 0x0000,
                          "'f'": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>([], () => Double_First_4_SubList
                                                                      , name: "Empty_DoubleSubList")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>(null, () => Double_First_4_SubList)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>(DoubleCharSequenceMap.ToList(),
                                                                        () => Double_First_4_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                        """
                        {
                         3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                         6.283185307179586: "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                         2.718281828459045: "One doesn't simply write Euler nature number.",
                         5.43656365691809: "One doesn't even appear at the start of Euler nature number." 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                        """
                        {
                        "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                        "6.283185307179586":"You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                        "2.718281828459045":"One doesn't simply write Euler nature number.",
                        "5.43656365691809":"One doesn't even appear at the start of Euler nature number."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                        """
                        {
                          3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                          6.283185307179586: "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          2.718281828459045: "One doesn't simply write Euler nature number.",
                          5.43656365691809: "One doesn't even appear at the start of Euler nature number."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                        """
                        {
                          "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                          "6.283185307179586":"You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          "2.718281828459045":"One doesn't simply write Euler nature number.",
                          "5.43656365691809":"One doesn't even appear at the start of Euler nature number."
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>(DoubleCharSequenceMap.ToList(), "", "{0,-17}"
                                                                       , () => Double_Second_4_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         8.539734222673566: "Oiler and Euler are very different things.",
                         1                : "All for one and one for all.",
                         -1               : "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "8.539734222673566":"Oiler and Euler are very different things.",
                        "1                ":"All for one and one for all.",
                        "-1               ":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          8.539734222673566: "Oiler and Euler are very different things.",
                          1                : "All for one and one for all.",
                          -1               : "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "8.539734222673566":"Oiler and Euler are very different things.",
                          "1                ":"All for one and one for all.",
                          "-1               ":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new KeyedSubListDictionaryExpect<double?, ICharSequence?>(NullDoubleNullCharSequence, "", "|{0,-17}|"
                                                                        , () => NullDouble_NullOrNeg1_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          |null            |: null,
                          |null            |: ""Your contract is null AND void", apparently not the same thing or why say both.",
                          |-1              |: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "|null            |": null,
                        "|null            |": "\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                        "|-1              |": "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          |null            |: null,
                          |null            |: ""Your contract is null AND void", apparently not the same thing or why say both.",
                          |-1              |: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "|null            |": null,
                          "|null            |": "\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                          "|-1              |": "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new KeyedSubListDictionaryExpect<UInt128, BigInteger>(VeryULongBigIntegerMap.ToList(), "{0:###,##0.0}", "'{0}'", () => VeryULong_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         '0': 0.0,
                         '170141183460469231731687303715884105727': 170,141,183,460,469,231,731,687,303,715,884,105,727.0,
                         '1': 1.0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "'0'": 0.0,
                        "'170141183460469231731687303715884105727'": 170,141,183,460,469,231,731,687,303,715,884,105,727.0,
                        "'1'": 1.0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          '0': 0.0,
                          '170141183460469231731687303715884105727': 170,141,183,460,469,231,731,687,303,715,884,105,727.0,
                          '1': 1.0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "'0'": 0.0,
                          "'170141183460469231731687303715884105727'": 170,141,183,460,469,231,731,687,303,715,884,105,727.0,
                          "'1'": 1.0
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new KeyedSubListDictionaryExpect<UInt128, BigInteger?>(NullVeryULongBigIntegerMap, "{0,45}","\"{0,-45}\""
                                                                       , () => VeryULong_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                         "85070591730234615865843651857942052863       ":                                          null,
                         "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                        "85070591730234615865843651857942052863       ":                                          null,
                        "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                          "85070591730234615865843651857942052863       ":                                          null,
                          "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                          "85070591730234615865843651857942052863       ":                                          null,
                          "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.RemoveLineEndings()
                    }
                }
            };
}
