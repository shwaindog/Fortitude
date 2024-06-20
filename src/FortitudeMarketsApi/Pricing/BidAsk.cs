// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskPair
{
    decimal  BidTop         { get; }
    decimal  AskTop         { get; }
    DateTime AtTime         { get; }
    ushort   SourceId       { get; }
    uint     SequenceNumber { get; }
}

public struct BidAskPair : IBidAskPair
{
    public decimal  BidTop         { get; set; }
    public decimal  AskTop         { get; set; }
    public DateTime AtTime         { get; set; }
    public ushort   SourceId       { get; set; }
    public uint     SequenceNumber { get; set; }
}

public interface IBidAskPairNode : IBidAskPair, IDoublyLinkedListNode<IBidAskPairNode> { }

public class BidAsk : ReusableObject<IBidAskPair>, IBidAskPairNode
{
    private BidAskPair bidAskPairState;

    public BidAsk() { }
    public BidAsk(BidAskPair bidAskPair) => bidAskPairState = bidAskPair;

    public BidAsk(BidAsk bidAsk) => bidAskPairState = bidAsk.bidAskPairState;

    public decimal BidTop
    {
        get => bidAskPairState.BidTop;
        set => bidAskPairState.BidTop = value;
    }

    public decimal AskTop
    {
        get => bidAskPairState.AskTop;
        set => bidAskPairState.AskTop = value;
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

    public override IBidAskPair CopyFrom(IBidAskPair source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidTop         = source.BidTop;
        AskTop         = source.AskTop;
        AtTime         = source.AtTime;
        SourceId       = source.SourceId;
        SequenceNumber = source.SequenceNumber;
        return this;
    }


    public override IBidAskPair Clone() => Recycler?.Borrow<BidAsk>().CopyFrom(this) ?? new BidAsk(this);

    protected bool Equals(BidAsk other) => bidAskPairState.Equals(other.bidAskPairState);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BidAsk)obj);
    }

    public override int GetHashCode() => bidAskPairState.GetHashCode();

    public override string ToString() =>
        $"{nameof(BidAsk)}{nameof(BidTop)}: {BidTop}, {nameof(AskTop)}: {AskTop}, " +
        $"{nameof(AtTime)}: {AtTime}, {nameof(SourceId)}: {SourceId}, " +
        $"{nameof(SequenceNumber)}: {SequenceNumber}";
}
