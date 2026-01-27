// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class StringBearerUnitSimpleContentAndFieldRevisitTests  : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBearersFields<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersFirstAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>?
        twoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect;

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
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child );
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
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child );
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
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameOneBranchTwoSameLeafNodesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>> TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValue
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child );
            var twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked
                = new TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>(secondFieldSame, secondFieldSame);
            return twoSameOneBranchTwoSameLeafNodesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBearersSecondAsSimpleCloakedValueContent<BinaryBranchNode<LeafNode>>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
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
                         SecondStringBearerField: (BinaryBranchNode<LeafNode>($ref: 2))
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
                          SecondStringBearerField: (BinaryBranchNode<LeafNode>($ref: 2))
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
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameOneBranchTwoSameLeafNodesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress> TwoSameIpAddressesOneSimpleCloakedStringOneField
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneCloakedOneFields =
                new TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField: "127.0.0.1",
                         SecondSpanFormattableField: 127.0.0.1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: "127.0.0.1",
                          SecondSpanFormattableField: 127.0.0.1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress> TwoSameIpAddressesOneFieldOneSimpleCloakedString
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneFieldsOneCloakedString
                = new TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField: 127.0.0.1,
                         SecondSpanFormattableField: "127.0.0.1"
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: 127.0.0.1,
                          SecondSpanFormattableField: "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
    
}
