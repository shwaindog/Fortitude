// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

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
            BidLayers[i] = new SnapshotLayerGeneratedValues(new List<SnapshotLayerTraderGeneratedValues>(maxTradersPerLayer));
            AskLayers[i] = new SnapshotLayerGeneratedValues(new List<SnapshotLayerTraderGeneratedValues>(maxTradersPerLayer));
        }
    }
}

public struct SnapshotLayerGeneratedValues(List<SnapshotLayerTraderGeneratedValues>? traders = null)
{
    public decimal?  Price;
    public decimal?  Volume;
    public ushort?   SourceId;
    public bool?     Executable;
    public uint?     SourceQuoteReference;
    public DateTime? ValueDate;
    public int?      NumTraders;

    public List<SnapshotLayerTraderGeneratedValues> Traders = traders ?? new List<SnapshotLayerTraderGeneratedValues>();
}

public struct SnapshotLayerTraderGeneratedValues(decimal? traderVolume = null, int? traderId = null)
{
    public SnapshotLayerTraderGeneratedValues(SnapshotLayerTraderGeneratedValues toClone)
        : this(toClone.TraderVolume, toClone.TraderId) { }

    public int?     TraderId;
    public decimal? TraderVolume;
}

public class QuoteBookValuesGenerator
{
    protected static readonly ConcurrentDictionary<int, string> CachedSourceNames = new();
    protected static readonly ConcurrentDictionary<int, string> CachedTraderNames = new();

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
        CurrentBookValues  = new SnapshotBookGeneratedValues(maxLayersToGenerate, bookGenerationInfo.GenerateBookLayerInfo.AverageTradersPerLayer);
        PreviousBookValues = new SnapshotBookGeneratedValues(maxLayersToGenerate, bookGenerationInfo.GenerateBookLayerInfo.AverageTradersPerLayer);
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
        return CurrentBookValues.BidLayers[depth].Price ??= prevNextLayerPrice ?? betterDepthPrice + priceDelta;
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
        return CurrentBookValues.AskLayers[depth].Price ??= prevNextLayerPrice ?? betterDepthPrice + priceDelta;
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
        return GenerateSourceName(prevBidSourceId.Value);
    }

    public virtual string BidSourceNameAt(int depth) => GenerateSourceName(BidSourceIdAt(depth));

    public virtual ushort BidSourceIdAt(int depth) =>
        CurrentBookValues.BidLayers[depth].SourceId ??= PreviousBidSourceIdAt(BidPriceAt(depth), depth) ?? GenerateSourceNumber();

    public virtual ushort? PreviousAskSourceIdAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.SourceId;

    public virtual string? PreviousAskSourceNameAt(decimal price, int? pos = null)
    {
        var prevAskSourceId = PreviousAskSourceIdAt(price, pos);
        if (!prevAskSourceId.HasValue) return null;
        return GenerateSourceName(prevAskSourceId.Value);
    }

    public virtual string AskSourceNameAt(int depth) => GenerateSourceName(AskSourceIdAt(depth));

    public virtual ushort AskSourceIdAt(int depth) =>
        CurrentBookValues.AskLayers[depth].SourceId ??= PreviousAskSourceIdAt(AskPriceAt(depth), depth) ?? GenerateSourceNumber();

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

    public virtual int? PreviousBidNumOfTradersAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos)?.NumTraders;

    public virtual int BidNumOfTradersAt(int depth) =>
        CurrentBookValues.BidLayers[depth].NumTraders ??= PreviousBidNumOfTradersAt(BidPriceAt(depth), depth) ?? GenerateNumOfTraders();

    public virtual int? PreviousAskNumOfTradersAt(decimal price, int? pos = null) =>
        PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos)?.NumTraders;

    public virtual int AskNumOfTradersAt(int depth) =>
        CurrentBookValues.AskLayers[depth].NumTraders ??= PreviousAskNumOfTradersAt(AskPriceAt(depth), depth) ?? GenerateNumOfTraders();

    public virtual int? PreviousBidTraderIdAt(decimal price, int traderPos, int? pos = null)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos);
        if (previousLayer == null) return null;
        return TraderLayerTraderId(previousLayer.Value, traderPos);
    }

    public virtual string? PreviousBidTraderNameAt(decimal price, int traderPos, int? pos = null)
    {
        var prevBidTraderId = PreviousBidTraderIdAt(price, traderPos, pos);
        if (!prevBidTraderId.HasValue) return null;
        return GenerateTraderName(prevBidTraderId.Value);
    }

    public virtual int BidTraderIdAt(int depth, int traderPos, int? pos = null)
    {
        var bidTraderAtDepth = CurrentBookValues.BidLayers[depth].Traders;
        for (var i = bidTraderAtDepth.Count; i <= traderPos; i++) bidTraderAtDepth.Add(new SnapshotLayerTraderGeneratedValues());
        var traderDepthPos = bidTraderAtDepth[traderPos];
        traderDepthPos.TraderId     ??= PreviousBidTraderIdAt(BidPriceAt(depth), traderPos, pos) ?? GenerateTraderNumber();
        bidTraderAtDepth[traderPos] =   traderDepthPos;
        return traderDepthPos.TraderId.Value;
    }

    public virtual string BidTraderNameAt(int depth, int traderPos, int? pos = null) => GenerateTraderName(BidTraderIdAt(depth, traderPos, pos));

    public virtual int? PreviousAskTraderIdAt(decimal price, int traderPos, int? pos = null)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos);
        if (previousLayer == null) return null;
        return TraderLayerTraderId(previousLayer.Value, traderPos);
    }

    public virtual string? PreviousAskTraderNameAt(decimal price, int traderPos, int? pos = null)
    {
        var prevAskTraderId = PreviousAskTraderIdAt(price, traderPos, pos);
        if (!prevAskTraderId.HasValue) return null;
        return GenerateTraderName(prevAskTraderId.Value);
    }

    public virtual int AskTraderIdAt(int depth, int traderPos, int? pos = null)
    {
        var askTraderAtDepth = CurrentBookValues.AskLayers[depth].Traders;
        for (var i = askTraderAtDepth.Count; i <= traderPos; i++) askTraderAtDepth.Add(new SnapshotLayerTraderGeneratedValues());
        var traderDepthPos = askTraderAtDepth[traderPos];
        traderDepthPos.TraderId     ??= PreviousAskTraderIdAt(BidPriceAt(depth), traderPos, pos) ?? GenerateTraderNumber();
        askTraderAtDepth[traderPos] =   traderDepthPos;
        return traderDepthPos.TraderId.Value;
    }

    public virtual string AskTraderNameAt(int depth, int traderPos, int? pos = null) => GenerateTraderName(AskTraderIdAt(depth, traderPos, pos));

    public virtual decimal? PreviousBidTraderVolumeAt(decimal price, int traderId, int? pos = null)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.BidLayers, price, pos);
        if (previousLayer == null) return null;
        return TraderLayerTraderVolume(previousLayer!.Value, traderId);
    }

    public virtual decimal BidTraderVolumeAt(int depth, int traderPos)
    {
        var bidTraderAtDepth     = CurrentBookValues.BidLayers[depth].Traders;
        var totalAllocatedVolume = 0m;
        var tradersAtDepth       = BidNumOfTradersAt(depth);
        for (var i = 0; i < tradersAtDepth; i++)
        {
            if (bidTraderAtDepth.Count <= i) bidTraderAtDepth.Add(new SnapshotLayerTraderGeneratedValues());
            totalAllocatedVolume += bidTraderAtDepth[i].TraderVolume ?? 0m;
        }
        var traderDepthPos = bidTraderAtDepth[traderPos];
        var bidPrice       = BidPriceAt(depth);
        var traderId       = traderDepthPos.TraderId ??= PreviousBidTraderIdAt(bidPrice, traderPos, depth) ?? GenerateTraderNumber();
        var bidVolume      = BidVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousBidVolumeAt(bidPrice) ?? 0) > bidVolume &&
                                      (PreviousBidNumOfTradersAt(bidPrice) ?? 0) > BidNumOfTradersAt(depth);
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? bidVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousBidTraderVolumeAt(bidPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume - totalAllocatedVolume);
        traderDepthPos.TraderVolume ??= GenerateTraderVolume(traderPos, tradersAtDepth, bidVolume);
        bidTraderAtDepth[traderPos] =   traderDepthPos;
        return traderDepthPos.TraderVolume!.Value;
    }

    public virtual decimal? PreviousAskTraderVolumeAt(decimal price, int traderId, int? pos = null)
    {
        var previousLayer = PreviousClosestLayerWithPrice(PreviousBookValues.AskLayers, price, pos);
        if (previousLayer == null) return null;
        return TraderLayerTraderVolume(previousLayer!.Value, traderId);
    }

    public virtual decimal AskTraderVolumeAt(int depth, int traderPos)
    {
        var askTraderAtDepth     = CurrentBookValues.AskLayers[depth].Traders;
        var totalAllocatedVolume = 0m;
        var tradersAtDepth       = AskNumOfTradersAt(depth);
        for (var i = 0; i < tradersAtDepth; i++)
        {
            if (askTraderAtDepth.Count <= i) askTraderAtDepth.Add(new SnapshotLayerTraderGeneratedValues());
            totalAllocatedVolume += askTraderAtDepth[i].TraderVolume ?? 0m;
        }
        var traderDepthPos = askTraderAtDepth[traderPos];
        var askPrice       = AskPriceAt(depth);
        var traderId       = traderDepthPos.TraderId ??= PreviousAskTraderIdAt(askPrice, traderPos, depth) ?? GenerateTraderNumber();
        var askVolume      = AskVolumeAt(depth);
        var canUsePreviousTraderVol = (PreviousAskVolumeAt(askPrice) ?? 0) > askVolume &&
                                      (PreviousAskNumOfTradersAt(askPrice) ?? 0) > tradersAtDepth;
        // traderDepthPos.TraderVolume ??= traderPos == tradersAtDepth - 1
        //     ? askVolume - totalAllocatedVolume
        //     : canUsePreviousTraderVol
        //         ? PreviousAskTraderVolumeAt(askPrice, traderId, traderPos) ??
        //           GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume)
        //         : GenerateTraderVolume(traderPos, tradersAtDepth, askVolume - totalAllocatedVolume);
        traderDepthPos.TraderVolume ??= GenerateTraderVolume(traderPos, tradersAtDepth, askVolume);
        askTraderAtDepth[traderPos] =   traderDepthPos;
        return traderDepthPos.TraderVolume!.Value;
    }

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

    protected virtual int? TraderLayerTraderId(SnapshotLayerGeneratedValues previousLayer, int pos)
    {
        var layerTraders = previousLayer.Traders;
        if (layerTraders.Count <= pos) return null;
        var traderLayer = layerTraders[pos];
        if (traderLayer.TraderId == null) return null;
        return GenerateTraderNumber();
    }

    protected virtual decimal? TraderLayerTraderVolume(SnapshotLayerGeneratedValues previousLayer, int traderId)
    {
        var traderLayer = previousLayer.Traders.FirstOrDefault(sltgv => sltgv.TraderId == traderId);
        if (traderLayer.TraderId != traderId) return null;
        return traderLayer.TraderVolume;
    }

    protected virtual int GenerateNumOfTraders() =>
        Math.Min
            (bookGenerationInfo.GenerateBookLayerInfo.MaxNumberOfUniqueTraderName,
             Math.Max(0, bookGenerationInfo.GenerateBookLayerInfo.AverageTradersPerLayer +
                         (int)(BookNormalDist.Sample() *
                               bookGenerationInfo.GenerateBookLayerInfo.TradersPerLayerStandardDeviation)));

    protected virtual SnapshotLayerGeneratedValues? PreviousBidLayerWorseThan(decimal price) =>
        PreviousBookValues.BidLayers.FirstOrDefault(slgv => slgv.Price < price);

    protected virtual SnapshotLayerGeneratedValues? PreviousAskLayerWorseThan(decimal price) =>
        PreviousBookValues.AskLayers.FirstOrDefault(slgv => slgv.Price > price);

    protected virtual SnapshotLayerGeneratedValues? PreviousClosestLayerWithPrice
        (SnapshotLayerGeneratedValues[] layers, decimal price, int? pos = null)
    {
        if (pos != null && layers.Count(slgv => slgv.Price == price) > 1)
        {
            var checkLayer = layers[pos!.Value];
            if (layers.Length > pos && checkLayer.Price == price) return checkLayer;
            var countDown = pos;

            SnapshotLayerGeneratedValues? closestMatch = null;
            for (var i = 0; i < pos && i < layers.Length; i++)
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

    protected virtual string GenerateSourceName(int number) => CachedSourceNames.GetOrAdd(number, num => $"SourceName_{num}");

    protected virtual int GenerateTraderNumber() =>
        BookPseudoRandom.Next(1, bookGenerationInfo.GenerateBookLayerInfo.MaxNumberOfUniqueTraderName + 1);

    protected virtual string GenerateTraderName(int number) => CachedTraderNames.GetOrAdd(number, num => $"TraderName_{num}");

    protected virtual decimal GenerateTraderVolume
        (int pos, int tradersAtDepth, decimal remainingUnallocatedVolume) =>
        // pos == tradersAtDepth - 1 || tradersAtDepth == 0
        //     ? remainingUnallocatedVolume
        //     : remainingUnallocatedVolume / Math.Max(1, tradersAtDepth);
        remainingUnallocatedVolume / Math.Max(1, tradersAtDepth);

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
        (long)Math.Clamp(
                         (
                             bookGenerationInfo.AverageTopOfBookVolume
                           + Math.Max(bookGenerationInfo.HighestVolumeLayer - CurrentBookValues.BidLayers.Length
                                    , bookGenerationInfo.HighestVolumeLayer - Math.Abs(depth - bookGenerationInfo.HighestVolumeLayer)) *
                             bookGenerationInfo.AverageDeltaVolumePerLayer
                           + (decimal)BookNormalDist.Sample() * bookGenerationInfo.MaxVolumeVariance
                         ) / bookGenerationInfo.VolumeRounding * bookGenerationInfo.VolumeRounding
                       , decimal.Zero, bookGenerationInfo.HighestVolumeLayer);

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
            for (var i = 0; i < CurrentBookValues.BidLayers.Length; i++) increasedSize[i].Traders = CurrentBookValues.BidLayers[i].Traders;
            CurrentBookValues.BidLayers = increasedSize;
        }
        if (CurrentBookValues.AskLayers.Length < requireQuoteLayers)
        {
            var increasedSize = new SnapshotLayerGeneratedValues[requireQuoteLayers];
            for (var i = 0; i < CurrentBookValues.AskLayers.Length; i++) increasedSize[i].Traders = CurrentBookValues.AskLayers[i].Traders;
            CurrentBookValues.AskLayers = increasedSize;
        }
        for (var i = 0; i < requireQuoteLayers; i++)
        {
            var bidLayer = CurrentBookValues.BidLayers[i];
            CurrentBookValues.BidLayers[i] = new SnapshotLayerGeneratedValues(bidLayer.Traders);
            for (var j = 0; j < bookGenerationInfo.GenerateBookLayerInfo.AverageTradersPerLayer; j++)
                bidLayer.Traders.Add(new SnapshotLayerTraderGeneratedValues());

            var askLayer = CurrentBookValues.AskLayers[i];
            CurrentBookValues.AskLayers[i] = new SnapshotLayerGeneratedValues(askLayer.Traders);
            for (var j = 0; j < bookGenerationInfo.GenerateBookLayerInfo.AverageTradersPerLayer; j++)
                askLayer.Traders.Add(new SnapshotLayerTraderGeneratedValues());
        }
    }
}
