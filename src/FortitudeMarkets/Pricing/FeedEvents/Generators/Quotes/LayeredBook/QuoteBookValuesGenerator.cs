// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LayeredBook;

public struct SnapshotBookGeneratedValues
{
    public (decimal, decimal)? TopBidAskPrice;

    public decimal?  BidAskSpread;
    public decimal?  MidPrice;
    public DateTime? ValueDate;
    public uint?     BatchId;
    public uint?     QuoteReference;

    public SnapshotLayerGeneratedValues[] BidLayers;
    public SnapshotLayerGeneratedValues[] AskLayers;

    public SnapshotBookGeneratedValues(int maxLayersToGenerate, int maxTradersPerLayer)
    {
        BidLayers = new SnapshotLayerGeneratedValues[maxLayersToGenerate];
        AskLayers = new SnapshotLayerGeneratedValues[maxLayersToGenerate];
        for (var i = 0; i < maxLayersToGenerate; i++)
        {
            BidLayers[i] = new SnapshotLayerGeneratedValues(new List<SnapshotOrderLayerGeneratedValues>(maxTradersPerLayer));
            AskLayers[i] = new SnapshotLayerGeneratedValues(new List<SnapshotOrderLayerGeneratedValues>(maxTradersPerLayer));
        }
    }
}

public struct SnapshotLayerGeneratedValues(List<SnapshotOrderLayerGeneratedValues>? orders = null)
{
    public decimal?  Price;
    public decimal?  Volume;
    public ushort?   SourceId;
    public bool?     Executable;
    public uint?     SourceQuoteReference;
    public DateTime? ValueDate;
    public uint?     OrdersCount;
    public decimal?  InternalVolume;

    public List<SnapshotOrderLayerGeneratedValues> Orders = orders ?? new List<SnapshotOrderLayerGeneratedValues>();
}

public struct SnapshotOrderLayerGeneratedValues
{
    public int?               OrderId;
    public OrderGenesisFlags? OrderGenesisFlags;
    public DateTime?          CreatedTime;
    public DateTime?          UpdatedTime;
    public int?               CounterPartyId;
    public int?               TraderId;
    public decimal?           OrderVolume;
    public decimal?           OrderRemainingVolume;
}

public class QuoteBookValuesGenerator
{
    protected static readonly ConcurrentDictionary<int, string> CachedSourceNames       = new();
    protected static readonly ConcurrentDictionary<int, string> CachedTraderNames       = new();
    protected static readonly ConcurrentDictionary<int, string> CachedCounterPartyNames = new();

    protected static readonly NameIdLookupGenerator OrderBookNameToId = new();

    private readonly BookGenerationInfo                bookGenerationInfo;
    private readonly CurrentQuoteInstantValueGenerator quoteValueGenerator;

    protected SnapshotBookGeneratedValues CurrentBookValues;
    protected SnapshotBookGeneratedValues PreviousBookValues;

    public QuoteBookValuesGenerator(CurrentQuoteInstantValueGenerator quoteValueGenerator)
    {
        this.quoteValueGenerator = quoteValueGenerator;
        bookGenerationInfo       = quoteValueGenerator.GenerateQuoteInfo.BookGenerationInfo;

        var prev = quoteValueGenerator.CurrentMidPriceTimePair.PreviousMid;
        var curr = quoteValueGenerator.CurrentMidPriceTimePair.CurrentMid;

        var timeBasedPseudoRandom = new Random(prev.Time.GetHashCode() ^ curr.Time.GetHashCode());
        if (prev.Mid == curr.Mid && timeBasedPseudoRandom.NextDouble() < bookGenerationInfo.ChangeOnSameProbability)
        {
            BookPseudoRandom = timeBasedPseudoRandom;
            BookNormalDist   = new Normal(0, 1, BookPseudoRandom);
        }
        else
        {
            BookPseudoRandom = quoteValueGenerator.PseudoRandom;
            BookNormalDist   = quoteValueGenerator.NormalDist;
        }

        var maxLayersToGenerate = Math.Min(quoteValueGenerator.GenerateQuoteInfo.SourceTickerInfo!.MaximumPublishedLayers
                                         , bookGenerationInfo.NumberOfBookLayers);
        CurrentBookValues  = new SnapshotBookGeneratedValues(maxLayersToGenerate, bookGenerationInfo.GenerateBookLayerInfo.AverageOrdersPerLayer);
        PreviousBookValues = new SnapshotBookGeneratedValues(maxLayersToGenerate, bookGenerationInfo.GenerateBookLayerInfo.AverageOrdersPerLayer);
    }

    public BookGenerationInfo BookGenerationInfo => bookGenerationInfo;

    public Normal BookNormalDist   { get; set; }
    public Random BookPseudoRandom { get; set; }

    public virtual decimal BidAskSpread
    {
        get
        {
            CurrentBookValues.BidAskSpread ??=
                Math.Max(bookGenerationInfo.TightestSpreadPips,
                         bookGenerationInfo.AverageSpreadPips +
                         (decimal)BookNormalDist.Sample() * bookGenerationInfo.SpreadStandardDeviation);
            return CurrentBookValues.BidAskSpread!.Value;
        }
    }

