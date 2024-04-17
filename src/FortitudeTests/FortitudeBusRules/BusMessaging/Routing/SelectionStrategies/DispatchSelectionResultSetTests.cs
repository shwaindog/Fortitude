#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

[TestClass]
public class DispatchSelectionResultSetTests
{
    private DispatchSelectionResultSet dispatchSelectionResultSet = null!;
    private Mock<IMessageQueue> firstEventQueue = null!;
    private Mock<IMessageQueue> secondEventQueue = null!;


    [TestInitialize]
    public void Setup()
    {
        firstEventQueue = new Mock<IMessageQueue>();
        secondEventQueue = new Mock<IMessageQueue>();


        firstEventQueue.SetupGet(eq => eq.QueueType).Returns(MessageQueueType.Event);
        secondEventQueue.SetupGet(eq => eq.QueueType).Returns(MessageQueueType.Worker);

        dispatchSelectionResultSet = new DispatchSelectionResultSet();
    }


    [TestMethod]
    public void OnlyUniqueEventQueuesAreSelected()
    {
        dispatchSelectionResultSet.MaxUniqueResults = 2;
        dispatchSelectionResultSet.Add(new RouteSelectionResult(firstEventQueue.Object, "Something"
            , RoutingFlags.DefaultPublish));
        dispatchSelectionResultSet.Add(new RouteSelectionResult(firstEventQueue.Object, "Something"
            , RoutingFlags.DefaultPublish));

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsFalse(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
    }

    [TestMethod]
    public void OnlyAcceptsUpToMaxUniqueResults()
    {
        dispatchSelectionResultSet.MaxUniqueResults = 1;
        dispatchSelectionResultSet.Add(new RouteSelectionResult(firstEventQueue.Object, "Something"
            , RoutingFlags.DefaultPublish));
        dispatchSelectionResultSet.Add(new RouteSelectionResult(secondEventQueue.Object, "SomethingElse"
            , RoutingFlags.DefaultPublish));

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsTrue(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().MessageQueue);
    }

    [TestMethod]
    public void AddRangeOnlyAcceptsUpToMaxUniqueResults()
    {
        dispatchSelectionResultSet.MaxUniqueResults = 1;

        RouteSelectionResult[] selectionResults =
        {
            new(firstEventQueue.Object, "Something"
                , RoutingFlags.DefaultPublish)
            , new(secondEventQueue.Object, "SomethingElse"
                , RoutingFlags.DefaultPublish)
        };

        dispatchSelectionResultSet.AddRange(selectionResults);

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsTrue(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().MessageQueue);
    }
}
