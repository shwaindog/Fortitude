#region

using FortitudeBusRules.MessageBus;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.MessageBus.Pipelines.EventQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class SelectionStrategiesAggregatorTests
{
    private const string FixedOrderStrategyName = FixedOrderSelectionStrategy.StrategyName;
    private IEventQueueGroupContainer allQueues = null!;
    private Mock<IEventQueue> customQueue = null!;
    private Mock<IEventQueue> eventQueue = null!;
    private Mock<IEventQueue> ioInboundQueue = null!;
    private Mock<IEventQueue> ioOutboundQueue = null!;
    private IRecycler recycler = null!;
    private IRule respondingRuleFirstQueue = new RespondingRule();
    private IRule rule = null!;
    private Mock<IEventContext> ruleContext = null!;
    private SelectionStrategiesAggregator selectionStrategyAggregator = null!;
    private Mock<IEventQueue> workerQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler = new Recycler();
        selectionStrategyAggregator = new SelectionStrategiesAggregator();

        ioOutboundQueue = new Mock<IEventQueue>();
        ioInboundQueue = new Mock<IEventQueue>();
        eventQueue = new Mock<IEventQueue>();
        workerQueue = new Mock<IEventQueue>();
        customQueue = new Mock<IEventQueue>();
        ruleContext = new Mock<IEventContext>();

        ioOutboundQueue.SetupGet(eq => eq.QueueType).Returns(IOOutbound);
        ioInboundQueue.SetupGet(eq => eq.QueueType).Returns(IOInbound);
        eventQueue.SetupGet(eq => eq.QueueType).Returns(Event);
        eventQueue.Setup(eq => eq.IsListeningToAddress(It.IsAny<string>())).Returns(true);
        eventQueue.Setup(eq => eq.RulesListeningToAddress(It.IsAny<ISet<IRule>>(), It.IsAny<string>()))
            .Callback((ISet<IRule> toAddTo, string destination) => toAddTo.Add(respondingRuleFirstQueue));
        workerQueue.SetupGet(eq => eq.QueueType).Returns(Worker);
        customQueue.SetupGet(eq => eq.QueueType).Returns(Custom);
        customQueue.Setup(eq => eq.IsListeningToAddress(It.IsAny<string>())).Returns(true);
        customQueue.Setup(eq => eq.RulesListeningToAddress(It.IsAny<ISet<IRule>>(), It.IsAny<string>()))
            .Callback((ISet<IRule> toAddTo, string destination) => toAddTo.Add(respondingRuleFirstQueue));

        rule = new IncrementingRule();
        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(eventQueue.Object);
        rule.Context = ruleContext.Object;

        allQueues = new EventQueueGroupContainer(new Mock<IEventBus>().Object, new[]
        {
            ioOutboundQueue.Object, ioInboundQueue.Object, eventQueue.Object, workerQueue.Object, customQueue.Object
        });
    }

    [TestMethod]
    public void DefaultPublishWithCustomQueueTypeFixedOrderSelectionStrategyReturnsFirstCustomQueue()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var expectedStrategySelection
            = RoutingFlags.DefaultPublish;
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule
            , new DispatchOptions(expectedStrategySelection, Custom), "SomeAddress");

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(1, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(Custom, result.EventQueue.QueueType);
        Assert.IsNull(result.Rule);
        Assert.AreEqual(FixedOrderStrategyName, result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingFlags);
    }

    [TestMethod]
    public void DefaultDeployCustomEventQueueTypeFixedOrderStrategyReturnsFirstCustomQueue()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var expectedStrategySelection
            = RoutingFlags.DefaultDeploy;
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule, rule
            , new DeploymentOptions(expectedStrategySelection, Custom));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(Custom, result.EventQueue.QueueType);
        Assert.AreSame(customQueue.Object, result.EventQueue);
        Assert.IsNotNull(result.Rule);
        Assert.AreEqual(rule, result.Rule);
        Assert.AreEqual(FixedOrderStrategyName, result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingFlags);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectEventQueueTypeReturnsEventQueue()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var expectedStrategySelection
            = RoutingFlags.DefaultRequestResponse;
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule
            , new DispatchOptions(expectedStrategySelection, Event), "SomeAddress");

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(1, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(Event, result.EventQueue.QueueType);
        Assert.AreSame(eventQueue.Object, result.EventQueue);
        Assert.IsNotNull(result.Rule);
        Assert.AreEqual(respondingRuleFirstQueue, result.Rule);
        Assert.AreEqual(FixedOrderStrategyName, result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingFlags);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueRuleReturnsEmptySelectionResults()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule
            , new DispatchOptions(targetEventQueueType: None), "SomeAddress");

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueDeployRuleReturnsNull()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var resultEnum = selectionStrategyAggregator.Select(allQueues, rule, rule
            , new DeploymentOptions(eventGroupType: None));

        Assert.IsNull(resultEnum);
    }
}
