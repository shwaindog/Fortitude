using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators;

public interface IPublishedIndicators: IReusableObject<IPublishedIndicators>, IInterfacesComparable<IPublishedIndicators>
{
}

public interface IMutablePublishedIndicators : IPublishedIndicators
{
}