// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public struct ChannelEvent<TEvent>
{
    private TEvent? eventItem;

    public ChannelEvent(int sequenceNumber) => SequenceNumber = sequenceNumber;

    public ChannelEvent(TEvent eventItem, int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
        Event          = eventItem;
    }

    public int  SequenceNumber { get; set; }
    public bool IsLastEvent    => eventItem == null;
    public TEvent Event
    {
        readonly get => eventItem!;
        set => eventItem = value;
    }
}

public struct ChannelBatchedEvents<TEvent>
{
    private IEnumerable<TEvent>? events;
    public ChannelBatchedEvents() { }

    public ChannelBatchedEvents(IEnumerable<TEvent> events) => Events = events;

    public bool IsLastBatchEvent => events == null;

    public IEnumerable<TEvent> Events
    {
        readonly get => events!;
        set => events = value;
    }
}

public class InterQueueChannel<TEvent> : IChannel<TEvent>
{
    private const string ChannelPublishBase                     = "BusRule.Channel.Receive";
    private const string SingleEventPublishReceiveAddressFormat = $"{ChannelPublishBase}.SingleEvent.{{0}}.{{1}}";
    private const string BatchEventPublishReceiveAddressFormat  = $"{ChannelPublishBase}.BatchEvent.{{0}}.{{1}}";

    private static   int totalInstanceCount;
    private readonly int instanceNumber;

    private readonly Func<ChannelEvent<TEvent>, bool> receiverCallBack;
    private readonly IListeningRule                   receiverRule;

    private string?        batchEventChannelResponseAddress;
    private ISubscription? batchEventReceiverSubscription;
    private int            messageCount;

    private bool           receiverHasLastChannelMessageEvent;
    private string?        singleEventChannelResponseAddress;
    private ISubscription? singleEventReceiverSubscription;

    public InterQueueChannel(IListeningRule receiverRule, Func<ChannelEvent<TEvent>, bool> receiverCallBack)
    {
        this.receiverRule     = receiverRule;
        this.receiverCallBack = receiverCallBack;
        instanceNumber        = Interlocked.Increment(ref totalInstanceCount);
        IsOpen                = true;
    }

    protected int NexMsgNum => Interlocked.Increment(ref messageCount);

    public bool ReceiverAlive => receiverRule.LifeCycleState == RuleLifeCycle.Started;

    public bool IsOpen { get; set; }

    public Type EventType => typeof(TEvent);

    public async ValueTask<bool> Publish(IRule sender, TEvent toPublish)
    {
        if (!IsOpen || !ReceiverAlive) return false;
        bool sendMore;
        if (ReceiverIsSameQueueAs(sender))
        {
            sendMore = receiverCallBack(new ChannelEvent<TEvent>(toPublish, NexMsgNum));
        }
        else
        {
            await EnsureSingleEventReceiverListening();
            sendMore = await sender.RequestAsync<ChannelEvent<TEvent>, bool>
                (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(toPublish, NexMsgNum)
               , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        if (!sendMore) Close(sender);
        return sendMore;
    }

    public async ValueTask<bool> Publish(IRule sender, IEnumerable<TEvent> batchToPublish)
    {
        if (!IsOpen || !ReceiverAlive) return false;
        var sendMore = true;
        if (ReceiverIsSameQueueAs(sender))
        {
            foreach (var batchItemEvent in batchToPublish)
            {
                sendMore = receiverCallBack(new ChannelEvent<TEvent>(batchItemEvent, NexMsgNum));
                if (!sendMore) break;
            }
        }
        else
        {
            await EnsureBatchEventReceiverListening();
            sendMore = await sender.RequestAsync<ChannelBatchedEvents<TEvent>, bool>
                (batchEventChannelResponseAddress!, new ChannelBatchedEvents<TEvent>(batchToPublish)
               , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        if (!sendMore) Close(sender);
        return sendMore;
    }

    public async ValueTask<bool> PublishComplete(IRule sender)
    {
        if (!IsOpen || !ReceiverAlive) return false;
        var expectFalse = true;
        if (ReceiverIsSameQueueAs(sender))
        {
            expectFalse                        = receiverCallBack(new ChannelEvent<TEvent>(NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
        }
        else
        {
            if (singleEventChannelResponseAddress != null)
                expectFalse = await sender.RequestAsync<ChannelEvent<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(NexMsgNum)
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
            else if (batchEventChannelResponseAddress != null)
                expectFalse = await sender.RequestAsync<ChannelBatchedEvents<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelBatchedEvents<TEvent>()
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        return !expectFalse;
    }

    public async ValueTask<bool> Close(IRule caller)
    {
        var openChannelWasClosed = false;
        if (!IsOpen) return openChannelWasClosed;
        if (caller == receiverRule && !receiverHasLastChannelMessageEvent)
        {
            receiverCallBack(new ChannelEvent<TEvent>(NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
        }
        else if (!receiverHasLastChannelMessageEvent)
        {
            if (singleEventChannelResponseAddress != null)
                await caller.RequestAsync<ChannelEvent<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(NexMsgNum)
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
            else if (batchEventChannelResponseAddress != null)
                await caller.RequestAsync<ChannelBatchedEvents<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelBatchedEvents<TEvent>()
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        IsOpen = false;
        singleEventReceiverSubscription?.Unsubscribe();
        batchEventReceiverSubscription?.Unsubscribe();
        openChannelWasClosed = true;
        return openChannelWasClosed;
    }

    protected bool ReceiverIsSameQueueAs(IRule candidate) => receiverRule.Context == candidate.Context;

    protected async ValueTask EnsureSingleEventReceiverListening()
    {
        if (singleEventChannelResponseAddress != null) return;
        singleEventChannelResponseAddress = string.Format(SingleEventPublishReceiveAddressFormat, receiverRule.FriendlyName, instanceNumber);
        singleEventReceiverSubscription = await receiverRule.Context.MessageBus.RegisterRequestListenerAsync<ChannelEvent<TEvent>, bool>
            (receiverRule, singleEventChannelResponseAddress, CallReceiverSingleEventCallback);
    }

    protected async ValueTask EnsureBatchEventReceiverListening()
    {
        if (batchEventChannelResponseAddress != null) return;
        batchEventChannelResponseAddress = string.Format(BatchEventPublishReceiveAddressFormat, receiverRule.FriendlyName, instanceNumber);
        batchEventReceiverSubscription = await receiverRule.Context.MessageBus.RegisterRequestListenerAsync<ChannelBatchedEvents<TEvent>, bool>
            (receiverRule, batchEventChannelResponseAddress, CallReceiverBatchEventCallback);
    }

    private bool CallReceiverSingleEventCallback(IBusRespondingMessage<ChannelEvent<TEvent>, bool> respondingMessage)
    {
        var channelEvent = respondingMessage.Payload.Body();
        if (channelEvent.IsLastEvent)
        {
            receiverCallBack(channelEvent);
            receiverHasLastChannelMessageEvent = true;
            return false;
        }
        return receiverCallBack(channelEvent);
    }

    private bool CallReceiverBatchEventCallback(IBusRespondingMessage<ChannelBatchedEvents<TEvent>, bool> respondingMessage)
    {
        var channelEvent = respondingMessage.Payload.Body();
        if (channelEvent.IsLastBatchEvent)
        {
            receiverCallBack(new ChannelEvent<TEvent>(NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
            return false;
        }
        var sendMore = true;
        foreach (var batchItemEvent in channelEvent.Events)
        {
            sendMore = receiverCallBack(new ChannelEvent<TEvent>(batchItemEvent, NexMsgNum));
            if (!sendMore) break;
        }
        return sendMore;
    }
}
