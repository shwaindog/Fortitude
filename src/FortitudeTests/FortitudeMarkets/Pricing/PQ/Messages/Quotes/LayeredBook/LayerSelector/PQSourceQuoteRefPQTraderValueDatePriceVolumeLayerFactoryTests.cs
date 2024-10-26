#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory(
            new PQNameIdLookupGenerator(0));

        var emptyPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(NameIdLookupGenerator);

        Assert.AreEqual(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory(
            new PQNameIdLookupGenerator(0));

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.IsTrue(SourcePvl.AreEquivalent(srcPvl));

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.IsTrue(SourceQtRefPvl.AreEquivalent(srcQtRefPvl));

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.IsTrue(VlDtPvl.AreEquivalent(vlDtPvl));

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.IsTrue(TraderPvl.AreEquivalent(trdrPvl));

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(srcQtRefTrdrVlDtPvl, SrcQtRefTrdrVlDtPvl);
    }
}
