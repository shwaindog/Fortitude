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
public class LeastBusySelectionStrategyTests
{
    private const string StrategyName = LeastBusySelectionStrategy.StrategyName;
    private const string DestinationAddress = "SomeAddress";
    private const int LeastBusyOfType = -90;
    private const int MostBusyOfType = 90;
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
    private LeastBusySelectionStrategy leastBusySelectionStrategy = null!;
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

        var noQueueRules = new InsertionOrderSet<IRule>();
        var oneFirstQueueRules = new InsertionOrderSet<IRule> { respondingRuleFirstQueue };
        var oneSecondQueueRules = new InsertionOrderSet<IRule> { respondingRuleSecondQueue };


        Action<ISet<IRule>, string> addNoRules = (rule, ruleName) => { };
        Action<ISet<IRule>, string> addOneFirstQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneFirstQueueRules);
        Action<ISet<IRule>, string> addOneSecondQueueRule = (toAddTo, _) => toAddTo.UnionWith(oneSecondQueueRules);

        firstIoOutboundQueue.SetupEventQueueMock(IOOutbound, 1, true, addNoRules, MostBusyOfType);
        firstIoInboundQueue.SetupEventQueueMock(IOInbound, 1, true, addOneFirstQueueRule, LeastBusyOfType);
        firstEventQueue.SetupEventQueueMock(Event, 1, false, addNoRules, MostBusyOfType);
        firstWorkerQueue.SetupEventQueueMock(Worker, 1, true, addOneFirstQueueRule, LeastBusyOfType);
        firstCustomQueue.SetupEventQueueMock(Custom, 1, true, addNoRules, MostBusyOfType);
        secondIoOutboundQueue.SetupEventQueueMock(IOOutbound, 2, true, addOneSecondQueueRule, LeastBusyOfType);
        secondIoInboundQueue.SetupEventQueueMock(IOInbound, 2, true, addNoRules, MostBusyOfType);
        secondEventQueue.SetupEventQueueMock(Event, 2, true, addOneSecondQueueRule, LeastBusyOfType);
        secondWorkerQueue.SetupEventQueueMock(Worker, 2, true, addNoRules, MostBusyOfType);
        secondCustomQueue.SetupEventQueueMock(Custom, 2, true, addOneSecondQueueRule, LeastBusyOfType);

        ruleContext.SetupGet(ec => ec.RegisteredOn).Returns(firstEventQueue.Object);
        sendingRule.Context = ruleContext.Object;


        allQueues = new EventQueueGroupContainer(new Mock<IConfigureEventBus>().Object, new[]
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
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Custom;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondCustomQueue.Object, routingFlags
            , StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectCustomQueueTypeReturnsSecondCustomQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Custom;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondCustomQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Worker;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Worker;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstWorkerQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Event;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondEventQueue.Object, routingFlags
            , StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectEventQueueTypeReturnsSecondEventQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = Event;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondEventQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectIoInboundTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = IOInbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags
            , StrategyName, respondingRuleFirstQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = IOInbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, firstIoInboundQueue.Object, routingFlags, StrategyName
            , deployRule);
    }

    [TestMethod]
    public void LeastBusyPublishOneOrRequestResponseSelectIoOutboundTypeReturnsSecondIoOutboundQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = IOOutbound;
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        var selectionResultSet = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , dispatchOptions, DestinationAddress);

        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, secondIoOutboundQueue.Object, routingFlags
            , StrategyName, respondingRuleSecondQueue);
    }

    [TestMethod]
    public void LeastBusyDeploySelectIoOutboundQueueTypeReturnsSecondIoOutboundQueue()
    {
        var routingFlags = RoutingFlags.LeastBusyQueue;
        var eventGroupType = IOOutbound;
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);
        var routeResult = leastBusySelectionStrategy.Select(allQueues, sendingRule, deployRule, deploymentOptions);
        routeResult.AssertIsExpected(eventGroupType, secondIoOutboundQueue.Object, routingFlags, StrategyName
            , deployRule);
    }

    [TestMethod]
    public void LeastBusySelectNoEventQueueDispatchRuleReturnsEmptySelectionResults()
    {
        var resultEnum = leastBusySelectionStrategy.Select(allQueues, sendingRule
            , new DispatchOptions(RoutingFlags.LeastBusyQueue, None), "SomeAddress");

        Assert.IsNotNull(resultEnum);
        Assert.IsFalse(resultEnum.HasItems);
    }

    [TestMethod]
    public void NoneEventQueueDeployRuleReturnsNull()
    {
        var resultEnum = leastBusySelectionStrategy.Select(allQueues, sendingRule, sendingRule
            , new DeploymentOptions(RoutingFlags.LeastBusyQueue, None));

        Assert.IsNull(resultEnum);
    }
}
