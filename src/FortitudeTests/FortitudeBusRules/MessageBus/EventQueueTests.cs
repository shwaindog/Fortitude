#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeTests.FortitudeBusRules.Rules;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus;

[TestClass]
public class EventQueueTests
{
    private EventBus eventBus = null!;
    private EventQueue eventQueue = null!;

    [TestInitialize]
    public void Setup()
    {
        eventBus = new EventBus(evtBus =>
        {
            eventQueue = new EventQueue(evtBus, EventQueueType.Event, 1, 12);
            return new List<IEventQueue> { eventQueue };
        });
        eventBus.Start();
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task EventQueueCanLoadNewRuleAndRunStart()
    {
        var incRule = new IncrementingRule();
        var results = eventQueue.LaunchRule(incRule, incRule);
        var incRuleDispatchResult = await results;
        Assert.IsNotNull(results.IsCompleted);
        Assert.AreEqual(1, incRule.StartCount);
        Assert.AreEqual(eventQueue, incRule.Context.RegisteredOn);
        Assert.AreEqual(RuleLifeCycle.Started, incRule.LifeCycleState);
        Assert.AreEqual(1, incRuleDispatchResult.RefCount);
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartingListeningRuleThenPublishingRuleExpectToReceiveSamePublishedMessages()
    {
        var publishRule = new PublishingRule(2);
        var listeningRule = new ListeningRule(publishRule.PublishAddress);
        var listenDeploy = eventQueue.LaunchRule(listeningRule, listeningRule);
        var listenDispatchResult = await listenDeploy;
        Assert.IsTrue(listenDeploy.IsCompleted);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, listenDispatchResult.DecrementRefCount());
        var publishDispatchResult = await eventQueue.LaunchRule(publishRule, publishRule);
        await Task.Delay(45); // time for 2 timer events to complete
        Assert.AreEqual(0, publishDispatchResult.DecrementRefCount());
        Assert.AreEqual(2, publishRule.PublishNumber);
        Assert.AreEqual(2, listeningRule.ReceiveCount);
        Assert.AreEqual(2, listeningRule.LastReceivedPublishNumber);
        var publishRule2 = new PublishingRule(4);
        var publish2DispatchResult = await eventQueue.LaunchRule(publishRule2, publishRule2);
        await Task.Delay(105); // time for 4 timer events to complete
        Assert.AreEqual(0, publish2DispatchResult.DecrementRefCount());
        Assert.AreEqual(4, publishRule2.PublishNumber);
        Assert.AreEqual(6, listeningRule.ReceiveCount);
        Assert.AreEqual(4, listeningRule.LastReceivedPublishNumber);
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDispatchResult = await eventQueue.LaunchRule(respondingRule, respondingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(respondingRule.ListenAddress);
        var requestingDispatchResult = await eventQueue.LaunchRule(requestingRule, requestingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task
        StartResponderThenAsyncValueTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDispatchResult = await eventQueue.LaunchRule(respondingRule, respondingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        var asyncRespondingRule = new AsyncValueTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult = await eventQueue.LaunchRule(asyncRespondingRule, asyncRespondingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult = await eventQueue.LaunchRule(requestingRule, requestingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, asyncRespondingRule.LastReceivedRequestNumber);
        Assert.AreEqual(13, asyncRespondingRule.LastReceivedResponseNumber);
        Assert.AreEqual(3, asyncRespondingRule.ReceiveCount);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartResponderThenAsyncTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDisptachResult = await eventQueue.LaunchRule(respondingRule, respondingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDisptachResult.DecrementRefCount());
        var asyncRespondingRule = new AsyncTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult = await eventQueue.LaunchRule(asyncRespondingRule, asyncRespondingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult = await eventQueue.LaunchRule(requestingRule, requestingRule);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, asyncRespondingRule.LastReceivedRequestNumber);
        Assert.AreEqual(13, asyncRespondingRule.LastReceivedResponseNumber);
        Assert.AreEqual(3, asyncRespondingRule.ReceiveCount);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }
}
