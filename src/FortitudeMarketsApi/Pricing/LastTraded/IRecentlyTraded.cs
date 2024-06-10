// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IRecentlyTraded : IReusableObject<IRecentlyTraded>, IEnumerable<ILastTrade>,
    IInterfacesComparable<IRecentlyTraded>
{
    LastTradeType   LastTradesOfType       { get; }
    LastTradedFlags LastTradesSupportFlags { get; }

    bool HasLastTrades { get; }
    int  Count         { get; }
    int  Capacity      { get; }
    ILastTrade? this[int i] { get; }
}

public interface IMutableRecentlyTraded : IRecentlyTraded
{
    new int Capacity { get; set; }
    new IMutableLastTrade? this[int i] { get; set; }
    void Add(IMutableLastTrade newLastTrade);

    new IMutableRecentlyTraded         Clone();
    new IEnumerator<IMutableLastTrade> GetEnumerator();

    int AppendEntryAtEnd();
}
