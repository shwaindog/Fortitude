using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.Pivots;

public interface IPublishedPivots : IReusableObject<IPublishedPivots>, IInterfacesComparable<IPublishedPivots>
{
}
public interface IMutablePublishedPivots : IReusableObject<IPublishedPivots>, ITrackableReset<IMutablePublishedPivots>
{
}