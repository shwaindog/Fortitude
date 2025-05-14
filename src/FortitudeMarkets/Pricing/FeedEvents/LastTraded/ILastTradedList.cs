// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface ILastTradedList : IReusableObject<ILastTradedList>, IEnumerable<ILastTrade>,
    IInterfacesComparable<ILastTradedList>
{
    LastTradeType   LastTradesOfType       { get; }
    LastTradedFlags LastTradesSupportFlags { get; }

    bool HasLastTrades { get; }
    int  Count         { get; }
    int  Capacity      { get; }
    ILastTrade? this[int i] { get; }
}

public interface IMutableLastTradedList : ILastTradedList, IEnumerable<IMutableLastTrade>
{
    new int Capacity { get; set; }
    new IMutableLastTrade? this[int i] { get; set; }
    void Add(IMutableLastTrade newLastTrade);

    new IMutableLastTradedList         Clone();

    new IEnumerator<IMutableLastTrade> GetEnumerator();

    int AppendEntryAtEnd();
}
