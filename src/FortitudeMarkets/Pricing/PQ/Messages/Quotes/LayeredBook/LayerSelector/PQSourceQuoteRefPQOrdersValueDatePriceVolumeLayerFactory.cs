// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQSourceQuoteRefPQOrdersValueDatePriceVolumeLayerFactory : PQOrdersPriceVolumeLayerFactory
{
    public PQSourceQuoteRefPQOrdersValueDatePriceVolumeLayerFactory(IPQNameIdLookupGenerator nameIdLookup)
        : base(LayerType.SourceQuoteRefOrdersValueDatePriceVolume, nameIdLookup) { }

    public override Type LayerCreationType => typeof(PQSourceQuoteRefOrdersValueDatePriceVolumeLayer);

    public override IPQPriceVolumeLayer CreateNewLayer() => new PQSourceQuoteRefOrdersValueDatePriceVolumeLayer(NameIdLookup, 0, 0);

    public override IPQPriceVolumeLayer UpgradeLayer(IPQPriceVolumeLayer original) =>
        new PQSourceQuoteRefOrdersValueDatePriceVolumeLayer(original, NameIdLookup);
}
