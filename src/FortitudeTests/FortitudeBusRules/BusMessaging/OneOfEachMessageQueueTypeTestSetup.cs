// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeBusRules.Messages;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeBusRules.BusMessaging;

[NoMatchingProductionClass]
public class OneOfEachMessageQueueTypeTestSetup
{
    public const int AsyncRingPollerSize = 2550;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OneOfEachMessageQueueTypeTestSetup));

    public    MessageQueue           CustomQueue1 = null!;
    protected RouteSelectionResult   CustomQueue1SelectionResult;
    public    MessageQueue           EventQueue1 = null!;
    protected RouteSelectionResult   EventQueue1SelectionResult;
    public    IOInboundMessageQueue  IOInboundQueue1 = null!;
    protected RouteSelectionResult   IOInboundQueue1SelectionResult;
    public    IOOutboundMessageQueue IOOutboundQueue1 = null!;
    protected RouteSelectionResult   IOOutboundQueue1SelectionResult;
    public    MessageBus             MessageBus   = null!;
    public    MessageQueue           WorkerQueue1 = null!;
    protected RouteSelectionResult   WorkerQueue1SelectionResult;


    [TestInitialize]
    public void SetupMessageBus()
    {
        Logger.Info("Creating test message bus.");
        OneOfQueueTypeMessageBus();
    }

    public virtual ITimerProvider ResolverTimerProvider() => new RealTimerProvider();

    public MessageBus OneOfQueueTypeMessageBus()
    {
        var defaultQueuesConfig = new QueuesConfig
        {
            DefaultQueueSize         = AsyncRingPollerSize
          , EventQueueSize           = AsyncRingPollerSize
          , RequiredIOInboundQueues  = 1
          , RequiredIOOutboundQueues = 1
        };
        TimerContext.Provider = ResolverTimerProvider();
        var timer
            = TimerContext
                .CreateUpdateableTimer
                    ("FortitudeTests.FortitudeBusRules.BusMessaging.Pipelines.OneOfEachEventQueueTypeTestSetup.ThreadPoolTimer");

        MessageBus = new MessageBus(evtBus =>
        {
            EventQueue1      = new MessageQueue(evtBus, MessageQueueType.Event, 1, RingPoller("EventQueue"));
            WorkerQueue1     = new MessageQueue(evtBus, MessageQueueType.Worker, 1, RingPoller("WorkerQueue"));
            CustomQueue1     = new MessageQueue(evtBus, MessageQueueType.Custom, 1, RingPoller("CustomQueue"));
            IOOutboundQueue1 = new IOOutboundMessageQueue(evtBus, MessageQueueType.IOOutbound, 1, SocketSenderRingPoller());
            IOInboundQueue1  = new IOInboundMessageQueue(evtBus, MessageQueueType.IOInbound, 1, SocketListenerRingPoller(timer));

            var eventGroupContainer = new MessageQueueGroupContainer
                (evtBus, evBus => new MessageQueueTypeGroup
                     (evtBus, MessageQueueType.Event, new Recycler(), defaultQueuesConfig)
                     { EventQueue1 }
               , wrkBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Worker, new Recycler(), defaultQueuesConfig)
                     { WorkerQueue1 }
               , ioInBus => new SocketListenerMessageQueueGroup(evtBus, MessageQueueType.IOInbound, new Recycler(), defaultQueuesConfig)
                     { IOInboundQueue1 }
               , ioOutBus => new SocketSenderMessageQueueGroup(evtBus, MessageQueueType.IOOutbound, new Recycler(), defaultQueuesConfig)
                     { IOOutboundQueue1 }
               , cstBus => new MessageQueueTypeGroup(evtBus, MessageQueueType.Custom, new Recycler(), defaultQueuesConfig)
                     { CustomQueue1 }
                );
            return eventGroupContainer;
        });
        EventQueue1SelectionResult  = new RouteSelectionResult(EventQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        WorkerQueue1SelectionResult = new RouteSelectionResult(WorkerQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        CustomQueue1SelectionResult = new RouteSelectionResult(CustomQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOOutboundQueue1SelectionResult
            = new RouteSelectionResult(IOOutboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        IOInboundQueue1SelectionResult = new RouteSelectionResult(IOInboundQueue1, "OneOfEachMessageQueueTypeTestSetup", RoutingFlags.DefaultDeploy);
        MessageBus.Start();
        return MessageBus;
    }

    [TestCleanup]
    public void TearDownMessageBus()
    {
        MessageBus.Stop();
        Logger.Info("OneOfEachMessageQueueTypeTestSetup MessageBus stopped");
    }

    protected IAsyncValueTaskRingPoller<BusMessage> RingPoller(string name) => new AsyncValueTaskRingPoller<BusMessage>(PollingRing(name), 1);

    protected ISocketSenderMessageQueueRingPoller SocketSenderRingPoller() =>
        new SocketAsyncValueTaskEventQueueSender(PollingRing("MessageBusSocketSender"), 1);

    protected ISocketListenerMessageQueueRingPoller SocketListenerRingPoller(IUpdateableTimer timer) =>
        new SocketAsyncValueTaskEventQueueListener(PollingRing("MessageBusSocketListener"), 1, new SocketSelector(1), timer);

    protected IAsyncValueTaskPollingRing<BusMessage> PollingRing(string name, int size = AsyncRingPollerSize)
    {
        return new AsyncValueValueTaskPollingRing<BusMessage>
            (name, size, () => new BusMessage(), ClaimStrategyType.MultiProducers, null, false);
    }
}
