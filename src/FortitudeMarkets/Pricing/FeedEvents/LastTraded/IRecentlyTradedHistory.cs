using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IRecentlyTradedHistory : IReusableObject<IRecentlyTradedHistory>, IInterfacesComparable<IRecentlyTradedHistory>
{
}
public interface IMutableRecentlyTradedHistory : IRecentlyTradedHistory
{
}