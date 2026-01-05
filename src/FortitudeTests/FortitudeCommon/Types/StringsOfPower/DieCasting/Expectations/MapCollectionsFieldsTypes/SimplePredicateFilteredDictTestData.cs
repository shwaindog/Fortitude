// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.ValueTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestDictionaries;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.MapCollectionsFieldsTypes;

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
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new DictionaryExpect<bool, int>(null, () => BoolIntMap_First_1)
                {
                    { new EK(IsKeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "3.141592653589793": "Eating the crust edges of one pie means you have eaten the length of two pi",
                          "6.283185307179586": "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          "2.718281828459045": "One doesn't simply write Euler nature number.",
                          "5.43656365691809": "One doesn't even appear at the start of Euler nature number."
                        }
                        """.Dos2Unix()
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "8.539734222673566": "Oiler and Euler are very different things.",
                          "                1": "All for one and one for all.",
                          "               -1": "Imagine there's no tax havens, it's easy if you try"
                        }
                        """.Dos2Unix()
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
                        "null":null,
                        "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                        "2.718281828459045":"One doesn't simply write Euler nature number.",
                        "1":"All for one and one for all."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "3.141592653589793": "Eating the crust edges of one pie means you have eaten the length of two pi",
                          "2.718281828459045": "One doesn't simply write Euler nature number.",
                          "1": "All for one and one for all."
                        }
                        """.Dos2Unix()
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
                        "             null":"\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                        "               -1":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                                       null: ""Your contract is null AND void", apparently not the same thing or why say both.",
                                         -1: "Imagine there's no tax havens, it's easy if you try"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "             null": "\u0022Your contract is null AND void\u0022, apparently not the same thing or why say both.",
                          "               -1": "Imagine there's no tax havens, it's easy if you try"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<UInt128, BigInteger>(VeryULongBigIntegerMap.ToList(), "-{0}", "'{0}'"
                                                        , () => VeryULongBigInteger_First_3)
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
                        "'0'":"-0",
                        "'170141183460469231731687303715884105727'":"-170141183460469231731687303715884105727",
                        "'1'":"-1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          '0': -0,
                          '170141183460469231731687303715884105727': -170141183460469231731687303715884105727,
                          '1': -1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "'0'": "-0",
                          "'170141183460469231731687303715884105727'": "-170141183460469231731687303715884105727",
                          "'1'": "-1"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<UInt128, BigInteger?>(NullVeryULongBigIntegerMap, "\"{0,4}\"","{0,-45}"
                                                        , () => NullVeryULongBigInteger_First_3)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          0                                            : "   0",
                          170141183460469231731687303715884105727      : "null",
                          1                                            : "   1"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "0                                            ": "   0",
                          "170141183460469231731687303715884105727      ": "null",
                          "1                                            ": "   1"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<IPAddress, Uri>(IPAddressUriMap.ToList(), "==> {0}", "{0,18}", () => IPAddressUri_First_10)
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
              , new DictionaryExpect<IPAddress?, Uri?>(NullIPAddressUriMap, "==> {0}", "{0,18}", () => NullIPAddressUri_First_10)
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
                        "           0.0.0.0":"==\u003e http://first-null.com/",
                        "         127.0.0.1":"==\u003e tcp://localhost/",
                        "       192.168.1.1":"==\u003e tcp://default-gateway/",
                        "   255.255.255.255":==\u003e null,
                        "              null":==\u003e null
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
                             255.255.255.255: ==> null,
                                        null: ==> null
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
                          "   255.255.255.255": ==\u003e null,
                          "              null": ==\u003e null
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<MySpanFormattableStruct, MySpanFormattableClass>
                    (MySpanFormattableStructClassMap.ToList(), "{0,-20}", "{0,20}", () => MySpanFormattableStructClass_Second_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
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
                          "   Fourth_SpanStruct": "Fourth_SpanClass    ",
                          "    Fifth_SpanStruct": "Fifth_SpanClass     ",
                          "    Sixth_SpanStruct": "Sixth_SpanClass     "
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<MySpanFormattableStruct?, MySpanFormattableClass?>
                    (NullMySpanFormattableStructClassMap, "{0,20}", "{0,-20}", () => NullMySpanFormattableStructClass_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         First_SpanStruct    :      First_SpanClass,
                         null                :     Second_SpanClass,
                         Third_SpanStruct    :      Third_SpanClass 
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
                        "Third_SpanStruct    ":"     Third_SpanClass"
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
                          Third_SpanStruct    :      Third_SpanClass
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
                          "Third_SpanStruct    ": "     Third_SpanClass"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableClassStructMap.ToList(), "{0,-20}", "{0,20}", () => MySpanFormattableClassStruct_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                              First_SpanClass: First_SpanStruct    ,
                             Second_SpanClass: Second_SpanStruct   ,
                              Third_SpanClass: Third_SpanStruct     
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
                        "     Third_SpanClass":"Third_SpanStruct    "
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
                               Third_SpanClass: Third_SpanStruct    
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
                          "     Third_SpanClass": "Third_SpanStruct    "
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<MySpanFormattableClass?, MySpanFormattableStruct?>
                    (NullMySpanFormattableClassStructMap.ToList(), "{0,20}", "{0,-20}", () => NullMySpanFormattableClassStruct_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         First_SpanClass     :     First_SpanStruct,
                         null                :    Second_SpanStruct,
                         Third_SpanClass     :     Third_SpanStruct 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "First_SpanClass     ":"    First_SpanStruct",
                        "null                ":"   Second_SpanStruct",
                        "Third_SpanClass     ":"    Third_SpanStruct"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          First_SpanClass     :     First_SpanStruct,
                          null                :    Second_SpanStruct,
                          Third_SpanClass     :     Third_SpanStruct
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "First_SpanClass     ": "    First_SpanStruct",
                          "null                ": "   Second_SpanStruct",
                          "Third_SpanClass     ": "    Third_SpanStruct"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<ComplexStructContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => StructBearerToComplexBearer_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 3.142 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ },
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ },
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ } 
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
              , new DictionaryExpect<ComplexStructContentAsValueSpanFormattable<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
                    (NullStructBearerToComplexBearerMap, "{0,30}", "N3", () => NullStructBearerToComplexBearerMap_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ },
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 2.718 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/ },
                         ComplexStructContentAsValueSpanFormattable<decimal> {
                         SpanFormattableComplexStructContentAsValue: 31.416 }: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ } 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://first-value.com/"},
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
                          null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
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
                          "null": {
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
              , new DictionaryExpect<ComplexContentAsValueSpanFormattable<decimal>
                      , FieldSpanFormattableAlwaysAddStringBearer<Uri>>
                    (ClassBearerToComplexBearerMap.ToList(), "{0,30}", "N3", () => ClassBearerToComplexBearer_First_3)
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
              , new DictionaryExpect<ComplexContentAsValueSpanFormattable<decimal>?
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>?>
                    (NullClassBearerToComplexBearerMap, "{0,30}", "N3", () => NullClassBearerToComplexBearer_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/ },
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
                        "null":{"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":"http://first-value.com/"},
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
                          null: FieldSpanFormattableAlwaysAddStructStringBearer<Uri> {
                            ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/
                          },
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
                          "null": {
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
              , new DictionaryExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToWdWfMap.ToList(), "", null, () => EnumLongNdNfToWdWf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_4: WithDefaultLongWithFlagsEnum.WDLWFE_4,
                         NoDefaultLongNoFlagsEnum.NDLNFE_34: WithDefaultLongWithFlagsEnum.WDLWFE_34,
                         NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.Default 
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
                        "0":"Default"
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
                          NoDefaultLongNoFlagsEnum.0: WithDefaultLongWithFlagsEnum.Default
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
                          "0": "Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<NoDefaultLongNoFlagsEnum?, WithDefaultLongWithFlagsEnum?>
                    (NullEnumLongNdNfToWdWfMap.ToList(), null, "", () => NullEnumLongNdNfToWdWf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_4: null,
                         null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                         NoDefaultLongNoFlagsEnum.NDLNFE_1: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLNFE_4":null,
                        "null":"WDLWFE_Second4Mask",
                        "NDLNFE_1":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          NoDefaultLongNoFlagsEnum.NDLNFE_4: null,
                          null: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                          NoDefaultLongNoFlagsEnum.NDLNFE_1: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDLNFE_4": null,
                          "null": "WDLWFE_Second4Mask",
                          "NDLNFE_1": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToWdwfMap.ToList(), "", null, () => EnumULongNdNfToWdWf_First_3)
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
              , new DictionaryExpect<NoDefaultULongNoFlagsEnum?, WithDefaultULongWithFlagsEnum?>
                    (NullEnumULongNdNfToWdWfMap, "", null, () => NullEnumULongNdNfToWdWf_First_3)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "NDUNFE_4": null,
                          "null": "WDUWFE_34",
                          "0": "Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNdWfMap.ToList(), "", null, () => EnumLongWdNfToNdWf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_4: NoDefaultLongWithFlagsEnum.NDLWFE_4,
                         WithDefaultLongNoFlagsEnum.WDLNFE_34: NoDefaultLongWithFlagsEnum.NDLWFE_34,
                         WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0 
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
                        "Default":0
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
                          WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0
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
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultLongNoFlagsEnum?, NoDefaultLongWithFlagsEnum?>
                    (NullEnumLongWdNfToNdWfMap, "", null, () => NullEnumLongWdNfToNdWf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_4: null,
                         null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                         WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_4":null,
                        "null":"NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                        "Default":0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_4: null,
                          null: NoDefaultLongWithFlagsEnum.NDLWFE_2 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask,
                          WithDefaultLongNoFlagsEnum.Default: NoDefaultLongWithFlagsEnum.0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDLNFE_4": null,
                          "null": "NDLWFE_2, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask",
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNdWfMap.ToList(), "", null, () => EnumULongWdNfToNdWf_First_3)
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
              , new DictionaryExpect<WithDefaultULongNoFlagsEnum?, NoDefaultULongWithFlagsEnum?>
                    (NullEnumULongWdNfToNdWfMap, "", null, () => NullEnumULongWdNfToNdWf_First_3)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
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
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "WDUNFE_4": null,
                          "null": "NDUWFE_2",
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToWdNfMap.ToList(), "", null, () => EnumLongNdWfToWdNf_First_3)
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
              , new DictionaryExpect<NoDefaultLongWithFlagsEnum?, WithDefaultLongNoFlagsEnum?>
                    (NullEnumLongNdWfToWdNfMap, "", null, () => NullEnumLongNdWfToWdNf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         null: null,
                         NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                         NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "null":null,
                        "NDLWFE_4":"WDLNFE_4",
                        "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          NoDefaultLongWithFlagsEnum.NDLWFE_4: WithDefaultLongNoFlagsEnum.WDLNFE_4,
                          NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask | NoDefaultLongWithFlagsEnum.NDLWFE_5 | NoDefaultLongWithFlagsEnum.NDLWFE_7 | NoDefaultLongWithFlagsEnum.NDLWFE_8: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "NDLWFE_4": "WDLNFE_4",
                          "NDLWFE_First4Mask, NDLWFE_5, NDLWFE_7, NDLWFE_8": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToWdNfMap.ToList(), "", null, () => EnumULongNdWfToWdNfM_First_3)
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
              , new DictionaryExpect<NoDefaultULongWithFlagsEnum?, WithDefaultULongNoFlagsEnum?>
                    (NullEnumULongNdWfToWdNfMap, "", null, () => NullEnumULongNdWfToWdNf_First_3)
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
                        "null":null,
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
                          null: null,
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
                          "null": null,
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": "WDUNFE_1",
                          "NDUWFE_34": "WDUNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNdNfMap.ToList(), "", null, () => EnumLongWdWfToNdNf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongWithFlagsEnum.WDLWFE_4: NoDefaultLongNoFlagsEnum.NDLNFE_4,
                         WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                         WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0 
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
                        "Default":0
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
                          WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0
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
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultLongWithFlagsEnum?, NoDefaultLongNoFlagsEnum?>
                    (NullEnumLongWdWfToNdNfMap, "", null, () => NullEnumLongWdWfToNdNf_First_3)
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
                        "null":null,
                        "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask":"NDLNFE_8",
                        "Default":0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          WithDefaultLongWithFlagsEnum.WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask: NoDefaultLongNoFlagsEnum.NDLNFE_8,
                          WithDefaultLongWithFlagsEnum.Default: NoDefaultLongNoFlagsEnum.0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask": "NDLNFE_8",
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNdNfMap.ToList(), "", null, () => EnumULongWdWfToNdNf_First_3)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongWithFlagsEnum.WDUWFE_4: NoDefaultULongNoFlagsEnum.NDUNFE_4,
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
                        "WDUWFE_4":"NDUNFE_4",
                        "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask":"NDUNFE_8",
                        "Default":0
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
                          WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0
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
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<WithDefaultULongWithFlagsEnum?, NoDefaultULongNoFlagsEnum?>
                    (NullEnumULongWdWfToNdNfMap, "", null, () => NullEnumULongWdWfToNdNf_First_3)
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
                        "null":null,
                        "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask":"NDUNFE_8",
                        "Default":0
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          null: null,
                          WithDefaultULongWithFlagsEnum.WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: NoDefaultULongNoFlagsEnum.0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "null": null,
                          "WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask": "NDUNFE_8",
                          "Default": 0
                        }
                        """.Dos2Unix()
                    }
                }
            };
}
