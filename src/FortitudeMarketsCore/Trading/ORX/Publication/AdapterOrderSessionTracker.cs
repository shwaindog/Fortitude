#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public class AdapterOrderSessionTracker
{
    private static readonly Func<IMutableString, IMutableString, bool> OrderKeyComparison = (s1, s2) => Equals(s1, s2);

    private readonly IMap<IConversation, IMap<IMutableString, OrxOrder>> orderFromSessionCache =
        new GarbageAndLockFreeMap<IConversation, IMap<IMutableString, OrxOrder>>(ReferenceEquals);

    private readonly IRecycler recycler;

    private readonly IMap<IMutableString, IConversation> sessionFromOrderIdCache =
        new GarbageAndLockFreeMap<IMutableString, IConversation>(OrderKeyComparison);

    private readonly GarbageAndLockFreePooledFactory<IMap<IMutableString, OrxOrder>> surplusOrderMaps =
        new(() =>
            new GarbageAndLockFreeMap<IMutableString, OrxOrder>(OrderKeyComparison));

    public AdapterOrderSessionTracker(IRecycler recycler) => this.recycler = recycler;

    public void RegisterOrderIdWithSession(OrxOrder order, IConversation repositorySession)
    {
        var orderKey = order.OrderId.VenueAdapterOrderId!.Clone();
        lock (orderFromSessionCache)
        {
            if (!orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
            {
                sessionOrders = surplusOrderMaps.Borrow();
                sessionOrders.Clear();
                orderFromSessionCache.Add(repositorySession, sessionOrders);
            }

            sessionOrders?.Add(orderKey, order);
            order.IncrementRefCount();
        }

        lock (sessionFromOrderIdCache)
        {
            sessionFromOrderIdCache.Add(orderKey, repositorySession);
        }
    }

    public void UnregisterOrderWithSession(IOrder order)
    {
        var orderKeyToRemove = (MutableString)order.OrderId.VenueAdapterOrderId!;
        lock (orderFromSessionCache)
        {
            if (orderFromSessionCache.TryGetValue(order.OrderPublisher!.UnderlyingSession!, out var sessionOrders))
                if (sessionOrders?.TryGetValue(orderKeyToRemove, out var removeThis) == true)
                {
                    sessionOrders.Remove(orderKeyToRemove);
                    removeThis!.DecrementRefCount();
                }
        }

        lock (sessionFromOrderIdCache)
        {
            sessionFromOrderIdCache.Remove(orderKeyToRemove);
        }
    }

    public void UnregisterSession(IConversation repositorySession)
    {
        lock (sessionFromOrderIdCache)
        {
            foreach (var orders in ReturnAllOrdersForSession(repositorySession))
                sessionFromOrderIdCache.Remove((MutableString)orders.OrderId.VenueAdapterOrderId!);
        }

        lock (orderFromSessionCache)
        {
            orderFromSessionCache.Remove(repositorySession);
        }
    }

    public IEnumerable<IOrder> ReturnAllOrdersForSession(IConversation repositorySession)
    {
        lock (repositorySession)
        {
            if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                if (sessionOrders != null)
                    foreach (var orderKvp in sessionOrders)
                        yield return orderKvp.Value;
        }
    }

    public void ClearAll()
    {
        lock (orderFromSessionCache)
        {
            foreach (var sessionMaps in orderFromSessionCache)
            {
                var ordersMap = sessionMaps.Value;
                foreach (var orderEntry in ordersMap)
                {
                    ordersMap.Remove(orderEntry.Key);
                    orderEntry.Key.DecrementRefCount();
                    orderEntry.Value.DecrementRefCount();
                }

                if (orderFromSessionCache.TryGetValue(sessionMaps.Key, out var oldSession))
                {
                    orderFromSessionCache.Remove(sessionMaps.Key);
                    surplusOrderMaps.ReturnBorrowed(oldSession!);
                }
            }

            orderFromSessionCache.Clear();
        }

        lock (sessionFromOrderIdCache)
        {
            foreach (var ordersSession in sessionFromOrderIdCache) sessionFromOrderIdCache.Remove(ordersSession.Key);
            sessionFromOrderIdCache.Clear();
        }
    }

    public IEnumerable<IConversation> AllRegisteredSessions()
    {
        lock (orderFromSessionCache)
        {
            foreach (var session in orderFromSessionCache) yield return session.Key;
        }
    }

    public IOrder? FindOrderFromSessionId(IMutableString adapterId, IConversation repositorySession)
    {
        OrxOrder? order = null;
        lock (orderFromSessionCache)
        {
            if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                sessionOrders?.TryGetValue(adapterId, out order);
        }

        return order;
    }

    public IConversation? FindSessionFromOrderId(IOrderId orderId)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        sessionFromOrderIdCache.TryGetValue(orderId.VenueAdapterOrderId!, out var session);
        return session;
    }
}
