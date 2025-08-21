#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.ORX.Publication;

public class AdapterOrderSessionTracker
{
    private static readonly Func<uint, uint, bool> OrderKeyComparison = (s1, s2) => s1 ==s2;

    private readonly IMap<IConversation, IMap<uint, OrxOrder>> orderFromSessionCache =
        new GarbageAndLockFreeMap<IConversation, IMap<uint, OrxOrder>>(ReferenceEquals);

    private readonly IRecycler recycler;

    private readonly IMap<uint, IConversationRequester> sessionFromOrderIdCache =
        new GarbageAndLockFreeMap<uint, IConversationRequester>(OrderKeyComparison);

    private readonly GarbageAndLockFreePooledFactory<IMap<uint, OrxOrder>> surplusOrderMaps =
        new(() =>
            new GarbageAndLockFreeMap<uint, OrxOrder>(OrderKeyComparison));

    public AdapterOrderSessionTracker(IRecycler recycler) => this.recycler = recycler;

    public void RegisterOrderIdWithSession(OrxOrder order, IConversation repositorySession)
    {
        uint orderKey = order.OrderId.AdapterOrderId!.Value;
        lock (orderFromSessionCache)
        {
            if (!orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
            {
                sessionOrders = surplusOrderMaps.Borrow();
                sessionOrders.Clear();
                orderFromSessionCache.TryAdd(repositorySession, sessionOrders);
            }

            sessionOrders?.TryAdd(orderKey, order);
            order.IncrementRefCount();
        }

        lock (sessionFromOrderIdCache)
        {
            sessionFromOrderIdCache.TryAdd(orderKey, (IConversationRequester)repositorySession);
        }
    }

    public void UnregisterOrderWithSession(ITransmittableOrder order)
    {
        var orderKeyToRemove = order.OrderId.AdapterOrderId!.Value;
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
                sessionFromOrderIdCache.Remove(orders.OrderId.AdapterOrderId!.Value);
        }

        lock (orderFromSessionCache)
        {
            orderFromSessionCache.Remove(repositorySession);
        }
    }

    public IEnumerable<ITransmittableOrder> ReturnAllOrdersForSession(IConversation repositorySession)
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

    public ITransmittableOrder? FindOrderFromSessionId(uint adapterId, IConversation repositorySession)
    {
        OrxOrder? order = null;
        lock (orderFromSessionCache)
        {
            if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                sessionOrders?.TryGetValue(adapterId, out order);
        }

        return order;
    }

    public IConversationRequester? FindSessionFromOrderId(IOrderId orderId)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        sessionFromOrderIdCache.TryGetValue(orderId.AdapterOrderId!.Value, out var session);
        return session;
    }
}
