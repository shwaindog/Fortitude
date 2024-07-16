// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskPair
{
    decimal BidPrice { get; }
    decimal AskPrice { get; }
}

public readonly struct BidAskPair // not inheriting from IBidAskPair to prevent accidental boxing unboxing
{
    public BidAskPair(decimal bidPrice, decimal askPrice)
    {
        AskPrice = askPrice;
        BidPrice = bidPrice;
    }

    public BidAskPair(IBidAskPair toClone)
    {
        AskPrice = toClone.AskPrice;
        BidPrice = toClone.BidPrice;
    }

    public decimal BidPrice { get; }
    public decimal AskPrice { get; }

    public override string ToString() => $"{nameof(BidAskPair)}({nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice})";
}

public static class BidAskPairExtensions
{
    public static BidAskPair SetBidPrice(this BidAskPair pair, decimal bidPrice) => new(bidPrice, pair.AskPrice);
    public static BidAskPair SetAskPrice(this BidAskPair pair, decimal askPrice) => new(pair.BidPrice, askPrice);
}

public interface IBidAsk : IBidAskPair, IReusableObject<IBidAskPair>
{
    new decimal BidPrice { get; set; }
    new decimal AskPrice { get; set; }
}

public class BidAsk : ReusableObject<IBidAsk>, IBidAsk
{
    public BidAsk() { }

    public BidAsk(IBidAsk bidAsk)
    {
        AskPrice = bidAsk.AskPrice;
        BidPrice = bidAsk.BidPrice;
    }

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

    IBidAskPair IStoreState<IBidAskPair>.CopyFrom(IBidAskPair source, CopyMergeFlags copyMergeFlags) => CopyFrom((IBidAsk)source, copyMergeFlags);

    IReusableObject<IBidAskPair> IStoreState<IReusableObject<IBidAskPair>>.CopyFrom
        (IReusableObject<IBidAskPair> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IBidAsk)source, copyMergeFlags);

    IBidAskPair ICloneable<IBidAskPair>.Clone() => Clone();

    public override void StateReset()
    {
        BidPrice = 0m;
        AskPrice = 0m;
        base.StateReset();
    }

    public void Configure(decimal bidPrice, decimal askPrice)
    {
        AskPrice = askPrice;
        BidPrice = bidPrice;
    }

    public void Configure(BidAskPair bidAskPair)
    {
        AskPrice = bidAskPair.AskPrice;
        BidPrice = bidAskPair.BidPrice;
    }


    public override IBidAsk CopyFrom(IBidAsk source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPrice;
        AskPrice = source.AskPrice;
        return this;
    }

    public override IBidAsk Clone() => Recycler?.Borrow<BidAsk>() ?? new BidAsk((IBidAsk)this);

    public override string ToString() => $"{nameof(BidAsk)}({nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice})";


    public static implicit operator BidAskPair(BidAsk pricePoint) => new(pricePoint);
}
