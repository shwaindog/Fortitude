#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

[TestClass]
public class PQClientSnapshotRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private PQClientSnapshotRule pqClientSnapshotRule = null!;
    private PQPublisher<PQLevel3Quote> pqPublisher = null!;
    private LocalHostPQServerLevel3QuoteTestSetup pqServerL3QuoteServerSetup = null!;
    private IRecycler recycler = null!;
    private MessageDeserializationRepository sharedDeserializationRepo = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler = new Recycler();
        sharedDeserializationRepo = new MessageDeserializationRepository("SharedSnapshotUpdateRepository", recycler);
        pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();
        pqPublisher = pqServerL3QuoteServerSetup.CreatePQPublisher();
        pqClientSnapshotRule = new PQClientSnapshotRule("PQClientSnapshotRuleTests"
            , pqServerL3QuoteServerSetup.MarketConnectionConfig.ToggleProtocolDirection()
            , sharedDeserializationRepo, MessageBus.BusIOResolver.GetDispatcherResolver(QueueSelectionStrategy.FirstInSet));
    }

    [TestCleanup]
    public void TearDown()
    {
        pqServerL3QuoteServerSetup.TearDown();
        FLoggerFactory.GracefullyTerminateProcessLogging();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(30_000)]
    public async Task StartedPQServer_DeployPQClientSnapshotClient_ReceivesSourceTickerInfo()
    {
        var results = await EventQueue.LaunchRuleAsync(pqClientSnapshotRule, pqClientSnapshotRule, EventQueueSelectionResult);
    }
}
