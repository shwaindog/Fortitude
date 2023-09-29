using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public interface IPQSocketSubscriptionRegristrationFactory<out T> where T : ISocketSubscriber
    {
        T RegisterSocketSubscriber(string socketUseDescription, IConnectionConfig cfg, uint streamId,
            ISocketDispatcher dispatcher, int wholeMessagesPerReceive,
            IPQQuoteSerializerFactory pqQuoteSerializerFactory, string multicastInterface = null);

        void UnregisterSocketSubscriber(IConnectionConfig cfg, uint streamId);
        T FindSocketSubscription(IConnectionConfig configKey);
    }
}