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
public class MultiRevisitFieldsCombinationTest : CommonStyleExpectationTestBase
{
    private OrderedBranchNodeAsField<IChildNode> firstMatchNode   = null!;
    private OrderedBranchNodeAsField<IChildNode> secondMatchNode  = null!;
    private OrderedBranchNodeAsField<IChildNode> thirdMatchNode   = null!;
    private OrderedBranchNodeAsField<IChildNode> fourthMatchNode  = null!;
    private OrderedBranchNodeAsField<IChildNode> fifthMatchNode   = null!;
    private OrderedBranchNodeAsField<IChildNode> sixthMatchNode   = null!;
    private OrderedBranchNodeAsField<IChildNode> seventhMatchNode = null!;

    private OrderedRootNodeAsField? multiRevisitStringBearer;

    private static InputBearerExpect<OrderedRootNodeAsField>? multiRevisitExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }


    private OrderedRootNodeAsField WireUpTree()
    {
        firstMatchNode =
            new([new LeafNode("First Match Leaf")]
              , "First Match Node"
              , showParent: false);
        fifthMatchNode =
            new([new LeafNode("Fifth Match Leaf")]
              , "Fifth Match Node"
              , showParent: false);
        var linkNodeBranch =
            new OrderedBranchNodeAsField<IChildNode>
                ([
                     new LeafNode("1st Branch Link 3->1 Leaf")
                   , firstMatchNode
                   , fifthMatchNode
                 ]
               , "1st BranchLink 3->1 Node"
               , showParent: false);
        thirdMatchNode =
            new(
                [
                    linkNodeBranch
                  , new LeafNode("Third Match Leaf")
                ]
              , "Third Match Node"
              , showParent: false);


        seventhMatchNode =
            new(
                [new LeafNode("Seventh Match Leaf")]
              , "Seventh Match Node", showParent: false);
        sixthMatchNode =
            new(
                [new LeafNode("Sixth Match Leaf")]
              , "Sixth Match Node", showParent: false);
        linkNodeBranch =
            new OrderedBranchNodeAsField<IChildNode>
                ([
                     thirdMatchNode
                   , new LeafNode("1st Branch Link 2->3 Leaf")
                   , seventhMatchNode
                 ]
               , "1st Branch Link 2->3 Node"
               , showParent: false);

        secondMatchNode =
            new(
                [
                    new LeafNode("Second Match Leaf")
                  , linkNodeBranch
                ], "Second Match Node"
              , showParent: false);

        var firstBranchRootChild =
            new OrderedBranchNodeAsField<IChildNode>
                ([
                     new LeafNode("1st Branch Root 1st Leaf")
                   , secondMatchNode
                   , new LeafNode("1st Branch Root 2nd Leaf")
                 ]
               , "1st Branch Root Node"
               , showParent: false);


        linkNodeBranch = new OrderedBranchNodeAsField<IChildNode>
            ([new LeafNode("2nd Branch 2->3 Link Leaf"), thirdMatchNode]
           , showParent: false);

        fourthMatchNode =
            new(
                [new LeafNode("Fourth Match Leaf")]
              , "Fourth Match Node"
              , showParent: false);

        linkNodeBranch = new OrderedBranchNodeAsField<IChildNode>
            ([
                 fourthMatchNode
               , new LeafNode("2nd Branch 1->2 Link Step 3-3 1st Leaf")
               , secondMatchNode
               , new LeafNode("2nd Branch 1->2 Link Step 3-3 2nd Leaf")
               , linkNodeBranch
             ]
           , "2nd Branch 1->2 Link Step 3-3 Node"
           , showParent: false);

        var doubleLink = new OrderedBranchNodeAsField<IChildNode>
            ([linkNodeBranch, new LeafNode("2nd Branch 1->2 Link Step 2-3 Leaf")]
           , showParent: false);

        linkNodeBranch = new OrderedBranchNodeAsField<IChildNode>
            ([new LeafNode("2nd Branch 1->2 Link Step 1-3 Leaf"), doubleLink]
           , showParent: false);

        var secondBranchRootChild =
            new OrderedBranchNodeAsField<IChildNode>
                ([new LeafNode("2nd Branch Root Leaf"), firstMatchNode, linkNodeBranch]
               , showParent: false);

        linkNodeBranch = new OrderedBranchNodeAsField<IChildNode>
            ([new LeafNode("3rd Branch Link -> 4th Leaf"), fourthMatchNode]
           , showParent: false);

        var thirdBranchRoot =
            new OrderedBranchNodeAsField<IChildNode>
                ([new LeafNode("3rd Branch Root Leaf"), linkNodeBranch]
               , "3rd Branch Root Node"
               , showParent: false);

        var fourthBranchRoot =
            new OrderedBranchNodeAsField<IChildNode>
                ([fifthMatchNode, new LeafNode("4th Branch Root Leaf")], "4th Branch Root Node"
               , showParent: false);


        sixthMatchNode = new(
                             [
                                 firstBranchRootChild
                               , new LeafNode("Sixth Match And Branches Root \"1st\" Leaf")
                               , secondBranchRootChild
                               , new LeafNode("Sixth Match And Branches Root \"2nd\" Leaf")
                               , thirdBranchRoot
                               , fourthBranchRoot
                               , new LeafNode("Sixth Match And Branches Root \"3rd\" Leaf")
                             ], "Sixth Match And Branches Root Node"
                           , showParent: false);

        linkNodeBranch = new OrderedBranchNodeAsField<IChildNode>
            ([new LeafNode("Branches Link -> 4th Leaf"), sixthMatchNode]
           , showParent: false);

        var rootNode =
            new OrderedRootNodeAsField
                ([linkNodeBranch, new LeafNode("Branches Link Leaf"), sixthMatchNode, seventhMatchNode]
               , "Branches Link Node");

        return rootNode;
    }


    public InputBearerExpect<OrderedRootNodeAsField> MultiReferencingExpect
    {
        get
        {
            multiRevisitStringBearer ??= WireUpTree();
            return multiRevisitExpect ??=
                new InputBearerExpect<OrderedRootNodeAsField>(multiRevisitStringBearer)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        OrderedRootNodeAsField {
                         RootInstanceId: 1,
                         Name: "Branches Link Node",
                         GlobalNodeInstanceId: 46,
                         NodeType: NodeType.RootNode,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 20,
                         Name: "OrderedBranchNodeAsField`1_44",
                         GlobalNodeInstanceId: 44,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 1,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 24,
                         Name: "Branches Link -> 4th Leaf",
                         GlobalNodeInstanceId: 43,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 6,
                         BranchInstanceId: 19,
                         Name: "Sixth Match And Branches Root Node",
                         GlobalNodeInstanceId: 42,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 1,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 9,
                         Name: "1st Branch Root Node",
                         GlobalNodeInstanceId: 19,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 2,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 9,
                         Name: "1st Branch Root 1st Leaf",
                         GlobalNodeInstanceId: 17,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 3
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 2,
                         BranchInstanceId: 8,
                         Name: "Second Match Node",
                         GlobalNodeInstanceId: 16,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 6,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 8,
                         Name: "Second Match Leaf",
                         GlobalNodeInstanceId: 15,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 7
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 7,
                         Name: "1st Branch Link 2->3 Node",
                         GlobalNodeInstanceId: 14,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 7,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 3,
                         BranchInstanceId: 4,
                         Name: "Third Match Node",
                         GlobalNodeInstanceId: 8,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 7,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 3,
                         Name: "1st BranchLink 3->1 Node",
                         GlobalNodeInstanceId: 6,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 8,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 3,
                         Name: "1st Branch Link 3->1 Leaf",
                         GlobalNodeInstanceId: 5,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 9
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 1,
                         BranchInstanceId: 1,
                         Name: "First Match Node",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 3,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 1,
                         Name: "First Match Leaf",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 4
                         }
                         ]
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 5,
                         BranchInstanceId: 2,
                         Name: "Fifth Match Node",
                         GlobalNodeInstanceId: 4,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 3,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 2,
                         Name: "Fifth Match Leaf",
                         GlobalNodeInstanceId: 3,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 4
                         }
                         ]
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 4,
                         Name: "Third Match Leaf",
                         GlobalNodeInstanceId: 7,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 8
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 7,
                         Name: "1st Branch Link 2->3 Leaf",
                         GlobalNodeInstanceId: 13,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 8
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 7,
                         BranchInstanceId: 5,
                         Name: "Seventh Match Node",
                         GlobalNodeInstanceId: 10,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 1,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 5,
                         Name: "Seventh Match Leaf",
                         GlobalNodeInstanceId: 9,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         }
                         ]
                         }
                         ]
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 10,
                         Name: "1st Branch Root 2nd Leaf",
                         GlobalNodeInstanceId: 18,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 3
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 21,
                         Name: "Sixth Match And Branches Root "1st" Leaf",
                         GlobalNodeInstanceId: 39,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 15,
                         Name: "OrderedBranchNodeAsField`1_32",
                         GlobalNodeInstanceId: 32,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 2,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 17,
                         Name: "2nd Branch Root Leaf",
                         GlobalNodeInstanceId: 31,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 3
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 1
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 14,
                         Name: "OrderedBranchNodeAsField`1_30",
                         GlobalNodeInstanceId: 30,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 3,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 16,
                         Name: "2nd Branch 1->2 Link Step 1-3 Leaf",
                         GlobalNodeInstanceId: 29,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 4
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 13,
                         Name: "OrderedBranchNodeAsField`1_28",
                         GlobalNodeInstanceId: 28,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 4,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 12,
                         Name: "2nd Branch 1->2 Link Step 3-3 Node",
                         GlobalNodeInstanceId: 26,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 5,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         $id: 4,
                         BranchInstanceId: 11,
                         Name: "Fourth Match Node",
                         GlobalNodeInstanceId: 23,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 4,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 12,
                         Name: "Fourth Match Leaf",
                         GlobalNodeInstanceId: 22,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 5
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 13,
                         Name: "2nd Branch 1->2 Link Step 3-3 1st Leaf",
                         GlobalNodeInstanceId: 24,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 6
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 2
                         },
                         LeafNode {
                         LeafInstanceId: 14,
                         Name: "2nd Branch 1->2 Link Step 3-3 2nd Leaf",
                         GlobalNodeInstanceId: 25,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 6
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 10,
                         Name: "OrderedBranchNodeAsField`1_21",
                         GlobalNodeInstanceId: 21,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 6,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 11,
                         Name: "2nd Branch 2->3 Link Leaf",
                         GlobalNodeInstanceId: 20,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 7
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 3
                         }
                         ]
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 15,
                         Name: "2nd Branch 1->2 Link Step 2-3 Leaf",
                         GlobalNodeInstanceId: 27,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 5
                         }
                         ]
                         }
                         ]
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 22,
                         Name: "Sixth Match And Branches Root "2nd" Leaf",
                         GlobalNodeInstanceId: 40,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 17,
                         Name: "3rd Branch Root Node",
                         GlobalNodeInstanceId: 36,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 2,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 19,
                         Name: "3rd Branch Root Leaf",
                         GlobalNodeInstanceId: 35,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 3
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 16,
                         Name: "OrderedBranchNodeAsField`1_34",
                         GlobalNodeInstanceId: 34,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 3,
                         ChildNodes: (List<IChildNode>) [
                         LeafNode {
                         LeafInstanceId: 18,
                         Name: "3rd Branch Link -> 4th Leaf",
                         GlobalNodeInstanceId: 33,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 4
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 4
                         }
                         ]
                         }
                         ]
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         BranchInstanceId: 18,
                         Name: "4th Branch Root Node",
                         GlobalNodeInstanceId: 38,
                         NodeType: NodeType.BranchNode,
                         DepthToRoot: 2,
                         ChildNodes: (List<IChildNode>) [
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 5
                         },
                         LeafNode {
                         LeafInstanceId: 20,
                         Name: "4th Branch Root Leaf",
                         GlobalNodeInstanceId: 37,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 3
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 23,
                         Name: "Sixth Match And Branches Root "3rd" Leaf",
                         GlobalNodeInstanceId: 41,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 2
                         }
                         ]
                         }
                         ]
                         },
                         LeafNode {
                         LeafInstanceId: 25,
                         Name: "Branches Link Leaf",
                         GlobalNodeInstanceId: 45,
                         NodeType: NodeType.LeafNode,
                         DepthToRoot: 1
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 6
                         },
                         OrderedBranchNodeAsField<IChildNode> {
                         $ref: 7
                         }
                         ]
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        OrderedRootNodeAsField {
                          RootInstanceId: 1,
                          Name: "Branches Link Node",
                          GlobalNodeInstanceId: 46,
                          NodeType: NodeType.RootNode,
                          ChildNodes: (List<IChildNode>) [
                            OrderedBranchNodeAsField<IChildNode> {
                              BranchInstanceId: 20,
                              Name: "OrderedBranchNodeAsField`1_44",
                              GlobalNodeInstanceId: 44,
                              NodeType: NodeType.BranchNode,
                              DepthToRoot: 1,
                              ChildNodes: (List<IChildNode>) [
                                LeafNode {
                                  LeafInstanceId: 24,
                                  Name: "Branches Link -> 4th Leaf",
                                  GlobalNodeInstanceId: 43,
                                  NodeType: NodeType.LeafNode,
                                  DepthToRoot: 2
                                },
                                OrderedBranchNodeAsField<IChildNode> {
                                  $id: 6,
                                  BranchInstanceId: 19,
                                  Name: "Sixth Match And Branches Root Node",
                                  GlobalNodeInstanceId: 42,
                                  NodeType: NodeType.BranchNode,
                                  DepthToRoot: 1,
                                  ChildNodes: (List<IChildNode>) [
                                    OrderedBranchNodeAsField<IChildNode> {
                                      BranchInstanceId: 9,
                                      Name: "1st Branch Root Node",
                                      GlobalNodeInstanceId: 19,
                                      NodeType: NodeType.BranchNode,
                                      DepthToRoot: 2,
                                      ChildNodes: (List<IChildNode>) [
                                        LeafNode {
                                          LeafInstanceId: 9,
                                          Name: "1st Branch Root 1st Leaf",
                                          GlobalNodeInstanceId: 17,
                                          NodeType: NodeType.LeafNode,
                                          DepthToRoot: 3
                                        },
                                        OrderedBranchNodeAsField<IChildNode> {
                                          $id: 2,
                                          BranchInstanceId: 8,
                                          Name: "Second Match Node",
                                          GlobalNodeInstanceId: 16,
                                          NodeType: NodeType.BranchNode,
                                          DepthToRoot: 6,
                                          ChildNodes: (List<IChildNode>) [
                                            LeafNode {
                                              LeafInstanceId: 8,
                                              Name: "Second Match Leaf",
                                              GlobalNodeInstanceId: 15,
                                              NodeType: NodeType.LeafNode,
                                              DepthToRoot: 7
                                            },
                                            OrderedBranchNodeAsField<IChildNode> {
                                              BranchInstanceId: 7,
                                              Name: "1st Branch Link 2->3 Node",
                                              GlobalNodeInstanceId: 14,
                                              NodeType: NodeType.BranchNode,
                                              DepthToRoot: 7,
                                              ChildNodes: (List<IChildNode>) [
                                                OrderedBranchNodeAsField<IChildNode> {
                                                  $id: 3,
                                                  BranchInstanceId: 4,
                                                  Name: "Third Match Node",
                                                  GlobalNodeInstanceId: 8,
                                                  NodeType: NodeType.BranchNode,
                                                  DepthToRoot: 7,
                                                  ChildNodes: (List<IChildNode>) [
                                                    OrderedBranchNodeAsField<IChildNode> {
                                                      BranchInstanceId: 3,
                                                      Name: "1st BranchLink 3->1 Node",
                                                      GlobalNodeInstanceId: 6,
                                                      NodeType: NodeType.BranchNode,
                                                      DepthToRoot: 8,
                                                      ChildNodes: (List<IChildNode>) [
                                                        LeafNode {
                                                          LeafInstanceId: 3,
                                                          Name: "1st Branch Link 3->1 Leaf",
                                                          GlobalNodeInstanceId: 5,
                                                          NodeType: NodeType.LeafNode,
                                                          DepthToRoot: 9
                                                        },
                                                        OrderedBranchNodeAsField<IChildNode> {
                                                          $id: 1,
                                                          BranchInstanceId: 1,
                                                          Name: "First Match Node",
                                                          GlobalNodeInstanceId: 2,
                                                          NodeType: NodeType.BranchNode,
                                                          DepthToRoot: 3,
                                                          ChildNodes: (List<IChildNode>) [
                                                            LeafNode {
                                                              LeafInstanceId: 1,
                                                              Name: "First Match Leaf",
                                                              GlobalNodeInstanceId: 1,
                                                              NodeType: NodeType.LeafNode,
                                                              DepthToRoot: 4
                                                            }
                                                          ]
                                                        },
                                                        OrderedBranchNodeAsField<IChildNode> {
                                                          $id: 5,
                                                          BranchInstanceId: 2,
                                                          Name: "Fifth Match Node",
                                                          GlobalNodeInstanceId: 4,
                                                          NodeType: NodeType.BranchNode,
                                                          DepthToRoot: 3,
                                                          ChildNodes: (List<IChildNode>) [
                                                            LeafNode {
                                                              LeafInstanceId: 2,
                                                              Name: "Fifth Match Leaf",
                                                              GlobalNodeInstanceId: 3,
                                                              NodeType: NodeType.LeafNode,
                                                              DepthToRoot: 4
                                                            }
                                                          ]
                                                        }
                                                      ]
                                                    },
                                                    LeafNode {
                                                      LeafInstanceId: 4,
                                                      Name: "Third Match Leaf",
                                                      GlobalNodeInstanceId: 7,
                                                      NodeType: NodeType.LeafNode,
                                                      DepthToRoot: 8
                                                    }
                                                  ]
                                                },
                                                LeafNode {
                                                  LeafInstanceId: 7,
                                                  Name: "1st Branch Link 2->3 Leaf",
                                                  GlobalNodeInstanceId: 13,
                                                  NodeType: NodeType.LeafNode,
                                                  DepthToRoot: 8
                                                },
                                                OrderedBranchNodeAsField<IChildNode> {
                                                  $id: 7,
                                                  BranchInstanceId: 5,
                                                  Name: "Seventh Match Node",
                                                  GlobalNodeInstanceId: 10,
                                                  NodeType: NodeType.BranchNode,
                                                  DepthToRoot: 1,
                                                  ChildNodes: (List<IChildNode>) [
                                                    LeafNode {
                                                      LeafInstanceId: 5,
                                                      Name: "Seventh Match Leaf",
                                                      GlobalNodeInstanceId: 9,
                                                      NodeType: NodeType.LeafNode,
                                                      DepthToRoot: 2
                                                    }
                                                  ]
                                                }
                                              ]
                                            }
                                          ]
                                        },
                                        LeafNode {
                                          LeafInstanceId: 10,
                                          Name: "1st Branch Root 2nd Leaf",
                                          GlobalNodeInstanceId: 18,
                                          NodeType: NodeType.LeafNode,
                                          DepthToRoot: 3
                                        }
                                      ]
                                    },
                                    LeafNode {
                                      LeafInstanceId: 21,
                                      Name: "Sixth Match And Branches Root "1st" Leaf",
                                      GlobalNodeInstanceId: 39,
                                      NodeType: NodeType.LeafNode,
                                      DepthToRoot: 2
                                    },
                                    OrderedBranchNodeAsField<IChildNode> {
                                      BranchInstanceId: 15,
                                      Name: "OrderedBranchNodeAsField`1_32",
                                      GlobalNodeInstanceId: 32,
                                      NodeType: NodeType.BranchNode,
                                      DepthToRoot: 2,
                                      ChildNodes: (List<IChildNode>) [
                                        LeafNode {
                                          LeafInstanceId: 17,
                                          Name: "2nd Branch Root Leaf",
                                          GlobalNodeInstanceId: 31,
                                          NodeType: NodeType.LeafNode,
                                          DepthToRoot: 3
                                        },
                                        OrderedBranchNodeAsField<IChildNode> {
                                          $ref: 1
                                        },
                                        OrderedBranchNodeAsField<IChildNode> {
                                          BranchInstanceId: 14,
                                          Name: "OrderedBranchNodeAsField`1_30",
                                          GlobalNodeInstanceId: 30,
                                          NodeType: NodeType.BranchNode,
                                          DepthToRoot: 3,
                                          ChildNodes: (List<IChildNode>) [
                                            LeafNode {
                                              LeafInstanceId: 16,
                                              Name: "2nd Branch 1->2 Link Step 1-3 Leaf",
                                              GlobalNodeInstanceId: 29,
                                              NodeType: NodeType.LeafNode,
                                              DepthToRoot: 4
                                            },
                                            OrderedBranchNodeAsField<IChildNode> {
                                              BranchInstanceId: 13,
                                              Name: "OrderedBranchNodeAsField`1_28",
                                              GlobalNodeInstanceId: 28,
                                              NodeType: NodeType.BranchNode,
                                              DepthToRoot: 4,
                                              ChildNodes: (List<IChildNode>) [
                                                OrderedBranchNodeAsField<IChildNode> {
                                                  BranchInstanceId: 12,
                                                  Name: "2nd Branch 1->2 Link Step 3-3 Node",
                                                  GlobalNodeInstanceId: 26,
                                                  NodeType: NodeType.BranchNode,
                                                  DepthToRoot: 5,
                                                  ChildNodes: (List<IChildNode>) [
                                                    OrderedBranchNodeAsField<IChildNode> {
                                                      $id: 4,
                                                      BranchInstanceId: 11,
                                                      Name: "Fourth Match Node",
                                                      GlobalNodeInstanceId: 23,
                                                      NodeType: NodeType.BranchNode,
                                                      DepthToRoot: 4,
                                                      ChildNodes: (List<IChildNode>) [
                                                        LeafNode {
                                                          LeafInstanceId: 12,
                                                          Name: "Fourth Match Leaf",
                                                          GlobalNodeInstanceId: 22,
                                                          NodeType: NodeType.LeafNode,
                                                          DepthToRoot: 5
                                                        }
                                                      ]
                                                    },
                                                    LeafNode {
                                                      LeafInstanceId: 13,
                                                      Name: "2nd Branch 1->2 Link Step 3-3 1st Leaf",
                                                      GlobalNodeInstanceId: 24,
                                                      NodeType: NodeType.LeafNode,
                                                      DepthToRoot: 6
                                                    },
                                                    OrderedBranchNodeAsField<IChildNode> {
                                                      $ref: 2
                                                    },
                                                    LeafNode {
                                                      LeafInstanceId: 14,
                                                      Name: "2nd Branch 1->2 Link Step 3-3 2nd Leaf",
                                                      GlobalNodeInstanceId: 25,
                                                      NodeType: NodeType.LeafNode,
                                                      DepthToRoot: 6
                                                    },
                                                    OrderedBranchNodeAsField<IChildNode> {
                                                      BranchInstanceId: 10,
                                                      Name: "OrderedBranchNodeAsField`1_21",
                                                      GlobalNodeInstanceId: 21,
                                                      NodeType: NodeType.BranchNode,
                                                      DepthToRoot: 6,
                                                      ChildNodes: (List<IChildNode>) [
                                                        LeafNode {
                                                          LeafInstanceId: 11,
                                                          Name: "2nd Branch 2->3 Link Leaf",
                                                          GlobalNodeInstanceId: 20,
                                                          NodeType: NodeType.LeafNode,
                                                          DepthToRoot: 7
                                                        },
                                                        OrderedBranchNodeAsField<IChildNode> {
                                                          $ref: 3
                                                        }
                                                      ]
                                                    }
                                                  ]
                                                },
                                                LeafNode {
                                                  LeafInstanceId: 15,
                                                  Name: "2nd Branch 1->2 Link Step 2-3 Leaf",
                                                  GlobalNodeInstanceId: 27,
                                                  NodeType: NodeType.LeafNode,
                                                  DepthToRoot: 5
                                                }
                                              ]
                                            }
                                          ]
                                        }
                                      ]
                                    },
                                    LeafNode {
                                      LeafInstanceId: 22,
                                      Name: "Sixth Match And Branches Root "2nd" Leaf",
                                      GlobalNodeInstanceId: 40,
                                      NodeType: NodeType.LeafNode,
                                      DepthToRoot: 2
                                    },
                                    OrderedBranchNodeAsField<IChildNode> {
                                      BranchInstanceId: 17,
                                      Name: "3rd Branch Root Node",
                                      GlobalNodeInstanceId: 36,
                                      NodeType: NodeType.BranchNode,
                                      DepthToRoot: 2,
                                      ChildNodes: (List<IChildNode>) [
                                        LeafNode {
                                          LeafInstanceId: 19,
                                          Name: "3rd Branch Root Leaf",
                                          GlobalNodeInstanceId: 35,
                                          NodeType: NodeType.LeafNode,
                                          DepthToRoot: 3
                                        },
                                        OrderedBranchNodeAsField<IChildNode> {
                                          BranchInstanceId: 16,
                                          Name: "OrderedBranchNodeAsField`1_34",
                                          GlobalNodeInstanceId: 34,
                                          NodeType: NodeType.BranchNode,
                                          DepthToRoot: 3,
                                          ChildNodes: (List<IChildNode>) [
                                            LeafNode {
                                              LeafInstanceId: 18,
                                              Name: "3rd Branch Link -> 4th Leaf",
                                              GlobalNodeInstanceId: 33,
                                              NodeType: NodeType.LeafNode,
                                              DepthToRoot: 4
                                            },
                                            OrderedBranchNodeAsField<IChildNode> {
                                              $ref: 4
                                            }
                                          ]
                                        }
                                      ]
                                    },
                                    OrderedBranchNodeAsField<IChildNode> {
                                      BranchInstanceId: 18,
                                      Name: "4th Branch Root Node",
                                      GlobalNodeInstanceId: 38,
                                      NodeType: NodeType.BranchNode,
                                      DepthToRoot: 2,
                                      ChildNodes: (List<IChildNode>) [
                                        OrderedBranchNodeAsField<IChildNode> {
                                          $ref: 5
                                        },
                                        LeafNode {
                                          LeafInstanceId: 20,
                                          Name: "4th Branch Root Leaf",
                                          GlobalNodeInstanceId: 37,
                                          NodeType: NodeType.LeafNode,
                                          DepthToRoot: 3
                                        }
                                      ]
                                    },
                                    LeafNode {
                                      LeafInstanceId: 23,
                                      Name: "Sixth Match And Branches Root "3rd" Leaf",
                                      GlobalNodeInstanceId: 41,
                                      NodeType: NodeType.LeafNode,
                                      DepthToRoot: 2
                                    }
                                  ]
                                }
                              ]
                            },
                            LeafNode {
                              LeafInstanceId: 25,
                              Name: "Branches Link Leaf",
                              GlobalNodeInstanceId: 45,
                              NodeType: NodeType.LeafNode,
                              DepthToRoot: 1
                            },
                            OrderedBranchNodeAsField<IChildNode> {
                              $ref: 6
                            },
                            OrderedBranchNodeAsField<IChildNode> {
                              $ref: 7
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
                        "RootInstanceId":1,
                        "Name":"Branches Link Node",
                        "GlobalNodeInstanceId":46,
                        "NodeType":"RootNode",
                        "ChildNodes":[
                        {
                        "BranchInstanceId":20,
                        "Name":"OrderedBranchNodeAsField`1_44",
                        "GlobalNodeInstanceId":44,
                        "NodeType":"BranchNode",
                        "DepthToRoot":1,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":24,
                        "Name":"Branches Link -\u003e 4th Leaf",
                        "GlobalNodeInstanceId":43,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        },
                        {
                        "$id":"6",
                        "BranchInstanceId":19,
                        "Name":"Sixth Match And Branches Root Node",
                        "GlobalNodeInstanceId":42,
                        "NodeType":"BranchNode",
                        "DepthToRoot":1,
                        "ChildNodes":[
                        {
                        "BranchInstanceId":9,
                        "Name":"1st Branch Root Node",
                        "GlobalNodeInstanceId":19,
                        "NodeType":"BranchNode",
                        "DepthToRoot":2,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":9,
                        "Name":"1st Branch Root 1st Leaf",
                        "GlobalNodeInstanceId":17,
                        "NodeType":"LeafNode",
                        "DepthToRoot":3
                        },
                        {
                        "$id":"2",
                        "BranchInstanceId":8,
                        "Name":"Second Match Node",
                        "GlobalNodeInstanceId":16,
                        "NodeType":"BranchNode",
                        "DepthToRoot":6,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":8,
                        "Name":"Second Match Leaf",
                        "GlobalNodeInstanceId":15,
                        "NodeType":"LeafNode",
                        "DepthToRoot":7
                        },
                        {
                        "BranchInstanceId":7,
                        "Name":"1st Branch Link 2-\u003e3 Node",
                        "GlobalNodeInstanceId":14,
                        "NodeType":"BranchNode",
                        "DepthToRoot":7,
                        "ChildNodes":[
                        {
                        "$id":"3",
                        "BranchInstanceId":4,
                        "Name":"Third Match Node",
                        "GlobalNodeInstanceId":8,
                        "NodeType":"BranchNode",
                        "DepthToRoot":7,
                        "ChildNodes":[
                        {
                        "BranchInstanceId":3,
                        "Name":"1st BranchLink 3-\u003e1 Node",
                        "GlobalNodeInstanceId":6,
                        "NodeType":"BranchNode",
                        "DepthToRoot":8,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":3,
                        "Name":"1st Branch Link 3-\u003e1 Leaf",
                        "GlobalNodeInstanceId":5,
                        "NodeType":"LeafNode",
                        "DepthToRoot":9
                        },
                        {
                        "$id":"1",
                        "BranchInstanceId":1,
                        "Name":"First Match Node",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"BranchNode",
                        "DepthToRoot":3,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":1,
                        "Name":"First Match Leaf",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode",
                        "DepthToRoot":4
                        }
                        ]
                        },
                        {
                        "$id":"5",
                        "BranchInstanceId":2,
                        "Name":"Fifth Match Node",
                        "GlobalNodeInstanceId":4,
                        "NodeType":"BranchNode",
                        "DepthToRoot":3,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":2,
                        "Name":"Fifth Match Leaf",
                        "GlobalNodeInstanceId":3,
                        "NodeType":"LeafNode",
                        "DepthToRoot":4
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":4,
                        "Name":"Third Match Leaf",
                        "GlobalNodeInstanceId":7,
                        "NodeType":"LeafNode",
                        "DepthToRoot":8
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":7,
                        "Name":"1st Branch Link 2-\u003e3 Leaf",
                        "GlobalNodeInstanceId":13,
                        "NodeType":"LeafNode",
                        "DepthToRoot":8
                        },
                        {
                        "$id":"7",
                        "BranchInstanceId":5,
                        "Name":"Seventh Match Node",
                        "GlobalNodeInstanceId":10,
                        "NodeType":"BranchNode",
                        "DepthToRoot":1,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":5,
                        "Name":"Seventh Match Leaf",
                        "GlobalNodeInstanceId":9,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        }
                        ]
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":10,
                        "Name":"1st Branch Root 2nd Leaf",
                        "GlobalNodeInstanceId":18,
                        "NodeType":"LeafNode",
                        "DepthToRoot":3
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":21,
                        "Name":"Sixth Match And Branches Root \u00221st\u0022 Leaf",
                        "GlobalNodeInstanceId":39,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        },
                        {
                        "BranchInstanceId":15,
                        "Name":"OrderedBranchNodeAsField`1_32",
                        "GlobalNodeInstanceId":32,
                        "NodeType":"BranchNode",
                        "DepthToRoot":2,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":17,
                        "Name":"2nd Branch Root Leaf",
                        "GlobalNodeInstanceId":31,
                        "NodeType":"LeafNode",
                        "DepthToRoot":3
                        },
                        {
                        "$ref":"1"
                        },
                        {
                        "BranchInstanceId":14,
                        "Name":"OrderedBranchNodeAsField`1_30",
                        "GlobalNodeInstanceId":30,
                        "NodeType":"BranchNode",
                        "DepthToRoot":3,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":16,
                        "Name":"2nd Branch 1-\u003e2 Link Step 1-3 Leaf",
                        "GlobalNodeInstanceId":29,
                        "NodeType":"LeafNode",
                        "DepthToRoot":4
                        },
                        {
                        "BranchInstanceId":13,
                        "Name":"OrderedBranchNodeAsField`1_28",
                        "GlobalNodeInstanceId":28,
                        "NodeType":"BranchNode",
                        "DepthToRoot":4,
                        "ChildNodes":[
                        {
                        "BranchInstanceId":12,
                        "Name":"2nd Branch 1-\u003e2 Link Step 3-3 Node",
                        "GlobalNodeInstanceId":26,
                        "NodeType":"BranchNode",
                        "DepthToRoot":5,
                        "ChildNodes":[
                        {
                        "$id":"4",
                        "BranchInstanceId":11,
                        "Name":"Fourth Match Node",
                        "GlobalNodeInstanceId":23,
                        "NodeType":"BranchNode",
                        "DepthToRoot":4,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":12,
                        "Name":"Fourth Match Leaf",
                        "GlobalNodeInstanceId":22,
                        "NodeType":"LeafNode",
                        "DepthToRoot":5
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":13,
                        "Name":"2nd Branch 1-\u003e2 Link Step 3-3 1st Leaf",
                        "GlobalNodeInstanceId":24,
                        "NodeType":"LeafNode",
                        "DepthToRoot":6
                        },
                        {
                        "$ref":"2"
                        },
                        {
                        "LeafInstanceId":14,
                        "Name":"2nd Branch 1-\u003e2 Link Step 3-3 2nd Leaf",
                        "GlobalNodeInstanceId":25,
                        "NodeType":"LeafNode",
                        "DepthToRoot":6
                        },
                        {
                        "BranchInstanceId":10,
                        "Name":"OrderedBranchNodeAsField`1_21",
                        "GlobalNodeInstanceId":21,
                        "NodeType":"BranchNode",
                        "DepthToRoot":6,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":11,
                        "Name":"2nd Branch 2-\u003e3 Link Leaf",
                        "GlobalNodeInstanceId":20,
                        "NodeType":"LeafNode",
                        "DepthToRoot":7
                        },
                        {
                        "$ref":"3"
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":15,
                        "Name":"2nd Branch 1-\u003e2 Link Step 2-3 Leaf",
                        "GlobalNodeInstanceId":27,
                        "NodeType":"LeafNode",
                        "DepthToRoot":5
                        }
                        ]
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":22,
                        "Name":"Sixth Match And Branches Root \u00222nd\u0022 Leaf",
                        "GlobalNodeInstanceId":40,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        },
                        {
                        "BranchInstanceId":17,
                        "Name":"3rd Branch Root Node",
                        "GlobalNodeInstanceId":36,
                        "NodeType":"BranchNode",
                        "DepthToRoot":2,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":19,
                        "Name":"3rd Branch Root Leaf",
                        "GlobalNodeInstanceId":35,
                        "NodeType":"LeafNode",
                        "DepthToRoot":3
                        },
                        {
                        "BranchInstanceId":16,
                        "Name":"OrderedBranchNodeAsField`1_34",
                        "GlobalNodeInstanceId":34,
                        "NodeType":"BranchNode",
                        "DepthToRoot":3,
                        "ChildNodes":[
                        {
                        "LeafInstanceId":18,
                        "Name":"3rd Branch Link -\u003e 4th Leaf",
                        "GlobalNodeInstanceId":33,
                        "NodeType":"LeafNode",
                        "DepthToRoot":4
                        },
                        {
                        "$ref":"4"
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "BranchInstanceId":18,
                        "Name":"4th Branch Root Node",
                        "GlobalNodeInstanceId":38,
                        "NodeType":"BranchNode",
                        "DepthToRoot":2,
                        "ChildNodes":[
                        {
                        "$ref":"5"
                        },
                        {
                        "LeafInstanceId":20,
                        "Name":"4th Branch Root Leaf",
                        "GlobalNodeInstanceId":37,
                        "NodeType":"LeafNode",
                        "DepthToRoot":3
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":23,
                        "Name":"Sixth Match And Branches Root \u00223rd\u0022 Leaf",
                        "GlobalNodeInstanceId":41,
                        "NodeType":"LeafNode",
                        "DepthToRoot":2
                        }
                        ]
                        }
                        ]
                        },
                        {
                        "LeafInstanceId":25,
                        "Name":"Branches Link Leaf",
                        "GlobalNodeInstanceId":45,
                        "NodeType":"LeafNode",
                        "DepthToRoot":1
                        },
                        {
                        "$ref":"6"
                        },
                        {
                        "$ref":"7"
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
                          "RootInstanceId": 1,
                          "Name": "Branches Link Node",
                          "GlobalNodeInstanceId": 46,
                          "NodeType": "RootNode",
                          "ChildNodes": [
                            {
                              "BranchInstanceId": 20,
                              "Name": "OrderedBranchNodeAsField`1_44",
                              "GlobalNodeInstanceId": 44,
                              "NodeType": "BranchNode",
                              "DepthToRoot": 1,
                              "ChildNodes": [
                                {
                                  "LeafInstanceId": 24,
                                  "Name": "Branches Link -\u003e 4th Leaf",
                                  "GlobalNodeInstanceId": 43,
                                  "NodeType": "LeafNode",
                                  "DepthToRoot": 2
                                },
                                {
                                  "$id": "6",
                                  "BranchInstanceId": 19,
                                  "Name": "Sixth Match And Branches Root Node",
                                  "GlobalNodeInstanceId": 42,
                                  "NodeType": "BranchNode",
                                  "DepthToRoot": 1,
                                  "ChildNodes": [
                                    {
                                      "BranchInstanceId": 9,
                                      "Name": "1st Branch Root Node",
                                      "GlobalNodeInstanceId": 19,
                                      "NodeType": "BranchNode",
                                      "DepthToRoot": 2,
                                      "ChildNodes": [
                                        {
                                          "LeafInstanceId": 9,
                                          "Name": "1st Branch Root 1st Leaf",
                                          "GlobalNodeInstanceId": 17,
                                          "NodeType": "LeafNode",
                                          "DepthToRoot": 3
                                        },
                                        {
                                          "$id": "2",
                                          "BranchInstanceId": 8,
                                          "Name": "Second Match Node",
                                          "GlobalNodeInstanceId": 16,
                                          "NodeType": "BranchNode",
                                          "DepthToRoot": 6,
                                          "ChildNodes": [
                                            {
                                              "LeafInstanceId": 8,
                                              "Name": "Second Match Leaf",
                                              "GlobalNodeInstanceId": 15,
                                              "NodeType": "LeafNode",
                                              "DepthToRoot": 7
                                            },
                                            {
                                              "BranchInstanceId": 7,
                                              "Name": "1st Branch Link 2-\u003e3 Node",
                                              "GlobalNodeInstanceId": 14,
                                              "NodeType": "BranchNode",
                                              "DepthToRoot": 7,
                                              "ChildNodes": [
                                                {
                                                  "$id": "3",
                                                  "BranchInstanceId": 4,
                                                  "Name": "Third Match Node",
                                                  "GlobalNodeInstanceId": 8,
                                                  "NodeType": "BranchNode",
                                                  "DepthToRoot": 7,
                                                  "ChildNodes": [
                                                    {
                                                      "BranchInstanceId": 3,
                                                      "Name": "1st BranchLink 3-\u003e1 Node",
                                                      "GlobalNodeInstanceId": 6,
                                                      "NodeType": "BranchNode",
                                                      "DepthToRoot": 8,
                                                      "ChildNodes": [
                                                        {
                                                          "LeafInstanceId": 3,
                                                          "Name": "1st Branch Link 3-\u003e1 Leaf",
                                                          "GlobalNodeInstanceId": 5,
                                                          "NodeType": "LeafNode",
                                                          "DepthToRoot": 9
                                                        },
                                                        {
                                                          "$id": "1",
                                                          "BranchInstanceId": 1,
                                                          "Name": "First Match Node",
                                                          "GlobalNodeInstanceId": 2,
                                                          "NodeType": "BranchNode",
                                                          "DepthToRoot": 3,
                                                          "ChildNodes": [
                                                            {
                                                              "LeafInstanceId": 1,
                                                              "Name": "First Match Leaf",
                                                              "GlobalNodeInstanceId": 1,
                                                              "NodeType": "LeafNode",
                                                              "DepthToRoot": 4
                                                            }
                                                          ]
                                                        },
                                                        {
                                                          "$id": "5",
                                                          "BranchInstanceId": 2,
                                                          "Name": "Fifth Match Node",
                                                          "GlobalNodeInstanceId": 4,
                                                          "NodeType": "BranchNode",
                                                          "DepthToRoot": 3,
                                                          "ChildNodes": [
                                                            {
                                                              "LeafInstanceId": 2,
                                                              "Name": "Fifth Match Leaf",
                                                              "GlobalNodeInstanceId": 3,
                                                              "NodeType": "LeafNode",
                                                              "DepthToRoot": 4
                                                            }
                                                          ]
                                                        }
                                                      ]
                                                    },
                                                    {
                                                      "LeafInstanceId": 4,
                                                      "Name": "Third Match Leaf",
                                                      "GlobalNodeInstanceId": 7,
                                                      "NodeType": "LeafNode",
                                                      "DepthToRoot": 8
                                                    }
                                                  ]
                                                },
                                                {
                                                  "LeafInstanceId": 7,
                                                  "Name": "1st Branch Link 2-\u003e3 Leaf",
                                                  "GlobalNodeInstanceId": 13,
                                                  "NodeType": "LeafNode",
                                                  "DepthToRoot": 8
                                                },
                                                {
                                                  "$id": "7",
                                                  "BranchInstanceId": 5,
                                                  "Name": "Seventh Match Node",
                                                  "GlobalNodeInstanceId": 10,
                                                  "NodeType": "BranchNode",
                                                  "DepthToRoot": 1,
                                                  "ChildNodes": [
                                                    {
                                                      "LeafInstanceId": 5,
                                                      "Name": "Seventh Match Leaf",
                                                      "GlobalNodeInstanceId": 9,
                                                      "NodeType": "LeafNode",
                                                      "DepthToRoot": 2
                                                    }
                                                  ]
                                                }
                                              ]
                                            }
                                          ]
                                        },
                                        {
                                          "LeafInstanceId": 10,
                                          "Name": "1st Branch Root 2nd Leaf",
                                          "GlobalNodeInstanceId": 18,
                                          "NodeType": "LeafNode",
                                          "DepthToRoot": 3
                                        }
                                      ]
                                    },
                                    {
                                      "LeafInstanceId": 21,
                                      "Name": "Sixth Match And Branches Root \u00221st\u0022 Leaf",
                                      "GlobalNodeInstanceId": 39,
                                      "NodeType": "LeafNode",
                                      "DepthToRoot": 2
                                    },
                                    {
                                      "BranchInstanceId": 15,
                                      "Name": "OrderedBranchNodeAsField`1_32",
                                      "GlobalNodeInstanceId": 32,
                                      "NodeType": "BranchNode",
                                      "DepthToRoot": 2,
                                      "ChildNodes": [
                                        {
                                          "LeafInstanceId": 17,
                                          "Name": "2nd Branch Root Leaf",
                                          "GlobalNodeInstanceId": 31,
                                          "NodeType": "LeafNode",
                                          "DepthToRoot": 3
                                        },
                                        {
                                          "$ref": "1"
                                        },
                                        {
                                          "BranchInstanceId": 14,
                                          "Name": "OrderedBranchNodeAsField`1_30",
                                          "GlobalNodeInstanceId": 30,
                                          "NodeType": "BranchNode",
                                          "DepthToRoot": 3,
                                          "ChildNodes": [
                                            {
                                              "LeafInstanceId": 16,
                                              "Name": "2nd Branch 1-\u003e2 Link Step 1-3 Leaf",
                                              "GlobalNodeInstanceId": 29,
                                              "NodeType": "LeafNode",
                                              "DepthToRoot": 4
                                            },
                                            {
                                              "BranchInstanceId": 13,
                                              "Name": "OrderedBranchNodeAsField`1_28",
                                              "GlobalNodeInstanceId": 28,
                                              "NodeType": "BranchNode",
                                              "DepthToRoot": 4,
                                              "ChildNodes": [
                                                {
                                                  "BranchInstanceId": 12,
                                                  "Name": "2nd Branch 1-\u003e2 Link Step 3-3 Node",
                                                  "GlobalNodeInstanceId": 26,
                                                  "NodeType": "BranchNode",
                                                  "DepthToRoot": 5,
                                                  "ChildNodes": [
                                                    {
                                                      "$id": "4",
                                                      "BranchInstanceId": 11,
                                                      "Name": "Fourth Match Node",
                                                      "GlobalNodeInstanceId": 23,
                                                      "NodeType": "BranchNode",
                                                      "DepthToRoot": 4,
                                                      "ChildNodes": [
                                                        {
                                                          "LeafInstanceId": 12,
                                                          "Name": "Fourth Match Leaf",
                                                          "GlobalNodeInstanceId": 22,
                                                          "NodeType": "LeafNode",
                                                          "DepthToRoot": 5
                                                        }
                                                      ]
                                                    },
                                                    {
                                                      "LeafInstanceId": 13,
                                                      "Name": "2nd Branch 1-\u003e2 Link Step 3-3 1st Leaf",
                                                      "GlobalNodeInstanceId": 24,
                                                      "NodeType": "LeafNode",
                                                      "DepthToRoot": 6
                                                    },
                                                    {
                                                      "$ref": "2"
                                                    },
                                                    {
                                                      "LeafInstanceId": 14,
                                                      "Name": "2nd Branch 1-\u003e2 Link Step 3-3 2nd Leaf",
                                                      "GlobalNodeInstanceId": 25,
                                                      "NodeType": "LeafNode",
                                                      "DepthToRoot": 6
                                                    },
                                                    {
                                                      "BranchInstanceId": 10,
                                                      "Name": "OrderedBranchNodeAsField`1_21",
                                                      "GlobalNodeInstanceId": 21,
                                                      "NodeType": "BranchNode",
                                                      "DepthToRoot": 6,
                                                      "ChildNodes": [
                                                        {
                                                          "LeafInstanceId": 11,
                                                          "Name": "2nd Branch 2-\u003e3 Link Leaf",
                                                          "GlobalNodeInstanceId": 20,
                                                          "NodeType": "LeafNode",
                                                          "DepthToRoot": 7
                                                        },
                                                        {
                                                          "$ref": "3"
                                                        }
                                                      ]
                                                    }
                                                  ]
                                                },
                                                {
                                                  "LeafInstanceId": 15,
                                                  "Name": "2nd Branch 1-\u003e2 Link Step 2-3 Leaf",
                                                  "GlobalNodeInstanceId": 27,
                                                  "NodeType": "LeafNode",
                                                  "DepthToRoot": 5
                                                }
                                              ]
                                            }
                                          ]
                                        }
                                      ]
                                    },
                                    {
                                      "LeafInstanceId": 22,
                                      "Name": "Sixth Match And Branches Root \u00222nd\u0022 Leaf",
                                      "GlobalNodeInstanceId": 40,
                                      "NodeType": "LeafNode",
                                      "DepthToRoot": 2
                                    },
                                    {
                                      "BranchInstanceId": 17,
                                      "Name": "3rd Branch Root Node",
                                      "GlobalNodeInstanceId": 36,
                                      "NodeType": "BranchNode",
                                      "DepthToRoot": 2,
                                      "ChildNodes": [
                                        {
                                          "LeafInstanceId": 19,
                                          "Name": "3rd Branch Root Leaf",
                                          "GlobalNodeInstanceId": 35,
                                          "NodeType": "LeafNode",
                                          "DepthToRoot": 3
                                        },
                                        {
                                          "BranchInstanceId": 16,
                                          "Name": "OrderedBranchNodeAsField`1_34",
                                          "GlobalNodeInstanceId": 34,
                                          "NodeType": "BranchNode",
                                          "DepthToRoot": 3,
                                          "ChildNodes": [
                                            {
                                              "LeafInstanceId": 18,
                                              "Name": "3rd Branch Link -\u003e 4th Leaf",
                                              "GlobalNodeInstanceId": 33,
                                              "NodeType": "LeafNode",
                                              "DepthToRoot": 4
                                            },
                                            {
                                              "$ref": "4"
                                            }
                                          ]
                                        }
                                      ]
                                    },
                                    {
                                      "BranchInstanceId": 18,
                                      "Name": "4th Branch Root Node",
                                      "GlobalNodeInstanceId": 38,
                                      "NodeType": "BranchNode",
                                      "DepthToRoot": 2,
                                      "ChildNodes": [
                                        {
                                          "$ref": "5"
                                        },
                                        {
                                          "LeafInstanceId": 20,
                                          "Name": "4th Branch Root Leaf",
                                          "GlobalNodeInstanceId": 37,
                                          "NodeType": "LeafNode",
                                          "DepthToRoot": 3
                                        }
                                      ]
                                    },
                                    {
                                      "LeafInstanceId": 23,
                                      "Name": "Sixth Match And Branches Root \u00223rd\u0022 Leaf",
                                      "GlobalNodeInstanceId": 41,
                                      "NodeType": "LeafNode",
                                      "DepthToRoot": 2
                                    }
                                  ]
                                }
                              ]
                            },
                            {
                              "LeafInstanceId": 25,
                              "Name": "Branches Link Leaf",
                              "GlobalNodeInstanceId": 45,
                              "NodeType": "LeafNode",
                              "DepthToRoot": 1
                            },
                            {
                              "$ref": "6"
                            },
                            {
                              "$ref": "7"
                            }
                          ]
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void MultiReferencingTreeNodeCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(MultiReferencingExpect, CompactLog);
    }

    [TestMethod]
    public void MultiReferencingTreeNodeCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(MultiReferencingExpect, CompactJson);
    }

    [TestMethod]
    public void MultiReferencingTreeNodePrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(MultiReferencingExpect, PrettyLog);
    }

    [TestMethod]
    public void MultiReferencingTreeNodePrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(MultiReferencingExpect, PrettyJson);
    }
}
