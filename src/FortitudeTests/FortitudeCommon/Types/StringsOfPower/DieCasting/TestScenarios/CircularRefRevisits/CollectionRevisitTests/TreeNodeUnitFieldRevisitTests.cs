// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.SimpleCollection;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.CollectionRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class TreeNodeCollectionFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<OrderedParentNodeAsSimpleCollection<IChildNode>>?       selfReferencingExpect;
    private static InputBearerExpect<OrderedBranchNodeAsComplexCollection<IChildNode>>?      dualReferencingPairExpect;
    private static InputBearerExpect<BinaryBranchNodeAsComplexCollection<LeafNode>>?         secondFieldSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>>?        allThreeFieldsSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>>?        repeatedSequenceSameExpect;
    private static InputBearerExpect<OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>>? repeatedEmptySequenceSameExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static OrderedParentNodeAsSimpleCollection<IChildNode> SelfReferencing
    {
        get
        {
            var selfReferencing        = new OrderedParentNodeAsSimpleCollection<IChildNode>();
            selfReferencing.ChildNodes!.Add(selfReferencing);
            return selfReferencing;
        }
    }

    public static InputBearerExpect<OrderedParentNodeAsSimpleCollection<IChildNode>> SelfReferencingExpect
    {
        get
        {
            return selfReferencingExpect ??=
                new InputBearerExpect<OrderedParentNodeAsSimpleCollection<IChildNode>>(SelfReferencing)
                {
                    { new EK( AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        (OrderedParentNodeAsSimpleCollection<IChildNode>($id: 1)) (List<IChildNode>) [
                         (OrderedParentNodeAsSimpleCollection<IChildNode>) { $ref: 1 }
                         ]
                        """.RemoveLineEndings()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                       , """
                         (OrderedParentNodeAsSimpleCollection<IChildNode>($id: 1)) (List<IChildNode>) [
                           (OrderedParentNodeAsSimpleCollection<IChildNode>) {
                             $ref: 1
                           }
                         ]
                         """.Dos2Unix()
                    }
                   , { new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "$id":"1",
                        "$values":[
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
                           "$id": "1",
                           "$values": [
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
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, CompactLog);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, CompactJson);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, PrettyLog);
    }

    [TestMethod]
    public void SelfReferencingParentNodeAsSimpleCollectionPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(SelfReferencingExpect, PrettyJson);
    }
    
    public static OrderedBranchNodeAsComplexCollection<IChildNode> DualReferencingPair
    {
        get
        {
            var childLeaf           = new LeafNode("DuelReferenceLeaf");
            var child               = new OrderedBranchNodeAsComplexCollection<IChildNode>([childLeaf]);
            var dualReferencingPair = new OrderedBranchNodeAsComplexCollection<IChildNode>([child]);
            return dualReferencingPair;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsComplexCollection<IChildNode>> DualReferencingPairExpect
    {
        get
        {
            return dualReferencingPairExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsComplexCollection<IChildNode>>(DualReferencingPair)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<IChildNode>) {
                         $id: 1,
                         (List<IChildNode>) $values: [
                         (OrderedBranchNodeAsComplexCollection<IChildNode>) {
                         (List<IChildNode>) $values: [
                         LeafNode {
                         LeafInstanceId: 1,
                         Name: "DuelReferenceLeaf",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         }
                         ],
                         BranchInstanceId: 1,
                         Parent: (OrderedBranchNodeAsComplexCollection<IChildNode>) { $ref: 1 }
                         }
                         ],
                         BranchInstanceId: 2
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<IChildNode>) {
                          $id: 1,
                          (List<IChildNode>) $values: [
                            (OrderedBranchNodeAsComplexCollection<IChildNode>) {
                              (List<IChildNode>) $values: [
                                LeafNode {
                                  LeafInstanceId: 1,
                                  Name: "DuelReferenceLeaf",
                                  GlobalNodeInstanceId: 1,
                                  NodeType: NodeType.LeafNode,
                                  DepthToRoot: 2
                                }
                              ],
                              BranchInstanceId: 1,
                              Parent: (OrderedBranchNodeAsComplexCollection<IChildNode>) {
                                $ref: 1
                              }
                            }
                          ],
                          BranchInstanceId: 2
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        [
                        [
                        {
                        "LeafInstanceId":1,
                        "Name":"DuelReferenceLeaf",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        }
                        ]
                        ]
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        [
                          [
                            {
                              "LeafInstanceId": 1,
                              "Name": "DuelReferenceLeaf",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 2
                            }
                          ]
                        ]
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
    
    
    public static BinaryBranchNodeAsComplexCollection<LeafNode> SecondFieldSame
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNodeAsComplexCollection<LeafNode>("SameOnLeftAndRight", child, child );
            return secondFieldSame;
        }
    }

    public static InputBearerExpect<BinaryBranchNodeAsComplexCollection<LeafNode>> SecondFieldSameExpect
    {
        get
        {
            return secondFieldSameExpect ??=
                new InputBearerExpect<BinaryBranchNodeAsComplexCollection<LeafNode>>(SecondFieldSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        BinaryBranchNodeAsComplexCollection<LeafNode> {
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
                        BinaryBranchNodeAsComplexCollection<LeafNode> {
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

    public static OrderedBranchNodeAsComplexCollection<LeafNode> AllThreeFieldsSame
    {
        get
        {
            var child           = new LeafNode("AllThreeFieldsSame");
            var allThreeFieldsSame = new OrderedBranchNodeAsComplexCollection<LeafNode>([child, child, child], "AllThreeFieldsSame");
            return allThreeFieldsSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>> AllThreeFieldsSameExpect
    {
        get
        {
            return allThreeFieldsSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>>(AllThreeFieldsSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<LeafNode>) {
                         (List<LeafNode>) $values: [
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
                         ],
                         BranchInstanceId: 1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<LeafNode>) {
                          (List<LeafNode>) $values: [
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
                          ],
                          BranchInstanceId: 1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        [
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
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        [
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
    
    public static OrderedBranchNodeAsComplexCollection<LeafNode> RepeatedSequenceSame
    {
        get
        {
            var firstChild           = new LeafNode("FirstToRepeat");
            var secondChild           = new LeafNode("SecondToRepeat");
            var thirdChild           = new LeafNode("ThirdToRepeat");
            var repeatedSequenceSame = 
                new OrderedBranchNodeAsComplexCollection<LeafNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                              , "RepeatedSequenceSame");
            return repeatedSequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>> RepeatedSequenceSameExpect
    {
        get
        {
            return repeatedSequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsComplexCollection<LeafNode>>(RepeatedSequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<LeafNode>) {
                         (List<LeafNode>) $values: [
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
                         ],
                         BranchInstanceId: 1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<LeafNode>) {
                          (List<LeafNode>) $values: [
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
                          ],
                          BranchInstanceId: 1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
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
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        [
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

    public static OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode> RepeatedEmptySequenceSame
    {
        get
        {
            var firstChild  = new AlwaysEmptyNode();
            var secondChild = new AlwaysEmptyNode();
            var thirdChild  = new AlwaysEmptyNode();
            var repeatedEmptySequenceSame = 
                new OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>([firstChild, secondChild, thirdChild, firstChild, secondChild, thirdChild]
                                                     , "RepeatedEmptySequenceSame");
            return repeatedEmptySequenceSame;
        }
    }

    public static InputBearerExpect<OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>> RepeatedEmptySequenceSameExpect
    {
        get
        {
            return repeatedEmptySequenceSameExpect ??=
                new InputBearerExpect<OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>>(RepeatedEmptySequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>) {
                         (List<AlwaysEmptyNode>) $values: [
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
                         ],
                         BranchInstanceId: 1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        (OrderedBranchNodeAsComplexCollection<AlwaysEmptyNode>) {
                          (List<AlwaysEmptyNode>) $values: [
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
                          ],
                          BranchInstanceId: 1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
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
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        [
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
