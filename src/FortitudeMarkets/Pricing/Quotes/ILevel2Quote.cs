// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public interface ILevel2Quote : ILevel1Quote, ICloneable<ILevel2Quote>, IDoublyLinkedListNode<ILevel2Quote>
{
    IOrderBook OrderBook { get; }

    IOrderBookSide BidBook { get; }

    IOrderBookSide AskBook { get; }


    new ILevel2Quote? Next     { get; set; }
    new ILevel2Quote? Previous { get; set; }

    new ILevel2Quote Clone();
}

public interface IMutableLevel2Quote : ILevel2Quote, IMutableLevel1Quote
{
    new IMutableOrderBook OrderBook { get; set; }

    new IMutableLevel2Quote Clone();
}
