using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Accounts;

public interface IAccountPosition : IReusableObject<IAccountPosition>, IInterfacesComparable<IAccountPosition>
{
}

public interface IMutableAccountPosition : IAccountPosition , ITrackableReset<IMutableAccountPosition> 
{
}
