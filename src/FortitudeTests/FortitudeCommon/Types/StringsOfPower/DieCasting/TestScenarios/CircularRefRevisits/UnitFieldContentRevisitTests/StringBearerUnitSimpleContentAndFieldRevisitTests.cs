// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class StringBearerUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;
    
    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingxpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoStringBearersFields<BinaryBranchNode<LeafNode>> TwoSameOneBranchTwoSameLeafFields
    {
        get
        {
            var child                                = new LeafNode("SameChild");
            var secondFieldSame                      = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchNodeTwoSameLeafNodes = new TwoStringBearersFields<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchNodeTwoSameLeafNodes;
        }
    }

    public static InputBearerExpect<TwoStringBearersFields<BinaryBranchNode<LeafNode>>>
        TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFields<BinaryBranchNode<LeafNode>>>(TwoSameOneBranchTwoSameLeafFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFields<BinaryBranchNode<LeafNode>> {
                         FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                         SecondStringBearerField: BinaryBranchNode<LeafNode> {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersFields<BinaryBranchNode<LeafNode>> {
                          FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                          SecondStringBearerField: BinaryBranchNode<LeafNode> {
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
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchNodeTwoSameLeafNodes =
                new TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchNodeTwoSameLeafNodes;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNode<LeafNode>) {
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
                         SecondStringBearerField: BinaryBranchNode<LeafNode> {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNode<LeafNode>) {
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
                          SecondStringBearerField: BinaryBranchNode<LeafNode> {
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValue
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked
                = new TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>> {
                         FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                         SecondStringBearerField: (BinaryBranchNode<LeafNode>) {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>> {
                          FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                          SecondStringBearerField: (BinaryBranchNode<LeafNode>) {
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields =
                new TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNode<LeafNode>($id: 2)) "{
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
                         SecondStringBearerField: BinaryBranchNode<LeafNode> {
                         $ref: 2
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNode<LeafNode>($id: 2)) "{
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
                          SecondStringBearerField: BinaryBranchNode<LeafNode> {
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    public static TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedString
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString
                = new TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>> {
                         FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                         SecondStringBearerField: (BinaryBranchNode<LeafNode>) { $ref: 2 }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNode<LeafNode>> {
                          FirstStringBearerField: BinaryBranchNode<LeafNode> {
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
                          SecondStringBearerField: (BinaryBranchNode<LeafNode>) {
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
}
