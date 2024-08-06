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
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class FixedOrderSelectionStrategyTests
{
    private const string StrategyName       = FixedOrderSelectionStrategy.StrategyName;
    private const string DestinationAddress = "SomeAddress";

    private readonly IRule deployRule                = new ListeningRule();
    private readonly IRule respondingRuleFirstQueue  = new RespondingRule();
    private readonly IRule respondingRuleSecondQueue = new RespondingRule();
    private readonly IRule sendingRule               = new RequestingRule();

    private IMessageQueueGroupContainer        allQueues                   = null!;
    private Mock<IMessageQueue>                firstCustomQueue            = null!;
    private Mock<IMessageQueue>                firstEventQueue             = null!;
    private Mock<INetworkInboundMessageQueue>  firstIoInboundQueue         = null!;
    private Mock<INetworkOutboundMessageQueue> firstIoOutboundQueue        = null!;
    private Mock<IMessageQueue>                firstWorkerQueue            = null!;
    private FixedOrderSelectionStrategy        fixedOrderSelectionStrategy = null!;
    private IRecycler                          recycler                    = null!;
    private Mock<IQueueContext>                ruleContext                 = null!;
    private Mock<IMessageQueue>                secondCustomQueue           = null!;
    private Mock<IMessageQueue>                secondEventQueue            = null!;
    private Mock<INetworkInboundMessageQueue>  secondIoInboundQueue        = null!;
    private Mock<INetworkOutboundMessageQueue> secondIoOutboundQueue       = null!;
    private Mock<IMessageQueue>                secondWorkerQueue           = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler = new Recycler();

        firstIoOutboundQueue  = new Mock<INetworkOutboundMessageQueue>();
        firstIoInboundQueue   = new Mock<INetworkInboundMessageQueue>();
        firstEventQueue       = new Mock<IMessageQueue>();
        firstWorkerQueue      = new Mock<IMessageQueue>();
        firstCustomQueue      = new Mock<IMessageQueue>();
        secondIoOutboundQueue = new Mock<INetworkOutboundMessageQueue>();
        secondIoInboundQueue  = new Mock<INetworkInboundMessageQueue>();
        secondEventQueue      = new Mock<IMessageQueue>();
        secondWorkerQueue     = new Mock<IMessageQueue>();
        secondCustomQueue     = new Mock<IMessageQueue>();
        ruleContext           = new Mock<IQueueContext>();

        var oneFirstQueueRules  = new InsertionOrderSet<IRule> { respondingRuleFirstQueue };
        var oneSecondQueueRules = new InsertionOrderSet<IRule> { respondingRuleSecondQueue };

        Action<ISet<IRule>, string> addNoRules            = (rule, ruleName) => { };
        Action<ISet<IRule>, string> addOneFirstQueueRule  = (toAddTo, _) => toAddTo.UnionWith(oneFirstQueueRules);
        Action<ISet<IRule>, string> addOneSecondQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneSecondQueueRules);

        firstIoOutboundQueue.SetupEventQueueMock(NetworkOutbound, 1, true, addOneFirstQueueRule);
        firstIoInboundQueue.SetupEventQueueMock(NetworkInbound, 1, true, addOneFirstQueueRule);
        firstEventQueue.SetupEventQueueMock(Event, 1, false, addNoRules);
        firstWorkerQueue.SetupEventQueueMock(Worker, 1, true, addOneFirstQueueRule);
        firstCustomQueue.SetupEventQueueMock(Custom, 1, true, addOneFirstQueueRule);
        secondIoOutboundQueue.SetupEventQueueMock(NetworkOutbound, 2, true, addOneSecondQueueRule);
        secondIoInboundQueue.SetupEventQueueMock(NetworkInbound, 2, true, addOneSecondQueueRule);
        secondEventQueue.SetupEventQueueMock(Event, 2, true, addOneSecondQueueRule);
        secondWorkerQueue.SetupEventQueueMock(Worker, 2, true, addOneSecondQueueRule);
        secondCustomQueue.SetupEventQueueMock(Custom, 2, true, addOneSecondQueueRule);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        sendingRule.Context = ruleContext.Object;

        allQueues = new MessageQueueGroupContainer(new Mock<IConfigureMessageBus>().Object, new[]
        {
            firstIoOutboundQueue.Object, secondIoOutboundQueue.Object, firstIoInboundQueue.Object
          , secondIoInboundQueue.Object, firstEventQueue.Object, secondEventQueue.Object, firstWorkerQueue.Object
          , secondWorkerQueue.Object, firstCustomQueue.Object, secondCustomQueue.Object
        });

        fixedOrderSelectionStrategy = new FixedOrderSelectionStrategy(recycler);
    }

    [TestMethod]
    public void DefaultPublishSelectCustomQueueTypeReturnsAllCustomQueues()
    {
        var routingFlags       = RoutingFlags.DefaultPublish;
        var eventGroupType     = Custom;
        var dispatchOptions    = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(2, selectionResultSet.Count);
        selectionResultSet.First()
                          .AssertIsExpected(eventGroupType, firstCustomQueue.Object, routingFlags, StrategyName);
        selectionResultSet.Last()
                          .AssertIsExpected(eventGroupType, secondCustomQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectCustomQueueTypeReturnsFirstCustomQueue()
    {
        var routingFlags    = RoutingFlags.DefaultRequestResponse;
        var eventGroupType  = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, firstCustomQueue.Object, routingFlags, StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectCustomQueueTypeReturnsFirstCustomQueue()
    {
        var routingFlags      = RoutingFlags.DefaultDeploy;
        var eventGroupType    = Custom;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstCustomQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectWorkerQueueTypeReturnsAllWorkerQueues()
    {
        var routingFlags       = RoutingFlags.DefaultPublish;
        var eventGroupType     = Worker;
        var dispatchOptions    = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(2, selectionResultSet.Count);
        selectionResultSet.First()
                          .AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName);
        selectionResultSet.Last()
                          .AssertIsExpected(eventGroupType, secondWorkerQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags    = RoutingFlags.DefaultRequestResponse;
        var eventGroupType  = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags      = RoutingFlags.DefaultDeploy;
        var eventGroupType    = Worker;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags       = RoutingFlags.DefaultPublish;
        var eventGroupType     = Event;
        var dispatchOptions    = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(1, selectionResultSet.Count);
        selectionResultSet.First()
                          .AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags    = RoutingFlags.DefaultRequestResponse;
        var eventGroupType  = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags      = RoutingFlags.DefaultDeploy;
        var eventGroupType    = Event;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectIoInboundTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags    = RoutingFlags.DefaultPublish;
        var eventGroupType  = NetworkInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(2, selectionResultSet.Count);
        selectionResultSet.First()
                          .AssertIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName);
        selectionResultSet.Last()
                          .AssertIsExpected(eventGroupType, secondIoInboundQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectIoInboundQueueTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags    = RoutingFlags.DefaultRequestResponse;
        var eventGroupType  = NetworkInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags      = RoutingFlags.DefaultDeploy;
        var eventGroupType    = NetworkInbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectIoOutboundTypeReturnsFirstIoOutboundQueue()
    {
        var routingFlags       = RoutingFlags.DefaultPublish;
        var eventGroupType     = NetworkOutbound;
        var dispatchOptions    = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(2, selectionResultSet.Count);
        selectionResultSet.First()
                          .AssertIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags, StrategyName);
        selectionResultSet.Last()
                          .AssertIsExpected(eventGroupType, secondIoOutboundQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectIoOutboundQueueTypeReturnsFirstIoOutboundQueue()
    {
        var routingFlags       = RoutingFlags.DefaultRequestResponse;
        var eventGroupType     = NetworkOutbound;
        var dispatchOptions    = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags, StrategyName
                                                      , respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectIoOutboundQueueTypeReturnsFirstIoOutboundQueue()
    {
        var routingFlags      = RoutingFlags.DefaultDeploy;
        var eventGroupType    = NetworkOutbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void NoneEventQueueDispatchRuleReturnsEmptySelectionResults()
    {
        var resultEnum = fixedOrderSelectionStrategy.Select
            (allQueues, sendingRule, new DispatchOptions(targetMessageQueueType: None), "SomeAddress");

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, sendingRule, new DeploymentOptions(messageGroupType: None));

        Assert.IsNull(resultEnum);
    }
}
