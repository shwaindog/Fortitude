#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class EventQueueTypeOrderSelectionStrategyTests
{
    private readonly EventQueueTypeOrderSelectionStrategy orderSelectionStrategy = new();
    private IReusableList<IEventQueue> allQueues = null!;
    private Mock<IEventQueue> firstCustomQueue = null!;
    private Mock<IEventQueue> firstEventQueue = null!;
    private Mock<IEventQueue> firstIoInboundQueue = null!;
    private Mock<IEventQueue> firstIoOutboundQueue = null!;
    private Mock<IEventQueue> firstWorkerQueue = null!;
    private IRule rule = new IncrementingRule();
    private Mock<IEventContext> ruleContext = null!;
    private Mock<IEventQueue> secondCustomQueue = null!;
    private Mock<IEventQueue> secondEventQueue = null!;
    private Mock<IEventQueue> secondIoInboundQueue = null!;
    private Mock<IEventQueue> secondIoOutboundQueue = null!;
    private Mock<IEventQueue> secondWorkerQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        firstIoOutboundQueue = new Mock<IEventQueue>();
        firstIoInboundQueue = new Mock<IEventQueue>();
        firstEventQueue = new Mock<IEventQueue>();
        firstWorkerQueue = new Mock<IEventQueue>();
        firstCustomQueue = new Mock<IEventQueue>();
        secondIoOutboundQueue = new Mock<IEventQueue>();
        secondIoInboundQueue = new Mock<IEventQueue>();
        secondEventQueue = new Mock<IEventQueue>();
        secondWorkerQueue = new Mock<IEventQueue>();
        secondCustomQueue = new Mock<IEventQueue>();
        ruleContext = new Mock<IEventContext>();

        firstIoOutboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOOutbound);
        firstIoInboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOInbound);
        firstEventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Event);
        firstWorkerQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Worker);
        firstCustomQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Custom);
        secondIoOutboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOOutbound);
        secondIoInboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOInbound);
        secondEventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Event);
        secondWorkerQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Worker);
        secondCustomQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Custom);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        rule.Context = ruleContext.Object;

        allQueues = new ReusableList<IEventQueue>()
        {
            firstIoOutboundQueue.Object, secondIoOutboundQueue.Object, firstIoInboundQueue.Object
            , secondIoInboundQueue.Object, firstEventQueue.Object, secondEventQueue.Object, firstWorkerQueue.Object
            , secondWorkerQueue.Object, firstCustomQueue.Object, secondCustomQueue.Object
        };
    }

    [TestMethod]
    public void CustomRuleDispatchReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.Custom));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(2, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.Custom, result.EventQueue.QueueType);
        Assert.AreSame(firstCustomQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void CustomRuleDeployReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.Custom));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.Custom, result.EventQueue.QueueType);
        Assert.AreSame(firstCustomQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void WorkerRuleDispatchReturnsWorkerEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.Worker));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(2, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.Worker, result.EventQueue.QueueType);
        Assert.AreSame(firstWorkerQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void WorkerRuleDeployReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.Worker));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.Worker, result.EventQueue.QueueType);
        Assert.AreSame(firstWorkerQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void EventRuleDispatchReturnsEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.Event));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(2, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.Event, result.EventQueue.QueueType);
        Assert.AreSame(firstEventQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void EventRuleDeployReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.Event));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.Event, result.EventQueue.QueueType);
        Assert.AreSame(firstEventQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void IoInboundRuleDispatchReturnsIoInboundQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.IOInbound));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(2, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.IOInbound, result.EventQueue.QueueType);
        Assert.AreSame(firstIoInboundQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void IoInboundRuleDeployReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.IOInbound));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.IOInbound, result.EventQueue.QueueType);
        Assert.AreSame(firstIoInboundQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void IoOutboundDispatchRuleReturnsIoOutboundQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.IOOutbound));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(2, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.IOOutbound, result.EventQueue.QueueType);
        Assert.AreSame(firstIoOutboundQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void IoOutboundRuleDeployReturnsCustomEventQueueType()
    {
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.IOOutbound));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.IOOutbound, result.EventQueue.QueueType);
        Assert.AreSame(firstIoOutboundQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void NoneEventQueueDispatchRuleReturnsEmptySelectionResults()
    {
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.None));

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = orderSelectionStrategy.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.None));

        Assert.IsNull(resultEnum);
    }
}
