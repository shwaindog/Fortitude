// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskInstantPair : IBidAskPair, IInterfacesComparable<IBidAskInstantPair>
{
    DateTime AtTime         { get; }
    ushort   SourceId       { get; set; }
    uint     SequenceNumber { get; set; }
}

public struct BidAskInstantPair : IBidAskInstantPair
{
    public BidAskInstantPair() { }

    public BidAskInstantPair(BidAskInstantPair toClone)
    {
        BidPrice       = toClone.BidPrice;
        AskPrice       = toClone.AskPrice;
        AtTime         = toClone.AtTime;
        SourceId       = toClone.SourceId;
        SequenceNumber = toClone.SequenceNumber;
    }

    public BidAskInstantPair(ILevel1Quote toCapture)
    {
        BidPrice = toCapture.BidPriceTop;
        AskPrice = toCapture.AskPriceTop;
        AtTime   = toCapture.SourceTime;
        SourceId = toCapture.SourceTickerQuoteInfo?.SourceId ?? 0;
    }

    public BidAskInstantPair(decimal bidPrice, decimal askPrice, DateTime? atTime = null, ushort sourceId = 0, uint sequenceNumber = 0)
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

    public bool AreEquivalent(IBidAskInstantPair? other, bool exactTypes = false)
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

public interface IBidAskInstant : IBidAskInstantPair, IDoublyLinkedListNode<IBidAskInstant> { }

public class BidAskInstant : ReusableObject<IBidAskInstantPair>, IBidAskInstant
{
    private BidAskInstantPair bidAskInstantPairState;

    public BidAskInstant() { }
    public BidAskInstant(BidAskInstantPair bidAskInstantPair) => bidAskInstantPairState = bidAskInstantPair;

    public BidAskInstant(BidAskInstant bidAskInstant) => bidAskInstantPairState = bidAskInstant.bidAskInstantPairState;

    public BidAskInstant(ILevel1Quote toCapture) => bidAskInstantPairState = new BidAskInstantPair(toCapture);

    public decimal BidPrice
    {
        get => bidAskInstantPairState.BidPrice;
        set => bidAskInstantPairState.BidPrice = value;
    }

    public decimal AskPrice
    {
        get => bidAskInstantPairState.AskPrice;
        set => bidAskInstantPairState.AskPrice = value;
    }

    public DateTime AtTime
    {
        get => bidAskInstantPairState.AtTime;
        set => bidAskInstantPairState.AtTime = value;
    }

    public ushort SourceId
    {
        get => bidAskInstantPairState.SourceId;
        set => bidAskInstantPairState.SourceId = value;
    }

    public uint SequenceNumber
    {
        get => bidAskInstantPairState.SequenceNumber;
        set => bidAskInstantPairState.SequenceNumber = value;
    }

    public IBidAskInstant? Previous { get; set; }
    public IBidAskInstant? Next     { get; set; }

    public bool AreEquivalent(IBidAskInstantPair? other, bool exactTypes = false)
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

    public override IBidAskInstantPair CopyFrom(IBidAskInstantPair source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPrice;
        AskPrice = source.AskPrice;
        AtTime   = source.AtTime;
        SourceId = source.SourceId;

        SequenceNumber = source.SequenceNumber;
        return this;
    }

    public IBidAskInstantPair CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidPrice = source.BidPriceTop;
        AskPrice = source.AskPriceTop;
        AtTime   = source.SourceTime;
        SourceId = source.SourceTickerQuoteInfo?.SourceId ?? 0;

        return this;
    }


    public override IBidAskInstantPair Clone() => Recycler?.Borrow<BidAskInstant>().CopyFrom(this) ?? new BidAskInstant(this);

    protected bool Equals(BidAskInstant other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BidAskInstant)obj);
    }

    public override int GetHashCode() => bidAskInstantPairState.GetHashCode();

    public override string ToString() =>
        $"{nameof(BidAskInstant)}{nameof(BidPrice)}: {BidPrice}, {nameof(AskPrice)}: {AskPrice}, " +
        $"{nameof(AtTime)}: {AtTime}, {nameof(SourceId)}: {SourceId}, " +
        $"{nameof(SequenceNumber)}: {SequenceNumber}";
}
