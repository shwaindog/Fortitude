#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Sockets;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public class AdapterOrderSessionTracker
{
    private static readonly Func<MutableString, MutableString, bool> OrderKeyComparison = (s1, s2) => s1 == s2;

    private readonly GarbageAndLockFreePooledFactory<MutableString> mutableStringPool
        = new(() => new MutableString());

    private readonly IMap<ISession, IMap<MutableString, OrxOrder>> orderFromSessionCache =
        new GarbageAndLockFreeMap<ISession, IMap<MutableString, OrxOrder>>(ReferenceEquals);

    private readonly IRecycler orxRecyclingFactory;

    private readonly IMap<MutableString, ISession> sessionFromOrderIdCache =
        new GarbageAndLockFreeMap<MutableString, ISession>(OrderKeyComparison);

    private readonly GarbageAndLockFreePooledFactory<IMap<MutableString, OrxOrder>> surplusOrderMaps =
        new(() =>
            new GarbageAndLockFreeMap<MutableString, OrxOrder>(OrderKeyComparison));

    public AdapterOrderSessionTracker(IRecycler orxRecyclingFactory) => this.orxRecyclingFactory = orxRecyclingFactory;

    public void RegisterOrderIdWithSession(OrxOrder order, ISession repositorySession)
    {
        lock (orderFromSessionCache)
        {
            if (!orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
            {
                sessionOrders = surplusOrderMaps.Borrow();
                sessionOrders.Clear();
                orderFromSessionCache.Add(repositorySession, sessionOrders);
            }

            sessionOrders?.Add(
                mutableStringPool.Borrow().Clear().Append(order.OrderId.VenueAdapterOrderId!), order);
        }

        lock (sessionFromOrderIdCache)
        {
            sessionFromOrderIdCache.Add(
                mutableStringPool.Borrow().Clear().Append(order.OrderId.VenueAdapterOrderId!), repositorySession);
        }
    }

    public void UnregisterOrderWithSession(IOrder order)
    {
        lock (orderFromSessionCache)
        {
            if (orderFromSessionCache.TryGetValue(order.OrderPublisher!.UnderlyingSession!, out var sessionOrders))
                sessionOrders?.Remove((MutableString)order.OrderId.VenueAdapterOrderId!);
        }

        lock (sessionFromOrderIdCache)
        {
            sessionFromOrderIdCache.Remove((MutableString)order.OrderId.VenueAdapterOrderId!);
        }
    }

    public void UnregisterSession(ISession repositorySession)
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

    public IEnumerable<IOrder> ReturnAllOrdersForSession(ISession repositorySession)
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
                    var removedEntry = ordersMap.Remove(orderEntry.Key);
                }

                var removedSessionMap = orderFromSessionCache.Remove(sessionMaps.Key);
            }

            orderFromSessionCache.Clear();
        }

        lock (sessionFromOrderIdCache)
        {
            foreach (var ordersSession in sessionFromOrderIdCache) sessionFromOrderIdCache.Remove(ordersSession.Key);
            sessionFromOrderIdCache.Clear();
        }
    }

    public IEnumerable<ISession> AllRegisteredSessions()
    {
        lock (orderFromSessionCache)
        {
            foreach (var session in orderFromSessionCache) yield return session.Key;
        }
    }

    public IOrder? FindOrderFromSessionId(IMutableString adapterId, ISession repositorySession)
    {
        OrxOrder? order = null;
        lock (orderFromSessionCache)
        {
            if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                sessionOrders?.TryGetValue(adapterId.ToString()!, out order);
        }

        return order;
    }

    public ISession? FindSessionFromOrderId(IOrderId orderId)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        sessionFromOrderIdCache.TryGetValue(orderId.VenueAdapterOrderId!.ToString()!, out var session);
        return session;
    }
}
