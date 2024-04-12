#region

using FortitudeBusRules.Config;
using FortitudeBusRules.MessageBus;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeTests.FortitudeBusRules.Rules;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Pipelines;

[TestClass]
public class EventQueueTests
{
    private EventBus eventBus = null!;
    private EventQueue eventQueue = null!;
    private RouteSelectionResult selectionResult;

    [TestInitialize]
    public void Setup()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = 12;
        defaultQueuesConfig.EventQueueSize = 12;

        var ring = new AsyncValueValueTaskPollingRing<Message>(
            $"EventQueueTests",
            12,
            () => new Message(),
            ClaimStrategyType.MultiProducers, null, null, false);
        var ringPoller = new AsyncValueTaskRingPoller<Message>(ring, 30);
        eventBus = new EventBus(evtBus =>
        {
            eventQueue = new EventQueue(evtBus, EventQueueType.Event, 1, ringPoller);
            var eventGroupContainer = new EventQueueGroupContainer(evtBus, evBus =>
                new SpecificEventQueueGroup(evtBus, EventQueueType.Event, new Recycler(), defaultQueuesConfig)
                    { eventQueue });
            return eventGroupContainer;
        });
        eventBus.Start();
        selectionResult = new RouteSelectionResult(eventQueue, "EventQueueTests", RoutingFlags.DefaultDeploy);
    }

    [TestCleanup]
    public void TearDown()
    {
        eventBus.Stop();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task EventQueueCanLoadNewRuleAndRunStart()
    {
        var incRule = new IncrementingRule();
        var results = eventQueue.LaunchRuleAsync(incRule, incRule, selectionResult);
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
        var listenDeploy = eventQueue.LaunchRuleAsync(listeningRule, listeningRule, selectionResult);
        var listenDispatchResult = await listenDeploy;
        Assert.IsTrue(listenDeploy.IsCompleted);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, listenDispatchResult.DecrementRefCount());
        var publishDispatchResult = await eventQueue.LaunchRuleAsync(publishRule, publishRule, selectionResult);
        await Task.Delay(45); // time for 2 timer events to complete
        Assert.AreEqual(0, publishDispatchResult.DecrementRefCount());
        Assert.AreEqual(2, publishRule.PublishNumber);
        Assert.AreEqual(2, listeningRule.ReceiveCount);
        Assert.AreEqual(2, listeningRule.LastReceivedPublishNumber);
        var publishRule2 = new PublishingRule(4);
        var publish2DispatchResult = await eventQueue.LaunchRuleAsync(publishRule2, publishRule2, selectionResult);
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
        var respondingDispatchResult
            = await eventQueue.LaunchRuleAsync(respondingRule, respondingRule, selectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(respondingRule.ListenAddress);
        var requestingDispatchResult
            = await eventQueue.LaunchRuleAsync(requestingRule, requestingRule, selectionResult);
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
        var respondingDispatchResult
            = await eventQueue.LaunchRuleAsync(respondingRule, respondingRule, selectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        var asyncRespondingRule = new AsyncValueTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult
            = await eventQueue.LaunchRuleAsync(asyncRespondingRule, asyncRespondingRule, selectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult
            = await eventQueue.LaunchRuleAsync(requestingRule, requestingRule, selectionResult);
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
        var respondingDisptachResult
            = await eventQueue.LaunchRuleAsync(respondingRule, respondingRule, selectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDisptachResult.DecrementRefCount());
        var asyncRespondingRule = new AsyncTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult
            = await eventQueue.LaunchRuleAsync(asyncRespondingRule, asyncRespondingRule, selectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult
            = await eventQueue.LaunchRuleAsync(requestingRule, requestingRule, selectionResult);
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
