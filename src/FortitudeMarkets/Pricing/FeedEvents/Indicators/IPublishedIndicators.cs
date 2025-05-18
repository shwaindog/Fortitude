using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators;

public interface IPublishedIndicators: IReusableObject<IPublishedIndicators>, IInterfacesComparable<IPublishedIndicators>
{
}

public interface IMutablePublishedIndicators : IPublishedIndicators, ITrackableReset<IMutablePublishedIndicators> 
{
}