// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.CollectionRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class StringCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldStringArrayStructUnionRevisit>?  prefieldStringArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringArrayPostFieldStructUnionRevisit>? boolArrayPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringArrayClassUnionRevisit>?  prefieldStringArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringArrayPostFieldClassUnionRevisit>? boolArrayPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringSpanClassUnionRevisit>?  prefieldStringSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringSpanPostFieldClassUnionRevisit>? boolSpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringReadOnlySpanClassUnionRevisit>?  prefieldStringReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringReadOnlySpanPostFieldClassUnionRevisit>? boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringListStructUnionRevisit>?  prefieldStringListNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringListPostFieldStructUnionRevisit>? boolListPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringListClassUnionRevisit>?  prefieldStringListNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringListPostFieldClassUnionRevisit>? boolListPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringEnumerableStructUnionRevisit>?  prefieldStringEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringEnumerablePostFieldStructUnionRevisit>? boolEnumerablePostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringEnumerableClassUnionRevisit>?  prefieldStringEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringEnumerablePostFieldClassUnionRevisit>? boolEnumerablePostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringEnumeratorStructUnionRevisit>?  prefieldStringEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringEnumeratorPostFieldStructUnionRevisit>? boolEnumeratorPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringEnumeratorClassUnionRevisit>?  prefieldStringEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringEnumeratorPostFieldClassUnionRevisit>? boolEnumeratorPostFieldNoRevealersClassUnionExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldStringArrayStructUnionRevisit> PrefieldStringArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringArrayStructUnionRevisit>(new PreFieldStringArrayStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringArrayStructUnionRevisit {
                         firstPreField: null,
                         firstArray: (StringOrArrayStructUnion[]) [
                         (StringOrArrayStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrArrayStructUnion) [],
                         (StringOrArrayStructUnion) { $id: 2, $values: [ null, new string 1, new string 2, new string 3 ] },
                         (StringOrArrayStructUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrArrayStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringArrayStructUnionRevisit {
                          firstPreField: null,
                          firstArray: (StringOrArrayStructUnion[]) [
                            (StringOrArrayStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrArrayStructUnion) [],
                            (StringOrArrayStructUnion) {
                              $id: 2,
                              $values: [
                                null,
                                new string 1,
                                new string 2,
                                new string 3
                              ]
                            },
                            (StringOrArrayStructUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrArrayStructUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstArray":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new string 1",
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": null,
                          "firstArray": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new string 1",
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringArrayPostFieldStructUnionRevisit> StringArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringArrayPostFieldStructUnionRevisit>(new StringArrayPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringArrayPostFieldStructUnionRevisit {
                         firstArray: (StringOrArrayStructUnion[]) [
                         (StringOrArrayStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrArrayStructUnion) [],
                         (StringOrArrayStructUnion) { $id: 4, $values: [
                         { $id: 2, $values: interned string 1 },
                         { $ref: 1, $values: interned string 2 },
                         { $id: 3, $values: interned string 3 },
                         null
                         ]
                         },
                         (StringOrArrayStructUnion) [
                         { $ref: 2, $values: interned string 1 },
                         { $ref: 1, $values: interned string 2 },
                         { $ref: 3, $values: interned string 3 }
                         ],
                         (StringOrArrayStructUnion) { $ref: 4 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "interned string 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringArrayPostFieldStructUnionRevisit {
                          firstArray: (StringOrArrayStructUnion[]) [
                            (StringOrArrayStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrArrayStructUnion) [],
                            (StringOrArrayStructUnion) {
                              $id: 4,
                              $values: [
                                {
                                  $id: 2,
                                  $values: interned string 1
                                },
                                {
                                  $ref: 1,
                                  $values: interned string 2
                                },
                                {
                                  $id: 3,
                                  $values: interned string 3
                                },
                                null
                              ]
                            },
                            (StringOrArrayStructUnion) [
                              {
                                $ref: 2,
                                $values: interned string 1
                              },
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              {
                                $ref: 3,
                                $values: interned string 3
                              }
                            ],
                            (StringOrArrayStructUnion) {
                              $ref: 4
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"4",
                        "$values":[
                        {
                        "$id":"2",
                        "$values":"interned string 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        {
                        "$id":"3",
                        "$values":"interned string 3"
                        },
                        null
                        ]
                        },
                        [
                        {
                        "$ref":"2",
                        "$values":"interned string 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        {
                        "$ref":"3",
                        "$values":"interned string 3"
                        }
                        ],
                        {
                        "$ref":"4"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"interned string 2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstArray": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "4",
                              "$values": [
                                {
                                  "$id": "2",
                                  "$values": "interned string 1"
                                },
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                {
                                  "$id": "3",
                                  "$values": "interned string 3"
                                },
                                null
                              ]
                            },
                            [
                              {
                                "$ref": "2",
                                "$values": "interned string 1"
                              },
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              {
                                "$ref": "3",
                                "$values": "interned string 3"
                              }
                            ],
                            {
                              "$ref": "4"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }


    [TestMethod]
    public void StringArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringArrayClassUnionRevisit> PrefieldStringArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringArrayClassUnionRevisit>(new PreFieldStringArrayClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (StringOrArrayClassUnion[]) [
                         (StringOrArrayClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrArrayClassUnion) null,
                         (StringOrArrayClassUnion($id: 2)) [ new string 1, null, new string 2, new string 3 ],
                         (StringOrArrayClassUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrArrayClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringArrayClassUnionRevisit {
                          firstPreField: null,
                          firstArray: (StringOrArrayClassUnion[]) [
                            (StringOrArrayClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrArrayClassUnion) null,
                            (StringOrArrayClassUnion($id: 2)) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrArrayClassUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrArrayClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstArray":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": null,
                          "firstArray": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                null,
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringArrayPostFieldClassUnionRevisit> StringArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringArrayPostFieldClassUnionRevisit>(new StringArrayPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringArrayPostFieldClassUnionRevisit {
                         firstArray: (StringOrArrayClassUnion[]) [
                         (StringOrArrayClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrArrayClassUnion) [],
                         (StringOrArrayClassUnion($id: 2)) [ interned string 1, { $ref: 1, $values: interned string 2 }, null, interned string 3 ],
                         (StringOrArrayClassUnion) [ new string 1, new string 2, new string 3, null ],
                         (StringOrArrayClassUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "interned string 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringArrayPostFieldClassUnionRevisit {
                          firstArray: (StringOrArrayClassUnion[]) [
                            (StringOrArrayClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrArrayClassUnion) [],
                            (StringOrArrayClassUnion($id: 2)) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              null,
                              interned string 3
                            ],
                            (StringOrArrayClassUnion) [
                              new string 1,
                              new string 2,
                              new string 3,
                              null
                            ],
                            (StringOrArrayClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        null,
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        "new string 2",
                        "new string 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"interned string 2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstArray": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                null,
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              "new string 2",
                              "new string 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringSpanClassUnionRevisit> PrefieldStringSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringSpanClassUnionRevisit>(new PreFieldStringSpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringSpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "interned string 2" },
                         firstSpan: (Span<StringOrSpanClassUnion>) [
                         (StringOrSpanClassUnion) { $ref: 1, $values: interned string 2 },
                         (StringOrSpanClassUnion) [],
                         (StringOrSpanClassUnion($id: 2)) [ null, new string 1, new string 2, new string 3 ],
                         (StringOrSpanClassUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrSpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringSpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "interned string 2"
                          },
                          firstSpan: (Span<StringOrSpanClassUnion>) [
                            (StringOrSpanClassUnion) {
                              $ref: 1,
                              $values: interned string 2
                            },
                            (StringOrSpanClassUnion) [],
                            (StringOrSpanClassUnion($id: 2)) [
                              null,
                              new string 1,
                              new string 2,
                              new string 3
                            ],
                            (StringOrSpanClassUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrSpanClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":{
                        "$id":"1",
                        "$values":"interned string 2"
                        },
                        "firstSpan":[
                        {
                        "$ref":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new string 1",
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": {
                            "$id": "1",
                            "$values": "interned string 2"
                          },
                          "firstSpan": [
                            {
                              "$ref": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new string 1",
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringSpanPostFieldClassUnionRevisit> StringSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringSpanPostFieldClassUnionRevisit>(new StringSpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<StringOrSpanClassUnion>) [
                         (StringOrSpanClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrSpanClassUnion) null,
                         (StringOrSpanClassUnion($id: 2)) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3, null ],
                         (StringOrSpanClassUnion) [ new string 1, null, new string 2, new string 3 ],
                         (StringOrSpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringSpanPostFieldClassUnionRevisit {
                          firstSpan: (Span<StringOrSpanClassUnion>) [
                            (StringOrSpanClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrSpanClassUnion) null,
                            (StringOrSpanClassUnion($id: 2)) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3,
                              null
                            ],
                            (StringOrSpanClassUnion) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrSpanClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstSpan":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3",
                        null
                        ]
                        },
                        [
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstSpan": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3",
                                null
                              ]
                            },
                            [
                              "new string 1",
                              null,
                              "new string 2",
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": null
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringReadOnlySpanClassUnionRevisit> PrefieldStringReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringReadOnlySpanClassUnionRevisit>(new PreFieldStringReadOnlySpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringReadOnlySpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "interned string 2" },
                         firstReadOnlySpan: (ReadOnlySpan<StringOrReadOnlySpanClassUnion>) [
                         (StringOrReadOnlySpanClassUnion) { $ref: 1, $values: interned string 2 },
                         (StringOrReadOnlySpanClassUnion) null,
                         (StringOrReadOnlySpanClassUnion($id: 2)) [ new string 1, null, new string 2, null, new string 3 ],
                         (StringOrReadOnlySpanClassUnion) [ null, interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrReadOnlySpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringReadOnlySpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "interned string 2"
                          },
                          firstReadOnlySpan: (ReadOnlySpan<StringOrReadOnlySpanClassUnion>) [
                            (StringOrReadOnlySpanClassUnion) {
                              $ref: 1,
                              $values: interned string 2
                            },
                            (StringOrReadOnlySpanClassUnion) null,
                            (StringOrReadOnlySpanClassUnion($id: 2)) [
                              new string 1,
                              null,
                              new string 2,
                              null,
                              new string 3
                            ],
                            (StringOrReadOnlySpanClassUnion) [
                              null,
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrReadOnlySpanClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":{
                        "$id":"1",
                        "$values":"interned string 2"
                        },
                        "firstReadOnlySpan":[
                        {
                        "$ref":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        null,
                        "new string 2",
                        null,
                        "new string 3"
                        ]
                        },
                        [
                        null,
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": {
                            "$id": "1",
                            "$values": "interned string 2"
                          },
                          "firstReadOnlySpan": [
                            {
                              "$ref": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                null,
                                "new string 2",
                                null,
                                "new string 3"
                              ]
                            },
                            [
                              null,
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringReadOnlySpanPostFieldClassUnionRevisit> StringReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringReadOnlySpanPostFieldClassUnionRevisit>(new StringReadOnlySpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<StringOrReadOnlySpanClassUnion>) [
                         (StringOrReadOnlySpanClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrReadOnlySpanClassUnion) [],
                         (StringOrReadOnlySpanClassUnion($id: 2)) [ null, interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrReadOnlySpanClassUnion) [ new string 1, null, new string 2, new string 3 ],
                         (StringOrReadOnlySpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringReadOnlySpanPostFieldClassUnionRevisit {
                          firstReadOnlySpan: (ReadOnlySpan<StringOrReadOnlySpanClassUnion>) [
                            (StringOrReadOnlySpanClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrReadOnlySpanClassUnion) [],
                            (StringOrReadOnlySpanClassUnion($id: 2)) [
                              null,
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrReadOnlySpanClassUnion) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrReadOnlySpanClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstReadOnlySpan":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstReadOnlySpan": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              null,
                              "new string 2",
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": null
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringListStructUnionRevisit> PrefieldStringListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringListStructUnionRevisit>(new PreFieldStringListStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringListStructUnionRevisit {
                         firstPreField: null,
                         firstList: (List<StringOrListStructUnion>) [
                         (StringOrListStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrListStructUnion) null,
                         (StringOrListStructUnion) { $id: 2, $values: [ new string 1, new string 2, new string 3, null ] },
                         (StringOrListStructUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrListStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringListStructUnionRevisit {
                          firstPreField: null,
                          firstList: (List<StringOrListStructUnion>) [
                            (StringOrListStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrListStructUnion) null,
                            (StringOrListStructUnion) {
                              $id: 2,
                              $values: [
                                new string 1,
                                new string 2,
                                new string 3,
                                null
                              ]
                            },
                            (StringOrListStructUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrListStructUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstList":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        "new string 2",
                        "new string 3",
                        null
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": null,
                          "firstList": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                "new string 2",
                                "new string 3",
                                null
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringListPostFieldStructUnionRevisit> StringListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringListPostFieldStructUnionRevisit>(new StringListPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringListPostFieldStructUnionRevisit {
                         firstList: (List<StringOrListStructUnion>) [
                         (StringOrListStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrListStructUnion) [],
                         (StringOrListStructUnion) { $id: 2, $values: [ interned string 1, null, { $ref: 1, $values: interned string 2 }, interned string 3 ] },
                         (StringOrListStructUnion) [ new string 1, new string 2, null, new string 3 ],
                         (StringOrListStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "interned string 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringListPostFieldStructUnionRevisit {
                          firstList: (List<StringOrListStructUnion>) [
                            (StringOrListStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrListStructUnion) [],
                            (StringOrListStructUnion) {
                              $id: 2,
                              $values: [
                                interned string 1,
                                null,
                                {
                                  $ref: 1,
                                  $values: interned string 2
                                },
                                interned string 3
                              ]
                            },
                            (StringOrListStructUnion) [
                              new string 1,
                              new string 2,
                              null,
                              new string 3
                            ],
                            (StringOrListStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        "new string 2",
                        null,
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"interned string 2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstList": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              "new string 2",
                              null,
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringInstances = true
         , InstanceMarkingIncludeStringContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringListClassUnionRevisit> PrefieldStringListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringListClassUnionRevisit>(new PreFieldStringListClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringListClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "interned string 2" },
                         firstList: (List<StringOrListClassUnion>) [
                         (StringOrListClassUnion) { $ref: 1, $values: interned string 2 },
                         (StringOrListClassUnion) null,
                         (StringOrListClassUnion($id: 2)) [ new string 1, new string 2, null, new string 3 ],
                         (StringOrListClassUnion) [ interned string 1, null, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrListClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringListClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "interned string 2"
                          },
                          firstList: (List<StringOrListClassUnion>) [
                            (StringOrListClassUnion) {
                              $ref: 1,
                              $values: interned string 2
                            },
                            (StringOrListClassUnion) null,
                            (StringOrListClassUnion($id: 2)) [
                              new string 1,
                              new string 2,
                              null,
                              new string 3
                            ],
                            (StringOrListClassUnion) [
                              interned string 1,
                              null,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrListClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":{
                        "$id":"1",
                        "$values":"interned string 2"
                        },
                        "firstList":[
                        {
                        "$ref":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        "new string 2",
                        null,
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": {
                            "$id": "1",
                            "$values": "interned string 2"
                          },
                          "firstList": [
                            {
                              "$ref": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                "new string 2",
                                null,
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringListPostFieldClassUnionRevisit> StringListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringListPostFieldClassUnionRevisit>(new StringListPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringListPostFieldClassUnionRevisit {
                         firstList: (List<StringOrListClassUnion>) [
                         (StringOrListClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrListClassUnion) [],
                         (StringOrListClassUnion($id: 2)) [ null, interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrListClassUnion) [ new string 1, new string 2, new string 3, null ],
                         (StringOrListClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringListPostFieldClassUnionRevisit {
                          firstList: (List<StringOrListClassUnion>) [
                            (StringOrListClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrListClassUnion) [],
                            (StringOrListClassUnion($id: 2)) [
                              null,
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrListClassUnion) [
                              new string 1,
                              new string 2,
                              new string 3,
                              null
                            ],
                            (StringOrListClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        "new string 2",
                        "new string 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstList": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              "new string 2",
                              "new string 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": null
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringEnumerableStructUnionRevisit> PrefieldStringEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringEnumerableStructUnionRevisit>(new PreFieldStringEnumerableStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringEnumerableStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<StringOrEnumerableStructUnion>) [
                         (StringOrEnumerableStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumerableStructUnion) [],
                         (StringOrEnumerableStructUnion) { $id: 2, $values: [ new string 1, null, new string 2, new string 3 ] },
                         (StringOrEnumerableStructUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, null, interned string 3 ],
                         (StringOrEnumerableStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringEnumerableStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerable: (List<StringOrEnumerableStructUnion>) [
                            (StringOrEnumerableStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumerableStructUnion) [],
                            (StringOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                new string 1,
                                null,
                                new string 2,
                                new string 3
                              ]
                            },
                            (StringOrEnumerableStructUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              null,
                              interned string 3
                            ],
                            (StringOrEnumerableStructUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstEnumerable":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        null,
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": null,
                          "firstEnumerable": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                null,
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              null,
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringEnumerablePostFieldStructUnionRevisit> StringEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringEnumerablePostFieldStructUnionRevisit>(new StringEnumerablePostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<StringOrEnumerableStructUnion>) [
                         (StringOrEnumerableStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumerableStructUnion) [],
                         (StringOrEnumerableStructUnion) { $id: 2, $values: [ interned string 1, { $ref: 1, $values: interned string 2 }, null, interned string 3 ] },
                         (StringOrEnumerableStructUnion) [ new string 1, null, new string 2, new string 3 ],
                         (StringOrEnumerableStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "interned string 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringEnumerablePostFieldStructUnionRevisit {
                          firstEnumerable: (List<StringOrEnumerableStructUnion>) [
                            (StringOrEnumerableStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumerableStructUnion) [],
                            (StringOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                interned string 1,
                                {
                                  $ref: 1,
                                  $values: interned string 2
                                },
                                null,
                                interned string 3
                              ]
                            },
                            (StringOrEnumerableStructUnion) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrEnumerableStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        null,
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"interned string 2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstEnumerable": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                null,
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              null,
                              "new string 2",
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringEnumerableClassUnionRevisit> PrefieldStringEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringEnumerableClassUnionRevisit>(new PreFieldStringEnumerableClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringEnumerableClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "interned string 2" },
                         firstEnumerable: (List<StringOrEnumerableClassUnion>) [
                         (StringOrEnumerableClassUnion) { $ref: 1, $values: interned string 2 },
                         (StringOrEnumerableClassUnion) null,
                         (StringOrEnumerableClassUnion($id: 2)) [ null, new string 1, new string 2, new string 3 ],
                         (StringOrEnumerableClassUnion) [ interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3, null ],
                         (StringOrEnumerableClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringEnumerableClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "interned string 2"
                          },
                          firstEnumerable: (List<StringOrEnumerableClassUnion>) [
                            (StringOrEnumerableClassUnion) {
                              $ref: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumerableClassUnion) null,
                            (StringOrEnumerableClassUnion($id: 2)) [
                              null,
                              new string 1,
                              new string 2,
                              new string 3
                            ],
                            (StringOrEnumerableClassUnion) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3,
                              null
                            ],
                            (StringOrEnumerableClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":{
                        "$id":"1",
                        "$values":"interned string 2"
                        },
                        "firstEnumerable":[
                        {
                        "$ref":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new string 1",
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": {
                            "$id": "1",
                            "$values": "interned string 2"
                          },
                          "firstEnumerable": [
                            {
                              "$ref": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new string 1",
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringEnumerablePostFieldClassUnionRevisit> StringEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringEnumerablePostFieldClassUnionRevisit>(new StringEnumerablePostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<StringOrEnumerableClassUnion>) [
                         (StringOrEnumerableClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumerableClassUnion) [],
                         (StringOrEnumerableClassUnion($id: 2)) [ interned string 1, null, { $ref: 1, $values: interned string 2 }, interned string 3 ],
                         (StringOrEnumerableClassUnion) [ new string 1, new string 2, null, new string 3 ],
                         (StringOrEnumerableClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringEnumerablePostFieldClassUnionRevisit {
                          firstEnumerable: (List<StringOrEnumerableClassUnion>) [
                            (StringOrEnumerableClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumerableClassUnion) [],
                            (StringOrEnumerableClassUnion($id: 2)) [
                              interned string 1,
                              null,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrEnumerableClassUnion) [
                              new string 1,
                              new string 2,
                              null,
                              new string 3
                            ],
                            (StringOrEnumerableClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        "new string 2",
                        null,
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstEnumerable": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              "new string 2",
                              null,
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": null
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringEnumeratorStructUnionRevisit> PrefieldStringEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringEnumeratorStructUnionRevisit>(new PreFieldStringEnumeratorStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringEnumeratorStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<StringOrEnumeratorStructUnion>.Enumerator) [
                         (StringOrEnumeratorStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumeratorStructUnion) null,
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>($id: 2)) [ new string 1, new string 2, null, new string 3 ],
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) [
                         interned string 1,
                         null,
                         { $ref: 1, $values: interned string 2 },
                         interned string 3
                         ],
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringEnumeratorStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerator: (List<StringOrEnumeratorStructUnion>.Enumerator) [
                            (StringOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumeratorStructUnion) null,
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>($id: 2)) [
                              new string 1,
                              new string 2,
                              null,
                              new string 3
                            ],
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) [
                              interned string 1,
                              null,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstEnumerator":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        "new string 2",
                        null,
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": null,
                          "firstEnumerator": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                "new string 2",
                                null,
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringEnumeratorPostFieldStructUnionRevisit> StringEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringEnumeratorPostFieldStructUnionRevisit>(new StringEnumeratorPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<StringOrEnumeratorStructUnion>.Enumerator) [
                         (StringOrEnumeratorStructUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumeratorStructUnion) [],
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>($id: 2)) [
                         null, interned string 1, { $ref: 1, $values: interned string 2 }, interned string 3
                         ],
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) [ new string 1, new string 2, new string 3, null ],
                         (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) { $ref: 2 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "interned string 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringEnumeratorPostFieldStructUnionRevisit {
                          firstEnumerator: (List<StringOrEnumeratorStructUnion>.Enumerator) [
                            (StringOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumeratorStructUnion) [],
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>($id: 2)) [
                              null,
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              interned string 3
                            ],
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) [
                              new string 1,
                              new string 2,
                              new string 3,
                              null
                            ],
                            (StringOrEnumeratorStructUnion) (ReusableWrappingEnumerator<string>) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        "new string 2",
                        "new string 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"interned string 2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstEnumerator": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              "new string 2",
                              "new string 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "interned string 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringEnumeratorClassUnionRevisit> PrefieldStringEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringEnumeratorClassUnionRevisit>(new PreFieldStringEnumeratorClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringEnumeratorClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "interned string 2" },
                         firstEnumerator: (List<StringOrEnumeratorClassUnion>.Enumerator) [
                         (StringOrEnumeratorClassUnion) { $ref: 1, $values: interned string 2 },
                         (StringOrEnumeratorClassUnion) null,
                         (StringOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<string>) [ new string 1, null, new string 2, new string 3 ],
                         (StringOrEnumeratorClassUnion) (ReusableWrappingEnumerator<string>) [
                         interned string 1,
                         { $ref: 1, $values: interned string 2 },
                         null,
                         interned string 3
                         ],
                         (StringOrEnumeratorClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringEnumeratorClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "interned string 2"
                          },
                          firstEnumerator: (List<StringOrEnumeratorClassUnion>.Enumerator) [
                            (StringOrEnumeratorClassUnion) {
                              $ref: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumeratorClassUnion) null,
                            (StringOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<string>) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrEnumeratorClassUnion) (ReusableWrappingEnumerator<string>) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              null,
                              interned string 3
                            ],
                            (StringOrEnumeratorClassUnion) {
                              $ref: 2
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":{
                        "$id":"1",
                        "$values":"interned string 2"
                        },
                        "firstEnumerator":[
                        {
                        "$ref":"1",
                        "$values":interned string 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ]
                        },
                        [
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        null,
                        "interned string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstPreField": {
                            "$id": "1",
                            "$values": "interned string 2"
                          },
                          "firstEnumerator": [
                            {
                              "$ref": "1",
                              "$values": interned string 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new string 1",
                                null,
                                "new string 2",
                                "new string 3"
                              ]
                            },
                            [
                              "interned string 1",
                              {
                                "$ref": "1",
                                "$values": "interned string 2"
                              },
                              null,
                              "interned string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    public static InputBearerExpect<StringEnumeratorPostFieldClassUnionRevisit> StringEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringEnumeratorPostFieldClassUnionRevisit>(new StringEnumeratorPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<StringOrEnumeratorClassUnion>.Enumerator) [
                         (StringOrEnumeratorClassUnion) { $id: 1, $values: interned string 2 },
                         (StringOrEnumeratorClassUnion) [],
                         (StringOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<string>) [
                         interned string 1,
                         { $ref: 1, $values: interned string 2 },
                         null,
                         interned string 3
                         ],
                         (StringOrEnumeratorClassUnion) (ReusableWrappingEnumerator<string>) [
                         new string 1, null, new string 2, new string 3
                         ],
                         (StringOrEnumeratorClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringEnumeratorPostFieldClassUnionRevisit {
                          firstEnumerator: (List<StringOrEnumeratorClassUnion>.Enumerator) [
                            (StringOrEnumeratorClassUnion) {
                              $id: 1,
                              $values: interned string 2
                            },
                            (StringOrEnumeratorClassUnion) [],
                            (StringOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<string>) [
                              interned string 1,
                              {
                                $ref: 1,
                                $values: interned string 2
                              },
                              null,
                              interned string 3
                            ],
                            (StringOrEnumeratorClassUnion) (ReusableWrappingEnumerator<string>) [
                              new string 1,
                              null,
                              new string 2,
                              new string 3
                            ],
                            (StringOrEnumeratorClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: null
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        {
                        "$id":"1",
                        "$values":interned string 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "interned string 1",
                        {
                        "$ref":"1",
                        "$values":"interned string 2"
                        },
                        null,
                        "interned string 3"
                        ]
                        },
                        [
                        "new string 1",
                        null,
                        "new string 2",
                        "new string 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "firstEnumerator": [
                            {
                              "$id": "1",
                              "$values": interned string 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "interned string 1",
                                {
                                  "$ref": "1",
                                  "$values": "interned string 2"
                                },
                                null,
                                "interned string 3"
                              ]
                            },
                            [
                              "new string 1",
                              null,
                              "new string 2",
                              "new string 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": null
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }

    [TestMethod]
    public void StringEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents   = true
             });
    }
}
