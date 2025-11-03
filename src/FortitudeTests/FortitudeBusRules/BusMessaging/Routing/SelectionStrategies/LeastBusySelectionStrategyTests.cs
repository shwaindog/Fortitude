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
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class LeastBusySelectionStrategyTests
{
    private const string StrategyName       = LeastBusySelectionStrategy.StrategyName;
    private const string DestinationAddress = "SomeAddress";
    private const int    LeastBusyOfType    = -90;
    private const int    MostBusyOfType     = 90;

    private readonly IRule deployRule                = new ListeningRule();
    private readonly IRule respondingRuleFirstQueue  = new RespondingRule();
    private readonly IRule respondingRuleSecondQueue = new RespondingRule();
    private readonly IRule sendingRule               = new RequestingRule();

    private IMessageQueueGroupContainer        allQueues                  = null!;
    private Mock<IMessageQueue>                firstCustomQueue           = null!;
    private Mock<IMessageQueue>                firstEventQueue            = null!;
    private Mock<INetworkInboundMessageQueue>  firstIoInboundQueue        = null!;
    private Mock<INetworkOutboundMessageQueue> firstIoOutboundQueue       = null!;
    private Mock<IMessageQueue>                firstWorkerQueue           = null!;
    private LeastBusySelectionStrategy         leastBusySelectionStrategy = null!;
    private IRecycler                          recycler                   = null!;
    private Mock<IQueueContext>                ruleContext                = null!;
    private Mock<IMessageQueue>                secondCustomQueue          = null!;
    private Mock<IMessageQueue>                secondEventQueue           = null!;
    private Mock<INetworkInboundMessageQueue>  secondIoInboundQueue       = null!;
    private Mock<INetworkOutboundMessageQueue> secondIoOutboundQueue      = null!;
    private Mock<IMessageQueue>                secondWorkerQueue          = null!;


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

        firstIoOutboundQueue.SetupEventQueueMock(NetworkOutbound, 1, true, addNoRules, MostBusyOfType);
        firstIoInboundQueue.SetupEventQueueMock(NetworkInbound, 1, true, addOneFirstQueueRule, LeastBusyOfType);
        firstEventQueue.SetupEventQueueMock(Event, 1, false, addNoRules, MostBusyOfType);
        firstWorkerQueue.SetupEventQueueMock(Worker, 1, true, addOneFirstQueueRule, LeastBusyOfType);
        firstCustomQueue.SetupEventQueueMock(Custom, 1, true, addNoRules, MostBusyOfType);
        secondIoOutboundQueue.SetupEventQueueMock(NetworkOutbound, 2, true, addOneSecondQueueRule, LeastBusyOfType);
        secondIoInboundQueue.SetupEventQueueMock(NetworkInbound, 2, true, addNoRules, MostBusyOfType);
        secondEventQueue.SetupEventQueueMock(Event, 2, true, addOneSecondQueueRule, LeastBusyOfType);
        secondWorkerQueue.SetupEventQueueMock(Worker, 2, true, addNoRules, MostBusyOfType);
        secondCustomQueue.SetupEventQueueMock(Custom, 2, true, addOneSecondQueueRule, LeastBusyOfType);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        sendingRule.Context = ruleContext.Object;


        allQueues = new MessageQueueGroupContainer(new Mock<IConfigureMessageBus>().Object, new[]
        {
            firstIoOutboundQueue.Object, secondIoOutboundQueue.Object, firstIoInboundQueue.Object
          , secondIoInboundQueue.Object, firstEventQueue.Object, secondEventQueue.Object, firstWorkerQueue.Object
          , secondWorkerQueue.Object, firstCustomQueue.Object, secondCustomQueue.Object
        });

        leastBusySelectionStrategy = new LeastBusySelectionStrategy(recycler);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectCustomQueueTypeReturnsSecondCustomQueue()
    {
        var routingFlags    = RoutingFlags.LeastBusyQueue;
        var eventGroupType  = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, secondCustomQueue.Object, routingFlags, StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectCustomQueueTypeReturnsSecondCustomQueue()
    {
        var routingFlags      = RoutingFlags.LeastBusyQueue;
        var eventGroupType    = Custom;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondCustomQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags    = RoutingFlags.LeastBusyQueue;
        var eventGroupType  = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags      = RoutingFlags.LeastBusyQueue;
        var eventGroupType    = Worker;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags    = RoutingFlags.LeastBusyQueue;
        var eventGroupType  = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags      = RoutingFlags.LeastBusyQueue;
        var eventGroupType    = Event;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectIoInboundTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags    = RoutingFlags.LeastBusyQueue;
        var eventGroupType  = NetworkInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags      = RoutingFlags.LeastBusyQueue;
        var eventGroupType    = NetworkInbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected
            (eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectIoOutboundTypeReturnsSecondIoOutboundQueue()
    {
        var routingFlags    = RoutingFlags.LeastBusyQueue;
        var eventGroupType  = NetworkOutbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected
            (eventGroupType, secondIoOutboundQueue.Object, routingFlags, StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectIoOutboundQueueTypeReturnsSecondIoOutboundQueue()
    {
        var routingFlags      = RoutingFlags.LeastBusyQueue;
        var eventGroupType    = NetworkOutbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult       = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected
            (eventGroupType, secondIoOutboundQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusySelectNoEventQueueDispatchRuleReturnsEmptySelectionResults()
    {
        var resultEnum = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, new DispatchOptions(RoutingFlags.LeastBusyQueue, None), "SomeAddress");

        Assert.IsNotNull(resultEnum);
        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = leastBusySelectionStrategy.Select
            (allQueues, sendingRule, sendingRule, new DeploymentOptions(RoutingFlags.LeastBusyQueue, None));

        Assert.IsNull(resultEnum);
    }
}
