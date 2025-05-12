using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Limits;

public interface IPublishedLimits : IReusableObject<ILimitBreaches>, IInterfacesComparable<IPublishedLimits>
{
}
public interface IMutablePublishedLimits : IPublishedLimits
{
}