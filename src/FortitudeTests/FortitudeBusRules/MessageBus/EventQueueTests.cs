#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
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
    }

    [TestMethod]
    public void EventQueueCanLoadNewRuleAndRunStart()
    {
        var incRule = new IncrementingRule();
        var results = eventQueue.LaunchRule(incRule, incRule);
        var asTask = results.ToTask();
        asTask.Wait(2_000);
        Assert.IsNotNull(results.IsCompleted);
        Assert.AreEqual(1, incRule.StartCount);
        Assert.AreEqual(eventQueue, incRule.Context.RegisteredOn);
        Assert.AreEqual(RuleLifeCycle.Started, incRule.LifeCycleState);
    }

    [TestMethod]
    public void StartingListeningRuleThenPublishingRuleExpectToReceiveSamePublishedMessages()
    {
        var publishRule = new PublishingRule(2);
        var listeningRule = new ListeningRule(publishRule.PublishAddress);
        var listenDeploy = eventQueue.LaunchRule(listeningRule, listeningRule);
        var listenDeployTask = listenDeploy.ToTask();
        listenDeployTask.Wait(2_000);
        Assert.IsTrue(listenDeploy.IsCompleted);
        Thread.Sleep(10);
        Assert.AreEqual(0, listenDeploy.Result.DecrementRefCount());
        var publishDeployVTask = eventQueue.LaunchRule(publishRule, publishRule);
        var publishRuleTask = publishDeployVTask.ToTask();
        publishRuleTask.Wait(2_000);
        Assert.IsTrue(publishDeployVTask.IsCompleted);
        Thread.Sleep(45);
        Assert.AreEqual(0, publishDeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(2, publishRule.PublishNumber);
        Assert.AreEqual(2, listeningRule.ReceiveCount);
        Assert.AreEqual(2, listeningRule.LastReceivedPublishNumber);
        var publishRule2 = new PublishingRule(4);
        var publish2DeployVTask = eventQueue.LaunchRule(publishRule2, publishRule2);
        var publish2RuleTask = publish2DeployVTask.ToTask();
        publish2RuleTask.Wait(2_000);
        Assert.IsTrue(publish2DeployVTask.IsCompleted);
        Thread.Sleep(105);
        Assert.AreEqual(0, publish2DeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(4, publishRule2.PublishNumber);
        Assert.AreEqual(6, listeningRule.ReceiveCount);
        Assert.AreEqual(4, listeningRule.LastReceivedPublishNumber);
    }

    [TestMethod]
    public void StartResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDeploy = eventQueue.LaunchRule(respondingRule, respondingRule);
        var respondingDeployTask = respondingDeploy.ToTask();
        respondingDeployTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.AreEqual(0, respondingDeploy.Result.DecrementRefCount());
        var requestingRule = new RequestingRule(respondingRule.ListenAddress);
        var requestingDeployVTask = eventQueue.LaunchRule(requestingRule, requestingRule);
        var requestingRuleTask = requestingDeployVTask.ToTask();
        requestingRuleTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.IsTrue(requestingDeployVTask.IsCompleted);
        Assert.AreEqual(0, requestingDeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }

    [TestMethod]
    public void StartResponderThenAsyncValueTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDeploy = eventQueue.LaunchRule(respondingRule, respondingRule);
        var respondingDeployTask = respondingDeploy.ToTask();
        respondingDeployTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.AreEqual(0, respondingDeploy.Result.DecrementRefCount());
        var asyncRespondingRule = new AsyncValueTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDeploy = eventQueue.LaunchRule(asyncRespondingRule, asyncRespondingRule);
        var asyncRespondingDeployTask = asyncRespondingDeploy.ToTask();
        asyncRespondingDeployTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.AreEqual(0, asyncRespondingDeploy.Result.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDeployVTask = eventQueue.LaunchRule(requestingRule, requestingRule);
        var requestingRuleTask = requestingDeployVTask.ToTask();
        requestingRuleTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.IsTrue(requestingDeployVTask.IsCompleted);
        Assert.AreEqual(0, requestingDeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, asyncRespondingRule.LastReceivedRequestNumber);
        Assert.AreEqual(13, asyncRespondingRule.LastReceivedResponseNumber);
        Assert.AreEqual(3, asyncRespondingRule.ReceiveCount);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }

    [TestMethod]
    public void StartResponderThenAsyncTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations()
    {
        var respondingRule = new RespondingRule();
        var respondingDeploy = eventQueue.LaunchRule(respondingRule, respondingRule);
        var respondingDeployTask = respondingDeploy.ToTask();
        respondingDeployTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.AreEqual(0, respondingDeploy.Result.DecrementRefCount());
        var asyncRespondingRule = new AsyncTaskRespondingRule(requestAddress: respondingRule.ListenAddress);
        var asyncRespondingDeploy = eventQueue.LaunchRule(asyncRespondingRule, asyncRespondingRule);
        var asyncRespondingDeployTask = asyncRespondingDeploy.ToTask();
        asyncRespondingDeployTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.AreEqual(0, asyncRespondingDeploy.Result.DecrementRefCount());
        var requestingRule = new RequestingRule(asyncRespondingRule.ListenAddress);
        var requestingDeployVTask = eventQueue.LaunchRule(requestingRule, requestingRule);
        var requestingRuleTask = requestingDeployVTask.ToTask();
        requestingRuleTask.Wait(2_000);
        Thread.Sleep(10);
        Assert.IsTrue(requestingDeployVTask.IsCompleted);
        Assert.AreEqual(0, requestingDeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(3, requestingRule.PublishNumber);
        Assert.AreEqual(3, asyncRespondingRule.LastReceivedRequestNumber);
        Assert.AreEqual(13, asyncRespondingRule.LastReceivedResponseNumber);
        Assert.AreEqual(3, asyncRespondingRule.ReceiveCount);
        Assert.AreEqual(3, respondingRule.ReceiveCount);
        Assert.AreEqual(3, requestingRule.ReceiveCount);
    }
}
