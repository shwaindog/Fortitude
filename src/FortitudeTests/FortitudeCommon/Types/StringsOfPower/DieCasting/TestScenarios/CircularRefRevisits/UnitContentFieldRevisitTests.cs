// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits;

[TestClass]
public class UnitContentFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<OrderedBranchNode<IChildNode>>? selfReferencingExpect;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

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
                         BranchNodeInstanceId: 1,
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
                           BranchNodeInstanceId: 1,
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
                        "BranchNodeInstanceId":1,
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
                           "BranchNodeInstanceId": 1,
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

}
