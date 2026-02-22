// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
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
public class StringBearerUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodeFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodeAsStringFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>?
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

    public static TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> TwoSameOneBranchTwoSameLeafFields
    {
        get
        {
            var child                                = new LeafNode("SameChild");
            var secondFieldSame                      = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchNodeTwoSameLeafNodes = new TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchNodeTwoSameLeafNodes;
        }
    }

    public static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodeFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>(TwoSameOneBranchTwoSameLeafFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
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
                        TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
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
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodeAsStringFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafFields, formatFlags: FormatFlags.AsStringContent)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
                             FirstStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                             }",
                             SecondStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
                             $ref: 2
                             }"
                             }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                          , """
                            TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
                              FirstStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                              }",
                              SecondStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
                                $ref: 2
                              }"
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
                           +
                            "\n"
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
    public void TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesAsStringFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchNodeTwoSameLeafNodes =
                new TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchNodeTwoSameLeafNodes;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>($id: 2)) {
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
                        TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>($id: 2)) {
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

    public static TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValue
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked
                = new TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
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
                        TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNodeAsField<LeafNode>> {
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

    public static TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneField
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields =
                new TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                         FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>($id: 2)) "{
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
                        TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                          FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>($id: 2)) "{
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

    public static TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedString
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child);
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString
                = new TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>(
                 TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
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
                        TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
                                           , CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
                                           , CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
                                           , PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
                                           , PrettyJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeFieldsWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect
                                                      , new StyleOptions(CompactLog)
                                                        {
                                                            InstanceTrackingAllAsStringHaveLocalTracking = true
                                                        });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeFieldsWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect
                                                      , new StyleOptions(CompactJson)
                                                        {
                                                            InstanceTrackingAllAsStringHaveLocalTracking = false
                                                        });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeFieldsWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect
                                                      , new StyleOptions(PrettyLog)
                                                        {
                                                            InstanceTrackingAllAsStringHaveLocalTracking = true
                                                        });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeFieldsWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions(TwoSameOneBranchTwoSameLeafNodesFieldsWithDefaultRevisitSettingsExpect
                                                      , new StyleOptions(PrettyJson)
                                                        {
                                                            InstanceTrackingAllAsStringHaveLocalTracking = false
                                                        });
    }

    public static InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafFields, formatFlags: FormatFlags.AsStringContent)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
                             FirstStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                             SecondStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                             }"
                             }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                          , """
                            TwoStringBearersFields<BinaryBranchNodeAsField<LeafNode>> {
                              FirstStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                              SecondStringBearerField: "BinaryBranchNodeAsField<LeafNode> {
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
                              }"
                            }
                            """.Dos2Unix()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                          , """
                            {
                            "FirstStringBearerField":"{
                            \u0022$id\u0022:\u00222\u0022,
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
                            "SecondStringBearerField":"{
                            \u0022$ref\u0022:\u00222\u0022
                            }"
                            }
                            """.RemoveLineEndings()
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
                            \u0022$id\u0022:\u00222\u0022,
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
                           +
                            "\n"
                           +
                            """
                              "SecondStringBearerField": "{
                            \u0022$ref\u0022:\u00222\u0022
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
    public void TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodeAsStringFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    public static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneField)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                             FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) "{
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
                            TwoStringBearersFirstAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
                              FirstStringBearerField: (BinaryBranchNodeAsField<LeafNode>) "{
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
    public void TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
        TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>>>
                    (TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedString)
                    {
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                          , """
                            TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
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
                             SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) "{
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
                             }"
                             }
                            """.RemoveLineEndings()
                        }
                       ,
                        {
                            new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                          , """
                            TwoStringBearersSecondAsSimpleCloakedStringContent<BinaryBranchNodeAsField<LeafNode>> {
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
                              SecondStringBearerField: (BinaryBranchNodeAsField<LeafNode>) "{
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
                              }"
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
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
}
