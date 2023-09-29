using System;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    public class PQOrderBookLayerFactorySelector : LayerFlagsSelector<IOrderBookLayerFactory, IPQSourceTickerQuoteInfo>, 
        IPQOrderBookLayerFactorySelector
    {
        protected override IOrderBookLayerFactory SelectSimplePriceVolumeLayer(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new PriceVolumeLayerFactory();
        }

        protected override IOrderBookLayerFactory SelectValueDatePriceVolumeLayer(
            IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new ValueDatePriceVolumeLayerFactory();
        }

        protected override IOrderBookLayerFactory SelectSourcePriceVolumeLayer(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            var sourceNameIdLookup = sourceTickerQuoteInfo.SourceNameIdLookup ??
                                     new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                         PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            return new SourcePriceVolumeLayerFactory(sourceNameIdLookup);
        }

        protected override IOrderBookLayerFactory SelectSourceQuoteRefPriceVolumeLayer(
            IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            var sourceNameIdLookup = sourceTickerQuoteInfo.SourceNameIdLookup ??
                                     new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                         PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            return new SourceQuoteRefPriceVolumeLayerFactory(sourceNameIdLookup);
        }

        protected override IOrderBookLayerFactory SelectTraderPriceVolumeLayer(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            var traderNameIdLookup = sourceTickerQuoteInfo.TraderNameIdLookup ??
                                     new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                         PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
            return new TraderPriceVolumeLayerFactory(traderNameIdLookup);
        }

        protected override IOrderBookLayerFactory SelectSourceQuoteRefTraderValueDatePriceVolumeLayer(
            IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            var sourceNameIdLookup = sourceTickerQuoteInfo.SourceNameIdLookup ??
                                     new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                         PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            var traderNameIdLookup = sourceTickerQuoteInfo.TraderNameIdLookup ??
                                     new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                         PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
            return new SourceQuoteRefTraderValueDatePriceVolumeLayerFactory(sourceNameIdLookup, traderNameIdLookup);
        }


        public override IPriceVolumeLayer ConvertToExpectedImplementation(IPriceVolumeLayer checkForConvert, 
            bool clone = false)
        {
            switch (checkForConvert)
            {
                case null:
                    return null;
                case PQPriceVolumeLayer pqPriceVolumeLayer:
                    return clone ? pqPriceVolumeLayer.Clone() : pqPriceVolumeLayer;
                case ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtePvLayer:
                    return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(srcQtRefTrdrVlDtePvLayer);
                case ITraderPriceVolumeLayer trdrPvLayer:
                    return new PQTraderPriceVolumeLayer(trdrPvLayer);
                case IValueDatePriceVolumeLayer valueDatePriceVolumeLayer:
                    return new PQValueDatePriceVolumeLayer(valueDatePriceVolumeLayer);
                case ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer:
                    return new PQSourceQuoteRefPriceVolumeLayer(srcQtRefPvLayer);
                case ISourcePriceVolumeLayer sourcePriceVolumeLayer:
                    return new PQSourcePriceVolumeLayer(sourcePriceVolumeLayer);
                default:
                    return new PQPriceVolumeLayer(checkForConvert);
            }
        }

        public virtual bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType)
        {
            if (copySourceType == typeof(PQPriceVolumeLayer) || copySourceType == typeof(PriceVolumeLayer))
            {
                return true;
            }
            if (copySourceType == typeof(SourcePriceVolumeLayer) ||
                copySourceType == typeof(PQSourcePriceVolumeLayer))
            {
                return copyDestinationType == typeof(PQSourcePriceVolumeLayer) ||
                       copyDestinationType == typeof(PQSourceQuoteRefPriceVolumeLayer) ||
                       copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                copySourceType == typeof(PQSourceQuoteRefPriceVolumeLayer))
            {
                return copyDestinationType == typeof(PQSourceQuoteRefPriceVolumeLayer) ||
                       copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(ValueDatePriceVolumeLayer) ||
                copySourceType == typeof(PQValueDatePriceVolumeLayer))
            {
                return copyDestinationType == typeof(PQValueDatePriceVolumeLayer) ||
                       copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(TraderPriceVolumeLayer) ||
                copySourceType == typeof(PQTraderPriceVolumeLayer))
            {
                return copyDestinationType == typeof(PQTraderPriceVolumeLayer) ||
                       copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer) ||
                copySourceType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer))
            {
                return copyDestinationType == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            return false;
        }

        public virtual IPQPriceVolumeLayer SelectPriceVolumeLayer(IPQPriceVolumeLayer original, IPriceVolumeLayer desired)
        {
            if (desired == null) return null;
            if (original == null)
            {
                var cloneOfSrc = (IPQPriceVolumeLayer)ConvertToExpectedImplementation(desired)?.Clone();
                cloneOfSrc?.Reset();
                return cloneOfSrc;
            }
            if (original.GetType() != desired.GetType() &&
                !TypeCanWholeyContain(desired.GetType(), original.GetType()))
            {
                return new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(original);
            }
            return original;
        }
    }
}