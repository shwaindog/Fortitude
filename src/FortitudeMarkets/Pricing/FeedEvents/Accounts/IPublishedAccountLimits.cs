using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Accounts;

public interface IPublishedAccountLimits : IReusableObject<IPublishedAccountLimits>, IInterfacesComparable<IPublishedAccountLimits>
{
}

public interface IMutablePublishedAccountLimits : IPublishedAccountLimits
{
}