using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.BoundaryLines;

public interface IPublishedBoundaryLines : IReusableObject<IPublishedBoundaryLines>, IInterfacesComparable<IPublishedBoundaryLines>
{
}
public interface IMutablePublishedBoundaryLines : IPublishedBoundaryLines, ITrackableReset<IMutablePublishedBoundaryLines> 
{
}