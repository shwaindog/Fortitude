#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTradeFactoryTests : PQLastTradeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var ltFactory = new PQLastTradeFactory();

        var emptyPQLastTrade = new PQLastTrade();

        Assert.AreEqual(typeof(PQLastTrade), ltFactory.EntryCreationType);
        Assert.AreEqual(emptyPQLastTrade, ltFactory.CreateNewLastTradeEntry());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var ltFactory = new PQLastTradeFactory();

        var fromPopulatedPQLastTrade = ltFactory.UpgradeLayer(PopulatedLastTrade);
        Assert.AreEqual(fromPopulatedPQLastTrade, PopulatedLastTrade);

        var paidGivenLastTrade = ltFactory.UpgradeLayer(PopulatedLastPaidGivenTrade);
        Assert.IsTrue(paidGivenLastTrade.AreEquivalent(PopulatedLastPaidGivenTrade));

        var traderLastTrade =
            ltFactory.UpgradeLayer(PopulatedLastTraderPQLastPaidGivenTrade);
        Assert.IsTrue(traderLastTrade.AreEquivalent(PopulatedLastTraderPQLastPaidGivenTrade));
    }
}
