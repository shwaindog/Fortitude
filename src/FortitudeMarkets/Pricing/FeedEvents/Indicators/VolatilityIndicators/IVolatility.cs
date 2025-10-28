using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.VolatilityIndicators;

public interface IVolatility  : IReusableObject<IVolatility>, IInterfacesComparable<IVolatility>
{
}
public interface IMutableVolatility  : IVolatility, ITrackableReset<IMutableVolatility>
{
}