#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastPaidGivenTradeFactoryTests : PQLastTradeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var ltFactory = new PQLastPaidGivenTradeFactory();

        var emptyPaidGivenTrade = new PQLastPaidGivenTrade();

        Assert.AreEqual(typeof(PQLastPaidGivenTrade), ltFactory.EntryCreationType);
        Assert.AreEqual(emptyPaidGivenTrade, ltFactory.CreateNewLastTradeEntry());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var ltFactory = new PQLastPaidGivenTradeFactory();

        var simpleLastTrade = ltFactory.UpgradeLayer(PopulatedLastTrade);
        Assert.IsTrue(PopulatedLastTrade.AreEquivalent(simpleLastTrade));

        var paidGivenLastTrade = ltFactory.UpgradeLayer(PopulatedLastPaidGivenTrade);
        Assert.AreEqual(paidGivenLastTrade, PopulatedLastPaidGivenTrade);

        var traderLastTrade =
            ltFactory.UpgradeLayer(PopulatedLastExternalCounterPartyTrade);
        Assert.IsTrue(traderLastTrade.AreEquivalent(PopulatedLastExternalCounterPartyTrade));
    }
}
