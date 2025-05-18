using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IRecentlyTradedHistory : IReusableObject<IRecentlyTradedHistory>, IInterfacesComparable<IRecentlyTradedHistory>
{
}

public interface IMutableRecentlyTradedHistory : IRecentlyTradedHistory, ITrackableReset<IMutableRecentlyTradedHistory>, IEmptyable
{
}