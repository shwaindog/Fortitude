// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskPair : IInterfacesComparable<IBidAskPair>
{
    decimal  BidPrice       { get; }
    decimal  AskPrice       { get; }
    DateTime AtTime         { get; }
    ushort   SourceId       { get; set; }
    uint     SequenceNumber { get; set; }
}

public struct BidAskPair : IBidAskPair
{
    public BidAskPair() { }

    public BidAskPair(BidAskPair toClone)
    {
        BidPrice       = toClone.BidPrice;
        AskPrice       = toClone.AskPrice;
        AtTime         = toClone.AtTime;
        SourceId       = toClone.SourceId;
        SequenceNumber = toClone.SequenceNumber;
    }

    public BidAskPair(ILevel1Quote toCapture)
    {
        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;
        SourceId = toCapture.SourceTickerQuoteInfo?.SourceId ?? 0;
    }

    public BidAskPair(decimal bidPrice, decimal askPrice, DateTime? atTime = null, ushort sourceId = 0, uint sequenceNumber = 0)
    {
        BidPrice       = bidPrice;
        AskPrice       = askPrice;
        AtTime         = atTime ?? DateTime.UtcNow;
        SourceId       = sourceId;
        SequenceNumber = sequenceNumber;
    }

    public decimal  BidPrice       { get; set; }
    public decimal  AskPrice       { get; set; }
    public DateTime AtTime         { get; set; }
    public ushort   SourceId       { get; set; }
    public uint     SequenceNumber { get; set; }

    public bool AreEquivalent(IBidAskPair? other, bool exactTypes = false)
    {
        var bidPriceSame       = BidPrice == other.BidPrice;
        var askPriceSame       = AskPrice == other.AskPrice;
        var atTimeSame         = AtTime == other.AtTime;
        var sourceIdSame       = SourceId == other.SourceId;
        var sequenceNumberSame = true;

        if (exactTypes) sequenceNumberSame = SequenceNumber == other.SequenceNumber;

        var allAreSame = bidPriceSame && askPriceSame && atTimeSame && sourceIdSame && sequenceNumberSame;
        return allAreSame;
    }
}

public interface IBidAskPairNode : IBidAskPair, IDoublyLinkedListNode<IBidAskPairNode> { }

public class BidAsk : ReusableObject<IBidAskPair>, IBidAskPairNode
{
    private BidAskPair bidAskPairState;

    public BidAsk() { }
    public BidAsk(BidAskPair bidAskPair) => bidAskPairState = bidAskPair;

    public BidAsk(BidAsk bidAsk) => bidAskPairState = bidAsk.bidAskPairState;

    public BidAsk(ILevel1Quote toCapture) => bidAskPairState = new BidAskPair(toCapture);

    public decimal BidPrice
    {
        get => bidAskPairState.BidPrice;
        set => bidAskPairState.BidPrice = value;
    }

    public decimal AskPrice
    {
        get => bidAskPairState.AskPrice;
        set => bidAskPairState.AskPrice = value;
    }

    public DateTime AtTime
    {
        get => bidAskPairState.AtTime;
        set => bidAskPairState.AtTime = value;
    }

    public ushort SourceId
    {
        get => bidAskPairState.SourceId;
        set => bidAskPairState.SourceId = value;
    }

    public uint SequenceNumber
    {
        get => bidAskPairState.SequenceNumber;
        set => bidAskPairState.SequenceNumber = value;
    }

    public IBidAskPairNode? Previous { get; set; }
    public IBidAskPairNode? Next     { get; set; }

    public bool AreEquivalent(IBidAskPair? other, bool exactTypes = false)
    {
        var bidPriceSame       = BidPrice == other.BidPrice;
        var askPriceSame       = AskPrice == other.AskPrice;
        var atTimeSame         = AtTime == other.AtTime;
        var sourceIdSame       = SourceId == other.SourceId;
        var sequenceNumberSame = true;

        if (exactTypes) sequenceNumberSame = SequenceNumber == other.SequenceNumber;

        var allAreSame = bidPriceSame && askPriceSame && atTimeSame && sourceIdSame && sequenceNumberSame;
        return allAreSame;
    }

    public override IBidAskPair CopyFrom(IBidAskPair source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPrice;
        AskPrice = source.AskPrice;
        AtTime   = source.AtTime;
        SourceId = source.SourceId;

        SequenceNumber = source.SequenceNumber;
        return this;
    }

    public IBidAskPair CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPriceTop;
        AskPrice = source.AskPriceTop;
        AtTime   = source.SourceTime;
        SourceId = source.SourceTickerQuoteInfo?.SourceId ?? 0;

        return this;
    }


    public override IBidAskPair Clone() => Recycler?.Borrow<BidAsk>().CopyFrom(this) ?? new BidAsk(this);

    protected bool Equals(BidAsk other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BidAsk)obj);
    }

    public override int GetHashCode() => bidAskPairState.GetHashCode();

    public override string ToString() =>
        $"{nameof(BidAsk)}{nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice}, " +
        $"{nameof(AtTime)}: {AtTime}, {nameof(SourceId)}: {SourceId}, " +
        $"{nameof(SequenceNumber)}: {SequenceNumber}";
}
