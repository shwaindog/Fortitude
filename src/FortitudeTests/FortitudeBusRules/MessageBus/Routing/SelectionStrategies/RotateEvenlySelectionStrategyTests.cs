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
public class RotateEvenlySelectionStrategyTests
{
    private const string StrategyName = RotateEvenlySelectionStrategy.StrategyName;
    private const string DestinationAddress = "SomeAddress";
    private readonly IRule deployRule = new ListeningRule();
    private readonly IRule firstRespondingRuleFirstQueue = new RespondingRule();
    private readonly IRule firstRespondingRuleSecondQueue = new RespondingRule();
    private readonly IRule firstRespondingRuleThirdQueue = new RespondingRule();
    private readonly IRule secondRespondingRuleFirstQueue = new RespondingRule();
    private readonly IRule secondRespondingRuleThirdQueue = new RespondingRule();
    private readonly IRule sendingRule = new RequestingRule();
    private IEventQueueGroupContainer allQueues = null!;
    private DeployDispatchDestinationCache deployDispatchDestinationCache = null!;
    private Mock<IEventQueue> firstCustomQueue = null!;
    private Mock<IEventQueue> firstEventQueue = null!;
    private Mock<IEventQueue> firstIoInboundQueue = null!;
    private Mock<IEventQueue> firstIoOutboundQueue = null!;
    private Mock<IEventQueue> firstWorkerQueue = null!;
    private FixedOrderSelectionStrategy fixedOrderSelectionStrategy = null!;
    private IRecycler recycler = null!;
    private RotateEvenlySelectionStrategy rotateEvenlySelectionStrategy = null!;
    private Mock<IEventContext> ruleContext = null!;
    private Mock<IEventQueue> secondCustomQueue = null!;
    private Mock<IEventQueue> secondEventQueue = null!;
    private Mock<IEventQueue> secondIoInboundQueue = null!;
    private Mock<IEventQueue> secondIoOutboundQueue = null!;
    private Mock<IEventQueue> secondWorkerQueue = null!;
    private Mock<IEventQueue> thirdCustomQueue = null!;
    private Mock<IEventQueue> thirdEventQueue = null!;
    private Mock<IEventQueue> thirdIoInboundQueue = null!;
    private Mock<IEventQueue> thirdIoOutboundQueue = null!;
    private Mock<IEventQueue> thirdWorkerQueue = null!;


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
        thirdIoOutboundQueue = new Mock<IEventQueue>();
        thirdIoInboundQueue = new Mock<IEventQueue>();
        thirdEventQueue = new Mock<IEventQueue>();
        thirdWorkerQueue = new Mock<IEventQueue>();
        thirdCustomQueue = new Mock<IEventQueue>();
        ruleContext = new Mock<IEventContext>();

        var noQueueRules = new InsertionOrderSet<IRule>();
        var oneFirstQueueRules = new InsertionOrderSet<IRule> { firstRespondingRuleFirstQueue };
        var twoFirstQueueRules = new InsertionOrderSet<IRule>
            { firstRespondingRuleFirstQueue, secondRespondingRuleFirstQueue };

        var oneSecondQueueRules = new InsertionOrderSet<IRule> { firstRespondingRuleSecondQueue };

        var oneThirdQueueRules = new InsertionOrderSet<IRule> { firstRespondingRuleThirdQueue };
        var twoThirdQueueRules = new InsertionOrderSet<IRule>
            { firstRespondingRuleThirdQueue, secondRespondingRuleThirdQueue };

