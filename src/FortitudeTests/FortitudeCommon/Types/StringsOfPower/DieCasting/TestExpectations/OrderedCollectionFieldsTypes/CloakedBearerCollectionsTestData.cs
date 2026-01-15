// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

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
              { new EK(   IsOrderedCollectionType | AcceptsAnyExceptNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsAnyExceptNullableStruct | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsAnyExceptNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, StringBearerClassListRevealer, name: "NullNonNullableClass")
            {
              { new EK( IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, StringBearerClassListRevealer, name: "All_CloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsAnyExceptNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
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
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":32.6193819415085}
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
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":21.9911485751286}
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
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":37.6991118430775}
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
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":32.6193819415085}
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
            
            // nullable class CloakedBearer Collections
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                ([], StringBearerClassListRevealer, name: "NullEmpty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsAnyExceptNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsAnyExceptNullableStruct | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsAnyExceptNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, StringBearerClassListRevealer, name: "NullNullableClass")
            {
              { new EK( IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, StringBearerClassListRevealer, name: "All_NullCloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                     null,
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904 },
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":12.5663706143592},
                    null,
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":27.1828182845904},
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":32.6193819415085},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592
                      },
                      null,
                      null,
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
                      null,
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 32.6193819415085
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592
                      },
                      null,
                      null,
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
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 32.6193819415085
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, StringBearerClassListRevealer, () => NullStringBearerClassList_First_5,
                 name: "All_CloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":12.5663706143592},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 12.5663706143592
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 12.5663706143592
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, StringBearerClassListRevealer, () => NullStringBearerClassList_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904 },
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":2.71828182845904},
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":37.6991118430775},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 2.71828182845904
                      },
                      null,
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 27.1828182845904
                      },
                      FieldSpanFormattableAlwaysAddStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattable: 37.6991118430775
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 2.71828182845904
                      },
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 27.1828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattable": 37.6991118430775
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, StringBearerClassListRevealer, () => NullStringBearerClassList_Second_5)
            {
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattable: 31.4159265358979 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattable":31.4159265358979}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
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
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsAnyClass | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
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
                      }
                    ]
                    """.Dos2Unix()
                }
            }
            
            // class CloakedBearer Collections
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                ([], StringBearerStructListRevealer, name: "StructEmpty")
            {
                { new EK(IsOrderedCollectionType | AcceptsStruct), "[]" }
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
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (null, StringBearerStructListRevealer, name: "NullNonNullableStruct")
            {
              { new EK( IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, StringBearerStructListRevealer, name: "All_StructCloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":32.6193819415085}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 12.5663706143592
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 21.9911485751286
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 31.4159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 27.1828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 37.6991118430775
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 32.6193819415085
                      }
                    ]
                    """.Dos2Unix()
                }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, StringBearerStructListRevealer, () => StringBearerStructList_First_5)
            {
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":21.9911485751286}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 12.5663706143592
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 21.9911485751286
                      }
                    ]
                    """.Dos2Unix()
                }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, StringBearerStructListRevealer, () => StringBearerStructList_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":12.5663706143592},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":37.6991118430775}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 12.5663706143592
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 21.9911485751286
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 31.4159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 37.6991118430775
                      }
                    ]
                    """.Dos2Unix()
                }
            }
          , new CloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, StringBearerStructListRevealer, () => StringBearerStructList_Second_5)
            {
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":32.6193819415085}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 31.4159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 27.1828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 37.6991118430775
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 32.6193819415085
                      }
                    ]
                    """.Dos2Unix()
                }
            }
            
            // nullable struct CloakedBearer Collections
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                ([], StringBearerStructListRevealer, name: "NullEmpty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsNullableStruct | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (null, StringBearerStructListRevealer, name: "NullNullableStruct")
            {
              { new EK( IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsNullableStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, StringBearerStructListRevealer, name: "All_NullStructCloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592 },
                     null,
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904 },
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":12.5663706143592},
                    null,
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":31.4159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":27.1828182845904},
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":37.6991118430775},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":32.6193819415085},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592
                      },
                      null,
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904
                      },
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 32.6193819415085
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 12.5663706143592
                      },
                      null,
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 21.9911485751286
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 31.4159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 27.1828182845904
                      },
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 37.6991118430775
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 32.6193819415085
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, StringBearerStructListRevealer, () => NullStringBearerStructList_First_5,
                 name: "All_CloakedBearerNoFilter")
            {
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":3.14159265358979},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":2.71828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":12.5663706143592},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 3.14159265358979
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 12.5663706143592
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 3.14159265358979
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 2.71828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 12.5663706143592
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, StringBearerStructListRevealer, () => NullStringBearerStructList_Skip_Odd_Index)
            {
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904 },
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775 },
                     null
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":2.71828182845904},
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":27.1828182845904},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":37.6991118430775},
                    null
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 2.71828182845904
                      },
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 27.1828182845904
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 37.6991118430775
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 2.71828182845904
                      },
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 27.1828182845904
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 37.6991118430775
                      },
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new NullStructCloakedOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, StringBearerStructListRevealer, () => NullStringBearerStructList_Second_5)
            {
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactLog)
                  , """
                    [
                     null,
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133 },
                     FieldSpanFormattableAlwaysAddStructStringBearer<decimal> { ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979 }
                     ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, CompactJson)
                  , """
                    [
                    null,
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":10.8731273138362},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":21.9911485751286},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":19.0279727992133},
                    {"ComplexTypeFieldAlwaysAddSpanFormattableFromStruct":31.4159265358979}
                    ]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyLog)
                  , """
                    [
                      null,
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 10.8731273138362
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 21.9911485751286
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 19.0279727992133
                      },
                      FieldSpanFormattableAlwaysAddStructStringBearer<decimal> {
                        ComplexTypeFieldAlwaysAddSpanFormattableFromStruct: 31.4159265358979
                      }
                    ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsNullableStruct | CallsAsReadOnlySpan | CallsAsSpan | AllOutputConditionsMask, PrettyJson)
                  , """
                    [
                      null,
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 10.8731273138362
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 21.9911485751286
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 19.0279727992133
                      },
                      {
                        "ComplexTypeFieldAlwaysAddSpanFormattableFromStruct": 31.4159265358979
                      }
                    ]
                    """.Dos2Unix()
                }
            }
        };
}
