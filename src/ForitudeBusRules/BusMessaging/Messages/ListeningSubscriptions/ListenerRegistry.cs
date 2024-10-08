﻿using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public class ListenerRegistry
{
    private readonly IRecycler    recycler;
    private readonly List<string> destinationAddresses = new();

    public List<IMessageListenerRegistration> matcherListenerSubscriptions = new();

    public IMap<string, List<IMessageListenerRegistration>> Listeners
        = new ConcurrentMap<string, List<IMessageListenerRegistration>>();
    private List<IListenSubscribeInterceptor> subscribeInterceptors = new ();

    private readonly AutoRecycledEnumerable<IMessageListenerRegistration> foundMatchingSubscriptions = new();

    public ListenerRegistry(IRecycler recycler)
    {
        this.recycler  = recycler;

        foundMatchingSubscriptions.Recycler = recycler;

        foundMatchingSubscriptions.AutoRecycleAtRefCountZero = false;
        foundMatchingSubscriptions.DisableEnumeratorAutoRecycle();
    }

    public bool IsListeningOn(string address)
    {
        foreach (var matcherListener in matcherListenerSubscriptions)
        {
            if (matcherListener.Matcher!.IsMatch(address))
            {
                return true;
            }
        }
        if (Listeners.TryGetValue(address, out var ruleListeners))
            foreach (var ruleListener in ruleListeners!)
                return true;
        return false;
    }

    public ValueTask RemoveListenerFromWatchList(MessageListenerSubscription subscriptionPayload)
    {
        var listeningAddress = subscriptionPayload.PublishAddress;
        if (AddressMatcher.IsMatcherPattern(listeningAddress))
        {
            var foundMatcher = matcherListenerSubscriptions.FirstOrDefault(ls => ls.PublishAddress == listeningAddress);
            if (foundMatcher != null)
            {
                foundMatcher.Dispose();
                matcherListenerSubscriptions.Remove(foundMatcher);
            }
        }
        else
        {
            if (Listeners.TryGetValue(listeningAddress, out var ruleListeners))
            {
                for (var i = 0; i < ruleListeners!.Count; i++)
                {
                    var ruleListener = ruleListeners[i];
                    if (ruleListener.SubscriberId == subscriptionPayload.SubscriberId)
                    {
                        ruleListener.Dispose();
                        ruleListeners.RemoveAt(i);
                    }
                }

                if (ruleListeners.Count == 0) Listeners.Remove(listeningAddress);
            }
        }

        subscriptionPayload.SubscriberRule.DecrementLifeTimeCount();
        return ValueTask.CompletedTask;
    }

    private IEnumerable<IMessageListenerRegistration> AllRegiListenerSubscriptions => Listeners.SelectMany(kvp => kvp.Value);

    public async ValueTask AddSubscribeInterceptor(IListenSubscribeInterceptor interceptor)
    {
        subscribeInterceptors.Add(interceptor);
        foreach (var messageListenerSubscription in AllRegiListenerSubscriptions)
        {
            if (interceptor.ShouldRunIntercept(messageListenerSubscription))
            {
                await interceptor.Intercept(messageListenerSubscription);
            }
        }
    }

    public ValueTask RemoveSubscribeInterceptor(IListenSubscribeInterceptor interceptor)
    {
        subscribeInterceptors.Remove(interceptor);
        foreach (var messageListenerSubscription in AllRegiListenerSubscriptions)
        {
            if (messageListenerSubscription.ActiveListenSubscribeInterceptors.Any( lsi => lsi.Name == interceptor.Name))
            {
                messageListenerSubscription.RemoveListenSubscriptionInterceptor(interceptor.Name);
            }
        }
        return ValueTask.CompletedTask;
    }

    public async ValueTask AddListenerToWatchList(BusMessageValue data)
    {
        var subscribePayload  = (IMessageListenerRegistration)data.Payload.BodyObj(PayloadRequestType.QueueReceive)!;
        var processorRegistry = data.ProcessorRegistry;
        processorRegistry?.RegisterStart(subscribePayload.SubscriberRule);
        subscribePayload.SubscriberRule.IncrementLifeTimeCount();
        foreach (var listenSubscribeInterceptor in subscribeInterceptors)
        {
            await listenSubscribeInterceptor.Intercept(subscribePayload);
        }
        if (subscribePayload.Matcher != null)
        {
            matcherListenerSubscriptions.Add(subscribePayload);
        }
        else
        {
            var listeningAddress = subscribePayload.PublishAddress;
            if (!Listeners.TryGetValue(listeningAddress, out var ruleListeners))
            {
                ruleListeners = [];
                Listeners.Add(listeningAddress, ruleListeners);
            }

            ruleListeners!.Add(subscribePayload);
        }
        processorRegistry?.RegisterFinish(subscribePayload.SubscriberRule);
    }


    public ValueTask UnsubscribeAllListenersForRule(IRule removeListeners)
    {
        destinationAddresses.Clear();
        for (var i = 0; i < matcherListenerSubscriptions.Count; i++)
        {
            var currentListener = matcherListenerSubscriptions[i];
            if (currentListener.SubscriberRule == removeListeners)
            {
                currentListener.Dispose();
                matcherListenerSubscriptions.RemoveAt(i--);
            }
        }
        foreach (var listenKvp in Listeners)
        {
            for (var i = 0; i < listenKvp.Value.Count; i++)
            {
                var currentListener = listenKvp.Value[i];
                if (currentListener.SubscriberRule == removeListeners)
                {
                    currentListener.Dispose();
                    listenKvp.Value.RemoveAt(i--);
                }
            }

            if (listenKvp.Value.Count <= 0) destinationAddresses.Add(listenKvp.Key);
        }

        for (var i = 0; i < destinationAddresses.Count; i++)
        {
            var emptyDestinationAddress = destinationAddresses[i];
            Listeners.Remove(emptyDestinationAddress);
        }
        return ValueTask.CompletedTask;
    }

    public IEnumerable<IMessageListenerRegistration> MatchingSubscriptions(string? address)
    {
        if(address == null) return Enumerable.Empty<IMessageListenerRegistration>();
        foundMatchingSubscriptions.Clear();
        foreach (var matcherListener in matcherListenerSubscriptions)
        {
            if (matcherListener.Matcher!.IsMatch(address))
            {
                foundMatchingSubscriptions.Add(matcherListener);
            }
        }
        if (Listeners.TryGetValue(address, out var ruleListeners))
            foreach (var ruleListener in ruleListeners!)
                foundMatchingSubscriptions.Add(ruleListener);
        return foundMatchingSubscriptions;
    }
}