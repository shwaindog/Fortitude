#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory : PQTraderPriceVolumeLayerFactory
{
    public PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup)
        : base(nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(NameIdLookup, 0, 0);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(original, NameIdLookup);
}
