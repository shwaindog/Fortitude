// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeMarkets.Configuration;

namespace FortitudeTests.FortitudeMarkets.Configuration;

[TestClass]
public class MarketsConfigTests
{
    private IList<IMarketConnectionConfig> dummyServerConfigs      = null!;
    private IMarketConnectionConfig        marketConnectionConfig1 = null!;
    private IMarketConnectionConfig        marketConnectionConfig2 = null!;

    private MarketsConfig marketsConfig = null!;

    [TestInitialize]
    public void ManualSetup()
    {
        marketsConfig = new MarketsConfig("MarketsConfigTests");

        dummyServerConfigs = MarketConnectionConfigTests.ListOfSampleServerConfigs;

        marketConnectionConfig1 = dummyServerConfigs[0];
        marketConnectionConfig2 = dummyServerConfigs[1];
    }


    [TestMethod]
    public void NonEmptyRepo_FindConfig_RetirevesConfig()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        var foundItem = marketsConfig.Find(marketConnectionConfig2.SourceName!);

        marketConnectionConfig2.ConnectionName = marketsConfig.ConnectionName;

        Assert.AreEqual(foundItem, marketConnectionConfig2);
    }


    [TestMethod]
    public void NonEmptyRepo_SubscribeToUpdateStream_RetirevesExistingConfig()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);
    }


    [TestMethod]
    public void EmptyRepo_AddOrUpdate_UpdateEventsAreReceived()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        // attempt to addorupdate a second time does nothing
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        var serverConfig21 = marketConnectionConfig2.Clone();
        serverConfig21.ConnectionName = marketsConfig.ConnectionName;
        MarketConnectionConfigTests.UpdateServerConfigWithValues(serverConfig21, "UpdatedTestConfigName2", MarketConnectionType.Pricing);
        marketsConfig.AddOrUpdate(serverConfig21);

        var foundConfig = marketsConfig.Find("UpdatedTestConfigName2");
        // replaced config is now equal to original as it was updated
        Assert.IsTrue(serverConfig21.Equals(foundConfig));
    }
}
