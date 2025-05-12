using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.VolatilityIndicators;

internal interface IVolatility  : IReusableObject<IVolatility>, IInterfacesComparable<IVolatility>
{
}