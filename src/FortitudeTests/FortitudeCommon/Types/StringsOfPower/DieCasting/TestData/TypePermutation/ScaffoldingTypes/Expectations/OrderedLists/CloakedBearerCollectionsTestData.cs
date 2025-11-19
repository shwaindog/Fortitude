// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class CloakedBearerCollectionsTestData
{
    private static PositionUpdatingList<IComplexOrderedListExpect>? allCloakedBearerCollectionExpectations;

    public static PositionUpdatingList<IComplexOrderedListExpect> AllCloakedBearerCollectionExpectations => allCloakedBearerCollectionExpectations ??=
        new PositionUpdatingList<IComplexOrderedListExpect>(typeof(CloakedBearerCollectionsTestData))
        {
            // class CloakedBearer Collections
            new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                ([], StringBearerClassListRevealer, name: "Empty")
                {
                    { new EK(OrderedCollectionType | AcceptsStruct), "[]" }
                  , { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
                   ,
                    {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , CompactJson)
                      , "[]"
                    }
                   ,
                    {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , Pretty)
                      , "[]"
                    }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, StringBearerClassListRevealer, name: "NullNonNullableClass")
                {
                    { new EK(OrderedCollectionType | AcceptsStruct | AlwaysWrites), "[]" }
                  , { new EK(AcceptsStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, Pretty), "null" }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, name: "All_CloakedBearerNoFilter")
                {
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog), """
                            [
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 },
                             FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085 }
                             ]
                            """.RemoveLineEndings()
                    }
                  , { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
                   ,
                    {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , CompactJson)
                      , "[]"
                    }
                   ,
                    {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , Pretty)
                      , "[]"
                    }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, name: "All_CloakedBearerNoFilter")
                {
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                      , """
                        [
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085 }
                         ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                      , """
                        [
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 10.8731273138362},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 19.0279727992133},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 27.1828182845904},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 32.6193819415085}
                        ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        [
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085
                          }
                        ]
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        [
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 10.8731273138362
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 19.0279727992133
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 27.1828182845904
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 32.6193819415085
                          }
                        ]
                        """.Dos2Unix()
                    }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, () => StringBearerClassList_First_5,
                 name: "All_CloakedBearerNoFilter")
                {
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                      , """
                        [
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 }
                         ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                      , """
                        [
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 10.8731273138362},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286}
                        ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        [
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286
                          }
                        ]
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        [
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 10.8731273138362
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286
                          }
                        ]
                        """.Dos2Unix()
                    }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, () => StringBearerClassList_Skip_Odd_Index)
                {
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                      , """
                        [
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 }
                         ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                      , """
                        [
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775}
                        ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        [
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775
                          }
                        ]
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        [
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 21.9911485751286
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775
                          }
                        ]
                        """.Dos2Unix()
                    }
                }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, () => StringBearerClassList_Second_5)
                {
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                      , """
                        [
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 },
                         FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085 }
                         ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                      , """
                        [
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 19.0279727992133},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 27.1828182845904},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775},
                        {"ComplexTypeFieldAlwaysAddSpanFormattable": 32.6193819415085}
                        ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                      , """
                        [
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775
                          },
                          FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                            ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085
                          }
                        ]
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                      , """
                        [
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 19.0279727992133
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 31.4159265358979
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 27.1828182845904
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775
                          },
                          {
                            "ComplexTypeFieldAlwaysAddSpanFormattable": 32.6193819415085
                          }
                        ]
                        """.Dos2Unix()
                    }
                }


            // class CloakedBearer Collections
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                ([], StringBearerClassListRevealer, name: "NullEmpty")
                {
                    { new EK(OrderedCollectionType | AcceptsStruct), "[]" }
                  , { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
                  , {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , CompactJson)
                      , "[]"
                    }
                  ,
                    {
                        new EK(CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                             , Pretty)
                      , "[]"
                    }
                }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, StringBearerClassListRevealer, name: "NullNullableClass")
                {
                    { new EK(OrderedCollectionType | AcceptsStruct | AlwaysWrites), "[]" }
                  , { new EK(AcceptsStruct | AlwaysWrites), "null" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
                  , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, Pretty), "null" }
                }
        };
}
