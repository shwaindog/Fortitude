#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeTests.FortitudeBusRules.Rules;
using FortitudeTests.FortitudeCommon.Chronometry;
using Moq;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;
using static FortitudeBusRules.BusMessaging.Routing.SelectionStrategies.RoutingFlags;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class DeployDispatchDestinationCacheTests
{
    private const int OneHundred = 100;
    private const int MoreThan100 = OneHundred + 2;
    private const string FirstStrategyName = "FirstSelectionStrategy";
    private const string SecondStrategyName = "SecondSelectionStrategy";
    private const string FirstDestinationAddress = "FirstDestinationAddress";
    private readonly DateTime createResultTime = new(2023, 12, 19, 8, 0, 0);
    private readonly DateTime oneMinOneSecondAfterCreateResultTime = new(2023, 12, 19, 8, 1, 1);
    private DeployDispatchDestinationCache cache = null!;
    private IRule deployRule = null!;
    private IDispatchSelectionResultSet firstResultSet = null!;
    private RouteSelectionResult firstRouteSelectionResult = default;

    private IRule firstSenderRule = null!;
    private Mock<IMessageQueue> moqFirstEventQueue = null!;
    private Mock<IMessageQueue> moqSecondEventQueue = null!;
    private IDispatchSelectionResultSet secondResultSet = null!;
    private RouteSelectionResult secondRouteSelectionResult = default;
    private IRule secondSenderRule = null!;
    private TimeContextTests.StubTimeContext stubTime = null!;

    [TestInitialize]
    public void SetUp()
    {
        stubTime = new TimeContextTests.StubTimeContext
        {
            UtcNow = createResultTime
        };
        TimeContext.Provider = stubTime;
        firstSenderRule = new IncrementingRule();
        secondSenderRule = new RespondingRule();
        deployRule = new IncrementingRule();
        cache = new DeployDispatchDestinationCache();
        moqFirstEventQueue = new Mock<IMessageQueue>();
        moqSecondEventQueue = new Mock<IMessageQueue>();

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

    [TestCleanup]
    public void TearDown()
    {
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    [TestMethod]
    public void SaveDispatchSenderDestinationWithNoExpiryButNotDestinationSavesJustExpectedEntry()
    {
        var flags = SenderCacheLast;
        var onlySenderSaveOptions = new DispatchOptions(flags);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = onlySenderSaveOptions;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, onlySenderSaveOptions, FirstDestinationAddress, firstResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule
                , FirstDestinationAddress
                , onlySenderSaveOptions);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }

        var noRetrieval = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, onlySenderSaveOptions);
        Assert.IsNull(noRetrieval);
    }

    [TestMethod]
    public void SaveDispatchDestinationWithNoExpiryButNotSenderDestinationSavesJustExpectedEntry()
    {
        var flags = DestinationCacheLast;
        var onlyDestinationSaveOptions = new DispatchOptions(flags);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = onlyDestinationSaveOptions;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, onlyDestinationSaveOptions, FirstDestinationAddress, firstResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet
                = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, onlyDestinationSaveOptions);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }

        var noRetrieval = cache.LastSenderDestinationSelectionResultSet(firstSenderRule, FirstDestinationAddress
            , onlyDestinationSaveOptions);
        Assert.IsNull(noRetrieval);
    }

    [TestMethod]
    public void SaveDispatchBothDestinationAndSendDestinationWithNoExpirySavesBoth()
    {
        var flags = DestinationCacheLast | SenderCacheLast;
        var saveBothNoExpiryOptions = new DispatchOptions(flags);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = saveBothNoExpiryOptions;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, saveBothNoExpiryOptions, FirstDestinationAddress, firstResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule
                , FirstDestinationAddress
                , saveBothNoExpiryOptions);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);

            selectionResultSet
                = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, saveBothNoExpiryOptions);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }
    }

    [TestMethod]
    public void SaveDispatchBothDestinationAndSendDestinationWith100ExpirySavesBothUntilRetrievedMoreThan100Times()
    {
        var flags = DestinationCacheLast | SenderCacheLast | ExpireCacheAfter100Reads;
        var senderCacheWith100Expiry = new DispatchOptions(flags);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = senderCacheWith100Expiry;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, senderCacheWith100Expiry, FirstDestinationAddress, firstResultSet);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < OneHundred; i++)
        {
            var selectionResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule
                , FirstDestinationAddress
                , senderCacheWith100Expiry);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);

            selectionResultSet
                = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, senderCacheWith100Expiry);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }

        var noRetrieval = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, senderCacheWith100Expiry);
        Assert.IsNull(noRetrieval);
        noRetrieval = cache.LastSenderDestinationSelectionResultSet(firstSenderRule, FirstDestinationAddress
            , senderCacheWith100Expiry);
        Assert.IsNull(noRetrieval);
    }

    [TestMethod]
    public void SaveDispatchBothDestinationAndSendDestinationWith1MinExpirySavesBothUntilTimeExpires()
    {
        var flags = DestinationCacheLast | SenderCacheLast | ExpireCacheAfterAMinute;
        var senderCacheWith1MinExpiry = new DispatchOptions(flags);
        firstResultSet.Clear();
        firstResultSet.DispatchOptions = senderCacheWith1MinExpiry;
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        firstResultSet.Add(firstRouteSelectionResult);
        cache.Save(firstSenderRule, senderCacheWith1MinExpiry, FirstDestinationAddress, firstResultSet);

        for (var i = 0; i < MoreThan100; i++)
        {
            var selectionResultSet = cache.LastSenderDestinationSelectionResultSet(firstSenderRule
                , FirstDestinationAddress
                , senderCacheWith1MinExpiry);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);

            selectionResultSet
                = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, senderCacheWith1MinExpiry);
            selectionResultSet.AssertSingleResultIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;

        var noRetrieval = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, senderCacheWith1MinExpiry);
        Assert.IsNull(noRetrieval);
        noRetrieval = cache.LastSenderDestinationSelectionResultSet(firstSenderRule, FirstDestinationAddress
            , senderCacheWith1MinExpiry);
        Assert.IsNull(noRetrieval);
    }


    [TestMethod]
    public void SaveDeployDestinationWithNoExpiryButNotCacheSenderDestinationSavesJustExpectedEntry()
    {
        var flags = DestinationCacheLast;
        var onlySenderSaveOptions = new DeploymentOptions(flags);
        firstRouteSelectionResult
            = new RouteSelectionResult(moqFirstEventQueue.Object, FirstStrategyName, flags, firstSenderRule);
        cache.Save(firstSenderRule, deployRule, onlySenderSaveOptions, firstRouteSelectionResult);

        stubTime.UtcNow = oneMinOneSecondAfterCreateResultTime;
        for (var i = 0; i < MoreThan100; i++)
        {
            var routeSelectionResult = cache.LastDeploySelectionResult(Event, onlySenderSaveOptions);

            routeSelectionResult.AssertIsExpected(Event, moqFirstEventQueue.Object, flags, FirstStrategyName
                , firstSenderRule);
        }

        var onlySenderSaveDispatchOptions = new DispatchOptions(flags);
        var noRetrieval
            = cache.LastDestinationSelectionResultSet(FirstDestinationAddress, onlySenderSaveDispatchOptions);
        Assert.IsNull(noRetrieval);
    }
}
