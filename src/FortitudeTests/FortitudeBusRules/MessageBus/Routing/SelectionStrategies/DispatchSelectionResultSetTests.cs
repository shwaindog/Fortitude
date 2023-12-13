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
        dispatchSelectionResultSet.Add(new SelectionResult(firstEventQueue.Object, "Something"
            , RoutingStrategySelectionFlags.DefaultPublish));
        dispatchSelectionResultSet.Add(new SelectionResult(firstEventQueue.Object, "Something"
            , RoutingStrategySelectionFlags.DefaultPublish));

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsFalse(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
    }

    [TestMethod]
    public void OnlyAcceptsUpToMaxUniqueResults()
    {
        dispatchSelectionResultSet.MaxUniqueResults = 1;
        dispatchSelectionResultSet.Add(new SelectionResult(firstEventQueue.Object, "Something"
            , RoutingStrategySelectionFlags.DefaultPublish));
        dispatchSelectionResultSet.Add(new SelectionResult(secondEventQueue.Object, "SomethingElse"
            , RoutingStrategySelectionFlags.DefaultPublish));

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsTrue(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().EventQueue);
    }

    [TestMethod]
    public void AddRangeOnlyAcceptsUpToMaxUniqueResults()
    {
        dispatchSelectionResultSet.MaxUniqueResults = 1;

        SelectionResult[] selectionResults =
        {
            new(firstEventQueue.Object, "Something"
                , RoutingStrategySelectionFlags.DefaultPublish)
            , new(secondEventQueue.Object, "SomethingElse"
                , RoutingStrategySelectionFlags.DefaultPublish)
        };

        dispatchSelectionResultSet.AddRange(selectionResults);

        Assert.IsTrue(dispatchSelectionResultSet.HasItems);
        Assert.IsTrue(dispatchSelectionResultSet.HasFinished);
        Assert.AreEqual(1, dispatchSelectionResultSet.Count);
        Assert.AreEqual(firstEventQueue.Object, dispatchSelectionResultSet.First().EventQueue);
    }
}
