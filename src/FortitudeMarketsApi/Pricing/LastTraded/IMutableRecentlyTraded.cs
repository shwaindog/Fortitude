#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableRecentlyTraded : IRecentlyTraded, IStoreState<IRecentlyTraded>
{
    new int Capacity { get; set; }
    new IMutableLastTrade? this[int i] { get; set; }
    void Reset();
    void Add(IMutableLastTrade newLastTrade);
    new IMutableRecentlyTraded Clone();
    new IEnumerator<IMutableLastTrade> GetEnumerator();
}
