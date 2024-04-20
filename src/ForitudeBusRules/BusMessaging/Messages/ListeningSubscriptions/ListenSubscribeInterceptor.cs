namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IListenSubscribeInterceptor
{
    void Intercept(IMessageListenerSubscription messageListenerSubscription);
}

public abstract class AddressListenSubscribeInterceptor : IListenSubscribeInterceptor
{
    private readonly IAddressMatcher addressMatcher;

    protected AddressListenSubscribeInterceptor(IAddressMatcher addressMatcher) => this.addressMatcher = addressMatcher;

    public void Intercept(IMessageListenerSubscription messageListenerSubscription)
    {
        if (addressMatcher.IsMatch(messageListenerSubscription.PublishAddress)) RunInterceptorAction(messageListenerSubscription);
    }

    public abstract void RunInterceptorAction(IMessageListenerSubscription messageListenerSubscription);
}