    public virtual decimal MidPrice
    {
        get
        {
            CurrentBookValues.MidPrice ??=
                quoteValueGenerator.CurrentMidPriceTimePair.CurrentMid.Mid;
            return CurrentBookValues.MidPrice!.Value;
        }
    }

    public virtual (decimal, decimal) TopBidAskPrice
    {
        get
        {
            if (CurrentBookValues.TopBidAskPrice != null) return CurrentBookValues.TopBidAskPrice!.Value;

            var topBidAskSpread = BidAskSpread;
            var mid             = MidPrice;
            var roundedTopBid   = decimal.Round(mid - topBidAskSpread / 2);
            var roundedTopAsk   = decimal.Round(mid + topBidAskSpread / 2);

            while (roundedTopAsk - roundedTopBid > bookGenerationInfo.TightestSpreadPips) roundedTopBid -= bookGenerationInfo.SmallestPriceLayerPips;

            CurrentBookValues.TopBidAskPrice = (roundedTopBid, roundedTopAsk);
            return CurrentBookValues.TopBidAskPrice!.Value;
        }
    }

    public virtual DateTime BookValueDate => CurrentBookValues.ValueDate ??= GenerateValueDate();

    public virtual uint BookBatchId => CurrentBookValues.BatchId ??= (PreviousBookValues.BatchId ?? 0) + 1;

    public virtual uint BookSourceQuoteRef => CurrentBookValues.QuoteReference ??= GenerateQuoteRef();

