#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeTests.FortitudeBusRules.Rules;
using Moq;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus;

[TestClass]
public class EventQueueTests
{
    private EventQueue eventQueue = null!;
    private Mock<IEventBus> moqEventBus = null!;

    [TestInitialize]
    public void Setup()
    {
        moqEventBus = new Mock<IEventBus>();
        eventQueue = new EventQueue(moqEventBus.Object, EventQueueType.Event, 1, 12);
    }

    [TestMethod]
    public void EventQueueCanLoadNewRuleAndRunStart()
    {
        var incRule = new IncrementingRule();
        var results = eventQueue.LaunchRule(incRule, incRule);
        var asTask = ReusableValueTaskSource<IDispatchResult>.ExtractTask(results)!;
        asTask.Wait(200_000);
        Assert.IsNotNull(results.IsCompleted);
        Assert.AreEqual(1, incRule.StartCount);
        Assert.AreEqual(eventQueue, incRule.Context.RegisteredOn);
        Assert.AreEqual(RuleLifeCycle.Started, incRule.LifeCycleState);
    }
}
