// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public interface IOrderBook : IReusableObject<IOrderBook>, IInterfacesComparable<IOrderBook>
{
    LayerType  LayersSupportedType { get; }
    LayerFlags LayerSupportedFlags   { get; }

    IOrderBookSide AskSide { get; }
    IOrderBookSide BidSide { get; }

    ushort MaxAllowedSize { get; }

    bool IsBidBookChanged { get; }
    
    bool IsAskBookChanged { get; }

    decimal? MidPrice { get; }

    bool          HasNonEmptyOpenInterest { get; }

    IMarketAggregate OpenInterest                          { get; }

    uint DailyTickUpdateCount { get; }
    bool IsLadder             { get; }
}

public interface IMutableOrderBook : IOrderBook, IInterfacesComparable<IMutableOrderBook>, ICloneable<IMutableOrderBook>, ITrackableReset<IMutableOrderBook>
{
    new LayerType  LayersSupportedType { get; set; }
    new LayerFlags LayerSupportedFlags { get; set; }

    new IMutableOrderBookSide AskSide { get; set; }
    new IMutableOrderBookSide BidSide { get; set; }

    new bool IsBidBookChanged { get; set; }
    
    new bool IsAskBookChanged        { get; set; }
    new bool HasNonEmptyOpenInterest { get; set; }

    new IMutableMarketAggregate? OpenInterest  { get; set; }

    new uint DailyTickUpdateCount { get; set; }
    new bool IsLadder             { get; set; }

    new IMutableOrderBook Clone();
}
