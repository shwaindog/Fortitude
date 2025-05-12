using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

public interface IMarginDetails : IReusableObject<IMarginDetails>, IInterfacesComparable<IMarginDetails>
{
}

public interface IMutableMarginDetails : IMarginDetails
{
}