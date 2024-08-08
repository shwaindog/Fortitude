// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class SelectionStrategiesAggregatorTests
{
    private const string FixedOrderStrategyName = FixedOrderSelectionStrategy.StrategyName;

    private IMessageQueueGroupContainer allQueues = null!;

    private Mock<IMessageQueue> customQueue = null!;
    private Mock<IMessageQueue> eventQueue  = null!;

    private Mock<INetworkInboundMessageQueue>  ioInboundQueue  = null!;
    private Mock<INetworkOutboundMessageQueue> ioOutboundQueue = null!;

    private IRecycler recycler = null!;

    private IRule respondingRuleFirstQueue = new RespondingRule();

    private IRule rule = null!;

    private Mock<IQueueContext> ruleContext = null!;

    private SelectionStrategiesAggregator selectionStrategyAggregator = null!;

    private Mock<IMessageQueue> workerQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler = new Recycler();

        selectionStrategyAggregator = new SelectionStrategiesAggregator();

        ioOutboundQueue = new Mock<INetworkOutboundMessageQueue>();
        ioInboundQueue  = new Mock<INetworkInboundMessageQueue>();

        eventQueue  = new Mock<IMessageQueue>();
        workerQueue = new Mock<IMessageQueue>();
        customQueue = new Mock<IMessageQueue>();
        ruleContext = new Mock<IQueueContext>();

        ioOutboundQueue.SetupGet(eq => eq.QueueType).Returns(NetworkOutbound);
        ioInboundQueue.SetupGet(eq => eq.QueueType).Returns(NetworkInbound);
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

        allQueues = new MessageQueueGroupContainer(new Mock<IConfigureMessageBus>().Object, new[]
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
        var resultEnum = selectionStrategyAggregator.Select
            (allQueues, rule, new DispatchOptions(expectedStrategySelection, Custom), "SomeAddress");

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(1, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(Custom, result.MessageQueue.QueueType);
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
        var resultEnum = selectionStrategyAggregator.Select
            (allQueues, rule, rule, new DeploymentOptions(expectedStrategySelection, Custom));

        Assert.IsNotNull(resultEnum);
        var result = resultEnum.Value;
        Assert.AreEqual(Custom, result.MessageQueue.QueueType);
        Assert.AreSame(customQueue.Object, result.MessageQueue);
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
        var resultEnum = selectionStrategyAggregator.Select
            (allQueues, rule, new DispatchOptions(expectedStrategySelection, Event), "SomeAddress");

        Assert.IsTrue(resultEnum.HasItems);
        Assert.AreEqual(1, resultEnum.Count);
        var result = resultEnum.First();
        Assert.AreEqual(Event, result.MessageQueue.QueueType);
        Assert.AreSame(eventQueue.Object, result.MessageQueue);
        Assert.IsNotNull(result.Rule);
        Assert.AreEqual(respondingRuleFirstQueue, result.Rule);
        Assert.AreEqual(FixedOrderStrategyName, result.StrategyName);
        Assert.AreEqual(expectedStrategySelection, result.RoutingFlags);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueRuleReturnsEmptySelectionResults()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var resultEnum = selectionStrategyAggregator.Select
            (allQueues, rule, new DispatchOptions(targetMessageQueueType: None), "SomeAddress");

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void WithOrderSelectionStrategyNoneEventQueueDeployRuleReturnsNull()
    {
        selectionStrategyAggregator.Add(new FixedOrderSelectionStrategy(recycler));
        var resultEnum = selectionStrategyAggregator.Select
            (allQueues, rule, rule, new DeploymentOptions(messageGroupType: None));

        Assert.IsNull(resultEnum);
    }
}
