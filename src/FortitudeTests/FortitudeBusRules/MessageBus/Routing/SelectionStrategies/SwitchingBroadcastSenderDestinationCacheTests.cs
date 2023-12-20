#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;
using static FortitudeBusRules.MessageBus.Pipelines.EventQueueType;
using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlags;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class SwitchingBroadcastSenderDestinationCacheTests
{
    private const string FirstStrategyName = "FirstSelectionStrategy";
    private const string SecondStrategyName = "SecondSelectionStrategy";
    private const string FirstDestinationAddress = "FirstDestinationAddress";
    private SwitchingBroadcastSenderDestinationCache cache = null!;
    private IDispatchSelectionResultSet firstResultSet = null!;
    private RouteSelectionResult firstRouteSelectionResult;

    private IRule firstSenderRule = null!;
    private Mock<IEventQueue> moqFirstEventQueue = null!;
    private Mock<IEventQueue> moqSecondEventQueue = null!;
    private IDispatchSelectionResultSet secondResultSet = null!;
    private RouteSelectionResult secondRouteSelectionResult;
    private IRule secondSenderRule = null!;

    [TestInitialize]
    public void SetUp()
    {
        firstSenderRule = new IncrementingRule();
        secondSenderRule = new RespondingRule();
        cache = new SwitchingBroadcastSenderDestinationCache();
        moqFirstEventQueue = new Mock<IEventQueue>();
        moqSecondEventQueue = new Mock<IEventQueue>();

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
    }

    [TestMethod]
    public void DefaultPublishAllResultsDoesNotAffectDefaultRequestResponseWithSenderCacheLastForTheSameDestination()
    {
        var requestResponseFlags = DefaultRequestResponse | DestinationCacheLast;
        var requestResponseOptions = new DispatchOptions(requestResponseFlags, Event);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = requestResponseOptions;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, requestResponseFlags
                , firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, requestResponseOptions, FirstDestinationAddress, firstResultSet);

        var publishFlags = DefaultPublish | SenderCacheLast;
        var publishOptions = new DispatchOptions(publishFlags, Worker);
        secondResultSet.Clear();
        secondResultSet.DispatchOptions = publishOptions;
        secondRouteSelectionResult
            = new RouteSelectionResult(moqSecondEventQueue.Object, SecondStrategyName, publishFlags, secondSenderRule);
        secondResultSet.Add(secondRouteSelectionResult);
        cache.Save(firstSenderRule, publishOptions, FirstDestinationAddress, secondResultSet);

        var requestResponseResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule
            , FirstDestinationAddress
            , requestResponseOptions);
        requestResponseResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, requestResponseFlags
            , FirstStrategyName
            , firstSenderRule);

        var publishResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule, FirstDestinationAddress
            , publishOptions);
        publishResultSet.AssertSingleResultIsExpected(Worker, moqSecondEventQueue.Object, publishFlags
            , SecondStrategyName
            , secondSenderRule);

        requestResponseResultSet = cache.LastDestinationSelectionResultSet(FirstDestinationAddress
            , requestResponseOptions);
        requestResponseResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, requestResponseFlags
            , FirstStrategyName
            , firstSenderRule);

        publishResultSet = cache.LastDestinationSelectionResultSet(FirstDestinationAddress
            , publishOptions);
        publishResultSet.AssertSingleResultIsExpected(Worker, moqSecondEventQueue.Object, publishFlags
            , SecondStrategyName
            , secondSenderRule);
    }
}
