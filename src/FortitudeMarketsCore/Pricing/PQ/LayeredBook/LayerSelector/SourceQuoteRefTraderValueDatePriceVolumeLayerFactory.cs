#region

using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public class SourceQuoteRefTraderValueDatePriceVolumeLayerFactory : TraderPriceVolumeLayerFactory
{
    public SourceQuoteRefTraderValueDatePriceVolumeLayerFactory(
        IPQNameIdLookupGenerator sourceNameIdLookup, IPQNameIdLookupGenerator traderNameIdLookupGenerator)
        : base(traderNameIdLookupGenerator) =>
        SourceNameIdLookup = sourceNameIdLookup;

    public IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() =>
        new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(0, 0, SourceNameIdLookup,
            TraderNameIdLookup);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(original);
}
