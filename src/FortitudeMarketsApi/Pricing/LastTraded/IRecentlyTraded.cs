﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IRecentlyTraded : IReusableObject<IRecentlyTraded>, IEnumerable<ILastTrade>,
    IInterfacesComparable<IRecentlyTraded>
{
    bool HasLastTrades { get; }
    int Count { get; }
    int Capacity { get; }
    ILastTrade? this[int i] { get; }
}
