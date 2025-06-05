// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

public interface IPQOrderBookLayerFactorySelector : ILayerFlagsSelector<IPQOrderBookLayerFactory>
{
    IPQPriceVolumeLayer UpgradeExistingLayer(IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator, QuoteLayerInstantBehaviorFlags layerBehavior
      , LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    IPQPriceVolumeLayer CreateExpectedImplementation(LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup, QuoteLayerInstantBehaviorFlags layerBehavior
      , IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public class PQOrderBookLayerFactorySelector : LayerFlagsSelector<IPQOrderBookLayerFactory>,
    IPQOrderBookLayerFactorySelector, ISupportsPQNameIdLookupGenerator
{
    static PQOrderBookLayerFactorySelector()
    {
        var pqNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType, pqNameIdLookup).GetType());
        }
    }

    public PQOrderBookLayerFactorySelector(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    public IPQPriceVolumeLayer UpgradeExistingLayer
    (IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator, QuoteLayerInstantBehaviorFlags layerBehavior
      , LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (original == null)
        {
            var cloneOfSrc = CreateExpectedImplementation(desiredLayerType, nameIdLookupGenerator, layerBehavior);
            if (copy != null) cloneOfSrc.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
            return cloneOfSrc;
        }

        if ((original.LayerType != desiredLayerType &&
             !OriginalCanWhollyContain(desiredLayerType.SupportedLayerFlags(), original.SupportsLayerFlags))
         || !AllowedImplementations.Contains(original.GetType()))
        {
            var mergeOrginalDesiredLayerFlags = original.SupportsLayerFlags | desiredLayerType.SupportedLayerFlags();
            var mostCompatibleSupportsBoth    = mergeOrginalDesiredLayerFlags.MostCompactLayerType();
            var upgradeLayer                  = CreateExpectedImplementation(mostCompatibleSupportsBoth, nameIdLookupGenerator, layerBehavior);
            upgradeLayer.CopyFrom(original, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
            if (copy != null) upgradeLayer.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
            return upgradeLayer;
        }

        if (copy != null) original.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);

        return original;
    }

    public IPQPriceVolumeLayer CreateExpectedImplementation
    (LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var newLayer = LayerFlagToImplementation(checkForConvert, nameIdLookup);
        if (copy != null) newLayer.CopyFrom(copy, (QuoteInstantBehaviorFlags)layerBehavior, copyMergeFlags);
        return newLayer;
    }

    public override IMutablePriceVolumeLayer CreateExpectedImplementation
        (LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CreateExpectedImplementation(desiredLayerType, NameIdLookup, layerBehavior, copy, copyMergeFlags);

    public override IMutablePriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType, QuoteLayerInstantBehaviorFlags layerBehavior, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        UpgradeExistingLayer(original as IPQPriceVolumeLayer, NameIdLookup, layerBehavior, desiredLayerType, copy, copyMergeFlags);

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public static IPQPriceVolumeLayer LayerFlagToImplementation(LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup)
    {
        var newLayer = checkForConvert switch
                       {
                           LayerType.PriceVolume                => new PQPriceVolumeLayer()
                         , LayerType.SourcePriceVolume          => new PQSourcePriceVolumeLayer(nameIdLookup)
                         , LayerType.SourceQuoteRefPriceVolume  => new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup)
                         , LayerType.ValueDatePriceVolume       => new PQValueDatePriceVolumeLayer()
                         , LayerType.OrdersCountPriceVolume     => new PQOrdersCountPriceVolumeLayer()
                         , LayerType.OrdersAnonymousPriceVolume => new PQOrdersPriceVolumeLayer(checkForConvert, nameIdLookup)
                         , LayerType.OrdersFullPriceVolume      => new PQOrdersPriceVolumeLayer(checkForConvert, nameIdLookup)

                         , LayerType.FullSupportPriceVolume => new PQFullSupportPriceVolumeLayer(nameIdLookup)

                         , _ => new PQPriceVolumeLayer()
                       };
        return newLayer;
    }

    protected override IPQOrderBookLayerFactory SelectSimplePriceVolumeLayer() => new PQPriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectValueDatePriceVolumeLayer() =>
        new ValueDatePriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectSourcePriceVolumeLayer() => new PQSourcePriceVolumeLayerFactory(NameIdLookup);


    protected override IPQOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer() =>
        new PQSourceQuoteRefPriceVolumeLayerFactory(NameIdLookup);


    protected override IPQOrderBookLayerFactory SelectOrdersCountPriceVolumeLayer() =>
        new PQOrdersCountPriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectAnonymousOrdersPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior) =>
        new PQOrdersPriceVolumeLayerFactory(LayerType.OrdersAnonymousPriceVolume, NameIdLookup, layerBehavior);

    protected override IPQOrderBookLayerFactory SelectCounterPartyOrdersPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior) =>
        new PQOrdersPriceVolumeLayerFactory(LayerType.OrdersFullPriceVolume, NameIdLookup, layerBehavior);


    protected override IPQOrderBookLayerFactory SelectFullSupportPriceVolumeLayer(QuoteLayerInstantBehaviorFlags layerBehavior) =>
        new PQFullSupportPriceVolumeLayerFactory(NameIdLookup, layerBehavior);
}
