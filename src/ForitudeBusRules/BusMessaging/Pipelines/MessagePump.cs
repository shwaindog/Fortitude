// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IMessagePump : IAsyncValueTaskRingPoller<BusMessage>
{
    QueueContext QueueContext { get; set; }

    IQueueMessageRing QueueMessageRing { get; }

    bool IsListeningOn(string address);

    int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo);

    IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address);
}

public class MessagePump : AsyncValueTaskRingPoller<BusMessage>, IMessagePump
{
    public MessagePump
        (IQueueMessageRing queueMessageRing, uint emptyQueueMaxSleepMs) : base(queueMessageRing, emptyQueueMaxSleepMs) =>
        ThreadStartInitialization = QueueMessageRing.InitializeInPollingThread;

    public MessagePump
        (string name, int size, uint emptyQueueMaxSleepMs) : base(new QueueMessageRing(name, size), emptyQueueMaxSleepMs) =>
        ThreadStartInitialization = QueueMessageRing.InitializeInPollingThread;

    public QueueContext QueueContext
    {
        get => QueueMessageRing.QueueContext;
        set
        {
            QueueMessageRing.QueueContext = value;

            Recycler = value.PooledRecycler;
        }
    }

    public IQueueMessageRing QueueMessageRing => (QueueMessageRing)Ring;

    public bool IsListeningOn(string address) => QueueMessageRing.IsListeningOn(address);

    public IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address) => QueueMessageRing.ListeningSubscriptionsOn(address);

    public int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo) => QueueMessageRing.CopyLivingRulesTo(toCopyTo);
}
