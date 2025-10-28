using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.VolatilityIndicators;

public interface IPublishedVolatilities  : IReusableObject<IPublishedVolatilities>, IInterfacesComparable<IPublishedVolatilities>
{
}

public interface IMutablePublishedVolatilities  : IPublishedVolatilities, ITrackableReset<IMutablePublishedVolatilities>
{
}