    public virtual decimal? PreviousBidVolumeAt(decimal price) => PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price)?.Volume;

    public virtual decimal BidVolumeAt(int depth)
    {
        return CurrentBookValues.BidLayers[depth].Volume ??= GenerateVolumeAt(depth);
    }

    public virtual decimal? PreviousAskVolumeAt(decimal price) => PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price)?.Volume;

    public virtual decimal AskVolumeAt(int depth)
    {
        return CurrentBookValues.AskLayers[depth].Volume ??= GenerateVolumeAt(depth);
    }

    public virtual decimal? PreviousBidPriceWorseThan(decimal price) => PreviousBidLayerWorseThan(price)?.Price;

    public virtual decimal BidPriceAt(int depth)
    {
        if (CurrentBookValues.BidLayers[depth].Price != null) return CurrentBookValues.BidLayers[depth].Price!.Value;
        var (bidTop, _) = TopBidAskPrice;
        var betterDepthPrice   = depth > 0 ? BidPriceAt(depth - 1) : bidTop;
        var prevNextLayerPrice = PreviousBidPriceWorseThan(betterDepthPrice);
        var priceDelta         = depth == 0 ? 0m : -GeneratePriceDelta();
        if (prevNextLayerPrice.HasValue)
            prevNextLayerPrice = betterDepthPrice - prevNextLayerPrice > bookGenerationInfo.MaxPriceLayerPips * bookGenerationInfo.Pip
                ? null
                : prevNextLayerPrice;
        return CurrentBookValues.BidLayers[depth].Price
            ??= decimal.Round(prevNextLayerPrice ?? betterDepthPrice + priceDelta, bookGenerationInfo.PriceRoundingDp);
        ;
    }

    public virtual decimal? PreviousAskPriceWorseThan(decimal price) => PreviousAskLayerWorseThan(price)?.Price;

    public virtual decimal AskPriceAt(int depth)
    {
        if (CurrentBookValues.AskLayers[depth].Price != null) return CurrentBookValues.AskLayers[depth].Price!.Value;
        var (_, askTop) = TopBidAskPrice;
        var betterDepthPrice   = depth > 0 ? AskPriceAt(depth - 1) : askTop;
        var prevNextLayerPrice = PreviousAskPriceWorseThan(betterDepthPrice);
        var priceDelta         = depth == 0 ? 0m : GeneratePriceDelta();
        if (prevNextLayerPrice.HasValue)
            prevNextLayerPrice = prevNextLayerPrice - betterDepthPrice > bookGenerationInfo.MaxPriceLayerPips * bookGenerationInfo.Pip
                ? null
                : prevNextLayerPrice;
        return CurrentBookValues.AskLayers[depth].Price
            ??= decimal.Round(prevNextLayerPrice ?? betterDepthPrice + priceDelta, bookGenerationInfo.PriceRoundingDp);
    }

    public virtual DateTime? PreviousBidValueDateAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.ValueDate;

    public virtual DateTime BidValueDateAt(int depth) =>
        CurrentBookValues.BidLayers[depth].ValueDate ??= PreviousBidValueDateAt(BidPriceAt(depth), depth) ?? GenerateValueDate();

    public virtual DateTime? PreviousAskValueDateAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.ValueDate;

    public virtual DateTime AskValueDateAt(int depth) =>
        CurrentBookValues.AskLayers[depth].ValueDate ??= PreviousAskValueDateAt(AskPriceAt(depth), depth) ?? GenerateValueDate();

    public virtual ushort? PreviousBidSourceIdAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.SourceId;

    public virtual string? PreviousBidSourceNameAt(decimal price, int? pos = null)
    {
        var prevBidSourceId = PreviousBidSourceIdAt(price, pos);
        if (!prevBidSourceId.HasValue) return null;
        return BidNameFromId(prevBidSourceId.Value);
    }

    public virtual string BidSourceNameAt(int depth) => BidNameFromId(BidSourceIdAt(depth));

    public virtual ushort BidSourceIdAt(int depth) =>
        CurrentBookValues.BidLayers[depth].SourceId
            ??= PreviousBidSourceIdAt(BidPriceAt(depth), depth) ?? (ushort)BidIdFromName(GenerateSourceName(true));

    public virtual ushort? PreviousAskSourceIdAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.SourceId;

    public virtual string? PreviousAskSourceNameAt(decimal price, int? pos = null)
    {
        var prevAskSourceId = PreviousAskSourceIdAt(price, pos);
        if (!prevAskSourceId.HasValue) return null;
        return AskNameFromId(prevAskSourceId.Value);
    }

    public virtual string AskSourceNameAt(int depth) => AskNameFromId(AskSourceIdAt(depth));

    public virtual ushort AskSourceIdAt(int depth) =>
        CurrentBookValues.AskLayers[depth].SourceId
            ??= PreviousAskSourceIdAt(AskPriceAt(depth), depth) ?? (ushort)AskIdFromName(GenerateSourceName(false));

    public virtual bool? PreviousBidExecutableAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.Executable;

    public virtual bool BidExecutableAt(int depth) =>
        CurrentBookValues.BidLayers[depth].Executable ??= PreviousBidExecutableAt(BidPriceAt(depth), depth) ?? GenerateExecutable();

    public virtual bool? PreviousAskExecutableAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.Executable;

    public virtual bool AskExecutableAt(int depth) =>
        CurrentBookValues.AskLayers[depth].Executable ??= PreviousAskExecutableAt(AskPriceAt(depth), depth) ?? GenerateExecutable();

    public virtual uint? PreviousBidQuoteRefAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.SourceQuoteReference;

    public virtual uint BidQuoteRefAt(int depth) =>
        CurrentBookValues.BidLayers[depth].SourceQuoteReference ??= PreviousBidQuoteRefAt(BidPriceAt(depth), depth) ?? GenerateQuoteRef();

    public virtual uint? PreviousAskQuoteRefAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.SourceQuoteReference;

    public virtual uint AskQuoteRefAt(int depth) =>
        CurrentBookValues.AskLayers[depth].SourceQuoteReference ??= PreviousAskQuoteRefAt(AskPriceAt(depth), depth) ?? GenerateQuoteRef();

    public virtual uint? PreviousBidOrdersCountAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.OrdersCount;

    public virtual uint BidOrdersCountAt(int depth)
    {
        var bidOrderCount = CurrentBookValues.BidLayers[depth].OrdersCount
            ??= PreviousBidOrdersCountAt(BidPriceAt(depth), depth) ?? GenerateOrdersCount();
        return bidOrderCount;
    }

    public virtual uint? PreviousAskOrdersCountAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.OrdersCount;

    public virtual uint AskOrdersCountAt(int depth)
    {
        var askOrdersCount = CurrentBookValues.AskLayers[depth].OrdersCount ??=
            PreviousAskOrdersCountAt(AskPriceAt(depth), depth) ?? GenerateOrdersCount();
        return askOrdersCount;
    }

    public virtual decimal? PreviousInternalVolumeAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.InternalVolume;

    public virtual decimal BidInternalVolumeAt(int depth) =>
        CurrentBookValues.BidLayers[depth].InternalVolume
            ??= PreviousInternalVolumeAt(BidPriceAt(depth), depth) ?? GenerateInternalVolume(depth, BookSide.BidBook);

    public virtual decimal? PreviousAskInternalVolumeAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.InternalVolume;

    public virtual decimal AskInternalVolumeAt(int depth) =>
        CurrentBookValues.AskLayers[depth].InternalVolume
            ??= PreviousAskInternalVolumeAt(AskPriceAt(depth), depth) ?? GenerateInternalVolume(depth, BookSide.BidBook);

    public virtual int? PreviousBidOrderIdAt(decimal price, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderAtPos(previousLayer.Value, orderPos);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderId;
    }

    public virtual int BidOrderIdAt(int depth, int orderPos)
    {
        var bidOrdersAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidOrdersAtDepth.Count; i <= orderPos; i++) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        orderDepthPos.OrderId      ??= PreviousBidOrderIdAt(BidPriceAt(depth), orderPos) ?? GenerateOrderId();
        bidOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderId.Value;
    }

    public virtual int? PreviousAskOrderIdAt(decimal price, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderAtPos(previousLayer.Value, orderPos);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderId;
    }

    public virtual int AskOrderIdAt(int depth, int orderPos)
    {
        var askOrdersAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askOrdersAtDepth.Count; i <= orderPos; i++) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = askOrdersAtDepth[orderPos];
        orderDepthPos.OrderId      ??= PreviousAskOrderIdAt(AskPriceAt(depth), orderPos) ?? GenerateOrderId();
        askOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderId.Value;
    }

    public virtual OrderGenesisFlags? PreviousBidOrderGenesisFlagsAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderGenesisFlags;
    }

    public virtual OrderGenesisFlags BidOrderGenesisFlagsAt(int depth, int orderId, int orderPos)
    {
        var bidOrdersAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidOrdersAtDepth.Count; i <= orderPos; i++) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        orderDepthPos.OrderGenesisFlags   ??= PreviousBidOrderGenesisFlagsAt(BidPriceAt(depth), orderId, orderPos) ?? GenerateOrderGenesisFlags();
        bidOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderGenesisFlags.Value;
    }

    public virtual OrderGenesisFlags? PreviousAskOrderGenesisFlagsAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderGenesisFlags;
    }

    public virtual OrderGenesisFlags AskOrderGenesisFlagsAt(int depth, int orderId, int orderPos)
    {
        var askOrdersAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askOrdersAtDepth.Count; i <= orderPos; i++) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = askOrdersAtDepth[orderPos];
        orderDepthPos.OrderGenesisFlags   ??= PreviousAskOrderGenesisFlagsAt(AskPriceAt(depth), orderId, orderPos) ?? GenerateOrderGenesisFlags();
        askOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderGenesisFlags.Value;
    }

    public virtual DateTime? PreviousBidOrderCreatedTimeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.CreatedTime;
    }

    public virtual DateTime BidOrderCreatedTimeAt(int depth, int orderId, int orderPos)
    {
        var bidOrdersAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidOrdersAtDepth.Count; i <= orderPos; i++) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        orderDepthPos.CreatedTime  ??= PreviousBidOrderCreatedTimeAt(BidPriceAt(depth), orderId, orderPos) ?? GenerateOrderCreatedTime();
        bidOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.CreatedTime.Value;
    }

    public virtual DateTime? PreviousAskOrderCreatedTimeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.CreatedTime;
    }

    public virtual DateTime AskOrderCreatedTimeAt(int depth, int orderId, int orderPos)
    {
        var askOrdersAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askOrdersAtDepth.Count; i <= orderPos; i++) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = askOrdersAtDepth[orderPos];
        orderDepthPos.CreatedTime  ??= PreviousAskOrderCreatedTimeAt(AskPriceAt(depth), orderId, orderPos) ?? GenerateOrderCreatedTime();
        askOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.CreatedTime.Value;
    }

    public virtual DateTime? PreviousBidOrderUpdatedTimeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.UpdatedTime;
    }

    public virtual DateTime BidOrderUpdatedTimeAt(int depth, int orderId, int orderPos)
    {
        var bidOrdersAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidOrdersAtDepth.Count; i <= orderPos; i++) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        orderDepthPos.UpdatedTime  ??= PreviousBidOrderUpdatedTimeAt(BidPriceAt(depth), orderId, orderPos) ?? GenerateOrderUpdatedTime();
        bidOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.UpdatedTime.Value;
    }

    public virtual DateTime? PreviousAskOrderUpdatedTimeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.UpdatedTime;
    }

    public virtual DateTime AskOrderUpdatedTimeAt(int depth, int orderId, int orderPos)
    {
        var askOrdersAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askOrdersAtDepth.Count; i <= orderPos; i++) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = askOrdersAtDepth[orderPos];
        orderDepthPos.UpdatedTime  ??= PreviousAskOrderUpdatedTimeAt(AskPriceAt(depth), orderId, orderPos) ?? GenerateOrderUpdatedTime();
        askOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.UpdatedTime.Value;
    }

    public virtual decimal? PreviousBidOrderVolumeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderVolume;
    }

    public virtual decimal BidOrderVolumeAt(int depth, int orderPos)
    {
        var bidOrdersAtDepth     = CurrentBookValues.BidLayers[depth].Orders;
        var totalAllocatedVolume = 0m;
        var orderCountAtDepth    = BidOrdersCountAt(depth);
        for (var i = 0; i < orderCountAtDepth && i < ushort.MaxValue; i++)
        {
            if (bidOrdersAtDepth.Count <= i) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
            totalAllocatedVolume += bidOrdersAtDepth[i].OrderVolume ?? 0m;
        }
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        var bidPrice      = BidPriceAt(depth);
        var orderId       = orderDepthPos.OrderId ??= PreviousBidOrderIdAt(bidPrice, orderPos) ?? GenerateOrderId();
        var bidVolume     = BidVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousBidVolumeAt(bidPrice) ?? 0) > bidVolume &&
                                      (PreviousBidOrdersCountAt(bidPrice) ?? 0) > BidOrdersCountAt(depth);
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? bidVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousBidTraderVolumeAt(bidPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume);
        orderDepthPos.OrderVolume  ??= GenerateOrderVolume(orderPos, orderCountAtDepth, bidVolume);
        bidOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderVolume!.Value;
    }

    public virtual decimal? PreviousAskOrderVolumeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderVolume;
    }

    public virtual decimal AskOrderVolumeAt(int depth, int orderPos)
    {
        var askOrdersAtDepth     = CurrentBookValues.AskLayers[depth].Orders;
        var totalAllocatedVolume = 0m;
        var orderCountAtDepth    = AskOrdersCountAt(depth);
        for (var i = 0; i < orderCountAtDepth && i < ushort.MaxValue; i++)
        {
            if (askOrdersAtDepth.Count <= i) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
            totalAllocatedVolume += askOrdersAtDepth[i].OrderVolume ?? 0m;
        }
        var orderDepthPos = askOrdersAtDepth[orderPos];
        var askPrice      = AskPriceAt(depth);
        var orderId       = orderDepthPos.OrderId ??= PreviousAskOrderIdAt(askPrice, orderPos) ?? GenerateOrderId();
        var askVolume     = AskVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousAskVolumeAt(askPrice) ?? 0) > askVolume &&
                                      (PreviousAskOrdersCountAt(askPrice) ?? 0) > orderCountAtDepth;
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? askVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousAskTraderVolumeAt(askPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume);
        orderDepthPos.OrderVolume  ??= GenerateOrderVolume(orderPos, orderCountAtDepth, askVolume);
        askOrdersAtDepth[orderPos] =   orderDepthPos;
        return orderDepthPos.OrderVolume!.Value;
    }

    public virtual decimal? PreviousBidOrderRemainingVolumeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderRemainingVolume;
    }

    public virtual decimal BidOrderRemainingVolumeAt(int depth, int orderPos)
    {
        var bidOrdersAtDepth     = CurrentBookValues.BidLayers[depth].Orders;
        var totalAllocatedVolume = 0m;
        var orderCountAtDepth    = BidOrdersCountAt(depth);
        for (var i = 0; i < orderCountAtDepth && i < ushort.MaxValue; i++)
        {
            if (bidOrdersAtDepth.Count <= i) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
            totalAllocatedVolume += bidOrdersAtDepth[i].OrderVolume ?? 0m;
        }
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        var bidPrice      = BidPriceAt(depth);
        var orderId       = orderDepthPos.OrderId ??= PreviousBidOrderIdAt(bidPrice, orderPos) ?? GenerateOrderId();
        var bidVolume     = BidVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousBidVolumeAt(bidPrice) ?? 0) > bidVolume &&
                                      (PreviousBidOrdersCountAt(bidPrice) ?? 0) > BidOrdersCountAt(depth);
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? bidVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousBidTraderVolumeAt(bidPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume);
        orderDepthPos.OrderRemainingVolume ??= GenerateOrderRemainingVolume(orderPos, orderCountAtDepth, bidVolume);
        bidOrdersAtDepth[orderPos]         =   orderDepthPos;
        return orderDepthPos.OrderRemainingVolume!.Value;
    }

    public virtual decimal? PreviousAskOrderRemainingVolumeAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.OrderVolume;
    }

    public virtual decimal AskOrderRemainingVolumeAt(int depth, int orderPos)
    {
        var askOrdersAtDepth     = CurrentBookValues.AskLayers[depth].Orders;
        var totalAllocatedVolume = 0m;
        var orderCountAtDepth    = AskOrdersCountAt(depth);
        for (var i = 0; i < orderCountAtDepth && i < ushort.MaxValue; i++)
        {
            if (askOrdersAtDepth.Count <= i) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
            totalAllocatedVolume += askOrdersAtDepth[i].OrderVolume ?? 0m;
        }
        var orderDepthPos = askOrdersAtDepth[orderPos];
        var askPrice      = AskPriceAt(depth);
        var orderId       = orderDepthPos.OrderId ??= PreviousAskOrderIdAt(askPrice, orderPos) ?? GenerateOrderId();
        var askVolume     = AskVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousAskVolumeAt(askPrice) ?? 0) > askVolume &&
                                      (PreviousAskOrdersCountAt(askPrice) ?? 0) > orderCountAtDepth;
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? askVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousAskTraderVolumeAt(askPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume);
        orderDepthPos.OrderRemainingVolume ??= GenerateOrderRemainingVolume(orderPos, orderCountAtDepth, askVolume);
        askOrdersAtDepth[orderPos]         =   orderDepthPos;
        return orderDepthPos.OrderRemainingVolume!.Value;
    }

    public virtual int? PreviousBidOrderCounterPartyIdAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.CounterPartyId;
    }

    public virtual string? PreviousBidOrderCounterPartyNameAt(decimal price, int orderId, int orderPos)
    {
        var prevBidCounterPartyId = PreviousBidOrderCounterPartyIdAt(price, orderId, orderPos);
        if (!prevBidCounterPartyId.HasValue) return null;
        return BidNameFromId(prevBidCounterPartyId.Value);
    }

    public virtual int BidOrderCounterPartyIdAt(int depth, int orderId, int orderPos)
    {
        var bidOrdersAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidOrdersAtDepth.Count; i <= orderPos && i < ushort.MaxValue; i++) bidOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = bidOrdersAtDepth[orderPos];
        orderDepthPos.CounterPartyId ??= PreviousBidOrderCounterPartyIdAt(BidPriceAt(depth), orderId, orderPos)
                                      ?? BidIdFromName(GenerateCounterPartyName(true));
        bidOrdersAtDepth[orderPos] = orderDepthPos;
        return orderDepthPos.CounterPartyId.Value;
    }

    public virtual string BidOrderCounterPartyNameAt
        (int depth, int orderId, int orderPos) =>
        BidNameFromId(BidOrderCounterPartyIdAt(depth, orderId, orderPos));

    public virtual int? PreviousAskOrderCounterPartyIdAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.CounterPartyId;
    }

    public virtual string? PreviousAskOrderCounterPartyNameAt(decimal price, int orderId, int orderPos)
    {
        var prevAskOrderCounterPartyId = PreviousAskOrderCounterPartyIdAt(price, orderId, orderPos);
        if (!prevAskOrderCounterPartyId.HasValue) return null;
        return AskNameFromId(prevAskOrderCounterPartyId.Value);
    }

    public virtual int AskOrderCounterPartyIdAt(int depth, int orderId, int orderPos)
    {
        var askOrdersAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askOrdersAtDepth.Count; i <= orderPos && i < ushort.MaxValue; i++) askOrdersAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var orderDepthPos = askOrdersAtDepth[orderPos];
        orderDepthPos.CounterPartyId ??= PreviousAskOrderCounterPartyIdAt(AskPriceAt(depth), orderId, orderPos)
                                      ?? AskIdFromName(GenerateCounterPartyName(false));
        askOrdersAtDepth[orderPos] = orderDepthPos;
        return orderDepthPos.CounterPartyId.Value;
    }

    public virtual string AskOrderCounterPartyNameAt
        (int depth, int orderId, int orderPos) =>
        AskNameFromId(AskOrderCounterPartyIdAt(depth, orderId, orderPos));

    public virtual int? PreviousBidOrderTraderIdAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.TraderId;
    }

    public virtual string? PreviousBidOrderTraderNameAt(decimal price, int orderId, int orderPos)
    {
        var prevBidTraderId = PreviousBidOrderTraderIdAt(price, orderId, orderPos);
        if (!prevBidTraderId.HasValue) return null;
        return BidNameFromId(prevBidTraderId.Value);
    }

    public virtual int BidOrderTraderIdAt(int depth, int orderId, int orderPos)
    {
        var bidTraderAtDepth = CurrentBookValues.BidLayers[depth].Orders;
        for (var i = bidTraderAtDepth.Count; i <= orderPos && i < ushort.MaxValue; i++) bidTraderAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var traderDepthPos = bidTraderAtDepth[orderPos];
        traderDepthPos.TraderId ??= PreviousBidOrderTraderIdAt(BidPriceAt(depth), orderId, orderPos)
                                 ?? BidIdFromName(GenerateTraderName(true));
        bidTraderAtDepth[orderPos] = traderDepthPos;
        return traderDepthPos.TraderId.Value;
    }

    public virtual string BidOrderTraderNameAt
        (int depth, int orderId, int orderPos) =>
        BidNameFromId(BidOrderTraderIdAt(depth, orderId, orderPos));

    public virtual int? PreviousAskOrderTraderIdAt(decimal price, int orderId, int orderPos)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price);
        if (previousLayer == null) return null;
        var orderLayer = PreviousOrderLayerWithId(previousLayer.Value, orderId);
        if (orderLayer == null) return null;
        return orderLayer.Value.TraderId;
    }

    public virtual string? PreviousAskOrderTraderNameAt(decimal price, int orderId, int orderPos)
    {
        var prevAskTraderId = PreviousAskOrderTraderIdAt(price, orderId, orderPos);
        if (!prevAskTraderId.HasValue) return null;
        return AskNameFromId(prevAskTraderId.Value);
    }

    public virtual int AskOrderTraderIdAt(int depth, int orderId, int orderPos)
    {
        var askTraderAtDepth = CurrentBookValues.AskLayers[depth].Orders;
        for (var i = askTraderAtDepth.Count; i <= orderPos && i < ushort.MaxValue; i++) askTraderAtDepth.Add(new SnapshotOrderLayerGeneratedValues());
        var traderDepthPos = askTraderAtDepth[orderPos];
        traderDepthPos.TraderId ??= PreviousAskOrderTraderIdAt(AskPriceAt(depth), orderId, orderPos)
                                 ?? AskIdFromName(GenerateTraderName(false));
        askTraderAtDepth[orderPos] = traderDepthPos;
        return traderDepthPos.TraderId.Value;
    }

    public virtual string AskOrderTraderNameAt
        (int depth, int orderId, int orderPos) =>
        AskNameFromId(AskOrderTraderIdAt(depth, orderId, orderPos));


    public virtual void NextBookValuesInitialise()
    {
        var maxLayersToGenerate = Math.Min(quoteValueGenerator.GenerateQuoteInfo.SourceTickerInfo!.MaximumPublishedLayers
                                         , bookGenerationInfo.NumberOfBookLayers);
        CurrentBookValues.BidAskSpread          = null;
        CurrentBookValues.MidPrice              = null;
        CurrentBookValues.TopBidAskPrice        = null;
        (PreviousBookValues, CurrentBookValues) = (CurrentBookValues, PreviousBookValues);
        EnsureDepthSupported(maxLayersToGenerate);
    }

    protected virtual SnapshotOrderLayerGeneratedValues? PreviousOrderAtPos(SnapshotLayerGeneratedValues previousLayer, int orderPos)
    {
        var layerOrders = previousLayer.Orders;
        var orderLayer  = layerOrders.Skip(orderPos).FirstOrDefault();
        return orderLayer;
    }

    protected virtual SnapshotOrderLayerGeneratedValues? PreviousOrderLayerWithId(SnapshotLayerGeneratedValues previousLayer, int orderId)
    {
        var layerOrders = previousLayer.Orders;
        var orderLayer  = layerOrders.FirstOrDefault(solgv => solgv.OrderId == orderId);
        return orderLayer;
    }


    protected virtual uint GenerateOrdersCount() =>
        (uint)Math.Clamp(Math.Max(0, bookGenerationInfo.GenerateBookLayerInfo.AverageOrdersPerLayer +
                                     (int)(BookNormalDist.Sample() *
                                           bookGenerationInfo.GenerateBookLayerInfo.OrdersCountPerLayerStandardDeviation)),
                         bookGenerationInfo.GenerateBookLayerInfo.MinOrdersPerLayer,
                         bookGenerationInfo.GenerateBookLayerInfo.MaxOrdersPerLayer);

    protected virtual decimal GenerateInternalVolume(int depth, BookSide side) =>
        (uint)BookPseudoRandom.NextDouble() < bookGenerationInfo.GenerateBookLayerInfo.OrderIsInternalProbability
            ? side == BookSide.AskBook
                ? AskOrderVolumeAt(depth, 0)
                : BidOrderVolumeAt(depth, 0)
            : 0m;

    protected virtual SnapshotLayerGeneratedValues? PreviousBidLayerWorseThan(decimal price) =>
        PreviousBookValues.BidLayers.FirstOrDefault(slgv => slgv.Price < price);

    protected virtual SnapshotLayerGeneratedValues? PreviousAskLayerWorseThan(decimal price) =>
        PreviousBookValues.AskLayers.FirstOrDefault(slgv => slgv.Price > price);

    protected virtual SnapshotLayerGeneratedValues? PreviousClosestLayerWithPrice
        (SnapshotLayerGeneratedValues[] layers, decimal price, int? orderPos = null)
    {
        if (orderPos != null && layers.Count(slgv => slgv.Price == price) > 1)
        {
            var checkLayer = layers[orderPos!.Value];
            if (layers.Length > orderPos && checkLayer.Price == price) return checkLayer;
            var countDown = orderPos;

            SnapshotLayerGeneratedValues? closestMatch = null;
            for (var i = 0; i < orderPos && i < layers.Length; i++)
            {
                var layer = layers[i];
                if (layer.Price == price)
                {
                    if (countDown <= 0)
                        return layer;
                    else
                        closestMatch = layer;
                }
                countDown--;
            }
            return closestMatch;
        }
        else
        {
            var previousLayer = layers.FirstOrDefault(slgv => slgv.Price == price);
            if (previousLayer.Price != price) return null;
            return previousLayer;
        }
    }

    protected virtual uint GenerateQuoteRef() =>
        (uint)((quoteValueGenerator.CurrentMidPriceTimePair.CurrentMid.Time.Ticks / TimeSpan.TicksPerMillisecond) & 0xFFFF_FFFF);

    protected virtual bool GenerateExecutable() => BookPseudoRandom.NextDouble() < bookGenerationInfo.GenerateBookLayerInfo.ExecutableProbability;

    protected virtual ushort GenerateSourceNumber() =>
        (ushort)BookPseudoRandom.Next(1, bookGenerationInfo.GenerateBookLayerInfo.MaxNumberOfSourceNames + 1);

    protected virtual string GenerateSourceName(bool isBid)
    {
        var number     = GenerateSourceNumber();
        var sourceName = CachedSourceNames.GetOrAdd(number, num => $"SourceName_{num}");
        switch (isBid)
        {
            case true:  BidIdFromName(sourceName); break;
            case false: AskIdFromName(sourceName); break;
        }
        return sourceName;
    }

    protected virtual int GenerateOrderId() =>
        (int)((quoteValueGenerator.CurrentMidPriceTimePair.CurrentMid.Time.Ticks / TimeSpan.TicksPerMillisecond) & 0x7FFF_FFFF);

    protected virtual OrderGenesisFlags GenerateOrderGenesisFlags() =>
        (OrderGenesisFlags)((quoteValueGenerator.CurrentMidPriceTimePair.CurrentMid.Time.Ticks / TimeSpan.TicksPerMillisecond) & 0x03FF_FFFF);

    protected virtual DateTime GenerateOrderCreatedTime() => DateTime.Now;
    protected virtual DateTime GenerateOrderUpdatedTime() => DateTime.Now;

    protected virtual int GenerateCounterPartyNumber() =>
        BookPseudoRandom.Next(1, bookGenerationInfo.GenerateBookLayerInfo.MaxNumberOfUniqueCounterPartyNames + 1);

    protected virtual int GenerateTraderNumber() =>
        BookPseudoRandom.Next(1, bookGenerationInfo.GenerateBookLayerInfo.MaxNumberOfUniqueTraderNames + 1);

    protected virtual int AskIdFromName(string name)
    {
        var value = OrderBookNameToId.GetOrAddId(name);
        // Console.Out.WriteLine($"Set AskBookNameId[{value}] = {name}");
        return value;
        // BidIdFromName(name);
    }

    protected virtual int BidIdFromName(string name)
    {
        var value = OrderBookNameToId.GetOrAddId(name);
        // Console.Out.WriteLine($"Set JointBookNameId[{value}] = {name}");
        // Console.Out.WriteLine($"Set BidBookNameId[{value}] = {name}");
        return value;
    }

    protected virtual string AskNameFromId(int nameId) => OrderBookNameToId[nameId]!;

    protected virtual string BidNameFromId(int nameId) => OrderBookNameToId[nameId]!;

    protected virtual string GenerateCounterPartyName(bool isBid)
    {
        var number           = GenerateCounterPartyNumber();
        var counterPartyName = CachedCounterPartyNames.GetOrAdd(number, num => $"CounterParty_{num}");
        switch (isBid)
        {
            case true:  BidIdFromName(counterPartyName); break;
            case false: AskIdFromName(counterPartyName); break;
        }
        return counterPartyName;
    }

    protected virtual string GenerateTraderName(bool isBid)
    {
        var number     = GenerateTraderNumber();
        var traderName = CachedTraderNames.GetOrAdd(number, num => $"TraderName_{num}");
        switch (isBid)
        {
            case true:  BidIdFromName(traderName); break;
            case false: AskIdFromName(traderName); break;
        }
        return traderName;
    }

    protected virtual decimal GenerateOrderVolume
        (int pos, uint ordersCount, decimal remainingUnallocatedVolume) =>
        // pos == tradersAtDepth - 1 || tradersAtDepth == 0
        //     ? remainingUnallocatedVolume
        //     : remainingUnallocatedVolume / Math.Max(1, tradersAtDepth);
        decimal.Round(remainingUnallocatedVolume / Math.Max(1, ordersCount), 2);

    protected virtual decimal GenerateOrderRemainingVolume
        (int pos, uint ordersCount, decimal remainingUnallocatedVolume) =>
        decimal.Round(remainingUnallocatedVolume / Math.Max(1, ordersCount), 2);

    protected virtual DateTime GenerateValueDate()
    {
        var index = BookPseudoRandom.Next(0, bookGenerationInfo.GenerateBookLayerInfo.CandidateValueDateAddDays.Length);
        var date =
            quoteValueGenerator
                .CurrentMidPriceTimePair.CurrentMid.Time
                .AddDays(bookGenerationInfo.GenerateBookLayerInfo.CandidateValueDateAddDays[index]).Date;
        return date;
    }

    protected virtual decimal GenerateVolumeAt(int depth) =>
        decimal.Round(Math.Clamp(
                                 bookGenerationInfo.AverageTopOfBookVolume
                               + Math.Max(bookGenerationInfo.HighestVolumeLayer - CurrentBookValues.BidLayers.Length
                                        , bookGenerationInfo.HighestVolumeLayer - Math.Abs(depth - bookGenerationInfo.HighestVolumeLayer)) *
                                 bookGenerationInfo.AverageDeltaVolumePerLayer
                               + (decimal)BookNormalDist.Sample() * bookGenerationInfo.MaxVolumeVariance
                               , 100m, bookGenerationInfo.HighestLayerAverageVolume), bookGenerationInfo.VolumeRoundingDp);

    protected virtual decimal GeneratePriceDelta() =>
        Math.Clamp(Math.Ceiling((bookGenerationInfo.AverageLayerPips +
                                 (decimal)BookNormalDist.Sample() * bookGenerationInfo.AverageLayerPips) /
                                bookGenerationInfo.SmallestPriceLayerPips) * bookGenerationInfo.SmallestPriceLayerPips
                 , bookGenerationInfo.SmallestPriceLayerPips, bookGenerationInfo.MaxPriceLayerPips) *
        bookGenerationInfo.Pip;

    private void EnsureDepthSupported(int requireQuoteLayers)
    {
        if (CurrentBookValues.BidLayers.Length < requireQuoteLayers)
        {
            var increasedSize = new SnapshotLayerGeneratedValues[requireQuoteLayers];
            for (var i = 0; i < CurrentBookValues.BidLayers.Length; i++) increasedSize[i].Orders = CurrentBookValues.BidLayers[i].Orders;
            CurrentBookValues.BidLayers = increasedSize;
        }
        if (CurrentBookValues.AskLayers.Length < requireQuoteLayers)
        {
            var increasedSize = new SnapshotLayerGeneratedValues[requireQuoteLayers];
            for (var i = 0; i < CurrentBookValues.AskLayers.Length; i++) increasedSize[i].Orders = CurrentBookValues.AskLayers[i].Orders;
            CurrentBookValues.AskLayers = increasedSize;
        }
        for (var i = 0; i < requireQuoteLayers; i++)
        {
            var bidLayer = CurrentBookValues.BidLayers[i];
            CurrentBookValues.BidLayers[i] = new SnapshotLayerGeneratedValues(bidLayer.Orders);
            for (var j = 0; j < bookGenerationInfo.GenerateBookLayerInfo.AverageOrdersPerLayer; j++)
                bidLayer.Orders.Add(new SnapshotOrderLayerGeneratedValues());

            var askLayer = CurrentBookValues.AskLayers[i];
            CurrentBookValues.AskLayers[i] = new SnapshotLayerGeneratedValues(askLayer.Orders);
            for (var j = 0; j < bookGenerationInfo.GenerateBookLayerInfo.AverageOrdersPerLayer; j++)
                askLayer.Orders.Add(new SnapshotOrderLayerGeneratedValues());
        }
    }
}
