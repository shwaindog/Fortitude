// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public interface ILevel2Quote : ILevel1Quote, ICloneable<ILevel2Quote>
{
    IOrderBook OrderBook { get; }

    IOrderBookSide BidBook { get; }

    IOrderBookSide AskBook { get; }

    new bool       AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new ILevel2Quote Clone();
}

public interface IMutableLevel2Quote : ILevel2Quote, IMutableLevel1Quote
{
    new IMutableOrderBook OrderBook { get; set; }
    
    new bool                AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new IMutableLevel2Quote Clone();
}

public interface IPublishableLevel2Quote : IPublishableLevel1Quote, ILevel2Quote, ICloneable<IPublishableLevel2Quote>, IDoublyLinkedListNode<IPublishableLevel2Quote>
{
    new IPublishableLevel2Quote? Next     { get; set; }
    new IPublishableLevel2Quote? Previous { get; set; }

    new ILevel2Quote AsNonPublishable { get; }

    new bool         AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IPublishableLevel2Quote Clone();
}

public interface IMutablePublishableLevel2Quote : IPublishableLevel2Quote, IMutableLevel2Quote, IMutablePublishableLevel1Quote
{
    new IMutableOrderBook OrderBook { get; set; }

    new IMutableLevel2Quote AsNonPublishable { get; }
    
    new bool         AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IMutablePublishableLevel2Quote Clone();
}
