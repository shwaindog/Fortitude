using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IMarketNewsPanel : IReusableObject<IMarketNewsPanel>, IInterfacesComparable<IMarketNewsPanel>
{
}

public interface IMutableMarketNewsPanel : IMarketNewsPanel, ITrackableReset<IMutableMarketNewsPanel>, IEmptyable
{

}