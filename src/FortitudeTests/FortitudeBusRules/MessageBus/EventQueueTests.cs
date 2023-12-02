#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
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
        var listeningRule = new ListeningRule();
        var listenDeploy = eventQueue.LaunchRule(listeningRule, listeningRule);
        var listenDeployTask = listenDeploy.ToTask();
        listenDeployTask.Wait(2_000);
        Assert.IsTrue(listenDeploy.IsCompleted);
        Thread.Sleep(100);
        Assert.AreEqual(0, listenDeploy.Result.DecrementRefCount());
        var publishRule = new PublishingRule(2);
        var publishDeployVTask = eventQueue.LaunchRule(publishRule, publishRule);
        var publishRuleTask = publishDeployVTask.ToTask();
        publishRuleTask.Wait(2_000);
        Assert.IsTrue(publishDeployVTask.IsCompleted);
        Thread.Sleep(450);
        Assert.AreEqual(0, publishDeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(2, publishRule.PublishNumber);
        Assert.AreEqual(2, listeningRule.ReceiveCount);
        Assert.AreEqual(2, listeningRule.LastReceivedPublishNumber);
        var publishRule2 = new PublishingRule(4);
        var publish2DeployVTask = eventQueue.LaunchRule(publishRule2, publishRule2);
        var publish2RuleTask = publish2DeployVTask.ToTask();
        publish2RuleTask.Wait(2_000);
        Assert.IsTrue(publish2DeployVTask.IsCompleted);
        Thread.Sleep(850);
        Assert.AreEqual(0, publish2DeployVTask.Result.DecrementRefCount());
        Assert.AreEqual(4, publishRule2.PublishNumber);
        Assert.AreEqual(6, listeningRule.ReceiveCount);
        Assert.AreEqual(4, listeningRule.LastReceivedPublishNumber);
    }
}
