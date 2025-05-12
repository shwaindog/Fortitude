#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQSourcePriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQSourcePriceVolumeLayerFactory(new PQNameIdLookupGenerator(0));

        var emptyPvl = new PQSourcePriceVolumeLayer(NameIdLookupGenerator.Clone());

        Assert.AreEqual(typeof(PQSourcePriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQSourcePriceVolumeLayerFactory(new PQNameIdLookupGenerator(0));

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.AreEqual(srcPvl, SourcePvl);

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.IsTrue(srcQtRefPvl.AreEquivalent(SourceQtRefPvl));

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
