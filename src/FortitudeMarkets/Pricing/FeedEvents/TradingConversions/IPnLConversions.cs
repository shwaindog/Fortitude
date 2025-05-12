using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

public interface IPnLConversions : IReusableObject<IPnLConversions>, IInterfacesComparable<IPnLConversions>
{
}

public interface IMutablePnLConversions : IPnLConversions
{
}