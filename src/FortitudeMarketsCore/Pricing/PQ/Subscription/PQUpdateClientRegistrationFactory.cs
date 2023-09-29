using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public class PQUpdateClientRegistrationFactory : PQSocketSubscriptionRegistrationFactoryBase<PQUpdateClient>
    {
        public PQUpdateClientRegistrationFactory(IOSNetworkingController networkingController)
            : base(networkingController)
        {
        }

        protected override PQUpdateClient CreateNewSocketSubscriptionType(ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string socketUseDescription, uint cxTimeoutS, int wholeMessagesPerReceive,
            IPQQuoteSerializerFactory pqQuoteSerializerFactory, string multicastInterface)
        {
            var pqUpdateClient = new PQUpdateClient(dispatcher, networkingController, connectionConfig,
                socketUseDescription, multicastInterface, wholeMessagesPerReceive, pqQuoteSerializerFactory);
            pqUpdateClient.Connect();
            return pqUpdateClient;
        }
    }
}