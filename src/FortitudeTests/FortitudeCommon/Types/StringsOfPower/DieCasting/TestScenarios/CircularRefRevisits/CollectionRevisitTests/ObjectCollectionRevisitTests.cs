// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.CollectionRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class ObjectCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldObjectArrayStructUnionRevisit>?  prefieldObjectArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<ObjectArrayPostFieldStructUnionRevisit>? boolArrayPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldObjectArrayClassUnionRevisit>?  prefieldObjectArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectArrayPostFieldClassUnionRevisit>? boolArrayPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldObjectSpanClassUnionRevisit>?  prefieldObjectSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectSpanPostFieldClassUnionRevisit>? boolSpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldObjectReadOnlySpanClassUnionRevisit>?  prefieldObjectReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectReadOnlySpanPostFieldClassUnionRevisit>? boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldObjectListStructUnionRevisit>?  prefieldObjectListNoRevealersStructUnionExpect;
    private static InputBearerExpect<ObjectListPostFieldStructUnionRevisit>? boolListPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldObjectListClassUnionRevisit>?  prefieldObjectListNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectListPostFieldClassUnionRevisit>? boolListPostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldObjectEnumerableStructUnionRevisit>?  prefieldObjectEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<ObjectEnumerablePostFieldStructUnionRevisit>? boolEnumerablePostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldObjectEnumerableClassUnionRevisit>?  prefieldObjectEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectEnumerablePostFieldClassUnionRevisit>? boolEnumerablePostFieldNoRevealersClassUnionExpect;

    private static InputBearerExpect<PreFieldObjectEnumeratorStructUnionRevisit>?  prefieldObjectEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<ObjectEnumeratorPostFieldStructUnionRevisit>? boolEnumeratorPostFieldNoRevealersStructUnionExpect;

    private static InputBearerExpect<PreFieldObjectEnumeratorClassUnionRevisit>?  prefieldObjectEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<ObjectEnumeratorPostFieldClassUnionRevisit>? boolEnumeratorPostFieldNoRevealersClassUnionExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldObjectArrayStructUnionRevisit> PrefieldObjectArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldObjectArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldObjectArrayStructUnionRevisit>(new PreFieldObjectArrayStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectArrayStructUnionRevisit {
                         firstPreField: null,
                         firstArray: (ObjectOrArrayStructUnion[]) [
                         (ObjectOrArrayStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrArrayStructUnion) [],
                         (ObjectOrArrayStructUnion) { $id: 2, $values: [
                         null,
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ]
                         },
                         (ObjectOrArrayStructUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrArrayStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectArrayStructUnionRevisit {
                          firstPreField: null,
                          firstArray: (ObjectOrArrayStructUnion[]) [
                            (ObjectOrArrayStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrArrayStructUnion) [],
                            (ObjectOrArrayStructUnion) {
                              $id: 2,
                              $values: [
                                null,
                                (MyOtherTypeClass) new Object 1,
                                (MyOtherTypeClass) new Object 2,
                                (MyOtherTypeClass) new Object 3
                              ]
                            },
                            (ObjectOrArrayStructUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrArrayStructUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new Object 1",
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new Object 1",
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectArrayNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectArrayPostFieldStructUnionRevisit> ObjectArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<ObjectArrayPostFieldStructUnionRevisit>(new ObjectArrayPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectArrayPostFieldStructUnionRevisit {
                         firstArray: (ObjectOrArrayStructUnion[]) [
                         (ObjectOrArrayStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrArrayStructUnion) [],
                         (ObjectOrArrayStructUnion) { $id: 4, $values: [
                         (MyOtherTypeClass($id: 2)) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass($id: 3)) singleton Object 3,
                         null
                         ]
                         },
                         (ObjectOrArrayStructUnion) [
                         (MyOtherTypeClass) { $ref: 2 },
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) { $ref: 3 }
                         ],
                         (ObjectOrArrayStructUnion) { $ref: 4 }
                         ],
                         firstPostField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectArrayPostFieldStructUnionRevisit {
                          firstArray: (ObjectOrArrayStructUnion[]) [
                            (ObjectOrArrayStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrArrayStructUnion) [],
                            (ObjectOrArrayStructUnion) {
                              $id: 4,
                              $values: [
                                (MyOtherTypeClass($id: 2)) singleton Object 1,
                                (MyOtherTypeClass) {
                                  $ref: 1
                                },
                                (MyOtherTypeClass($id: 3)) singleton Object 3,
                                null
                              ]
                            },
                            (ObjectOrArrayStructUnion) [
                              (MyOtherTypeClass) {
                                $ref: 2
                              },
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) {
                                $ref: 3
                              }
                            ],
                            (ObjectOrArrayStructUnion) {
                              $ref: 4
                            }
                          ],
                          firstPostField: (MyOtherTypeClass) {
                            $ref: 1
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"4",
                        "$values":[
                        {
                        "$id":"2",
                        "$values":"singleton Object 1"
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "$id":"3",
                        "$values":"singleton Object 3"
                        },
                        null
                        ]
                        },
                        [
                        {
                        "$ref":"2"
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "$ref":"3"
                        }
                        ],
                        {
                        "$ref":"4"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "4",
                              "$values": [
                                {
                                  "$id": "2",
                                  "$values": "singleton Object 1"
                                },
                                {
                                  "$ref": "1"
                                },
                                {
                                  "$id": "3",
                                  "$values": "singleton Object 3"
                                },
                                null
                              ]
                            },
                            [
                              {
                                "$ref": "2"
                              },
                              {
                                "$ref": "1"
                              },
                              {
                                "$ref": "3"
                              }
                            ],
                            {
                              "$ref": "4"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }


    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectArrayClassUnionRevisit> PrefieldObjectArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectArrayClassUnionRevisit>(new PreFieldObjectArrayClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (ObjectOrArrayClassUnion[]) [
                         (ObjectOrArrayClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrArrayClassUnion) null,
                         (ObjectOrArrayClassUnion($id: 2)) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrArrayClassUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrArrayClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectArrayClassUnionRevisit {
                          firstPreField: null,
                          firstArray: (ObjectOrArrayClassUnion[]) [
                            (ObjectOrArrayClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrArrayClassUnion) null,
                            (ObjectOrArrayClassUnion($id: 2)) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrArrayClassUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrArrayClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                              "$values": singleton Object 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                null,
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectArrayNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectArrayNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectArrayPostFieldClassUnionRevisit> ObjectArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectArrayPostFieldClassUnionRevisit>(new ObjectArrayPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectArrayPostFieldClassUnionRevisit {
                         firstArray: (ObjectOrArrayClassUnion[]) [
                         (ObjectOrArrayClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrArrayClassUnion) [],
                         (ObjectOrArrayClassUnion($id: 2)) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         null,
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrArrayClassUnion) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3,
                         null
                         ],
                         (ObjectOrArrayClassUnion) { $ref: 2 }
                         ],
                         firstPostField: (MyOtherTypeClass) { $ref: 1 }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectArrayPostFieldClassUnionRevisit {
                          firstArray: (ObjectOrArrayClassUnion[]) [
                            (ObjectOrArrayClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrArrayClassUnion) [],
                            (ObjectOrArrayClassUnion($id: 2)) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              null,
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrArrayClassUnion) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3,
                              null
                            ],
                            (ObjectOrArrayClassUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: (MyOtherTypeClass) {
                            $ref: 1
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        null,
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        "new Object 2",
                        "new Object 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                null,
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              "new Object 2",
                              "new Object 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectArrayPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectSpanClassUnionRevisit> PrefieldObjectSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectSpanClassUnionRevisit>(new PreFieldObjectSpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectSpanClassUnionRevisit {
                         firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                         firstSpan: (Span<ObjectOrSpanClassUnion>) [
                         (ObjectOrSpanClassUnion) (MyOtherTypeClass) { $ref: 1 },
                         (ObjectOrSpanClassUnion) [],
                         (ObjectOrSpanClassUnion($id: 2)) [
                         null,
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrSpanClassUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrSpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectSpanClassUnionRevisit {
                          firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                          firstSpan: (Span<ObjectOrSpanClassUnion>) [
                            (ObjectOrSpanClassUnion) (MyOtherTypeClass) {
                              $ref: 1
                            },
                            (ObjectOrSpanClassUnion) [],
                            (ObjectOrSpanClassUnion($id: 2)) [
                              null,
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrSpanClassUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrSpanClassUnion) {
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
                        "$values":"singleton Object 2"
                        },
                        "firstSpan":[
                        {
                        "$ref":"1"
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new Object 1",
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                            "$values": "singleton Object 2"
                          },
                          "firstSpan": [
                            {
                              "$ref": "1"
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new Object 1",
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectSpanNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectSpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectSpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectSpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectSpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectSpanPostFieldClassUnionRevisit> ObjectSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectSpanPostFieldClassUnionRevisit>(new ObjectSpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<ObjectOrSpanClassUnion>) [
                         (ObjectOrSpanClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrSpanClassUnion) null,
                         (ObjectOrSpanClassUnion($id: 2)) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3,
                         null
                         ],
                         (ObjectOrSpanClassUnion) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrSpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectSpanPostFieldClassUnionRevisit {
                          firstSpan: (Span<ObjectOrSpanClassUnion>) [
                            (ObjectOrSpanClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrSpanClassUnion) null,
                            (ObjectOrSpanClassUnion($id: 2)) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3,
                              null
                            ],
                            (ObjectOrSpanClassUnion) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrSpanClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3",
                        null
                        ]
                        },
                        [
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
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
                              "$values": singleton Object 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3",
                                null
                              ]
                            },
                            [
                              "new Object 1",
                              null,
                              "new Object 2",
                              "new Object 3"
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
    public void ObjectSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectSpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectSpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectSpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectSpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectReadOnlySpanClassUnionRevisit> PrefieldObjectReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectReadOnlySpanClassUnionRevisit>(new PreFieldObjectReadOnlySpanClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectReadOnlySpanClassUnionRevisit {
                         firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                         firstReadOnlySpan: (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>) [
                         (ObjectOrReadOnlySpanClassUnion) (MyOtherTypeClass) { $ref: 1 },
                         (ObjectOrReadOnlySpanClassUnion) null,
                         (ObjectOrReadOnlySpanClassUnion($id: 2)) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         null,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrReadOnlySpanClassUnion) [
                         null,
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrReadOnlySpanClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectReadOnlySpanClassUnionRevisit {
                          firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                          firstReadOnlySpan: (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>) [
                            (ObjectOrReadOnlySpanClassUnion) (MyOtherTypeClass) {
                              $ref: 1
                            },
                            (ObjectOrReadOnlySpanClassUnion) null,
                            (ObjectOrReadOnlySpanClassUnion($id: 2)) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              null,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrReadOnlySpanClassUnion) [
                              null,
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrReadOnlySpanClassUnion) {
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
                        "$values":"singleton Object 2"
                        },
                        "firstReadOnlySpan":[
                        {
                        "$ref":"1"
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        null,
                        "new Object 2",
                        null,
                        "new Object 3"
                        ]
                        },
                        [
                        null,
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                            "$values": "singleton Object 2"
                          },
                          "firstReadOnlySpan": [
                            {
                              "$ref": "1"
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                null,
                                "new Object 2",
                                null,
                                "new Object 3"
                              ]
                            },
                            [
                              null,
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectReadOnlySpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectReadOnlySpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectReadOnlySpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectReadOnlySpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectReadOnlySpanPostFieldClassUnionRevisit> ObjectReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectReadOnlySpanPostFieldClassUnionRevisit>(new ObjectReadOnlySpanPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>) [
                         (ObjectOrReadOnlySpanClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrReadOnlySpanClassUnion) [],
                         (ObjectOrReadOnlySpanClassUnion($id: 2)) [
                         null,
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrReadOnlySpanClassUnion) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrReadOnlySpanClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectReadOnlySpanPostFieldClassUnionRevisit {
                          firstReadOnlySpan: (ReadOnlySpan<ObjectOrReadOnlySpanClassUnion>) [
                            (ObjectOrReadOnlySpanClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrReadOnlySpanClassUnion) [],
                            (ObjectOrReadOnlySpanClassUnion($id: 2)) [
                              null,
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrReadOnlySpanClassUnion) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrReadOnlySpanClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              null,
                              "new Object 2",
                              "new Object 3"
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
    public void ObjectReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectListStructUnionRevisit> PrefieldObjectListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldObjectListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldObjectListStructUnionRevisit>(new PreFieldObjectListStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectListStructUnionRevisit {
                         firstPreField: null,
                         firstList: (List<ObjectOrListStructUnion>) [
                         (ObjectOrListStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrListStructUnion) null,
                         (ObjectOrListStructUnion) { $id: 2, $values: [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3,
                         null
                         ]
                         },
                         (ObjectOrListStructUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrListStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectListStructUnionRevisit {
                          firstPreField: null,
                          firstList: (List<ObjectOrListStructUnion>) [
                            (ObjectOrListStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrListStructUnion) null,
                            (ObjectOrListStructUnion) {
                              $id: 2,
                              $values: [
                                (MyOtherTypeClass) new Object 1,
                                (MyOtherTypeClass) new Object 2,
                                (MyOtherTypeClass) new Object 3,
                                null
                              ]
                            },
                            (ObjectOrListStructUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrListStructUnion) {
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
                        "$values":singleton Object 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        "new Object 2",
                        "new Object 3",
                        null
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                              "$values": singleton Object 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                "new Object 2",
                                "new Object 3",
                                null
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectListNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectListPostFieldStructUnionRevisit> ObjectListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<ObjectListPostFieldStructUnionRevisit>(new ObjectListPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectListPostFieldStructUnionRevisit {
                         firstList: (List<ObjectOrListStructUnion>) [
                         (ObjectOrListStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrListStructUnion) [],
                         (ObjectOrListStructUnion) { $id: 2, $values: [
                         (MyOtherTypeClass) singleton Object 1,
                         null,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ]
                         },
                         (ObjectOrListStructUnion) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         null,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrListStructUnion) { $ref: 2 }
                         ],
                         firstPostField: (MyOtherTypeClass) { $ref: 1 }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectListPostFieldStructUnionRevisit {
                          firstList: (List<ObjectOrListStructUnion>) [
                            (ObjectOrListStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrListStructUnion) [],
                            (ObjectOrListStructUnion) {
                              $id: 2,
                              $values: [
                                (MyOtherTypeClass) singleton Object 1,
                                null,
                                (MyOtherTypeClass) {
                                  $ref: 1
                                },
                                (MyOtherTypeClass) singleton Object 3
                              ]
                            },
                            (ObjectOrListStructUnion) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              null,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrListStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: (MyOtherTypeClass) {
                            $ref: 1
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        null,
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        "new Object 2",
                        null,
                        "new Object 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                null,
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              "new Object 2",
                              null,
                              "new Object 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectListClassUnionRevisit> PrefieldObjectListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectListClassUnionRevisit>(new PreFieldObjectListClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectListClassUnionRevisit {
                         firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                         firstList: (List<ObjectOrListClassUnion>) [
                         (ObjectOrListClassUnion) (MyOtherTypeClass) { $ref: 1 },
                         (ObjectOrListClassUnion) null,
                         (ObjectOrListClassUnion($id: 2)) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         null,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrListClassUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         null,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrListClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectListClassUnionRevisit {
                          firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                          firstList: (List<ObjectOrListClassUnion>) [
                            (ObjectOrListClassUnion) (MyOtherTypeClass) {
                              $ref: 1
                            },
                            (ObjectOrListClassUnion) null,
                            (ObjectOrListClassUnion($id: 2)) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              null,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrListClassUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              null,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrListClassUnion) {
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
                        "$values":"singleton Object 2"
                        },
                        "firstList":[
                        {
                        "$ref":"1"
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        "new Object 2",
                        null,
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        null,
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                            "$values": "singleton Object 2"
                          },
                          "firstList": [
                            {
                              "$ref": "1"
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                "new Object 2",
                                null,
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              null,
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectListNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectListNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectListNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectListPostFieldClassUnionRevisit> ObjectListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectListPostFieldClassUnionRevisit>(new ObjectListPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectListPostFieldClassUnionRevisit {
                         firstList: (List<ObjectOrListClassUnion>) [
                         (ObjectOrListClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrListClassUnion) [],
                         (ObjectOrListClassUnion($id: 2)) [
                         null,
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrListClassUnion) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3,
                         null
                         ],
                         (ObjectOrListClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectListPostFieldClassUnionRevisit {
                          firstList: (List<ObjectOrListClassUnion>) [
                            (ObjectOrListClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrListClassUnion) [],
                            (ObjectOrListClassUnion($id: 2)) [
                              null,
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrListClassUnion) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3,
                              null
                            ],
                            (ObjectOrListClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        "new Object 2",
                        "new Object 3",
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              "new Object 2",
                              "new Object 3",
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
    public void ObjectListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectListPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectEnumerableStructUnionRevisit> PrefieldObjectEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldObjectEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldObjectEnumerableStructUnionRevisit>(new PreFieldObjectEnumerableStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectEnumerableStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<ObjectOrEnumerableStructUnion>) [
                         (ObjectOrEnumerableStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumerableStructUnion) [],
                         (ObjectOrEnumerableStructUnion) { $id: 2, $values: [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ]
                         },
                         (ObjectOrEnumerableStructUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         null,
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumerableStructUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectEnumerableStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerable: (List<ObjectOrEnumerableStructUnion>) [
                            (ObjectOrEnumerableStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumerableStructUnion) [],
                            (ObjectOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                (MyOtherTypeClass) new Object 1,
                                null,
                                (MyOtherTypeClass) new Object 2,
                                (MyOtherTypeClass) new Object 3
                              ]
                            },
                            (ObjectOrEnumerableStructUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              null,
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumerableStructUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        null,
                        "singleton Object 3"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                null,
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              null,
                              "singleton Object 3"
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
    public void PrefieldObjectEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectEnumerablePostFieldStructUnionRevisit> ObjectEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<ObjectEnumerablePostFieldStructUnionRevisit>(new ObjectEnumerablePostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<ObjectOrEnumerableStructUnion>) [
                         (ObjectOrEnumerableStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumerableStructUnion) [],
                         (ObjectOrEnumerableStructUnion) { $id: 2, $values: [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         null,
                         (MyOtherTypeClass) singleton Object 3
                         ]
                         },
                         (ObjectOrEnumerableStructUnion) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumerableStructUnion) { $ref: 2 }
                         ],
                         firstPostField: (MyOtherTypeClass) { $ref: 1 }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectEnumerablePostFieldStructUnionRevisit {
                          firstEnumerable: (List<ObjectOrEnumerableStructUnion>) [
                            (ObjectOrEnumerableStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumerableStructUnion) [],
                            (ObjectOrEnumerableStructUnion) {
                              $id: 2,
                              $values: [
                                (MyOtherTypeClass) singleton Object 1,
                                (MyOtherTypeClass) {
                                  $ref: 1
                                },
                                null,
                                (MyOtherTypeClass) singleton Object 3
                              ]
                            },
                            (ObjectOrEnumerableStructUnion) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumerableStructUnion) {
                              $ref: 2
                            }
                          ],
                          firstPostField: (MyOtherTypeClass) {
                            $ref: 1
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        null,
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                null,
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              null,
                              "new Object 2",
                              "new Object 3"
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectEnumerableClassUnionRevisit> PrefieldObjectEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectEnumerableClassUnionRevisit>(new PreFieldObjectEnumerableClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectEnumerableClassUnionRevisit {
                         firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                         firstEnumerable: (List<ObjectOrEnumerableClassUnion>) [
                         (ObjectOrEnumerableClassUnion) (MyOtherTypeClass) { $ref: 1 },
                         (ObjectOrEnumerableClassUnion) null,
                         (ObjectOrEnumerableClassUnion($id: 2)) [
                         null,
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumerableClassUnion) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3,
                         null
                         ],
                         (ObjectOrEnumerableClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectEnumerableClassUnionRevisit {
                          firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                          firstEnumerable: (List<ObjectOrEnumerableClassUnion>) [
                            (ObjectOrEnumerableClassUnion) (MyOtherTypeClass) {
                              $ref: 1
                            },
                            (ObjectOrEnumerableClassUnion) null,
                            (ObjectOrEnumerableClassUnion($id: 2)) [
                              null,
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumerableClassUnion) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3,
                              null
                            ],
                            (ObjectOrEnumerableClassUnion) {
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
                        "$values":"singleton Object 2"
                        },
                        "firstEnumerable":[
                        {
                        "$ref":"1"
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "new Object 1",
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3",
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
                            "$values": "singleton Object 2"
                          },
                          "firstEnumerable": [
                            {
                              "$ref": "1"
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "new Object 1",
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3",
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
    public void PrefieldObjectEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumerableNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectEnumerablePostFieldClassUnionRevisit> ObjectEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectEnumerablePostFieldClassUnionRevisit>(new ObjectEnumerablePostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<ObjectOrEnumerableClassUnion>) [
                         (ObjectOrEnumerableClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumerableClassUnion) [],
                         (ObjectOrEnumerableClassUnion($id: 2)) [
                         (MyOtherTypeClass) singleton Object 1,
                         null,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumerableClassUnion) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         null,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumerableClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectEnumerablePostFieldClassUnionRevisit {
                          firstEnumerable: (List<ObjectOrEnumerableClassUnion>) [
                            (ObjectOrEnumerableClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumerableClassUnion) [],
                            (ObjectOrEnumerableClassUnion($id: 2)) [
                              (MyOtherTypeClass) singleton Object 1,
                              null,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumerableClassUnion) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              null,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumerableClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        null,
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        "new Object 2",
                        null,
                        "new Object 3"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                null,
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              "new Object 2",
                              null,
                              "new Object 3"
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
    public void ObjectEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumerablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectEnumeratorStructUnionRevisit> PrefieldObjectEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldObjectEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldObjectEnumeratorStructUnionRevisit>(new PreFieldObjectEnumeratorStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectEnumeratorStructUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<ObjectOrEnumeratorStructUnion>.Enumerator) [
                         (ObjectOrEnumeratorStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumeratorStructUnion) null,
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>($id: 2)) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         null,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) singleton Object 1,
                         null,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectEnumeratorStructUnionRevisit {
                          firstPreField: null,
                          firstEnumerator: (List<ObjectOrEnumeratorStructUnion>.Enumerator) [
                            (ObjectOrEnumeratorStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumeratorStructUnion) null,
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>($id: 2)) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              null,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) singleton Object 1,
                              null,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) {
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
                        "$values":singleton Object 2
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        "new Object 2",
                        null,
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        null,
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
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
                              "$values": singleton Object 2
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                "new Object 2",
                                null,
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              null,
                              {
                                "$ref": "1"
                              },
                              "singleton Object 3"
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
    public void PrefieldObjectEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectEnumeratorPostFieldStructUnionRevisit> ObjectEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<ObjectEnumeratorPostFieldStructUnionRevisit>(new ObjectEnumeratorPostFieldStructUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<ObjectOrEnumeratorStructUnion>.Enumerator) [
                         (ObjectOrEnumeratorStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumeratorStructUnion) [],
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>($id: 2)) [
                         null,
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) new Object 1,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3,
                         null
                         ],
                         (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) { $ref: 2 }
                         ],
                         firstPostField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectEnumeratorPostFieldStructUnionRevisit {
                          firstEnumerator: (List<ObjectOrEnumeratorStructUnion>.Enumerator) [
                            (ObjectOrEnumeratorStructUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumeratorStructUnion) [],
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>($id: 2)) [
                              null,
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) new Object 1,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3,
                              null
                            ],
                            (ObjectOrEnumeratorStructUnion) (ReusableWrappingEnumerator<object>) {
                              $ref: 2
                            }
                          ],
                          firstPostField: (MyOtherTypeClass) {
                            $ref: 1
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        null,
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        "new Object 2",
                        "new Object 3",
                        null
                        ],
                        {
                        "$ref":"2"
                        }
                        ],
                        "firstPostField":{
                        "$ref":"1"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                null,
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              "new Object 2",
                              "new Object 3",
                              null
                            ],
                            {
                              "$ref": "2"
                            }
                          ],
                          "firstPostField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldObjectEnumeratorClassUnionRevisit> PrefieldObjectEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldObjectEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldObjectEnumeratorClassUnionRevisit>(new PreFieldObjectEnumeratorClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldObjectEnumeratorClassUnionRevisit {
                         firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                         firstEnumerator: (List<ObjectOrEnumeratorClassUnion>.Enumerator) [
                         (ObjectOrEnumeratorClassUnion) (MyOtherTypeClass) { $ref: 1 },
                         (ObjectOrEnumeratorClassUnion) null,
                         (ObjectOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumeratorClassUnion) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         null,
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumeratorClassUnion) { $ref: 2 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        PreFieldObjectEnumeratorClassUnionRevisit {
                          firstPreField: (MyOtherTypeClass($id: 1)) "singleton Object 2",
                          firstEnumerator: (List<ObjectOrEnumeratorClassUnion>.Enumerator) [
                            (ObjectOrEnumeratorClassUnion) (MyOtherTypeClass) {
                              $ref: 1
                            },
                            (ObjectOrEnumeratorClassUnion) null,
                            (ObjectOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumeratorClassUnion) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              null,
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumeratorClassUnion) {
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
                        "$values":"singleton Object 2"
                        },
                        "firstEnumerator":[
                        {
                        "$ref":"1"
                        },
                        null,
                        {
                        "$id":"2",
                        "$values":[
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
                        ]
                        },
                        [
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        null,
                        "singleton Object 3"
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
                            "$values": "singleton Object 2"
                          },
                          "firstEnumerator": [
                            {
                              "$ref": "1"
                            },
                            null,
                            {
                              "$id": "2",
                              "$values": [
                                "new Object 1",
                                null,
                                "new Object 2",
                                "new Object 3"
                              ]
                            },
                            [
                              "singleton Object 1",
                              {
                                "$ref": "1"
                              },
                              null,
                              "singleton Object 3"
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
    public void PrefieldObjectEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldObjectEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(PrefieldObjectEnumeratorNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<ObjectEnumeratorPostFieldClassUnionRevisit> ObjectEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<ObjectEnumeratorPostFieldClassUnionRevisit>(new ObjectEnumeratorPostFieldClassUnionRevisit())
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        ObjectEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<ObjectOrEnumeratorClassUnion>.Enumerator) [
                         (ObjectOrEnumeratorClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                         (ObjectOrEnumeratorClassUnion) [],
                         (ObjectOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) singleton Object 1,
                         (MyOtherTypeClass) { $ref: 1 },
                         null,
                         (MyOtherTypeClass) singleton Object 3
                         ],
                         (ObjectOrEnumeratorClassUnion) (ReusableWrappingEnumerator<object>) [
                         (MyOtherTypeClass) new Object 1,
                         null,
                         (MyOtherTypeClass) new Object 2,
                         (MyOtherTypeClass) new Object 3
                         ],
                         (ObjectOrEnumeratorClassUnion) { $ref: 2 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        ObjectEnumeratorPostFieldClassUnionRevisit {
                          firstEnumerator: (List<ObjectOrEnumeratorClassUnion>.Enumerator) [
                            (ObjectOrEnumeratorClassUnion) (MyOtherTypeClass($id: 1)) singleton Object 2,
                            (ObjectOrEnumeratorClassUnion) [],
                            (ObjectOrEnumeratorClassUnion($id: 2)) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) singleton Object 1,
                              (MyOtherTypeClass) {
                                $ref: 1
                              },
                              null,
                              (MyOtherTypeClass) singleton Object 3
                            ],
                            (ObjectOrEnumeratorClassUnion) (ReusableWrappingEnumerator<object>) [
                              (MyOtherTypeClass) new Object 1,
                              null,
                              (MyOtherTypeClass) new Object 2,
                              (MyOtherTypeClass) new Object 3
                            ],
                            (ObjectOrEnumeratorClassUnion) {
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
                        "$values":singleton Object 2
                        },
                        [],
                        {
                        "$id":"2",
                        "$values":[
                        "singleton Object 1",
                        {
                        "$ref":"1"
                        },
                        null,
                        "singleton Object 3"
                        ]
                        },
                        [
                        "new Object 1",
                        null,
                        "new Object 2",
                        "new Object 3"
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
                              "$values": singleton Object 2
                            },
                            [],
                            {
                              "$id": "2",
                              "$values": [
                                "singleton Object 1",
                                {
                                  "$ref": "1"
                                },
                                null,
                                "singleton Object 3"
                              ]
                            },
                            [
                              "new Object 1",
                              null,
                              "new Object 2",
                              "new Object 3"
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
    public void ObjectEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void ObjectEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
      ExecuteIndividualScaffoldExpectation(ObjectEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
}
