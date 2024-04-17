#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Config;
using FortitudeBusRules.Messages;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class SingleEventQueueTestSetup
{
    protected MessageQueue EventQueue = null!;
    protected RouteSelectionResult EventQueueSelectionResult;
    protected MessageBus MessageBus = null!;

    [TestInitialize]
    public void SetupEventQueue()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = 12;
        defaultQueuesConfig.EventQueueSize = 12;

        var ring = new AsyncValueValueTaskPollingRing<BusMessage>(
            $"EventQueueTests",
            12,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
        var ringPoller = new AsyncValueTaskRingPoller<BusMessage>(ring, 1);
        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue = new MessageQueue(evtBus, MessageQueueType.Event, 1, ringPoller);
            var eventGroupContainer = new MessageQueueGroupContainer(evtBus, evBus =>
                new MessageQueueTypeGroup(evtBus, MessageQueueType.Event, new Recycler(), defaultQueuesConfig)
                    { EventQueue });
            return eventGroupContainer;
        });
        EventQueueSelectionResult = new RouteSelectionResult(EventQueue, "SingleEventQueueTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
    }
}
