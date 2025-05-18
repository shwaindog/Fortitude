using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

public interface IPnLConversions : IReusableObject<IPnLConversions>, IInterfacesComparable<IPnLConversions>
{
}

public interface IMutablePnLConversions : IPnLConversions, ITrackableReset<IMutablePnLConversions>, IEmptyable
{
}