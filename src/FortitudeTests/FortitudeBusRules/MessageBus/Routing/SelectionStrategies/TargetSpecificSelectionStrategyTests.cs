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
using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlags;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class TargetSpecificSelectionStrategyTests
{
    public const string StrategyName = TargetSpecificSelectionStrategy.StrategyName;
    private const string FirstStrategyName = "FirstSelectionStrategy";
    private const string FirstQueueName = "Event_1";
    private const string SecondStrategyName = "SecondSelectionStrategy";
    private const string FirstDestinationAddress = "FirstDestinationAddress";

    private IEventQueueGroupContainer allQueues = null!;
    private IRule deployRule = null!;
    private IDispatchSelectionResultSet firstResultSet = null!;
    private RouteSelectionResult firstRouteSelectionResult;
    private IRule firstSenderRule = null!;
    private Mock<IRule> firstTargetRule = null!;
    private Mock<IEventContext> firstTargetRuleContext = null!;
    private Mock<IEventQueue> moqFirstEventQueue = null!;
    private Mock<IEventQueue> moqSecondEventQueue = null!;
    private IDispatchSelectionResultSet secondResultSet = null!;
    private RouteSelectionResult secondRouteSelectionResult;
    private IRule secondSenderRule = null!;
    private TargetSpecificSelectionStrategy targetSpecificSelectionStrategy = null!;

    [TestInitialize]
    public void SetUp()
    {
        firstSenderRule = new IncrementingRule();
        secondSenderRule = new RespondingRule();
        firstTargetRuleContext = new Mock<IEventContext>();
        firstTargetRule = new Mock<IRule>();
        moqFirstEventQueue = new Mock<IEventQueue>();
        moqSecondEventQueue = new Mock<IEventQueue>();

        firstTargetRuleContext.SetupGet(ec => ec.RegisteredOn).Returns(moqFirstEventQueue.Object);
        firstTargetRule.SetupGet(r => r.Context).Returns(firstTargetRuleContext.Object);
        firstTargetRule.SetupGet(r => r.LifeCycleState).Returns(RuleLifeCycle.Started);
        moqFirstEventQueue.SetupGet(eq => eq.Name).Returns(FirstQueueName);
        moqFirstEventQueue.Setup(eq => eq.QueueType).Returns(Event);
        moqSecondEventQueue.Setup(eq => eq.QueueType).Returns(Worker);
        firstRouteSelectionResult = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName
            , RoutingFlags.None
            , firstSenderRule);
        secondRouteSelectionResult = new RouteSelectionResult(moqSecondEventQueue.Object, SecondStrategyName
            , DefaultPublish, secondSenderRule);
        firstResultSet = new DispatchSelectionResultSet();
        firstResultSet.MaxUniqueResults = 1;
        firstResultSet.StrategyName = FirstStrategyName;
        firstResultSet.Add(firstRouteSelectionResult);
        secondResultSet = new DispatchSelectionResultSet();
        secondResultSet.MaxUniqueResults = 2;
        secondResultSet.StrategyName = SecondStrategyName;
        secondResultSet.Add(secondRouteSelectionResult);
        targetSpecificSelectionStrategy = new TargetSpecificSelectionStrategy(new Recycler());

        allQueues = new EventQueueGroupContainer(new Mock<IConfigureEventBus>().Object, new[]
        {
            moqFirstEventQueue.Object, moqSecondEventQueue.Object
        });
    }

    [TestMethod]
    public void DispatchTargetSpecificRuleFindsEventQueueInAllQueuesAndReturnsExpectedResult()
    {
        var flags = TargetSpecific;
        var targetSpecificOptions = new DispatchOptions(flags, Event, firstTargetRule.Object);
        var dispatchResultSet = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, targetSpecificOptions
            , FirstDestinationAddress);
        dispatchResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, StrategyName
            , firstTargetRule.Object);
    }

    [TestMethod]
    public void DispatchTargetSpecificRuleThatIsNotStartedReturnsNoResult()
    {
        firstTargetRule.SetupGet(r => r.LifeCycleState).Returns(RuleLifeCycle.NotStarted);
        var flags = TargetSpecific;
        var targetSpecificOptions = new DispatchOptions(flags, Event, firstTargetRule.Object);
        var dispatchResultSet = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, targetSpecificOptions
            , FirstDestinationAddress);
        Assert.IsNull(dispatchResultSet);
    }

    [TestMethod]
    public void DispatchTargetSpecificRuleThatIsOnEventQueueInAvailableQueuesReturnsNoResult()
    {
        firstTargetRuleContext.SetupGet(ec => ec.RegisteredOn).Returns(new Mock<IEventQueue>().Object);
        var flags = TargetSpecific;
        var targetSpecificOptions = new DispatchOptions(flags, Event, firstTargetRule.Object);
        var dispatchResultSet = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, targetSpecificOptions
            , FirstDestinationAddress);
        Assert.IsNull(dispatchResultSet);
    }

    [TestMethod]
    public void DispatchNoSetTargetSpecificRuleReturnsNoResult()
    {
        var flags = TargetSpecific;
        var targetSpecificOptions = new DispatchOptions(flags, Event);
        var dispatchResultSet = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, targetSpecificOptions
            , FirstDestinationAddress);
        Assert.IsNull(dispatchResultSet);
    }

    [TestMethod]
    public void DeployTargetSpecificQueueNameRuleReturnsExpectedResult()
    {
        var flags = TargetSpecific;
        var targetSpecificOptions = new DeploymentOptions(flags, Event, 1, FirstQueueName);
        var routeSelectionResult
            = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, deployRule, targetSpecificOptions);
        routeSelectionResult.AssertIsExpected(Event, moqFirstEventQueue.Object, flags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DeployTargetSpecificQueueNameThatCantBeFoundRuleReturnsNull()
    {
        var flags = TargetSpecific;
        var targetSpecificOptions = new DeploymentOptions(flags, Event, 1, "DoesNotExist");
        var routeSelectionResult
            = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, deployRule, targetSpecificOptions);
        Assert.IsNull(routeSelectionResult);
    }

    [TestMethod]
    public void DeployTargetSpecificWithNoQueueNameReturnsNull()
    {
        var flags = TargetSpecific;
        var targetSpecificOptions = new DeploymentOptions(flags);
        var routeSelectionResult
            = targetSpecificSelectionStrategy.Select(allQueues, firstSenderRule, deployRule, targetSpecificOptions);
        Assert.IsNull(routeSelectionResult);
    }
}
