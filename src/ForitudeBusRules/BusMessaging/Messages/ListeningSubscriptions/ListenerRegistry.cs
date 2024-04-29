using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps;

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public class ListenerRegistry
{
    private readonly List<string> destinationAddresses = new();
    public List<IMessageListenerSubscription> matcherListenerSubscriptions = new();
    public IMap<string, List<IMessageListenerSubscription>> Listeners
        = new ConcurrentMap<string, List<IMessageListenerSubscription>>();
    private List<IListenSubscribeInterceptor> subscribeInterceptors = new ();

    private readonly AutoRecycledEnumerable<IMessageListenerSubscription> foundMatchingSubscriptions = new();

    public ListenerRegistry()
    {
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

    public ValueTask RemoveListenerFromWatchList(MessageListenerUnsubscribe unsubscribePayload)
    {
        var listeningAddress = unsubscribePayload.PublishAddress;
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
                    if (ruleListener.SubscriberId == unsubscribePayload.SubscriberId)
                    {
                        ruleListener.Dispose();
                        ruleListeners.RemoveAt(i);
                    }
                }

                if (ruleListeners.Count == 0) Listeners.Remove(listeningAddress);
            }
        }

        unsubscribePayload.SubscriberRule.DecrementLifeTimeCount();
        return ValueTask.CompletedTask;
    }

    private IEnumerable<IMessageListenerSubscription> AllRegiListenerSubscriptions => Listeners.SelectMany(kvp => kvp.Value);

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

    public async ValueTask AddListenerToWatchList(IMessageListenerSubscription subscribePayload)
    {
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

    public IEnumerable<IMessageListenerSubscription> MatchingSubscriptions(string address)
    {
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