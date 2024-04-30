#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public interface IPQOrderBookLayerFactorySelector :
    ILayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerQuoteInfo>
{
    bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);

    IPQPriceVolumeLayer? SelectPriceVolumeLayer(IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
        , IPriceVolumeLayer? desired);
}

public class PQOrderBookLayerFactorySelector : LayerFlagsSelector<IPQOrderBookLayerFactory, IPQSourceTickerQuoteInfo>,
    IPQOrderBookLayerFactorySelector, ISupportsPQNameIdLookupGenerator
{
    public PQOrderBookLayerFactorySelector(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    public override IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer? priceVolumeLayer, bool clone = false) =>
        ConvertToExpectedImplementation(priceVolumeLayer, NameIdLookup, clone);

    public virtual bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PQPriceVolumeLayer) || copySourceType == typeof(PriceVolumeLayer)) return true;
        if (copySourceType == typeof(SourcePriceVolumeLayer) ||
            copySourceType == typeof(PQSourcePriceVolumeLayer))
            return copyDestinationType == typeof(PQSourcePriceVolumeLayer) ||
                   copyDestinationType == typeof(PQSourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer) ||
            copySourceType == typeof(PQSourceQuoteRefPriceVolumeLayer))
            return copyDestinationType == typeof(PQSourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(ValueDatePriceVolumeLayer) ||
            copySourceType == typeof(PQValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(PQValueDatePriceVolumeLayer) ||
                   copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(TraderPriceVolumeLayer) ||
            copySourceType == typeof(PQTraderPriceVolumeLayer))
            return copyDestinationType == typeof(PQTraderPriceVolumeLayer) ||
                   copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer) ||
            copySourceType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
        return false;
    }

    public virtual IPQPriceVolumeLayer? SelectPriceVolumeLayer(IPQPriceVolumeLayer? original, IPQNameIdLookupGenerator nameIdLookupGenerator
        , IPriceVolumeLayer? desired)
    {
        if (desired == null) return null;
        if (original == null)
        {
            var cloneOfSrc = (IPQPriceVolumeLayer?)ConvertToExpectedImplementation(desired, nameIdLookupGenerator, true);
            cloneOfSrc?.StateReset();
            return cloneOfSrc;
        }

        if (original.GetType() != desired.GetType() &&
            !TypeCanWholeyContain(desired.GetType(), original.GetType()))
            return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(original, nameIdLookupGenerator);
        return original;
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer? checkForConvert, IPQNameIdLookupGenerator nameIdLookup,
        bool clone = false)
    {
        switch (checkForConvert)
        {
            case null:
                return new PQPriceVolumeLayer();
            case PQPriceVolumeLayer pqPriceVolumeLayer:
            {
                var origOrClone = clone ? pqPriceVolumeLayer.Clone() : pqPriceVolumeLayer;
                if (origOrClone is ISupportsPQNameIdLookupGenerator hasNameIdGen) hasNameIdGen.NameIdLookup = nameIdLookup;
                return origOrClone;
            }
            case ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtePvLayer:
                return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(srcQtRefTrdrVlDtePvLayer, nameIdLookup);
            case ITraderPriceVolumeLayer trdrPvLayer:
                return new PQTraderPriceVolumeLayer(trdrPvLayer, nameIdLookup);
            case IValueDatePriceVolumeLayer valueDatePriceVolumeLayer:
                return new PQValueDatePriceVolumeLayer(valueDatePriceVolumeLayer);
            case ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer:
                return new PQSourceQuoteRefPriceVolumeLayer(srcQtRefPvLayer, nameIdLookup);
            case ISourcePriceVolumeLayer sourcePriceVolumeLayer:
                return new PQSourcePriceVolumeLayer(sourcePriceVolumeLayer, nameIdLookup);
            default:
                return new PQPriceVolumeLayer(checkForConvert);
        }
    }

    public virtual IPQPriceVolumeLayer? SelectPriceVolumeLayer(IPQPriceVolumeLayer? original, IPriceVolumeLayer? desired) =>
        SelectPriceVolumeLayer(original, NameIdLookup, desired);

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
