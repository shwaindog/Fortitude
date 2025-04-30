// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQOrdersPriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQOrdersPriceVolumeLayerFactory(LayerType.OrdersFullPriceVolume, NameIdLookupGenerator.Clone());

        var emptyPvl = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, NameIdLookupGenerator.Clone());

        Assert.AreEqual(typeof(PQOrdersPriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQOrdersPriceVolumeLayerFactory(LayerType.OrdersFullPriceVolume
                                                           , new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerNames));

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.AreEqual(SourcePvl.Price, srcPvl.Price);
        Assert.AreEqual(SourcePvl.Volume, srcPvl.Volume);

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.AreEqual(SourceQtRefPvl.Price, srcQtRefPvl.Price);
        Assert.AreEqual(SourceQtRefPvl.Volume, srcQtRefPvl.Volume);

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.AreEqual(VlDtPvl.Price, vlDtPvl.Price);
        Assert.AreEqual(VlDtPvl.Volume, vlDtPvl.Volume);

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.AreEqual(TraderPvl, trdrPvl);

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
    }
}
