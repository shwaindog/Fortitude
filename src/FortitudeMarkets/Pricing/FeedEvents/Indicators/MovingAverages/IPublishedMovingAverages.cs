using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.MovingAverages;

public interface IPublishedMovingAverages : IReusableObject<IPublishedMovingAverages>, IInterfacesComparable<IPublishedMovingAverages>
{
}
public interface IMutablePublishedMovingAverages : IPublishedMovingAverages, ITrackableReset<IMutablePublishedMovingAverages>
{
}