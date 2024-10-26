// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public interface ISameIndicatorBidAskInstantChain : IDoublyLinkedList<IBidAskInstant>, IRecyclableObject
{
    long               IndicatorSourceTickerId { get; }
    DiscreetTimePeriod CoveringPeriod          { get; }

    IBidAskInstant RefIncAddFirst(IBidAskInstant node);
    IBidAskInstant RefIncAddLast(IBidAskInstant node);
    IBidAskInstant RefDecRemove(IBidAskInstant node);
}

public class SameIndicatorBidAskInstantChain : DoublyLinkedList<IBidAskInstant>, ISameIndicatorBidAskInstantChain
{
    public long IndicatorSourceTickerId { get; set; }

    public DiscreetTimePeriod CoveringPeriod { get; set; }

    public IBidAskInstant RefIncAddFirst(IBidAskInstant node)
    {
        node.IncrementRefCount();
        return AddFirst(node);
    }

    public IBidAskInstant RefIncAddLast(IBidAskInstant node)
    {
        node.IncrementRefCount();
        return AddLast(node);
    }

    public IBidAskInstant RefDecRemove(IBidAskInstant node)
    {
        Remove(node);
        node.DecrementRefCount();
        return node;
    }

    public void Configure(long indicatorSourceTickerId, DiscreetTimePeriod coveringPeriod)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }
}
