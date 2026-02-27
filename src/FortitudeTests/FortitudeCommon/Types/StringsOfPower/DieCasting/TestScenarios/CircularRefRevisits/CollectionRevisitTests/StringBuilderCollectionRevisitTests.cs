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
public class StringBuilderBuilderCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldStringBuilderArrayStructUnionRevisit>?  prefieldStringBuilderArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringBuilderArrayPostFieldStructUnionRevisit>? boolArrayPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderArrayClassUnionRevisit>?  prefieldStringBuilderArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderArrayPostFieldClassUnionRevisit>? boolArrayPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderSpanClassUnionRevisit>?  prefieldStringBuilderSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderSpanPostFieldClassUnionRevisit>? boolSpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderReadOnlySpanClassUnionRevisit>?  prefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderReadOnlySpanPostFieldClassUnionRevisit>? boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderListStructUnionRevisit>?  prefieldStringBuilderListNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringBuilderListPostFieldStructUnionRevisit>? boolListPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderListClassUnionRevisit>?  prefieldStringBuilderListNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderListPostFieldClassUnionRevisit>? boolListPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderEnumerableStructUnionRevisit>?  prefieldStringBuilderEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringBuilderEnumerablePostFieldStructUnionRevisit>? boolEnumerablePostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderEnumerableClassUnionRevisit>?  prefieldStringBuilderEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderEnumerablePostFieldClassUnionRevisit>? boolEnumerablePostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderEnumeratorStructUnionRevisit>?  prefieldStringBuilderEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<StringBuilderEnumeratorPostFieldStructUnionRevisit>? boolEnumeratorPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldStringBuilderEnumeratorClassUnionRevisit>?  prefieldStringBuilderEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<StringBuilderEnumeratorPostFieldClassUnionRevisit>? boolEnumeratorPostFieldNoRevealersClassUnionExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldStringBuilderArrayStructUnionRevisit> PrefieldStringBuilderArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringBuilderArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderArrayStructUnionRevisit>(new PreFieldStringBuilderArrayStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderArrayStructUnionRevisit {
                         firstPreField: null,
                         firstArray: (StringBuilderOrArrayStructUnion[]) [
                         (StringBuilderOrArrayStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrArrayStructUnion) [],
                         (StringBuilderOrArrayStructUnion) { $id: 2, $values: [ null, new StringBuilder 1, new StringBuilder 2, new StringBuilder 3 ] },
                         (StringBuilderOrArrayStructUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrArrayStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderArrayStructUnionRevisit {
                          firstPreField: null,
                          firstArray: (StringBuilderOrArrayStructUnion[]) [
                            (StringBuilderOrArrayStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrArrayStructUnion) [],
                            (StringBuilderOrArrayStructUnion) {
                              $id: 2,
                              $values: [
                                null,
                                new StringBuilder 1,
                                new StringBuilder 2,
                                new StringBuilder 3
                              ]
                            },
                            (StringBuilderOrArrayStructUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrArrayStructUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderArrayPostFieldStructUnionRevisit> StringBuilderArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringBuilderArrayPostFieldStructUnionRevisit>(new StringBuilderArrayPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderArrayPostFieldStructUnionRevisit {
                         firstArray: (StringBuilderOrArrayStructUnion[]) [
                         (StringBuilderOrArrayStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrArrayStructUnion) [],
                         (StringBuilderOrArrayStructUnion) { $id: 4, $values: [
                         { $id: 2, $values: singleton StringBuilder 1 },
                         { $ref: 1, $values: singleton StringBuilder 2 },
                         { $id: 3, $values: singleton StringBuilder 3 },
                         null
                         ]
                         },
                         (StringBuilderOrArrayStructUnion) [
                         { $ref: 2, $values: singleton StringBuilder 1 },
                         { $ref: 1, $values: singleton StringBuilder 2 },
                         { $ref: 3, $values: singleton StringBuilder 3 }
                         ],
                         (StringBuilderOrArrayStructUnion) { $ref: 4 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "singleton StringBuilder 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderArrayPostFieldStructUnionRevisit {
                          firstArray: (StringBuilderOrArrayStructUnion[]) [
                            (StringBuilderOrArrayStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrArrayStructUnion) [],
                            (StringBuilderOrArrayStructUnion) {
                              $id: 4,
                              $values: [
                                {
                                  $id: 2,
                                  $values: singleton StringBuilder 1
                                },
                                {
                                  $ref: 1,
                                  $values: singleton StringBuilder 2
                                },
                                {
                                  $id: 3,
                                  $values: singleton StringBuilder 3
                                },
                                null
                              ]
                            },
                            (StringBuilderOrArrayStructUnion) [
                              {
                                $ref: 2,
                                $values: singleton StringBuilder 1
                              },
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              {
                                $ref: 3,
                                $values: singleton StringBuilder 3
                              }
                            ],
                            (StringBuilderOrArrayStructUnion) {
                              $ref: 4
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton StringBuilder 2"
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"4",
                        "$values":[
                        {
                        "$id":"2",
                        "$values":"singleton StringBuilder 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        {
                        "$id":"3",
                        "$values":"singleton StringBuilder 3"
                        },
                        null
                        ]
                        },
                        [
                        {
                        "$ref":"2",
                        "$values":"singleton StringBuilder 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        {
                        "$ref":"3",
                        "$values":"singleton StringBuilder 3"
                        }
                        ],
                        {
                        "$ref":"4"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "4",
                              "$values": [
                                {
                                  "$id": "2",
                                  "$values": "singleton StringBuilder 1"
                                },
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                {
                                  "$id": "3",
                                  "$values": "singleton StringBuilder 3"
                                },
                                null
                              ]
                            },
                            [
                              {
                                "$ref": "2",
                                "$values": "singleton StringBuilder 1"
                              },
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              {
                                "$ref": "3",
                                "$values": "singleton StringBuilder 3"
                              }
                            ],
                            {
                              "$ref": "4"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton StringBuilder 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }


    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderArrayClassUnionRevisit> PrefieldStringBuilderArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderArrayClassUnionRevisit>(new PreFieldStringBuilderArrayClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (StringBuilderOrArrayClassUnion[]) [
                         (StringBuilderOrArrayClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrArrayClassUnion) null,
                         (StringBuilderOrArrayClassUnion($id: 2)) [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrArrayClassUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrArrayClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderArrayClassUnionRevisit {
                          firstPreField: null,
                          firstArray: (StringBuilderOrArrayClassUnion[]) [
                            (StringBuilderOrArrayClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrArrayClassUnion) null,
                            (StringBuilderOrArrayClassUnion($id: 2)) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrArrayClassUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrArrayClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                null,
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderArrayPostFieldClassUnionRevisit> StringBuilderArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderArrayPostFieldClassUnionRevisit>(new StringBuilderArrayPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderArrayPostFieldClassUnionRevisit {
                         firstArray: (StringBuilderOrArrayClassUnion[]) [
                         (StringBuilderOrArrayClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrArrayClassUnion) [],
                         (StringBuilderOrArrayClassUnion($id: 2)) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, null, singleton StringBuilder 3 ],
                         (StringBuilderOrArrayClassUnion) [ new StringBuilder 1, new StringBuilder 2, new StringBuilder 3, null ],
                         (StringBuilderOrArrayClassUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton StringBuilder 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderArrayPostFieldClassUnionRevisit {
                          firstArray: (StringBuilderOrArrayClassUnion[]) [
                            (StringBuilderOrArrayClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrArrayClassUnion) [],
                            (StringBuilderOrArrayClassUnion($id: 2)) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              null,
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrArrayClassUnion) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              new StringBuilder 3,
                              null
                            ],
                            (StringBuilderOrArrayClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton StringBuilder 2"
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        null,
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                null,
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              "new StringBuilder 2",
                              "new StringBuilder 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton StringBuilder 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderSpanClassUnionRevisit> PrefieldStringBuilderSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderSpanClassUnionRevisit>(new PreFieldStringBuilderSpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderSpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton StringBuilder 2" },
                         firstSpan: (Span<StringBuilderOrSpanClassUnion>) [
                         (StringBuilderOrSpanClassUnion) { $ref: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrSpanClassUnion) [],
                         (StringBuilderOrSpanClassUnion($id: 2)) [ null, new StringBuilder 1, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrSpanClassUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrSpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderSpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton StringBuilder 2"
                          },
                          firstSpan: (Span<StringBuilderOrSpanClassUnion>) [
                            (StringBuilderOrSpanClassUnion) {
                              $ref: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrSpanClassUnion) [],
                            (StringBuilderOrSpanClassUnion($id: 2)) [
                              null,
                              new StringBuilder 1,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrSpanClassUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrSpanClassUnion) {
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
                        "$values":"singleton StringBuilder 2"
                        },
                        "firstSpan":[
                        {
                        "$ref":"1",
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                            "$values": "singleton StringBuilder 2"
                          },
                          "firstSpan": [
                            {
                              "$ref": "1",
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderSpanPostFieldClassUnionRevisit> StringBuilderSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderSpanPostFieldClassUnionRevisit>(new StringBuilderSpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<StringBuilderOrSpanClassUnion>) [
                         (StringBuilderOrSpanClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrSpanClassUnion) null,
                         (StringBuilderOrSpanClassUnion($id: 2)) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3, null ],
                         (StringBuilderOrSpanClassUnion) [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrSpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderSpanPostFieldClassUnionRevisit {
                          firstSpan: (Span<StringBuilderOrSpanClassUnion>) [
                            (StringBuilderOrSpanClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrSpanClassUnion) null,
                            (StringBuilderOrSpanClassUnion($id: 2)) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3,
                              null
                            ],
                            (StringBuilderOrSpanClassUnion) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrSpanClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3",
                        null
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3",
                                null
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              null,
                              "new StringBuilder 2",
                              "new StringBuilder 3"
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
    public void StringBuilderSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringBuilderReadOnlySpanClassUnionRevisit> PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderReadOnlySpanClassUnionRevisit>(new PreFieldStringBuilderReadOnlySpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderReadOnlySpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton StringBuilder 2" },
                         firstReadOnlySpan: (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>) [
                         (StringBuilderOrReadOnlySpanClassUnion) { $ref: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrReadOnlySpanClassUnion) null,
                         (StringBuilderOrReadOnlySpanClassUnion($id: 2)) [ new StringBuilder 1, null, new StringBuilder 2, null, new StringBuilder 3 ],
                         (StringBuilderOrReadOnlySpanClassUnion) [ null, singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrReadOnlySpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderReadOnlySpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton StringBuilder 2"
                          },
                          firstReadOnlySpan: (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>) [
                            (StringBuilderOrReadOnlySpanClassUnion) {
                              $ref: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrReadOnlySpanClassUnion) null,
                            (StringBuilderOrReadOnlySpanClassUnion($id: 2)) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              null,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrReadOnlySpanClassUnion) [
                              null,
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrReadOnlySpanClassUnion) {
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
                        "$values":"singleton StringBuilder 2"
                        },
                        "firstReadOnlySpan":[
                        {
                        "$ref":"1",
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        null,
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        null,
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                            "$values": "singleton StringBuilder 2"
                          },
                          "firstReadOnlySpan": [
                            {
                              "$ref": "1",
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                null,
                                "new StringBuilder 2",
                                null,
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              null,
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderReadOnlySpanPostFieldClassUnionRevisit> StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderReadOnlySpanPostFieldClassUnionRevisit>(new StringBuilderReadOnlySpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>) [
                         (StringBuilderOrReadOnlySpanClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrReadOnlySpanClassUnion) [],
                         (StringBuilderOrReadOnlySpanClassUnion($id: 2)) [ null, singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrReadOnlySpanClassUnion) [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrReadOnlySpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderReadOnlySpanPostFieldClassUnionRevisit {
                          firstReadOnlySpan: (ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion>) [
                            (StringBuilderOrReadOnlySpanClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrReadOnlySpanClassUnion) [],
                            (StringBuilderOrReadOnlySpanClassUnion($id: 2)) [
                              null,
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrReadOnlySpanClassUnion) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrReadOnlySpanClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              null,
                              "new StringBuilder 2",
                              "new StringBuilder 3"
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
    public void StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringBuilderListStructUnionRevisit> PrefieldStringBuilderListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringBuilderListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderListStructUnionRevisit>(new PreFieldStringBuilderListStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderListStructUnionRevisit {
                         firstPreField: null,
                         firstList: (List<StringBuilderOrListStructUnion>) [
                         (StringBuilderOrListStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrListStructUnion) null,
                         (StringBuilderOrListStructUnion) { $id: 2, $values: [ new StringBuilder 1, new StringBuilder 2, new StringBuilder 3, null ] },
                         (StringBuilderOrListStructUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrListStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderListStructUnionRevisit {
                          firstPreField: null,
                          firstList: (List<StringBuilderOrListStructUnion>) [
                            (StringBuilderOrListStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrListStructUnion) null,
                            (StringBuilderOrListStructUnion) {
                              $id: 2,
                              $values: [
                                new StringBuilder 1,
                                new StringBuilder 2,
                                new StringBuilder 3,
                                null
                              ]
                            },
                            (StringBuilderOrListStructUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrListStructUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3",
                        null
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                "new StringBuilder 3",
                                null
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderListPostFieldStructUnionRevisit> StringBuilderListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringBuilderListPostFieldStructUnionRevisit>(new StringBuilderListPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderListPostFieldStructUnionRevisit {
                         firstList: (List<StringBuilderOrListStructUnion>) [
                         (StringBuilderOrListStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrListStructUnion) [],
                         (StringBuilderOrListStructUnion) { $id: 2, $values: [ singleton StringBuilder 1, null, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ] },
                         (StringBuilderOrListStructUnion) [ new StringBuilder 1, new StringBuilder 2, null, new StringBuilder 3 ],
                         (StringBuilderOrListStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton StringBuilder 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderListPostFieldStructUnionRevisit {
                          firstList: (List<StringBuilderOrListStructUnion>) [
                            (StringBuilderOrListStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrListStructUnion) [],
                            (StringBuilderOrListStructUnion) {
                              $id: 2,
                              $values: [
                                singleton StringBuilder 1,
                                null,
                                {
                                  $ref: 1,
                                  $values: singleton StringBuilder 2
                                },
                                singleton StringBuilder 3
                              ]
                            },
                            (StringBuilderOrListStructUnion) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              null,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrListStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton StringBuilder 2"
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        null,
                        "new StringBuilder 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              "new StringBuilder 2",
                              null,
                              "new StringBuilder 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton StringBuilder 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (StringBuilderListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeStringBuilderInstances = true
         , InstanceMarkingIncludeStringBuilderContents   = true
         });
    }

    public static InputBearerExpect<PreFieldStringBuilderListClassUnionRevisit> PrefieldStringBuilderListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderListClassUnionRevisit>(new PreFieldStringBuilderListClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderListClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton StringBuilder 2" },
                         firstList: (List<StringBuilderOrListClassUnion>) [
                         (StringBuilderOrListClassUnion) { $ref: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrListClassUnion) null,
                         (StringBuilderOrListClassUnion($id: 2)) [ new StringBuilder 1, new StringBuilder 2, null, new StringBuilder 3 ],
                         (StringBuilderOrListClassUnion) [ singleton StringBuilder 1, null, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrListClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderListClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton StringBuilder 2"
                          },
                          firstList: (List<StringBuilderOrListClassUnion>) [
                            (StringBuilderOrListClassUnion) {
                              $ref: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrListClassUnion) null,
                            (StringBuilderOrListClassUnion($id: 2)) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              null,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrListClassUnion) [
                              singleton StringBuilder 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrListClassUnion) {
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
                        "$values":"singleton StringBuilder 2"
                        },
                        "firstList":[
                        {
                        "$ref":"1",
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        null,
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                            "$values": "singleton StringBuilder 2"
                          },
                          "firstList": [
                            {
                              "$ref": "1",
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                null,
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderListPostFieldClassUnionRevisit> StringBuilderListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderListPostFieldClassUnionRevisit>(new StringBuilderListPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderListPostFieldClassUnionRevisit {
                         firstList: (List<StringBuilderOrListClassUnion>) [
                         (StringBuilderOrListClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrListClassUnion) [],
                         (StringBuilderOrListClassUnion($id: 2)) [ null, singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrListClassUnion) [ new StringBuilder 1, new StringBuilder 2, new StringBuilder 3, null ],
                         (StringBuilderOrListClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderListPostFieldClassUnionRevisit {
                          firstList: (List<StringBuilderOrListClassUnion>) [
                            (StringBuilderOrListClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrListClassUnion) [],
                            (StringBuilderOrListClassUnion($id: 2)) [
                              null,
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrListClassUnion) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              new StringBuilder 3,
                              null
                            ],
                            (StringBuilderOrListClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3",
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              "new StringBuilder 2",
                              "new StringBuilder 3",
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
    public void StringBuilderListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderEnumerableStructUnionRevisit> PrefieldStringBuilderEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringBuilderEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderEnumerableStructUnionRevisit>(new PreFieldStringBuilderEnumerableStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderEnumerableStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<StringBuilderOrEnumerableStructUnion>) [
                         (StringBuilderOrEnumerableStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumerableStructUnion) [],
                         (StringBuilderOrEnumerableStructUnion) { $id: 2, $values: [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ] },
                         (StringBuilderOrEnumerableStructUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, null, singleton StringBuilder 3 ],
                         (StringBuilderOrEnumerableStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderEnumerableStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerable: (List<StringBuilderOrEnumerableStructUnion>) [
                            (StringBuilderOrEnumerableStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumerableStructUnion) [],
                            (StringBuilderOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                new StringBuilder 1,
                                null,
                                new StringBuilder 2,
                                new StringBuilder 3
                              ]
                            },
                            (StringBuilderOrEnumerableStructUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              null,
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumerableStructUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        null,
                        "singleton StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                null,
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              null,
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderEnumerablePostFieldStructUnionRevisit> StringBuilderEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringBuilderEnumerablePostFieldStructUnionRevisit>(new StringBuilderEnumerablePostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<StringBuilderOrEnumerableStructUnion>) [
                         (StringBuilderOrEnumerableStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumerableStructUnion) [],
                         (StringBuilderOrEnumerableStructUnion) { $id: 2, $values: [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, null, singleton StringBuilder 3 ] },
                         (StringBuilderOrEnumerableStructUnion) [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrEnumerableStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton StringBuilder 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderEnumerablePostFieldStructUnionRevisit {
                          firstEnumerable: (List<StringBuilderOrEnumerableStructUnion>) [
                            (StringBuilderOrEnumerableStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumerableStructUnion) [],
                            (StringBuilderOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                singleton StringBuilder 1,
                                {
                                  $ref: 1,
                                  $values: singleton StringBuilder 2
                                },
                                null,
                                singleton StringBuilder 3
                              ]
                            },
                            (StringBuilderOrEnumerableStructUnion) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumerableStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton StringBuilder 2"
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        null,
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                null,
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              null,
                              "new StringBuilder 2",
                              "new StringBuilder 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton StringBuilder 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderEnumerableClassUnionRevisit> PrefieldStringBuilderEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderEnumerableClassUnionRevisit>(new PreFieldStringBuilderEnumerableClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderEnumerableClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton StringBuilder 2" },
                         firstEnumerable: (List<StringBuilderOrEnumerableClassUnion>) [
                         (StringBuilderOrEnumerableClassUnion) { $ref: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumerableClassUnion) null,
                         (StringBuilderOrEnumerableClassUnion($id: 2)) [ null, new StringBuilder 1, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrEnumerableClassUnion) [ singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3, null ],
                         (StringBuilderOrEnumerableClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderEnumerableClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton StringBuilder 2"
                          },
                          firstEnumerable: (List<StringBuilderOrEnumerableClassUnion>) [
                            (StringBuilderOrEnumerableClassUnion) {
                              $ref: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumerableClassUnion) null,
                            (StringBuilderOrEnumerableClassUnion($id: 2)) [
                              null,
                              new StringBuilder 1,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumerableClassUnion) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3,
                              null
                            ],
                            (StringBuilderOrEnumerableClassUnion) {
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
                        "$values":"singleton StringBuilder 2"
                        },
                        "firstEnumerable":[
                        {
                        "$ref":"1",
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3",
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
                            "$values": "singleton StringBuilder 2"
                          },
                          "firstEnumerable": [
                            {
                              "$ref": "1",
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3",
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
    public void PrefieldStringBuilderEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderEnumerablePostFieldClassUnionRevisit> StringBuilderEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderEnumerablePostFieldClassUnionRevisit>(new StringBuilderEnumerablePostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<StringBuilderOrEnumerableClassUnion>) [
                         (StringBuilderOrEnumerableClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumerableClassUnion) [],
                         (StringBuilderOrEnumerableClassUnion($id: 2)) [ singleton StringBuilder 1, null, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3 ],
                         (StringBuilderOrEnumerableClassUnion) [ new StringBuilder 1, new StringBuilder 2, null, new StringBuilder 3 ],
                         (StringBuilderOrEnumerableClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderEnumerablePostFieldClassUnionRevisit {
                          firstEnumerable: (List<StringBuilderOrEnumerableClassUnion>) [
                            (StringBuilderOrEnumerableClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumerableClassUnion) [],
                            (StringBuilderOrEnumerableClassUnion($id: 2)) [
                              singleton StringBuilder 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumerableClassUnion) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              null,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumerableClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        null,
                        "new StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              "new StringBuilder 2",
                              null,
                              "new StringBuilder 3"
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
    public void StringBuilderEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderEnumeratorStructUnionRevisit> PrefieldStringBuilderEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldStringBuilderEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderEnumeratorStructUnionRevisit>(new PreFieldStringBuilderEnumeratorStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderEnumeratorStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<StringBuilderOrEnumeratorStructUnion>.Enumerator) [
                         (StringBuilderOrEnumeratorStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumeratorStructUnion) null,
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>($id: 2)) [
                         new StringBuilder 1, new StringBuilder 2, null, new StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                         singleton StringBuilder 1,
                         null,
                         { $ref: 1, $values: singleton StringBuilder 2 },
                         singleton StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderEnumeratorStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerator: (List<StringBuilderOrEnumeratorStructUnion>.Enumerator) [
                            (StringBuilderOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumeratorStructUnion) null,
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>($id: 2)) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              null,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                              singleton StringBuilder 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) {
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
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        null,
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                "new StringBuilder 2",
                                null,
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderEnumeratorPostFieldStructUnionRevisit> StringBuilderEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<StringBuilderEnumeratorPostFieldStructUnionRevisit>(new StringBuilderEnumeratorPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<StringBuilderOrEnumeratorStructUnion>.Enumerator) [
                         (StringBuilderOrEnumeratorStructUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumeratorStructUnion) [],
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>($id: 2)) [
                         null, singleton StringBuilder 1, { $ref: 1, $values: singleton StringBuilder 2 }, singleton StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) [ new StringBuilder 1, new StringBuilder 2, new StringBuilder 3, null ],
                         (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) { $ref: 2 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "singleton StringBuilder 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderEnumeratorPostFieldStructUnionRevisit {
                          firstEnumerator: (List<StringBuilderOrEnumeratorStructUnion>.Enumerator) [
                            (StringBuilderOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumeratorStructUnion) [],
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>($id: 2)) [
                              null,
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                              new StringBuilder 1,
                              new StringBuilder 2,
                              new StringBuilder 3,
                              null
                            ],
                            (StringBuilderOrEnumeratorStructUnion) (ReusableWrappingEnumerator<StringBuilder>) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton StringBuilder 2"
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        "new StringBuilder 2",
                        "new StringBuilder 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              "new StringBuilder 2",
                              "new StringBuilder 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton StringBuilder 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<PreFieldStringBuilderEnumeratorClassUnionRevisit> PrefieldStringBuilderEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldStringBuilderEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldStringBuilderEnumeratorClassUnionRevisit>(new PreFieldStringBuilderEnumeratorClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldStringBuilderEnumeratorClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton StringBuilder 2" },
                         firstEnumerator: (List<StringBuilderOrEnumeratorClassUnion>.Enumerator) [
                         (StringBuilderOrEnumeratorClassUnion) { $ref: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumeratorClassUnion) null,
                         (StringBuilderOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<StringBuilder>) [ new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3 ],
                         (StringBuilderOrEnumeratorClassUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                         singleton StringBuilder 1,
                         { $ref: 1, $values: singleton StringBuilder 2 },
                         null,
                         singleton StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldStringBuilderEnumeratorClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton StringBuilder 2"
                          },
                          firstEnumerator: (List<StringBuilderOrEnumeratorClassUnion>.Enumerator) [
                            (StringBuilderOrEnumeratorClassUnion) {
                              $ref: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumeratorClassUnion) null,
                            (StringBuilderOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<StringBuilder>) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorClassUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              null,
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorClassUnion) {
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
                        "$values":"singleton StringBuilder 2"
                        },
                        "firstEnumerator":[
                        {
                        "$ref":"1",
                        "$values":singleton StringBuilder 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
                        ]
                        },
                        [
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        null,
                        "singleton StringBuilder 3"
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
                            "$values": "singleton StringBuilder 2"
                          },
                          "firstEnumerator": [
                            {
                              "$ref": "1",
                              "$values": singleton StringBuilder 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new StringBuilder 1",
                                null,
                                "new StringBuilder 2",
                                "new StringBuilder 3"
                              ]
                            },
                            [
                              "singleton StringBuilder 1",
                              {
                                "$ref": "1",
                                "$values": "singleton StringBuilder 2"
                              },
                              null,
                              "singleton StringBuilder 3"
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
    public void PrefieldStringBuilderEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void PrefieldStringBuilderEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldStringBuilderEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    public static InputBearerExpect<StringBuilderEnumeratorPostFieldClassUnionRevisit> StringBuilderEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<StringBuilderEnumeratorPostFieldClassUnionRevisit>(new StringBuilderEnumeratorPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        StringBuilderEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<StringBuilderOrEnumeratorClassUnion>.Enumerator) [
                         (StringBuilderOrEnumeratorClassUnion) { $id: 1, $values: singleton StringBuilder 2 },
                         (StringBuilderOrEnumeratorClassUnion) [],
                         (StringBuilderOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<StringBuilder>) [
                         singleton StringBuilder 1,
                         { $ref: 1, $values: singleton StringBuilder 2 },
                         null,
                         singleton StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorClassUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                         new StringBuilder 1, null, new StringBuilder 2, new StringBuilder 3
                         ],
                         (StringBuilderOrEnumeratorClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        StringBuilderEnumeratorPostFieldClassUnionRevisit {
                          firstEnumerator: (List<StringBuilderOrEnumeratorClassUnion>.Enumerator) [
                            (StringBuilderOrEnumeratorClassUnion) {
                              $id: 1,
                              $values: singleton StringBuilder 2
                            },
                            (StringBuilderOrEnumeratorClassUnion) [],
                            (StringBuilderOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<StringBuilder>) [
                              singleton StringBuilder 1,
                              {
                                $ref: 1,
                                $values: singleton StringBuilder 2
                              },
                              null,
                              singleton StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorClassUnion) (ReusableWrappingEnumerator<StringBuilder>) [
                              new StringBuilder 1,
                              null,
                              new StringBuilder 2,
                              new StringBuilder 3
                            ],
                            (StringBuilderOrEnumeratorClassUnion) {
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
                        "$values":singleton StringBuilder 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton StringBuilder 1",
                        {
                        "$ref":"1",
                        "$values":"singleton StringBuilder 2"
                        },
                        null,
                        "singleton StringBuilder 3"
                        ]
                        },
                        [
                        "new StringBuilder 1",
                        null,
                        "new StringBuilder 2",
                        "new StringBuilder 3"
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
                              "$values": singleton StringBuilder 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton StringBuilder 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton StringBuilder 2"
                                },
                                null,
                                "singleton StringBuilder 3"
                              ]
                            },
                            [
                              "new StringBuilder 1",
                              null,
                              "new StringBuilder 2",
                              "new StringBuilder 3"
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
    public void StringBuilderEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }

    [TestMethod]
    public void StringBuilderEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (StringBuilderEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents   = true
             });
    }
}
