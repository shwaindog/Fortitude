// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface IOrderBook : IReusableObject<IOrderBook>, IInterfacesComparable<IOrderBook>
{
    LayerType  LayersOfType { get; }
    LayerFlags LayerFlags   { get; }

    IOrderBookSide AskSide { get; }
    IOrderBookSide BidSide { get; }

    ushort MaxPublishDepth { get; }

    bool IsBidBookChanged { get; }
    
    bool IsAskBookChanged { get; }

    decimal? MidPrice { get; }

    OpenInterest? SourceOpenInterest    { get; }
    OpenInterest  AdapterOpenInterest   { get; }
    OpenInterest  PublishedOpenInterest { get; }

    uint DailyTickUpdateCount { get; }
    bool IsLadder             { get; }
}

public interface IMutableOrderBook : IOrderBook, IInterfacesComparable<IMutableOrderBook>, ICloneable<IMutableOrderBook>
{
    new LayerType LayersOfType { get; set; }

    new IMutableOrderBookSide AskSide { get; set; }
    new IMutableOrderBookSide BidSide { get; set; }

    new bool IsBidBookChanged { get; set; }
    
    new bool IsAskBookChanged { get; set; }

    new OpenInterest? SourceOpenInterest  { get; set; }
    new OpenInterest  AdapterOpenInterest { get; set; }

    new uint DailyTickUpdateCount { get; set; }
    new bool IsLadder             { get; set; }

    new IMutableOrderBook Clone();
}
