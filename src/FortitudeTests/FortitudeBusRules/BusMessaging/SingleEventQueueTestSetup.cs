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
using FortitudeCommon.Monitoring.Logging;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class SingleEventQueueTestSetup
{
    public const int AsyncRingPollerSize = 32;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SingleEventQueueTestSetup));
    protected MessageQueue EventQueue = null!;
    protected RouteSelectionResult EventQueueSelectionResult;
    protected MessageBus MessageBus = null!;

    [TestInitialize]
    public void SetupEventQueue()
    {
        var defaultQueuesConfig = new QueuesConfig();
        defaultQueuesConfig.DefaultQueueSize = AsyncRingPollerSize;
        defaultQueuesConfig.EventQueueSize = AsyncRingPollerSize;

        var ring = new AsyncValueValueTaskPollingRing<BusMessage>($"EventQueueTests", AsyncRingPollerSize, () => new BusMessage()
            , ClaimStrategyType.MultiProducers);
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

    [TestCleanup]
    public void TearDownMessageBus()
    {
        MessageBus.Stop();
        Logger.Info("SingleEventQueueTestSetup MessageBus stopped");
    }
}
