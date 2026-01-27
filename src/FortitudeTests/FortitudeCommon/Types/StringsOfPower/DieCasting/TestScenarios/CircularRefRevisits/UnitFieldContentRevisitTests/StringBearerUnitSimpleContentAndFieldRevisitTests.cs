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
    private static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
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
                         FirstStringBearerField: BinaryBranchNode<LeafNode>= {
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
                          FirstStringBearerField: BinaryBranchNode<LeafNode>= {
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

    public static TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress> TwoSameIpAddressesOneFieldOneSimpleCloakedValue
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneFieldsOneCloaked
                = new TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField: 127.0.0.1,
                         SecondSpanFormattableField: 127.0.0.1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: 127.0.0.1,
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
    public void TwoSameIpAddressesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
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
    
    
    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref:
                         1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
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
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }
    
    
    public static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: "127.0.0.1"
                         },
                         SecondSpanFormattableField:
                         {
                         $ref:
                         1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: "127.0.0.1"
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":
                        "1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }
    
    
    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":
                        "1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }
    
    
    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         $values: 127.0.0.1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            $values: 127.0.0.1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "$values":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "$values": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }
    
    
    public static InputBearerExpect<TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: "127.0.0.1"
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         $values: 127.0.0.1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: "127.0.0.1"
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            $values: 127.0.0.1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "$values":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "$values": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }
    
    
    public static InputBearerExpect<TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         $values: "127.0.0.1"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsSimpleCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            $values: "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "$values":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "$values": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }


}
