// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public class MessageListenerSubscription : RecyclableObject, ISubscription
{
    private bool hasUnsubscribed;
    public MessageListenerSubscription() { }

    public MessageListenerSubscription
    (IListeningRule subscriberRule, string publishAddress, string subscriberId
      , IDispatchResult? dispatchResult = null)
    {
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
        SubscriberId   = subscriberId;
        DispatchResult = dispatchResult;
    }

    public IListeningRule   SubscriberRule { get; set; } = null!;
    public string           PublishAddress { get; set; } = null!;
    public string           SubscriberId   { get; set; } = null!;
    public IDispatchResult? DispatchResult { get; set; }

    public void Unsubscribe()
    {
        if (hasUnsubscribed) return;
        hasUnsubscribed = true;
        SubscriberRule.Context.RegisteredOn.EnqueuePayloadBody(this, SubscriberRule, MessageType.ListenerUnsubscribe, PublishAddress);
    }

    public async ValueTask UnsubscribeAsync()
    {
        if (hasUnsubscribed) return;
        hasUnsubscribed = true;
        await SubscriberRule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(this, SubscriberRule, MessageType.ListenerUnsubscribe
                                                                                 , PublishAddress);
    }

    public ValueTask DisposeAwaitValueTask { get; set; }
    public ValueTask Dispose()             => UnsubscribeAsync();

    public ValueTask DisposeAsync() => UnsubscribeAsync();
}
