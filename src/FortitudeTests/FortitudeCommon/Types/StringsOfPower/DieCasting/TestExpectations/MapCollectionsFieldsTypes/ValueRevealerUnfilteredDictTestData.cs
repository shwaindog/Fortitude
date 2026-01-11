// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType.FixtureScaffolding;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestDictionaries;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;

public partial class ValueRevealerDictTestData
{
    private static PositionUpdatingList<IKeyedCollectionExpect>? allValueRevealerUnfilteredKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllValueRevealerUnfilteredDictExpectations =>
        allValueRevealerUnfilteredKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(ValueRevealerDictTestData))
            {
                // Version Collections (non null class - json as string)
                new ValueRevealerDictExpect<bool, int>
                    ([], () => Int_NegativeString_Reveal, DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new ValueRevealerDictExpect<bool, int>
                    (null, () => Int_NegativeString_Reveal, DefaultCallerTypeFlags)
                {
                    { new EK(IsKeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
                }
              , new ValueRevealerDictExpect<bool, int>
                    (BoolIntMap.ToList(), () => Int_Money_Reveal, DefaultCallerTypeFlags
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
              , new ValueRevealerDictExpect<double, ICharSequence>([], () => CharSequenceMap_10Chars, DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>(null, () => CharSequenceMap_10Chars, DefaultCallerTypeFlags)
                {
                    { new EK(IsKeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
                }
              , new ValueRevealerDictExpect<double, ICharSequence>
                    (DoubleCharSequenceMap.ToList(), () => CharSequenceMap_10Chars, DefaultCallerTypeFlags, "All_NoFilter")
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
                       ,
                        {
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                            new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                          , """
                            {
                              "3.141592653589793": "Eating the",
                              "6.283185307179586": "You have n",
                              "2.718281828459045": "One doesn'",
                              "5.43656365691809": "One doesn'",
                              "8.539734222673566": "Oiler and ",
                              "1": "All for on",
                              "-1": "Imagine th"
                            }
                            """.Dos2Unix()
                        }
                    }
                
              , new ValueRevealerDictExpect<UInt128, BigInteger>(VeryULongBigIntegerMap.ToList()
                                                               , () => BigInteger_Reveal_Negative, "'{0}'")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         '0': 0,
                         '170141183460469231731687303715884105727': -170141183460469231731687303715884105727,
                         '1': -1,
                         '113427455640312821154458202477256070485': -113427455640312821154458202477256070485,
                         '85070591730234615865843651857942052863': -85070591730234615865843651857942052863,
                         '340282366920938463463374607431768211455': -340282366920938463463374607431768211455 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "'0'":"0",
                        "'170141183460469231731687303715884105727'":"-170141183460469231731687303715884105727",
                        "'1'":"-1",
                        "'113427455640312821154458202477256070485'":"-113427455640312821154458202477256070485",
                        "'85070591730234615865843651857942052863'":"-85070591730234615865843651857942052863",
                        "'340282366920938463463374607431768211455'":"-340282366920938463463374607431768211455"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          '0': 0,
                          '170141183460469231731687303715884105727': -170141183460469231731687303715884105727,
                          '1': -1,
                          '113427455640312821154458202477256070485': -113427455640312821154458202477256070485,
                          '85070591730234615865843651857942052863': -85070591730234615865843651857942052863,
                          '340282366920938463463374607431768211455': -340282366920938463463374607431768211455
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "'0'": "0",
                          "'170141183460469231731687303715884105727'": "-170141183460469231731687303715884105727",
                          "'1'": "-1",
                          "'113427455640312821154458202477256070485'": "-113427455640312821154458202477256070485",
                          "'85070591730234615865843651857942052863'": "-85070591730234615865843651857942052863",
                          "'340282366920938463463374607431768211455'": "-340282366920938463463374607431768211455"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<UInt128, BigInteger>(NullVeryULongBigIntegerMap
                                                                          , () => BigInteger_Reveal_Pad45,"\"{0,-45}\"")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         "0                                            ":                                             0,
                         "170141183460469231731687303715884105727      ": null,
                         "1                                            ":                                             1,
                         "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                         "85070591730234615865843651857942052863       ": null,
                         "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "0                                            ":"                                            0",
                        "170141183460469231731687303715884105727      ":null,
                        "1                                            ":"                                            1",
                        "113427455640312821154458202477256070485      ":"      113427455640312821154458202477256070485",
                        "85070591730234615865843651857942052863       ":null,
                        "340282366920938463463374607431768211455      ":"      340282366920938463463374607431768211455"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          "0                                            ":                                             0,
                          "170141183460469231731687303715884105727      ": null,
                          "1                                            ":                                             1,
                          "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                          "85070591730234615865843651857942052863       ": null,
                          "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "0                                            ": "                                            0",
                          "170141183460469231731687303715884105727      ": null,
                          "1                                            ": "                                            1",
                          "113427455640312821154458202477256070485      ": "      113427455640312821154458202477256070485",
                          "85070591730234615865843651857942052863       ": null,
                          "340282366920938463463374607431768211455      ": "      340282366920938463463374607431768211455"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<UInt128, BigInteger>
                    (NullVeryULongBigIntegerMap, () => BigInteger_DblQt_Pad4
                    ,"{0,-45}")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         0                                            : "   0",
                         170141183460469231731687303715884105727      : null,
                         1                                            : "   1",
                         113427455640312821154458202477256070485      : "113427455640312821154458202477256070485",
                         85070591730234615865843651857942052863       : null,
                         340282366920938463463374607431768211455      : "340282366920938463463374607431768211455" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "0                                            ":"   0",
                        "170141183460469231731687303715884105727      ":null,
                        "1                                            ":"   1",
                        "113427455640312821154458202477256070485      ":"113427455640312821154458202477256070485",
                        "85070591730234615865843651857942052863       ":null,
                        "340282366920938463463374607431768211455      ":"340282366920938463463374607431768211455"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          0                                            : "   0",
                          170141183460469231731687303715884105727      : null,
                          1                                            : "   1",
                          113427455640312821154458202477256070485      : "113427455640312821154458202477256070485",
                          85070591730234615865843651857942052863       : null,
                          340282366920938463463374607431768211455      : "340282366920938463463374607431768211455"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "0                                            ": "   0",
                          "170141183460469231731687303715884105727      ": null,
                          "1                                            ": "   1",
                          "113427455640312821154458202477256070485      ": "113427455640312821154458202477256070485",
                          "85070591730234615865843651857942052863       ": null,
                          "340282366920938463463374607431768211455      ": "340282366920938463463374607431768211455"
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<IPAddress, Uri>(IPAddressUriMap.ToList(), () => Uri_Reveal_RightArrow
                                                          , "{0,18}")
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
              , new NullClassValueRevealerDictExpect<IPAddress?, Uri>(NullIPAddressUriMap
                                                                     , () => Uri_Reveal_RightArrow, "{0,18}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                    0.0.0.0: ==> http://first-null.com/,
                                  127.0.0.1: ==> tcp://localhost/,
                                192.168.1.1: ==> tcp://default-gateway/,
                            255.255.255.255: null,
                                       null: null 
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
                        "       192.168.1.1":"==\u003e tcp://default-gateway/",
                        "   255.255.255.255":null,
                        "              null":null
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
                                 192.168.1.1: ==> tcp://default-gateway/,
                             255.255.255.255: null,
                                        null: null
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
                          "       192.168.1.1": "==\u003e tcp://default-gateway/",
                          "   255.255.255.255": null,
                          "              null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<MySpanFormattableStruct, MySpanFormattableClass>
                    (MySpanFormattableStructClassMap.ToList() , () => MySpanFormattableClass_Reveal_PadMinus20, "{0,20}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                             First_SpanStruct: First_SpanClass     ,
                            Second_SpanStruct: Second_SpanClass    ,
                             Third_SpanStruct: Third_SpanClass     ,
                            Fourth_SpanStruct: Fourth_SpanClass    ,
                             Fifth_SpanStruct: Fifth_SpanClass     ,
                             Sixth_SpanStruct: Sixth_SpanClass      
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
                        "    Third_SpanStruct":"Third_SpanClass     ",
                        "   Fourth_SpanStruct":"Fourth_SpanClass    ",
                        "    Fifth_SpanStruct":"Fifth_SpanClass     ",
                        "    Sixth_SpanStruct":"Sixth_SpanClass     "
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
                              Third_SpanStruct: Third_SpanClass     ,
                             Fourth_SpanStruct: Fourth_SpanClass    ,
                              Fifth_SpanStruct: Fifth_SpanClass     ,
                              Sixth_SpanStruct: Sixth_SpanClass     
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
                          "    Third_SpanStruct": "Third_SpanClass     ",
                          "   Fourth_SpanStruct": "Fourth_SpanClass    ",
                          "    Fifth_SpanStruct": "Fifth_SpanClass     ",
                          "    Sixth_SpanStruct": "Sixth_SpanClass     "
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullClassValueRevealerDictExpect<MySpanFormattableStruct?, MySpanFormattableClass>
                    (NullMySpanFormattableStructClassMap, () => MySpanFormattableClass_Reveal_Pad20, "{0,-20}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         First_SpanStruct    :      First_SpanClass,
                         null                :     Second_SpanClass,
                         Third_SpanStruct    :      Third_SpanClass,
                         Fourth_SpanStruct   : null,
                         null                : null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "First_SpanStruct    ":"     First_SpanClass",
                        "null                ":"    Second_SpanClass",
                        "Third_SpanStruct    ":"     Third_SpanClass",
                        "Fourth_SpanStruct   ":null,
                        "null                ":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          First_SpanStruct    :      First_SpanClass,
                          null                :     Second_SpanClass,
                          Third_SpanStruct    :      Third_SpanClass,
                          Fourth_SpanStruct   : null,
                          null                : null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "First_SpanStruct    ": "     First_SpanClass",
                          "null                ": "    Second_SpanClass",
                          "Third_SpanStruct    ": "     Third_SpanClass",
                          "Fourth_SpanStruct   ": null,
                          "null                ": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableClassStructMap.ToList(), () => MySpanFormattableStruct_Reveal_PadMinus20, "{0,20}")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                              First_SpanClass: First_SpanStruct    ,
                             Second_SpanClass: Second_SpanStruct   ,
                              Third_SpanClass: Third_SpanStruct    ,
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
                        "     First_SpanClass":"First_SpanStruct    ",
                        "    Second_SpanClass":"Second_SpanStruct   ",
                        "     Third_SpanClass":"Third_SpanStruct    ",
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
                               First_SpanClass: First_SpanStruct    ,
                              Second_SpanClass: Second_SpanStruct   ,
                               Third_SpanClass: Third_SpanStruct    ,
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
                          "     First_SpanClass": "First_SpanStruct    ",
                          "    Second_SpanClass": "Second_SpanStruct   ",
                          "     Third_SpanClass": "Third_SpanStruct    ",
                          "    Fourth_SpanClass": "Fourth_SpanStruct   ",
                          "     Fifth_SpanClass": "Fifth_SpanStruct    "
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<MySpanFormattableClass?, MySpanFormattableStruct>
                    (NullMySpanFormattableClassStructMap.ToList(), () => MySpanFormattableStruct_Reveal_Pad20, "{0,-20}")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         First_SpanClass     :     First_SpanStruct,
                         null                :    Second_SpanStruct,
                         Third_SpanClass     :     Third_SpanStruct,
                         Fourth_SpanClass    : null,
                         null                : null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "First_SpanClass     ":"    First_SpanStruct",
                        "null                ":"   Second_SpanStruct",
                        "Third_SpanClass     ":"    Third_SpanStruct",
                        "Fourth_SpanClass    ":null,
                        "null                ":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          First_SpanClass     :     First_SpanStruct,
                          null                :    Second_SpanStruct,
                          Third_SpanClass     :     Third_SpanStruct,
                          Fourth_SpanClass    : null,
                          null                : null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "First_SpanClass     ": "    First_SpanStruct",
                          "null                ": "   Second_SpanStruct",
                          "Third_SpanClass     ": "    Third_SpanStruct",
                          "Fourth_SpanClass    ": null,
                          "null                ": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<ComplexStructContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToComplexBearerMap.ToList(), () => StructBearer_Reveal_Pad30, "N3")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 3.142 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 27.183 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/ 
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
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://third-value.com/"},
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
                            SpanFormattableComplexStructContentAsValue: 3.142
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 27.183
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/
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
                          },
                          "27.183": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://fourth-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<ComplexStructContentAsValueSpanFormattable<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (NullStructBearerToComplexBearerMap, () => StructBearer_Reveal_Pad30, "N3")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 27.183 }: null,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://first-value.com/"},
                        "2.718":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://second-value.com/"},
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://third-value.com/"},
                        "27.183":null,
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                          ComplexStructContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexStructContentAsValue: 27.183
                          }: null,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://first-value.com/"
                          },
                          "2.718": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://second-value.com/"
                          },
                          "31.416": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://third-value.com/"
                          },
                          "27.183": null,
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<ComplexContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStringBearer<Uri>>
                    (ClassBearerToComplexBearerMap.ToList(), () => ClassBearer_Reveal_Pad30, "N3")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 3.142 }: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 27.183 }: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://fourth-value.com/ 
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
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattable":"http://third-value.com/"},
                        "27.183":{"ComplexTypeFieldAlwaysAddSpanFormattable":"http://fourth-value.com/"}
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
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 27.183
                          }: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://fourth-value.com/
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
                          },
                          "27.183": {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": "http://fourth-value.com/"
                          }
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<ComplexContentAsValueSpanFormattable<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (NullClassBearerToComplexBearerMap, () => StructBearer_Reveal_Pad30, "N3")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                         ComplexContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexContentAsValue: 27.183 }: null,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://first-value.com/"},
                        "2.718":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://second-value.com/"},
                        "31.416":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://third-value.com/"},
                        "27.183":null,
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 2.718
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 31.416
                          }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/,
                          ComplexContentAsValueSpanFormattable<decimal> {
                            SpanFormattableComplexContentAsValue: 27.183
                          }: null,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://first-value.com/"
                          },
                          "2.718": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://second-value.com/"
                          },
                          "31.416": {
                            "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": "http://third-value.com/"
                          },
                          "27.183": null,
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToWdWfMap.ToList(), () => WithDefaultLongWithFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_4: WithDefaultLongWithFlagsEnum.WDLWFE_4,
                         NoDefaultLongNoFlagsEnum.NDLNFE_34: WithDefaultLongWithFlagsEnum.WDLWFE_34,
                         NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.Default,
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
                        "NDLNFE_4":"WDLWFE_4",
                        "NDLNFE_34":"WDLWFE_34",
                        "0":"Default",
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
                          NoDefaultLongNoFlagsEnum.NDLNFE_4: WithDefaultLongWithFlagsEnum.WDLWFE_4,
                          NoDefaultLongNoFlagsEnum.NDLNFE_34: WithDefaultLongWithFlagsEnum.WDLWFE_34,
                          NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.Default,
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
                          "NDLNFE_4": "WDLWFE_4",
                          "NDLNFE_34": "WDLWFE_34",
                          "0": "Default",
                          "NDLNFE_1": "WDLWFE_2, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask",
                          "NDLNFE_13": "WDLWFE_13, WDLWFE_23",
                          "NDLNFE_2": "WDLWFE_2, WDLWFE_5"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum>
                    (NullEnumLongNdNfToWdWfMap.ToList(), () => WithDefaultLongWithFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_4: null,
                         null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                         NoDefaultLongNoFlagsEnum.NDLNFE_1: null,
                         NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.WDLWFE_All,
                         NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLNFE_4":null,
                        "null":"WDLWFE_Second4Mask",
                        "NDLNFE_1":null,
                        "0":"WDLWFE_All",
                        "NDLNFE_13":"WDLWFE_13",
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongNoFlagsEnum.NDLNFE_4: null,
                          null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                          NoDefaultLongNoFlagsEnum.NDLNFE_1: null,
                          NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.WDLWFE_All,
                          NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLNFE_4": null,
                          "null": "WDLWFE_Second4Mask",
                          "NDLNFE_1": null,
                          "0": "WDLWFE_All",
                          "NDLNFE_13": "WDLWFE_13",
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToWdwfMap.ToList(), () => WithDefaultULongWithFlags_Reveal , "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongNoFlagsEnum.NDUNFE_4: WithDefaultULongWithFlagsEnum.WDUWFE_4,
                         NoDefaultULongNoFlagsEnum.NDUNFE_34: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                         NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default,
                         NoDefaultULongNoFlagsEnum.NDUNFE_1: WithDefaultULongWithFlagsEnum.WDUWFE_1,
                         NoDefaultULongNoFlagsEnum.NDUNFE_13: WithDefaultULongWithFlagsEnum.WDUWFE_13,
                         NoDefaultULongNoFlagsEnum.NDUNFE_2: WithDefaultULongWithFlagsEnum.WDUWFE_2 
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
                        "0":"Default",
                        "NDUNFE_1":"WDUWFE_1",
                        "NDUNFE_13":"WDUWFE_13",
                        "NDUNFE_2":"WDUWFE_2"
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
                          NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default,
                          NoDefaultULongNoFlagsEnum.NDUNFE_1: WithDefaultULongWithFlagsEnum.WDUWFE_1,
                          NoDefaultULongNoFlagsEnum.NDUNFE_13: WithDefaultULongWithFlagsEnum.WDUWFE_13,
                          NoDefaultULongNoFlagsEnum.NDUNFE_2: WithDefaultULongWithFlagsEnum.WDUWFE_2
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
                          "0": "Default",
                          "NDUNFE_1": "WDUWFE_1",
                          "NDUNFE_13": "WDUWFE_13",
                          "NDUNFE_2": "WDUWFE_2"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum>
                    (NullEnumULongNdNfToWdWfMap, () => WithDefaultULongWithFlags_Reveal,  "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongNoFlagsEnum.NDUNFE_4: null,
                         null: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                         NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default,
                         NoDefaultULongNoFlagsEnum.NDUNFE_1: null,
                         NoDefaultULongNoFlagsEnum.NDUNFE_13: WithDefaultULongWithFlagsEnum.WDUWFE_13,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDUNFE_4":null,
                        "null":"WDUWFE_34",
                        "0":"Default",
                        "NDUNFE_1":null,
                        "NDUNFE_13":"WDUWFE_13",
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultULongNoFlagsEnum.NDUNFE_4: null,
                          null: WithDefaultULongWithFlagsEnum.WDUWFE_34,
                          NoDefaultULongNoFlagsEnum.0: WithDefaultULongWithFlagsEnum.Default,
                          NoDefaultULongNoFlagsEnum.NDUNFE_1: null,
                          NoDefaultULongNoFlagsEnum.NDUNFE_13: WithDefaultULongWithFlagsEnum.WDUWFE_13,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUNFE_4": null,
                          "null": "WDUWFE_34",
                          "0": "Default",
                          "NDUNFE_1": null,
                          "NDUNFE_13": "WDUWFE_13",
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNdWfMap.ToList(), () => NoDefaultLongWithFlags_Reveal,  "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_4: NoDefaultLongWithFlagsEnum.NDLWFE_4,
                         WithDefaultLongNoFlagsEnum.WDLNFE_34: NoDefaultLongWithFlagsEnum.NDLWFE_34,
                         WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0,
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
                        "WDLNFE_4":"NDLWFE_4",
                        "WDLNFE_34":"NDLWFE_34",
                        "Default":0,
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
                          WithDefaultLongNoFlagsEnum.WDLNFE_4: NoDefaultLongWithFlagsEnum.NDLWFE_4,
                          WithDefaultLongNoFlagsEnum.WDLNFE_34: NoDefaultLongWithFlagsEnum.NDLWFE_34,
                          WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0,
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
                          "WDLNFE_4": "NDLWFE_4",
                          "WDLNFE_34": "NDLWFE_34",
                          "Default": 0,
                          "WDLNFE_1": "NDLWFE_1",
                          "WDLNFE_2": "NDLWFE_2",
                          "WDLNFE_3": "NDLWFE_3"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum>
                    (NullEnumLongWdNfToNdWfMap, () => NoDefaultLongWithFlags_Reveal,  "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_4: null,
                         null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4
                         | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                         WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0,
                         WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                         WithDefaultLongNoFlagsEnum.WDLNFE_13: NoDefaultLongWithFlagsEnum.NDLWFE_13,
                         null: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_4":null,
                        "null":"NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                        "Default":0,
                        "WDLNFE_1":null,
                        "WDLNFE_13":"NDLWFE_13",
                        "null":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_4: null,
                          null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                          WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0,
                          WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                          WithDefaultLongNoFlagsEnum.WDLNFE_13: NoDefaultLongWithFlagsEnum.NDLWFE_13,
                          null: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLNFE_4": null,
                          "null": "NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                          "Default": 0,
                          "WDLNFE_1": null,
                          "WDLNFE_13": "NDLWFE_13",
                          "null": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNdWfMap.ToList(), () => NoDefaultULongWithFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.WDUNFE_2: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                         WithDefaultULongNoFlagsEnum.WDUNFE_4: NoDefaultULongWithFlagsEnum.NDUWFE_4,
                         WithDefaultULongNoFlagsEnum.WDUNFE_34: NoDefaultULongWithFlagsEnum.NDUWFE_34,
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
                        "WDUNFE_2":"NDUWFE_2",
                        "WDUNFE_4":"NDUWFE_4",
                        "WDUNFE_34":"NDUWFE_34",
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
                          WithDefaultULongNoFlagsEnum.WDUNFE_2: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                          WithDefaultULongNoFlagsEnum.WDUNFE_4: NoDefaultULongWithFlagsEnum.NDUWFE_4,
                          WithDefaultULongNoFlagsEnum.WDUNFE_34: NoDefaultULongWithFlagsEnum.NDUWFE_34,
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
                          "WDUNFE_2": "NDUWFE_2",
                          "WDUNFE_4": "NDUWFE_4",
                          "WDUNFE_34": "NDUWFE_34",
                          "Default": 0,
                          "WDUNFE_13": "NDUWFE_13"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum>
                    (NullEnumULongWdNfToNdWfMap, () => NoDefaultULongWithFlags_Reveal , "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.WDUNFE_4: null,
                         null: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                         WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                         WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13,
                         null: NoDefaultULongWithFlagsEnum.NDUWFE_All 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUNFE_4":null,
                        "null":"NDUWFE_2",
                        "Default":0,
                        "WDUNFE_13":"NDUWFE_13",
                        "null":"NDUWFE_All"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultULongNoFlagsEnum.WDUNFE_4: null,
                          null: NoDefaultULongWithFlagsEnum.NDUWFE_2,
                          WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                          WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13,
                          null: NoDefaultULongWithFlagsEnum.NDUWFE_All
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDUNFE_4": null,
                          "null": "NDUWFE_2",
                          "Default": 0,
                          "WDUNFE_13": "NDUWFE_13",
                          "null": "NDUWFE_All"
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToWdNfMap.ToList(), () => WithDefaultLongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7
                         | NoDefaultLongWithFlagsEnum.NDLWFE_8: WithDefaultLongNoFlagsEnum.WDLNFE_6,
                         NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                         NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                         NoDefaultLongWithFlagsEnum.NDLWFE_34: WithDefaultLongNoFlagsEnum.WDLNFE_34 
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
                        "0":"Default",
                        "NDLWFE_First8Mask, NDLWFE_LastTwoMask":"WDLNFE_3",
                        "NDLWFE_22":"WDLNFE_22",
                        "NDLWFE_34":"WDLNFE_34"
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
                          NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                          NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                          NoDefaultLongWithFlagsEnum.NDLWFE_34: WithDefaultLongNoFlagsEnum.WDLNFE_34
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
                          "0": "Default",
                          "NDLWFE_First8Mask, NDLWFE_LastTwoMask": "WDLNFE_3",
                          "NDLWFE_22": "WDLNFE_22",
                          "NDLWFE_34": "WDLNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum>
                    (NullEnumLongNdWfToWdNfMap, () => WithDefaultLongNoFlags_Reveal,  "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: null,
                         NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                         NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                         NoDefaultLongWithFlagsEnum.NDLWFE_34: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":null,
                        "NDLWFE_4":"WDLNFE_4",
                        "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8":null,
                        "0":"Default",
                        "NDLWFE_First8Mask, NDLWFE_LastTwoMask":"WDLNFE_3",
                        "NDLWFE_22":"WDLNFE_22",
                        "NDLWFE_34":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: null,
                          NoDefaultLongWithFlagsEnum.0: WithDefaultLongNoFlagsEnum.Default,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First8Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask: WithDefaultLongNoFlagsEnum.WDLNFE_3,
                          NoDefaultLongWithFlagsEnum.NDLWFE_22: WithDefaultLongNoFlagsEnum.WDLNFE_22,
                          NoDefaultLongWithFlagsEnum.NDLWFE_34: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "NDLWFE_4": "WDLNFE_4",
                          "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8": null,
                          "0": "Default",
                          "NDLWFE_First8Mask, NDLWFE_LastTwoMask": "WDLNFE_3",
                          "NDLWFE_22": "WDLNFE_22",
                          "NDLWFE_34": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToWdNfMap.ToList(), () => WithDefaultULongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultULongWithFlagsEnum.NDUWFE_4: WithDefaultULongNoFlagsEnum.WDUNFE_4,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                         | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                         NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34,
                         NoDefaultULongWithFlagsEnum.0: WithDefaultULongNoFlagsEnum.Default,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask: WithDefaultULongNoFlagsEnum.WDUNFE_8,
                         NoDefaultULongWithFlagsEnum.NDUWFE_22: WithDefaultULongNoFlagsEnum.WDUNFE_22 
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
                        "NDUWFE_34":"WDUNFE_34",
                        "0":"Default",
                        "NDUWFE_First8Mask, NDUWFE_LastTwoMask":"WDUNFE_8",
                        "NDUWFE_22":"WDUNFE_22"
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
                          NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34,
                          NoDefaultULongWithFlagsEnum.0: WithDefaultULongNoFlagsEnum.Default,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask: WithDefaultULongNoFlagsEnum.WDUNFE_8,
                          NoDefaultULongWithFlagsEnum.NDUWFE_22: WithDefaultULongNoFlagsEnum.WDUNFE_22
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
                          "NDUWFE_34": "WDUNFE_34",
                          "0": "Default",
                          "NDUWFE_First8Mask, NDUWFE_LastTwoMask": "WDUNFE_8",
                          "NDUWFE_22": "WDUNFE_22"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum>
                    (NullEnumULongNdWfToWdNfMap, () => WithDefaultULongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7
                         | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                         NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34,
                         NoDefaultULongWithFlagsEnum.0: WithDefaultULongNoFlagsEnum.Default,
                         NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask: WithDefaultULongNoFlagsEnum.WDUNFE_8,
                         NoDefaultULongWithFlagsEnum.NDUWFE_22: WithDefaultULongNoFlagsEnum.WDUNFE_22 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":null,
                        "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8":"WDUNFE_1",
                        "NDUWFE_34":"WDUNFE_34",
                        "0":"Default",
                        "NDUWFE_First8Mask, NDUWFE_LastTwoMask":"WDUNFE_8",
                        "NDUWFE_22":"WDUNFE_22"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask | NoDefaultULongWithFlagsEnum.NDUWFE_5 | NoDefaultULongWithFlagsEnum.NDUWFE_7 | NoDefaultULongWithFlagsEnum.NDUWFE_8: WithDefaultULongNoFlagsEnum.WDUNFE_1,
                          NoDefaultULongWithFlagsEnum.NDUWFE_34: WithDefaultULongNoFlagsEnum.WDUNFE_34,
                          NoDefaultULongWithFlagsEnum.0: WithDefaultULongNoFlagsEnum.Default,
                          NoDefaultULongWithFlagsEnum.NDUWFE_First8Mask | NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask: WithDefaultULongNoFlagsEnum.WDUNFE_8,
                          NoDefaultULongWithFlagsEnum.NDUWFE_22: WithDefaultULongNoFlagsEnum.WDUNFE_22
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
                          "NDUWFE_34": "WDUNFE_34",
                          "0": "Default",
                          "NDUWFE_First8Mask, NDUWFE_LastTwoMask": "WDUNFE_8",
                          "NDUWFE_22": "WDUNFE_22"
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNdNfMap.ToList(), () => NoDefaultLongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongWithFlagsEnum.WDLWFE_4: NoDefaultLongNoFlagsEnum.NDLNFE_4,
                         WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                         WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0,
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
                        "WDLWFE_4":"NDLNFE_4",
                        "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask":"NDLNFE_8",
                        "Default":0,
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
                          WithDefaultLongWithFlagsEnum.WDLWFE_4: NoDefaultLongNoFlagsEnum.NDLNFE_4,
                          WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                          WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0,
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
                          "WDLWFE_4": "NDLNFE_4",
                          "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask": "NDLNFE_8",
                          "Default": 0,
                          "WDLWFE_First8Mask, WDLWFE_LastTwoMask": "NDLNFE_6",
                          "WDLWFE_22": "NDLNFE_22",
                          "WDLWFE_32": "NDLNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum>
                    (NullEnumLongWdWfToNdNfMap, () => NoDefaultLongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4
                         | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                         WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0,
                         WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask: NoDefaultLongNoFlagsEnum.NDLNFE_6,
                         WithDefaultLongWithFlagsEnum.WDLWFE_22: NoDefaultLongNoFlagsEnum.NDLNFE_22,
                         WithDefaultLongWithFlagsEnum.WDLWFE_32: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":null,
                        "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask":"NDLNFE_8",
                        "Default":0,
                        "WDLWFE_First8Mask, WDLWFE_LastTwoMask":"NDLNFE_6",
                        "WDLWFE_22":"NDLNFE_22",
                        "WDLWFE_32":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                          WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0,
                          WithDefaultLongWithFlagsEnum.WDLWFE_First8Mask | WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask: NoDefaultLongNoFlagsEnum.NDLNFE_6,
                          WithDefaultLongWithFlagsEnum.WDLWFE_22: NoDefaultLongNoFlagsEnum.NDLNFE_22,
                          WithDefaultLongWithFlagsEnum.WDLWFE_32: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask": "NDLNFE_8",
                          "Default": 0,
                          "WDLWFE_First8Mask, WDLWFE_LastTwoMask": "NDLNFE_6",
                          "WDLWFE_22": "NDLNFE_22",
                          "WDLWFE_32": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new ValueRevealerDictExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNdNfMap.ToList(), () => NoDefaultULongNoFlags_Reveal, "")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongWithFlagsEnum.WDUWFE_4: NoDefaultULongNoFlagsEnum.NDUNFE_4,
                         WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4
                         | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                         WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0,
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
                        "WDUWFE_4":"NDUNFE_4",
                        "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask":"NDUNFE_8",
                        "Default":0,
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
                          WithDefaultULongWithFlagsEnum.WDUWFE_4: NoDefaultULongNoFlagsEnum.NDUNFE_4,
                          WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0,
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
                          "WDUWFE_4": "NDUNFE_4",
                          "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask": "NDUNFE_8",
                          "Default": 0,
                          "WDUWFE_First8Mask, WDUWFE_LastTwoMask": "NDUNFE_6",
                          "WDUWFE_22": "NDUNFE_22",
                          "WDUWFE_32": "NDUNFE_32"
                        }
                        """.Dos2Unix()
                    }
                }
              , new NullStructValueRevealerDictExpect<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum>
                    (NullEnumULongWdWfToNdNfMap, () => NoDefaultULongNoFlags_Reveal,  null, () => NullEnumULongWdWfToNdNf_First_3)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4
                         | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                         WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0,
                         WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask: NoDefaultULongNoFlagsEnum.NDUNFE_6,
                         WithDefaultULongWithFlagsEnum.WDUWFE_22: NoDefaultULongNoFlagsEnum.NDUNFE_22,
                         WithDefaultULongWithFlagsEnum.WDUWFE_32: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":null,
                        "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask":"NDUNFE_8",
                        "Default":0,
                        "WDUWFE_First8Mask, WDUWFE_LastTwoMask":"NDUNFE_6",
                        "WDUWFE_22":"NDUNFE_22",
                        "WDUWFE_32":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0,
                          WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask | WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask: NoDefaultULongNoFlagsEnum.NDUNFE_6,
                          WithDefaultULongWithFlagsEnum.WDUWFE_22: NoDefaultULongNoFlagsEnum.NDUNFE_22,
                          WithDefaultULongWithFlagsEnum.WDUWFE_32: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask": "NDUNFE_8",
                          "Default": 0,
                          "WDUWFE_First8Mask, WDUWFE_LastTwoMask": "NDUNFE_6",
                          "WDUWFE_22": "NDUNFE_22",
                          "WDUWFE_32": null
                        }
                        """.Dos2Unix()
                    }
                }
            };
}
