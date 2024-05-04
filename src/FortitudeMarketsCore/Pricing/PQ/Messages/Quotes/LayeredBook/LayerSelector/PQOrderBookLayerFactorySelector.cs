#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public interface IPQOrderBookLayerFactorySelector :
    ILayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerQuoteInfo>
{
    IPQPriceVolumeLayer UpgradeExistingLayer(IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
        , LayerType desiredLayerType, IPriceVolumeLayer? copy = null);
}

public class PQOrderBookLayerFactorySelector : LayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerQuoteInfo>,
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

    public override IPriceVolumeLayer CreateExpectedImplementation(LayerType desiredLayerType, IPriceVolumeLayer? copy = null) =>
        CreateExpectedImplementation(desiredLayerType, NameIdLookup, copy);

    public override IPriceVolumeLayer
        UpgradeExistingLayer(IPriceVolumeLayer? original, LayerType desiredLayerType, IPriceVolumeLayer? copy = null) =>
        UpgradeExistingLayer(original as IPQPriceVolumeLayer, NameIdLookup, desiredLayerType, copy);

    public IPQPriceVolumeLayer UpgradeExistingLayer(IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
        , LayerType desiredLayerType, IPriceVolumeLayer? copy = null)
    {
        if (original == null)
        {
            var cloneOfSrc = CreateExpectedImplementation(desiredLayerType, nameIdLookupGenerator);
            if (copy != null) cloneOfSrc.CopyFrom(copy);
            return cloneOfSrc;
        }

        if ((original.LayerType != desiredLayerType &&
             !OriginalCanWhollyContain(desiredLayerType.SupportedLayerFlags(), original.SupportsLayerFlags))
            || !AllowedImplementations.Contains(original.GetType()))
        {
            var mergeOrginalDesiredLayerFlags = original.SupportsLayerFlags | desiredLayerType.SupportedLayerFlags();
            var mostCompatibleSupportsBoth = mergeOrginalDesiredLayerFlags.MostCompactLayerType();
            var upgradeLayer = CreateExpectedImplementation(mostCompatibleSupportsBoth, nameIdLookupGenerator);
            upgradeLayer.CopyFrom(original);
            if (copy != null) upgradeLayer.CopyFrom(copy);
            return upgradeLayer;
        }

        if (copy != null) original.CopyFrom(copy);

        return original;
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public IPQPriceVolumeLayer CreateExpectedImplementation(LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup
        , IPriceVolumeLayer? copy = null)
    {
        var newLayer = LayerFlagToImplementation(checkForConvert, nameIdLookup);
        if (copy != null) newLayer.CopyFrom(copy);
        return newLayer;
    }

    public static IPQPriceVolumeLayer LayerFlagToImplementation(LayerType checkForConvert, IPQNameIdLookupGenerator nameIdLookup)
    {
        var newLayer = checkForConvert switch
        {
            LayerType.PriceVolume => new PQPriceVolumeLayer()
            , LayerType.SourceQuoteRefTraderValueDatePriceVolume => new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdLookup)
            , LayerType.TraderPriceVolume => new PQTraderPriceVolumeLayer(nameIdLookup)
            , LayerType.ValueDatePriceVolume => new PQValueDatePriceVolumeLayer()
            , LayerType.SourceQuoteRefPriceVolume => new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup)
            , LayerType.SourcePriceVolume => new PQSourcePriceVolumeLayer(nameIdLookup)
            , _ => new PQPriceVolumeLayer()
        };
        return newLayer;
    }

    protected override IPQOrderBookLayerFactory SelectSimplePriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new PQPriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory SelectValueDatePriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new ValueDatePriceVolumeLayerFactory();

    protected override IPQOrderBookLayerFactory
        SelectSourcePriceVolumeLayer(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        SelectSourcePriceVolumeLayer(sourceTickerQuoteInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory
        SelectSourcePriceVolumeLayer(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourcePriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        SelectSourceQuoteRefPriceVolumeLayer(sourceTickerQuoteInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourceQuoteRefPriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectTraderPriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        SelectTraderPriceVolumeLayer(sourceTickerQuoteInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectTraderPriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQTraderPriceVolumeLayerFactory(nameIdLookupGenerator);

    protected override IPQOrderBookLayerFactory SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(sourceTickerQuoteInfo, NameIdLookup);

    protected IPQOrderBookLayerFactory SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory(nameIdLookupGenerator);
}
