// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface ILastTradedList : IReusableObject<ILastTradedList>, IInterfacesComparable<ILastTradedList>, ICapacityList<ILastTrade>
{
    LastTradeType LastTradesOfType { get; }

    LastTradedFlags LastTradesSupportFlags { get; }

    bool HasLastTrades { get; }

    new int Capacity { get; }
}

public interface IMutableLastTradedList : ILastTradedList, ITrackableReset<IMutableLastTradedList>, IMutableCapacityList<ILastTrade>
  , IMutableCapacityList<IMutableLastTrade>
{
    new int Capacity { get; set; }

    new IMutableLastTrade this[int i] { get; set; }

    new int Count { get; set; }

    new void Clear();

    new bool IsReadOnly { get; }

    new void RemoveAt(int index);

    new IMutableLastTradedList Clone();

    new IEnumerator<IMutableLastTrade> GetEnumerator();

    int AppendEntryAtEnd();
}
