// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public interface IPQOrderBookLayerFactorySelector :
    ILayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerInfo>
{
    IPQPriceVolumeLayer UpgradeExistingLayer
    (IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
      , LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    IPQPriceVolumeLayer CreateExpectedImplementation
    (LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup
      , IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public class PQOrderBookLayerFactorySelector : LayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerInfo>,
    IPQOrderBookLayerFactorySelector, ISupportsPQNameIdLookupGenerator
{
    static PQOrderBookLayerFactorySelector()
    {
        var pqNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        foreach (var layerType in Enum.GetValues<LayerType>())
        {
            if (layerType == LayerType.None) continue;
            AllowedImplementations.Add(LayerFlagToImplementation(layerType, pqNameIdLookup).GetType());
        }
    }

    public PQOrderBookLayerFactorySelector(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    public IPQPriceVolumeLayer UpgradeExistingLayer
    (IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
      , LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (original == null)
        {
            var cloneOfSrc = CreateExpectedImplementation(desiredLayerType, nameIdLookupGenerator);
            if (copy != null) cloneOfSrc.CopyFrom(copy, copyMergeFlags);
            return cloneOfSrc;
        }

        if ((original.LayerType != desiredLayerType &&
             !OriginalCanWhollyContain(desiredLayerType.SupportedLayerFlags(), original.SupportsLayerFlags))
         || !AllowedImplementations.Contains(original.GetType()))
        {
            var mergeOrginalDesiredLayerFlags = original.SupportsLayerFlags | desiredLayerType.SupportedLayerFlags();
            var mostCompatibleSupportsBoth    = mergeOrginalDesiredLayerFlags.MostCompactLayerType();
            var upgradeLayer                  = CreateExpectedImplementation(mostCompatibleSupportsBoth, nameIdLookupGenerator);
            upgradeLayer.CopyFrom(original, copyMergeFlags);
            if (copy != null) upgradeLayer.CopyFrom(copy, copyMergeFlags);
            return upgradeLayer;
        }

        if (copy != null) original.CopyFrom(copy, copyMergeFlags);

        return original;
    }

    public IPQPriceVolumeLayer CreateExpectedImplementation
    (LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var newLayer = LayerFlagToImplementation(checkForConvert, nameIdLookup);
        if (copy != null) newLayer.CopyFrom(copy, copyMergeFlags);
        return newLayer;
    }

    public override IPriceVolumeLayer CreateExpectedImplementation
        (LayerType desiredLayerType, IPriceVolumeLayer? copy = null, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CreateExpectedImplementation(desiredLayerType, NameIdLookup, copy, copyMergeFlags);

    public override IPriceVolumeLayer UpgradeExistingLayer
    (IPriceVolumeLayer? original, LayerType desiredLayerType, IPriceVolumeLayer? copy = null
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        UpgradeExistingLayer(original as IPQPriceVolumeLayer, NameIdLookup, desiredLayerType, copy, copyMergeFlags);

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public static IPQPriceVolumeLayer LayerFlagToImplementation(LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup)
    {
        var newLayer = checkForConvert switch
                       {
                           LayerType.PriceVolume => new PQPriceVolumeLayer()

                         , LayerType.SourceQuoteRefTraderValueDatePriceVolume => new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdLookup)

                         , LayerType.TraderPriceVolume         => new PQTraderPriceVolumeLayer(nameIdLookup)
                         , LayerType.ValueDatePriceVolume      => new PQValueDatePriceVolumeLayer()
                         , LayerType.SourceQuoteRefPriceVolume => new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup)
                         , LayerType.SourcePriceVolume         => new PQSourcePriceVolumeLayer(nameIdLookup)

                         , _ => new PQPriceVolumeLayer()
                       };
        return newLayer;
    }

    protected override IPQOrderBookLayerFactory SelectSimplePriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) => new PQPriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectValueDatePriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) =>
        new ValueDatePriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectSourcePriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) =>
        SelectSourcePriceVolumeLayer(sourceTickerInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectSourcePriceVolumeLayer
        (IPQSourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourcePriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) =>
        SelectSourceQuoteRefPriceVolumeLayer(sourceTickerInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer
        (IPQSourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourceQuoteRefPriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectTraderPriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) =>
        SelectTraderPriceVolumeLayer(sourceTickerInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectTraderPriceVolumeLayer
        (IPQSourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQTraderPriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(IPQSourceTickerInfo sourceTickerInfo) =>
        SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(sourceTickerInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectSourceQuoteRefTraderValueDatePriceVolumeLayer
        (IPQSourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory(nameIdLookupGenerator);
}
