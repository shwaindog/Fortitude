// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
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
    private static PositionUpdatingList<IKeyedCollectionExpect>? allPredicateFilteredSimpleKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllPredicateFilteredSimpleDictExpectations =>
        allPredicateFilteredSimpleKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(SimpleDictTestData))
            {
                // Version Collections (non null class - json as string)
                new DictionaryExpect<bool, int>([], () => BoolIntMap_First_1, name: "Empty_Filtered")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<bool, int>(null, () => BoolIntMap_First_1)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<bool, int>(BoolIntMap.ToList(), "0x{0}", "{0[..1]}"
                                                   , () => BoolIntMap_First_1)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ t: 0x1 }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"t\":0x1}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                        """
                        {
                          t: 0x1 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                        """
                        {
                          "t": 0x1
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<bool, int>(BoolIntMap.ToList(), "\"{0}\"", "\"{0}\""
                                                   , () => BoolIntMap_Second_1)
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
              , new DictionaryExpect<bool?, int?>(NullBoolNullIntKvpList.ToList(), () => NullBoolIntMap_First_1, 
                                                  name: "NullAll_NoFilter")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ null: 0 }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"null\":0}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: 0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<double, ICharSequence>(DoubleCharSequenceMap.ToList()
                                                               , () => DoubleCharSequenceMap_First_4)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                          "6.283185307179586":"You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          "2.718281828459045":"One doesn't simply write Euler nature number.",
                          "5.43656365691809":"One doesn't even appear at the start of Euler nature number."
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<double, ICharSequence>(DoubleCharSequenceMap.ToList(), "", "{0,17}"
                                                               , () => DoubleCharSequenceMap_Second_4)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         8.539734222673566: "Oiler and Euler are very different things.",
                                         1: "All for one and one for all.",
                                        -1: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "8.539734222673566":"Oiler and Euler are very different things.",
                        "                1":"All for one and one for all.",
                        "               -1":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          8.539734222673566: "Oiler and Euler are very different things.",
                                          1: "All for one and one for all.",
                                         -1: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "8.539734222673566":"Oiler and Euler are very different things.",
                          "                1":"All for one and one for all.",
                          "               -1":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<double?, ICharSequence?>(NullDoubleNullCharSequence.ToList()
                                                               , () => NullDoubleCharSequenceMap_First_4)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                         2.718281828459045: "One doesn't simply write Euler nature number.",
                         1: "All for one and one for all." 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":"null",
                        "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                        "2.718281828459045":"One doesn't simply write Euler nature number.",
                        "1": "All for one and one for all."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: null,
                          3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                          2.718281828459045: "One doesn't simply write Euler nature number.",
                          1: "All for one and one for all."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          null: null,
                          "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                          "2.718281828459045":"One doesn't simply write Euler nature number.",
                          "1": "All for one and one for all."
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<double?, ICharSequence?>(NullDoubleNullCharSequence, "", "{0,17}"
                                                               , () => NullDoubleCharSequenceMap_Second_4)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                      null: ""Your contract is null AND void", apparently not the same thing or why say both.",
                                        -1: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "             null":""Your contract is null AND void", apparently not the same thing or why say both.",
                        "               -1":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                       null: ""Your contract is null AND void", apparently not the same thing or why say both."
                                         -1: "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "             null": ""Your contract is null AND void", apparently not the same thing or why say both.",
                          "               -1": "Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<UInt128, BigInteger>(VeryULongBigIntegerMap.ToList(), "-{0}", "'{0}'"
                                                        , () => VeryULongBigIntegerMap_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         '0': -0,
                         '170141183460469231731687303715884105727': -170141183460469231731687303715884105727,
                         '1': -1 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "'0'": -0,
                        "'170141183460469231731687303715884105727'": -170141183460469231731687303715884105727,
                        "'1'": -1
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          "'0'": -0,
                          "'170141183460469231731687303715884105727'": -170141183460469231731687303715884105727,
                          "'1'": -1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "'0'": -0,
                          "'170141183460469231731687303715884105727'": -170141183460469231731687303715884105727,
                          "'1'": -1
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<UInt128, BigInteger?>(NullVeryULongBigIntegerMap, "\"{0,4}\"","{0,-45}"
                                                        , () => NullVeryULongBigIntegerMap_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         0                                            : "   0",
                         170141183460469231731687303715884105727      : "null",
                         1                                            : "   1" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "0                                            ":"   0",
                        "170141183460469231731687303715884105727      ":"null",
                        "1                                            ":"   1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          "0                                            ":"   0",
                          "170141183460469231731687303715884105727      ":"null",
                          "1                                            ":"   1"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "0                                            ":"   0",
                          "170141183460469231731687303715884105727      ":"null",
                          "1                                            ":"   1"
                        }
                        """.RemoveLineEndings()
                    }
                }
              , new DictionaryExpect<IPAddress, Uri>(IPAddressUriMap.ToList(), "==> {0}", "{0,18}", () => IPAddress_First_10)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                    0.0.0.0: ==> http://first-null.com/,
                                  127.0.0.1: ==> tcp://localhost/,
                            255.255.255.255: ==> http://unknown.com/,
                                192.168.1.1: ==> tcp://default-gateway/ 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "        0.0.0.0": ==> http://first-null.com/,
                        "      127.0.0.1": ==> tcp://localhost/,
                        "255.255.255.255": ==> http://unknown.com/,
                        "    192.168.1.1": ==> tcp://default-gateway/
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          "        0.0.0.0": ==> http://first-null.com/,
                          "      127.0.0.1": ==> tcp://localhost/,
                          "255.255.255.255": ==> http://unknown.com/,
                          "    192.168.1.1": ==> tcp://default-gateway/
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "        0.0.0.0": ==> http://first-null.com/,
                          "      127.0.0.1": ==> tcp://localhost/,
                          "255.255.255.255": ==> http://unknown.com/,
                          "    192.168.1.1": ==> tcp://default-gateway/
                        }
                        """.RemoveLineEndings()
                    }
                }
            };
}
