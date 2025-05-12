using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Signals;

public interface IPublishedSignals : IReusableObject<IPublishedSignals>, IInterfacesComparable<IPublishedSignals>
{
}
public interface IMutablePublishedSignals : IPublishedSignals
{
}