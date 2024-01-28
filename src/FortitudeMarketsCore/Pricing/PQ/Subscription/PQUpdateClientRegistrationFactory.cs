#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQUpdateClientRegistrationFactory : PQSocketSubscriptionRegistrationFactoryBase<PQUpdateClient>
{
    public PQUpdateClientRegistrationFactory(IOSNetworkingController networkingController)
        : base(networkingController) { }

    protected override PQUpdateClient CreateNewSocketSubscriptionType(ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
        string socketUseDescription, uint cxTimeoutS, int wholeMessagesPerReceive,
        IPQQuoteSerializerRepository ipqQuoteSerializerRepository, string? multicastInterface)
    {
        var pqUpdateClient = new PQUpdateClient(dispatcher, networkingController, connectionConfig,
            socketUseDescription, multicastInterface, wholeMessagesPerReceive, ipqQuoteSerializerRepository);
        pqUpdateClient.Connect();
        return pqUpdateClient;
    }
}
