// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableRecentlyTraded : IRecentlyTraded
{
    new int Capacity { get; set; }
    new IMutableLastTrade? this[int i] { get; set; }
    void Add(IMutableLastTrade newLastTrade);

    new IMutableRecentlyTraded         Clone();
    new IEnumerator<IMutableLastTrade> GetEnumerator();

    int AppendEntryAtEnd();
}
