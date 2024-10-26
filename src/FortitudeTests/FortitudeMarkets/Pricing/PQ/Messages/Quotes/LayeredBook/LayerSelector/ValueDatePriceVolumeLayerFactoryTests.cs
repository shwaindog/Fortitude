#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class ValueDatePriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new ValueDatePriceVolumeLayerFactory();

        var emptyPvl = new PQValueDatePriceVolumeLayer();

        Assert.AreEqual(typeof(PQValueDatePriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new ValueDatePriceVolumeLayerFactory();

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.AreEqual(SourcePvl.Price, srcPvl.Price);
        Assert.AreEqual(SourcePvl.Volume, srcPvl.Volume);

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.AreEqual(SourceQtRefPvl.Price, srcQtRefPvl.Price);
        Assert.AreEqual(SourceQtRefPvl.Volume, srcQtRefPvl.Volume);

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.AreEqual(VlDtPvl, vlDtPvl);

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.AreEqual(TraderPvl.Price, trdrPvl.Price);
        Assert.AreEqual(TraderPvl.Volume, trdrPvl.Volume);

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
    }
}
