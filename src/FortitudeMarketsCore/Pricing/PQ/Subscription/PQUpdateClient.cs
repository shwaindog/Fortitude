#region

using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public sealed class PQUpdateClient : UdpSubscriber, IPQUpdateClient
{
    public const int PQReceiveBufferSize = 2097152;

    private readonly IPQQuoteSerializerRepository snapshotSerializationRepository = new PQQuoteSerializerRepository();

    public PQUpdateClient(ISocketDispatcher dispatcher, IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig, string socketUseDescription, string? networkAddress,
        int wholeMessagesPerReceive, IPQQuoteSerializerRepository ipqQuoteSerializerRepository)
        : base(FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!), dispatcher,
            networkingController, connectionConfig, socketUseDescription + " PQUpdateClient", networkAddress,
            wholeMessagesPerReceive, new LinkedListUintKeyMap<IMessageDeserializer>()) =>
        snapshotSerializationRepository = ipqQuoteSerializerRepository ?? snapshotSerializationRepository;

    public override IMessageStreamDecoder GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers) =>
        new PQClientMessageStreamDecoder(decoderDeserializers, PQFeedType.Update);

    public override int RecvBufferSize => PQReceiveBufferSize;

    public override IBinaryStreamPublisher? StreamToPublisher => null;

    protected override IMessageIdDeserializationRepository GetFactory() => snapshotSerializationRepository;
}
