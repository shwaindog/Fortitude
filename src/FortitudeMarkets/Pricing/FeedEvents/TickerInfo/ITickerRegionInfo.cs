using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

public interface ITickerRegionInfo : IReusableObject<ITickerRegionInfo>, IInterfacesComparable<ITickerRegionInfo>
{
}
public interface IMutableTickerRegionInfo : ITickerRegionInfo
{
}