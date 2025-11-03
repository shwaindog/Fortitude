using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

public interface ITickerRegionInfo : IReusableObject<ITickerRegionInfo>, IInterfacesComparable<ITickerRegionInfo>
{
}
public interface IMutableTickerRegionInfo : ITickerRegionInfo, ITrackableReset<IMutableTickerRegionInfo>, IEmptyable
{
}