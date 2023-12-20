#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsApi.Trading;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Subscription;

public sealed class OrxTradingClientMessaging : OrxClientMessaging
{
    private readonly OrxSerializationFactory serializationFactory;

    private int nextSequence;

    public OrxTradingClientMessaging(ISocketDispatcher dispatcher, IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig, string socketUseDescription, int wholeMessagesPerReceive,
        bool keepalive = false)
        : base(
            dispatcher, networkingController,
            connectionConfig, socketUseDescription, wholeMessagesPerReceive, keepalive)
    {
        serializationFactory = new OrxSerializationFactory(RecyclingFactory);
        OnDisconnected += () => nextSequence = 0;
    }


    public override void Send(IVersionedMessage versionedMessage)
    {
        lock (SyncLock)
        {
            if (versionedMessage is ITradingMessage tradingMessage) tradingMessage.SequenceNumber = nextSequence++;
            base.Send(versionedMessage);
        }
    }
}
