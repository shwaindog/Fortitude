using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IPublishedMarketEvents : IReusableObject<IPublishedMarketEvents>, IInterfacesComparable<IPublishedMarketEvents>
{
}

public interface IMutablePublishedMarketEvents : IPublishedMarketEvents
{
}