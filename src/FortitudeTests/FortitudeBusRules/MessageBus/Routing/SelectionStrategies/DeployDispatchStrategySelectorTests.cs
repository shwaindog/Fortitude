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
    public void DefaultDeploySelectsRotateEvenlyLeastBusyAndFixedOrderSelectionStrategy()
    {
        var result = selector.SelectDeployStrategy(rule, rule
            , new DeploymentOptions(RoutingFlags.DefaultDeploy));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count == 3);
        Assert.AreEqual(RotateEvenlySelectionStrategy.StrategyName, strategies[0].Name);
        Assert.AreEqual(LeastBusySelectionStrategy.StrategyName, strategies[1].Name);
        Assert.AreEqual(FixedOrderSelectionStrategy.StrategyName, strategies[2].Name);
    }

    [TestMethod]
    public void DispatchDefaultPublishSelectsReuseLastCachedAndFixedOrderSelectionStrategy()
    {
        var result = selector.SelectDispatchStrategy(rule
            , new DispatchOptions(RoutingFlags.DefaultPublish));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count == 2);
        Assert.AreEqual(ReuseLastCachedResultSelectionStrategy.StrategyName, strategies[0].Name);
        Assert.AreEqual(FixedOrderSelectionStrategy.StrategyName, strategies[1].Name);
    }

    [TestMethod]
    public void DispatchDefaultRequestResponseSelectsRotateEvenlyFixedOrderSelectionStrategy()
    {
        var result = selector.SelectDispatchStrategy(rule
            , new DispatchOptions(RoutingFlags.DefaultRequestResponse));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count == 2);
        Assert.AreEqual(RotateEvenlySelectionStrategy.StrategyName, strategies[0].Name);
        Assert.AreEqual(FixedOrderSelectionStrategy.StrategyName, strategies[1].Name);
    }

    [TestMethod]
    public void DispatchPublishOneLeastBusyResponseSelectsLeastBusyFixedOrderSelectionStrategy()
    {
        var result = selector.SelectDispatchStrategy(rule
            , new DispatchOptions(RoutingFlags.LeastBusyQueue));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count == 2);
        Assert.AreEqual(LeastBusySelectionStrategy.StrategyName, strategies[0].Name);
        Assert.AreEqual(FixedOrderSelectionStrategy.StrategyName, strategies[1].Name);
    }

    [TestMethod]
    public void DispatchNoFlagsResponseSelectsFixedOrderSelectionStrategy()
    {
        var result = selector.SelectDispatchStrategy(rule
            , new DispatchOptions(RoutingFlags.None));

        var strategies = NonPublicInvocator.GetInstanceField<ReusableList<ISelectionStrategy>>(result, "backingList");

        Assert.IsTrue(strategies.Count == 1);
        Assert.AreEqual(FixedOrderSelectionStrategy.StrategyName, strategies[0].Name);
    }
}
