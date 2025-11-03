using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Accounts;

public interface IPublishedAccounts : IReusableObject<IPublishedAccounts>, IInterfacesComparable<IPublishedAccounts>
{
}

public interface IMutablePublishedAccounts : IPublishedAccounts, ITrackableReset<IMutablePublishedAccounts>, IEmptyable 
{
}