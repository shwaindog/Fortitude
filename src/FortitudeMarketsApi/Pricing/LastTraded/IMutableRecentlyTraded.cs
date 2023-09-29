using System.Collections;
using System.Collections.Generic;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LastTraded
{
    public interface IMutableRecentlyTraded : IRecentlyTraded, IStoreState<IRecentlyTraded>
    {
        new int Capacity { get; set; }
        void Reset();
        void Add(IMutableLastTrade newLastTrade);
        new IMutableLastTrade this[int i] { get; set; }
        new IMutableRecentlyTraded Clone();
        new IEnumerator<IMutableLastTrade> GetEnumerator();
    }
}