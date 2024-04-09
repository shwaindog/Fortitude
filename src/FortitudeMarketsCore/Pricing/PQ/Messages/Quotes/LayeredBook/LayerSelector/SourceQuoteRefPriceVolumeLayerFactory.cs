#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class SourceQuoteRefPriceVolumeLayerFactory : SourcePriceVolumeLayerFactory
{
    public SourceQuoteRefPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQSourceQuoteRefPriceVolumeLayer(0, 0, SourceNameIdLookup);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQSourceQuoteRefPriceVolumeLayer(original);
}
