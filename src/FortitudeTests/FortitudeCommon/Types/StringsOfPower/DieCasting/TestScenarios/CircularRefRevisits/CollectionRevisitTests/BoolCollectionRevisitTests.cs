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
public class BoolCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldBoolArrayStructUnionRevisit>?                  prefieldBoolArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<BoolArrayPostFieldStructUnionRevisit>?                 boolArrayPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolArrayStructUnionRevisit>?  nullablePrefieldNullableBoolArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableBoolArrayNullablePostFieldStructUnionRevisit>? nullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolArrayClassUnionRevisit>?                  prefieldBoolArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolArrayPostFieldClassUnionRevisit>?                 boolArrayPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolArrayClassUnionRevisit>?  nullablePrefieldNullableBoolArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolArrayNullablePostFieldClassUnionRevisit>? nullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolSpanClassUnionRevisit>?                  prefieldBoolSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolSpanPostFieldClassUnionRevisit>?                 boolSpanPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolSpanClassUnionRevisit>?  nullablePrefieldNullableBoolSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolSpanNullablePostFieldClassUnionRevisit>? nullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolReadOnlySpanClassUnionRevisit>?                  prefieldBoolReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolReadOnlySpanPostFieldClassUnionRevisit>?                 boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit>?  nullablePrefieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit>? nullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolListStructUnionRevisit>?                  prefieldBoolListNoRevealersStructUnionExpect;
    private static InputBearerExpect<BoolListPostFieldStructUnionRevisit>?                 boolListPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolListStructUnionRevisit>?  nullablePrefieldNullableBoolListNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableBoolListNullablePostFieldStructUnionRevisit>? nullableBoolListNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolListClassUnionRevisit>?                  prefieldBoolListNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolListPostFieldClassUnionRevisit>?                 boolListPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolListClassUnionRevisit>?  nullablePrefieldNullableBoolListNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolListNullablePostFieldClassUnionRevisit>? nullableBoolListNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolEnumerableStructUnionRevisit>?                  prefieldBoolEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<BoolEnumerablePostFieldStructUnionRevisit>?                 boolEnumerablePostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolEnumerableStructUnionRevisit>?  nullablePrefieldNullableBoolEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableBoolEnumerableNullablePostFieldStructUnionRevisit>? nullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolEnumerableClassUnionRevisit>?                  prefieldBoolEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolEnumerablePostFieldClassUnionRevisit>?                 boolEnumerablePostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolEnumerableClassUnionRevisit>?  nullablePrefieldNullableBoolEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolEnumerableNullablePostFieldClassUnionRevisit>? nullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolEnumeratorStructUnionRevisit>?                  prefieldBoolEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<BoolEnumeratorPostFieldStructUnionRevisit>?                 boolEnumeratorPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolEnumeratorStructUnionRevisit>?  nullablePrefieldNullableBoolEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableBoolEnumeratorNullablePostFieldStructUnionRevisit>? nullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldBoolEnumeratorClassUnionRevisit>?                  prefieldBoolEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<BoolEnumeratorPostFieldClassUnionRevisit>?                 boolEnumeratorPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolEnumeratorClassUnionRevisit>?  nullablePrefieldNullableBoolEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableBoolEnumeratorNullablePostFieldClassUnionRevisit>? nullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldBoolArrayStructUnionRevisit> PrefieldBoolArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldBoolArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldBoolArrayStructUnionRevisit>(new PreFieldBoolArrayStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolArrayStructUnionRevisit {
                         firstPreField: true,
                         firstArray: (BoolOrArrayStructUnion[]) [
                         (BoolOrArrayStructUnion) false,
                         (BoolOrArrayStructUnion) [],
                         (BoolOrArrayStructUnion) { $id: 1, $values: [ true, false, true ] },
                         (BoolOrArrayStructUnion) [ false, true, false ],
                         (BoolOrArrayStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolArrayStructUnionRevisit {
                           firstPreField: true,
                           firstArray: (BoolOrArrayStructUnion[]) [
                             (BoolOrArrayStructUnion) false,
                             (BoolOrArrayStructUnion) [],
                             (BoolOrArrayStructUnion) {
                               $id: 1,
                               $values: [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (BoolOrArrayStructUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrArrayStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":true,
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": true,
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolArrayPostFieldStructUnionRevisit> BoolArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<BoolArrayPostFieldStructUnionRevisit>(new BoolArrayPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolArrayPostFieldStructUnionRevisit {
                         firstArray: (BoolOrArrayStructUnion[]) [
                         (BoolOrArrayStructUnion) false,
                         (BoolOrArrayStructUnion) [],
                         (BoolOrArrayStructUnion) { $id: 1, $values: [ false, true, false ] },
                         (BoolOrArrayStructUnion) [ true, false, true ],
                         (BoolOrArrayStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolArrayPostFieldStructUnionRevisit {
                           firstArray: (BoolOrArrayStructUnion[]) [
                             (BoolOrArrayStructUnion) false,
                             (BoolOrArrayStructUnion) [],
                             (BoolOrArrayStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             (BoolOrArrayStructUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrArrayStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolArrayStructUnionRevisit> NullablePreFieldNullableBoolArrayNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolArrayStructUnionRevisit>(new NullablePreFieldNullableBoolArrayStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolArrayStructUnionRevisit {
                         firstPreField: false,
                         firstArray: (NullableStructBoolOrArrayStructUnion[]) [
                         (NullableStructBoolOrArrayStructUnion) false,
                         (NullableStructBoolOrArrayStructUnion) null,
                         (NullableStructBoolOrArrayStructUnion) { $id: 1, $values: [ null, true, false, true ] },
                         (NullableStructBoolOrArrayStructUnion) [ false, null, true, false ],
                         (NullableStructBoolOrArrayStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolArrayStructUnionRevisit {
                           firstPreField: false,
                           firstArray: (NullableStructBoolOrArrayStructUnion[]) [
                             (NullableStructBoolOrArrayStructUnion) false,
                             (NullableStructBoolOrArrayStructUnion) null,
                             (NullableStructBoolOrArrayStructUnion) {
                               $id: 1,
                               $values: [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (NullableStructBoolOrArrayStructUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrArrayStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstArray":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstArray": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolArrayNullablePostFieldStructUnionRevisit> NullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableBoolArrayNullablePostFieldStructUnionRevisit>(new NullableBoolArrayNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolArrayNullablePostFieldStructUnionRevisit {
                         firstArray: (NullableStructBoolOrArrayStructUnion[]) [
                         (NullableStructBoolOrArrayStructUnion) false,
                         (NullableStructBoolOrArrayStructUnion) [],
                         (NullableStructBoolOrArrayStructUnion) { $id: 1, $values: [ false, true, null, false ] },
                         (NullableStructBoolOrArrayStructUnion) [ true, false, true, null ],
                         (NullableStructBoolOrArrayStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolArrayNullablePostFieldStructUnionRevisit {
                           firstArray: (NullableStructBoolOrArrayStructUnion[]) [
                             (NullableStructBoolOrArrayStructUnion) false,
                             (NullableStructBoolOrArrayStructUnion) [],
                             (NullableStructBoolOrArrayStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             (NullableStructBoolOrArrayStructUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrArrayStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolArrayClassUnionRevisit> PrefieldBoolArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolArrayClassUnionRevisit>(new PreFieldBoolArrayClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolArrayClassUnionRevisit {
                         firstPreField: false,
                         firstArray: (BoolOrArrayClassUnion[]) [
                         (BoolOrArrayClassUnion) false,
                         (BoolOrArrayClassUnion) [],
                         (BoolOrArrayClassUnion($id: 1)) [ true, false, true ],
                         (BoolOrArrayClassUnion) [ false, true, false ],
                         (BoolOrArrayClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolArrayClassUnionRevisit {
                           firstPreField: false,
                           firstArray: (BoolOrArrayClassUnion[]) [
                             (BoolOrArrayClassUnion) false,
                             (BoolOrArrayClassUnion) [],
                             (BoolOrArrayClassUnion($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrArrayClassUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrArrayClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolArrayPostFieldClassUnionRevisit> BoolArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolArrayPostFieldClassUnionRevisit>(new BoolArrayPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolArrayPostFieldClassUnionRevisit {
                         firstArray: (BoolOrArrayClassUnion[]) [
                         (BoolOrArrayClassUnion) false,
                         (BoolOrArrayClassUnion) [],
                         (BoolOrArrayClassUnion($id: 1)) [ false, true, false ],
                         (BoolOrArrayClassUnion) [ true, false, true ],
                         (BoolOrArrayClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolArrayPostFieldClassUnionRevisit {
                           firstArray: (BoolOrArrayClassUnion[]) [
                             (BoolOrArrayClassUnion) false,
                             (BoolOrArrayClassUnion) [],
                             (BoolOrArrayClassUnion($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrArrayClassUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrArrayClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolArrayClassUnionRevisit> NullablePreFieldNullableBoolArrayNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolArrayClassUnionRevisit>(new NullablePreFieldNullableBoolArrayClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (NullableStructBoolOrArrayClassUnion[]) [
                         (NullableStructBoolOrArrayClassUnion) false,
                         (NullableStructBoolOrArrayClassUnion) null,
                         (NullableStructBoolOrArrayClassUnion($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrArrayClassUnion) [ false, null, true, false ],
                         (NullableStructBoolOrArrayClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolArrayClassUnionRevisit {
                           firstPreField: null,
                           firstArray: (NullableStructBoolOrArrayClassUnion[]) [
                             (NullableStructBoolOrArrayClassUnion) false,
                             (NullableStructBoolOrArrayClassUnion) null,
                             (NullableStructBoolOrArrayClassUnion($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrArrayClassUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrArrayClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstArray":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstArray": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolArrayNullablePostFieldClassUnionRevisit> NullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolArrayNullablePostFieldClassUnionRevisit>(new NullableBoolArrayNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolArrayNullablePostFieldClassUnionRevisit {
                         firstArray: (NullableStructBoolOrArrayClassUnion[]) [
                         (NullableStructBoolOrArrayClassUnion) false,
                         (NullableStructBoolOrArrayClassUnion) [],
                         (NullableStructBoolOrArrayClassUnion($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrArrayClassUnion) [ true, false, true, null ],
                         (NullableStructBoolOrArrayClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolArrayNullablePostFieldClassUnionRevisit {
                           firstArray: (NullableStructBoolOrArrayClassUnion[]) [
                             (NullableStructBoolOrArrayClassUnion) false,
                             (NullableStructBoolOrArrayClassUnion) [],
                             (NullableStructBoolOrArrayClassUnion($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrArrayClassUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrArrayClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolArrayNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolSpanClassUnionRevisit> PrefieldBoolSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolSpanClassUnionRevisit>(new PreFieldBoolSpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolSpanClassUnionRevisit {
                         firstPreField: false,
                         firstSpan: (Span<BoolOrSpanClassUnion>) [
                         (BoolOrSpanClassUnion) false,
                         (BoolOrSpanClassUnion) [],
                         (BoolOrSpanClassUnion($id: 1)) [ true, false, true ],
                         (BoolOrSpanClassUnion) [ false, true, false ],
                         (BoolOrSpanClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolSpanClassUnionRevisit {
                           firstPreField: false,
                           firstSpan: (Span<BoolOrSpanClassUnion>) [
                             (BoolOrSpanClassUnion) false,
                             (BoolOrSpanClassUnion) [],
                             (BoolOrSpanClassUnion($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrSpanClassUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrSpanClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstSpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstSpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolSpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolSpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolSpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolSpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolSpanPostFieldClassUnionRevisit> BoolSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolSpanPostFieldClassUnionRevisit>(new BoolSpanPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<BoolOrSpanClassUnion>) [
                         (BoolOrSpanClassUnion) false,
                         (BoolOrSpanClassUnion) [],
                         (BoolOrSpanClassUnion($id: 1)) [ false, true, false ],
                         (BoolOrSpanClassUnion) [ true, false, true ],
                         (BoolOrSpanClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolSpanPostFieldClassUnionRevisit {
                           firstSpan: (Span<BoolOrSpanClassUnion>) [
                             (BoolOrSpanClassUnion) false,
                             (BoolOrSpanClassUnion) [],
                             (BoolOrSpanClassUnion($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrSpanClassUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrSpanClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstSpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstSpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolSpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolSpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolSpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolSpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolSpanClassUnionRevisit> NullablePreFieldNullableBoolSpanNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolSpanClassUnionRevisit>(new NullablePreFieldNullableBoolSpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolSpanClassUnionRevisit {
                         firstPreField: null,
                         firstSpan: (Span<NullableStructBoolOrSpanClassUnion>) [
                         (NullableStructBoolOrSpanClassUnion) false,
                         (NullableStructBoolOrSpanClassUnion) null,
                         (NullableStructBoolOrSpanClassUnion($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrSpanClassUnion) [ false, null, true, false ],
                         (NullableStructBoolOrSpanClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolSpanClassUnionRevisit {
                           firstPreField: null,
                           firstSpan: (Span<NullableStructBoolOrSpanClassUnion>) [
                             (NullableStructBoolOrSpanClassUnion) false,
                             (NullableStructBoolOrSpanClassUnion) null,
                             (NullableStructBoolOrSpanClassUnion($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrSpanClassUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrSpanClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstSpan":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstSpan": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolSpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolSpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolSpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolSpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolSpanNullablePostFieldClassUnionRevisit> NullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolSpanNullablePostFieldClassUnionRevisit>(new NullableBoolSpanNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolSpanNullablePostFieldClassUnionRevisit {
                         firstSpan: (Span<NullableStructBoolOrSpanClassUnion>) [
                         (NullableStructBoolOrSpanClassUnion) false,
                         (NullableStructBoolOrSpanClassUnion) [],
                         (NullableStructBoolOrSpanClassUnion($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrSpanClassUnion) [ true, false, true, null ],
                         (NullableStructBoolOrSpanClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolSpanNullablePostFieldClassUnionRevisit {
                           firstSpan: (Span<NullableStructBoolOrSpanClassUnion>) [
                             (NullableStructBoolOrSpanClassUnion) false,
                             (NullableStructBoolOrSpanClassUnion) [],
                             (NullableStructBoolOrSpanClassUnion($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrSpanClassUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrSpanClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstSpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstSpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolSpanNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolSpanNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolSpanNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolSpanNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolSpanNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolReadOnlySpanClassUnionRevisit> PrefieldBoolReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolReadOnlySpanClassUnionRevisit>(new PreFieldBoolReadOnlySpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolReadOnlySpanClassUnionRevisit {
                         firstPreField: false,
                         firstReadOnlySpan: (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>) [
                         (BoolOrReadOnlySpanClassUnion) false,
                         (BoolOrReadOnlySpanClassUnion) [],
                         (BoolOrReadOnlySpanClassUnion($id: 1)) [ true, false, true ],
                         (BoolOrReadOnlySpanClassUnion) [ false, true, false ],
                         (BoolOrReadOnlySpanClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolReadOnlySpanClassUnionRevisit {
                           firstPreField: false,
                           firstReadOnlySpan: (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>) [
                             (BoolOrReadOnlySpanClassUnion) false,
                             (BoolOrReadOnlySpanClassUnion) [],
                             (BoolOrReadOnlySpanClassUnion($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrReadOnlySpanClassUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrReadOnlySpanClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstReadOnlySpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstReadOnlySpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolReadOnlySpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolReadOnlySpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolReadOnlySpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolReadOnlySpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolReadOnlySpanPostFieldClassUnionRevisit> BoolReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolReadOnlySpanPostFieldClassUnionRevisit>(new BoolReadOnlySpanPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>) [
                         (BoolOrReadOnlySpanClassUnion) false,
                         (BoolOrReadOnlySpanClassUnion) [],
                         (BoolOrReadOnlySpanClassUnion($id: 1)) [ false, true, false ],
                         (BoolOrReadOnlySpanClassUnion) [ true, false, true ],
                         (BoolOrReadOnlySpanClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolReadOnlySpanPostFieldClassUnionRevisit {
                           firstReadOnlySpan: (ReadOnlySpan<BoolOrReadOnlySpanClassUnion>) [
                             (BoolOrReadOnlySpanClassUnion) false,
                             (BoolOrReadOnlySpanClassUnion) [],
                             (BoolOrReadOnlySpanClassUnion($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrReadOnlySpanClassUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrReadOnlySpanClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstReadOnlySpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstReadOnlySpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit> NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit>(new NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit {
                         firstPreField: null,
                         firstReadOnlySpan: (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>) [
                         (NullableStructBoolOrReadOnlySpanClassUnion) false,
                         (NullableStructBoolOrReadOnlySpanClassUnion) null,
                         (NullableStructBoolOrReadOnlySpanClassUnion($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrReadOnlySpanClassUnion) [ false, null, true, false ],
                         (NullableStructBoolOrReadOnlySpanClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolReadOnlySpanClassUnionRevisit {
                           firstPreField: null,
                           firstReadOnlySpan: (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>) [
                             (NullableStructBoolOrReadOnlySpanClassUnion) false,
                             (NullableStructBoolOrReadOnlySpanClassUnion) null,
                             (NullableStructBoolOrReadOnlySpanClassUnion($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrReadOnlySpanClassUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrReadOnlySpanClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstReadOnlySpan":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstReadOnlySpan": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolReadOnlySpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit> NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit>(new NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>) [
                         (NullableStructBoolOrReadOnlySpanClassUnion) false,
                         (NullableStructBoolOrReadOnlySpanClassUnion) [],
                         (NullableStructBoolOrReadOnlySpanClassUnion($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrReadOnlySpanClassUnion) [ true, false, true, null ],
                         (NullableStructBoolOrReadOnlySpanClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolReadOnlySpanNullablePostFieldClassUnionRevisit {
                           firstReadOnlySpan: (ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion>) [
                             (NullableStructBoolOrReadOnlySpanClassUnion) false,
                             (NullableStructBoolOrReadOnlySpanClassUnion) [],
                             (NullableStructBoolOrReadOnlySpanClassUnion($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrReadOnlySpanClassUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrReadOnlySpanClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstReadOnlySpan":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstReadOnlySpan": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldBoolListStructUnionRevisit> PrefieldBoolListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldBoolListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldBoolListStructUnionRevisit>(new PreFieldBoolListStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolListStructUnionRevisit {
                         firstPreField: true,
                         firstList: (List<BoolOrListStructUnion>) [
                         (BoolOrListStructUnion) false,
                         (BoolOrListStructUnion) [],
                         (BoolOrListStructUnion) { $id: 1, $values: [ true, false, true ] },
                         (BoolOrListStructUnion) [ false, true, false ],
                         (BoolOrListStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolListStructUnionRevisit {
                           firstPreField: true,
                           firstList: (List<BoolOrListStructUnion>) [
                             (BoolOrListStructUnion) false,
                             (BoolOrListStructUnion) [],
                             (BoolOrListStructUnion) {
                               $id: 1,
                               $values: [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (BoolOrListStructUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrListStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":true,
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": true,
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolListPostFieldStructUnionRevisit> BoolListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<BoolListPostFieldStructUnionRevisit>(new BoolListPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolListPostFieldStructUnionRevisit {
                         firstList: (List<BoolOrListStructUnion>) [
                         (BoolOrListStructUnion) false,
                         (BoolOrListStructUnion) [],
                         (BoolOrListStructUnion) { $id: 1, $values: [ false, true, false ] },
                         (BoolOrListStructUnion) [ true, false, true ],
                         (BoolOrListStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolListPostFieldStructUnionRevisit {
                           firstList: (List<BoolOrListStructUnion>) [
                             (BoolOrListStructUnion) false,
                             (BoolOrListStructUnion) [],
                             (BoolOrListStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             (BoolOrListStructUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrListStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolListStructUnionRevisit> NullablePreFieldNullableBoolListNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolListStructUnionRevisit>(new NullablePreFieldNullableBoolListStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolListStructUnionRevisit {
                         firstPreField: false,
                         firstList: (List<NullableStructBoolOrListStructUnion>) [
                         (NullableStructBoolOrListStructUnion) false,
                         (NullableStructBoolOrListStructUnion) null,
                         (NullableStructBoolOrListStructUnion) { $id: 1, $values: [ null, true, false, true ] },
                         (NullableStructBoolOrListStructUnion) [ false, null, true, false ],
                         (NullableStructBoolOrListStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolListStructUnionRevisit {
                           firstPreField: false,
                           firstList: (List<NullableStructBoolOrListStructUnion>) [
                             (NullableStructBoolOrListStructUnion) false,
                             (NullableStructBoolOrListStructUnion) null,
                             (NullableStructBoolOrListStructUnion) {
                               $id: 1,
                               $values: [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (NullableStructBoolOrListStructUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrListStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstList":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstList": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolListNullablePostFieldStructUnionRevisit> NullableBoolListNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableBoolListNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableBoolListNullablePostFieldStructUnionRevisit>(new NullableBoolListNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolListNullablePostFieldStructUnionRevisit {
                         firstList: (List<NullableStructBoolOrListStructUnion>) [
                         (NullableStructBoolOrListStructUnion) false,
                         (NullableStructBoolOrListStructUnion) [],
                         (NullableStructBoolOrListStructUnion) { $id: 1, $values: [ false, true, null, false ] },
                         (NullableStructBoolOrListStructUnion) [ true, false, true, null ],
                         (NullableStructBoolOrListStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolListNullablePostFieldStructUnionRevisit {
                           firstList: (List<NullableStructBoolOrListStructUnion>) [
                             (NullableStructBoolOrListStructUnion) false,
                             (NullableStructBoolOrListStructUnion) [],
                             (NullableStructBoolOrListStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             (NullableStructBoolOrListStructUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrListStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolListClassUnionRevisit> PrefieldBoolListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolListClassUnionRevisit>(new PreFieldBoolListClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolListClassUnionRevisit {
                         firstPreField: false,
                         firstList: (List<BoolOrListClassUnion>) [
                         (BoolOrListClassUnion) false,
                         (BoolOrListClassUnion) [],
                         (BoolOrListClassUnion($id: 1)) [ true, false, true ],
                         (BoolOrListClassUnion) [ false, true, false ],
                         (BoolOrListClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolListClassUnionRevisit {
                           firstPreField: false,
                           firstList: (List<BoolOrListClassUnion>) [
                             (BoolOrListClassUnion) false,
                             (BoolOrListClassUnion) [],
                             (BoolOrListClassUnion($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrListClassUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrListClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolListNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolListPostFieldClassUnionRevisit> BoolListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolListPostFieldClassUnionRevisit>(new BoolListPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolListPostFieldClassUnionRevisit {
                         firstList: (List<BoolOrListClassUnion>) [
                         (BoolOrListClassUnion) false,
                         (BoolOrListClassUnion) [],
                         (BoolOrListClassUnion($id: 1)) [ false, true, false ],
                         (BoolOrListClassUnion) [ true, false, true ],
                         (BoolOrListClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolListPostFieldClassUnionRevisit {
                           firstList: (List<BoolOrListClassUnion>) [
                             (BoolOrListClassUnion) false,
                             (BoolOrListClassUnion) [],
                             (BoolOrListClassUnion($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrListClassUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrListClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolListPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolListClassUnionRevisit> NullablePreFieldNullableBoolListNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolListClassUnionRevisit>(new NullablePreFieldNullableBoolListClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolListClassUnionRevisit {
                         firstPreField: null,
                         firstList: (List<NullableStructBoolOrListClassUnion>) [
                         (NullableStructBoolOrListClassUnion) false,
                         (NullableStructBoolOrListClassUnion) null,
                         (NullableStructBoolOrListClassUnion($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrListClassUnion) [ false, null, true, false ],
                         (NullableStructBoolOrListClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolListClassUnionRevisit {
                           firstPreField: null,
                           firstList: (List<NullableStructBoolOrListClassUnion>) [
                             (NullableStructBoolOrListClassUnion) false,
                             (NullableStructBoolOrListClassUnion) null,
                             (NullableStructBoolOrListClassUnion($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrListClassUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrListClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstList":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstList": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolListNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolListNullablePostFieldClassUnionRevisit> NullableBoolListNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolListNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolListNullablePostFieldClassUnionRevisit>(new NullableBoolListNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolListNullablePostFieldClassUnionRevisit {
                         firstList: (List<NullableStructBoolOrListClassUnion>) [
                         (NullableStructBoolOrListClassUnion) false,
                         (NullableStructBoolOrListClassUnion) [],
                         (NullableStructBoolOrListClassUnion($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrListClassUnion) [ true, false, true, null ],
                         (NullableStructBoolOrListClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolListNullablePostFieldClassUnionRevisit {
                           firstList: (List<NullableStructBoolOrListClassUnion>) [
                             (NullableStructBoolOrListClassUnion) false,
                             (NullableStructBoolOrListClassUnion) [],
                             (NullableStructBoolOrListClassUnion($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrListClassUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrListClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolListNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolListNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolListNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolEnumerableStructUnionRevisit> PrefieldBoolEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldBoolEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldBoolEnumerableStructUnionRevisit>(new PreFieldBoolEnumerableStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolEnumerableStructUnionRevisit {
                         firstPreField: true,
                         firstEnumerable: (List<BoolOrEnumerableStructUnion>) [
                         (BoolOrEnumerableStructUnion) false,
                         (BoolOrEnumerableStructUnion) [],
                         (BoolOrEnumerableStructUnion) { $id: 1, $values: [ true, false, true ] },
                         (BoolOrEnumerableStructUnion) [ false, true, false ],
                         (BoolOrEnumerableStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolEnumerableStructUnionRevisit {
                           firstPreField: true,
                           firstEnumerable: (List<BoolOrEnumerableStructUnion>) [
                             (BoolOrEnumerableStructUnion) false,
                             (BoolOrEnumerableStructUnion) [],
                             (BoolOrEnumerableStructUnion) {
                               $id: 1,
                               $values: [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (BoolOrEnumerableStructUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumerableStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":true,
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": true,
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolEnumerablePostFieldStructUnionRevisit> BoolEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<BoolEnumerablePostFieldStructUnionRevisit>(new BoolEnumerablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<BoolOrEnumerableStructUnion>) [
                         (BoolOrEnumerableStructUnion) false,
                         (BoolOrEnumerableStructUnion) [],
                         (BoolOrEnumerableStructUnion) { $id: 1, $values: [ false, true, false ] },
                         (BoolOrEnumerableStructUnion) [ true, false, true ],
                         (BoolOrEnumerableStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolEnumerablePostFieldStructUnionRevisit {
                           firstEnumerable: (List<BoolOrEnumerableStructUnion>) [
                             (BoolOrEnumerableStructUnion) false,
                             (BoolOrEnumerableStructUnion) [],
                             (BoolOrEnumerableStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             (BoolOrEnumerableStructUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumerableStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolEnumerableStructUnionRevisit> NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolEnumerableStructUnionRevisit>(new NullablePreFieldNullableBoolEnumerableStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolEnumerableStructUnionRevisit {
                         firstPreField: false,
                         firstEnumerable: (List<NullableStructBoolOrEnumerableStructUnion>) [
                         (NullableStructBoolOrEnumerableStructUnion) false,
                         (NullableStructBoolOrEnumerableStructUnion) null,
                         (NullableStructBoolOrEnumerableStructUnion) { $id: 1, $values: [ null, true, false, true ] },
                         (NullableStructBoolOrEnumerableStructUnion) [ false, null, true, false ],
                         (NullableStructBoolOrEnumerableStructUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolEnumerableStructUnionRevisit {
                           firstPreField: false,
                           firstEnumerable: (List<NullableStructBoolOrEnumerableStructUnion>) [
                             (NullableStructBoolOrEnumerableStructUnion) false,
                             (NullableStructBoolOrEnumerableStructUnion) null,
                             (NullableStructBoolOrEnumerableStructUnion) {
                               $id: 1,
                               $values: [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             (NullableStructBoolOrEnumerableStructUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrEnumerableStructUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstEnumerable":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstEnumerable": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolEnumerableNullablePostFieldStructUnionRevisit> NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableBoolEnumerableNullablePostFieldStructUnionRevisit>(new NullableBoolEnumerableNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolEnumerableNullablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<NullableStructBoolOrEnumerableStructUnion>) [
                         (NullableStructBoolOrEnumerableStructUnion) false,
                         (NullableStructBoolOrEnumerableStructUnion) [],
                         (NullableStructBoolOrEnumerableStructUnion) { $id: 1, $values: [ false, true, null, false ] },
                         (NullableStructBoolOrEnumerableStructUnion) [ true, false, true, null ],
                         (NullableStructBoolOrEnumerableStructUnion) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolEnumerableNullablePostFieldStructUnionRevisit {
                           firstEnumerable: (List<NullableStructBoolOrEnumerableStructUnion>) [
                             (NullableStructBoolOrEnumerableStructUnion) false,
                             (NullableStructBoolOrEnumerableStructUnion) [],
                             (NullableStructBoolOrEnumerableStructUnion) {
                               $id: 1,
                               $values: [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             (NullableStructBoolOrEnumerableStructUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrEnumerableStructUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolEnumerableClassUnionRevisit> PrefieldBoolEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolEnumerableClassUnionRevisit>(new PreFieldBoolEnumerableClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolEnumerableClassUnionRevisit {
                         firstPreField: false,
                         firstEnumerable: (List<BoolOrEnumerableClassUnion>) [
                         (BoolOrEnumerableClassUnion) false,
                         (BoolOrEnumerableClassUnion) [],
                         (BoolOrEnumerableClassUnion($id: 1)) [ true, false, true ],
                         (BoolOrEnumerableClassUnion) [ false, true, false ],
                         (BoolOrEnumerableClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolEnumerableClassUnionRevisit {
                           firstPreField: false,
                           firstEnumerable: (List<BoolOrEnumerableClassUnion>) [
                             (BoolOrEnumerableClassUnion) false,
                             (BoolOrEnumerableClassUnion) [],
                             (BoolOrEnumerableClassUnion($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumerableClassUnion) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumerableClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumerableNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolEnumerablePostFieldClassUnionRevisit> BoolEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolEnumerablePostFieldClassUnionRevisit>(new BoolEnumerablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<BoolOrEnumerableClassUnion>) [
                         (BoolOrEnumerableClassUnion) false,
                         (BoolOrEnumerableClassUnion) [],
                         (BoolOrEnumerableClassUnion($id: 1)) [ false, true, false ],
                         (BoolOrEnumerableClassUnion) [ true, false, true ],
                         (BoolOrEnumerableClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolEnumerablePostFieldClassUnionRevisit {
                           firstEnumerable: (List<BoolOrEnumerableClassUnion>) [
                             (BoolOrEnumerableClassUnion) false,
                             (BoolOrEnumerableClassUnion) [],
                             (BoolOrEnumerableClassUnion($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumerableClassUnion) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumerableClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumerablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<NullablePreFieldNullableBoolEnumerableClassUnionRevisit> NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolEnumerableClassUnionRevisit>(new NullablePreFieldNullableBoolEnumerableClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolEnumerableClassUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<NullableStructBoolOrEnumerableClassUnion>) [
                         (NullableStructBoolOrEnumerableClassUnion) false,
                         (NullableStructBoolOrEnumerableClassUnion) null,
                         (NullableStructBoolOrEnumerableClassUnion($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrEnumerableClassUnion) [ false, null, true, false ],
                         (NullableStructBoolOrEnumerableClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolEnumerableClassUnionRevisit {
                           firstPreField: null,
                           firstEnumerable: (List<NullableStructBoolOrEnumerableClassUnion>) [
                             (NullableStructBoolOrEnumerableClassUnion) false,
                             (NullableStructBoolOrEnumerableClassUnion) null,
                             (NullableStructBoolOrEnumerableClassUnion($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrEnumerableClassUnion) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrEnumerableClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstEnumerable":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstEnumerable": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumerableNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolEnumerableNullablePostFieldClassUnionRevisit> NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolEnumerableNullablePostFieldClassUnionRevisit>(new NullableBoolEnumerableNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolEnumerableNullablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<NullableStructBoolOrEnumerableClassUnion>) [
                         (NullableStructBoolOrEnumerableClassUnion) false,
                         (NullableStructBoolOrEnumerableClassUnion) [],
                         (NullableStructBoolOrEnumerableClassUnion($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrEnumerableClassUnion) [ true, false, true, null ],
                         (NullableStructBoolOrEnumerableClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolEnumerableNullablePostFieldClassUnionRevisit {
                           firstEnumerable: (List<NullableStructBoolOrEnumerableClassUnion>) [
                             (NullableStructBoolOrEnumerableClassUnion) false,
                             (NullableStructBoolOrEnumerableClassUnion) [],
                             (NullableStructBoolOrEnumerableClassUnion($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrEnumerableClassUnion) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrEnumerableClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumerableNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
  
    public static InputBearerExpect<PreFieldBoolEnumeratorStructUnionRevisit> PrefieldBoolEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldBoolEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldBoolEnumeratorStructUnionRevisit>(new PreFieldBoolEnumeratorStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolEnumeratorStructUnionRevisit {
                         firstPreField: true,
                         firstEnumerator: (List<BoolOrEnumeratorStructUnion>.Enumerator) [
                         (BoolOrEnumeratorStructUnion) false,
                         (BoolOrEnumeratorStructUnion) [],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>($id: 1)) [ true, false, true ],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) [ false, true, false ],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolEnumeratorStructUnionRevisit {
                           firstPreField: true,
                           firstEnumerator: (List<BoolOrEnumeratorStructUnion>.Enumerator) [
                             (BoolOrEnumeratorStructUnion) false,
                             (BoolOrEnumeratorStructUnion) [],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>($id: 1)) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":true,
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": true,
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolEnumeratorPostFieldStructUnionRevisit> BoolEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<BoolEnumeratorPostFieldStructUnionRevisit>(new BoolEnumeratorPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<BoolOrEnumeratorStructUnion>.Enumerator) [
                         (BoolOrEnumeratorStructUnion) false,
                         (BoolOrEnumeratorStructUnion) [],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>($id: 1)) [ false, true, false ],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) [ true, false, true ],
                         (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolEnumeratorPostFieldStructUnionRevisit {
                           firstEnumerator: (List<BoolOrEnumeratorStructUnion>.Enumerator) [
                             (BoolOrEnumeratorStructUnion) false,
                             (BoolOrEnumeratorStructUnion) [],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>($id: 1)) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolEnumeratorStructUnionRevisit> NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolEnumeratorStructUnionRevisit>(new NullablePreFieldNullableBoolEnumeratorStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolEnumeratorStructUnionRevisit {
                         firstPreField: false,
                         firstEnumerator: (List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator) [
                         (NullableStructBoolOrEnumeratorStructUnion) false,
                         (NullableStructBoolOrEnumeratorStructUnion) null,
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>($id: 1)) [ null, true, false, true ],
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) [ false, null, true, false ],
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolEnumeratorStructUnionRevisit {
                           firstPreField: false,
                           firstEnumerator: (List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator) [
                             (NullableStructBoolOrEnumeratorStructUnion) false,
                             (NullableStructBoolOrEnumeratorStructUnion) null,
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>($id: 1)) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstEnumerator":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstEnumerator": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolEnumeratorNullablePostFieldStructUnionRevisit> NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableBoolEnumeratorNullablePostFieldStructUnionRevisit>(new NullableBoolEnumeratorNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolEnumeratorNullablePostFieldStructUnionRevisit {
                         firstEnumerator: (List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator) [
                         (NullableStructBoolOrEnumeratorStructUnion) false,
                         (NullableStructBoolOrEnumeratorStructUnion) [],
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>($id: 1)) [ false, true, null, false ],
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) [ true, false, true, null ],
                         (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) { $ref: 1 }
                         ],
                         firstPostField: true
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolEnumeratorNullablePostFieldStructUnionRevisit {
                           firstEnumerator: (List<NullableStructBoolOrEnumeratorStructUnion>.Enumerator) [
                             (NullableStructBoolOrEnumeratorStructUnion) false,
                             (NullableStructBoolOrEnumeratorStructUnion) [],
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>($id: 1)) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrEnumeratorStructUnion) (ReusableWrappingEnumerator<bool?>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: true
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":true
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": true
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldBoolEnumeratorClassUnionRevisit> PrefieldBoolEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldBoolEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldBoolEnumeratorClassUnionRevisit>(new PreFieldBoolEnumeratorClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolEnumeratorClassUnionRevisit {
                         firstPreField: false,
                         firstEnumerator: (List<BoolOrEnumeratorClassUnion>.Enumerator) [
                         (BoolOrEnumeratorClassUnion) false,
                         (BoolOrEnumeratorClassUnion) [],
                         (BoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool>) [ true, false, true ],
                         (BoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool>) [ false, true, false ],
                         (BoolOrEnumeratorClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldBoolEnumeratorClassUnionRevisit {
                           firstPreField: false,
                           firstEnumerator: (List<BoolOrEnumeratorClassUnion>.Enumerator) [
                             (BoolOrEnumeratorClassUnion) false,
                             (BoolOrEnumeratorClassUnion) [],
                             (BoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool>) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool>) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumeratorClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":false,
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": false,
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolEnumeratorNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolEnumeratorPostFieldClassUnionRevisit> BoolEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<BoolEnumeratorPostFieldClassUnionRevisit>(new BoolEnumeratorPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<BoolOrEnumeratorClassUnion>.Enumerator) [
                         (BoolOrEnumeratorClassUnion) false,
                         (BoolOrEnumeratorClassUnion) [],
                         (BoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool>) [ false, true, false ],
                         (BoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool>) [ true, false, true ],
                         (BoolOrEnumeratorClassUnion) { $ref: 1 }
                         ],
                         firstPostField: false
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         BoolEnumeratorPostFieldClassUnionRevisit {
                           firstEnumerator: (List<BoolOrEnumeratorClassUnion>.Enumerator) [
                             (BoolOrEnumeratorClassUnion) false,
                             (BoolOrEnumeratorClassUnion) [],
                             (BoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool>) [
                               false,
                               true,
                               false
                             ],
                             (BoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool>) [
                               true,
                               false,
                               true
                             ],
                             (BoolOrEnumeratorClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: false
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":false
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": false
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<NullablePreFieldNullableBoolEnumeratorClassUnionRevisit> NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableBoolEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolEnumeratorClassUnionRevisit>(new NullablePreFieldNullableBoolEnumeratorClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolEnumeratorClassUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator) [
                         (NullableStructBoolOrEnumeratorClassUnion) false,
                         (NullableStructBoolOrEnumeratorClassUnion) null,
                         (NullableStructBoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool?>) [ null, true, false, true ],
                         (NullableStructBoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool?>) [ false, null, true, false ],
                         (NullableStructBoolOrEnumeratorClassUnion) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableBoolEnumeratorClassUnionRevisit {
                           firstPreField: null,
                           firstEnumerator: (List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator) [
                             (NullableStructBoolOrEnumeratorClassUnion) false,
                             (NullableStructBoolOrEnumeratorClassUnion) null,
                             (NullableStructBoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool?>) [
                               null,
                               true,
                               false,
                               true
                             ],
                             (NullableStructBoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool?>) [
                               false,
                               null,
                               true,
                               false
                             ],
                             (NullableStructBoolOrEnumeratorClassUnion) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":null,
                        "firstEnumerator":[
                        false,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        true,
                        false,
                        true
                        ]
                        },
                        [
                        false,
                        null,
                        true,
                        false
                        ],
                        {
                        "$ref":"1"
                        }
                        ]
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstPreField": null,
                           "firstEnumerator": [
                             false,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 true,
                                 false,
                                 true
                               ]
                             },
                             [
                               false,
                               null,
                               true,
                               false
                             ],
                             {
                               "$ref": "1"
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolEnumeratorNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolEnumeratorNullablePostFieldClassUnionRevisit> NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableBoolEnumeratorNullablePostFieldClassUnionRevisit>(new NullableBoolEnumeratorNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolEnumeratorNullablePostFieldClassUnionRevisit {
                         firstEnumerator: (List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator) [
                         (NullableStructBoolOrEnumeratorClassUnion) false,
                         (NullableStructBoolOrEnumeratorClassUnion) [],
                         (NullableStructBoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool?>) [ false, true, null, false ],
                         (NullableStructBoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool?>) [ true, false, true, null ],
                         (NullableStructBoolOrEnumeratorClassUnion) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableBoolEnumeratorNullablePostFieldClassUnionRevisit {
                           firstEnumerator: (List<NullableStructBoolOrEnumeratorClassUnion>.Enumerator) [
                             (NullableStructBoolOrEnumeratorClassUnion) false,
                             (NullableStructBoolOrEnumeratorClassUnion) [],
                             (NullableStructBoolOrEnumeratorClassUnion($id: 1)) (ReusableWrappingEnumerator<bool?>) [
                               false,
                               true,
                               null,
                               false
                             ],
                             (NullableStructBoolOrEnumeratorClassUnion) (ReusableWrappingEnumerator<bool?>) [
                               true,
                               false,
                               true,
                               null
                             ],
                             (NullableStructBoolOrEnumeratorClassUnion) {
                               $ref: 1
                             }
                           ],
                           firstPostField: null
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        false,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        false,
                        true,
                        null,
                        false
                        ]
                        },
                        [
                        true,
                        false,
                        true,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":null
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             false,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 false,
                                 true,
                                 null,
                                 false
                               ]
                             },
                             [
                               true,
                               false,
                               true,
                               null
                             ],
                             {
                               "$ref": "1"
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
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolEnumeratorNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
}
