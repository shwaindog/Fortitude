#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public static class SelectionStrategyTestHelpers
{
    public static void SetupEventQueueMock(this Mock<IMessageQueue> subject, MessageQueueType queueType, int id
        , bool isListeningToDestination
        , Action<ISet<IRule>, string> listeningRules, int compareResult = 0)
    {
        subject.SetupGet(eq => eq.QueueType).Returns(queueType);
        subject.SetupGet(eq => eq.Id).Returns(id);
        subject.Setup(eq => eq.IsListeningToAddress(It.IsAny<string>())).Returns(isListeningToDestination);
        subject.Setup(eq => eq.RulesListeningToAddress(It.IsAny<ISet<IRule>>(), It.IsAny<string>()))
            .Callback(listeningRules);
        subject.Setup(eq => eq.CompareTo(It.IsAny<IMessageQueue>())).Returns(compareResult);
    }

    public static void SetupEventQueueMock(this Mock<IIOOutboundMessageQueue> subject, MessageQueueType queueType, int id
        , bool isListeningToDestination
        , Action<ISet<IRule>, string> listeningRules, int compareResult = 0)
    {
        subject.SetupGet(eq => eq.QueueType).Returns(queueType);
        subject.SetupGet(eq => eq.Id).Returns(id);
        subject.Setup(eq => eq.IsListeningToAddress(It.IsAny<string>())).Returns(isListeningToDestination);
        subject.Setup(eq => eq.RulesListeningToAddress(It.IsAny<ISet<IRule>>(), It.IsAny<string>()))
            .Callback(listeningRules);
        subject.Setup(eq => eq.CompareTo(It.IsAny<IMessageQueue>())).Returns(compareResult);
    }

    public static void SetupEventQueueMock(this Mock<IIOInboundMessageQueue> subject, MessageQueueType queueType, int id
        , bool isListeningToDestination
        , Action<ISet<IRule>, string> listeningRules, int compareResult = 0)
    {
        subject.SetupGet(eq => eq.QueueType).Returns(queueType);
        subject.SetupGet(eq => eq.Id).Returns(id);
        subject.Setup(eq => eq.IsListeningToAddress(It.IsAny<string>())).Returns(isListeningToDestination);
        subject.Setup(eq => eq.RulesListeningToAddress(It.IsAny<ISet<IRule>>(), It.IsAny<string>()))
            .Callback(listeningRules);
        subject.Setup(eq => eq.CompareTo(It.IsAny<IMessageQueue>())).Returns(compareResult);
    }

    public static void AssertIsExpected(this RouteSelectionResult? toCheck, MessageQueueType expectedQueueType
        , IMessageQueue expectedMessageQueue, RoutingFlags expectedRoutingFlags, string expectedStrategyName
        , IRule expectedRule)
    {
        Assert.IsNotNull(toCheck);
        toCheck.Value.AssertIsExpected(expectedQueueType, expectedMessageQueue, expectedRoutingFlags, expectedStrategyName
            , expectedRule);
    }

    public static void AssertIsExpected(this RouteSelectionResult toCheck, MessageQueueType expectedQueueType
        , IMessageQueue expectedMessageQueue, RoutingFlags expectedRoutingFlags, string expectedStrategyName
        , IRule? expectedRule = null)
    {
        Assert.AreEqual(expectedQueueType, toCheck.MessageQueue.QueueType);
        Assert.AreSame(expectedMessageQueue, toCheck.MessageQueue);

        if (expectedRule != null)
        {
            Assert.IsNotNull(toCheck.Rule);
            Assert.AreEqual(expectedRule, toCheck.Rule);
        }
        else
        {
            Assert.IsNull(toCheck.Rule);
        }

        Assert.AreEqual(expectedStrategyName, toCheck.StrategyName);
        Assert.AreEqual(expectedRoutingFlags, toCheck.RoutingFlags);
    }

    public static void AssertSingleResultIsExpected(this IDispatchSelectionResultSet? toCheck
        , MessageQueueType expectedQueueType
        , IMessageQueue expectedMessageQueue, RoutingFlags expectedRoutingFlags, string expectedStrategyName
        , IRule? expectedRule = null)
    {
        Assert.IsNotNull(toCheck);
        Assert.IsTrue(toCheck.HasItems);
        Assert.AreEqual(1, toCheck.Count);
        var result = toCheck.First();
        Assert.AreEqual(expectedQueueType, result.MessageQueue.QueueType);
        Assert.AreSame(expectedMessageQueue, result.MessageQueue);
        if (expectedRule != null)
        {
            Assert.IsNotNull(result.Rule);
            Assert.AreEqual(expectedRule, result.Rule);
        }
        else
        {
            Assert.IsNull(result.Rule);
        }

        Assert.AreEqual(expectedStrategyName, toCheck.StrategyName);
        Assert.AreEqual(expectedRoutingFlags, toCheck.DispatchOptions.RoutingFlags);
    }
}
