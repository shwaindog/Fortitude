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
    private static InputBearerExpect<PreFieldBoolArrayRevisit>?                  prefieldBoolArrayNoRevealersExpect;
    private static InputBearerExpect<BoolArrayPostFieldRevisit>?                 boolArrayPostFieldNoRevealersExpect;
    private static InputBearerExpect<NullablePreFieldNullableBoolArrayRevisit>?  nullablePrefieldNullableBoolArrayNoRevealersExpect;
    private static InputBearerExpect<NullableBoolArrayNullablePostFieldRevisit>? nullableBoolArrayNullablePostFieldNoRevealersExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldBoolArrayRevisit> PrefieldBoolArrayNoRevealersExpect
    {
        get
        {
            return prefieldBoolArrayNoRevealersExpect ??=
                new InputBearerExpect<PreFieldBoolArrayRevisit>(new PreFieldBoolArrayRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldBoolArrayRevisit {
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
                         PreFieldBoolArrayRevisit {
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
    public void PrefieldBoolArrayNoRevealersExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldBoolArrayNoRevealersExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldBoolArrayNoRevealersExpect, PrettyJson);
    }

    public static InputBearerExpect<BoolArrayPostFieldRevisit> BoolArrayPostFieldNoRevealersExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersExpect ??=
                new InputBearerExpect<BoolArrayPostFieldRevisit>(new BoolArrayPostFieldRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BoolArrayPostFieldRevisit {
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
                         BoolArrayPostFieldRevisit {
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
    public void BoolArrayPostFieldNoRevealersCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersExpect, CompactLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersExpect, CompactJson);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersExpect, PrettyLog);
    }

    [TestMethod]
    public void BoolArrayPostFieldNoRevealersPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(BoolArrayPostFieldNoRevealersExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableBoolArrayRevisit> NullablePreFieldNullableBoolArrayNoRevealersExpect
    {
        get
        {
            return nullablePrefieldNullableBoolArrayNoRevealersExpect ??=
                new InputBearerExpect<NullablePreFieldNullableBoolArrayRevisit>(new NullablePreFieldNullableBoolArrayRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableBoolArrayRevisit {
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
                         NullablePreFieldNullableBoolArrayRevisit {
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
    public void NullablePreFieldNullableBoolArrayNoRevealersCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableBoolArrayNoRevealersPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableBoolArrayNoRevealersExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableBoolArrayNullablePostFieldRevisit> NullableBoolArrayNullablePostFieldNoRevealersExpect
    {
        get
        {
            return nullableBoolArrayNullablePostFieldNoRevealersExpect ??=
                new InputBearerExpect<NullableBoolArrayNullablePostFieldRevisit>(new NullableBoolArrayNullablePostFieldRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableBoolArrayNullablePostFieldRevisit {
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
                         NullableBoolArrayNullablePostFieldRevisit {
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
    public void NullableBoolArrayNullablePostFieldNoRevealersCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersExpect, CompactLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersExpect, CompactJson);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableBoolArrayNullablePostFieldNoRevealersPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableBoolArrayNullablePostFieldNoRevealersExpect, PrettyJson);
    }
    
    
}
