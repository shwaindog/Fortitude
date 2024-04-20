#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeBusRules.Rules;
using FortitudeTests.FortitudeCommon.Chronometry;
using Moq;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class ReuseLastCachedResultSelectionStrategyTests
{
    private const int OneHundred = 100;
    private const int MoreThan100 = OneHundred + 2;
    private const string StrategyName = ReuseLastCachedResultSelectionStrategy.StrategyName;
    private const string InitialInsertStrategyName = "SomeCreationStrategyName";
    private const string AnotherStrategyName = "SomeOtherStrategyName";
    private const string DestinationAddress = "SomeAddress";
    private const string AnotherDestinationAddress = "SomeOtherAddress";
    private readonly DateTime createResultTime = new(2023, 12, 19, 8, 0, 0);
    private readonly IRule deployRule = new ListeningRule();
    private readonly IRule differentDeployRule = new RespondingRule();
    private readonly IRule differentSendingRule = new IncrementingRule();
    private readonly DateTime oneMinOneSecondAfterCreateResultTime = new(2023, 12, 19, 8, 1, 1);
    private readonly IRule resultRule = new IncrementingRule();
    private readonly IRule sendingRule = new ListeningRule();
    private Mock<IMessageQueueGroupContainer> allQueues = null!;
    private DeployDispatchDestinationCache deployDispatchDestinationCache = null!;
    private IDispatchSelectionResultSet differentDispatchSelectionResultSet = null!;
    private RouteSelectionResult differentRouteSelectionResult;
    private IDispatchSelectionResultSet firstDispatchSelectionResultSet = null!;
    private Mock<IMessageQueue> firstQueue = null!;
    private RouteSelectionResult initialRouteSelectionResult;

    private ReuseLastCachedResultSelectionStrategy reuseLastCachedResultSelectionStrategy = null!;
    private Mock<IMessageQueue> secondQueue = null!;
    private TimeContextTests.StubTimeContext stubTime = null!;

    [TestInitialize]
    public void Setup()
    {
        stubTime = new TimeContextTests.StubTimeContext
        {
            UtcNow = createResultTime
        };
        TimeContext.Provider = stubTime;

        firstQueue = new Mock<IMessageQueue>();
        secondQueue = new Mock<IMessageQueue>();
        allQueues = new Mock<IMessageQueueGroupContainer>();

        firstDispatchSelectionResultSet = new DispatchSelectionResultSet()
        {
            MaxUniqueResults = 1
        };
        initialRouteSelectionResult = new RouteSelectionResult(firstQueue.Object, InitialInsertStrategyName
            , RoutingFlags.None, resultRule);
        firstDispatchSelectionResultSet.Add(initialRouteSelectionResult);
        differentRouteSelectionResult = new RouteSelectionResult(secondQueue.Object, AnotherStrategyName
            , RoutingFlags.None, resultRule);
        differentDispatchSelectionResultSet = new DispatchSelectionResultSet()
        {
            MaxUniqueResults = 1
        };
        differentDispatchSelectionResultSet.Add(differentRouteSelectionResult);

        deployDispatchDestinationCache = new DeployDispatchDestinationCache();
        reuseLastCachedResultSelectionStrategy
            = new ReuseLastCachedResultSelectionStrategy(deployDispatchDestinationCache);
    }

    [TestCleanup]
    public void TearDown()
    {
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    [TestMethod]
    public void DispatchDefaultPublishPreviouslyCachedResultIsReturnedUntilExpiredAfter100Requests()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);

        for (var i = 0; i < OneHundred; i++)
        {
            var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , dispatchOptions, DestinationAddress);
            selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, resultRule);
        }

        var noSelectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        Assert.IsNull(noSelectionResultSet);
    }

    [TestMethod]
    public void DispatchDefaultPublishPreviouslyCachedResultIsReturnedUntilExpiredAfter1Min()
    {
        var routingFlags = RoutingFlags.DefaultPublish;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);

        for (var i = 0; i < OneHundred / 2; i++)
        {
            var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , dispatchOptions, DestinationAddress);
            selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, resultRule);
        }

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        var noSelectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        Assert.IsNull(noSelectionResultSet);
    }

    [TestMethod]
    public void DispatchPublishNoExpiryPreviouslyCachedResultIsReturnedIndefinitely()
    {
        var routingFlags = RoutingFlags.SendToAll | RoutingFlags.DestinationCacheLast | RoutingFlags.UseLastCacheEntry;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , dispatchOptions, DestinationAddress);
            selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, resultRule);
        }
    }

    [TestMethod]
    public void DispatchPublishNoExpiryPreviouslyCachedResultIsReturnedUnlessRecalculateCacheIsSent()
    {
        var routingFlags = RoutingFlags.SendToAll | RoutingFlags.DestinationCacheLast | RoutingFlags.UseLastCacheEntry;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < OneHundred / 2; i++)
        {
            var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , dispatchOptions, DestinationAddress);
            selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, resultRule);
        }

        var recalculateCacheFlags = RoutingFlags.RecalculateCache | RoutingFlags.SendToAll |
                                    RoutingFlags.DestinationCacheLast | RoutingFlags.UseLastCacheEntry;

        var noResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , new DispatchOptions(recalculateCacheFlags, eventGroupType), DestinationAddress);
        Assert.IsNull(noResultSet);
    }

    [TestMethod]
    public void DispatchDefaultRequestResponseWithDifferingSendersRetainsCachedResultForEachSender()
    {
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
            , routingFlags, StrategyName, resultRule);

        secondQueue.SetupGet(eq => eq.QueueType).Returns(Worker);
        differentRouteSelectionResult = new RouteSelectionResult(secondQueue.Object, AnotherStrategyName, routingFlags
            , differentDeployRule);
        differentDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        differentDispatchSelectionResultSet.Clear();
        differentDispatchSelectionResultSet.Add(differentRouteSelectionResult);
        deployDispatchDestinationCache.Save(differentSendingRule, dispatchOptions, DestinationAddress
            , differentDispatchSelectionResultSet);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, differentSendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(Worker, secondQueue.Object
            , routingFlags, StrategyName, differentDeployRule);

        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
            , routingFlags, StrategyName, resultRule);
    }

    [TestMethod]
    public void DispatchDefaultRequestResponseDiffDestinationAddressCachesResultForEachDestination()
    {
        var routingFlags = RoutingFlags.DefaultRequestResponse;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var dispatchOptions = new DispatchOptions(routingFlags, eventGroupType);
        firstDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, DestinationAddress
            , firstDispatchSelectionResultSet);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
            , routingFlags, StrategyName, resultRule);

        secondQueue.SetupGet(eq => eq.QueueType).Returns(Worker);
        differentRouteSelectionResult = new RouteSelectionResult(secondQueue.Object, AnotherStrategyName, routingFlags
            , differentDeployRule);
        differentDispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        differentDispatchSelectionResultSet.Clear();
        differentDispatchSelectionResultSet.Add(differentRouteSelectionResult);
        deployDispatchDestinationCache.Save(sendingRule, dispatchOptions, AnotherDestinationAddress
            , differentDispatchSelectionResultSet);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, AnotherDestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(Worker, secondQueue.Object
            , routingFlags, StrategyName, differentDeployRule);

        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , dispatchOptions, DestinationAddress);
        selectionResultSet.AssertSingleResultIsExpected(eventGroupType, firstQueue.Object
            , routingFlags, StrategyName, resultRule);
    }

    [TestMethod]
    public void DefaultDeployPreviouslyCachedResultIsReturnedIndefinitely()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);

        initialRouteSelectionResult
            = new RouteSelectionResult(firstQueue.Object, InitialInsertStrategyName, routingFlags, resultRule);

        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, initialRouteSelectionResult);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , deployRule, deploymentOptions);
            selectionResultSet.AssertIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, deployRule);
        }
    }

    [TestMethod]
    public void DefaultDeployWithDifferingQueuesGroupsRetainPreviousCacheStatus()
    {
        var routingFlags = RoutingFlags.DefaultDeploy;
        var eventGroupType = Event | Worker;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(Worker);
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);

        initialRouteSelectionResult
            = new RouteSelectionResult(firstQueue.Object, InitialInsertStrategyName, routingFlags, resultRule);

        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, initialRouteSelectionResult);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        var selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , deployRule, deploymentOptions);
        selectionResultSet.AssertIsExpected(Worker, firstQueue.Object, routingFlags, StrategyName, deployRule);


        var justEventGroupType = Event;
        secondQueue.SetupGet(eq => eq.QueueType).Returns(Event);
        var justEventDeploymentOps = new DeploymentOptions(routingFlags, justEventGroupType);

        var justEventRoute
            = new RouteSelectionResult(secondQueue.Object, AnotherStrategyName, routingFlags, resultRule);

        deployDispatchDestinationCache.Save(sendingRule, differentDeployRule, justEventDeploymentOps, justEventRoute);
        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , differentDeployRule, justEventDeploymentOps);
        selectionResultSet.AssertIsExpected(Event, secondQueue.Object, routingFlags, StrategyName
            , differentDeployRule);

        selectionResultSet = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , deployRule, deploymentOptions);
        selectionResultSet.AssertIsExpected(Worker, firstQueue.Object, routingFlags, StrategyName, deployRule);
    }

    [TestMethod]
    public void DeployWithExpiryAfter100ReadsPreviouslyCachedResultIsReturnedUntilItExpires()
    {
        var routingFlags = RoutingFlags.DefaultDeploy | RoutingFlags.ExpireCacheAfter100Reads;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);

        initialRouteSelectionResult
            = new RouteSelectionResult(firstQueue.Object, InitialInsertStrategyName, routingFlags, resultRule);

        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, initialRouteSelectionResult);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < OneHundred; i++)
        {
            var routingSelectionResult = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , deployRule, deploymentOptions);
            routingSelectionResult.AssertIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, deployRule);
        }

        var noSelectionResult = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , deployRule, deploymentOptions);
        Assert.IsNull(noSelectionResult);
    }

    [TestMethod]
    public void DeployWithExpiryAfter1MinPreviouslyCachedResultIsReturnedUntilItExpires()
    {
        var routingFlags = RoutingFlags.DefaultDeploy | RoutingFlags.ExpireCacheAfterAMinute;
        var eventGroupType = Event;
        firstQueue.SetupGet(eq => eq.QueueType).Returns(eventGroupType);
        var deploymentOptions = new DeploymentOptions(routingFlags, eventGroupType);

        initialRouteSelectionResult
            = new RouteSelectionResult(firstQueue.Object, InitialInsertStrategyName, routingFlags, resultRule);

        deployDispatchDestinationCache.Save(sendingRule, deployRule, deploymentOptions, initialRouteSelectionResult);

        for (var i = 0; i < OneHundred / 2; i++)
        {
            var routingSelectionResult = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
                , deployRule, deploymentOptions);
            routingSelectionResult.AssertIsExpected(eventGroupType, firstQueue.Object
                , routingFlags, StrategyName, deployRule);
        }

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;

        var noSelectionResult = reuseLastCachedResultSelectionStrategy.Select(allQueues.Object, sendingRule
            , deployRule, deploymentOptions);
        Assert.IsNull(noSelectionResult);
    }
}
