using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Signals;

public interface IPublishedSignals : IReusableObject<IPublishedSignals>, IInterfacesComparable<IPublishedSignals>
{
}
public interface IMutablePublishedSignals : IPublishedSignals, ITrackableReset<IMutablePublishedSignals>
{
}