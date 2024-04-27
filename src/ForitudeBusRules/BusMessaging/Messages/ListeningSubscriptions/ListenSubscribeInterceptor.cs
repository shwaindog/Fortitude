namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IListenSubscribeInterceptor
{
    string Name { get; }
    ValueTask Intercept(IMessageListenerSubscription messageListenerSubscription);
    bool ShouldRunIntercept(IMessageListenerSubscription messageListenerSubscription);
}

public abstract class AddressListenSubscribeInterceptor : IListenSubscribeInterceptor
{
    protected AddressListenSubscribeInterceptor(string name, IAddressMatcher addressMatcher)
    {
        Name = name;
        AddressMatcher = addressMatcher;
    }

    public IAddressMatcher AddressMatcher { get; }

    public string Name { get; }

    public async ValueTask Intercept(IMessageListenerSubscription messageListenerSubscription)
    {
        if (ShouldRunIntercept(messageListenerSubscription))
        {
            await RunInterceptorAction(messageListenerSubscription);
            messageListenerSubscription.AddRunListenSubscriptionInterceptor(this);
        }
    }

    public bool ShouldRunIntercept(IMessageListenerSubscription messageListenerSubscription) =>
        AddressMatcher.IsMatch(messageListenerSubscription.PublishAddress)
        && messageListenerSubscription.ActiveListenSubscribeInterceptors.Any(lsi => lsi.Name == Name);

    public abstract ValueTask RunInterceptorAction(IMessageListenerSubscription messageListenerSubscription);
}
