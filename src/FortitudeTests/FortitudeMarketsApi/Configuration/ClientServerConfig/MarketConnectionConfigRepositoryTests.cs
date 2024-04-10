#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

[TestClass]
public class MarketConnectionConfigRepositoryTests
{
    private IList<IMarketConnectionConfig> dummyServerConfigs = null!;
    private bool hitExpectedSubscriberState;
    private IMarketConnectionConfig marketConnectionConfig1 = null!;
    private IMarketConnectionConfig marketConnectionConfig2 = null!;

    private MarketConnectionConfigRepository marketConnectionConfigRepository = null!;

    [TestInitialize]
    public void ManualSetup()
    {
        marketConnectionConfigRepository = new MarketConnectionConfigRepository();

        dummyServerConfigs = MarketConnectionConfigTests.ListOfSampleServerConfigs;

        dummyServerConfigs[1].Name = "SomeOtherTradingName";

        marketConnectionConfig1 = dummyServerConfigs[0];
        marketConnectionConfig2 = dummyServerConfigs[1];

        hitExpectedSubscriberState = false;
    }


    [TestMethod]
    public void NonEmptyRepo_FindConfig_RetirevesConfig()
    {
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig1);
        Assert.IsFalse(hitExpectedSubscriberState);
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig2);
        Assert.IsFalse(hitExpectedSubscriberState);

        var foundItem = marketConnectionConfigRepository.Find(marketConnectionConfig2.Name!);

        Assert.AreSame(foundItem, marketConnectionConfig2);
    }


    [TestMethod]
    public void NonEmptyRepo_SubscribeToUpdateStream_RetirevesExistingConfig()
    {
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig1);
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig2);
    }

    [TestMethod]
    public void EmptyRepo_AddOrUpdateThenDelete_DeleteEventsAreReceived()
    {
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig1);
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig2);

        marketConnectionConfigRepository.Delete(marketConnectionConfig2);

        hitExpectedSubscriberState = false;
        marketConnectionConfigRepository.Delete(marketConnectionConfig1);
    }

    [TestMethod]
    public void EmptyRepo_AddOrUpdate_UpdateEventsAreReceived()
    {
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig1);
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig2);

        // attempt to addorupdate a second time does nothing
        marketConnectionConfigRepository.AddOrUpdate(marketConnectionConfig2);

        var serverConfig21 = marketConnectionConfig2.Clone();
        MarketConnectionConfigTests.UpdateServerConfigWithValues(serverConfig21, "UpdatedTestConfigName2", MarketConnectionType.Pricing);
        marketConnectionConfigRepository.AddOrUpdate(serverConfig21);

        var foundConfig = marketConnectionConfigRepository.Find("UpdatedTestConfigName2");
        // replaced config is now equal to original as it was updated
        Assert.IsTrue(serverConfig21.Equals(foundConfig));
    }
}
