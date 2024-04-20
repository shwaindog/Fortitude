#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQClientSnapshotRule : Rule
{
    private readonly ISocketDispatcherResolver dispatcherResolver;
    private readonly string feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQClientSnapshotRule));
    private readonly IMarketConnectionConfig marketConnectionConfig;
    private readonly IMessageDeserializationRepository sharedUpdateAndSnapshotDeserializationRepo;
    private readonly INetworkTopicConnectionConfig snapshotClienTopicConnectionConfig;
    private PQSourceTickerInfoResponse? lastPQSourceTickerInfoResponse;
    private PQBusRulesSnapshotClient? snapshotClient;

    public PQClientSnapshotRule(string feedName, IMarketConnectionConfig marketConnectionConfig,
        IMessageDeserializationRepository sharedUpdateAndSnapshotDeserializationRepo, ISocketDispatcherResolver dispatcherResolver)
        : base("PQClient_" + feedName + "_PQClientSnapshotRule")
    {
        this.feedName = feedName;
        this.marketConnectionConfig = marketConnectionConfig;
        this.sharedUpdateAndSnapshotDeserializationRepo = sharedUpdateAndSnapshotDeserializationRepo;
        this.dispatcherResolver = dispatcherResolver;
        snapshotClienTopicConnectionConfig = marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig;
    }

    public override async ValueTask StartAsync()
    {
        snapshotClient = PQBusRulesSnapshotClient.BuildTcpRequester(marketConnectionConfig.PricingServerConfig!.SnapshotConnectionConfig,
            dispatcherResolver, feedName, this, sharedUpdateAndSnapshotDeserializationRepo);
        while (lastPQSourceTickerInfoResponse == null)
        {
            snapshotClient.SocketSessionContext.SocketReceiverUpdated += () =>
            {
                var currentDeserializerRepo = snapshotClient.SocketSessionContext.SocketReceiver!.Decoder!.MessageDeserializationRepository;
                currentDeserializerRepo.AttachToEndOfConnectedFallbackRepos(sharedUpdateAndSnapshotDeserializationRepo);
            };
            var connected = await snapshotClient.StartAsync(10_000, null);
            await Task.Delay((int)snapshotClienTopicConnectionConfig.ReconnectConfig.NextReconnectIntervalMs);
            if (connected)
            {
                lastPQSourceTickerInfoResponse = await snapshotClient.RequestSourceTickerQuoteInfoListAsync();

                foreach (var sourceTickerQuoteInfo in lastPQSourceTickerInfoResponse!.SourceTickerQuoteInfos)
                    logger.Info("Received SourceTickerQuoteInfo {0}", sourceTickerQuoteInfo);
                break;
            }

            var nextAttemptTime = marketConnectionConfig.PricingServerConfig.SnapshotConnectionConfig.ReconnectConfig.NextReconnectIntervalMs;
            logger.Warn("Warning did not connect to PQSnapshot Client will wait {0}ms before trying again", nextAttemptTime);

            await Task.Delay((int)nextAttemptTime);
        }
    }
}
