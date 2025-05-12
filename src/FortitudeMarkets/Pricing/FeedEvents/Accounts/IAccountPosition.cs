using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Accounts;

public interface IAccountPosition : IReusableObject<IAccountPosition>, IInterfacesComparable<IAccountPosition> 
{
}

public interface IMutableAccountPosition : IAccountPosition 
{
}
