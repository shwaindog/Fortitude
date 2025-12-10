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

public partial class SimpleDictTestData
{
    private static PositionUpdatingList<IKeyedCollectionExpect>? allUnfilteredSimpleKeyedCollectionExpectations;

    public static PositionUpdatingList<IKeyedCollectionExpect> AllUnfilteredSimpleDictExpectations =>
        allUnfilteredSimpleKeyedCollectionExpectations ??=
            new PositionUpdatingList<IKeyedCollectionExpect>(typeof(SimpleDictTestData))
            {
                // Version Collections (non null class - json as string)
                new DictionaryExpect<bool, int>([], DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<bool, int>(null, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<bool, int>(TestDictionaries.BoolIntMap.ToList(), DefaultCallerTypeFlags, name: "All_NoFilter")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , "{ true: 1, false: 0 }"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson)
                      , "{\"true\":1,\"false\":0}"
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        {
                          true: 1,
                          false: 0
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        {
                          "true": 1,
                          "false": 0
                        }
                        """.Dos2Unix()
                    }
                }
              , new DictionaryExpect<double, ICharSequence>([], DefaultCallerTypeFlags, name: "Empty")
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites | NonNullWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<double, ICharSequence>(null, DefaultCallerTypeFlags)
                {
                    { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                  , { new EK(AcceptsTypeAllButNullableStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "{}" }
                }
              , new DictionaryExpect<double, ICharSequence>(TestDictionaries.DoubleCharSequenceMap.ToList(), DefaultCallerTypeFlags
                                                               , "All_NoFilter")
                {
                    {
                        new EK(AcceptsTypeAllButNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog)
                      , """
                        {
                         3.141592653589793: "Eating the crust edges of one pie means you have eaten the length of two pi",
                         6.283185307179586: "You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                         2.718281828459045: "One doesn't simply write Euler nature number.",
                         5.43656365691809: "One doesn't even appear at the start of Euler nature number.",
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
                        "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                        "6.283185307179586":"You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                        "2.718281828459045":"One doesn't simply write Euler nature number.",
                        "5.43656365691809":"One doesn't even appear at the start of Euler nature number.",
                        "8.539734222673566":"Oiler and Euler are very different things.",
                        "1":"All for one and one for all.",
                        "-1":"Imagine there's no tax havens, it's easy if you try"
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
                          5.43656365691809: "One doesn't even appear at the start of Euler nature number.",
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
                          "3.141592653589793":"Eating the crust edges of one pie means you have eaten the length of two pi",
                          "6.283185307179586":"You have now eaten only 1 pie length, but if it is blood pudding pie it will feel like 2",
                          "2.718281828459045":"One doesn't simply write Euler nature number.",
                          "5.43656365691809":"One doesn't even appear at the start of Euler nature number.",
                          "8.539734222673566":"Oiler and Euler are very different things.",
                          "1":"All for one and one for all.",
                          "-1":"Imagine there's no tax havens, it's easy if you try"
                        }
                        """.RemoveLineEndings()
                    }
                }
            };
}
