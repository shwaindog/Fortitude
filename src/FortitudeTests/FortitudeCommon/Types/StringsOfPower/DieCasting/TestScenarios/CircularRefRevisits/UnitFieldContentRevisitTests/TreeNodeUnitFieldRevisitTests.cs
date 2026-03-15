// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class TreeNodeUnitFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<OrderedBranchNodeAsField<IChildNode>>?      selfReferencingExpect;
    private static InputBearerExpect<OrderedBranchNodeAsField<IChildNode>>?      dualReferencingPairExpect;
    private static InputBearerExpect<BinaryBranchNodeAsField<LeafNode>>?         secondFieldSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsField<LeafNode>>?        allThreeFieldsSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsField<LeafNode>>?        repeatedSequenceSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsField<AlwaysEmptyNode>>? repeatedEmptySequenceSameExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static OrderedBranchNodeAsField<IChildNode> SelfReferencing
    {
        get
        {
            var selfReferencing        = new OrderedBranchNodeAsField<IChildNode>();
            selfReferencing.Parent = selfReferencing;
            return selfReferencing;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsField<IChildNode>> SelfReferencingExpect
    {
        get
        {
            return selfReferencingExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsField<IChildNode>>(SelfReferencing)
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNodeAsField<IChildNode>($id: 1) {
                         BranchInstanceId: 1,
                         Name: "OrderedBranchNodeAsField`1_1",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: -2147483647,
                         ChildNodes: null,
                         Parent: OrderedBranchNodeAsField<IChildNode>($ref: 1)
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         OrderedBranchNodeAsField<IChildNode>($id: 1) {
                           BranchInstanceId: 1,
                           Name: "OrderedBranchNodeAsField`1_1",
                           GlobalNodeInstanceId: 1,
                           NodeType: NodeType.BranchNode,
                           DepthToRoot: -2147483647,
                           ChildNodes: null,
                           Parent: OrderedBranchNodeAsField<IChildNode>($ref: 1)
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "$id":"1",
                        "BranchInstanceId":1,
                        "Name":"OrderedBranchNodeAsField`1_1",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"BranchNode",
                        "DepthToRoot":-2147483647,
                        "ChildNodes":null,
                        "Parent":{
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                       , """ 
                         {
                           "$id": "1",
                           "BranchInstanceId": 1,
                           "Name": "OrderedBranchNodeAsField`1_1",
                           "GlobalNodeInstanceId": 1,
                           "NodeType": "BranchNode",
                           "DepthToRoot": -2147483647,
                           "ChildNodes": null,
                           "Parent": {
                             "$ref": "1"
                           }
                         }
                         """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void SelfReferencingTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, CompactLog);
    }

    [TestMethod]
    public void SelfReferencingTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, CompactJson);
    }

    [TestMethod]
    public void SelfReferencingTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, PrettyLog);
    }

    [TestMethod]
    public void SelfReferencingTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, PrettyJson);
    }
    
    public static OrderedBranchNodeAsField<IChildNode> DualReferencingPair
    {
        get
        {
            var child               = new OrderedBranchNodeAsField<IChildNode>();
            var dualReferencingPair = new OrderedBranchNodeAsField<IChildNode>([child]);
            return dualReferencingPair;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsField<IChildNode>> DualReferencingPairExpect
    {
        get
        {
            return dualReferencingPairExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsField<IChildNode>>(DualReferencingPair)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNodeAsField<IChildNode>($id: 1) {
                         BranchInstanceId: 2,
                         Name: "OrderedBranchNodeAsField`1_2",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<IChildNode>) [
                         (OrderedBranchNodeAsField<IChildNode>) {
                         BranchInstanceId: 1,
                         Name: "OrderedBranchNodeAsField`1_1",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 1,
                         ChildNodes: null,
                         Parent: OrderedBranchNodeAsField<IChildNode>($ref: 1)
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNodeAsField<IChildNode>($id: 1) {
                          BranchInstanceId: 2,
                          Name: "OrderedBranchNodeAsField`1_2",
                          GlobalNodeInstanceId: 2,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<IChildNode>) [
                            (OrderedBranchNodeAsField<IChildNode>) {
                              BranchInstanceId: 1,
                              Name: "OrderedBranchNodeAsField`1_1",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.BranchNode,
                              DepthToRoot: 1,
                              ChildNodes: null,
                              Parent: OrderedBranchNodeAsField<IChildNode>($ref: 1)
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
                        "$id":"1",
                        "BranchInstanceId":2,
                        "Name":"OrderedBranchNodeAsField`1_2",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "ChildNodes":
                        [
                        {
                        "BranchInstanceId":1,
                        "Name":"OrderedBranchNodeAsField`1_1",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"BranchNode",
                        "DepthToRoot":1,
                        "ChildNodes":null,
                        "Parent":
                        {
                        "$ref":"1"
                        }
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
                          "$id": "1",
                          "BranchInstanceId": 2,
                          "Name": "OrderedBranchNodeAsField`1_2",
                          "GlobalNodeInstanceId": 2,
                          "NodeType": "BranchNode",
                          "ChildNodes": [
                            {
                              "BranchInstanceId": 1,
                              "Name": "OrderedBranchNodeAsField`1_1",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "BranchNode",
                              "DepthToRoot": 1,
                              "ChildNodes": null,
                              "Parent": {
                                "$ref": "1"
                              }
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void DualReferencingPairTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DualReferencingPairExpect, CompactLog);
    }

    [TestMethod]
    public void DualReferencingPairTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DualReferencingPairExpect, CompactJson);
    }

    [TestMethod]
    public void DualReferencingPairTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DualReferencingPairExpect, PrettyLog);
    }

    [TestMethod]
    public void DualReferencingPairTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(DualReferencingPairExpect, PrettyJson);
    }
    
    
    public static BinaryBranchNodeAsField<LeafNode> SecondFieldSame
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsField<LeafNode>("SameOnLeftAndRight", child, child );
            return secondFieldSame;
        }
    }

    public static InputBearerExpect<BinaryBranchNodeAsField<LeafNode>> SecondFieldSameExpect
    {
        get
        {
            return secondFieldSameExpect ??=
                new InputBearerExpect<BinaryBranchNodeAsField<LeafNode>>(SecondFieldSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BinaryBranchNodeAsField<LeafNode> {
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode($id: 1) {
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode($ref: 1)
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        BinaryBranchNodeAsField<LeafNode> {
                          Name: "SameOnLeftAndRight",
                          GlobalNodeInstanceId: 2,
                          NodeType: NodeType.BranchNode,
                          Left: LeafNode($id: 1) {
                            LeafInstanceId: 1,
                            Name: "SameChild",
                            GlobalNodeInstanceId: 1,
                            NodeType: NodeType.LeafNode,
                            DepthToRoot: 1
                          },
                          Right: LeafNode($ref: 1)
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "Name":"SameOnLeftAndRight",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "Left":
                        {
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        "Right":
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
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void SecondFieldSameTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SecondFieldSameExpect, CompactLog);
    }

    [TestMethod]
    public void SecondFieldSameTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SecondFieldSameExpect, CompactJson);
    }

    [TestMethod]
    public void SecondFieldSameTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SecondFieldSameExpect, PrettyLog);
    }

    [TestMethod]
    public void SecondFieldSameTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SecondFieldSameExpect, PrettyJson);
    }

    public static OrderedBranchNodeAsField<LeafNode> AllThreeFieldsSame
    {
        get
        {
            var child           = new LeafNode("AllThreeFieldsSame");
            var allThreeFieldsSame = new OrderedBranchNodeAsField<LeafNode>([child, child, child], "AllThreeFieldsSame");
            return allThreeFieldsSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsField<LeafNode>> AllThreeFieldsSameExpect
    {
        get
        {
            return allThreeFieldsSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsField<LeafNode>>(AllThreeFieldsSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNodeAsField<LeafNode> {
                         BranchInstanceId: 1,
                         Name: "AllThreeFieldsSame",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<LeafNode>) [
                         (LeafNode($id: 1)) {
                         LeafInstanceId: 1,
                         Name: "AllThreeFieldsSame",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         (LeafNode($ref: 1)),
                         (LeafNode($ref: 1))
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNodeAsField<LeafNode> {
                          BranchInstanceId: 1,
                          Name: "AllThreeFieldsSame",
                          GlobalNodeInstanceId: 2,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<LeafNode>) [
                            (LeafNode($id: 1)) {
                              LeafInstanceId: 1,
                              Name: "AllThreeFieldsSame",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            (LeafNode($ref: 1)),
                            (LeafNode($ref: 1))
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "BranchInstanceId":1,
                        "Name":"AllThreeFieldsSame",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "ChildNodes":[
                        {
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"AllThreeFieldsSame",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "$ref":"1"
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
                          "BranchInstanceId": 1,
                          "Name": "AllThreeFieldsSame",
                          "GlobalNodeInstanceId": 2,
                          "NodeType": "BranchNode",
                          "ChildNodes": [
                            {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "AllThreeFieldsSame",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            {
                              "$ref": "1"
                            },
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
    public void AllThreeFieldsSameTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(AllThreeFieldsSameExpect, CompactLog);
    }

    [TestMethod]
    public void AllThreeFieldsSameTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(AllThreeFieldsSameExpect, CompactJson);
    }

    [TestMethod]
    public void AllThreeFieldsSameTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(AllThreeFieldsSameExpect, PrettyLog);
    }

    [TestMethod]
    public void AllThreeFieldsSameTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(AllThreeFieldsSameExpect, PrettyJson);
    }
    
    public static OrderedBranchNodeAsField<LeafNode> RepeatedSequenceSame
    {
        get
        {
            var firstChild           = new LeafNode("FirstToRepeat");
            var secondChild           = new LeafNode("SecondToRepeat");
            var thirdChild           = new LeafNode("ThirdToRepeat");
            var repeatedSequenceSame = 
                new OrderedBranchNodeAsField<LeafNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                              , "RepeatedSequenceSame");
            return repeatedSequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsField<LeafNode>> RepeatedSequenceSameExpect
    {
        get
        {
            return repeatedSequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsField<LeafNode>>(RepeatedSequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNodeAsField<LeafNode> {
                         BranchInstanceId: 1,
                         Name: "RepeatedSequenceSame",
                         GlobalNodeInstanceId: 4,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<LeafNode>) [
                         (LeafNode($id: 1)) {
                         LeafInstanceId: 1,
                         Name: "FirstToRepeat",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         (LeafNode($id: 2)) {
                         LeafInstanceId: 2,
                         Name: "SecondToRepeat",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         (LeafNode($id: 3)) {
                         LeafInstanceId: 3,
                         Name: "ThirdToRepeat",
                         GlobalNodeInstanceId: 3,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         (LeafNode($ref: 1)),
                         (LeafNode($ref: 2)),
                         (LeafNode($ref: 3))
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNodeAsField<LeafNode> {
                          BranchInstanceId: 1,
                          Name: "RepeatedSequenceSame",
                          GlobalNodeInstanceId: 4,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<LeafNode>) [
                            (LeafNode($id: 1)) {
                              LeafInstanceId: 1,
                              Name: "FirstToRepeat",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            (LeafNode($id: 2)) {
                              LeafInstanceId: 2,
                              Name: "SecondToRepeat",
                              GlobalNodeInstanceId: 2,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            (LeafNode($id: 3)) {
                              LeafInstanceId: 3,
                              Name: "ThirdToRepeat",
                              GlobalNodeInstanceId: 3,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            (LeafNode($ref: 1)),
                            (LeafNode($ref: 2)),
                            (LeafNode($ref: 3))
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "BranchInstanceId":1,
                        "Name":"RepeatedSequenceSame",
                        "GlobalNodeInstanceId":4,
                        "NodeType":"BranchNode",
                        "ChildNodes":[
                        {
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"FirstToRepeat",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        {
                        "$id":"2",
                        "LeafInstanceId":2,
                        "Name":"SecondToRepeat",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        {
                        "$id":"3",
                        "LeafInstanceId":3,
                        "Name":"ThirdToRepeat",
                        "GlobalNodeInstanceId":3,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "$ref":"2"
                        },
                        {
                        "$ref":"3"
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
                          "BranchInstanceId": 1,
                          "Name": "RepeatedSequenceSame",
                          "GlobalNodeInstanceId": 4,
                          "NodeType": "BranchNode",
                          "ChildNodes": [
                            {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "FirstToRepeat",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            {
                              "$id": "2",
                              "LeafInstanceId": 2,
                              "Name": "SecondToRepeat",
                              "GlobalNodeInstanceId": 2,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            {
                              "$id": "3",
                              "LeafInstanceId": 3,
                              "Name": "ThirdToRepeat",
                              "GlobalNodeInstanceId": 3,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            {
                              "$ref": "1"
                            },
                            {
                              "$ref": "2"
                            },
                            {
                              "$ref": "3"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void RepeatedSequenceSameTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedSequenceSameExpect, CompactLog);
    }

    [TestMethod]
    public void RepeatedSequenceSameTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedSequenceSameExpect, CompactJson);
    }

    [TestMethod]
    public void RepeatedSequenceSameTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedSequenceSameExpect, PrettyLog);
    }

    [TestMethod]
    public void RepeatedSequenceSameTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedSequenceSameExpect, PrettyJson);
    }

    public static OrderedBranchNodeAsField<AlwaysEmptyNode> RepeatedEmptySequenceSame
    {
        get
        {
            var firstChild  = new AlwaysEmptyNode();
            var secondChild = new AlwaysEmptyNode();
            var thirdChild  = new AlwaysEmptyNode();
            var repeatedEmptySequenceSame = 
                new OrderedBranchNodeAsField<AlwaysEmptyNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                                     , "RepeatedEmptySequenceSame");
            return repeatedEmptySequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsField<AlwaysEmptyNode>> RepeatedEmptySequenceSameExpect
    {
        get
        {
            return repeatedEmptySequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsField<AlwaysEmptyNode>>(RepeatedEmptySequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNodeAsField<AlwaysEmptyNode> {
                         BranchInstanceId: 1,
                         Name: "RepeatedEmptySequenceSame",
                         GlobalNodeInstanceId: 4,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<AlwaysEmptyNode>) [
                         (AlwaysEmptyNode($id: 1)) {},
                         (AlwaysEmptyNode($id: 2)) {},
                         (AlwaysEmptyNode($id: 3)) {},
                         (AlwaysEmptyNode($ref: 1)),
                         (AlwaysEmptyNode($ref: 2)),
                         (AlwaysEmptyNode($ref: 3))
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNodeAsField<AlwaysEmptyNode> {
                          BranchInstanceId: 1,
                          Name: "RepeatedEmptySequenceSame",
                          GlobalNodeInstanceId: 4,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<AlwaysEmptyNode>) [
                            (AlwaysEmptyNode($id: 1)) {},
                            (AlwaysEmptyNode($id: 2)) {},
                            (AlwaysEmptyNode($id: 3)) {},
                            (AlwaysEmptyNode($ref: 1)),
                            (AlwaysEmptyNode($ref: 2)),
                            (AlwaysEmptyNode($ref: 3))
                          ]
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "BranchInstanceId":1,
                        "Name":"RepeatedEmptySequenceSame",
                        "GlobalNodeInstanceId":4,
                        "NodeType":"BranchNode",
                        "ChildNodes":[
                        {
                        "$id":"1"
                        },
                        {
                        "$id":"2"
                        },
                        {
                        "$id":"3"
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "$ref":"2"
                        },
                        {
                        "$ref":"3"
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
                          "BranchInstanceId": 1,
                          "Name": "RepeatedEmptySequenceSame",
                          "GlobalNodeInstanceId": 4,
                          "NodeType": "BranchNode",
                          "ChildNodes": [
                            {
                              "$id": "1"
                            },
                            {
                              "$id": "2"
                            },
                            {
                              "$id": "3"
                            },
                            {
                              "$ref": "1"
                            },
                            {
                              "$ref": "2"
                            },
                            {
                              "$ref": "3"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void RepeatedEmptySequenceSameTreeBranchClassCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedEmptySequenceSameExpect, CompactLog);
    }

    [TestMethod]
    public void RepeatedEmptySequenceSameTreeBranchClassCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedEmptySequenceSameExpect, CompactJson);
    }

    [TestMethod]
    public void RepeatedEmptySequenceSameTreeBranchClassPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedEmptySequenceSameExpect, PrettyLog);
    }

    [TestMethod]
    public void RepeatedEmptySequenceSameTreeBranchClassPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(RepeatedEmptySequenceSameExpect, PrettyJson);
    }

}
