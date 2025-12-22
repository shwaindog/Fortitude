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

public partial class BothRevealersDictTestData
{
    private static PositionUpdatingList<IKeyedCollectionExpect>? allBothRevealersSubListFilteredDictExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllBothRevealersSubListFilteredDictExpectations =>
        allBothRevealersSubListFilteredDictExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(ValueRevealerDictTestData))
            {
                // Version Collections (non null class - json as string)
                new BothRevealersKeyedSubListDictExpect<bool, int>
                    ([], () => Int_NegativeString_Reveal, () => Bool_Reveal, () => Bool_True_SubList,  name: "Empty_Filtered")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new BothRevealersKeyedSubListDictExpect<bool, int>
                    (null,() => Int_NegativeString_Reveal, () => Bool_Reveal, () => Bool_False_SubList)
                {
                    { new EK(KeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
                }
              , new BothRevealersKeyedSubListDictExpect<bool, int>
                    (BoolIntMap.ToList(), () => Int_NegativeString_Reveal, () => Bool_OneChar_Reveal, name: "AllKeysSubList")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ t: \"-1\", f: \"0\" }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"t\":\"-1\",\"f\":\"0\"}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          t: "-1",
                          f: "0"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "t": "-1",
                          "f": "0"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    ([], () => CharSequenceMap_Pad50, () => Double_Reveal, name: "Empty_DoubleSubList")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites), "{}" }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    (null, () => CharSequenceMap_Pad50, () => Double_Reveal)
                {
                    { new EK(KeyedCollectionType | AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "null" }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    (DoubleCharSequenceMap.ToList(), () => CharSequenceMap_Pad50
                   , () => Double_Reveal, () => Double_First_4_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                        """
                        {
                         3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                         6.283185307179586: "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                         2.718281828459045: "One doesn't simply write Euler nature number.     ",
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
                        "2.718281828459045":"One doesn't simply write Euler nature number.     ",
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
                          2.718281828459045: "One doesn't simply write Euler nature number.     ",
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
                          "2.718281828459045":"One doesn't simply write Euler nature number.     ",
                          "5.43656365691809":"One doesn't even appear at the start of Euler nature number."
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    (DoubleCharSequenceMap.ToList()
                   , () => CharSequenceMap_Pad50
                   , () => Double_Reveal_PadMinus17
                   , () => Double_Second_4_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         8.539734222673566: "Oiler and Euler are very different things.        ",
                         1                : "All for one and one for all.                      ",
                         -1               : "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "8.539734222673566":"Oiler and Euler are very different things.        ",
                        "1                ":"All for one and one for all.                      ",
                        "-1               ":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          8.539734222673566: "Oiler and Euler are very different things.        ",
                          1                : "All for one and one for all.                      ",
                          -1               : "Imagine there's no tax havens, it's easy if you try" 
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "8.539734222673566":"Oiler and Euler are very different things.        ",
                          "1                ":"All for one and one for all.                      ",
                          "-1               ":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<UInt128, BigInteger>
                    (VeryULongBigIntegerMap.ToList()
                   , () => BigInteger_Separators
                   , () => UInt128_Reveal_SglQt
                   , () => VeryULong_First_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<UInt128, BigInteger>
                    (NullVeryULongBigIntegerMap
                   , () => BigInteger_Reveal_Pad45 
                   , () => UInt128_Reveal_DblQtPadMinus45
                   , () => VeryULong_Second_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
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
                        "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                        "85070591730234615865843651857942052863       ":null,
                        "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                          "85070591730234615865843651857942052863       ": null,
                          "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "113427455640312821154458202477256070485      ":       113427455640312821154458202477256070485,
                          "85070591730234615865843651857942052863       ": null,
                          "340282366920938463463374607431768211455      ":       340282366920938463463374607431768211455
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<IPAddress, Uri>
                    (IPAddressUriMap.ToList()
                   , () => Uri_Reveal_RightArrow
                   , () => IPAddress_Reveal_Pad18)
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
              , new  KeyRevealerNullClassValueRevealerKeyedSubListDictExpect<IPAddress, Uri>
                    (IPAddressNullUriMap , () => Uri_Reveal_RightArrow , () => IPAddress_Reveal_Pad18)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                    0.0.0.0: null,
                                  127.0.0.1: ==> tcp://localhost/,
                                192.168.1.1: ==> tcp://default-gateway/,
                            255.255.255.255: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "        0.0.0.0":null,
                        "      127.0.0.1":"==> tcp://localhost/",
                        "    192.168.1.1":"==> tcp://default-gateway/",
                        "255.255.255.255":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                                  0.0.0.0: null,
                                127.0.0.1: ==> tcp://localhost/,
                              192.168.1.1: ==> tcp://default-gateway/,
                          255.255.255.255: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "        0.0.0.0": null,
                          "      127.0.0.1": "==> tcp://localhost/",
                          "    192.168.1.1": "==> tcp://default-gateway/",
                          "255.255.255.255": null 
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<MySpanFormattableStruct, MySpanFormattableClass>
                    (MySpanFormattableStructClassMap.ToList()
                   , () => MySpanFormattableClass_Reveal_PadMinus20
                   , () => MySpanFormattableStruct_Reveal_Pad20
                   , () => MySpanFormattableStruct_First_3_SubList)
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
              , new KeyRevealerNullClassValueRevealerKeyedSubListDictExpect<MySpanFormattableStruct, MySpanFormattableClass>
                    (MySpanFormattableStructNullClassMap
                   , () => MySpanFormattableClass_Reveal_Pad20
                   , () => MySpanFormattableStruct_Reveal_PadMinus20
                   , () => MySpanFormattableStruct_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         Fourth_SpanStruct   : null,
                         Fifth_SpanStruct    : null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Fourth_SpanStruct   ":null,
                        "Fifth_SpanStruct    ":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          Fourth_SpanStruct   : null,
                          Fifth_SpanStruct    : null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "Fourth_SpanStruct   ": null,
                          "Fifth_SpanStruct    ": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableClassStructMap.ToList()
                   , () => MySpanFormattableStruct_Reveal_PadMinus20
                   , () => MySpanFormattableClass_Reveal_Pad20
                   , () => MySpanFormattableClass_Second_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<MySpanFormattableClass, MySpanFormattableStruct>
                    (MySpanFormattableNullClassStructMap.ToList()
                   , () => MySpanFormattableStruct_Reveal_PadMinus20
                   , () => MySpanFormattableClass_Reveal_Pad20
                   , () => MySpanFormattableClass_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                              First_SpanClass: First_SpanStruct    ,
                             Second_SpanClass: null,
                              Third_SpanClass: null 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "     First_SpanClass":"First_SpanStruct    ",
                        "    Second_SpanClass":null,
                        "     Third_SpanClass":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                               First_SpanClass: First_SpanStruct    ,
                              Second_SpanClass: null,
                               Third_SpanClass: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "     First_SpanClass":"First_SpanStruct    ",
                          "    Second_SpanClass":null,
                          "     Third_SpanClass":null
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>, FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToComplexBearerMap.ToList()
                   , () => StructBearer_Reveal_Pad30
                   , () => StructBearerDecimal_Reveal_N3
                   , () => StructBearer_Second_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 27.183: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/ 
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
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 27.183: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://fourth-value.com/
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (StructBearerToNullComplexStructBearerMap
                   , () => StructBearer_Reveal_Pad30
                   , () => StructBearerDecimal_Reveal_N3
                   , () => StructBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattableStruct: 3.142: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattableStruct: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattableStruct: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 3.142: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://first-value.com/,
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<decimal>= SimpleTypeAsValueSpanFormattableStruct: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "3.142": "http://first-value.com/",
                          "2.718": "http://second-value.com/",
                          "31.416": "http://third-value.com/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>
                      , FieldSpanFormattableAlwaysAddStringBearer<Uri>>
                    ( ClassBearerToComplexBearerMap.ToList()
                   , () => ClassBearer_Reveal_Pad30
                   , () => ClassBearerDecimal_Reveal_N3
                   , () => ClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 3.142: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/ 
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
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 3.142: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://first-value.com/,
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://second-value.com/,
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStringBearer<Uri>= ComplexTypeFieldAlwaysAddSpanFormattable: http://third-value.com/ 
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>
                      , FieldSpanFormattableAlwaysAddStructStringBearer<Uri>>
                    (ClassBearerToNullStructComplexBearerMap
                   , () => StructBearer_Reveal_Pad30
                   , () => ClassBearerDecimal_Reveal_N3
                   , () => ClassBearer_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 3.142: null,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 2.718: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://second-value.com/,
                         SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>=
                         SimpleTypeAsValueSpanFormattable: 31.416: FieldSpanFormattableAlwaysAddStructStringBearer<Uri>=
                         ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: http://third-value.com/ 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "3.142":null,
                        "2.718":"http://second-value.com/",
                        "31.416":"http://third-value.com/"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<decimal>= SimpleTypeAsValueSpanFormattable: 3.142: null,
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "3.142": null,
                          "2.718": "http://second-value.com/",
                          "31.416": "http://third-value.com/"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToWdWfMap.ToList()
                   , () => WithDefaultLongWithFlags_Reveal
                   , () => NoDefaultLongNoFlags_Reveal
                   , () => EnumLongNdNfToWdWf_Second_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<NoDefaultLongNoFlagsEnum, WithDefaultLongWithFlagsEnum>
                    (EnumLongNdNfToNullWdWfMap
                    , () => WithDefaultLongWithFlags_Reveal
                    , () => NoDefaultLongNoFlags_Reveal
                   , () => EnumLongNdNfToWdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         NoDefaultLongNoFlagsEnum.NDLNFE_1: null,
                         NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLNFE_1":"WDLWFE_Second4Mask",
                        "NDLNFE_13":"WDLWFE_13" 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          NoDefaultLongNoFlagsEnum.NDLNFE_1: WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask,
                          NoDefaultLongNoFlagsEnum.NDLNFE_13: WithDefaultLongWithFlagsEnum.WDLWFE_13
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDLNFE_1":"WDLWFE_Second4Mask",
                          "NDLNFE_13":"WDLWFE_13"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToWdwfMap.ToList()
                   , () => WithDefaultULongWithFlags_Reveal
                   , () => NoDefaultULongNoFlags_Reveal
                   , () => EnumULongNdNfDateTime_First_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<NoDefaultULongNoFlagsEnum, WithDefaultULongWithFlagsEnum>
                    (EnumULongNdNfToNullWdWfMap
                   , () => WithDefaultULongWithFlags_Reveal
                   , () => NoDefaultULongNoFlags_Reveal
                   , () => EnumULongNdNfDateTime_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDUNFE_4":"WDUWFE_4",
                          "null":"WDUWFE_34",
                          "0":"Default"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNdWfMap.ToList()
                   , () => NoDefaultLongWithFlags_Reveal
                   , () => WithDefaultLongNoFlags_Reveal
                   , () => EnumLongWdNfToNdWf_Second_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<WithDefaultLongNoFlagsEnum, NoDefaultLongWithFlagsEnum>
                    (EnumLongWdNfToNullNdWfMap
                   , () => NoDefaultLongWithFlags_Reveal
                   , () => WithDefaultLongNoFlags_Reveal
                   , () => EnumLongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                         WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLNFE_1":null,
                        "WDLNFE_2":"NDLWFE_2"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          WithDefaultLongNoFlagsEnum.WDLNFE_1: null,
                          WithDefaultLongNoFlagsEnum.WDLNFE_2: NoDefaultLongWithFlagsEnum.NDLWFE_2
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDLNFE_1":null,
                          "WDLNFE_2":"NDLWFE_2"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNdWfMap.ToList()
                   , () => NoDefaultULongWithFlags_Reveal
                   , () => WithDefaultULongNoFlags_Reveal
                   , () => EnumULongWdNfToNdWf_First_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<WithDefaultULongNoFlagsEnum, NoDefaultULongWithFlagsEnum>
                    (EnumULongWdNfToNullNdWfMap
                   , () =>  NoDefaultULongWithFlags_Reveal
                   , () => WithDefaultULongNoFlags_Reveal
                   , () => EnumULongWdNfToNdWf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                         WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13 
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "Default":0,
                        "WDUNFE_13":"NDUWFE_13"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          WithDefaultULongNoFlagsEnum.Default: NoDefaultULongWithFlagsEnum.0,
                          WithDefaultULongNoFlagsEnum.WDUNFE_13: NoDefaultULongWithFlagsEnum.NDUWFE_13
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "Default":0,
                          "WDUNFE_13":"NDUWFE_13"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToWdNfMap.ToList()
                   , () =>  WithDefaultLongNoFlags_Reveal
                   , () => NoDefaultLongWithFlags_Reveal
                   , () => EnumLongNdWfToWdNf_First_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<NoDefaultLongWithFlagsEnum, WithDefaultLongNoFlagsEnum>
                    (EnumLongNdWfToNullWdNfMap
                   , () =>  WithDefaultLongNoFlags_Reveal
                   , () => NoDefaultLongWithFlags_Reveal
                   , () => EnumLongNdWfToWdNf_Second_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "NDLWFE_First8Mask, NDLWFE_LastTwoMask":null,
                        "NDLWFE_22":"WDLNFE_22",
                        "NDLWFE_22":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDLWFE_First8Mask, NDLWFE_LastTwoMask": null,
                          "NDLWFE_22": "WDLNFE_22",
                          "NDLWFE_34": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToWdNfMap.ToList()
                   , () =>  WithDefaultULongNoFlags_Reveal
                   , () => NoDefaultULongWithFlags_Reveal
                   , () => EnumULongNdWfToWdNf_First_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<NoDefaultULongWithFlagsEnum, WithDefaultULongNoFlagsEnum>
                    (EnumULongNdWfToNullWdNfMap
                   , () =>  WithDefaultULongNoFlags_Reveal
                   , () => NoDefaultULongWithFlags_Reveal
                   , () => EnumULongNdWfToWdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "NDUWFE_4":"WDUNFE_4",
                          "NDUWFE_First4Mask, NDUWFE_5, NDUWFE_7, NDUWFE_8": null,
                          "NDUWFE_34": "WDUNFE_34"
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNdNfMap.ToList()
                   , () =>  NoDefaultLongNoFlags_Reveal
                   , () => WithDefaultLongWithFlags_Reveal
                   , () => EnumLongWdWfToNdNf_Second_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<WithDefaultLongWithFlagsEnum, NoDefaultLongNoFlagsEnum>
                    (EnumLongWdWfToNullNdNfMap
                   , () =>  NoDefaultLongNoFlags_Reveal
                   , () => WithDefaultLongWithFlags_Reveal
                   , () => EnumLongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDLWFE_4":"NDLNFE_4",
                        "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8": "NDLNFE_8",
                        "Default":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDLWFE_4":"NDLNFE_4",
                          "WDLWFE_First4Mask, WDLWFE_5, WDLWFE_7, WDLWFE_8": "NDLNFE_8",
                          "Default": null
                        }
                        """.Dos2Unix()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNdNfMap.ToList()
                   , () =>  NoDefaultULongNoFlags_Reveal
                   , () => WithDefaultULongWithFlags_Reveal
                   , () => EnumULongWdWfToNdNf_Second_3_SubList)
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
              , new KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<WithDefaultULongWithFlagsEnum, NoDefaultULongNoFlagsEnum>
                    (EnumULongWdWfToNullNdNfMap
                   , () =>  NoDefaultULongNoFlags_Reveal
                   , () => WithDefaultULongWithFlags_Reveal
                   , () => EnumULongWdWfToNdNf_First_3_SubList)
                {
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
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
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                        "WDUWFE_4": null,
                        "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8": "NDUNFE_8",
                        "Default": null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                          WithDefaultULongWithFlagsEnum.WDUWFE_4: null,
                          WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask | WithDefaultULongWithFlagsEnum.WDUWFE_5 | WithDefaultULongWithFlagsEnum.WDUWFE_7 | WithDefaultULongWithFlagsEnum.WDUWFE_8: NoDefaultULongNoFlagsEnum.NDUNFE_8,
                          WithDefaultULongWithFlagsEnum.Default: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , """
                        {
                          "WDUWFE_4": null,
                          "WDUWFE_First4Mask, WDUWFE_5, WDUWFE_7, WDUWFE_8": "NDUNFE_8",
                          "Default": null
                        }
                        """.Dos2Unix()
                    }
                }
                
            };
}
