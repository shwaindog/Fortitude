// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeTests.FortitudeBusRules.Rules;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines;

[TestClass]
public class MessageQueueTests : SingleEventQueueTestSetup
{
    [TestCleanup]
    public void TearDown()
    {
        MessageBus.Stop();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task EventQueueCanLoadNewRuleAndRunStart()
    {
        var incRule = new IncrementingRule();
        var results = EventQueue.LaunchRuleAsync(incRule, incRule, EventQueueSelectionResult);

        var incRuleDeploymentLifeTime = await results;
        Assert.AreEqual(1, incRule.StartCount);
        Assert.AreEqual(EventQueue, incRule.Context.RegisteredOn);
        Assert.AreEqual(RuleLifeCycle.Started, incRule.LifeCycleState);
        Assert.IsTrue(incRuleDeploymentLifeTime.RefCount is >= 1 and <= 2);
        Assert.IsFalse(incRuleDeploymentLifeTime.IsInRecycler);
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartingListeningRuleThenPublishingRuleExpectToReceiveSamePublishedMessages()
    {
        var publishRule   = new PublishingRule(2);
        var listeningRule = new ListeningRule(publishRule.PublishAddress);
        var listenDeploy  = EventQueue.LaunchRuleAsync(listeningRule, listeningRule, EventQueueSelectionResult);

        var listenDispatchResult = await listenDeploy;
        await Task.Delay(50); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, listenDispatchResult.DecrementRefCount());
        Assert.IsTrue(listenDispatchResult.IsInRecycler);
        var publishDispatchResult = await EventQueue.LaunchRuleAsync(publishRule, publishRule, EventQueueSelectionResult);
        await Task.Delay(45); // time for 2 timer events to complete
        Assert.AreEqual(0, publishDispatchResult.DecrementRefCount());
        Assert.IsTrue(publishDispatchResult.IsInRecycler);
        Assert.AreEqual(2, publishRule.PublishNumber);
        Assert.AreEqual(2, listeningRule.ReceiveCount);
        Assert.AreEqual(2, listeningRule.LastReceivedPublishNumber);
        var publishRule2           = new PublishingRule(4);
        var publish2DispatchResult = await EventQueue.LaunchRuleAsync(publishRule2, publishRule2, EventQueueSelectionResult);
        await Task.Delay(125); // time for 4 timer events to complete
        Assert.AreEqual(0, publish2DispatchResult.DecrementRefCount());
        Assert.IsTrue(publish2DispatchResult.IsInRecycler);
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
            = await EventQueue.LaunchRuleAsync(respondingRule, respondingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        Assert.IsTrue(respondingDispatchResult.IsInRecycler);
        var requestingRule = new RequestingRule(respondingRule.ListenAddress);
        var requestingDispatchResult
            = await EventQueue.LaunchRuleAsync(requestingRule, requestingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.IsTrue(requestingDispatchResult.IsInRecycler);
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
            = await EventQueue.LaunchRuleAsync(respondingRule, respondingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDispatchResult.DecrementRefCount());
        Assert.IsTrue(respondingDispatchResult.IsInRecycler);
        var asyncRespondingRule = new AsyncValueTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult
            = await EventQueue.LaunchRuleAsync(asyncRespondingRule, asyncRespondingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        Assert.IsTrue(asyncRespondingDispatchResult.IsInRecycler);
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult
            = await EventQueue.LaunchRuleAsync(requestingRule, requestingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.IsTrue(requestingDispatchResult.IsInRecycler);
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
            = await EventQueue.LaunchRuleAsync(respondingRule, respondingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, respondingDisptachResult.DecrementRefCount());
        Assert.IsTrue(respondingDisptachResult.IsInRecycler);
        var asyncRespondingRule = new AsyncTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDispatchResult
            = await EventQueue.LaunchRuleAsync(asyncRespondingRule, asyncRespondingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, asyncRespondingDispatchResult.DecrementRefCount());
        Assert.IsTrue(asyncRespondingDispatchResult.IsInRecycler);
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDispatchResult
            = await EventQueue.LaunchRuleAsync(requestingRule, requestingRule, EventQueueSelectionResult);
        await Task.Delay(10); // time for message queue to decrement at end of processing message
        Assert.AreEqual(0, requestingDispatchResult.DecrementRefCount());
        Assert.IsTrue(requestingDispatchResult.IsInRecycler);
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, asyncRespondingRule.LastReceivedRequestNumber);
        Assert.AreEqual(13, asyncRespondingRule.LastReceivedResponseNumber);
        Assert.AreEqual(3, asyncRespondingRule.ReceiveCount);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }
}
