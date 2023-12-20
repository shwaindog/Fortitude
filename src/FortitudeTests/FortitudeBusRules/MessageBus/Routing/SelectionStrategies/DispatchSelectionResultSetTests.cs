#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class DispatchSelectionResultSetTests
{
    private DispatchSelectionResultSet dispatchSelectionResultSet = null!;
    private Mock<IEventQueue> firstEventQueue = null!;
    private Mock<IEventQueue> secondEventQueue = null!;


    [TestInitialize]
    public void Setup()
    {
        firstEventQueue = new Mock<IEventQueue>();
        secondEventQueue = new Mock<IEventQueue>();


        firstEventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Event);
        secondEventQueue.SetupGet(eq => eq.QueueType).Returns(EventQueueType.Worker);

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
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().EventQueue);
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
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().EventQueue);
    }
}
