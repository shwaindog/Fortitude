using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IPublishedInternalOrders: IReusableObject<IPublishedInternalOrders>, IInterfacesComparable<IPublishedInternalOrders>
{
}
public interface IMutablePublishedInternalOrders : IPublishedInternalOrders, ITrackableReset<IMutablePublishedInternalOrders>, IEmptyable
{
}