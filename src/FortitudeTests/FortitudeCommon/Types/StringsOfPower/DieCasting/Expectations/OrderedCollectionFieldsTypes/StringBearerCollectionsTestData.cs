// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.OrderedCollectionFieldsTypes;

public class StringBearerCollectionsTestData
{
    private static PositionUpdatingList<IComplexOrderedListExpect>? allStringBearerCollectionExpectations;

    public static PositionUpdatingList<IComplexOrderedListExpect> AllStringBearerCollectionExpectations => allStringBearerCollectionExpectations ??=
        new PositionUpdatingList<IComplexOrderedListExpect>(typeof(StringBearerCollectionsTestData))
        {
            // class StringBearer Collections
            new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                ([], name: "Empty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsAnyClass | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, name: "NullNonNullableClass")
            {
              { new EK( IsOrderedCollectionType | AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsAnyClass | AlwaysWrites), "null" }
            , { new EK(AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, name: "All_StringBearerNoFilter")
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, () => StringBearerClassList_First_5,
                 name: "All_StringBearerNoFilter")
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, () => StringBearerClassList_Skip_Odd_Index)
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (StringBearerClassList.Value, () => StringBearerClassList_Second_5)
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
            
            // nullable class StringBearer Collections
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                ([], name: "NullEmpty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsAnyClass | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsAnyClass  | CallsAsSpan | CallsAsReadOnlySpan| AlwaysWrites), "[]" }
            }
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (null, name: "NullNullableClass")
            {
              { new EK( IsOrderedCollectionType | AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
            , { new EK(AcceptsAnyClass | AlwaysWrites), "null" }
            , { new EK(AcceptsAnyClass | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, name: "All_NullStringBearerNoFilter")
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
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, () => NullStringBearerClassList_First_5,
                 name: "All_StringBearerNoFilter")
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
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, () => NullStringBearerClassList_Skip_Odd_Index)
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
          , new NullStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStringBearer<decimal>>
                (NullStringBearerClassList.Value, () => NullStringBearerClassList_Second_5)
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
            
            // struct StringBearer Collections
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                ([], name: "StructEmpty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsStruct | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (null, name: "NullNonNullableStruct")
            {
              { new EK( IsOrderedCollectionType | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            , { new EK(AcceptsStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, name: "All_StructStringBearerNoFilter")
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, () => StringBearerStructList_First_5)
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, () => StringBearerStructList_Skip_Odd_Index)
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
          , new StringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (StringBearerStructList.Value, () => StringBearerStructList_Second_5)
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
            
            // nullable struct StringBearer Collections
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                ([], name: "NullEmpty")
            {
              { new EK(   IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
             ,{ new EK(   AcceptsNullableStruct | AlwaysWrites | NonNullWrites), "[]" }
             ,{ new EK(   AcceptsNullableStruct  | CallsAsSpan | CallsAsReadOnlySpan| AlwaysWrites), "[]" }
            }
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (null, name: "NullNullableStruct")
            {
              { new EK( IsOrderedCollectionType | AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
            , { new EK(AcceptsNullableStruct | AlwaysWrites), "null" }
            , { new EK(AcceptsNullableStruct | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, name: "All_NullStructStringBearerNoFilter")
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
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, () => NullStringBearerStructList_First_5,
                 name: "All_StringBearerNoFilter")
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
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, () => NullStringBearerStructList_Skip_Odd_Index)
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
          , new NullStructStringBearerOrderedListExpect<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>
                (NullStringBearerStructList.Value, () => NullStringBearerStructList_Second_5)
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
