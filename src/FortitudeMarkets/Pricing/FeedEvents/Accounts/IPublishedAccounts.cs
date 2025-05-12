using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Accounts;

public interface IPublishedAccounts : IReusableObject<IPublishedAccounts>, IInterfacesComparable<IPublishedAccounts>
{
}

public interface IMutablePublishedAccounts : IPublishedAccounts
{
}