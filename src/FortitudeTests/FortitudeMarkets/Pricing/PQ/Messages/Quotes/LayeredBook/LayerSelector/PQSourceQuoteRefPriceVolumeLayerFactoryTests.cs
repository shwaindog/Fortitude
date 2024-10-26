#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQSourceQuoteRefPriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQSourceQuoteRefPriceVolumeLayerFactory(new PQNameIdLookupGenerator(0));

        var emptyPvl = new PQSourceQuoteRefPriceVolumeLayer(NameIdLookupGenerator);

        Assert.AreEqual(typeof(PQSourceQuoteRefPriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQSourceQuoteRefPriceVolumeLayerFactory(new PQNameIdLookupGenerator(0));

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.IsTrue(SourcePvl.AreEquivalent(srcPvl));

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.AreEqual(srcQtRefPvl, SourceQtRefPvl);

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.AreEqual(VlDtPvl.Price, vlDtPvl.Price);
        Assert.AreEqual(VlDtPvl.Volume, vlDtPvl.Volume);

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.AreEqual(TraderPvl.Price, trdrPvl.Price);
        Assert.AreEqual(TraderPvl.Volume, trdrPvl.Volume);

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
    }
}
