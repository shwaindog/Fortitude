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
    
    private static InputBearerExpect<PreFieldBoolArrayRevisit>? simpleValuePrefieldNoRevealersBoolArrayExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static InputBearerExpect<PreFieldBoolArrayRevisit> SimpleValuePrefieldNoRevealersBoolArrayExpect
    {
        get
        {
            return simpleValuePrefieldNoRevealersBoolArrayExpect ??=
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
    public void SelfReferencingParentNodeAsSimpleCollectionCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SimpleValuePrefieldNoRevealersBoolArrayExpect, CompactLog);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SimpleValuePrefieldNoRevealersBoolArrayExpect, CompactJson);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SimpleValuePrefieldNoRevealersBoolArrayExpect, PrettyLog);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SimpleValuePrefieldNoRevealersBoolArrayExpect, PrettyJson);
    }
}
