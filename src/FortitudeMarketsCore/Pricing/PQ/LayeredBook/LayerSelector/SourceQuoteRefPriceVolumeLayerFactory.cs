#region

using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public class SourceQuoteRefPriceVolumeLayerFactory : SourcePriceVolumeLayerFactory
{
    public SourceQuoteRefPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() =>
        new PQSourceQuoteRefPriceVolumeLayer(0, 0, SourceNameIdLookup);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQSourceQuoteRefPriceVolumeLayer(original);
}
