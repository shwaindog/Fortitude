// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskPair
{
    decimal BidPrice { get; set; }
    decimal AskPrice { get; set; }
}

public struct BidAskPair : IBidAskPair
{
    public BidAskPair(decimal bidPrice, decimal askPrice)
    {
        AskPrice = askPrice;
        BidPrice = bidPrice;
    }

    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }

    public override string ToString() => $"{nameof(BidAskPair)}({nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice})";
}

public static class BidAskPairExtensions
{
    public static BidAskPair SetBidPrice(this BidAskPair pair, decimal bidPrice) => new(bidPrice, pair.AskPrice);
    public static BidAskPair SetAskPrice(this BidAskPair pair, decimal askPrice) => new(pair.BidPrice, askPrice);
}

public interface IBidAsk : IBidAskPair, IStoreState<IBidAsk> { }

public class BidAsk : IBidAsk
{
    public BidAsk(BidAskPair bidAskPair)
    {
        AskPrice = bidAskPair.AskPrice;
        BidPrice = bidAskPair.BidPrice;
    }

    public BidAsk(decimal bidPrice, decimal askPrice)
    {
        AskPrice = askPrice;
        BidPrice = bidPrice;
    }

    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IBidAsk)source, copyMergeFlags);

    public IBidAsk CopyFrom(IBidAsk source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPrice;
        AskPrice = source.AskPrice;
        return this;
    }

    public override string ToString() => $"{nameof(BidAsk)}({nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice})";
}
