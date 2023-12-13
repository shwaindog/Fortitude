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
public class SelectionStrategiesAggregatorTests
{
    private IReusableList<IEventQueue> allQueues = null!;
    private Mock<IEventQueue> customQueue = null!;
    private Mock<IEventQueue> eventQueue = null!;
    private Mock<IEventQueue> ioInboundQueue = null!;
    private Mock<IEventQueue> ioOutboundQueue = null!;
    private IRule rule = new IncrementingRule();
    private Mock<IEventContext> ruleContext = null!;
    private SelectionStrategiesAggregator selectionStrategyAggregator = null!;
    private Mock<IEventQueue> workerQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        selectionStrategyAggregator = new SelectionStrategiesAggregator();

        ioOutboundQueue = new Mock<IEventQueue>();
        ioInboundQueue = new Mock<IEventQueue>();
        eventQueue = new Mock<IEventQueue>();
        workerQueue = new Mock<IEventQueue>();
        customQueue = new Mock<IEventQueue>();
        ruleContext = new Mock<IEventContext>();

        ioOutboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOOutbound);
        ioInboundQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.IOInbound);
        eventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Event);
        workerQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Worker);
        customQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Custom);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(eventQueue.Object);
        rule.Context = ruleContext.Object;

        allQueues = new ReusableList<IEventQueue>()
        {
            ioOutboundQueue.Object, ioInboundQueue.Object, eventQueue.Object, workerQueue.Object, customQueue.Object
        };
    }

    [TestMethod]
    public void WithOrderSelectionStrategyCustomRuleReturnsCustomEventQueueType()
    {
        selectionStrategyAggregator.Add(new EventQueueTypeOrderSelectionStrategy());
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.Custom));

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(1, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(EventQueueType.Custom, result.EventQueue.QueueType);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyCustomRuleDeployReturnsCustomEventQueueType()
    {
        selectionStrategyAggregator.Add(new EventQueueTypeOrderSelectionStrategy());
        var expectedStrategySelection
            = RoutingStrategySelectionFlags.DefaultPublish | RoutingStrategySelectionFlags.LeastBusyQueue;
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.Custom));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(EventQueueType.Custom, result.EventQueue.QueueType);
        Assert.AreSame(customQueue.Object, result.EventQueue);
        Assert.IsNull(result.Rule);
        Assert.AreEqual("EventQueueTypeOrderStrategy", result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingStrategySelectionFlags);
    }


    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueRuleReturnsEmptySelectionResults()
    {
        selectionStrategyAggregator.Add(new EventQueueTypeOrderSelectionStrategy());
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: EventQueueType.None));

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueDeployRuleReturnsNull()
    {
        selectionStrategyAggregator.Add(new EventQueueTypeOrderSelectionStrategy());
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: EventQueueType.None));

        Assert.IsNull(resultEnum);
    }
}
