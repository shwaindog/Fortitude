// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQPriceVolumeFactoryTestsBase
{
    protected IPQNameIdLookupGenerator                        NameIdLookupGenerator             = null!;
    protected PQPriceVolumeLayer                              SimplePvl                         = null!;
    protected PQSourcePriceVolumeLayer                        SourcePvl                         = null!;
    protected PQSourceQuoteRefPriceVolumeLayer                SourceQtRefPvl                    = null!;
    protected PQFullSupportPriceVolumeLayer SrcQtRefTrdrVlDtPvl               = null!;
    protected PQOrdersPriceVolumeLayer                        TraderPvl                         = null!;
    protected IPQNameIdLookupGenerator                        TraderSourceNameIdLookupGenerator = null!;
    protected PQValueDatePriceVolumeLayer                     VlDtPvl                           = null!;

    [TestInitialize]
    public void Setup()
    {
        NameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        TraderSourceNameIdLookupGenerator = NameIdLookupGenerator.Clone();
        SimplePvl                         = new PQPriceVolumeLayer(1.234567m, 3_000_000);
        SourcePvl                         = new PQSourcePriceVolumeLayer(NameIdLookupGenerator.Clone(), 2.345678m, 4_000_000, "TestSourceName", true);
        SourceQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer(NameIdLookupGenerator.Clone(), 3.456789m, 5_000_000,
                                                              "TestSourceNameForQtRef", true, 42_192_192);
        VlDtPvl = new PQValueDatePriceVolumeLayer(4.567890m, 6_000_000, new DateTime(2018, 1, 7, 18, 0, 0));
        TraderPvl = new PQOrdersPriceVolumeLayer(TraderSourceNameIdLookupGenerator, LayerType.OrdersFullPriceVolume, 5.678901m, 7_000_000)
        {
            [0] = new PQCounterPartyOrderLayerInfo(TraderSourceNameIdLookupGenerator, orderVolume: 1_000_000, traderName: "TestTraderName1")
          , [1] = new PQCounterPartyOrderLayerInfo(TraderSourceNameIdLookupGenerator, orderVolume: 2_000_000, traderName: "TestTraderName2")
        };
        SrcQtRefTrdrVlDtPvl = new PQFullSupportPriceVolumeLayer(NameIdLookupGenerator.Clone(), 6.789012m, 8_000_000
                                                                                , new DateTime(2018, 1, 6, 15, 0, 0)
                                                                                , "TestSourceNameForSrcQtRefTrdrVlDt", true, 123445);
    }
}
