using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

public interface IMarginDetails : IReusableObject<IMarginDetails>, IInterfacesComparable<IMarginDetails>
{
}

public interface IMutableMarginDetails : IMarginDetails, ITrackableReset<IMutableMarginDetails>, IEmptyable
{
}