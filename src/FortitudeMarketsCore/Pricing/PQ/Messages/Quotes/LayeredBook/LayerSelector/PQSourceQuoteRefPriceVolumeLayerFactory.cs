#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQSourceQuoteRefPriceVolumeLayerFactory : PQSourcePriceVolumeLayerFactory
{
    public PQSourceQuoteRefPriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefPriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQSourceQuoteRefPriceVolumeLayer(NameIdLookup);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) => new PQSourceQuoteRefPriceVolumeLayer(original, NameIdLookup);
}