        Action<ISet<IRule>, string> addNoRules = (rule, ruleName) => { };
        Action<ISet<IRule>, string> addOneFirstQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneFirstQueueRules);
        Action<ISet<IRule>, string> addTwoFirstQueueRule = (toAddTo, _) => toAddTo.UnionWith(twoFirstQueueRules);
        Action<ISet<IRule>, string> addOneSecondQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneSecondQueueRules);
        Action<ISet<IRule>, string> addOneThirdQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneThirdQueueRules);
        Action<ISet<IRule>, string> addTwoThirdQueueRule = (toAddTo, _) => toAddTo.UnionWith(twoThirdQueueRules);

        firstIoOutboundQueue.SetupEventQueueMock(IOOutbound, 1, false, addNoRules);
        firstIoInboundQueue.SetupEventQueueMock(IOInbound, 1, true, addTwoFirstQueueRule);
        firstEventQueue.SetupEventQueueMock(Event, 1, true, addOneFirstQueueRule);
        firstWorkerQueue.SetupEventQueueMock(Worker, 1, false, addNoRules);
        firstCustomQueue.SetupEventQueueMock(Custom, 1, true, addOneFirstQueueRule);
        secondIoOutboundQueue.SetupEventQueueMock(IOOutbound, 2, true, addOneSecondQueueRule);
        secondIoInboundQueue.SetupEventQueueMock(IOInbound, 2, false, addNoRules);
        secondEventQueue.SetupEventQueueMock(Event, 2, true, addOneSecondQueueRule);
        secondWorkerQueue.SetupEventQueueMock(Worker, 2, false, addNoRules);
        secondCustomQueue.SetupEventQueueMock(Custom, 2, true, addOneSecondQueueRule);
        thirdIoOutboundQueue.SetupEventQueueMock(IOOutbound, 3, true, addOneThirdQueueRule);
        thirdIoInboundQueue.SetupEventQueueMock(IOInbound, 3, false, addNoRules);
        thirdEventQueue.SetupEventQueueMock(Event, 3, true, addTwoThirdQueueRule);
        thirdWorkerQueue.SetupEventQueueMock(Worker, 3, true, addOneThirdQueueRule);
        thirdCustomQueue.SetupEventQueueMock(Custom, 3, true, addOneThirdQueueRule);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        sendingRule.Context = ruleContext.Object;


        allQueues = new EventQueueGroupContainer(new Mock<IConfigureEventBus>().Object, new[]
        {
            firstIoOutboundQueue.Object, secondIoOutboundQueue.Object, thirdIoOutboundQueue.Object
            , firstIoInboundQueue.Object, secondIoInboundQueue.Object, thirdIoInboundQueue.Object
            , firstEventQueue.Object, secondEventQueue.Object, thirdEventQueue.Object, firstWorkerQueue.Object
            , secondWorkerQueue.Object, thirdWorkerQueue.Object, firstCustomQueue.Object, secondCustomQueue.Object
            , thirdCustomQueue.Object
        });

        deployDispatchDestinationCache = new DeployDispatchDestinationCache();
        fixedOrderSelectionStrategy = new FixedOrderSelectionStrategy(recycler);
        rotateEvenlySelectionStrategy = new RotateEvenlySelectionStrategy(recycler, deployDispatchDestinationCache);
    }

    [TestMethod]
    public void RotateEvenlyPublishOneOrRequestResponseSelectCustomQueueTypeReturnsSecondThirdFirstCustomQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, firstResult);

        var selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondCustomQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleSecondQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdCustomQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstCustomQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleFirstQueue);
    }

    [TestMethod]
    public void RotateEvenlyDeploySelectCustomQueueTypeReturnsSecondThirdAndFirstCustomQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Custom;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, firstResult!.Value);

        var routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondCustomQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, thirdCustomQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstCustomQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);
    }

    [TestMethod]
    public void RotateEvenlyPublishOneOrRequestResponseSelectWorkerQueueTypeReturnsOnly3WorkerQueueRepeatedly()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, firstResult);

        var selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdWorkerQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdWorkerQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdWorkerQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);
    }

    [TestMethod]
    public void RotateEvenlyDeploySelectWorkerQueueTypeReturns2_3_1WorkerQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Worker;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, firstResult!.Value);

        var routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondWorkerQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, thirdWorkerQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);
    }

    [TestMethod]
    public void RotateEvenlyPublishOneOrRequestResponseSelectEventQueueTypeReturns2_3_3_1EventQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, firstResult);

        var selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondEventQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleSecondQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdEventQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdEventQueue.Object
            , routingFlags, StrategyName, secondRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstEventQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleFirstQueue);
    }

    [TestMethod]
    public void RotateEvenlyDeploySelectEventQueueTypeReturns2_3_1EventQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = Event;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, firstResult!.Value);

        var routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, thirdEventQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstEventQueue.Object, routingFlags, StrategyName, deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);
    }

    [TestMethod]
    public void RotateEvenlyPublishOneOrRequestResponseSelectIoInboundTypeReturnsFirstIoInboundQueueRepeatedly()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = IOInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, firstResult);

        var selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoInboundQueue.Object
            , routingFlags, StrategyName, secondRespondingRuleFirstQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoInboundQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleFirstQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoInboundQueue.Object
            , routingFlags, StrategyName, secondRespondingRuleFirstQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);
    }

    [TestMethod]
    public void RotateEvenlyDeploySelectIoInboundQueueTypeReturnsReturns2_3_1IoInboundQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = IOInbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, firstResult!.Value);

        var routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondIoInboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, thirdIoInboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);
    }

    [TestMethod]
    public void RotateEvenlyPublishOneOrRequestResponseSelectIoOutboundTypeReturns3_2_3IoOutboundQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = IOOutbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, firstResult);

        var selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdIoOutboundQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondIoOutboundQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleSecondQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);

        selectionResultSet
            = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, thirdIoOutboundQueue.Object
            , routingFlags, StrategyName, firstRespondingRuleThirdQueue);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress, selectionResultSet!);
    }

    [TestMethod]
    public void RotateEvenlyDeploySelectIoOutboundQueueTypeReturns2_3_1IoOutboundQueue()
    {
        var routingFlags = RoutingFlags.RotateEvenly;
        var eventGroupType = IOOutbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var firstResult
            = fixedOrderSelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, firstResult!.Value);

        var routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondIoOutboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, thirdIoOutboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);

        routeResult = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, deployRule
            , deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoOutboundQueue.Object, routingFlags, StrategyName
            , deployRule);
        deployDispatchDestinationCache.Save(sendingRule, sendingRule, deploymentOptions, routeResult!.Value);
    }

    [TestMethod]
    public void RotateEvenlySelectNoCacheQueueDispatchRuleReturnsNull()
    {
        var selectionResultSet = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule
            , new DispatchOptions(RoutingFlags.RotateEvenly, Event), "SomeAddress");

        Assert.IsNull(selectionResultSet);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = rotateEvenlySelectionStrategy.Select(allQueues, sendingRule, sendingRule
            , new DeploymentOptions(RoutingFlags.RotateEvenly, None));

        Assert.IsNull(resultEnum);
    }
}
