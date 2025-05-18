using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Strategies;

public interface IPublishedStrategyDecisions : IReusableObject<IPublishedStrategyDecisions>, IInterfacesComparable<IPublishedStrategyDecisions>
{
}
public interface IMutablePublishedStrategyDecisions : IPublishedStrategyDecisions, ITrackableReset<IMutablePublishedStrategyDecisions>
{
}