using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IPublishedMarketEvents : IReusableObject<IPublishedMarketEvents>, IInterfacesComparable<IPublishedMarketEvents>
{
}

public interface IMutablePublishedMarketEvents : IPublishedMarketEvents, ITrackableReset<IMutablePublishedMarketEvents>, IEmptyable
{
}