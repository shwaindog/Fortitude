#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public class PQSourceQuoteRefPriceVolumeLayerFactory : PQSourcePriceVolumeLayerFactory
{
    public PQSourceQuoteRefPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQSourceQuoteRefPriceVolumeLayer(NameIdLookup);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQSourceQuoteRefPriceVolumeLayer(original, NameIdLookup);
}
