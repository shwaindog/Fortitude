#region

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
public class PQClientSourceFeedRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQClientSourceFeedRuleTests));
    private PQClientSourceFeedRule pqClientSourceFeedRule = null!;
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
        var clientMarketConfig
            = pqServerL3QuoteServerSetup.DefaultServerMarketConnectionConfig.ToggleProtocolDirection("PQClientSourceFeedRuleTestsClient");
        clientMarketConfig.Name = "PQClientSourceFeedRuleTests";
        pqClientSourceFeedRule = new PQClientSourceFeedRule(clientMarketConfig);
    }

    [TestCleanup]
    public void TearDown()
    {
        logger.Info("Test complete starting services shutdown");
        pqServerL3QuoteServerSetup.TearDown();
        TearDownMessageBus();
        FLoggerFactory.GracefullyTerminateProcessLogging();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task StartedPQServer_DeployPQClientSourceFeedRule_StartSnapshotAndUpdateRuleAndRequestSourceTickerInfo()
    {
        var results = await EventQueue1.LaunchRuleAsync(pqClientSourceFeedRule, pqClientSourceFeedRule, EventQueue1SelectionResult);
        logger.Info("StartedPQServer_DeployPQClientSourceFeedRule_StartSnapshotAndUpdateRuleAndRequestSourceTickerInfo Completed");
    }
}
