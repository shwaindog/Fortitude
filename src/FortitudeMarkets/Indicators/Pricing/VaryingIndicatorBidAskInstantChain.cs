// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public interface IVaryingIndicatorBidAskInstantChain : IDoublyLinkedList<IIndicatorValidRangeBidAskPeriod>, IRecyclableObject
{
    IIndicatorValidRangeBidAskPeriod RefIncAddFirst(IIndicatorValidRangeBidAskPeriod node);
    IIndicatorValidRangeBidAskPeriod RefIncAddLast(IIndicatorValidRangeBidAskPeriod node);
    IIndicatorValidRangeBidAskPeriod RefDecRemove(IIndicatorValidRangeBidAskPeriod node);
}

public class VaryingIndicatorBidAskInstantChain : DoublyLinkedList<IIndicatorValidRangeBidAskPeriod>, IVaryingIndicatorBidAskInstantChain
{
    public IIndicatorValidRangeBidAskPeriod RefIncAddFirst(IIndicatorValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddFirst(node);
    }

    public IIndicatorValidRangeBidAskPeriod RefIncAddLast(IIndicatorValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddLast(node);
    }

    public IIndicatorValidRangeBidAskPeriod RefDecRemove(IIndicatorValidRangeBidAskPeriod node)
    {
        Remove(node);
        node.DecrementRefCount();
        return node;
    }
}
