// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public interface ISameIndicatorValidRangeBidAskPeriodChain : IDoublyLinkedList<IValidRangeBidAskPeriod>, IRecyclableObject
{
    long       IndicatorSourceTickerId { get; }
    TimePeriod CoveringPeriod          { get; }

    IBidAskInstant RefIncAddFirst(IValidRangeBidAskPeriod node);
    IBidAskInstant RefIncAddLast(IValidRangeBidAskPeriod node);
    IBidAskInstant RefDecRemove(IValidRangeBidAskPeriod node);
}

public class SameIndicatorValidRangeBidAskPeriodChain : DoublyLinkedList<IValidRangeBidAskPeriod>, ISameIndicatorValidRangeBidAskPeriodChain
{
    public long IndicatorSourceTickerId { get; set; }

    public TimePeriod CoveringPeriod { get; set; }

    public IBidAskInstant RefIncAddFirst(IValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddFirst(node);
    }

    public IBidAskInstant RefIncAddLast(IValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddLast(node);
    }

    public IBidAskInstant RefDecRemove(IValidRangeBidAskPeriod node)
    {
        Remove(node);
        node.DecrementRefCount();
        return node;
    }

    public void Configure(long indicatorSourceTickerId, TimePeriod coveringPeriod)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }
}
