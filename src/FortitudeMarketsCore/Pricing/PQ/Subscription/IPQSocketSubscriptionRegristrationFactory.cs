#region

using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSocketSubscriptionRegristrationFactory<out T> where T : ISocketSubscriber
{
    T RegisterSocketSubscriber(string socketUseDescription, IConnectionConfig cfg, uint streamId,
        ISocketDispatcher dispatcher, int wholeMessagesPerReceive,
        IPQQuoteSerializerRepository ipqQuoteSerializerRepository, string? multicastInterface = null);

    void UnregisterSocketSubscriber(IConnectionConfig cfg, uint streamId);
    T? FindSocketSubscription(IConnectionConfig configKey);
}
