// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;
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
                        """.Dos2Unix()
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
                        "        0.0.0.0":"==> http://first-null.com/",
                        "      127.0.0.1":"==> tcp://localhost/",
                        "255.255.255.255":"==> http://unknown.com/",
                        "    192.168.1.1":"==> tcp://default-gateway/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "        0.0.0.0": "==> http://first-null.com/",
                          "      127.0.0.1": "==> tcp://localhost/",
                          "255.255.255.255": "==> http://unknown.com/",
                          "    192.168.1.1": "==> tcp://default-gateway/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<IPAddress?, Uri?>(NullIPAddressUriMap, "==> {0}", "{0,18}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                    0.0.0.0: ==> http://first-null.com/,
                                  127.0.0.1: ==> tcp://localhost/,
                                192.168.1.1: ==> tcp://default-gateway/,
                            255.255.255.255: ==> null,
                                       null: ==> null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "        0.0.0.0":"==> http://first-null.com/",
                        "      127.0.0.1":"==> tcp://localhost/",
                        "    192.168.1.1":"==> tcp://default-gateway/",
                        "255.255.255.255":==> null,
                        "           null":==> null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                  0.0.0.0: ==> http://first-null.com/,
                                127.0.0.1: ==> tcp://localhost/,
                              192.168.1.1: ==> tcp://default-gateway/,
                          255.255.255.255: ==> null,
                                     null: ==> null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "        0.0.0.0": "==> http://first-null.com/",
                          "      127.0.0.1": "==> tcp://localhost/",
                          "    192.168.1.1": "==> tcp://default-gateway/",
                          "255.255.255.255": ==> null,
                          "           null": ==> null 
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
                        "   First_SpanStruct":"First_SpanClass      ",
                        "  Second_SpanStruct":"Second_SpanClass     ",
                        "   Third_SpanStruct":"Third_SpanClass      "
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                First_SpanStruct: First_SpanClass      ,
                               Second_SpanStruct: Second_SpanClass     ,
                                Third_SpanStruct: Third_SpanClass      
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "   First_SpanStruct": "First_SpanClass      ",
                          "  Second_SpanStruct": "Second_SpanClass     ",
                          "   Third_SpanStruct": "Third_SpanClass      "
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<MySpanFormattableStruct?, MySpanFormattableClass?>
                    (NullMySpanFormattableStructClassMap, "{0,20}", "{0,-20}", () => NullMySpanFormattableStruct_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         Fourth_SpanStruct   :                 null,
                         null                :                 null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Fourth_SpanStruct   ":"                 null",
                        "null                ":"                 null"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          Fourth_SpanStruct   :                 null,
                          null                :                 null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "Fourth_SpanStruct   ":"                 null",
                          "null                ":"                 null"
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                               Fourth_SpanClass: Fourth_SpanStruct   ,
                                Fifth_SpanClass: Fifth_SpanStruct    
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "    Fourth_SpanClass":"Fourth_SpanStruct   ",
                          "     Fifth_SpanClass":"Fifth_SpanStruct    "
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<MySpanFormattableClass?, MySpanFormattableStruct?>
                    (NullMySpanFormattableClassStructMap.ToList(), "{0,20}", "{0,-20}", () => NullMySpanFormattableClass_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null                :    Second_SpanStruct,
                         Fourth_SpanClass    :                 null,
                         null                :                 null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null                ":"   Second_SpanStruct",
                        "Fourth_SpanClass    ":"                null",
                        "null                ":"                null"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null                :    Second_SpanStruct,
                          Fourth_SpanClass    :                 null,
                          null                :                 null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null                ":"   Second_SpanStruct",
                          "Fourth_SpanClass    ":"                null",
                          "null                ":"                null"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => StructBearer_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 27.183: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>
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
                        "27.183":"http://fourth-value.com/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 27.183: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "27.183":"http://fourth-value.com/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
                    (NullStructBearerToComplexBearerMap, "{0,30}", "N3", () => NullStructBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattableStruct: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattableStruct: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":"http://first-value.com/",
                        "2.718":"http://second-value.com/",
                        "31.416":"http://third-value.com/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> { 
                             ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ 
                          },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> { 
                             ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ 
                          },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                             ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ 
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null":"http://first-value.com/",
                          "2.718":"http://second-value.com/",
                          "31.416":"http://third-value.com/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>
                      , FieldSpanFormattableAlwaysAddStringBearer<Uri>>
                    (ClassBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => ClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 3.142: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "3.142":"http://first-value.com/",
                        "2.718":"http://second-value.com/",
                        "31.416":"http://third-value.com/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 3.142: FieldSpanFormattableAlwaysAddStringBearer<Uri> { 
                             ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/ 
                           },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStringBearer<Uri> { 
                             ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/ 
                           },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStringBearer<Uri> {
                             ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/ 
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "3.142":"http://first-value.com/",
                          "2.718":"http://second-value.com/",
                          "31.416":"http://third-value.com/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
                    (NullClassBearerToComplexBearerMap, "{0,30}", "N3", () => NullClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ },
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":"http://first-value.com/",
                        "2.718":"http://second-value.com/",
                        "31.416":"http://third-value.com/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<Uri> { 
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ 
                          },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> { 
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ 
                          },
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ 
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null":"http://first-value.com/",
                          "2.718":"http://second-value.com/",
                          "31.416":"http://third-value.com/"
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDLNFE_1": "WDLWFE_2, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask",
                          "NDLNFE_13": "WDLWFE_13, WDLWFE_23",
                          "NDLNFE_2": "WDLWFE_2, WDLWFE_5"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>
                    (NullEnumLongNdNfToWdWfMap, null, "", () => NullEnumLongNdNfToWdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                         NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.WDLWFE_All,
                         NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":"WDLWFE_Second4Mask",
                        "0":"WDLWFE_All",
                        "NDLNFE_13":"WDLWFE_13",
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                          NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.WDLWFE_All,
                          NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null": null,
                          "0": "WDLWFE_All",
                          "NDLNFE_13": "WDLWFE_13",
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfDateTimeMap.ToList(), "", null, () => EnumULongNdNfDateTime_First_3_SubList)
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
                        "NDUWFE_4":"WDUWFE_4",
                        "NDUNFE_34":"WDUWFE_34",
                        "0":"Default" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDUNFE_4":"WDUWFE_4",
                          "NDUNFE_34":"WDUWFE_34",
                          "0":"Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>
                    (NullEnumULongNdNfNullStringMap, "", null, () => NullEnumULongNdNfDateTime_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongNoFlagsEnum.NDUNFE_4: null,
                         null: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                         NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUNFE_4":null,
                        "null":"WDUWFE_34",
                        "0":"Default" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          NoDefaultULongNoFlagsEnum.NDUNFE_4: null,
                          null: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                          NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDUNFE_4":null,
                          "null":"WDUWFE_34",
                          "0":"Default"
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDLNFE_1":"NDLWFE_1",
                          "WDLNFE_2":"NDLWFE_2",
                          "WDLNFE_3":"NDLWFE_3"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>
                    (NullEnumLongWdNfToNdWfMap, "", null, () => NullEnumLongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                         WithDefaultLongNoFlagsEnum.WDLNFE_13: NoDefaultLongWithFlagsEnum.NDLWFE_13,
                         null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_1":null,
                        "WDLNFE_13":"NDLWFE_13",
                        "null":"NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                          WithDefaultLongNoFlagsEnum.WDLNFE_13: NoDefaultLongWithFlagsEnum.NDLWFE_13,
                          null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDLNFE_1":null,
                          "WDLNFE_13":"NDLWFE_13",
                          "null":"NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                          "null":null
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDUNFE_2":"NDUWFE_2",
                          "WDUNFE_4":"NDUWFE_4",
                          "WDUNFE_34":"NDUWFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>
                    (NullEnumULongWdNfToNdWfMap, "", null, () => NullEnumULongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.WDUNFE_4: null,
                         null: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                         WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUNFE_4":null,
                        "null":"NDUWFE_2",
                        "Default":0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          WithDefaultULongNoFlagsEnum.WDUNFE_4: null,
                          null: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                          WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDUNFE_4": null,
                          "null": "NDUWFE_2",
                          "Default": 0
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
                        "NDLWFE_4":"NDLNFE_4",
                        "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8":"WDLNFE_6",
                        "0":"Default"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.NDLWFE_4,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: WithDefaultLongNoFlagsEnum.WDLNFE_6,
                          NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDLWFE_4": "NDLNFE_4",
                          "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8": "WDLNFE_6",
                          "0": "Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>
                    (NullEnumLongNdWfToWdNfMap, "", null, () => NullEnumLongNdWfToWdNf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                         NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "0":"Default",
                        "NDLWFE_First8Mask, NDLWFE_LastTwoMask": "WDLNFE_3",
                        "NDLWFE_22": "WDLNFE_22"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                          NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "0":"Default",
                          "NDLWFE_First8Mask, NDLWFE_LastTwoMask": "WDLNFE_3",
                          "NDLWFE_22": "WDLNFE_22"
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
                        "NDLWFE_4":"WDUNFE_4",
                        "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8":"WDUNFE_1",
                        "NDUWFE_34":"WDUNFE_34"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDUWFE_4": "WDUNFE_4",
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
                          "NDUWFE_34": "WDUNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>
                    (NullEnumULongNdWfToWdNfMap, "", null, () => NullEnumULongNdWfToWdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
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
                        "null": null,
                        "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
                        "NDUWFE_34": "WDUNFE_34"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: null,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                          NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null": null,
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDLWFE_First8Mask, WDLWFE_LastTwoMask":"NDLNFE_6",
                          "WDLWFE_22":"NDLNFE_22",
                          "WDLWFE_32":"NDLNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>
                    (NullEnumLongWdWfToNdNfMap, "", null, () => NullEnumLongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4
                         | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                         WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null": null,
                        "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8": "NDLNFE_8",
                        "Default": 0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: null,
                          WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask | WithDefaultLongWithFlagsEnum.WDLWFE_5 | WithDefaultLongWithFlagsEnum.WDLWFE_7 | WithDefaultLongWithFlagsEnum.WDLWFE_8: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                          WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null": null,
                          "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8": "NDLNFE_8",
                          "Default": 0
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDUWFE_First8Mask, WDUWFE_LastTwoMask":"NDUNFE_6",
                          "WDUWFE_22":"NDUNFE_22",
                          "WDUWFE_32":"NDUNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new KeyedSubListDictionaryExpect<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>
                    (NullEnumULongWdWfNullStringBuilderMap, "", null, () => NullEnumULongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4
                         | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                         WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null": null,
                        "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8": "NDUNFE_8",
                        "Default": 0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          null: null,
                          WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7 | WithDefaultULongWithFlagsEnum.WDUWFE_8: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "null": null,
                          "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8": "NDUNFE_8",
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
            };
}
