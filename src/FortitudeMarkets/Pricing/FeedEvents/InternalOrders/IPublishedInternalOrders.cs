using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IPublishedInternalOrders: IReusableObject<IPublishedInternalOrders>, IInterfacesComparable<IPublishedInternalOrders>
{
}
public interface IMutablePublishedInternalOrders : IPublishedInternalOrders
{
}