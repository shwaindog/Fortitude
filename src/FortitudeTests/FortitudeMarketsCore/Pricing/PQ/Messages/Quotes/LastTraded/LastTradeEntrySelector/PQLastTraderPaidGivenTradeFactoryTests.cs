#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

[TestClass]
public class PQLastTraderPaidGivenTradeFactoryTests : PQLastTradeFactoryTestsBase
{
    private PQNameIdLookupGenerator pqNameIdLookupGenerator = null!;

    [TestInitialize]
    public void SetupNameGenerator()
    {
        pqNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand,
            PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
    }

    [TestMethod]
    public void NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var ltFactory = new PQLastTraderPaidGivenTradeFactory(pqNameIdLookupGenerator);

        var emptyPaidGivenTrade = new PQLastTraderPaidGivenTrade();

        Assert.AreEqual(typeof(PQLastTraderPaidGivenTrade), ltFactory.EntryCreationType);
        Assert.AreEqual(emptyPaidGivenTrade, ltFactory.CreateNewLastTradeEntry());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var ltFactory = new PQLastTraderPaidGivenTradeFactory(pqNameIdLookupGenerator);

        var simpleLastTrade = ltFactory.UpgradeLayer(populatedLastTrade);
        Assert.IsTrue(populatedLastTrade.AreEquivalent(simpleLastTrade));

        var paidGivenLastTrade = ltFactory.UpgradeLayer(populatedLastPaidGivenTrade);
        Assert.IsTrue(populatedLastPaidGivenTrade.AreEquivalent(paidGivenLastTrade));

        var traderLastTrade =
            ltFactory.UpgradeLayer(populatedLastTraderPQLastPaidGivenTrade);
        Assert.AreEqual(traderLastTrade, populatedLastTraderPQLastPaidGivenTrade);
    }
}
