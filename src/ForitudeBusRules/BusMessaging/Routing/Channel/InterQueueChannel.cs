// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public class InterQueueChannel<TEvent> : ReusableObject<IChannel>, IChannel<TEvent>
{
    private const string ChannelPublishBase                     = "BusRule.Channel.Receive";
    private const string SingleEventPublishReceiveAddressFormat = $"{ChannelPublishBase}.SingleEvent.{{0}}.{{1}}";
    private const string BatchEventPublishReceiveAddressFormat  = $"{ChannelPublishBase}.BatchEvent.{{0}}.{{1}}";

    private static int totalInstanceCount;

    private string?        batchEventChannelResponseAddress;
    private ISubscription? batchEventReceiverSubscription;
    private int            messageCount;

    private Func<ChannelEvent<TEvent>, bool>?            receiverCallBack;
    private Func<ChannelEvent<TEvent>, ValueTask<bool>>? receiverCallBackAsync;

    private bool receiverHasLastChannelMessageEvent;

    private IListeningRule receiverRule = null!;
    private string?        singleEventChannelResponseAddress;
    private ISubscription? singleEventReceiverSubscription;

    public InterQueueChannel()
    {
        IsOpen = true;
        Id     = Interlocked.Increment(ref totalInstanceCount);
    }

    public InterQueueChannel(IListeningRule receiverRule, Func<ChannelEvent<TEvent>, bool> receiverCallBack)
    {
        this.receiverRule     = receiverRule;
        this.receiverCallBack = receiverCallBack;

        IsOpen = true;
        Id     = Interlocked.Increment(ref totalInstanceCount);
    }

    public InterQueueChannel(IListeningRule receiverRule, Func<ChannelEvent<TEvent>, ValueTask<bool>> receiverCallBackAsync)
    {
        this.receiverRule          = receiverRule;
        this.receiverCallBackAsync = receiverCallBackAsync;

        IsOpen = true;
        Id     = Interlocked.Increment(ref totalInstanceCount);
    }

    public InterQueueChannel(InterQueueChannel<TEvent> toClone)
    {
        receiverRule          = toClone.receiverRule;
        receiverCallBackAsync = toClone.receiverCallBackAsync;
        receiverCallBack      = toClone.receiverCallBack;
    }

    protected int NexMsgNum => Interlocked.Increment(ref messageCount);

    public int Id { get; }

    public bool ReceiverAlive => receiverRule.LifeCycleState is RuleLifeCycle.Starting or RuleLifeCycle.Started;

    public int MaxInflight { get; set; }

    public bool IsOpen { get; set; }

    public Type EventType => typeof(TEvent);

    public async ValueTask<bool> Publish(IRule sender, TEvent toPublish)
    {
        if (!IsOpen || !ReceiverAlive) return false;
        bool sendMore;
        if (ReceiverIsSameQueueAs(sender))
        {
            sendMore = await CallReceiver(new ChannelEvent<TEvent>(Id, toPublish, NexMsgNum));
        }
        else
        {
            await EnsureSingleEventReceiverListening();
            sendMore = await sender.RequestAsync<ChannelEvent<TEvent>, bool>
                (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(Id, toPublish, NexMsgNum)
               , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        if (!sendMore) Close(sender);
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                sendMore = await CallReceiver(new ChannelEvent<TEvent>(Id, batchItemEvent, NexMsgNum));
                if (!sendMore) break;
            }
        }
        else
        {
            await EnsureBatchEventReceiverListening();
            sendMore = await sender.RequestAsync<ChannelBatchedEvents<TEvent>, bool>
                (batchEventChannelResponseAddress!, new ChannelBatchedEvents<TEvent>(Id, batchToPublish)
               , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        if (!sendMore) Close(sender);
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        return sendMore;
    }

    public async ValueTask<bool> PublishComplete(IRule sender)
    {
        if (!IsOpen || !ReceiverAlive) return false;
        var expectFalse = true;
        if (ReceiverIsSameQueueAs(sender))
        {
            expectFalse                        = await CallReceiver(new ChannelEvent<TEvent>(Id, NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
        }
        else
        {
            if (singleEventChannelResponseAddress != null)
                expectFalse = await sender.RequestAsync<ChannelEvent<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(Id, NexMsgNum)
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
            await CallReceiver(new ChannelEvent<TEvent>(Id, NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
        }
        else if (!receiverHasLastChannelMessageEvent)
        {
            if (singleEventChannelResponseAddress != null)
                await caller.RequestAsync<ChannelEvent<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelEvent<TEvent>(Id, NexMsgNum)
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
            else if (batchEventChannelResponseAddress != null)
                await caller.RequestAsync<ChannelBatchedEvents<TEvent>, bool>
                    (singleEventChannelResponseAddress!, new ChannelBatchedEvents<TEvent>()
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: receiverRule));
        }
        IsOpen = false;
        // ReSharper disable MethodHasAsyncOverload
        singleEventReceiverSubscription?.Unsubscribe();
        batchEventReceiverSubscription?.Unsubscribe();
        // ReSharper restore MethodHasAsyncOverload
        openChannelWasClosed = true;
        return openChannelWasClosed;
    }

    public override IChannel CopyFrom(IChannel source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is InterQueueChannel<TEvent> interQueueChannel)
        {
            receiverRule          = interQueueChannel.receiverRule;
            receiverCallBackAsync = interQueueChannel.receiverCallBackAsync;
            receiverCallBack      = interQueueChannel.receiverCallBack;
        }
        return this;
    }

    public override IChannel Clone() => Recycler?.Borrow<InterQueueChannel<TEvent>>().CopyFrom(this) ?? new InterQueueChannel<TEvent>(this);

    public InterQueueChannel<TEvent> Configure(IListeningRule listeningRule, Func<ChannelEvent<TEvent>, bool> callBack)
    {
        receiverRule     = listeningRule;
        receiverCallBack = callBack;
        IsOpen           = true;
        return this;
    }

    public InterQueueChannel<TEvent> Configure(IListeningRule listeningRule, Func<ChannelEvent<TEvent>, ValueTask<bool>> callBackAsync)
    {
        receiverRule          = listeningRule;
        receiverCallBackAsync = callBackAsync;
        IsOpen                = true;
        return this;
    }

    private async ValueTask<bool> CallReceiver(ChannelEvent<TEvent> channelEvent)
    {
        var sendMore = false;
        if (receiverCallBack != null)
            sendMore                                     = receiverCallBack(channelEvent);
        else if (receiverCallBackAsync != null) sendMore = await receiverCallBackAsync(channelEvent);
        return sendMore;
    }

    protected bool ReceiverIsSameQueueAs(IRule candidate) => receiverRule.Context == candidate.Context;

    protected async ValueTask EnsureSingleEventReceiverListening()
    {
        if (singleEventChannelResponseAddress != null) return;
        singleEventChannelResponseAddress = string.Format(SingleEventPublishReceiveAddressFormat, receiverRule.FriendlyName, Id);
        singleEventReceiverSubscription = await receiverRule.Context.MessageBus.RegisterRequestListenerAsync<ChannelEvent<TEvent>, bool>
            (receiverRule, singleEventChannelResponseAddress, CallReceiverSingleEventCallback);
    }

    protected async ValueTask EnsureBatchEventReceiverListening()
    {
        if (batchEventChannelResponseAddress != null) return;
        batchEventChannelResponseAddress = string.Format(BatchEventPublishReceiveAddressFormat, receiverRule.FriendlyName, Id);
        batchEventReceiverSubscription = await receiverRule.Context.MessageBus.RegisterRequestListenerAsync<ChannelBatchedEvents<TEvent>, bool>
            (receiverRule, batchEventChannelResponseAddress, CallReceiverBatchEventCallback);
    }

    private async ValueTask<bool> CallReceiverSingleEventCallback(IBusRespondingMessage<ChannelEvent<TEvent>, bool> respondingMessage)
    {
        var channelEvent = respondingMessage.Payload.Body();
        if (channelEvent.IsLastEvent)
        {
            await CallReceiver(channelEvent);
            receiverHasLastChannelMessageEvent = true;
            return false;
        }
        return await CallReceiver(channelEvent);
    }

    private async ValueTask<bool> CallReceiverBatchEventCallback(IBusRespondingMessage<ChannelBatchedEvents<TEvent>, bool> respondingMessage)
    {
        var channelEvent = respondingMessage.Payload.Body();
        if (channelEvent.IsLastBatchEvent)
        {
            await CallReceiver(new ChannelEvent<TEvent>(Id, NexMsgNum));
            receiverHasLastChannelMessageEvent = true;
            return false;
        }
        var sendMore = true;
        foreach (var batchItemEvent in channelEvent.Events)
        {
            sendMore = await CallReceiver(new ChannelEvent<TEvent>(Id, batchItemEvent, NexMsgNum));
            if (!sendMore) break;
        }
        return sendMore;
    }
}
