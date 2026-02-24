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
public class DoubleCollectionRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<PreFieldDoubleArrayStructUnionRevisit>?                  prefieldDoubleArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<DoubleArrayPostFieldStructUnionRevisit>?                 boolArrayPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleArrayStructUnionRevisit>?  nullablePrefieldNullableDoubleArrayNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableDoubleArrayNullablePostFieldStructUnionRevisit>? nullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleArrayClassUnionRevisit>?                  prefieldDoubleArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleArrayPostFieldClassUnionRevisit>?                 boolArrayPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleArrayClassUnionRevisit>?  nullablePrefieldNullableDoubleArrayNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleArrayNullablePostFieldClassUnionRevisit>? nullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleSpanClassUnionRevisit>?                  prefieldDoubleSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleSpanPostFieldClassUnionRevisit>?                 boolSpanPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleSpanClassUnionRevisit>?  nullablePrefieldNullableDoubleSpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleSpanNullablePostFieldClassUnionRevisit>? nullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleReadOnlySpanClassUnionRevisit>?                  prefieldDoubleReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleReadOnlySpanPostFieldClassUnionRevisit>?                 boolReadOnlySpanPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit>?  nullablePrefieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit>? nullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleListStructUnionRevisit>?                  prefieldDoubleListNoRevealersStructUnionExpect;
    private static InputBearerExpect<DoubleListPostFieldStructUnionRevisit>?                 boolListPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleListStructUnionRevisit>?  nullablePrefieldNullableDoubleListNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableDoubleListNullablePostFieldStructUnionRevisit>? nullableDoubleListNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleListClassUnionRevisit>?                  prefieldDoubleListNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleListPostFieldClassUnionRevisit>?                 boolListPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleListClassUnionRevisit>?  nullablePrefieldNullableDoubleListNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleListNullablePostFieldClassUnionRevisit>? nullableDoubleListNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleEnumerableStructUnionRevisit>?                  prefieldDoubleEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<DoubleEnumerablePostFieldStructUnionRevisit>?                 boolEnumerablePostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleEnumerableStructUnionRevisit>?  nullablePrefieldNullableDoubleEnumerableNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableDoubleEnumerableNullablePostFieldStructUnionRevisit>? nullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleEnumerableClassUnionRevisit>?                  prefieldDoubleEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleEnumerablePostFieldClassUnionRevisit>?                 boolEnumerablePostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleEnumerableClassUnionRevisit>?  nullablePrefieldNullableDoubleEnumerableNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleEnumerableNullablePostFieldClassUnionRevisit>? nullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleEnumeratorStructUnionRevisit>?                  prefieldDoubleEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<DoubleEnumeratorPostFieldStructUnionRevisit>?                 boolEnumeratorPostFieldNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit>?  nullablePrefieldNullableDoubleEnumeratorNoRevealersStructUnionExpect;
    private static InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit>? nullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect;
    
    private static InputBearerExpect<PreFieldDoubleEnumeratorClassUnionRevisit>?                  prefieldDoubleEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<DoubleEnumeratorPostFieldClassUnionRevisit>?                 boolEnumeratorPostFieldNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit>?  nullablePrefieldNullableDoubleEnumeratorNoRevealersClassUnionExpect;
    private static InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit>? nullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldDoubleArrayStructUnionRevisit> PrefieldDoubleArrayNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldDoubleArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleArrayStructUnionRevisit>(new PreFieldDoubleArrayStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleArrayStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstArray: (SpanFormattableOrArrayStructUnion<double, double>[]) [
                         (SpanFormattableOrArrayStructUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrArrayStructUnion<double, double>) [],
                         (SpanFormattableOrArrayStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrArrayStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrArrayStructUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleArrayStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstArray: (SpanFormattableOrArrayStructUnion<double, double>[]) [
                             (SpanFormattableOrArrayStructUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrArrayStructUnion<double, double>) [],
                             (SpanFormattableOrArrayStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrArrayStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrArrayStructUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstArray":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstArray": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleArrayPostFieldStructUnionRevisit> DoubleArrayPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<DoubleArrayPostFieldStructUnionRevisit>(new DoubleArrayPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleArrayPostFieldStructUnionRevisit {
                         firstArray: (SpanFormattableOrArrayStructUnion<double, double>[]) [
                         (SpanFormattableOrArrayStructUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrArrayStructUnion<double, double>) [],
                         (SpanFormattableOrArrayStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrArrayStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrArrayStructUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleArrayPostFieldStructUnionRevisit {
                           firstArray: (SpanFormattableOrArrayStructUnion<double, double>[]) [
                             (SpanFormattableOrArrayStructUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrArrayStructUnion<double, double>) [],
                             (SpanFormattableOrArrayStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrArrayStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrArrayStructUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleArrayStructUnionRevisit> NullablePreFieldNullableDoubleArrayNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleArrayNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleArrayStructUnionRevisit>(new NullablePreFieldNullableDoubleArrayStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleArrayStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstArray: (NullableStructSpanFormattableOrArrayStructUnion<double>[]) [
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) null,
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) { $id: 1, $values: [ null, 1, 2, 3 ] },
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleArrayStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstArray: (NullableStructSpanFormattableOrArrayStructUnion<double>[]) [
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) null,
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstArray":[
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstArray": [
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleArrayNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleArrayNullablePostFieldStructUnionRevisit> NullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableDoubleArrayNullablePostFieldStructUnionRevisit>(new NullableDoubleArrayNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleArrayNullablePostFieldStructUnionRevisit {
                         firstArray: (NullableStructSpanFormattableOrArrayStructUnion<double>[]) [
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) [],
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) { $id: 1, $values: [ 1, 2, null, 3 ] },
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrArrayStructUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleArrayNullablePostFieldStructUnionRevisit {
                           firstArray: (NullableStructSpanFormattableOrArrayStructUnion<double>[]) [
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) [],
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrArrayStructUnion<double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleArrayClassUnionRevisit> PrefieldDoubleArrayNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleArrayClassUnionRevisit>(new PreFieldDoubleArrayClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleArrayClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstArray: (SpanFormattableOrArrayClassUnion<double, double>[]) [
                         (SpanFormattableOrArrayClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrArrayClassUnion<double, double>) [],
                         (SpanFormattableOrArrayClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrArrayClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrArrayClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleArrayClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstArray: (SpanFormattableOrArrayClassUnion<double, double>[]) [
                             (SpanFormattableOrArrayClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrArrayClassUnion<double, double>) [],
                             (SpanFormattableOrArrayClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrArrayClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrArrayClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstArray":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstArray": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleArrayNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleArrayPostFieldClassUnionRevisit> DoubleArrayPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolArrayPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleArrayPostFieldClassUnionRevisit>(new DoubleArrayPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleArrayPostFieldClassUnionRevisit {
                         firstArray: (SpanFormattableOrArrayClassUnion<double, double>[]) [
                         (SpanFormattableOrArrayClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrArrayClassUnion<double, double>) [],
                         (SpanFormattableOrArrayClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrArrayClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrArrayClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleArrayPostFieldClassUnionRevisit {
                           firstArray: (SpanFormattableOrArrayClassUnion<double, double>[]) [
                             (SpanFormattableOrArrayClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrArrayClassUnion<double, double>) [],
                             (SpanFormattableOrArrayClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrArrayClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrArrayClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstArray":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstArray": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleArrayPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleArrayPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleArrayClassUnionRevisit> NullablePreFieldNullableDoubleArrayNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleArrayNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleArrayClassUnionRevisit>(new NullablePreFieldNullableDoubleArrayClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleArrayClassUnionRevisit {
                         firstPreField: null,
                         firstArray: (NullableStructSpanFormattableOrArrayClassUnion<double>[]) [
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) null,
                         (NullableStructSpanFormattableOrArrayClassUnion<double>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleArrayClassUnionRevisit {
                           firstPreField: null,
                           firstArray: (NullableStructSpanFormattableOrArrayClassUnion<double>[]) [
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) null,
                             (NullableStructSpanFormattableOrArrayClassUnion<double>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleArrayNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleArrayNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleArrayNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleArrayNullablePostFieldClassUnionRevisit> NullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleArrayNullablePostFieldClassUnionRevisit>(new NullableDoubleArrayNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleArrayNullablePostFieldClassUnionRevisit {
                         firstArray: (NullableStructSpanFormattableOrArrayClassUnion<double>[]) [
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) [],
                         (NullableStructSpanFormattableOrArrayClassUnion<double>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrArrayClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleArrayNullablePostFieldClassUnionRevisit {
                           firstArray: (NullableStructSpanFormattableOrArrayClassUnion<double>[]) [
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) [],
                             (NullableStructSpanFormattableOrArrayClassUnion<double>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrArrayClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleArrayNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleArrayNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleArrayNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleSpanClassUnionRevisit> PrefieldDoubleSpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleSpanClassUnionRevisit>(new PreFieldDoubleSpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleSpanClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstSpan: (Span<SpanFormattableOrSpanClassUnion<double, double>>) [
                         (SpanFormattableOrSpanClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrSpanClassUnion<double, double>) [],
                         (SpanFormattableOrSpanClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrSpanClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrSpanClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleSpanClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstSpan: (Span<SpanFormattableOrSpanClassUnion<double, double>>) [
                             (SpanFormattableOrSpanClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrSpanClassUnion<double, double>) [],
                             (SpanFormattableOrSpanClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrSpanClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrSpanClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstSpan":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstSpan": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleSpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleSpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleSpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleSpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleSpanPostFieldClassUnionRevisit> DoubleSpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolSpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleSpanPostFieldClassUnionRevisit>(new DoubleSpanPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleSpanPostFieldClassUnionRevisit {
                         firstSpan: (Span<SpanFormattableOrSpanClassUnion<double, double>>) [
                         (SpanFormattableOrSpanClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrSpanClassUnion<double, double>) [],
                         (SpanFormattableOrSpanClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrSpanClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrSpanClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleSpanPostFieldClassUnionRevisit {
                           firstSpan: (Span<SpanFormattableOrSpanClassUnion<double, double>>) [
                             (SpanFormattableOrSpanClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrSpanClassUnion<double, double>) [],
                             (SpanFormattableOrSpanClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrSpanClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrSpanClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstSpan":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstSpan": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleSpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleSpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleSpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleSpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleSpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleSpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleSpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleSpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleSpanClassUnionRevisit> NullablePreFieldNullableDoubleSpanNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleSpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleSpanClassUnionRevisit>(new NullablePreFieldNullableDoubleSpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleSpanClassUnionRevisit {
                         firstPreField: null,
                         firstSpan: (Span<NullableStructSpanFormattableOrSpanClassUnion<double>>) [
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) null,
                         (NullableStructSpanFormattableOrSpanClassUnion<double>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleSpanClassUnionRevisit {
                           firstPreField: null,
                           firstSpan: (Span<NullableStructSpanFormattableOrSpanClassUnion<double>>) [
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) null,
                             (NullableStructSpanFormattableOrSpanClassUnion<double>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleSpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleSpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleSpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleSpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleSpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleSpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleSpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleSpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleSpanNullablePostFieldClassUnionRevisit> NullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleSpanNullablePostFieldClassUnionRevisit>(new NullableDoubleSpanNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleSpanNullablePostFieldClassUnionRevisit {
                         firstSpan: (Span<NullableStructSpanFormattableOrSpanClassUnion<double>>) [
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) [],
                         (NullableStructSpanFormattableOrSpanClassUnion<double>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrSpanClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleSpanNullablePostFieldClassUnionRevisit {
                           firstSpan: (Span<NullableStructSpanFormattableOrSpanClassUnion<double>>) [
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) [],
                             (NullableStructSpanFormattableOrSpanClassUnion<double>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrSpanClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleSpanNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleSpanNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleSpanNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleSpanNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleSpanNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleReadOnlySpanClassUnionRevisit> PrefieldDoubleReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleReadOnlySpanClassUnionRevisit>(new PreFieldDoubleReadOnlySpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleReadOnlySpanClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstReadOnlySpan: (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>) [
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleReadOnlySpanClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstReadOnlySpan: (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>) [
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstReadOnlySpan":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstReadOnlySpan": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleReadOnlySpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleReadOnlySpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleReadOnlySpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleReadOnlySpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleReadOnlySpanPostFieldClassUnionRevisit> DoubleReadOnlySpanPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolReadOnlySpanPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleReadOnlySpanPostFieldClassUnionRevisit>(new DoubleReadOnlySpanPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleReadOnlySpanPostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>) [
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrReadOnlySpanClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleReadOnlySpanPostFieldClassUnionRevisit {
                           firstReadOnlySpan: (ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<double, double>>) [
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrReadOnlySpanClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstReadOnlySpan":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstReadOnlySpan": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleReadOnlySpanPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleReadOnlySpanPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleReadOnlySpanPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleReadOnlySpanPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleReadOnlySpanPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleReadOnlySpanPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit> NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit>(new NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit {
                         firstPreField: null,
                         firstReadOnlySpan: (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>) [
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) null,
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleReadOnlySpanClassUnionRevisit {
                           firstPreField: null,
                           firstReadOnlySpan: (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>) [
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) null,
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleReadOnlySpanNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit> NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit>(new NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit {
                         firstReadOnlySpan: (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>) [
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [],
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleReadOnlySpanNullablePostFieldClassUnionRevisit {
                           firstReadOnlySpan: (ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>>) [
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [],
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrReadOnlySpanClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleReadOnlySpanNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<PreFieldDoubleListStructUnionRevisit> PrefieldDoubleListNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldDoubleListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleListStructUnionRevisit>(new PreFieldDoubleListStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleListStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstList: (List<SpanFormattableOrListStructUnion<double, double>>) [
                         (SpanFormattableOrListStructUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrListStructUnion<double, double>) [],
                         (SpanFormattableOrListStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrListStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrListStructUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleListStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstList: (List<SpanFormattableOrListStructUnion<double, double>>) [
                             (SpanFormattableOrListStructUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrListStructUnion<double, double>) [],
                             (SpanFormattableOrListStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrListStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrListStructUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstList":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstList": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleListPostFieldStructUnionRevisit> DoubleListPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<DoubleListPostFieldStructUnionRevisit>(new DoubleListPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleListPostFieldStructUnionRevisit {
                         firstList: (List<SpanFormattableOrListStructUnion<double, double>>) [
                         (SpanFormattableOrListStructUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrListStructUnion<double, double>) [],
                         (SpanFormattableOrListStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrListStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrListStructUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleListPostFieldStructUnionRevisit {
                           firstList: (List<SpanFormattableOrListStructUnion<double, double>>) [
                             (SpanFormattableOrListStructUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrListStructUnion<double, double>) [],
                             (SpanFormattableOrListStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrListStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrListStructUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleListStructUnionRevisit> NullablePreFieldNullableDoubleListNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleListNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleListStructUnionRevisit>(new NullablePreFieldNullableDoubleListStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleListStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstList: (List<NullableStructSpanFormattableOrListStructUnion<double>>) [
                         (NullableStructSpanFormattableOrListStructUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrListStructUnion<double>) null,
                         (NullableStructSpanFormattableOrListStructUnion<double>) { $id: 1, $values: [ null, 1, 2, 3 ] },
                         (NullableStructSpanFormattableOrListStructUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrListStructUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleListStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstList: (List<NullableStructSpanFormattableOrListStructUnion<double>>) [
                             (NullableStructSpanFormattableOrListStructUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrListStructUnion<double>) null,
                             (NullableStructSpanFormattableOrListStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrListStructUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrListStructUnion<double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstList":[
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstList": [
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleListNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleListNullablePostFieldStructUnionRevisit> NullableDoubleListNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableDoubleListNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableDoubleListNullablePostFieldStructUnionRevisit>(new NullableDoubleListNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleListNullablePostFieldStructUnionRevisit {
                         firstList: (List<NullableStructSpanFormattableOrListStructUnion<double>>) [
                         (NullableStructSpanFormattableOrListStructUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrListStructUnion<double>) [],
                         (NullableStructSpanFormattableOrListStructUnion<double>) { $id: 1, $values: [ 1, 2, null, 3 ] },
                         (NullableStructSpanFormattableOrListStructUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrListStructUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleListNullablePostFieldStructUnionRevisit {
                           firstList: (List<NullableStructSpanFormattableOrListStructUnion<double>>) [
                             (NullableStructSpanFormattableOrListStructUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrListStructUnion<double>) [],
                             (NullableStructSpanFormattableOrListStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrListStructUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrListStructUnion<double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleListClassUnionRevisit> PrefieldDoubleListNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleListClassUnionRevisit>(new PreFieldDoubleListClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleListClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstList: (List<SpanFormattableOrListClassUnion<double, double>>) [
                         (SpanFormattableOrListClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrListClassUnion<double, double>) [],
                         (SpanFormattableOrListClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrListClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrListClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleListClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstList: (List<SpanFormattableOrListClassUnion<double, double>>) [
                             (SpanFormattableOrListClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrListClassUnion<double, double>) [],
                             (SpanFormattableOrListClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrListClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrListClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstList":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstList": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleListNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleListPostFieldClassUnionRevisit> DoubleListPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolListPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleListPostFieldClassUnionRevisit>(new DoubleListPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleListPostFieldClassUnionRevisit {
                         firstList: (List<SpanFormattableOrListClassUnion<double, double>>) [
                         (SpanFormattableOrListClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrListClassUnion<double, double>) [],
                         (SpanFormattableOrListClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrListClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrListClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleListPostFieldClassUnionRevisit {
                           firstList: (List<SpanFormattableOrListClassUnion<double, double>>) [
                             (SpanFormattableOrListClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrListClassUnion<double, double>) [],
                             (SpanFormattableOrListClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrListClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrListClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstList":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstList": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleListPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleListPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleListClassUnionRevisit> NullablePreFieldNullableDoubleListNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleListNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleListClassUnionRevisit>(new NullablePreFieldNullableDoubleListClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleListClassUnionRevisit {
                         firstPreField: null,
                         firstList: (List<NullableStructSpanFormattableOrListClassUnion<double>>) [
                         (NullableStructSpanFormattableOrListClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrListClassUnion<double>) null,
                         (NullableStructSpanFormattableOrListClassUnion<double>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrListClassUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrListClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleListClassUnionRevisit {
                           firstPreField: null,
                           firstList: (List<NullableStructSpanFormattableOrListClassUnion<double>>) [
                             (NullableStructSpanFormattableOrListClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrListClassUnion<double>) null,
                             (NullableStructSpanFormattableOrListClassUnion<double>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrListClassUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrListClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleListNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleListNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleListNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleListNullablePostFieldClassUnionRevisit> NullableDoubleListNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleListNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleListNullablePostFieldClassUnionRevisit>(new NullableDoubleListNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleListNullablePostFieldClassUnionRevisit {
                         firstList: (List<NullableStructSpanFormattableOrListClassUnion<double>>) [
                         (NullableStructSpanFormattableOrListClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrListClassUnion<double>) [],
                         (NullableStructSpanFormattableOrListClassUnion<double>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrListClassUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrListClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleListNullablePostFieldClassUnionRevisit {
                           firstList: (List<NullableStructSpanFormattableOrListClassUnion<double>>) [
                             (NullableStructSpanFormattableOrListClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrListClassUnion<double>) [],
                             (NullableStructSpanFormattableOrListClassUnion<double>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrListClassUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrListClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleListNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleListNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleListNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleEnumerableStructUnionRevisit> PrefieldDoubleEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldDoubleEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleEnumerableStructUnionRevisit>(new PreFieldDoubleEnumerableStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleEnumerableStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerable: (List<SpanFormattableOrEnumerableStructUnion<double, double>>) [
                         (SpanFormattableOrEnumerableStructUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrEnumerableStructUnion<double, double>) [],
                         (SpanFormattableOrEnumerableStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrEnumerableStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumerableStructUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleEnumerableStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerable: (List<SpanFormattableOrEnumerableStructUnion<double, double>>) [
                             (SpanFormattableOrEnumerableStructUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrEnumerableStructUnion<double, double>) [],
                             (SpanFormattableOrEnumerableStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrEnumerableStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumerableStructUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerable":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerable": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleEnumerablePostFieldStructUnionRevisit> DoubleEnumerablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<DoubleEnumerablePostFieldStructUnionRevisit>(new DoubleEnumerablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleEnumerablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<SpanFormattableOrEnumerableStructUnion<double, double>>) [
                         (SpanFormattableOrEnumerableStructUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrEnumerableStructUnion<double, double>) [],
                         (SpanFormattableOrEnumerableStructUnion<double, double>) { $id: 1, $values: [ 1, 2, 3 ] },
                         (SpanFormattableOrEnumerableStructUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumerableStructUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleEnumerablePostFieldStructUnionRevisit {
                           firstEnumerable: (List<SpanFormattableOrEnumerableStructUnion<double, double>>) [
                             (SpanFormattableOrEnumerableStructUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrEnumerableStructUnion<double, double>) [],
                             (SpanFormattableOrEnumerableStructUnion<double, double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (SpanFormattableOrEnumerableStructUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumerableStructUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleEnumerableStructUnionRevisit> NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleEnumerableNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleEnumerableStructUnionRevisit>(new NullablePreFieldNullableDoubleEnumerableStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleEnumerableStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>) [
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) null,
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) { $id: 1, $values: [ null, 1, 2, 3 ] },
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleEnumerableStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>) [
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) null,
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerable":[
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerable": [
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleEnumerableNullablePostFieldStructUnionRevisit> NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableDoubleEnumerableNullablePostFieldStructUnionRevisit>(new NullableDoubleEnumerableNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleEnumerableNullablePostFieldStructUnionRevisit {
                         firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>) [
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [],
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) { $id: 1, $values: [ 1, 2, null, 3 ] },
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrEnumerableStructUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleEnumerableNullablePostFieldStructUnionRevisit {
                           firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableStructUnion<double>>) [
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [],
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) {
                               $id: 1,
                               $values: [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrEnumerableStructUnion<double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleEnumerableClassUnionRevisit> PrefieldDoubleEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleEnumerableClassUnionRevisit>(new PreFieldDoubleEnumerableClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleEnumerableClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerable: (List<SpanFormattableOrEnumerableClassUnion<double, double>>) [
                         (SpanFormattableOrEnumerableClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrEnumerableClassUnion<double, double>) [],
                         (SpanFormattableOrEnumerableClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumerableClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumerableClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleEnumerableClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerable: (List<SpanFormattableOrEnumerableClassUnion<double, double>>) [
                             (SpanFormattableOrEnumerableClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrEnumerableClassUnion<double, double>) [],
                             (SpanFormattableOrEnumerableClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumerableClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumerableClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerable":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerable": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumerableNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleEnumerablePostFieldClassUnionRevisit> DoubleEnumerablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumerablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleEnumerablePostFieldClassUnionRevisit>(new DoubleEnumerablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleEnumerablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<SpanFormattableOrEnumerableClassUnion<double, double>>) [
                         (SpanFormattableOrEnumerableClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrEnumerableClassUnion<double, double>) [],
                         (SpanFormattableOrEnumerableClassUnion<double, double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumerableClassUnion<double, double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumerableClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleEnumerablePostFieldClassUnionRevisit {
                           firstEnumerable: (List<SpanFormattableOrEnumerableClassUnion<double, double>>) [
                             (SpanFormattableOrEnumerableClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrEnumerableClassUnion<double, double>) [],
                             (SpanFormattableOrEnumerableClassUnion<double, double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumerableClassUnion<double, double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumerableClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerable":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerable": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleEnumerablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumerablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleEnumerableClassUnionRevisit> NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleEnumerableNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleEnumerableClassUnionRevisit>(new NullablePreFieldNullableDoubleEnumerableClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleEnumerableClassUnionRevisit {
                         firstPreField: null,
                         firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>) [
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) null,
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleEnumerableClassUnionRevisit {
                           firstPreField: null,
                           firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>) [
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) null,
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumerableNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleEnumerableNullablePostFieldClassUnionRevisit> NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleEnumerableNullablePostFieldClassUnionRevisit>(new NullableDoubleEnumerableNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleEnumerableNullablePostFieldClassUnionRevisit {
                         firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>) [
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [],
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrEnumerableClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleEnumerableNullablePostFieldClassUnionRevisit {
                           firstEnumerable: (List<NullableStructSpanFormattableOrEnumerableClassUnion<double>>) [
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [],
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrEnumerableClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumerableNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
  
    public static InputBearerExpect<PreFieldDoubleEnumeratorStructUnionRevisit> PrefieldDoubleEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return prefieldDoubleEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleEnumeratorStructUnionRevisit>(new PreFieldDoubleEnumeratorStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleEnumeratorStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerator: (List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator) [
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) [],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleEnumeratorStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerator: (List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator) [
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) [],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerator":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerator": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleEnumeratorPostFieldStructUnionRevisit> DoubleEnumeratorPostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<DoubleEnumeratorPostFieldStructUnionRevisit>(new DoubleEnumeratorPostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleEnumeratorPostFieldStructUnionRevisit {
                         firstEnumerator: (List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator) [
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) [],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>($id: 1)) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleEnumeratorPostFieldStructUnionRevisit {
                           firstEnumerator: (List<SpanFormattableOrEnumeratorStructUnion<double, double>>.Enumerator) [
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) [],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>($id: 1)) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumeratorStructUnion<double, double>) (ReusableWrappingEnumerator<double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit> NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleEnumeratorNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit>(new NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator) [
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) null,
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>($id: 1)) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleEnumeratorStructUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator) [
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) null,
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>($id: 1)) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerator":[
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerator": [
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersStructUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit> NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect
    {
        get
        {
            return nullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect ??=
                new InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit>(new NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit {
                         firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator) [
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) [],
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>($id: 1)) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleEnumeratorNullablePostFieldStructUnionRevisit {
                           firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorStructUnion<double>>.Enumerator) [
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) [],
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>($id: 1)) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrEnumeratorStructUnion<double>) (ReusableWrappingEnumerator<double?>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
                        null
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
                               null
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersStructUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<PreFieldDoubleEnumeratorClassUnionRevisit> PrefieldDoubleEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return prefieldDoubleEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<PreFieldDoubleEnumeratorClassUnionRevisit>(new PreFieldDoubleEnumeratorClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        PreFieldDoubleEnumeratorClassUnionRevisit {
                         firstPreField: 2.718281828459045,
                         firstEnumerator: (List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator) [
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) 3.141592653589793,
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) [],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>($id: 1)) (ReusableWrappingEnumerator<double>) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) (ReusableWrappingEnumerator<double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         PreFieldDoubleEnumeratorClassUnionRevisit {
                           firstPreField: 2.718281828459045,
                           firstEnumerator: (List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator) [
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) 3.141592653589793,
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) [],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>($id: 1)) (ReusableWrappingEnumerator<double>) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) (ReusableWrappingEnumerator<double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) {
                               $ref: 1
                             }
                           ]
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstPreField":2.718281828459045,
                        "firstEnumerator":[
                        3.141592653589793,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
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
                           "firstPreField": 2.718281828459045,
                           "firstEnumerator": [
                             3.141592653589793,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
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
    public void PrefieldDoubleEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void PrefieldDoubleEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(PrefieldDoubleEnumeratorNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<DoubleEnumeratorPostFieldClassUnionRevisit> DoubleEnumeratorPostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return boolEnumeratorPostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<DoubleEnumeratorPostFieldClassUnionRevisit>(new DoubleEnumeratorPostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        DoubleEnumeratorPostFieldClassUnionRevisit {
                         firstEnumerator: (List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator) [
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) 2.718281828459045,
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) [],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>($id: 1)) (ReusableWrappingEnumerator<double>) [ 1, 2, 3 ],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) (ReusableWrappingEnumerator<double>) [ 4, 5, 6 ],
                         (SpanFormattableOrEnumeratorClassUnion<double, double>) { $ref: 1 }
                         ],
                         firstPostField: 3.141592653589793
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         DoubleEnumeratorPostFieldClassUnionRevisit {
                           firstEnumerator: (List<SpanFormattableOrEnumeratorClassUnion<double, double>>.Enumerator) [
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) 2.718281828459045,
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) [],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>($id: 1)) (ReusableWrappingEnumerator<double>) [
                               1,
                               2,
                               3
                             ],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) (ReusableWrappingEnumerator<double>) [
                               4,
                               5,
                               6
                             ],
                             (SpanFormattableOrEnumeratorClassUnion<double, double>) {
                               $ref: 1
                             }
                           ],
                           firstPostField: 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "firstEnumerator":[
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6
                        ],
                        {
                        "$ref":"1"
                        }
                        ],
                        "firstPostField":3.141592653589793
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """
                         {
                           "firstEnumerator": [
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6
                             ],
                             {
                               "$ref": "1"
                             }
                           ],
                           "firstPostField": 3.141592653589793
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void DoubleEnumeratorPostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DoubleEnumeratorPostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
    public static InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit> NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionExpect
    {
        get
        {
            return nullablePrefieldNullableDoubleEnumeratorNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit>(new NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit {
                         firstPreField: null,
                         firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator) [
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) 3.141592653589793,
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) null,
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>($id: 1)) (ReusableWrappingEnumerator<double?>) [ null, 1, 2, 3 ],
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) (ReusableWrappingEnumerator<double?>) [ 4, null, 5, 6 ],
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) { $ref: 1 }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullablePreFieldNullableDoubleEnumeratorClassUnionRevisit {
                           firstPreField: null,
                           firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator) [
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) 3.141592653589793,
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) null,
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>($id: 1)) (ReusableWrappingEnumerator<double?>) [
                               null,
                               1,
                               2,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) (ReusableWrappingEnumerator<double?>) [
                               4,
                               null,
                               5,
                               6
                             ],
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) {
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
                        3.141592653589793,
                        null,
                        {
                        "$id":"1",
                        "$values":[
                        null,
                        1,
                        2,
                        3
                        ]
                        },
                        [
                        4,
                        null,
                        5,
                        6
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
                             3.141592653589793,
                             null,
                             {
                               "$id": "1",
                               "$values": [
                                 null,
                                 1,
                                 2,
                                 3
                               ]
                             },
                             [
                               4,
                               null,
                               5,
                               6
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
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullablePreFieldNullableDoubleEnumeratorNoRevealersClassUnionExpect, PrettyJson);
    }

    public static InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit> NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect
    {
        get
        {
            return nullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect ??=
                new InputBearerExpect<NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit>(new NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit())
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit {
                         firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator) [
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) 2.718281828459045,
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) [],
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>($id: 1)) (ReusableWrappingEnumerator<double?>) [ 1, 2, null, 3 ],
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) (ReusableWrappingEnumerator<double?>) [ 4, 5, 6, null ],
                         (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) { $ref: 1 }
                         ],
                         firstPostField: null
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         NullableDoubleEnumeratorNullablePostFieldClassUnionRevisit {
                           firstEnumerator: (List<NullableStructSpanFormattableOrEnumeratorClassUnion<double>>.Enumerator) [
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) 2.718281828459045,
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) [],
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>($id: 1)) (ReusableWrappingEnumerator<double?>) [
                               1,
                               2,
                               null,
                               3
                             ],
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) (ReusableWrappingEnumerator<double?>) [
                               4,
                               5,
                               6,
                               null
                             ],
                             (NullableStructSpanFormattableOrEnumeratorClassUnion<double>) {
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
                        2.718281828459045,
                        [],
                        {
                        "$id":"1",
                        "$values":[
                        1,
                        2,
                        null,
                        3
                        ]
                        },
                        [
                        4,
                        5,
                        6,
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
                             2.718281828459045,
                             [],
                             {
                               "$id": "1",
                               "$values": [
                                 1,
                                 2,
                                 null,
                                 3
                               ]
                             },
                             [
                               4,
                               5,
                               6,
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
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect, CompactLog);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect, CompactJson);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect, PrettyLog);
    }

    [TestMethod]
    public void NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(NullableDoubleEnumeratorNullablePostFieldNoRevealersClassUnionExpect, PrettyJson);
    }
    
}
