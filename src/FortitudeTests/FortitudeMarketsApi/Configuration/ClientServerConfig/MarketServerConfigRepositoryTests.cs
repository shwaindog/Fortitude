#region

using FortitudeCommon.EventProcessing;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

[TestClass]
public class MarketServerConfigRepositoryTests
{
    private IList<MarketServerConfigTests.IDummyMarketServerConfig> dummyServerConfigs = null!;
    private bool hitExpectedSubscriberState;
    private MarketServerConfigTests.IDummyMarketServerConfig marketServerConfig1 = null!;
    private MarketServerConfigTests.IDummyMarketServerConfig marketServerConfig2 = null!;

    private MarketServerConfigRepository<MarketServerConfigTests.IDummyMarketServerConfig> marketServerConfigRepository
        = null!;

    [TestInitialize]
    public void ManualSetup()
    {
        marketServerConfigRepository
            = new MarketServerConfigRepository<MarketServerConfigTests.IDummyMarketServerConfig>();

        dummyServerConfigs = MarketServerConfigTests.ListOfSampleServerConfigs;

        marketServerConfig1 = dummyServerConfigs[0];
        marketServerConfig1.Updates = marketServerConfigRepository.ServerConfigUpdateStream;
        marketServerConfig2 = dummyServerConfigs[1];
        marketServerConfig2.Updates = marketServerConfigRepository.ServerConfigUpdateStream;

        hitExpectedSubscriberState = false;
    }


    [TestMethod]
    public void NonEmptyRepo_FindConfig_RetirevesConfig()
    {
        marketServerConfigRepository.AddOrUpdate(marketServerConfig1);
        Assert.IsFalse(hitExpectedSubscriberState);
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        var foundItem = marketServerConfigRepository.Find(marketServerConfig2.Name!);

        Assert.AreSame(foundItem, marketServerConfig2);
    }


    [TestMethod]
    public void NonEmptyRepo_SubscribeToUpdateStream_RetirevesExistingConfig()
    {
        marketServerConfigRepository.AddOrUpdate(marketServerConfig1);
        Assert.IsFalse(hitExpectedSubscriberState);
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        var count = 0;
        marketServerConfigRepository.ServerConfigUpdateStream.Subscribe(scu =>
        {
            if (scu.EventType == EventType.Retrieved) count++;
        });
        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public void EmptyRepo_AddOrUpdateThenDelete_DeleteEventsAreReceived()
    {
        marketServerConfigRepository.ServerConfigUpdateStream.Subscribe(scu =>
        {
            if (scu.EventType == EventType.Deleted) hitExpectedSubscriberState = true;
        });
        marketServerConfigRepository.AddOrUpdate(marketServerConfig1);
        Assert.IsFalse(hitExpectedSubscriberState);
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        marketServerConfigRepository.Delete(marketServerConfig2);
        Assert.IsTrue(hitExpectedSubscriberState);

        hitExpectedSubscriberState = false;
        marketServerConfigRepository.Delete(marketServerConfig1);
        Assert.IsTrue(hitExpectedSubscriberState);
    }

    [TestMethod]
    public void EmptyRepo_AddOrUpdate_UpdateEventsAreReceived()
    {
        marketServerConfigRepository.ServerConfigUpdateStream.Subscribe(scu =>
        {
            if (scu.EventType == EventType.Updated) hitExpectedSubscriberState = true;
        });
        marketServerConfigRepository.AddOrUpdate(marketServerConfig1);
        Assert.IsFalse(hitExpectedSubscriberState);
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        // attempt to addorupdate a second time does nothing
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        var serverConfig21 = marketServerConfig2.Clone();
        MarketServerConfigTests.UpdateServerConfigWithValues(serverConfig21, "UpdatedTestConfigName2",
            MarketServerType.TestOnly | MarketServerType.ConfigServer
            , marketServerConfig2.ServerConnections!.Select(scc => scc.ToConnectionConfig()));
        marketServerConfigRepository.AddOrUpdate(serverConfig21);
        Assert.IsTrue(hitExpectedSubscriberState);

        // replaced config is now equal to original as it was updated
        hitExpectedSubscriberState = false;
        Assert.IsTrue(serverConfig21.Equals(marketServerConfig2));
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);
    }

    [TestMethod]
    public void EmptyRepo_AddOrUpdate_AddEventsAreReceived()
    {
        marketServerConfigRepository.ServerConfigUpdateStream.Subscribe(scu =>
        {
            if (scu.EventType == EventType.Created) hitExpectedSubscriberState = true;
        });
        marketServerConfigRepository.AddOrUpdate(marketServerConfig1);
        Assert.IsTrue(hitExpectedSubscriberState);

        hitExpectedSubscriberState = false;
        marketServerConfigRepository.AddOrUpdate(marketServerConfig2);
        Assert.IsTrue(hitExpectedSubscriberState);
    }
}
