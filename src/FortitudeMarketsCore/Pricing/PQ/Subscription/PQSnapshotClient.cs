#region

using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Client;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public sealed class PQSnapshotClient : TcpSocketClient, IPQSnapshotClient
{
    private readonly uint cxTimeoutS;

    private readonly IPQQuoteSerializerFactory factory = new PQQuoteSerializerFactory();

    private readonly IIntraOSThreadSignal intraOSThreadSignal;

    private readonly IDictionary<uint, IUniqueSourceTickerIdentifier> requestsQueue =
        new Dictionary<uint, IUniqueSourceTickerIdentifier>();

    private DateTime lastSnapshotSent = DateTime.MinValue;
    private IBinaryStreamPublisher? streamToPublisher;
    private ITimerCallbackSubscription? timerSubscription;

    public PQSnapshotClient(ISocketDispatcher dispatcher, IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig,
        string socketUseDescription, uint cxTimeoutS, int wholeMessagesPerReceive,
        IPQQuoteSerializerFactory pqQuoteSerializerFactory)
        : base(
            FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!), dispatcher,
            networkingController, connectionConfig,
            socketUseDescription + " PQSnapshotClient", wholeMessagesPerReceive,
            new LinkedListUintKeyMap<IMessageDeserializer>())
    {
        intraOSThreadSignal = ParallelController.SingleOSThreadActivateSignal(false);
        OnConnected += OnResponse;
        OnConnected += SendQueuedRequests;
        OnDisconnected += DisableTimeout;
        this.cxTimeoutS = cxTimeoutS;
        factory = pqQuoteSerializerFactory ?? factory;
    }

    public override int RecvBufferSize => 131072;

    public override IBinaryStreamPublisher StreamToPublisher =>
        streamToPublisher ?? (streamToPublisher =
            new PQSnapshotStreamPublisher(Logger, Dispatcher, NetworkingController, SessionDescription, this));

    public void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds)
    {
        Connect();
        if (IsConnected)
        {
            Logger.Info("Sending snapshot request for streams {0}",
                string.Join(",", sourceTickerIds.Select(sti => sti.Id)));
            var allStreams = sourceTickerIds.Select(x => x.Id).ToArray();
            Send(new PQSnapshotIdsRequest(allStreams));
            lastSnapshotSent = TimeContext.UtcNow;
        }
        else
        {
            var queuing = string.Empty;
            lock (requestsQueue)
            {
                foreach (var srcTkr in sourceTickerIds)
                    if (!requestsQueue.ContainsKey(srcTkr.Id))
                    {
                        requestsQueue[srcTkr.Id] = srcTkr;
                        queuing += queuing.Length > 0 ? "," + srcTkr.Id : srcTkr.Id.ToString();
                    }
            }

            if (!string.IsNullOrEmpty(queuing))
                Logger.Info("Queuing snapshot request for ticker ids {0}", queuing);
            else
                Logger.Info("Snapshot request already queued for ticker ids {0}, last snapshot sent at {1}",
                    string.Join(",", sourceTickerIds.Select(sti => sti.Id)), lastSnapshotSent.ToString("O"));
        }
    }

    public override IMessageStreamDecoder GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers)
    {
        var decoder = new PQClientMessageStreamDecoder(deserializers, PQFeedType.Snapshot);
        decoder.OnResponse += OnResponse;
        return decoder;
    }

    private void SendQueuedRequests()
    {
        uint[] streamIDs;
        string streams;
        lock (requestsQueue)
        {
            streamIDs = requestsQueue.Keys.ToArray();
            streams = string.Join(",", streamIDs);
            requestsQueue.Clear();
        }

        if (streamIDs.Length > 0)
        {
            Logger.Info("Sending queued snapshot requests for streams: {0}", streams);
            Send(new PQSnapshotIdsRequest(streamIDs));
            lastSnapshotSent = TimeContext.UtcNow;
        }
    }

    private void EnableTimeout()
    {
        if (timerSubscription == null)
            timerSubscription = ParallelController.ScheduleWithEarlyTrigger(intraOSThreadSignal, TimeoutConnection!,
                cxTimeoutS * 1000u, false);
    }

    private void DisableTimeout()
    {
        if (timerSubscription == null) return;
        timerSubscription.Unregister(intraOSThreadSignal);
        timerSubscription = null;
    }

    private void TimeoutConnection(object state, bool timedOut)
    {
        if (timedOut) Disconnect(false);
    }

    protected override IBinaryDeserializationFactory GetFactory() => factory;

    private void OnResponse()
    {
        DisableTimeout();
        EnableTimeout();
    }

    public class SnapShotStreamPublisher : VersionedMessage
    {
        public SnapShotStreamPublisher() { }
        public SnapShotStreamPublisher(IVersionedMessage toClone) : base(toClone) { }
        public SnapShotStreamPublisher(byte version) : base(version) { }

        public SnapShotStreamPublisher(SnapShotStreamPublisher toClone)
        {
            Version = toClone.Version;
            Ids = toClone.Ids.ToArray();
        }

        public override uint MessageId { get; } = 2121502;
        private uint[] Ids { get; } = Array.Empty<uint>();

        public override IVersionedMessage Clone() => throw new NotImplementedException();
    }

    internal class PQSnapshotStreamPublisher : TcpSocketPublisher
    {
        private readonly PQSnapshotClient pqSnapshotClient;

        public PQSnapshotStreamPublisher(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, string sessionDescription,
            PQSnapshotClient pqSnapshotClient)
            : base(logger, dispatcher, networkingController, 0, sessionDescription)
        {
            this.pqSnapshotClient = pqSnapshotClient;
            RegisterSerializer<SnapShotStreamPublisher>(0);
        }

        public override int SendBufferSize => 131_072;
        public override IBinaryStreamSubscriber StreamFromSubscriber => pqSnapshotClient;

        public override IBinarySerializationFactory GetFactory() => pqSnapshotClient.factory;
    }
}
