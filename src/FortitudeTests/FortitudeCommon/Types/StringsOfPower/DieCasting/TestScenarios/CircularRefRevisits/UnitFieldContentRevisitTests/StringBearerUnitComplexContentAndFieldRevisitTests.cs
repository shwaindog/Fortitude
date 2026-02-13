// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class StringBearerUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchNodeTwoSameLeafNodes =
                new TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchNodeTwoSameLeafNodes;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                         $id: 2,
                         CloakedRevealerFirstStringBearerField: {
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode {
                         $ref: 1
                         }
                         },
                         logOnlyArray: [
                         1,
                         2,
                         3
                         ]
                         },
                         SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersFirstAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                            $id: 2,
                            CloakedRevealerFirstStringBearerField: {
                              Name: "SameOnLeftAndRight",
                              GlobalNodeInstanceId: 2,
                              NodeType: NodeType.BranchNode,
                              Left: LeafNode {
                                $id: 1,
                                LeafInstanceId: 1,
                                Name: "SameChild",
                                GlobalNodeInstanceId: 1,
                                NodeType: NodeType.LeafNode,
                                DepthToRoot: 1
                              },
                              Right: LeafNode {
                                $ref: 1
                              }
                            },
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                            $ref: 2
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBearerField":{
                        "$id":"2",
                        "Name":"SameOnLeftAndRight",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "Left":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        "Right":{
                        "$ref":"1"
                        }
                        },
                        "SecondStringBearerField":{
                        "$ref":"2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBearerField": {
                            "$id": "2",
                            "Name": "SameOnLeftAndRight",
                            "GlobalNodeInstanceId": 2,
                            "NodeType": "BranchNode",
                            "Left": {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "SameChild",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            "Right": {
                              "$ref": "1"
                            }
                          },
                          "SecondStringBearerField": {
                            "$ref": "2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValue
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked
                = new TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                         $id: 2,
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode {
                         $ref: 1
                         }
                         },
                         SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersSecondAsComplexCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                            $id: 2,
                            Name: "SameOnLeftAndRight",
                            GlobalNodeInstanceId: 2,
                            NodeType: NodeType.BranchNode,
                            Left: LeafNode {
                              $id: 1,
                              LeafInstanceId: 1,
                              Name: "SameChild",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            Right: LeafNode {
                              $ref: 1
                            }
                          },
                          SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                            $ref: 2
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBearerField":{
                        "$id":"2",
                        "Name":"SameOnLeftAndRight",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "Left":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        "Right":{
                        "$ref":"1"
                        }
                        },
                        "SecondStringBearerField":{
                        "$ref":"2"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBearerField": {
                            "$id": "2",
                            "Name": "SameOnLeftAndRight",
                            "GlobalNodeInstanceId": 2,
                            "NodeType": "BranchNode",
                            "Left": {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "SameChild",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            "Right": {
                              "$ref": "1"
                            }
                          },
                          "SecondStringBearerField": {
                            "$ref": "2"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields =
                new TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                         $id: 2,
                         CloakedRevealerFirstStringBearerField: "{
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode {
                         $ref: 1
                         }
                         }",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                            $id: 2,
                            CloakedRevealerFirstStringBearerField: "{
                              Name: "SameOnLeftAndRight",
                              GlobalNodeInstanceId: 2,
                              NodeType: NodeType.BranchNode,
                              Left: LeafNode {
                                $id: 1,
                                LeafInstanceId: 1,
                                Name: "SameChild",
                                GlobalNodeInstanceId: 1,
                                NodeType: NodeType.LeafNode,
                                DepthToRoot: 1
                              },
                              Right: LeafNode {
                                $ref: 1
                              }
                            }",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                            $ref: 2
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "FirstStringBearerField":"{
                        \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                        \u0022GlobalNodeInstanceId\u0022:2,
                        \u0022NodeType\u0022:\u0022BranchNode\u0022,
                        \u0022Left\u0022:{
                        \u0022$id\u0022:\u00221\u0022,
                        \u0022LeafInstanceId\u0022:1,
                        \u0022Name\u0022:\u0022SameChild\u0022,
                        \u0022GlobalNodeInstanceId\u0022:1,
                        \u0022NodeType\u0022:\u0022LeafNode\u0022,
                        \u0022DepthToRoot\u0022:1
                        },
                        \u0022Right\u0022:{
                        \u0022$ref\u0022:\u00221\u0022
                        }
                        }",
                        "SecondStringBearerField":{
                        "Name":"SameOnLeftAndRight",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "Left":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        "Right":{
                        "$ref":"1"
                        }
                        }
                        }
                        """.RemoveLineEndings()

                        // removed on default \u0022$id\u0022:\u00222\u0022,
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {

                        """.Dos2Unix()
                       +
                        """
                              "FirstStringBearerField": "{
                            \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                            \u0022GlobalNodeInstanceId\u0022:2,
                            \u0022NodeType\u0022:\u0022BranchNode\u0022,
                            \u0022Left\u0022:{
                            \u0022$id\u0022:\u00221\u0022,
                            \u0022LeafInstanceId\u0022:1,
                            \u0022Name\u0022:\u0022SameChild\u0022,
                            \u0022GlobalNodeInstanceId\u0022:1,
                            \u0022NodeType\u0022:\u0022LeafNode\u0022,
                            \u0022DepthToRoot\u0022:1
                            },
                            \u0022Right\u0022:{
                            \u0022$ref\u0022:\u00221\u0022
                            }
                            }",
                            """.RemoveLineEndings()
                        // +
                        //  """
                        //        "FirstStringBearerField": "{\u000a
                        //          \u0022$id\u0022: \u00222\u0022,\u000a
                        //          \u0022Name\u0022: \u0022SameOnLeftAndRight\u0022,\u000a
                        //          \u0022GlobalNodeInstanceId\u0022: 2,\u000a
                        //          \u0022NodeType\u0022: \u0022BranchNode\u0022,\u000a
                        //          \u0022Left\u0022: {\u000a
                        //            \u0022$id\u0022: \u00221\u0022,\u000a
                        //            \u0022LeafInstanceId\u0022: 1,\u000a
                        //            \u0022Name\u0022: \u0022SameChild\u0022,\u000a
                        //            \u0022GlobalNodeInstanceId\u0022: 1,\u000a
                        //            \u0022NodeType\u0022: \u0022LeafNode\u0022,\u000a
                        //            \u0022DepthToRoot\u0022: 1\u000a
                        //          },\u000a
                        //          \u0022Right\u0022: {\u000a
                        //            \u0022$ref\u0022: \u00221\u0022\u000a
                        //          }\u000a
                        //        }",
                        //      """.RemoveLineEndings()
                       +
                        """

                              "SecondStringBearerField": {
                                "Name": "SameOnLeftAndRight",
                                "GlobalNodeInstanceId": 2,
                                "NodeType": "BranchNode",
                                "Left": {
                                  "$id": "1",
                                  "LeafInstanceId": 1,
                                  "Name": "SameChild",
                                  "GlobalNodeInstanceId": 1,
                                  "NodeType": "LeafNode",
                                  "DepthToRoot": 1
                                },
                                "Right": {
                                  "$ref": "1"
                                }
                              }
                            }
                            """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedString
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString
                = new TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                         $id: 2,
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode {
                         $ref: 1
                         }
                         },
                         SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) { $ref: 2 }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                            $id: 2,
                            Name: "SameOnLeftAndRight",
                            GlobalNodeInstanceId: 2,
                            NodeType: NodeType.BranchNode,
                            Left: LeafNode {
                              $id: 1,
                              LeafInstanceId: 1,
                              Name: "SameChild",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            Right: LeafNode {
                              $ref: 1
                            }
                          },
                          SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                            $ref: 2
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBearerField":{
                        "Name":"SameOnLeftAndRight",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "Left":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        "Right":{
                        "$ref":"1"
                        }
                        },
                        "SecondStringBearerField":"{
                        \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                        \u0022GlobalNodeInstanceId\u0022:2,
                        \u0022NodeType\u0022:\u0022BranchNode\u0022,
                        \u0022Left\u0022:{
                        \u0022$id\u0022:\u00221\u0022,
                        \u0022LeafInstanceId\u0022:1,
                        \u0022Name\u0022:\u0022SameChild\u0022,
                        \u0022GlobalNodeInstanceId\u0022:1,
                        \u0022NodeType\u0022:\u0022LeafNode\u0022,
                        \u0022DepthToRoot\u0022:1
                        },
                        \u0022Right\u0022:{
                        \u0022$ref\u0022:\u00221\u0022
                        }
                        }"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBearerField": {
                            "Name": "SameOnLeftAndRight",
                            "GlobalNodeInstanceId": 2,
                            "NodeType": "BranchNode",
                            "Left": {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "SameChild",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            "Right": {
                              "$ref": "1"
                            }
                          },

                        """.Dos2Unix()
                       +
                        """
                              "SecondStringBearerField": "{
                            \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                            \u0022GlobalNodeInstanceId\u0022:2,
                            \u0022NodeType\u0022:\u0022BranchNode\u0022,
                            \u0022Left\u0022:{
                            \u0022$id\u0022:\u00221\u0022,
                            \u0022LeafInstanceId\u0022:1,
                            \u0022Name\u0022:\u0022SameChild\u0022,
                            \u0022GlobalNodeInstanceId\u0022:1,
                            \u0022NodeType\u0022:\u0022LeafNode\u0022,
                            \u0022DepthToRoot\u0022:1
                            },
                            \u0022Right\u0022:{
                            \u0022$ref\u0022:\u00221\u0022
                            }
                            }"
                            """.RemoveLineEndings()
                       +
                        """

                            }
                            """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }
    

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedValueWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    public static InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneField)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                             FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                             CloakedRevealerFirstStringBearerField: "{
                             Name: "SameOnLeftAndRight",
                             GlobalNodeInstanceId: 2,
                             NodeType: NodeType.BranchNode,
                             Left: LeafNode {
                             $id: 1,
                             LeafInstanceId: 1,
                             Name: "SameChild",
                             GlobalNodeInstanceId: 1,
                             NodeType: NodeType.LeafNode,
                             DepthToRoot: 1
                             },
                             Right: LeafNode {
                             $ref: 1
                             }
                             }",
                             logOnlyStringBuilder: "For your eyes only"
                             },
                             SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                             Name: "SameOnLeftAndRight",
                             GlobalNodeInstanceId: 2,
                             NodeType: NodeType.BranchNode,
                             Left: LeafNode {
                             $id: 2,
                             LeafInstanceId: 1,
                             Name: "SameChild",
                             GlobalNodeInstanceId: 1,
                             NodeType: NodeType.LeafNode,
                             DepthToRoot: 1
                             },
                             Right: LeafNode {
                             $ref: 2
                             }
                             }
                             }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                          , """
                            TwoStringBearersFirstAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                              FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                                CloakedRevealerFirstStringBearerField: "{
                                  Name: "SameOnLeftAndRight",
                                  GlobalNodeInstanceId: 2,
                                  NodeType: NodeType.BranchNode,
                                  Left: LeafNode {
                                    $id: 1,
                                    LeafInstanceId: 1,
                                    Name: "SameChild",
                                    GlobalNodeInstanceId: 1,
                                    NodeType: NodeType.LeafNode,
                                    DepthToRoot: 1
                                  },
                                  Right: LeafNode {
                                    $ref: 1
                                  }
                                }",
                                logOnlyStringBuilder: "For your eyes only"
                              },
                              SecondStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                                Name: "SameOnLeftAndRight",
                                GlobalNodeInstanceId: 2,
                                NodeType: NodeType.BranchNode,
                                Left: LeafNode {
                                  $id: 2,
                                  LeafInstanceId: 1,
                                  Name: "SameChild",
                                  GlobalNodeInstanceId: 1,
                                  NodeType: NodeType.LeafNode,
                                  DepthToRoot: 1
                                },
                                Right: LeafNode {
                                  $ref: 2
                                }
                              }
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                          , """
                            {
                            "FirstStringBearerField":{
                            "$id":"2",
                            "$values":"{
                            \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                            \u0022GlobalNodeInstanceId\u0022:2,
                            \u0022NodeType\u0022:\u0022BranchNode\u0022,
                            \u0022Left\u0022:{
                            \u0022$id\u0022:\u00221\u0022,
                            \u0022LeafInstanceId\u0022:1,
                            \u0022Name\u0022:\u0022SameChild\u0022,
                            \u0022GlobalNodeInstanceId\u0022:1,
                            \u0022NodeType\u0022:\u0022LeafNode\u0022,
                            \u0022DepthToRoot\u0022:1
                            },
                            \u0022Right\u0022:{
                            \u0022$ref\u0022:\u00221\u0022
                            }
                            }"
                            },
                            "SecondStringBearerField":{
                            "$ref":"2"
                            }
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                          , """ 
                            {
                              "FirstStringBearerField": {
                                "$id": "2",
                                "$values": "{
                            """.Dos2Unix()
                           +
                            """
                            \u0022Name\u0022:\u0022SameOnLeftAndRight\u0022,
                            \u0022GlobalNodeInstanceId\u0022:2,
                            \u0022NodeType\u0022:\u0022BranchNode\u0022,
                            \u0022Left\u0022:{
                            \u0022$id\u0022:\u00221\u0022,
                            \u0022LeafInstanceId\u0022:1,
                            \u0022Name\u0022:\u0022SameChild\u0022,
                            \u0022GlobalNodeInstanceId\u0022:1,
                            \u0022NodeType\u0022:\u0022LeafNode\u0022,
                            \u0022DepthToRoot\u0022:1
                            },
                            \u0022Right\u0022:{
                            \u0022$ref\u0022:\u00221\u0022
                            }
                            }"
                            """.RemoveLineEndings()
                           +
                            """
                            
                              },
                              "SecondStringBearerField": {
                                "$ref": "2"
                              }
                            }
                            """.Dos2Unix()
                        }
                    };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneComplexCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    public static InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedString)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                             FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                             Name: "SameOnLeftAndRight",
                             GlobalNodeInstanceId: 2,
                             NodeType: NodeType.BranchNode,
                             Left: LeafNode {
                             $id: 1,
                             LeafInstanceId: 1,
                             Name: "SameChild",
                             GlobalNodeInstanceId: 1,
                             NodeType: NodeType.LeafNode,
                             DepthToRoot: 1
                             },
                             Right: LeafNode {
                             $ref: 1
                             }
                             },
                             SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                             CloakedRevealerSecondStringBearerField: "{
                             Name: "SameOnLeftAndRight",
                             GlobalNodeInstanceId: 2,
                             NodeType: NodeType.BranchNode,
                             Left: LeafNode {
                             $id: 2,
                             LeafInstanceId: 1,
                             Name: "SameChild",
                             GlobalNodeInstanceId: 1,
                             NodeType: NodeType.LeafNode,
                             DepthToRoot: 1
                             },
                             Right: LeafNode {
                             $ref: 2
                             }
                             }",
                             logOnlyList: [ FirstCharSeq, SecondCharSeq, ThirdCharSeq ]
                             }
                             }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                          , """
                            TwoStringBearersSecondAsComplexCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                              FirstStringBearerField: BinaryBranchNodeAsField<LeafNode> {
                                Name: "SameOnLeftAndRight",
                                GlobalNodeInstanceId: 2,
                                NodeType: NodeType.BranchNode,
                                Left: LeafNode {
                                  $id: 1,
                                  LeafInstanceId: 1,
                                  Name: "SameChild",
                                  GlobalNodeInstanceId: 1,
                                  NodeType: NodeType.LeafNode,
                                  DepthToRoot: 1
                                },
                                Right: LeafNode {
                                  $ref: 1
                                }
                              },
                              SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) {
                                CloakedRevealerSecondStringBearerField: "{
                                  Name: "SameOnLeftAndRight",
                                  GlobalNodeInstanceId: 2,
                                  NodeType: NodeType.BranchNode,
                                  Left: LeafNode {
                                    $id: 2,
                                    LeafInstanceId: 1,
                                    Name: "SameChild",
                                    GlobalNodeInstanceId: 1,
                                    NodeType: NodeType.LeafNode,
                                    DepthToRoot: 1
                                  },
                                  Right: LeafNode {
                                    $ref: 2
                                  }
                                }",
                                logOnlyList: [
                                  FirstCharSeq,
                                  SecondCharSeq,
                                  ThirdCharSeq
                                ]
                              }
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                          , """
                            {
                            "FirstStringBearerField":{
                            "$id":"2",
                            "Name":"SameOnLeftAndRight",
                            "GlobalNodeInstanceId":2,
                            "NodeType":"BranchNode",
                            "Left":{
                            "$id":"1",
                            "LeafInstanceId":1,
                            "Name":"SameChild",
                            "GlobalNodeInstanceId":1,
                            "NodeType":"LeafNode",
                            "DepthToRoot":1
                            },
                            "Right":{
                            "$ref":"1"
                            }
                            },
                            "SecondStringBearerField":{
                            "$ref":"2"
                            }
                            }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                          , """
                            {
                              "FirstStringBearerField": {
                                "$id": "2",
                                "Name": "SameOnLeftAndRight",
                                "GlobalNodeInstanceId": 2,
                                "NodeType": "BranchNode",
                                "Left": {
                                  "$id": "1",
                                  "LeafInstanceId": 1,
                                  "Name": "SameChild",
                                  "GlobalNodeInstanceId": 1,
                                  "NodeType": "LeafNode",
                                  "DepthToRoot": 1
                                },
                                "Right": {
                                  "$ref": "1"
                                }
                              },
                              "SecondStringBearerField": {
                                "$ref": "2"
                              }
                            }
                            """.Dos2Unix()
                        }
                    };
        }
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneComplexCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
}
