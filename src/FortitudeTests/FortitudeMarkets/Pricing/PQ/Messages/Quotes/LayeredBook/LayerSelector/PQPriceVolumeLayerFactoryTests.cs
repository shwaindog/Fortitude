#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQPriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQPriceVolumeLayerFactory();

        var emptyPvl = new PQPriceVolumeLayer();

        Assert.AreEqual(typeof(PQPriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQPriceVolumeLayerFactory();

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.AreEqual(simplePvl, SimplePvl);

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.IsTrue(srcPvl.AreEquivalent(SourcePvl));

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.IsTrue(srcQtRefPvl.AreEquivalent(SourceQtRefPvl));

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.IsTrue(vlDtPvl.AreEquivalent(VlDtPvl));

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.IsTrue(trdrPvl.AreEquivalent(TraderPvl));

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
    }
}
