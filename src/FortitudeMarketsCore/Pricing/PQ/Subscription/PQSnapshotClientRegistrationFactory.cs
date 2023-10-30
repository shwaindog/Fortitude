#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQSnapshotClientRegistrationFactory : PQSocketSubscriptionRegistrationFactoryBase<PQSnapshotClient>
{
    public PQSnapshotClientRegistrationFactory(IOSNetworkingController networkingController)
        : base(networkingController) { }

    protected override PQSnapshotClient CreateNewSocketSubscriptionType(ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
        string socketUseDescription, uint cxTimeoutS, int wholeMessagesPerReceive,
        IPQQuoteSerializerFactory pqQuoteSerializerFactory, string? multicastInterface) =>
        new(dispatcher, networkingController, connectionConfig,
            socketUseDescription, 5, wholeMessagesPerReceive, pqQuoteSerializerFactory);
}
