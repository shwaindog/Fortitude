using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Limits;

public interface IPublishedLimits : IReusableObject<ILimitBreaches>, IInterfacesComparable<IPublishedLimits>
{
}
public interface IMutablePublishedLimits : IPublishedLimits, ITrackableReset<IMutablePublishedLimits>, IEmptyable
{
}