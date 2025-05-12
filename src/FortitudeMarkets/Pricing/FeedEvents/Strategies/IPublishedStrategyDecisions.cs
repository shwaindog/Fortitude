using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Strategies;

public interface IPublishedStrategyDecisions : IReusableObject<IPublishedStrategyDecisions>, IInterfacesComparable<IPublishedStrategyDecisions>
{
}
public interface IMutablePublishedStrategyDecisions : IPublishedStrategyDecisions
{
}