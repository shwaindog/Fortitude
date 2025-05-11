// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerOrders;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerOrders;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes.LayeredBook;

public class PQBookGenerator : BookGenerator
{
    private readonly IPQNameIdLookupGenerator
        consistentOrderBookNameIdGenerator = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);

    public PQBookGenerator(QuoteBookValuesGenerator quoteBookGenerator, ISourceTickerInfo sourceTickerInfo) : base(quoteBookGenerator, sourceTickerInfo) { }

    public override void InitializeBook(IMutableOrderBookSide newBookSide)
    {
        if (newBookSide is IPQOrderBookSide pqOrderBook)
            pqOrderBook.NameIdLookup.CopyFrom(consistentOrderBookNameIdGenerator, CopyMergeFlags.FullReplace);
        base.InitializeBook(newBookSide);
    }

    protected override void SetPrice(BookSide side, IMutablePriceVolumeLayer priceVolumeLayer, decimal price, decimal? prevPrice)
    {
        if (priceVolumeLayer is IPQPriceVolumeLayer pqOrdersCountPvl) pqOrdersCountPvl.IsPriceUpdated = price != prevPrice;
        base.SetPrice(side, priceVolumeLayer, price, prevPrice);
    }

    protected override void SetVolume(BookSide side, IMutablePriceVolumeLayer priceVolumeLayer, decimal volume, decimal? prevVolume)
    {
        if (priceVolumeLayer is IPQPriceVolumeLayer pqOrdersCountPvl) pqOrdersCountPvl.IsVolumeUpdated = volume != prevVolume;
        base.SetVolume(side, priceVolumeLayer, volume, prevVolume);
    }

    protected override void SetValueDate
        (BookSide side, IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, DateTime newValueDate, DateTime? prevValueDate)
    {
        if (valueDatePriceVolumeLayer is IPQValueDatePriceVolumeLayer pqValueDatePvl)
            pqValueDatePvl.IsValueDateUpdated = newValueDate != prevValueDate;
        base.SetValueDate(side, valueDatePriceVolumeLayer, newValueDate, prevValueDate);
    }

    protected override void SetExecutable(BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, bool executable, bool? prevExecutable)
    {
        var isExecutableUpdated = executable == prevExecutable || !executable;

        if (sourcePriceVolumeLayer is IPQSourcePriceVolumeLayer pqSourcePvl) pqSourcePvl.IsExecutableUpdated                   = isExecutableUpdated;
        if (sourcePriceVolumeLayer is IPQSourceQuoteRefPriceVolumeLayer pqSourceQtRefPvl) pqSourceQtRefPvl.IsExecutableUpdated = isExecutableUpdated;
        if (sourcePriceVolumeLayer is IPQFullSupportPriceVolumeLayer pqSourceQtRefOrderValueDtPvl)
            pqSourceQtRefOrderValueDtPvl.IsExecutableUpdated = isExecutableUpdated;

        base.SetExecutable(side, sourcePriceVolumeLayer, executable, prevExecutable);
    }

    protected override void SetSourceName
        (BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, string sourceName, ushort sourceId, ushort? prevSourceId)
    {
        consistentOrderBookNameIdGenerator.SetIdToName(sourceId, sourceName);
        if (sourcePriceVolumeLayer is IPQSourcePriceVolumeLayer pqBidSrcLyrInfo)
            pqBidSrcLyrInfo.NameIdLookup.CopyFrom(consistentOrderBookNameIdGenerator);
        var isSourceNameUpdated = sourceId == prevSourceId;

        if (sourcePriceVolumeLayer is IPQSourcePriceVolumeLayer pqSourcePvl) pqSourcePvl.IsSourceNameUpdated                   = isSourceNameUpdated;
        if (sourcePriceVolumeLayer is IPQSourceQuoteRefPriceVolumeLayer pqSourceQtRefPvl) pqSourceQtRefPvl.IsSourceNameUpdated = isSourceNameUpdated;
        if (sourcePriceVolumeLayer is IPQFullSupportPriceVolumeLayer pqSourceQtRefOrderValueDtPvl)
            pqSourceQtRefOrderValueDtPvl.IsSourceNameUpdated = isSourceNameUpdated;

        base.SetSourceName(side, sourcePriceVolumeLayer, sourceName, sourceId, prevSourceId);
    }

    protected override void SetOrdersCount
        (BookSide side, IMutableOrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer, uint ordersCount, uint? prevOrdersCount)
    {
        if (ordersCountPriceVolumeLayer is IPQOrdersPriceVolumeLayer pqOrdersCountPvl)
            pqOrdersCountPvl.IsOrdersCountUpdated = ordersCount != prevOrdersCount;
        base.SetOrdersCount(side, ordersCountPriceVolumeLayer, ordersCount, prevOrdersCount);
    }

    protected override void SetInternalVolume
        (BookSide side, IMutableOrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer, decimal internalVolume, decimal? prevInternalVolume)
    {
        if (ordersCountPriceVolumeLayer is IPQOrdersPriceVolumeLayer pqOrdersCountPvl)
            pqOrdersCountPvl.IsInternalVolumeUpdated = internalVolume != prevInternalVolume;
        base.SetInternalVolume(side, ordersCountPriceVolumeLayer, internalVolume, prevInternalVolume);
    }

    protected override void SetOrderId(BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, int orderId, int? prevOrderId)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo) pqAnonOrderLyrInfo.IsOrderIdUpdated = orderId != prevOrderId;
        base.SetOrderId(side, orderLayerInfo, pos, orderId, prevOrderId);
    }

    protected override void SetOrderFlags
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, LayerOrderFlags orderFlags, LayerOrderFlags? prevOrderFlags)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo) pqAnonOrderLyrInfo.IsOrderFlagsUpdated = orderFlags != prevOrderFlags;
        base.SetOrderFlags(side, orderLayerInfo, pos, orderFlags, prevOrderFlags);
    }

    protected override void SetOrderCreatedTime
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, DateTime orderCreatedTime, DateTime? prevOrderCreatedTime)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo)
        {
            pqAnonOrderLyrInfo.IsCreatedTimeDateUpdated = orderCreatedTime.Get2MinIntervalsFromUnixEpoch() != prevOrderCreatedTime?.Get2MinIntervalsFromUnixEpoch();
            pqAnonOrderLyrInfo.IsCreatedTimeSub2MinUpdated = orderCreatedTime.GetSub2MinComponent() != prevOrderCreatedTime?.GetSub2MinComponent();
        }
        base.SetOrderCreatedTime(side, orderLayerInfo, pos, orderCreatedTime, prevOrderCreatedTime);
    }

    protected override void SetOrderUpdatedTime
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, DateTime orderUpdatedTime, DateTime? prevOrderUpdatedTime)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo)
        {
            pqAnonOrderLyrInfo.IsUpdatedTimeDateUpdated = orderUpdatedTime.Get2MinIntervalsFromUnixEpoch() != prevOrderUpdatedTime?.Get2MinIntervalsFromUnixEpoch();
            pqAnonOrderLyrInfo.IsUpdatedTimeSub2MinUpdated = orderUpdatedTime.GetSub2MinComponent() != prevOrderUpdatedTime?.GetSub2MinComponent();
        }
        base.SetOrderUpdatedTime(side, orderLayerInfo, pos, orderUpdatedTime, prevOrderUpdatedTime);
    }

    protected override void SetOrderVolume
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, decimal orderVolume, decimal? prevOrderVolume)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqOrderLayerInfo) pqOrderLayerInfo.IsOrderVolumeUpdated = orderVolume != prevOrderVolume;
        base.SetOrderVolume(side, orderLayerInfo, pos, orderVolume, prevOrderVolume);
    }

    protected override void SetOrderRemainingVolume
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, decimal orderRemainingVolume, decimal? prevOrderRemainingVolume)
    {
        if (orderLayerInfo is IPQAnonymousOrderLayerInfo pqAnonOrderLyrInfo)
            pqAnonOrderLyrInfo.IsOrderRemainingVolumeUpdated = orderRemainingVolume != prevOrderRemainingVolume;
        base.SetOrderRemainingVolume(side, orderLayerInfo, pos, orderRemainingVolume, prevOrderRemainingVolume);
    }

    protected override void SetOrderCounterPartyName
    (BookSide side, IMutableCounterPartyOrderLayerInfo orderLayerInfo, int pos, string counterPartyName,
        int counterPartyId, int? prevCounterPartyNameId)
    {
        consistentOrderBookNameIdGenerator.SetIdToName(counterPartyId, counterPartyName);
        if (orderLayerInfo is IPQCounterPartyOrderLayerInfo pqBidCpOrderLyrInfo)
            pqBidCpOrderLyrInfo.NameIdLookup.CopyFrom(consistentOrderBookNameIdGenerator);
        if (orderLayerInfo is IPQCounterPartyOrderLayerInfo pqCpOrdLyrInfo) pqCpOrdLyrInfo.IsCounterPartyNameUpdated = true;
        base.SetOrderCounterPartyName(side, orderLayerInfo, pos, counterPartyName, counterPartyId, prevCounterPartyNameId);
    }

    protected override void SetOrderTraderName
        (BookSide side, IMutableCounterPartyOrderLayerInfo orderLayerInfo, int pos, string traderName, int traderNameId, int? prevTraderNameId)
    {
        consistentOrderBookNameIdGenerator.SetIdToName(traderNameId, traderName);
        if (orderLayerInfo is IPQCounterPartyOrderLayerInfo pqAskCpOrderLyrInfo)
            pqAskCpOrderLyrInfo.NameIdLookup.CopyFrom(consistentOrderBookNameIdGenerator);
        if (orderLayerInfo is IPQCounterPartyOrderLayerInfo pqCpOrdLyrInfo) pqCpOrdLyrInfo.IsTraderNameUpdated = true;
        base.SetOrderTraderName(side, orderLayerInfo, pos, traderName, traderNameId, prevTraderNameId);
    }
}
