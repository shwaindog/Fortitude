#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTraderPaidGivenTradeFactoryTests : PQLastTradeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var ltFactory = new PQLastTraderPaidGivenTradeFactory(NameIdLookupGenerator);

        var emptyPaidGivenTrade = new PQLastTraderPaidGivenTrade(NameIdLookupGenerator);

        Assert.AreEqual(typeof(PQLastTraderPaidGivenTrade), ltFactory.EntryCreationType);
        Assert.AreEqual(emptyPaidGivenTrade, ltFactory.CreateNewLastTradeEntry());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var ltFactory = new PQLastTraderPaidGivenTradeFactory(NameIdLookupGenerator);

        var simpleLastTrade = ltFactory.UpgradeLayer(PopulatedLastTrade);
        Assert.IsTrue(PopulatedLastTrade.AreEquivalent(simpleLastTrade));

        var paidGivenLastTrade = ltFactory.UpgradeLayer(PopulatedLastPaidGivenTrade);
        Assert.IsTrue(PopulatedLastPaidGivenTrade.AreEquivalent(paidGivenLastTrade));

        var traderLastTrade =
            ltFactory.UpgradeLayer(PopulatedLastTraderPQLastPaidGivenTrade);
        Assert.AreEqual(traderLastTrade, PopulatedLastTraderPQLastPaidGivenTrade);
    }
}
