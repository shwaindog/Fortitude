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
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new BothRevealersKeyedSubListDictExpect<bool, int>
                    (null,() => Int_NegativeString_Reveal, () => Bool_Reveal, () => Bool_False_SubList)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new BothRevealersKeyedSubListDictExpect<bool, int>
                    (BoolIntMap.ToList(), () => Int_NegativeString_Reveal, () => Bool_OneChar_Reveal, DefaultCallerTypeFlags
                   , name: "AllKeysSubList")
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
                    ([], () => CharSequenceMap_Pad50, () => Double_Reveal, DefaultCallerTypeFlags, name: "Empty_DoubleSubList")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    (null, () => CharSequenceMap_Pad50, () => Double_Reveal, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
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
                        """.RemoveLineEndings()
                    }
                }
              , new BothRevealersKeyedSubListDictExpect<double, ICharSequence>
                    (DoubleCharSequenceMap.ToList(), () => CharSequenceMap_Pad50
                   , () => Double_Reveal_PadMinus17, () => Double_Second_4_SubList)
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
                        """.RemoveLineEndings()
                    }
                }
            };
}
