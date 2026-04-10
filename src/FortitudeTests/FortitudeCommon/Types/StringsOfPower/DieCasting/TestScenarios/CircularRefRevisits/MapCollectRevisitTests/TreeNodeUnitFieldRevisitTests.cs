// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.MapCollection;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.MapCollectRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class TreeNodeMapCollectionFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<MapParentNodeAsKeyedCollection<IChildNode>>?      selfReferencingExpect;
    private static InputBearerExpect<MapBranchNodeAsKeyedCollection<IChildNode>>?      dualReferencingPairExpect;
    private static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>?        secondFieldSameExpect;
    private static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>?        allThreeFieldsSameExpect;
    private static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>?        repeatedSequenceSameExpect;
    private static InputBearerExpect<MapBranchNodeAsKeyedCollection<AlwaysEmptyNode>>? repeatedEmptySequenceSameExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static MapParentNodeAsKeyedCollection<IChildNode> SelfReferencing
    {
        get
        {
            var selfReferencing = new MapParentNodeAsKeyedCollection<IChildNode>();
            selfReferencing!.Add("self", selfReferencing);
            return selfReferencing;
        }
    }

    public static InputBearerExpect<MapParentNodeAsKeyedCollection<IChildNode>> SelfReferencingExpect
    {
        get
        {
            return selfReferencingExpect ??=
                new InputBearerExpect<MapParentNodeAsKeyedCollection<IChildNode>>(SelfReferencing)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapParentNodeAsKeyedCollection<IChildNode>($id: 1) {
                         self: MapParentNodeAsKeyedCollection<IChildNode>($ref: 1)
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapParentNodeAsKeyedCollection<IChildNode>($id: 1) {
                          self: MapParentNodeAsKeyedCollection<IChildNode>($ref: 1)
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "$id":"1",
                        "self":{
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
                          "$id": "1",
                          "self": {
                            "$ref": "1"
                          }
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

    public static MapBranchNodeAsKeyedCollection<IChildNode> DualReferencingPair
    {
        get
        {
            var childLeaf = new LeafNode("DuelReferenceLeaf");
            var child = new MapBranchNodeAsKeyedCollection<IChildNode>
            {
                { "SameLeaf", childLeaf }
            };

            var dualReferencingPair = new MapBranchNodeAsKeyedCollection<IChildNode>
            {
                { "HasSameLeaf", child }
              , { "SameLeaf", childLeaf }
            };
            return dualReferencingPair;
        }
    }

    public static InputBearerExpect<MapBranchNodeAsKeyedCollection<IChildNode>> DualReferencingPairExpect
    {
        get
        {
            return dualReferencingPairExpect ??=
                new InputBearerExpect<MapBranchNodeAsKeyedCollection<IChildNode>>(DualReferencingPair)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapBranchNodeAsKeyedCollection<IChildNode> {
                         HasSameLeaf: MapBranchNodeAsKeyedCollection<IChildNode> {
                         SameLeaf: LeafNode($id: 1) {
                         LeafInstanceId: 1,
                         Name: "DuelReferenceLeaf",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode
                         },
                         BranchInstanceId: 1
                         },
                         SameLeaf: LeafNode($ref: 1),
                         BranchInstanceId: 2
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapBranchNodeAsKeyedCollection<IChildNode> {
                          HasSameLeaf: MapBranchNodeAsKeyedCollection<IChildNode> {
                            SameLeaf: LeafNode($id: 1) {
                              LeafInstanceId: 1,
                              Name: "DuelReferenceLeaf",
                              GlobalNodeInstanceId: 1,
                              NodeType: NodeType.LeafNode
                            },
                            BranchInstanceId: 1
                          },
                          SameLeaf: LeafNode($ref: 1),
                          BranchInstanceId: 2
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "HasSameLeaf":{
                        "SameLeaf":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"DuelReferenceLeaf",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode"
                        },
                        "BranchInstanceId":1
                        },
                        "SameLeaf":{
                        "$ref":"1"
                        },
                        "BranchInstanceId":2
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "HasSameLeaf": {
                            "SameLeaf": {
                              "$id": "1",
                              "LeafInstanceId": 1,
                              "Name": "DuelReferenceLeaf",
                              "GlobalNodeInstanceId": 1,
                              "NodeType": "LeafNode"
                            },
                            "BranchInstanceId": 1
                          },
                          "SameLeaf": {
                            "$ref": "1"
                          },
                          "BranchInstanceId": 2
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


    public static MapBranchNodeAsKeyedCollection<LeafNode> SecondFieldSame
    {
        get
        {
            var child           = new LeafNode("SameChild");
            var secondFieldSame = new MapBranchNodeAsKeyedCollection<LeafNode>("SameOnLeftAndRight")
            {
                {"Left", child }
              , { "Right", child }
            };
            return secondFieldSame;
        }
    }

    public static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>> SecondFieldSameExpect
    {
        get
        {
            return secondFieldSameExpect ??=
                new InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>(SecondFieldSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                         Left: LeafNode($id: 1) {
                         LeafInstanceId: 1,
                         Name: "SameChild",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode
                         },
                         Right: LeafNode($ref: 1),
                         BranchInstanceId: 1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                          Left: LeafNode($id: 1) {
                            LeafInstanceId: 1,
                            Name: "SameChild",
                            GlobalNodeInstanceId: 1,
                            NodeType: NodeType.LeafNode
                          },
                          Right: LeafNode($ref: 1),
                          BranchInstanceId: 1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "Left":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"SameChild",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode"
                        },
                        "Right":{
                        "$ref":"1"
                        },
                        "BranchInstanceId":1
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "Left": {
                            "$id": "1",
                            "LeafInstanceId": 1,
                            "Name": "SameChild",
                            "GlobalNodeInstanceId": 1,
                            "NodeType": "LeafNode"
                          },
                          "Right": {
                            "$ref": "1"
                          },
                          "BranchInstanceId": 1
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

    public static MapBranchNodeAsKeyedCollection<LeafNode> AllThreeFieldsSame
    {
        get
        {
            var child              = new LeafNode("AllThreeFieldsSame");
            var allThreeFieldsSame = new MapBranchNodeAsKeyedCollection<LeafNode>("AllThreeFieldsSame")
            {
                { "1st", child }
              , { "2nd", child }
              , { "3rd", child }
            };
            return allThreeFieldsSame;
        }
    }

    public static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>> AllThreeFieldsSameExpect
    {
        get
        {
            return allThreeFieldsSameExpect ??=
                new InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>(AllThreeFieldsSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                         1st: LeafNode($id: 1) {
                         LeafInstanceId: 1,
                         Name: "AllThreeFieldsSame",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode
                         },
                         2nd: LeafNode($ref: 1),
                         3rd: LeafNode($ref: 1),
                         BranchInstanceId: 1,
                         Left: LeafNode($ref: 1),
                         Right: LeafNode($ref: 1)
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                          1st: LeafNode($id: 1) {
                            LeafInstanceId: 1,
                            Name: "AllThreeFieldsSame",
                            GlobalNodeInstanceId: 1,
                            NodeType: NodeType.LeafNode
                          },
                          2nd: LeafNode($ref: 1),
                          3rd: LeafNode($ref: 1),
                          BranchInstanceId: 1,
                          Left: LeafNode($ref: 1),
                          Right: LeafNode($ref: 1)
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "1st":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"AllThreeFieldsSame",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode"
                        },
                        "2nd":{"$ref":"1"},
                        "3rd":{"$ref":"1"},
                        "BranchInstanceId":1,
                        "Left":{"$ref":"1"},
                        "Right":{"$ref":"1"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "1st": {
                            "$id": "1",
                            "LeafInstanceId": 1,
                            "Name": "AllThreeFieldsSame",
                            "GlobalNodeInstanceId": 1,
                            "NodeType": "LeafNode"
                          },
                          "2nd": {
                            "$ref": "1"
                          },
                          "3rd": {
                            "$ref": "1"
                          },
                          "BranchInstanceId": 1,
                          "Left": {
                            "$ref": "1"
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

    public static MapBranchNodeAsKeyedCollection<LeafNode> RepeatedSequenceSame
    {
        get
        {
            var firstChild  = new LeafNode("FirstToRepeat");
            var secondChild = new LeafNode("SecondToRepeat");
            var thirdChild  = new LeafNode("ThirdToRepeat");
            var repeatedSequenceSame =
                new MapBranchNodeAsKeyedCollection<LeafNode>("RepeatedSequenceSame")
                {
                    { "1st", firstChild }
                  , { "2nd", secondChild }
                  , { "3rd", thirdChild }
                  , { "repeated1st", firstChild }
                  , { "repeated2nd", secondChild }
                  , { "repeated3rd", thirdChild }
                };
            return repeatedSequenceSame;
        }
    }

    public static InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>> RepeatedSequenceSameExpect
    {
        get
        {
            return repeatedSequenceSameExpect ??=
                new InputBearerExpect<MapBranchNodeAsKeyedCollection<LeafNode>>(RepeatedSequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                         1st: LeafNode($id: 1) {
                         LeafInstanceId: 1,
                         Name: "FirstToRepeat",
                         GlobalNodeInstanceId: 1,
                         NodeType: NodeType.LeafNode
                         },
                         2nd: LeafNode($id: 2) {
                         LeafInstanceId: 2,
                         Name: "SecondToRepeat",
                         GlobalNodeInstanceId: 2,
                         NodeType: NodeType.LeafNode
                         },
                         3rd: LeafNode($id: 3) {
                         LeafInstanceId: 3,
                         Name: "ThirdToRepeat",
                         GlobalNodeInstanceId: 3,
                         NodeType: NodeType.LeafNode
                         },
                         repeated1st: LeafNode($ref: 1),
                         repeated2nd: LeafNode($ref: 2),
                         repeated3rd: LeafNode($ref: 3),
                         BranchInstanceId: 1,
                         Left: LeafNode($ref: 1),
                         Right: LeafNode($ref: 3)
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapBranchNodeAsKeyedCollection<LeafNode> {
                          1st: LeafNode($id: 1) {
                            LeafInstanceId: 1,
                            Name: "FirstToRepeat",
                            GlobalNodeInstanceId: 1,
                            NodeType: NodeType.LeafNode
                          },
                          2nd: LeafNode($id: 2) {
                            LeafInstanceId: 2,
                            Name: "SecondToRepeat",
                            GlobalNodeInstanceId: 2,
                            NodeType: NodeType.LeafNode
                          },
                          3rd: LeafNode($id: 3) {
                            LeafInstanceId: 3,
                            Name: "ThirdToRepeat",
                            GlobalNodeInstanceId: 3,
                            NodeType: NodeType.LeafNode
                          },
                          repeated1st: LeafNode($ref: 1),
                          repeated2nd: LeafNode($ref: 2),
                          repeated3rd: LeafNode($ref: 3),
                          BranchInstanceId: 1,
                          Left: LeafNode($ref: 1),
                          Right: LeafNode($ref: 3)
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "1st":{
                        "$id":"1",
                        "LeafInstanceId":1,
                        "Name":"FirstToRepeat",
                        "GlobalNodeInstanceId":1,
                        "NodeType":"LeafNode"
                        },
                        "2nd":{
                        "$id":"2",
                        "LeafInstanceId":2,
                        "Name":"SecondToRepeat",
                        "GlobalNodeInstanceId":2,
                        "NodeType":"LeafNode"
                        },
                        "3rd":{
                        "$id":"3",
                        "LeafInstanceId":3,
                        "Name":"ThirdToRepeat",
                        "GlobalNodeInstanceId":3,
                        "NodeType":"LeafNode"
                        },
                        "repeated1st":{"$ref":"1"},
                        "repeated2nd":{"$ref":"2"},
                        "repeated3rd":{"$ref":"3"},
                        "BranchInstanceId":1,
                        "Left":{"$ref":"1"},
                        "Right":{"$ref":"3"}
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "1st": {
                            "$id": "1",
                            "LeafInstanceId": 1,
                            "Name": "FirstToRepeat",
                            "GlobalNodeInstanceId": 1,
                            "NodeType": "LeafNode"
                          },
                          "2nd": {
                            "$id": "2",
                            "LeafInstanceId": 2,
                            "Name": "SecondToRepeat",
                            "GlobalNodeInstanceId": 2,
                            "NodeType": "LeafNode"
                          },
                          "3rd": {
                            "$id": "3",
                            "LeafInstanceId": 3,
                            "Name": "ThirdToRepeat",
                            "GlobalNodeInstanceId": 3,
                            "NodeType": "LeafNode"
                          },
                          "repeated1st": {
                            "$ref": "1"
                          },
                          "repeated2nd": {
                            "$ref": "2"
                          },
                          "repeated3rd": {
                            "$ref": "3"
                          },
                          "BranchInstanceId": 1,
                          "Left": {
                            "$ref": "1"
                          },
                          "Right": {
                            "$ref": "3"
                          }
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

    public static MapBranchNodeAsKeyedCollection<AlwaysEmptyNode> RepeatedEmptySequenceSame
    {
        get
        {
            var firstChild  = new AlwaysEmptyNode();
            var secondChild = new AlwaysEmptyNode();
            var thirdChild  = new AlwaysEmptyNode();
            var repeatedEmptySequenceSame =
                new MapBranchNodeAsKeyedCollection<AlwaysEmptyNode>("RepeatedEmptySequenceSame")
                {
                    { "1st", firstChild }
                  , { "2nd", secondChild}
                  , { "3rd", thirdChild }
                  , { "repeated1st", firstChild}
                  , { "repeated2nd", secondChild}
                  , { "repeated3rd", thirdChild }
                };
            return repeatedEmptySequenceSame;
        }
    }

    public static InputBearerExpect<MapBranchNodeAsKeyedCollection<AlwaysEmptyNode>> RepeatedEmptySequenceSameExpect
    {
        get
        {
            return repeatedEmptySequenceSameExpect ??=
                new InputBearerExpect<MapBranchNodeAsKeyedCollection<AlwaysEmptyNode>>(RepeatedEmptySequenceSame)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        MapBranchNodeAsKeyedCollection<AlwaysEmptyNode> {
                         1st: AlwaysEmptyNode($id: 1) {},
                         2nd: AlwaysEmptyNode($id: 2) {},
                         3rd: AlwaysEmptyNode($id: 3) {},
                         repeated1st: AlwaysEmptyNode($ref: 1),
                         repeated2nd: AlwaysEmptyNode($ref: 2),
                         repeated3rd: AlwaysEmptyNode($ref: 3),
                         BranchInstanceId: 1,
                         Left: AlwaysEmptyNode($ref: 1),
                         Right: AlwaysEmptyNode($ref: 3)
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        MapBranchNodeAsKeyedCollection<AlwaysEmptyNode> {
                          1st: AlwaysEmptyNode($id: 1) {},
                          2nd: AlwaysEmptyNode($id: 2) {},
                          3rd: AlwaysEmptyNode($id: 3) {},
                          repeated1st: AlwaysEmptyNode($ref: 1),
                          repeated2nd: AlwaysEmptyNode($ref: 2),
                          repeated3rd: AlwaysEmptyNode($ref: 3),
                          BranchInstanceId: 1,
                          Left: AlwaysEmptyNode($ref: 1),
                          Right: AlwaysEmptyNode($ref: 3)
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "1st":{
                        "$id":"1"
                        },
                        "2nd":{
                        "$id":"2"
                        },
                        "3rd":{
                        "$id":"3"
                        },
                        "repeated1st":{
                        "$ref":"1"
                        },
                        "repeated2nd":{
                        "$ref":"2"
                        },
                        "repeated3rd":{
                        "$ref":"3"
                        },
                        "BranchInstanceId":1,
                        "Left":{
                        "$ref":"1"
                        },
                        "Right":{
                        "$ref":"3"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """
                        {
                          "1st": {
                            "$id": "1"
                          },
                          "2nd": {
                            "$id": "2"
                          },
                          "3rd": {
                            "$id": "3"
                          },
                          "repeated1st": {
                            "$ref": "1"
                          },
                          "repeated2nd": {
                            "$ref": "2"
                          },
                          "repeated3rd": {
                            "$ref": "3"
                          },
                          "BranchInstanceId": 1,
                          "Left": {
                            "$ref": "1"
                          },
                          "Right": {
                            "$ref": "3"
                          }
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
