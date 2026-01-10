// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ValueTypeScaffolds;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestDictionaries;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;

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
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<bool, int>(null, () => Bool_False_SubList)
                {
                    { new EK(IsKeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
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
              , new KeyedNullStructValueSubListDictionaryExpect<bool, int>(BoolNullIntKvpList.ToList(), "0x{0:000#}", "'{0[..1]}'"
                                                            , () => Bool_TrueFalse_SubList, name: "AllKeysSubList")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ 't': 0x0001, 'f': 0xnull }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"'t'\":0x0001,\"'f'\":0xnull}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          't': 0x0001,
                          'f': 0xnull
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "'t'": 0x0001,
                          "'f'": 0xnull
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>([], () => Double_First_4_SubList
                                                                      , name: "Empty_DoubleSubList")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence>(null, () => Double_First_4_SubList)
                {
                    { new EK(IsKeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                        """
                        {
                          "3.141592653589793": "Eating the crust edges of one pie means you have eaten the length of two pi",
                          "6.283185307179586": "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          "2.718281828459045": "One doesn't simply write Euler nature number.",
                          "5.43656365691809": "One doesn't even appear at the start of Euler nature number."
                        }
                        """.Dos2Unix()
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "8.539734222673566": "Oiler and Euler are very different things.",
                          "1                ": "All for one and one for all.",
                          "-1               ": "Imagine there's no tax havens, it's easy if you try"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<double, ICharSequence?>(DoubleNullCharSequence, "", "|{0,-17}|"
                                                                        , () => NullDouble_NullOrNeg1_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         |-1               |: null,
                         |3.333            |: ""Your contract is null AND void", apparently not the same thing or why say both.",
                         |1                |: "All for one and one for all.",
                         |0                |: null,
                         |2.718281828459045|: "One doesn't simply write Euler nature number.",
                         |3.141592653589793|: "Eating the crust edges of one pie means you have eaten the length of two pi" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "|-1               |":null,
                        "|3.333            |":"\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                        "|1                |":"All for one and one for all.",
                        "|0                |":null,
                        "|2.718281828459045|":"One doesn't simply write Euler nature number.",
                        "|3.141592653589793|":"Eating the crust edges of one pie means you have eaten the length of two pi"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          |-1               |: null,
                          |3.333            |: ""Your contract is null AND void", apparently not the same thing or why say both.",
                          |1                |: "All for one and one for all.",
                          |0                |: null,
                          |2.718281828459045|: "One doesn't simply write Euler nature number.",
                          |3.141592653589793|: "Eating the crust edges of one pie means you have eaten the length of two pi"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "|-1               |": null,
                          "|3.333            |": "\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                          "|1                |": "All for one and one for all.",
                          "|0                |": null,
                          "|2.718281828459045|": "One doesn't simply write Euler nature number.",
                          "|3.141592653589793|": "Eating the crust edges of one pie means you have eaten the length of two pi"
                        }
                        """.Dos2Unix()
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
                        "'0'":"0.0",
                        "'170141183460469231731687303715884105727'":"170,141,183,460,469,231,731,687,303,715,884,105,727.0",
                        "'1'":"1.0"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "'0'": "0.0",
                          "'170141183460469231731687303715884105727'": "170,141,183,460,469,231,731,687,303,715,884,105,727.0",
                          "'1'": "1.0"
                        }
                        """.Dos2Unix()
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
                        "113427455640312821154458202477256070485      ":"      113427455640312821154458202477256070485",
                        "85070591730234615865843651857942052863       ":                                         null,
                        "340282366920938463463374607431768211455      ":"      340282366920938463463374607431768211455"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "113427455640312821154458202477256070485      ": "      113427455640312821154458202477256070485",
                          "85070591730234615865843651857942052863       ":                                          null,
                          "340282366920938463463374607431768211455      ": "      340282366920938463463374607431768211455"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<IPAddress, Uri>(IPAddressUriMap.ToList(), "==> {0}", "{0,18}")
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
                        "           0.0.0.0":"==\u003e http://first-null.com/",
                        "         127.0.0.1":"==\u003e tcp://localhost/",
                        "   255.255.255.255":"==\u003e http://unknown.com/",
                        "       192.168.1.1":"==\u003e tcp://default-gateway/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                                     0.0.0.0: ==> http://first-null.com/,
                                   127.0.0.1: ==> tcp://localhost/,
                             255.255.255.255: ==> http://unknown.com/,
                                 192.168.1.1: ==> tcp://default-gateway/
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "           0.0.0.0": "==\u003e http://first-null.com/",
                          "         127.0.0.1": "==\u003e tcp://localhost/",
                          "   255.255.255.255": "==\u003e http://unknown.com/",
                          "       192.168.1.1": "==\u003e tcp://default-gateway/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<IPAddress, Uri?>(IPAddressNullUriMap, "==> {0}", "{0,18}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                    0.0.0.0: ==> null,
                                  127.0.0.1: ==> tcp://localhost/,
                                192.168.1.1: ==> tcp://default-gateway/,
                            255.255.255.255: ==> null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "           0.0.0.0":==\u003e null,
                        "         127.0.0.1":"==\u003e tcp://localhost/",
                        "       192.168.1.1":"==\u003e tcp://default-gateway/",
                        "   255.255.255.255":==\u003e null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                                     0.0.0.0: ==> null,
                                   127.0.0.1: ==> tcp://localhost/,
                                 192.168.1.1: ==> tcp://default-gateway/,
                             255.255.255.255: ==> null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "           0.0.0.0": ==\u003e null,
                          "         127.0.0.1": "==\u003e tcp://localhost/",
                          "       192.168.1.1": "==\u003e tcp://default-gateway/",
                          "   255.255.255.255": ==\u003e null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<MySpanFormattableStruct, MySpanFormattableClass>
                    (MySpanFormattableStructClassMap.ToList(), "{0,-20}", "{0,20}", () => MySpanFormattableStruct_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                             First_SpanStruct: First_SpanClass     ,
                            Second_SpanStruct: Second_SpanClass    ,
                             Third_SpanStruct: Third_SpanClass      
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "    First_SpanStruct":"First_SpanClass     ",
                        "   Second_SpanStruct":"Second_SpanClass    ",
                        "    Third_SpanStruct":"Third_SpanClass     "
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                              First_SpanStruct: First_SpanClass     ,
                             Second_SpanStruct: Second_SpanClass    ,
                              Third_SpanStruct: Third_SpanClass     
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "    First_SpanStruct": "First_SpanClass     ",
                          "   Second_SpanStruct": "Second_SpanClass    ",
                          "    Third_SpanStruct": "Third_SpanClass     "
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<MySpanFormattableStruct, MySpanFormattableClass?>
                    (MySpanFormattableStructNullClassMap, "{0,20}", "{0,-20}", () => MySpanFormattableStruct_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         Fourth_SpanStruct   :                 null,
                         Fifth_SpanStruct    :                 null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Fourth_SpanStruct   ":                null,
                        "Fifth_SpanStruct    ":                null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          Fourth_SpanStruct   :                 null,
                          Fifth_SpanStruct    :                 null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "Fourth_SpanStruct   ":                 null,
                          "Fifth_SpanStruct    ":                 null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableClassStructMap.ToList(), "{0,-20}", "{0,20}", () => MySpanFormattableClass_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                             Fourth_SpanClass: Fourth_SpanStruct   ,
                              Fifth_SpanClass: Fifth_SpanStruct     
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "    Fourth_SpanClass":"Fourth_SpanStruct   ",
                        "     Fifth_SpanClass":"Fifth_SpanStruct    "
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                              Fourth_SpanClass: Fourth_SpanStruct   ,
                               Fifth_SpanClass: Fifth_SpanStruct    
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "    Fourth_SpanClass": "Fourth_SpanStruct   ",
                          "     Fifth_SpanClass": "Fifth_SpanStruct    "
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableNullClassStructMap.ToList(), "{0,20}", "{0,-20}", () => MySpanFormattableNullClass_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         Fourth_SpanClass    :                 null,
                         Fifth_SpanClass     :     Fifth_SpanStruct 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Fourth_SpanClass    ":                null,
                        "Fifth_SpanClass     ":"    Fifth_SpanStruct"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          Fourth_SpanClass    :                 null,
                          Fifth_SpanClass     :     Fifth_SpanStruct
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "Fourth_SpanClass    ":                 null,
                          "Fifth_SpanClass     ": "    Fifth_SpanStruct"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<ComplexStructContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => StructBearer_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexStructContentAsValueSpanFormattable<decimal>
                         {
                         SpanFormattableComplexStructContentAsValue: 27.183
                         }:
                         FieldSpanFormattableAlwaysAddStructStringBearer<Uri>
                         {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/
                         } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "27.183":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://fourth-value.com/"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 27.183
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "27.183": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://fourth-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<ComplexStructContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToNullComplexStructBearerMap, "{0,30}", "N3", () => StructBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexStructContentAsValueSpanFormattable<decimal>
                         {
                         SpanFormattableComplexStructContentAsValue: 3.142
                         }:
                         FieldSpanFormattableAlwaysAddStructStringBearer<Uri>
                         {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/
                         },
                         ComplexStructContentAsValueSpanFormattable<decimal>
                         {
                         SpanFormattableComplexStructContentAsValue: 2.718
                         }:
                         FieldSpanFormattableAlwaysAddStructStringBearer<Uri>
                         {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/
                         },
                         ComplexStructContentAsValueSpanFormattable<decimal>
                         {
                         SpanFormattableComplexStructContentAsValue: 31.416
                         }:
                         FieldSpanFormattableAlwaysAddStructStringBearer<Uri>
                         {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/
                         } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "3.142":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://first-value.com/"},
                        "2.718":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://second-value.com/"},
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://third-value.com/"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 3.142
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/
                          },
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/
                          },
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "3.142": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://first-value.com/"
                          },
                          "2.718": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://second-value.com/"
                          },
                          "31.416": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://third-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<ComplexContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStringBearer<Uri>>
                    (ClassBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => ClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 3.142 }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/ },
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/ },
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "3.142":{"ComplexTypeFieldAlwaysAddSpanFormattable":"http://first-value.com/"},
                        "2.718":{"ComplexTypeFieldAlwaysAddSpanFormattable":"http://second-value.com/"},
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattable":"http://third-value.com/"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 3.142
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/
                          },
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/
                          },
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "3.142": {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": "http://first-value.com/"
                          },
                          "2.718": {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": "http://second-value.com/"
                          },
                          "31.416": {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": "http://third-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<ComplexContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (ClassBearerToNullStructComplexBearerMap, "{0,30}", "N3", () => ClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 3.142 }:                           null,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ },
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "3.142":                          null,
                        "2.718":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://second-value.com/"},
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://third-value.com/"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 3.142
                          }:                           null,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/
                          },
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "3.142":                           null,
                          "2.718": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://second-value.com/"
                          },
                          "31.416": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://third-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToWdWfMap.ToList(), "", null, () => EnumLongNdNfToWdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_1: WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_3
                         | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask
                         | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                         NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13 | WithDefaultLongWithFlagsEnum.WDLWFE_23,
                         NoDefaultLongNoFlagsEnum.NDLNFE_2: WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_5 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLNFE_1":"WDLWFE_2, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask",
                        "NDLNFE_13":"WDLWFE_13, WDLWFE_23",
                        "NDLNFE_2":"WDLWFE_2, WDLWFE_5"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongNoFlagsEnum.NDLNFE_1: WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask,
                          NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13 | WithDefaultLongWithFlagsEnum.WDLWFE_23,
                          NoDefaultLongNoFlagsEnum.NDLNFE_2: WithDefaultLongWithFlagsEnum.WDLWFE_2 | WithDefaultLongWithFlagsEnum.WDLWFE_5
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLNFE_1": "WDLWFE_2, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask",
                          "NDLNFE_13": "WDLWFE_13, WDLWFE_23",
                          "NDLNFE_2": "WDLWFE_2, WDLWFE_5"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToNullWdWfMap, null, "", () => EnumLongNdNfToWdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_1: null,
                         NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLNFE_1":null,
                        "NDLNFE_13":"WDLWFE_13"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongNoFlagsEnum.NDLNFE_1: null,
                          NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLNFE_1": null,
                          "NDLNFE_13": "WDLWFE_13"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToWdwfMap.ToList(), "", null, () => EnumULongNdNfDateTime_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongNoFlagsEnum.NDUNFE_4: WithDefaultULongWithFlagsEnum.WDUWFE_4,
                         NoDefaultULongNoFlagsEnum.NDUNFE_34: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                         NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUNFE_4":"WDUWFE_4",
                        "NDUNFE_34":"WDUWFE_34",
                        "0":"Default"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultULongNoFlagsEnum.NDUNFE_4: WithDefaultULongWithFlagsEnum.WDUWFE_4,
                          NoDefaultULongNoFlagsEnum.NDUNFE_34: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                          NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUNFE_4": "WDUWFE_4",
                          "NDUNFE_34": "WDUWFE_34",
                          "0": "Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToNullWdWfMap, "", null, () => EnumULongNdNfDateTime_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongNoFlagsEnum.NDUNFE_4: WithDefaultULongWithFlagsEnum.WDUWFE_4,
                         NoDefaultULongNoFlagsEnum.NDUNFE_34: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                         NoDefaultULongNoFlagsEnum.0: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUNFE_4":"WDUWFE_4",
                        "NDUNFE_34":"WDUWFE_34",
                        "0":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultULongNoFlagsEnum.NDUNFE_4: WithDefaultULongWithFlagsEnum.WDUWFE_4,
                          NoDefaultULongNoFlagsEnum.NDUNFE_34: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                          NoDefaultULongNoFlagsEnum.0: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUNFE_4": "WDUWFE_4",
                          "NDUNFE_34": "WDUWFE_34",
                          "0": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNdWfMap.ToList(), "", null, () => EnumLongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_1: NoDefaultLongWithFlagsEnum.NDLWFE_1,
                         WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2,
                         WithDefaultLongNoFlagsEnum.WDLNFE_3: NoDefaultLongWithFlagsEnum.NDLWFE_3 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_1":"NDLWFE_1",
                        "WDLNFE_2":"NDLWFE_2",
                        "WDLNFE_3":"NDLWFE_3"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_1: NoDefaultLongWithFlagsEnum.NDLWFE_1,
                          WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2,
                          WithDefaultLongNoFlagsEnum.WDLNFE_3: NoDefaultLongWithFlagsEnum.NDLWFE_3
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLNFE_1": "NDLWFE_1",
                          "WDLNFE_2": "NDLWFE_2",
                          "WDLNFE_3": "NDLWFE_3"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNullNdWfMap, "", null, () => EnumLongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                         WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_1":null,
                        "WDLNFE_2":"NDLWFE_2"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                          WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLNFE_1": null,
                          "WDLNFE_2": "NDLWFE_2"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNdWfMap.ToList(), "", null, () => EnumULongWdNfToNdWf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.WDUNFE_2: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                         WithDefaultULongNoFlagsEnum.WDUNFE_4: NoDefaultULongWithFlagsEnum.NDUWFE_4,
                         WithDefaultULongNoFlagsEnum.WDUNFE_34: NoDefaultULongWithFlagsEnum.NDUWFE_34 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUNFE_2":"NDUWFE_2",
                        "WDUNFE_4":"NDUWFE_4",
                        "WDUNFE_34":"NDUWFE_34"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultULongNoFlagsEnum.WDUNFE_2: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                          WithDefaultULongNoFlagsEnum.WDUNFE_4: NoDefaultULongWithFlagsEnum.NDUWFE_4,
                          WithDefaultULongNoFlagsEnum.WDUNFE_34: NoDefaultULongWithFlagsEnum.NDUWFE_34
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDUNFE_2": "NDUWFE_2",
                          "WDUNFE_4": "NDUWFE_4",
                          "WDUNFE_34": "NDUWFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNullNdWfMap, "", null, () => EnumULongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                         WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Default":0,
                        "WDUNFE_13":"NDUWFE_13"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                          WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "Default": 0,
                          "WDUNFE_13": "NDUWFE_13"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToWdNfMap.ToList(), "", null, () => EnumLongNdWfToWdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                         | NoDefaultLongWithFlagsEnum.NDLWFE_8: WithDefaultLongNoFlagsEnum.WDLNFE_6,
                         NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLWFE_4":"WDLNFE_4",
                        "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8":"WDLNFE_6",
                        "0":"Default"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: WithDefaultLongNoFlagsEnum.WDLNFE_6,
                          NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLWFE_4": "WDLNFE_4",
                          "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8": "WDLNFE_6",
                          "0": "Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToNullWdNfMap, "", null, () => EnumLongNdWfToWdNf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: null,
                         NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                         NoDefaultLongWithFlagsEnum.NDLWFE_34: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLWFE_First8Mask, NDLWFE_LastTwoMask":null,
                        "NDLWFE_22":"WDLNFE_22",
                        "NDLWFE_34":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: null,
                          NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                          NoDefaultLongWithFlagsEnum.NDLWFE_34: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLWFE_First8Mask, NDLWFE_LastTwoMask": null,
                          "NDLWFE_22": "WDLNFE_22",
                          "NDLWFE_34": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToWdNfMap.ToList(), "", null, () => EnumULongNdWfToWdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongWithFlagsEnum.NDUWFE_4: WithDefaultULongNoFlagsEnum.WDUNFE_4,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                         | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                         NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUWFE_4":"WDUNFE_4",
                        "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8":"WDUNFE_1",
                        "NDUWFE_34":"WDUNFE_34"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultULongWithFlagsEnum.NDUWFE_4: WithDefaultULongNoFlagsEnum.WDUNFE_4,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                          NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUWFE_4": "WDUNFE_4",
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
                          "NDUWFE_34": "WDUNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToNullWdNfMap, "", null, () => EnumULongNdWfToWdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongWithFlagsEnum.NDUWFE_4: WithDefaultULongNoFlagsEnum.WDUNFE_4,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                         | NoDefaultULongWithFlagsEnum.NDUWFE_8: null,
                         NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUWFE_4":"WDUNFE_4",
                        "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8":null,
                        "NDUWFE_34":"WDUNFE_34"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultULongWithFlagsEnum.NDUWFE_4: WithDefaultULongNoFlagsEnum.WDUNFE_4,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8: null,
                          NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUWFE_4": "WDUNFE_4",
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": null,
                          "NDUWFE_34": "WDUNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNdNfMap.ToList(), "", null, () => EnumLongWdWfToNdNf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask: NoDefaultLongNoFlagsEnum.NDLNFE_6,
                         WithDefaultLongWithFlagsEnum.WDLWFE_22: NoDefaultLongNoFlagsEnum.NDLNFE_22,
                         WithDefaultLongWithFlagsEnum.WDLWFE_32: NoDefaultLongNoFlagsEnum.NDLNFE_32 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLWFE_First8Mask, WDLWFE_LastTwoMask":"NDLNFE_6",
                        "WDLWFE_22":"NDLNFE_22",
                        "WDLWFE_32":"NDLNFE_32"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask: NoDefaultLongNoFlagsEnum.NDLNFE_6,
                          WithDefaultLongWithFlagsEnum.WDLWFE_22: NoDefaultLongNoFlagsEnum.NDLNFE_22,
                          WithDefaultLongWithFlagsEnum.WDLWFE_32: NoDefaultLongNoFlagsEnum.NDLNFE_32
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLWFE_First8Mask, WDLWFE_LastTwoMask": "NDLNFE_6",
                          "WDLWFE_22": "NDLNFE_22",
                          "WDLWFE_32": "NDLNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNullNdNfMap, "", null, () => EnumLongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongWithFlagsEnum.WDLWFE_4: NoDefaultLongNoFlagsEnum.NDLNFE_4,
                         WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4
                         | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                         WithDefaultLongWithFlagsEnum.Default: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLWFE_4":"NDLNFE_4",
                        "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask":"NDLNFE_8",
                        "Default":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongWithFlagsEnum.WDLWFE_4: NoDefaultLongNoFlagsEnum.NDLNFE_4,
                          WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                          WithDefaultLongWithFlagsEnum.Default: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLWFE_4": "NDLNFE_4",
                          "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask": "NDLNFE_8",
                          "Default": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNdNfMap.ToList(), "", null, () => EnumULongWdWfToNdNf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask: NoDefaultULongNoFlagsEnum.NDUNFE_6,
                         WithDefaultULongWithFlagsEnum.WDUWFE_22: NoDefaultULongNoFlagsEnum.NDUNFE_22,
                         WithDefaultULongWithFlagsEnum.WDUWFE_32: NoDefaultULongNoFlagsEnum.NDUNFE_32 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUWFE_First8Mask, WDUWFE_LastTwoMask":"NDUNFE_6",
                        "WDUWFE_22":"NDUNFE_22",
                        "WDUWFE_32":"NDUNFE_32"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask: NoDefaultULongNoFlagsEnum.NDUNFE_6,
                          WithDefaultULongWithFlagsEnum.WDUWFE_22: NoDefaultULongNoFlagsEnum.NDUNFE_22,
                          WithDefaultULongWithFlagsEnum.WDUWFE_32: NoDefaultULongNoFlagsEnum.NDUNFE_32
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDUWFE_First8Mask, WDUWFE_LastTwoMask": "NDUNFE_6",
                          "WDUWFE_22": "NDUNFE_22",
                          "WDUWFE_32": "NDUNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedNullStructValueSubListDictionaryExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNullNdNfMap, "", null, () => EnumULongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongWithFlagsEnum.WDUWFE_4: null,
                         WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4
                         | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                         WithDefaultULongWithFlagsEnum.Default: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUWFE_4":null,
                        "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask":"NDUNFE_8",
                        "Default":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultULongWithFlagsEnum.WDUWFE_4: null,
                          WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDUWFE_4": null,
                          "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask": "NDUNFE_8",
                          "Default": null
                        }
                        """.Dos2Unix()
                    }
                }
            };
}
