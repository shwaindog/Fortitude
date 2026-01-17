// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits;

[NoMatchingProductionClass]
[TestClass]
public class TreeNodeUnitContentFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<OrderedBranchNode<IChildNode>>?      selfReferencingExpect;
    private static InputBearerExpect<OrderedBranchNode<IChildNode>>?      dualReferencingPairExpect;
    private static InputBearerExpect<BinaryBranchNode<LeafNode>>?         secondFieldSameExpect;
    private static InputBearerExpect<OrderedBranchNode<LeafNode>>?        allThreeFieldsSameExpect;
    private static InputBearerExpect<OrderedBranchNode<LeafNode>>?        repeatedSequenceSameExpect;
    private static InputBearerExpect<OrderedBranchNode<AlwaysEmptyNode>>? repeatedEmptySequenceSameExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static OrderedBranchNode<IChildNode> SelfReferencing
    {
        get
        {
            var selfReferencing        = new OrderedBranchNode<IChildNode>();
            selfReferencing.Parent = selfReferencing;
            return selfReferencing;
        }
    }

    public static InputBearerExpect<OrderedBranchNode<IChildNode>> SelfReferencingExpect
    {
        get
        {
            return selfReferencingExpect ??=
                new InputBearerExpect<OrderedBranchNode<IChildNode>>(SelfReferencing)
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNode<IChildNode> 
                        {
                         $id: 1,
                         BranchInstanceId: 1,
                         Name: "OrderedBranchNode`1_1",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: -2147483647,
                         ChildNodes: null,
                         Parent: OrderedBranchNode<IChildNode>
                         {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         OrderedBranchNode<IChildNode> {
                           $id: 1,
                           BranchInstanceId: 1,
                           Name: "OrderedBranchNode`1_1",
                           GlobalNodeInstanceId: 1,
                           NodeType: NodeType.BranchNode,
                           DepthToRoot: -2147483647,
                           ChildNodes: null,
                           Parent: OrderedBranchNode<IChildNode> {
                             $ref: 1
                           }
                         }
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "$id":"1",
                        "BranchInstanceId":1,
                        "Name":"OrderedBranchNode`1_1",
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
                           "Name": "OrderedBranchNode`1_1",
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
    
    public static OrderedBranchNode<IChildNode> DualReferencingPair
    {
        get
        {
            var child               = new OrderedBranchNode<IChildNode>();
            var dualReferencingPair = new OrderedBranchNode<IChildNode>([child]);
            return dualReferencingPair;
        }
    }

    public static InputBearerExpect<OrderedBranchNode<IChildNode>> DualReferencingPairExpect
    {
        get
        {
            return dualReferencingPairExpect ??=
                new InputBearerExpect<OrderedBranchNode<IChildNode>>(DualReferencingPair)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNode<IChildNode>
                         {
                         $id: 1,
                         BranchInstanceId: 2,
                         Name: "OrderedBranchNode`1_2",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNode<IChildNode> {
                         BranchInstanceId: 1,
                         Name: "OrderedBranchNode`1_1",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 1,
                         ChildNodes: null,
                         Parent: OrderedBranchNode<IChildNode> {
                         $ref: 1
                         }
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNode<IChildNode> {
                          $id: 1,
                          BranchInstanceId: 2,
                          Name: "OrderedBranchNode`1_2",
                          GlobalNodeInstanceId: 2,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<IChildNode>) [
                            OrderedBranchNode<IChildNode> {
                              BranchInstanceId: 1,
                              Name: "OrderedBranchNode`1_1",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.BranchNode,
                              DepthToRoot: 1,
                              ChildNodes: null,
                              Parent: OrderedBranchNode<IChildNode> {
                                $ref: 1
                              }
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
                        "Name":"OrderedBranchNode`1_2",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "ChildNodes":
                        [
                        {
                        "BranchInstanceId":1,
                        "Name":"OrderedBranchNode`1_1",
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
                          "Name": "OrderedBranchNode`1_2",
                          "GlobalNodeInstanceId": 2,
                          "NodeType": "BranchNode",
                          "ChildNodes": [
                            {
                              "BranchInstanceId": 1,
                              "Name": "OrderedBranchNode`1_1",
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
    
    
    public static BinaryBranchNode<LeafNode> SecondFieldSame
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child );
            return secondFieldSame;
        }
    }

    public static InputBearerExpect<BinaryBranchNode<LeafNode>> SecondFieldSameExpect
    {
        get
        {
            return secondFieldSameExpect ??=
                new InputBearerExpect<BinaryBranchNode<LeafNode>>(SecondFieldSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BinaryBranchNode<LeafNode>
                         {
                         Name: "SameOnLeftAndRight",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         Left: LeafNode
                         {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         Right: LeafNode
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
                        BinaryBranchNode<LeafNode> {
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

    public static OrderedBranchNode<LeafNode> AllThreeFieldsSame
    {
        get
        {
            var child           = new LeafNode("AllThreeFieldsSame");
            var allThreeFieldsSame = new OrderedBranchNode<LeafNode>([child, child, child], "AllThreeFieldsSame");
            return allThreeFieldsSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNode<LeafNode>> AllThreeFieldsSameExpect
    {
        get
        {
            return allThreeFieldsSameExpect ??=
                new InputBearerExpect<OrderedBranchNode<LeafNode>>(AllThreeFieldsSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNode<LeafNode>
                         {
                         BranchInstanceId: 1,
                         Name: "AllThreeFieldsSame",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<LeafNode>)
                         [
                         LeafNode
                         {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "AllThreeFieldsSame",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         LeafNode
                         {
                         $ref: 1
                         },
                         LeafNode
                         {
                         $ref: 1
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNode<LeafNode> {
                          BranchInstanceId: 1,
                          Name: "AllThreeFieldsSame",
                          GlobalNodeInstanceId: 2,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<LeafNode>) [
                            LeafNode {
                              $id: 1,
                              LeafInstanceId: 1,
                              Name: "AllThreeFieldsSame",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            LeafNode {
                              $ref: 1
                            },
                            LeafNode {
                              $ref: 1
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
    
    public static OrderedBranchNode<LeafNode> RepeatedSequenceSame
    {
        get
        {
            var firstChild           = new LeafNode("FirstToRepeat");
            var secondChild           = new LeafNode("SecondToRepeat");
            var thirdChild           = new LeafNode("ThirdToRepeat");
            var repeatedSequenceSame = 
                new OrderedBranchNode<LeafNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                              , "RepeatedSequenceSame");
            return repeatedSequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNode<LeafNode>> RepeatedSequenceSameExpect
    {
        get
        {
            return repeatedSequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNode<LeafNode>>(RepeatedSequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNode<LeafNode>
                         {
                         BranchInstanceId: 1,
                         Name: "RepeatedSequenceSame",
                         GlobalNodeInstanceId: 4,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<LeafNode>)
                         [
                         LeafNode
                         {
                         $id: 1,
                         LeafInstanceId: 1,
                         Name: "FirstToRepeat",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         LeafNode
                         {
                         $id: 2,
                         LeafInstanceId: 2,
                         Name: "SecondToRepeat",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         LeafNode
                         {
                         $id: 3,
                         LeafInstanceId: 3,
                         Name: "ThirdToRepeat",
                         GlobalNodeInstanceId: 3,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         LeafNode
                         {
                         $ref: 1
                         },
                         LeafNode
                         {
                         $ref: 2
                         },
                         LeafNode
                         {
                         $ref: 3
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNode<LeafNode> {
                          BranchInstanceId: 1,
                          Name: "RepeatedSequenceSame",
                          GlobalNodeInstanceId: 4,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<LeafNode>) [
                            LeafNode {
                              $id: 1,
                              LeafInstanceId: 1,
                              Name: "FirstToRepeat",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            LeafNode {
                              $id: 2,
                              LeafInstanceId: 2,
                              Name: "SecondToRepeat",
                              GlobalNodeInstanceId: 2,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            LeafNode {
                              $id: 3,
                              LeafInstanceId: 3,
                              Name: "ThirdToRepeat",
                              GlobalNodeInstanceId: 3,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            LeafNode {
                              $ref: 1
                            },
                            LeafNode {
                              $ref: 2
                            },
                            LeafNode {
                              $ref: 3
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
                        "BranchInstanceId":1,
                        "Name":"RepeatedSequenceSame",
                        "GlobalNodeInstanceId":4,
                        "NodeType":"BranchNode",
                        "ChildNodes":
                        [
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

    public static OrderedBranchNode<AlwaysEmptyNode> RepeatedEmptySequenceSame
    {
        get
        {
            var firstChild  = new AlwaysEmptyNode();
            var secondChild = new AlwaysEmptyNode();
            var thirdChild  = new AlwaysEmptyNode();
            var repeatedEmptySequenceSame = 
                new OrderedBranchNode<AlwaysEmptyNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                                     , "RepeatedEmptySequenceSame");
            return repeatedEmptySequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNode<AlwaysEmptyNode>> RepeatedEmptySequenceSameExpect
    {
        get
        {
            return repeatedEmptySequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNode<AlwaysEmptyNode>>(RepeatedEmptySequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedBranchNode<AlwaysEmptyNode>
                         {
                         BranchInstanceId: 1,
                         Name: "RepeatedEmptySequenceSame",
                         GlobalNodeInstanceId: 4,
                         NodeType: NodeType.BranchNode,
                         ChildNodes: (List<AlwaysEmptyNode>)
                         [
                         AlwaysEmptyNode
                         {
                         $id: 1
                         },
                         AlwaysEmptyNode
                         {
                         $id: 2
                         },
                         AlwaysEmptyNode
                         {
                         $id: 3
                         },
                         AlwaysEmptyNode
                         {
                         $ref: 1
                         },
                         AlwaysEmptyNode
                         {
                         $ref: 2
                         },
                         AlwaysEmptyNode
                         {
                         $ref: 3
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedBranchNode<AlwaysEmptyNode> {
                          BranchInstanceId: 1,
                          Name: "RepeatedEmptySequenceSame",
                          GlobalNodeInstanceId: 4,
                          NodeType: NodeType.BranchNode,
                          ChildNodes: (List<AlwaysEmptyNode>) [
                            AlwaysEmptyNode {
                              $id: 1
                            },
                            AlwaysEmptyNode {
                              $id: 2
                            },
                            AlwaysEmptyNode {
                              $id: 3
                            },
                            AlwaysEmptyNode {
                              $ref: 1
                            },
                            AlwaysEmptyNode {
                              $ref: 2
                            },
                            AlwaysEmptyNode {
                              $ref: 3
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
                        "BranchInstanceId":1,
                        "Name":"RepeatedEmptySequenceSame",
                        "GlobalNodeInstanceId":4,
                        "NodeType":"BranchNode",
                        "ChildNodes":
                        [
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
