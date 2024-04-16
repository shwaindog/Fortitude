#region

using FortitudeBusRules.MessageBus;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.MessageBus.Pipelines.EventQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class FixedOrderSelectionStrategyTests
{
    private const string StrategyName = FixedOrderSelectionStrategy.StrategyName;
    private const string DestinationAddress = "SomeAddress";
    private readonly IRule deployRule = new ListeningRule();
    private readonly IRule respondingRuleFirstQueue = new RespondingRule();
    private readonly IRule respondingRuleSecondQueue = new RespondingRule();
    private readonly IRule sendingRule = new RequestingRule();

    private IEventQueueGroupContainer allQueues = null!;
    private Mock<IEventQueue> firstCustomQueue = null!;
    private Mock<IEventQueue> firstEventQueue = null!;
    private Mock<IEventQueue> firstIoInboundQueue = null!;
    private Mock<IEventQueue> firstIoOutboundQueue = null!;
    private Mock<IEventQueue> firstWorkerQueue = null!;
    private FixedOrderSelectionStrategy fixedOrderSelectionStrategy = null!;
    private IRecycler recycler = null!;
    private Mock<IEventContext> ruleContext = null!;
    private Mock<IEventQueue> secondCustomQueue = null!;
    private Mock<IEventQueue> secondEventQueue = null!;
    private Mock<IEventQueue> secondIoInboundQueue = null!;
    private Mock<IEventQueue> secondIoOutboundQueue = null!;
    private Mock<IEventQueue> secondWorkerQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler = new Recycler();

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

        var oneFirstQueueRules = new InsertionOrderSet<IRule> { respondingRuleFirstQueue };
        var oneSecondQueueRules = new InsertionOrderSet<IRule> { respondingRuleSecondQueue };

        Action<ISet<IRule>, string> addNoRules = (rule, ruleName) => { };
        Action<ISet<IRule>, string> addOneFirstQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneFirstQueueRules);
        Action<ISet<IRule>, string> addOneSecondQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneSecondQueueRules);

        firstIoOutboundQueue.SetupEventQueueMock(IOOutbound, 1, true, addOneFirstQueueRule);
        firstIoInboundQueue.SetupEventQueueMock(IOInbound, 1, true, addOneFirstQueueRule);
        firstEventQueue.SetupEventQueueMock(Event, 1, false, addNoRules);
        firstWorkerQueue.SetupEventQueueMock(Worker, 1, true, addOneFirstQueueRule);
        firstCustomQueue.SetupEventQueueMock(Custom, 1, true, addOneFirstQueueRule);
        secondIoOutboundQueue.SetupEventQueueMock(IOOutbound, 2, true, addOneSecondQueueRule);
        secondIoInboundQueue.SetupEventQueueMock(IOInbound, 2, true, addOneSecondQueueRule);
        secondEventQueue.SetupEventQueueMock(Event, 2, true, addOneSecondQueueRule);
        secondWorkerQueue.SetupEventQueueMock(Worker, 2, true, addOneSecondQueueRule);
        secondCustomQueue.SetupEventQueueMock(Custom, 2, true, addOneSecondQueueRule);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        sendingRule.Context = ruleContext.Object;

        allQueues = new EventQueueGroupContainer(new Mock<IConfigureEventBus>().Object, new[]
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
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

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
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstCustomQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectCustomQueueTypeReturnsFirstCustomQueue()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = Custom;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstCustomQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectWorkerQueueTypeReturnsAllWorkerQueues()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

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
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = Worker;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        Assert.IsTrue(selectionResultSet.HasItems);
        Assert.AreEqual(1, selectionResultSet.Count);
        selectionResultSet.First()
            .AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName);
    }

    [TestMethod]
    public void DefaultRequestResponseSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondEventQueue.Object, routingFlags
            , StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = Event;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectIoInboundTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = IOInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

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
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = IOInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = IOInbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName
            , deployRule);
    }

    [TestMethod]
    public void DefaultPublishSelectIoOutboundTypeReturnsFirstIoOutboundQueue()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = IOOutbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

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
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = IOOutbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void DefaultDeploySelectIoOutboundQueueTypeReturnsFirstIoOutboundQueue()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = IOOutbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags, StrategyName
            , deployRule);
    }

    [TestMethod]
    public void NoneEventQueueDispatchRuleReturnsEmptySelectionResults()
    {
        var resultEnum = fixedOrderSelectionStrategy.Select(allQueues, sendingRule
            , new DispatchOptions(targetEventQueueType: None), "SomeAddress");

        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, sendingRule
            , new DeploymentOptions(eventGroupType: None));

        Assert.IsNull(resultEnum);
    }
}
