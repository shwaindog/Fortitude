#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class DeployDispatchStrategySelectorTests
{
    private IRecycler recycler = null!;
    private IncrementingRule rule = null!;
    private Mock<IEventContext> ruleContext = null!;
    private Mock<IEventQueue> ruleEventQueue = null!;
    private DeployDispatchStrategySelector selector = null!;


    [TestInitialize]
    public void Setup()
    {
        ruleEventQueue = new Mock<IEventQueue>();
        ruleContext = new Mock<IEventContext>();
        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(ruleEventQueue.Object);
        ruleEventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Event);

        rule = new IncrementingRule();
        recycler = new Recycler();
        selector = new DeployDispatchStrategySelector(recycler);
    }

    [TestMethod]
    public void DefaultDeploySelectsOrderSelectionStrategy()
    {
        var result = selector.SelectDeployStrategy(rule, rule
            , new DeploymentOptions(1, EventQueueType.Event, RoutingStrategySelectionFlags.DefaultDeploy));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count > 0);
        var firstStrategy = strategies[0];
        Assert.AreEqual(new EventQueueTypeOrderSelectionStrategy().Name, firstStrategy.Name);
    }

    [TestMethod]
    public void DefaultDispatchPublishAllSelectsOrderSelectionStrategy()
    {
        var result = selector.SelectDispatchStrategy(rule
            , new DispatchOptions(TargetType.PublishAll, RoutingStrategySelectionFlags.DefaultPublish));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count > 0);
        var firstStrategy = strategies[0];
        Assert.AreEqual(new EventQueueTypeOrderSelectionStrategy().Name, firstStrategy.Name);
    }
}
