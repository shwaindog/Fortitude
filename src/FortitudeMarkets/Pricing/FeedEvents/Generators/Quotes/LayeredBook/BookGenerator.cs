// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.MidPrice;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

public interface IBookGenerator
{
    QuoteBookValuesGenerator QuoteBookGenerator { get; }

    void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, MidPriceTimePair midPriceTimePair);
}

public class BookGenerator : IBookGenerator
{
    private readonly ISourceTickerInfo sourceTickerInfo;

    public BookGenerator(QuoteBookValuesGenerator quoteBookGenerator, ISourceTickerInfo sourceTickerInfo)
    {
        this.sourceTickerInfo = sourceTickerInfo;
        QuoteBookGenerator    = quoteBookGenerator;
    }

    public QuoteBookValuesGenerator QuoteBookGenerator { get; }

    public void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, MidPriceTimePair midPriceTimePair)
    {
        var (roundedTopBid, roundedTopAsk) = QuoteBookGenerator.TopBidAskPrice;

        var maxLayersToGenerate = Math.Min(sourceTickerInfo.MaximumPublishedLayers
                                         , QuoteBookGenerator.BookGenerationInfo.NumberOfBookLayers);
        PopulateBook(level2Quote.OrderBook.BidSide, roundedTopBid, maxLayersToGenerate, midPriceTimePair);
        PopulateBook(level2Quote.OrderBook.AskSide, roundedTopAsk, maxLayersToGenerate, midPriceTimePair);
    }

    public virtual void InitializeBook(IMutableOrderBookSide newBookSide) { }

    public void PopulateBook
        (IMutableOrderBookSide newBookSide, decimal startingPrice, int maxLayersToGenerate, MidPriceTimePair midPriceTimePair)
    {
        InitializeBook(newBookSide);
        var missedCount = 0;

        var layerIndex = 0;
        for (var i = 0; i < maxLayersToGenerate; i++)
        {
            decimal layerVol;
            if (newBookSide.BookSide == BookSide.BidBook)
            {
                layerVol = QuoteBookGenerator.BidVolumeAt(i);
                if (layerVol > 0)
                {
                    missedCount = 0;
                    while (newBookSide.Capacity <= i) newBookSide.AppendEntryAtEnd();
                    var entry = newBookSide[layerIndex++]!;
                    entry.Price  = QuoteBookGenerator.BidPriceAt(i);
                    entry.Volume = layerVol;
                    SetBidLayerValues(entry, i, midPriceTimePair);
                }
            }
            else
            {
                layerVol = QuoteBookGenerator.AskVolumeAt(i);
                if (layerVol > 0)
                {
                    missedCount = 0;
                    while (newBookSide.Capacity <= i) newBookSide.AppendEntryAtEnd();
                    var entry = newBookSide[layerIndex++]!;
                    entry.Price  = QuoteBookGenerator.AskPriceAt(i);
                    entry.Volume = layerVol;
                    SetAskLayerValues(entry, i, midPriceTimePair);
                }
            }
            if (layerVol == 0)
            {
                if (QuoteBookGenerator.BookGenerationInfo.AllowEmptySlotGaps) layerIndex++;
                if (++missedCount > 3) break;
            }
        }
    }


    protected virtual void SetPrice
        (BookSide side, IMutablePriceVolumeLayer priceVolumeLayer, decimal price, decimal? prevPrice)
    {
        priceVolumeLayer.Price = price;
    }

    protected virtual void SetVolume
        (BookSide side, IMutablePriceVolumeLayer priceVolumeLayer, decimal volume, decimal? prevVolume)
    {
        priceVolumeLayer.Volume = volume;
    }

    public void SetBidLayerValues(IPriceVolumeLayer bookLayer, int depth, MidPriceTimePair midPriceTimePair)
    {
        PopulatePriceVolumeLayer(bookLayer, depth, BookSide.BidBook);
    }

    public void SetAskLayerValues(IPriceVolumeLayer bookLayer, int depth, MidPriceTimePair midPriceTimePair)
    {
        PopulatePriceVolumeLayer(bookLayer, depth, BookSide.AskBook);
    }

    protected virtual void PopulatePriceVolumeLayer(IPriceVolumeLayer bookLayer, int depth, BookSide side)
    {
        var layerType = bookLayer.LayerType;

        switch (layerType)
        {
            case LayerType.None:
            case LayerType.PriceVolume: // already done in BookGenerator
                return;
            case LayerType.ValueDatePriceVolume:
                PopulateValueDate((IMutableValueDatePriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.SourcePriceVolume:
                PopulateSourceName((IMutableSourcePriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.SourceQuoteRefPriceVolume:
                PopulateSourceQuoteRef((IMutableSourceQuoteRefPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.OrdersCountPriceVolume:
                PopulateOrdersCountPriceVolume((IMutableOrdersCountPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.OrdersAnonymousPriceVolume:
                PopulateAnonymousOrdersPriceVolume((IMutableOrdersPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.OrdersFullPriceVolume:
                PopulateCounterPartyOrdersPriceVolume((IMutableOrdersPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.FullSupportPriceVolume:
                PopulateSourceQuoteRefTraderPriceVolume((IMutableFullSupportPriceVolumeLayer)bookLayer, depth, side);
                return;
        }
    }

    protected virtual void PopulateValueDate(IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, int depth, BookSide side)
    {
        var newValueDate = side switch
                           {
                               BookSide.AskBook => QuoteBookGenerator.AskValueDateAt(depth)
                             , BookSide.BidBook => QuoteBookGenerator.BidValueDateAt(depth)
                             , _                => valueDatePriceVolumeLayer.ValueDate
                           };
        var previousValueDate = side switch
                                {
                                    BookSide.AskBook => QuoteBookGenerator.PreviousAskValueDateAt(QuoteBookGenerator.AskPriceAt(depth), depth)
                                  , BookSide.BidBook => QuoteBookGenerator.PreviousBidValueDateAt(QuoteBookGenerator.BidPriceAt(depth), depth)
                                  , _                => null
                                };
        SetValueDate(side, valueDatePriceVolumeLayer, newValueDate, previousValueDate);
    }

    protected virtual void SetValueDate
        (BookSide side, IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, DateTime newValueDate, DateTime? prevValueDate)
    {
        valueDatePriceVolumeLayer.ValueDate = newValueDate;
    }

    protected virtual void PopulateSourceName(IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, int depth, BookSide side)
    {
        switch (side)
        {
            case BookSide.AskBook:
                SetExecutable(side, sourcePriceVolumeLayer, QuoteBookGenerator.AskExecutableAt(depth)
                            , QuoteBookGenerator.PreviousAskExecutableAt(QuoteBookGenerator.AskPriceAt(depth), depth));
                SetSourceName(side, sourcePriceVolumeLayer, QuoteBookGenerator.AskSourceNameAt(depth), QuoteBookGenerator.AskSourceIdAt(depth)
                            , QuoteBookGenerator.PreviousAskSourceIdAt(QuoteBookGenerator.AskPriceAt(depth), depth));
                break;
            case BookSide.BidBook:
                SetExecutable(side, sourcePriceVolumeLayer, QuoteBookGenerator.BidExecutableAt(depth)
                            , QuoteBookGenerator.PreviousBidExecutableAt(QuoteBookGenerator.BidPriceAt(depth), depth));
                SetSourceName(side, sourcePriceVolumeLayer, QuoteBookGenerator.BidSourceNameAt(depth), QuoteBookGenerator.BidSourceIdAt(depth)
                            , QuoteBookGenerator.PreviousBidSourceIdAt(QuoteBookGenerator.BidPriceAt(depth), depth));
                break;
        }
    }

    protected virtual void SetExecutable(BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, bool executable, bool? prevExecutable)
    {
        sourcePriceVolumeLayer.Executable = executable;
    }

    protected virtual void SetSourceName
        (BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, string sourceName, ushort sourceId, ushort? prevSourceId)
    {
        sourcePriceVolumeLayer.SourceName = sourceName;
    }

    protected virtual void PopulateSourceQuoteRef
        (IMutableSourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer, int depth, BookSide side)
    {
        var sourceQtRef =
            side switch
            {
                BookSide.AskBook => QuoteBookGenerator.AskQuoteRefAt(depth)
              , BookSide.BidBook => QuoteBookGenerator.BidQuoteRefAt(depth)
              , _                => sourceQuoteRefPriceVolumeLayer.SourceQuoteReference
            };
        var prevSourceQtRef = side switch
                              {
                                  BookSide.AskBook => QuoteBookGenerator.PreviousAskQuoteRefAt(QuoteBookGenerator.AskPriceAt(depth), depth)
                                , BookSide.BidBook => QuoteBookGenerator.PreviousAskQuoteRefAt(QuoteBookGenerator.BidPriceAt(depth), depth)
                                , _                => null
                              };
        SetSourceQuoteRef(side, sourceQuoteRefPriceVolumeLayer, sourceQtRef, prevSourceQtRef);
        PopulateSourceName(sourceQuoteRefPriceVolumeLayer, depth, side);
    }

    protected virtual void SetSourceQuoteRef
        (BookSide side, IMutableSourceQuoteRefPriceVolumeLayer sourcePriceVolumeLayer, uint sourceQuoteRef, uint? prevQuoteRef)
    {
        sourcePriceVolumeLayer.SourceQuoteReference = sourceQuoteRef;
    }


    protected virtual void PopulateOrdersCountPriceVolume
        (IMutableOrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer, int depth, BookSide side)
    {
        uint  ordersCountOnLayer;
        uint? prevOrdersCountOnLayer;
        switch (side)
        {
            case BookSide.AskBook:
                ordersCountOnLayer     = QuoteBookGenerator.AskOrdersCountAt(depth);
                prevOrdersCountOnLayer = QuoteBookGenerator.PreviousAskOrdersCountAt(QuoteBookGenerator.AskPriceAt(depth));
                break;
            case BookSide.BidBook:
                ordersCountOnLayer     = QuoteBookGenerator.BidOrdersCountAt(depth);
                prevOrdersCountOnLayer = QuoteBookGenerator.PreviousBidOrdersCountAt(QuoteBookGenerator.BidPriceAt(depth));
                break;
            default:
                ordersCountOnLayer     = 0;
                prevOrdersCountOnLayer = 0;
                break;
        }
        SetOrdersCount(side, ordersCountPriceVolumeLayer, ordersCountOnLayer, prevOrdersCountOnLayer);

        ordersCountPriceVolumeLayer.OrdersCount = ordersCountOnLayer;
        decimal  internalVolumeOnLayer;
        decimal? prevInternalVolumeOnLayer;
        switch (side)
        {
            case BookSide.AskBook:
                internalVolumeOnLayer     = QuoteBookGenerator.AskInternalVolumeAt(depth);
                prevInternalVolumeOnLayer = QuoteBookGenerator.PreviousAskInternalVolumeAt(QuoteBookGenerator.AskPriceAt(depth));
                break;
            case BookSide.BidBook:
                internalVolumeOnLayer     = QuoteBookGenerator.BidInternalVolumeAt(depth);
                prevInternalVolumeOnLayer = QuoteBookGenerator.PreviousAskInternalVolumeAt(QuoteBookGenerator.BidPriceAt(depth));
                break;
            default:
                internalVolumeOnLayer     = 0;
                prevInternalVolumeOnLayer = 0;
                break;
        }
        SetInternalVolume(side, ordersCountPriceVolumeLayer, internalVolumeOnLayer, prevInternalVolumeOnLayer);
    }


    protected virtual void SetOrdersCount
        (BookSide side, IMutableOrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer, uint ordersCount, uint? prevOrdersCount)
    {
        ordersCountPriceVolumeLayer.OrdersCount = ordersCount;
    }


    protected virtual void SetInternalVolume
        (BookSide side, IMutableOrdersCountPriceVolumeLayer ordersCountPriceVolumeLayer, decimal internalVolume, decimal? prevInternalVolume)
    {
        ordersCountPriceVolumeLayer.InternalVolume = internalVolume;
    }


    protected virtual void PopulateAnonymousOrdersPriceVolume
        (IMutableOrdersPriceVolumeLayer ordersPriceVolumeLayer, int depth, BookSide side)
    {
        var ordersCountOnLayer = side switch
                                 {
                                     BookSide.AskBook => QuoteBookGenerator.AskOrdersCountAt(depth)
                                   , BookSide.BidBook => QuoteBookGenerator.BidOrdersCountAt(depth)
                                   , _                => (uint)0
                                 };

        if (ordersCountOnLayer == 0) return;
        for (var i = 0; i < ordersCountOnLayer; i++)
        {
            var orderLayer = ordersPriceVolumeLayer[i]!;
            SetAnonymousOrderValues(depth, side, i, orderLayer);
        }
    }

    protected virtual void SetAnonymousOrderValues(int depth, BookSide side, int i, IMutableAnonymousOrderLayerInfo orderLayer)
    {
        switch (side)
        {
            case BookSide.AskBook:
                var askPrice                = QuoteBookGenerator.AskPriceAt(depth);
                var askOrderId              = QuoteBookGenerator.AskOrderIdAt(depth, i);
                var askOrderFlags           = QuoteBookGenerator.AskOrderFlagsAt(depth, askOrderId, i);
                var askOrderCreatedTime     = QuoteBookGenerator.AskOrderCreatedTimeAt(depth, askOrderId, i);
                var askOrderUpdatedTime     = QuoteBookGenerator.AskOrderUpdatedTimeAt(depth, askOrderId, i);
                var askOrderVolume          = QuoteBookGenerator.AskOrderVolumeAt(depth, i);
                var askOrderRemainingVolume = QuoteBookGenerator.AskOrderRemainingVolumeAt(depth, i);
                SetOrderId(side, orderLayer, i, askOrderId
                         , QuoteBookGenerator.PreviousAskOrderIdAt(askPrice, i));
                SetOrderFlags(side, orderLayer, i, askOrderFlags
                            , QuoteBookGenerator.PreviousAskOrderFlagsAt(askPrice, askOrderId, depth));
                SetOrderCreatedTime(side, orderLayer, i, askOrderCreatedTime
                                  , QuoteBookGenerator.PreviousAskOrderCreatedTimeAt(askPrice, askOrderId, depth));
                SetOrderUpdatedTime(side, orderLayer, i, askOrderUpdatedTime
                                  , QuoteBookGenerator.PreviousAskOrderUpdatedTimeAt(askPrice, askOrderId, depth));
                SetOrderVolume(side, orderLayer, i, askOrderVolume
                             , QuoteBookGenerator.PreviousAskOrderVolumeAt(askPrice, askOrderId, depth));
                SetOrderRemainingVolume(side, orderLayer, i, askOrderRemainingVolume
                                      , QuoteBookGenerator.PreviousAskOrderRemainingVolumeAt(askPrice, askOrderId, depth));
                break;
            case BookSide.BidBook:
                var bidPrice                = QuoteBookGenerator.BidPriceAt(depth);
                var bidOrderId              = QuoteBookGenerator.BidOrderIdAt(depth, i);
                var bidOrderFlags           = QuoteBookGenerator.BidOrderFlagsAt(depth, bidOrderId, i);
                var bidOrderCreatedTime     = QuoteBookGenerator.BidOrderCreatedTimeAt(depth, bidOrderId, i);
                var bidOrderUpdatedTime     = QuoteBookGenerator.BidOrderUpdatedTimeAt(depth, bidOrderId, i);
                var bidOrderVolume          = QuoteBookGenerator.BidOrderVolumeAt(depth, i);
                var bidOrderRemainingVolume = QuoteBookGenerator.BidOrderRemainingVolumeAt(depth, i);
                SetOrderId(side, orderLayer, i, bidOrderId
                         , QuoteBookGenerator.PreviousBidOrderIdAt(bidPrice, i));
                SetOrderFlags(side, orderLayer, i, bidOrderFlags
                            , QuoteBookGenerator.PreviousBidOrderFlagsAt(bidPrice, bidOrderId, depth));
                SetOrderCreatedTime(side, orderLayer, i, bidOrderCreatedTime
                                  , QuoteBookGenerator.PreviousBidOrderCreatedTimeAt(bidPrice, bidOrderId, depth));
                SetOrderUpdatedTime(side, orderLayer, i, bidOrderUpdatedTime
                                  , QuoteBookGenerator.PreviousBidOrderUpdatedTimeAt(bidPrice, bidOrderId, depth));
                SetOrderVolume(side, orderLayer, i, bidOrderVolume
                             , QuoteBookGenerator.PreviousBidOrderVolumeAt(bidPrice, bidOrderId, depth));
                SetOrderRemainingVolume(side, orderLayer, i, bidOrderRemainingVolume
                                      , QuoteBookGenerator.PreviousBidOrderRemainingVolumeAt(bidPrice, bidOrderId, depth));
                break;
        }
    }


    protected virtual void PopulateCounterPartyOrdersPriceVolume
        (IMutableOrdersPriceVolumeLayer ordersPriceVolumeLayer, int depth, BookSide side)
    {
        var ordersCountOnLayer = side switch
                                 {
                                     BookSide.AskBook => QuoteBookGenerator.AskOrdersCountAt(depth)
                                   , BookSide.BidBook => QuoteBookGenerator.BidOrdersCountAt(depth)
                                   , _                => (uint)0
                                 };

        if (ordersCountOnLayer == 0) return;
        for (var i = 0; i < ordersCountOnLayer; i++)
        {
            var orderLayer = (IMutableCounterPartyOrderLayerInfo)ordersPriceVolumeLayer[i]!;
            SetAnonymousOrderValues(depth, side, i, orderLayer);

            switch (side)
            {
                case BookSide.AskBook:
                    var askPrice                   = QuoteBookGenerator.AskPriceAt(depth);
                    var askOrderId                 = QuoteBookGenerator.AskOrderIdAt(depth, i);
                    var askOrderCounterPartyName   = QuoteBookGenerator.AskOrderCounterPartyNameAt(depth, askOrderId, i);
                    var askOrderTraderName         = QuoteBookGenerator.AskOrderTraderNameAt(depth, askOrderId, i);
                    var askOrderCounterPartyIdAt   = QuoteBookGenerator.AskOrderCounterPartyIdAt(depth, askOrderId, i);
                    var previousAskOrderCpIdAt     = QuoteBookGenerator.PreviousAskOrderCounterPartyIdAt(askPrice, askOrderId, i);
                    var askOrderTraderIdAt         = QuoteBookGenerator.AskOrderTraderIdAt(depth, askOrderId, i);
                    var previousAskOrderTraderIdAt = QuoteBookGenerator.PreviousAskOrderTraderIdAt(askPrice, askOrderId, i);
                    SetOrderCounterPartyName(side, orderLayer, i, askOrderCounterPartyName, askOrderCounterPartyIdAt, previousAskOrderCpIdAt);
                    SetOrderTraderName(side, orderLayer, i, askOrderTraderName, askOrderTraderIdAt, previousAskOrderTraderIdAt);

                    break;
                case BookSide.BidBook:
                    var bidPrice                   = QuoteBookGenerator.BidPriceAt(depth);
                    var bidOrderId                 = QuoteBookGenerator.BidOrderIdAt(depth, i);
                    var bidOrderCounterPartyName   = QuoteBookGenerator.BidOrderCounterPartyNameAt(depth, bidOrderId, i);
                    var bidOrderTraderName         = QuoteBookGenerator.BidOrderTraderNameAt(depth, bidOrderId, i);
                    var bidOrderCounterPartyIdAt   = QuoteBookGenerator.BidOrderCounterPartyIdAt(depth, bidOrderId, i);
                    var bidOrderTraderIdAt         = QuoteBookGenerator.BidOrderTraderIdAt(depth, bidOrderId, i);
                    var previousBidOrderCpIdAt     = QuoteBookGenerator.PreviousBidOrderCounterPartyIdAt(bidPrice, bidOrderId, i);
                    var previousBidOrderTraderIdAt = QuoteBookGenerator.PreviousBidOrderTraderIdAt(bidPrice, bidOrderId, i);
                    SetOrderCounterPartyName(side, orderLayer, i, bidOrderCounterPartyName, bidOrderCounterPartyIdAt, previousBidOrderCpIdAt);
                    SetOrderTraderName(side, orderLayer, i, bidOrderTraderName, bidOrderTraderIdAt, previousBidOrderTraderIdAt);
                    break;
            }
        }
    }

    protected virtual void SetOrderId
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, int orderId, int? prevOrderId)
    {
        orderLayerInfo.OrderId = orderId;
    }

    protected virtual void SetOrderFlags
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, LayerOrderFlags orderFlags, LayerOrderFlags? prevOrderFlags)
    {
        orderLayerInfo.OrderFlags = orderFlags;
    }

    protected virtual void SetOrderCreatedTime
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, DateTime orderCreatedTime, DateTime? prevOrderCreatedTime)
    {
        orderLayerInfo.CreatedTime = orderCreatedTime;
    }

    protected virtual void SetOrderUpdatedTime
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, DateTime orderUpdatedTime, DateTime? prevOrderUpdatedTime)
    {
        orderLayerInfo.UpdatedTime = orderUpdatedTime;
    }

    protected virtual void SetOrderVolume
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, decimal orderVolume, decimal? prevOrderVolume)
    {
        orderLayerInfo.OrderVolume = orderVolume;
    }

    protected virtual void SetOrderRemainingVolume
        (BookSide side, IMutableAnonymousOrderLayerInfo orderLayerInfo, int pos, decimal orderRemainingVolume, decimal? prevOrderRemainingVolume)
    {
        orderLayerInfo.OrderRemainingVolume = orderRemainingVolume;
    }

    protected virtual void SetOrderCounterPartyName
    (BookSide side, IMutableCounterPartyOrderLayerInfo orderLayerInfo, int pos, string counterPartyName, int counterPartyId
      , int? prevCounterPartyNameId)
    {
        orderLayerInfo.ExternalCounterPartyName = counterPartyName;
    }

    protected virtual void SetOrderTraderName
        (BookSide side, IMutableCounterPartyOrderLayerInfo orderLayerInfo, int pos, string traderName, int traderNameId, int? prevTraderNameId)
    {
        orderLayerInfo.ExternalTraderName = traderName;
    }


    protected virtual void PopulateSourceQuoteRefTraderPriceVolume
    (
        IMutableFullSupportPriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer
      , int depth
      , BookSide side
    )
    {
        PopulateOrdersCountPriceVolume(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
        PopulateSourceQuoteRef(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
        PopulateValueDate(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
    }
}
