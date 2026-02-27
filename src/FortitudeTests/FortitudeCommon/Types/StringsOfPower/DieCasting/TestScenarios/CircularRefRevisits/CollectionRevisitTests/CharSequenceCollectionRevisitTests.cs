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
public class CharSequenceCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldCharSequenceArrayStructUnionRevisit>?  prefieldCharSequenceArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<CharSequenceArrayPostFieldStructUnionRevisit>? boolArrayPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceArrayClassUnionRevisit>?  prefieldCharSequenceArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceArrayPostFieldClassUnionRevisit>? boolArrayPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceSpanClassUnionRevisit>?  prefieldCharSequenceSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceSpanPostFieldClassUnionRevisit>? boolSpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceReadOnlySpanClassUnionRevisit>?  prefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceReadOnlySpanPostFieldClassUnionRevisit>? boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceListStructUnionRevisit>?  prefieldCharSequenceListNoRevealersStructUnionExpect;
    private static InputBearerExpect<CharSequenceListPostFieldStructUnionRevisit>? boolListPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceListClassUnionRevisit>?  prefieldCharSequenceListNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceListPostFieldClassUnionRevisit>? boolListPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceEnumerableStructUnionRevisit>?  prefieldCharSequenceEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<CharSequenceEnumerablePostFieldStructUnionRevisit>? boolEnumerablePostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceEnumerableClassUnionRevisit>?  prefieldCharSequenceEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceEnumerablePostFieldClassUnionRevisit>? boolEnumerablePostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceEnumeratorStructUnionRevisit>?  prefieldCharSequenceEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<CharSequenceEnumeratorPostFieldStructUnionRevisit>? boolEnumeratorPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldCharSequenceEnumeratorClassUnionRevisit>?  prefieldCharSequenceEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<CharSequenceEnumeratorPostFieldClassUnionRevisit>? boolEnumeratorPostFieldNoRevealersClassUnionExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldCharSequenceArrayStructUnionRevisit> PrefieldCharSequenceArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldCharSequenceArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceArrayStructUnionRevisit>(new PreFieldCharSequenceArrayStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceArrayStructUnionRevisit {
                         firstPreField: null,
                         firstArray: (CharSeqOrArrayStructUnion[]) [
                         (CharSeqOrArrayStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrArrayStructUnion) [],
                         (CharSeqOrArrayStructUnion) { $id: 2, $values: [ null, new CharSequence 1, new CharSequence 2, new CharSequence 3 ] },
                         (CharSeqOrArrayStructUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrArrayStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceArrayStructUnionRevisit {
                          firstPreField: null,
                          firstArray: (CharSeqOrArrayStructUnion[]) [
                            (CharSeqOrArrayStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrArrayStructUnion) [],
                            (CharSeqOrArrayStructUnion) {
                              $id: 2,
                              $values: [
                                null,
                                new CharSequence 1,
                                new CharSequence 2,
                                new CharSequence 3
                              ]
                            },
                            (CharSeqOrArrayStructUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrArrayStructUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new CharSequence 1",
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceArrayPostFieldStructUnionRevisit> CharSequenceArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<CharSequenceArrayPostFieldStructUnionRevisit>(new CharSequenceArrayPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceArrayPostFieldStructUnionRevisit {
                         firstArray: (CharSeqOrArrayStructUnion[]) [
                         (CharSeqOrArrayStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrArrayStructUnion) [],
                         (CharSeqOrArrayStructUnion) { $id: 4, $values: [
                         { $id: 2, $values: singleton CharSequence 1 },
                         { $ref: 1, $values: singleton CharSequence 2 },
                         { $id: 3, $values: singleton CharSequence 3 },
                         null
                         ]
                         },
                         (CharSeqOrArrayStructUnion) [
                         { $ref: 2, $values: singleton CharSequence 1 },
                         { $ref: 1, $values: singleton CharSequence 2 },
                         { $ref: 3, $values: singleton CharSequence 3 }
                         ],
                         (CharSeqOrArrayStructUnion) { $ref: 4 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "singleton CharSequence 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceArrayPostFieldStructUnionRevisit {
                          firstArray: (CharSeqOrArrayStructUnion[]) [
                            (CharSeqOrArrayStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrArrayStructUnion) [],
                            (CharSeqOrArrayStructUnion) {
                              $id: 4,
                              $values: [
                                {
                                  $id: 2,
                                  $values: singleton CharSequence 1
                                },
                                {
                                  $ref: 1,
                                  $values: singleton CharSequence 2
                                },
                                {
                                  $id: 3,
                                  $values: singleton CharSequence 3
                                },
                                null
                              ]
                            },
                            (CharSeqOrArrayStructUnion) [
                              {
                                $ref: 2,
                                $values: singleton CharSequence 1
                              },
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              {
                                $ref: 3,
                                $values: singleton CharSequence 3
                              }
                            ],
                            (CharSeqOrArrayStructUnion) {
                              $ref: 4
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton CharSequence 2"
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"4",
                        "$values":[
                        {
                        "$id":"2",
                        "$values":"singleton CharSequence 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        {
                        "$id":"3",
                        "$values":"singleton CharSequence 3"
                        },
                        null
                        ]
                        },
                        [
                        {
                        "$ref":"2",
                        "$values":"singleton CharSequence 1"
                        },
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        {
                        "$ref":"3",
                        "$values":"singleton CharSequence 3"
                        }
                        ],
                        {
                        "$ref":"4"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "4",
                              "$values": [
                                {
                                  "$id": "2",
                                  "$values": "singleton CharSequence 1"
                                },
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                {
                                  "$id": "3",
                                  "$values": "singleton CharSequence 3"
                                },
                                null
                              ]
                            },
                            [
                              {
                                "$ref": "2",
                                "$values": "singleton CharSequence 1"
                              },
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              {
                                "$ref": "3",
                                "$values": "singleton CharSequence 3"
                              }
                            ],
                            {
                              "$ref": "4"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton CharSequence 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }


    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceArrayClassUnionRevisit> PrefieldCharSequenceArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceArrayClassUnionRevisit>(new PreFieldCharSequenceArrayClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (CharSeqOrArrayClassUnion[]) [
                         (CharSeqOrArrayClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrArrayClassUnion) null,
                         (CharSeqOrArrayClassUnion($id: 2)) [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrArrayClassUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrArrayClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceArrayClassUnionRevisit {
                          firstPreField: null,
                          firstArray: (CharSeqOrArrayClassUnion[]) [
                            (CharSeqOrArrayClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrArrayClassUnion) null,
                            (CharSeqOrArrayClassUnion($id: 2)) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrArrayClassUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrArrayClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                null,
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceArrayNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceArrayPostFieldClassUnionRevisit> CharSequenceArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceArrayPostFieldClassUnionRevisit>(new CharSequenceArrayPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceArrayPostFieldClassUnionRevisit {
                         firstArray: (CharSeqOrArrayClassUnion[]) [
                         (CharSeqOrArrayClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrArrayClassUnion) [],
                         (CharSeqOrArrayClassUnion($id: 2)) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, null, singleton CharSequence 3 ],
                         (CharSeqOrArrayClassUnion) [ new CharSequence 1, new CharSequence 2, new CharSequence 3, null ],
                         (CharSeqOrArrayClassUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton CharSequence 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceArrayPostFieldClassUnionRevisit {
                          firstArray: (CharSeqOrArrayClassUnion[]) [
                            (CharSeqOrArrayClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrArrayClassUnion) [],
                            (CharSeqOrArrayClassUnion($id: 2)) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              null,
                              singleton CharSequence 3
                            ],
                            (CharSeqOrArrayClassUnion) [
                              new CharSequence 1,
                              new CharSequence 2,
                              new CharSequence 3,
                              null
                            ],
                            (CharSeqOrArrayClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton CharSequence 2"
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        null,
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                null,
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              "new CharSequence 2",
                              "new CharSequence 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton CharSequence 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceArrayPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceSpanClassUnionRevisit> PrefieldCharSequenceSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceSpanClassUnionRevisit>(new PreFieldCharSequenceSpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceSpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton CharSequence 2" },
                         firstSpan: (Span<CharSeqOrSpanClassUnion>) [
                         (CharSeqOrSpanClassUnion) { $ref: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrSpanClassUnion) [],
                         (CharSeqOrSpanClassUnion($id: 2)) [ null, new CharSequence 1, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrSpanClassUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrSpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceSpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton CharSequence 2"
                          },
                          firstSpan: (Span<CharSeqOrSpanClassUnion>) [
                            (CharSeqOrSpanClassUnion) {
                              $ref: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrSpanClassUnion) [],
                            (CharSeqOrSpanClassUnion($id: 2)) [
                              null,
                              new CharSequence 1,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrSpanClassUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrSpanClassUnion) {
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
                        "$values":"singleton CharSequence 2"
                        },
                        "firstSpan":[
                        {
                        "$ref":"1",
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                            "$values": "singleton CharSequence 2"
                          },
                          "firstSpan": [
                            {
                              "$ref": "1",
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new CharSequence 1",
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceSpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceSpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceSpanPostFieldClassUnionRevisit> CharSequenceSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceSpanPostFieldClassUnionRevisit>(new CharSequenceSpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<CharSeqOrSpanClassUnion>) [
                         (CharSeqOrSpanClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrSpanClassUnion) null,
                         (CharSeqOrSpanClassUnion($id: 2)) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3, null ],
                         (CharSeqOrSpanClassUnion) [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrSpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceSpanPostFieldClassUnionRevisit {
                          firstSpan: (Span<CharSeqOrSpanClassUnion>) [
                            (CharSeqOrSpanClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrSpanClassUnion) null,
                            (CharSeqOrSpanClassUnion($id: 2)) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3,
                              null
                            ],
                            (CharSeqOrSpanClassUnion) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrSpanClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3",
                        null
                        ]
                        },
                        [
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3",
                                null
                              ]
                            },
                            [
                              "new CharSequence 1",
                              null,
                              "new CharSequence 2",
                              "new CharSequence 3"
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
    public void CharSequenceSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceSpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    public static InputBearerExpect<PreFieldCharSequenceReadOnlySpanClassUnionRevisit> PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceReadOnlySpanClassUnionRevisit>(new PreFieldCharSequenceReadOnlySpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceReadOnlySpanClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton CharSequence 2" },
                         firstReadOnlySpan: (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>) [
                         (CharSeqOrReadOnlySpanClassUnion) { $ref: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrReadOnlySpanClassUnion) null,
                         (CharSeqOrReadOnlySpanClassUnion($id: 2)) [ new CharSequence 1, null, new CharSequence 2, null, new CharSequence 3 ],
                         (CharSeqOrReadOnlySpanClassUnion) [ null, singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrReadOnlySpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceReadOnlySpanClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton CharSequence 2"
                          },
                          firstReadOnlySpan: (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>) [
                            (CharSeqOrReadOnlySpanClassUnion) {
                              $ref: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrReadOnlySpanClassUnion) null,
                            (CharSeqOrReadOnlySpanClassUnion($id: 2)) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              null,
                              new CharSequence 3
                            ],
                            (CharSeqOrReadOnlySpanClassUnion) [
                              null,
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrReadOnlySpanClassUnion) {
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
                        "$values":"singleton CharSequence 2"
                        },
                        "firstReadOnlySpan":[
                        {
                        "$ref":"1",
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        null,
                        "new CharSequence 3"
                        ]
                        },
                        [
                        null,
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                            "$values": "singleton CharSequence 2"
                          },
                          "firstReadOnlySpan": [
                            {
                              "$ref": "1",
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                null,
                                "new CharSequence 2",
                                null,
                                "new CharSequence 3"
                              ]
                            },
                            [
                              null,
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceReadOnlySpanNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceReadOnlySpanPostFieldClassUnionRevisit> CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceReadOnlySpanPostFieldClassUnionRevisit>(new CharSequenceReadOnlySpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>) [
                         (CharSeqOrReadOnlySpanClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrReadOnlySpanClassUnion) [],
                         (CharSeqOrReadOnlySpanClassUnion($id: 2)) [ null, singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrReadOnlySpanClassUnion) [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrReadOnlySpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceReadOnlySpanPostFieldClassUnionRevisit {
                          firstReadOnlySpan: (ReadOnlySpan<CharSeqOrReadOnlySpanClassUnion>) [
                            (CharSeqOrReadOnlySpanClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrReadOnlySpanClassUnion) [],
                            (CharSeqOrReadOnlySpanClassUnion($id: 2)) [
                              null,
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrReadOnlySpanClassUnion) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrReadOnlySpanClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              null,
                              "new CharSequence 2",
                              "new CharSequence 3"
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
    public void CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceReadOnlySpanPostFieldNoRevealersClassUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    public static InputBearerExpect<PreFieldCharSequenceListStructUnionRevisit> PrefieldCharSequenceListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldCharSequenceListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceListStructUnionRevisit>(new PreFieldCharSequenceListStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceListStructUnionRevisit {
                         firstPreField: null,
                         firstList: (List<CharSeqOrListStructUnion>) [
                         (CharSeqOrListStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrListStructUnion) null,
                         (CharSeqOrListStructUnion) { $id: 2, $values: [ new CharSequence 1, new CharSequence 2, new CharSequence 3, null ] },
                         (CharSeqOrListStructUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrListStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceListStructUnionRevisit {
                          firstPreField: null,
                          firstList: (List<CharSeqOrListStructUnion>) [
                            (CharSeqOrListStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrListStructUnion) null,
                            (CharSeqOrListStructUnion) {
                              $id: 2,
                              $values: [
                                new CharSequence 1,
                                new CharSequence 2,
                                new CharSequence 3,
                                null
                              ]
                            },
                            (CharSeqOrListStructUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrListStructUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3",
                        null
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                "new CharSequence 2",
                                "new CharSequence 3",
                                null
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceListPostFieldStructUnionRevisit> CharSequenceListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<CharSequenceListPostFieldStructUnionRevisit>(new CharSequenceListPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceListPostFieldStructUnionRevisit {
                         firstList: (List<CharSeqOrListStructUnion>) [
                         (CharSeqOrListStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrListStructUnion) [],
                         (CharSeqOrListStructUnion) { $id: 2, $values: [ singleton CharSequence 1, null, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ] },
                         (CharSeqOrListStructUnion) [ new CharSequence 1, new CharSequence 2, null, new CharSequence 3 ],
                         (CharSeqOrListStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton CharSequence 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceListPostFieldStructUnionRevisit {
                          firstList: (List<CharSeqOrListStructUnion>) [
                            (CharSeqOrListStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrListStructUnion) [],
                            (CharSeqOrListStructUnion) {
                              $id: 2,
                              $values: [
                                singleton CharSequence 1,
                                null,
                                {
                                  $ref: 1,
                                  $values: singleton CharSequence 2
                                },
                                singleton CharSequence 3
                              ]
                            },
                            (CharSeqOrListStructUnion) [
                              new CharSequence 1,
                              new CharSequence 2,
                              null,
                              new CharSequence 3
                            ],
                            (CharSeqOrListStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton CharSequence 2"
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        "new CharSequence 2",
                        null,
                        "new CharSequence 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              "new CharSequence 2",
                              null,
                              "new CharSequence 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton CharSequence 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(CompactJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyLog)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectationWithOptions
        (CharSequenceListPostFieldNoRevealersStructUnionExpect
       , new StyleOptions(PrettyJson)
         {
           InstanceTrackingIncludeCharSequenceInstances = true
         , InstanceMarkingIncludeCharSequenceContents   = true
         });
    }

    public static InputBearerExpect<PreFieldCharSequenceListClassUnionRevisit> PrefieldCharSequenceListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceListClassUnionRevisit>(new PreFieldCharSequenceListClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceListClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton CharSequence 2" },
                         firstList: (List<CharSeqOrListClassUnion>) [
                         (CharSeqOrListClassUnion) { $ref: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrListClassUnion) null,
                         (CharSeqOrListClassUnion($id: 2)) [ new CharSequence 1, new CharSequence 2, null, new CharSequence 3 ],
                         (CharSeqOrListClassUnion) [ singleton CharSequence 1, null, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrListClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceListClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton CharSequence 2"
                          },
                          firstList: (List<CharSeqOrListClassUnion>) [
                            (CharSeqOrListClassUnion) {
                              $ref: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrListClassUnion) null,
                            (CharSeqOrListClassUnion($id: 2)) [
                              new CharSequence 1,
                              new CharSequence 2,
                              null,
                              new CharSequence 3
                            ],
                            (CharSeqOrListClassUnion) [
                              singleton CharSequence 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrListClassUnion) {
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
                        "$values":"singleton CharSequence 2"
                        },
                        "firstList":[
                        {
                        "$ref":"1",
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        "new CharSequence 2",
                        null,
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                            "$values": "singleton CharSequence 2"
                          },
                          "firstList": [
                            {
                              "$ref": "1",
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                "new CharSequence 2",
                                null,
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceListNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceListPostFieldClassUnionRevisit> CharSequenceListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceListPostFieldClassUnionRevisit>(new CharSequenceListPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceListPostFieldClassUnionRevisit {
                         firstList: (List<CharSeqOrListClassUnion>) [
                         (CharSeqOrListClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrListClassUnion) [],
                         (CharSeqOrListClassUnion($id: 2)) [ null, singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrListClassUnion) [ new CharSequence 1, new CharSequence 2, new CharSequence 3, null ],
                         (CharSeqOrListClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceListPostFieldClassUnionRevisit {
                          firstList: (List<CharSeqOrListClassUnion>) [
                            (CharSeqOrListClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrListClassUnion) [],
                            (CharSeqOrListClassUnion($id: 2)) [
                              null,
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrListClassUnion) [
                              new CharSequence 1,
                              new CharSequence 2,
                              new CharSequence 3,
                              null
                            ],
                            (CharSeqOrListClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3",
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              "new CharSequence 2",
                              "new CharSequence 3",
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
    public void CharSequenceListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceListPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceEnumerableStructUnionRevisit> PrefieldCharSequenceEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldCharSequenceEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceEnumerableStructUnionRevisit>(new PreFieldCharSequenceEnumerableStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceEnumerableStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<CharSeqOrEnumerableStructUnion>) [
                         (CharSeqOrEnumerableStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumerableStructUnion) [],
                         (CharSeqOrEnumerableStructUnion) { $id: 2, $values: [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ] },
                         (CharSeqOrEnumerableStructUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, null, singleton CharSequence 3 ],
                         (CharSeqOrEnumerableStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceEnumerableStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerable: (List<CharSeqOrEnumerableStructUnion>) [
                            (CharSeqOrEnumerableStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumerableStructUnion) [],
                            (CharSeqOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                new CharSequence 1,
                                null,
                                new CharSequence 2,
                                new CharSequence 3
                              ]
                            },
                            (CharSeqOrEnumerableStructUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              null,
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumerableStructUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        null,
                        "singleton CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                null,
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              null,
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceEnumerablePostFieldStructUnionRevisit> CharSequenceEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<CharSequenceEnumerablePostFieldStructUnionRevisit>(new CharSequenceEnumerablePostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<CharSeqOrEnumerableStructUnion>) [
                         (CharSeqOrEnumerableStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumerableStructUnion) [],
                         (CharSeqOrEnumerableStructUnion) { $id: 2, $values: [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, null, singleton CharSequence 3 ] },
                         (CharSeqOrEnumerableStructUnion) [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrEnumerableStructUnion) { $ref: 2 }
                         ],
                         firstPostField: { $ref: 1, $values: "singleton CharSequence 2" }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceEnumerablePostFieldStructUnionRevisit {
                          firstEnumerable: (List<CharSeqOrEnumerableStructUnion>) [
                            (CharSeqOrEnumerableStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumerableStructUnion) [],
                            (CharSeqOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                singleton CharSequence 1,
                                {
                                  $ref: 1,
                                  $values: singleton CharSequence 2
                                },
                                null,
                                singleton CharSequence 3
                              ]
                            },
                            (CharSeqOrEnumerableStructUnion) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumerableStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton CharSequence 2"
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        null,
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                null,
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              null,
                              "new CharSequence 2",
                              "new CharSequence 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton CharSequence 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceEnumerableClassUnionRevisit> PrefieldCharSequenceEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceEnumerableClassUnionRevisit>(new PreFieldCharSequenceEnumerableClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceEnumerableClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton CharSequence 2" },
                         firstEnumerable: (List<CharSeqOrEnumerableClassUnion>) [
                         (CharSeqOrEnumerableClassUnion) { $ref: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumerableClassUnion) null,
                         (CharSeqOrEnumerableClassUnion($id: 2)) [ null, new CharSequence 1, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrEnumerableClassUnion) [ singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3, null ],
                         (CharSeqOrEnumerableClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceEnumerableClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton CharSequence 2"
                          },
                          firstEnumerable: (List<CharSeqOrEnumerableClassUnion>) [
                            (CharSeqOrEnumerableClassUnion) {
                              $ref: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumerableClassUnion) null,
                            (CharSeqOrEnumerableClassUnion($id: 2)) [
                              null,
                              new CharSequence 1,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumerableClassUnion) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3,
                              null
                            ],
                            (CharSeqOrEnumerableClassUnion) {
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
                        "$values":"singleton CharSequence 2"
                        },
                        "firstEnumerable":[
                        {
                        "$ref":"1",
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3",
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
                            "$values": "singleton CharSequence 2"
                          },
                          "firstEnumerable": [
                            {
                              "$ref": "1",
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new CharSequence 1",
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3",
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
    public void PrefieldCharSequenceEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumerableNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceEnumerablePostFieldClassUnionRevisit> CharSequenceEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceEnumerablePostFieldClassUnionRevisit>(new CharSequenceEnumerablePostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<CharSeqOrEnumerableClassUnion>) [
                         (CharSeqOrEnumerableClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumerableClassUnion) [],
                         (CharSeqOrEnumerableClassUnion($id: 2)) [ singleton CharSequence 1, null, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3 ],
                         (CharSeqOrEnumerableClassUnion) [ new CharSequence 1, new CharSequence 2, null, new CharSequence 3 ],
                         (CharSeqOrEnumerableClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceEnumerablePostFieldClassUnionRevisit {
                          firstEnumerable: (List<CharSeqOrEnumerableClassUnion>) [
                            (CharSeqOrEnumerableClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumerableClassUnion) [],
                            (CharSeqOrEnumerableClassUnion($id: 2)) [
                              singleton CharSequence 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumerableClassUnion) [
                              new CharSequence 1,
                              new CharSequence 2,
                              null,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumerableClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        "new CharSequence 2",
                        null,
                        "new CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                null,
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              "new CharSequence 2",
                              null,
                              "new CharSequence 3"
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
    public void CharSequenceEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumerablePostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceEnumeratorStructUnionRevisit> PrefieldCharSequenceEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldCharSequenceEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceEnumeratorStructUnionRevisit>(new PreFieldCharSequenceEnumeratorStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceEnumeratorStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<CharSeqOrEnumeratorStructUnion>.Enumerator) [
                         (CharSeqOrEnumeratorStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumeratorStructUnion) null,
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>($id: 2)) [
                         new CharSequence 1, new CharSequence 2, null, new CharSequence 3
                         ],
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                         singleton CharSequence 1,
                         null,
                         { $ref: 1, $values: singleton CharSequence 2 },
                         singleton CharSequence 3
                         ],
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceEnumeratorStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerator: (List<CharSeqOrEnumeratorStructUnion>.Enumerator) [
                            (CharSeqOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumeratorStructUnion) null,
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>($id: 2)) [
                              new CharSequence 1,
                              new CharSequence 2,
                              null,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                              singleton CharSequence 1,
                              null,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) {
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
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        "new CharSequence 2",
                        null,
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        null,
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                "new CharSequence 2",
                                null,
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              null,
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceEnumeratorPostFieldStructUnionRevisit> CharSequenceEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<CharSequenceEnumeratorPostFieldStructUnionRevisit>(new CharSequenceEnumeratorPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<CharSeqOrEnumeratorStructUnion>.Enumerator) [
                         (CharSeqOrEnumeratorStructUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumeratorStructUnion) [],
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>($id: 2)) [
                         null, singleton CharSequence 1, { $ref: 1, $values: singleton CharSequence 2 }, singleton CharSequence 3
                         ],
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) [ new CharSequence 1, new CharSequence 2, new CharSequence 3, null ],
                         (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) { $ref: 2 }
                         ],
                         firstPostField: {
                         $ref: 1,
                         $values: "singleton CharSequence 2"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceEnumeratorPostFieldStructUnionRevisit {
                          firstEnumerator: (List<CharSeqOrEnumeratorStructUnion>.Enumerator) [
                            (CharSeqOrEnumeratorStructUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumeratorStructUnion) [],
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>($id: 2)) [
                              null,
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                              new CharSequence 1,
                              new CharSequence 2,
                              new CharSequence 3,
                              null
                            ],
                            (CharSeqOrEnumeratorStructUnion) (ReusableWrappingEnumerator<ICharSequence>) {
                              $ref: 2
                            }
                          ],
                          firstPostField: {
                            $ref: 1,
                            $values: "singleton CharSequence 2"
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        "new CharSequence 2",
                        "new CharSequence 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              "new CharSequence 2",
                              "new CharSequence 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1",
                            "$values": "singleton CharSequence 2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersStructUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<PreFieldCharSequenceEnumeratorClassUnionRevisit> PrefieldCharSequenceEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldCharSequenceEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldCharSequenceEnumeratorClassUnionRevisit>(new PreFieldCharSequenceEnumeratorClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldCharSequenceEnumeratorClassUnionRevisit {
                         firstPreField: { $id: 1, $values: "singleton CharSequence 2" },
                         firstEnumerator: (List<CharSeqOrEnumeratorClassUnion>.Enumerator) [
                         (CharSeqOrEnumeratorClassUnion) { $ref: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumeratorClassUnion) null,
                         (CharSeqOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<ICharSequence>) [ new CharSequence 1, null, new CharSequence 2, new CharSequence 3 ],
                         (CharSeqOrEnumeratorClassUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                         singleton CharSequence 1,
                         { $ref: 1, $values: singleton CharSequence 2 },
                         null,
                         singleton CharSequence 3
                         ],
                         (CharSeqOrEnumeratorClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldCharSequenceEnumeratorClassUnionRevisit {
                          firstPreField: {
                            $id: 1,
                            $values: "singleton CharSequence 2"
                          },
                          firstEnumerator: (List<CharSeqOrEnumeratorClassUnion>.Enumerator) [
                            (CharSeqOrEnumeratorClassUnion) {
                              $ref: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumeratorClassUnion) null,
                            (CharSeqOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<ICharSequence>) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumeratorClassUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              null,
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumeratorClassUnion) {
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
                        "$values":"singleton CharSequence 2"
                        },
                        "firstEnumerator":[
                        {
                        "$ref":"1",
                        "$values":singleton CharSequence 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
                        ]
                        },
                        [
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        null,
                        "singleton CharSequence 3"
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
                            "$values": "singleton CharSequence 2"
                          },
                          "firstEnumerator": [
                            {
                              "$ref": "1",
                              "$values": singleton CharSequence 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new CharSequence 1",
                                null,
                                "new CharSequence 2",
                                "new CharSequence 3"
                              ]
                            },
                            [
                              "singleton CharSequence 1",
                              {
                                "$ref": "1",
                                "$values": "singleton CharSequence 2"
                              },
                              null,
                              "singleton CharSequence 3"
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
    public void PrefieldCharSequenceEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void PrefieldCharSequenceEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (PrefieldCharSequenceEnumeratorNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    public static InputBearerExpect<CharSequenceEnumeratorPostFieldClassUnionRevisit> CharSequenceEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<CharSequenceEnumeratorPostFieldClassUnionRevisit>(new CharSequenceEnumeratorPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        CharSequenceEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<CharSeqOrEnumeratorClassUnion>.Enumerator) [
                         (CharSeqOrEnumeratorClassUnion) { $id: 1, $values: singleton CharSequence 2 },
                         (CharSeqOrEnumeratorClassUnion) [],
                         (CharSeqOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<ICharSequence>) [
                         singleton CharSequence 1,
                         { $ref: 1, $values: singleton CharSequence 2 },
                         null,
                         singleton CharSequence 3
                         ],
                         (CharSeqOrEnumeratorClassUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                         new CharSequence 1, null, new CharSequence 2, new CharSequence 3
                         ],
                         (CharSeqOrEnumeratorClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        CharSequenceEnumeratorPostFieldClassUnionRevisit {
                          firstEnumerator: (List<CharSeqOrEnumeratorClassUnion>.Enumerator) [
                            (CharSeqOrEnumeratorClassUnion) {
                              $id: 1,
                              $values: singleton CharSequence 2
                            },
                            (CharSeqOrEnumeratorClassUnion) [],
                            (CharSeqOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<ICharSequence>) [
                              singleton CharSequence 1,
                              {
                                $ref: 1,
                                $values: singleton CharSequence 2
                              },
                              null,
                              singleton CharSequence 3
                            ],
                            (CharSeqOrEnumeratorClassUnion) (ReusableWrappingEnumerator<ICharSequence>) [
                              new CharSequence 1,
                              null,
                              new CharSequence 2,
                              new CharSequence 3
                            ],
                            (CharSeqOrEnumeratorClassUnion) {
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
                        "$values":singleton CharSequence 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton CharSequence 1",
                        {
                        "$ref":"1",
                        "$values":"singleton CharSequence 2"
                        },
                        null,
                        "singleton CharSequence 3"
                        ]
                        },
                        [
                        "new CharSequence 1",
                        null,
                        "new CharSequence 2",
                        "new CharSequence 3"
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
                              "$values": singleton CharSequence 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton CharSequence 1",
                                {
                                  "$ref": "1",
                                  "$values": "singleton CharSequence 2"
                                },
                                null,
                                "singleton CharSequence 3"
                              ]
                            },
                            [
                              "new CharSequence 1",
                              null,
                              "new CharSequence 2",
                              "new CharSequence 3"
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
    public void CharSequenceEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void CharSequenceEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (CharSequenceEnumeratorPostFieldNoRevealersClassUnionExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }
}
